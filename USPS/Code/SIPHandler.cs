#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Web;
using System.Xml;
using LibServiceInfo;
using SIPLib.SIP;
using SIPLib.Utils;
using log4net;
using Timer = System.Timers.Timer;

#endregion

namespace USPS.Code
{
    internal class SIPHandler : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (SIPApp));
        private static Address _localparty;
        private SIPApp _app;
        private SQLiteDatabase _db;
        private DateTime _lastupdate;

        public SIPHandler(string resourcePath)
        {
            ResourcePath = resourcePath;
        }

        private string ResourcePath { get; set; }

        private SIPStack CreateStack(SIPApp app, string proxyIp = null, int proxyPort = -1)
        {
            SIPStack myStack = new SIPStack(app, "USPS");
            if (proxyIp != null)
            {
                myStack.ProxyHost = proxyIp;
                myStack.ProxyPort = (proxyPort == -1) ? 5060 : proxyPort;
            }
            return myStack;
        }

        private TransportInfo CreateTransport(string listenIp, int listenPort)
        {
            return new TransportInfo(IPAddress.Parse(listenIp), listenPort, ProtocolType.Udp);
        }

        private void AppResponseRecvEvent(object sender, SipMessageEventArgs e)
        {
            Log.Info("Response Received:" + e.Message);
            Message response = e.Message;
            string requestType = response.First("CSeq").ToString().Trim().Split()[1].ToUpper();
            switch (requestType)
            {
                default:
                    Log.Info("Response for Request Type " + requestType + " is unhandled ");
                    break;
            }
        }

        private void AppRequestRecvEvent(object sender, SipMessageEventArgs e)
        {
            Log.Info("Request Received:" + e.Message);
            Message request = e.Message;
            switch (request.Method.ToUpper())
            {
                case "MESSAGE":
                    {
                        _app.Useragents.Add(e.UA);
                        Message m = e.UA.CreateResponse(200, "OK");
                        e.UA.SendResponse(m);
                        if (request.First("Content-Type").ToString().ToUpper().Equals("APPLICATION/SERVLIST+XML"))
                        {
                            ProcessMessageBody(request.Body);
                        }
                        break;
                    }
                default:
                    {
                        Log.Info("Request with method " + request.Method.ToUpper() + " is unhandled");
                        Message m = e.UA.CreateResponse(501, "Not Implemented");
                        e.UA.SendResponse(m);
                        break;
                    }
            }
        }

        private void ProcessMessageBody(string messageBody)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string[] services = messageBody.Split('\n');
            foreach (string service in services)
            {
                xmlDoc.LoadXml(service);
                XmlNodeList guids = xmlDoc.GetElementsByTagName("GUID");
                string serviceGUID = guids[0].InnerText;
                xmlDoc.Save(ResourcePath + "Services\\" + serviceGUID + ".xml");
            }
            ServiceManager.LoadServices(ResourcePath + "Services\\");
        }

        private void StartTimer()
        {
            Timer aTimer = new Timer();
            aTimer.Elapsed += SendServicesChains;
            aTimer.Interval = 30000;
            aTimer.Enabled = true;
        }

        private DataTable ReadServiceFlowDB()
        {
            try
            {
                String query = "SELECT username,email,lastupdateddate,PropertyValuesString ";
                query += "FROM aspnet_Users ";
                query += "INNER JOIN aspnet_Profile ";
                query += "ON aspnet_Users.userid=aspnet_Profile.userid;";
                return _db.GetDataTable(query);
            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message + "\n\n";
                Log.Error(error);
            }
            return null;
        }

        private void SendServicesChains(object sender, ElapsedEventArgs e)
        {
            DataTable serviceFlows = ReadServiceFlowDB();
            bool updateNeeded = false;
            if (serviceFlows == null) return;
            Dictionary<string, List<ServiceFlow>> changedFlows = new Dictionary<string, List<ServiceFlow>>();
            foreach (DataRow r in serviceFlows.Rows)
            {
                DateTime t2 = Convert.ToDateTime(r["LastUpdatedDate"].ToString());
                if (_lastupdate >= t2) continue;
                changedFlows[r["email"].ToString()] =
                    r["PropertyValuesString"].ToString().Deserialize<List<ServiceFlow>>();
                updateNeeded = true;
            }
            if (updateNeeded)
            {
                UserAgent serviceChainUA = new UserAgent(_app.Stack)
                    {
                        RemoteParty = new Address("<sip:scim@open-ims.test>"),
                        LocalParty = _localparty
                    };
                _app.Useragents.Add(serviceChainUA);
                Message m = serviceChainUA.CreateRequest("MESSAGE", changedFlows.Serialize());
                m.InsertHeader(new Header("APPLICATION/SERV_DESC+XML", "Content-Type"));
                serviceChainUA.SendRequest(m);
                //request.InsertHeader(new Header("service.description", "Event"));
            }
            _lastupdate = DateTime.Now;
        }

        public void Start()
        {
            _lastupdate = DateTime.Now;
            _db = new SQLiteDatabase("|DataDirectory|app_data.sqlite;Version=3;");
            TransportInfo localTransport = CreateTransport(Helpers.GetLocalIP(), 7202);
            _app = new SIPApp(localTransport);
            _app.RequestRecvEvent += AppRequestRecvEvent;
            _app.ResponseRecvEvent += AppResponseRecvEvent;
            const string scscfIP = "scscf.open-ims.test";
            const int scscfPort = 6060;
            SIPStack stack = CreateStack(_app, scscfIP, scscfPort);
            stack.Uri = new SIPURI("usps@open-ims.test");
            _localparty = new Address("<sip:usps@open-ims.test>");
            StartTimer();
        }
    }
}