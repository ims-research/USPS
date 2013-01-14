using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Timers;
using System.Xml;
using SIPLib;
using SIPLib.SIP;
using SIPLib.Utils;
using LibServiceInfo;
using USPS_Sip;
using log4net;
using USPS;

namespace USPS.Code
{
    class SIPHandler : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SIPApp));
        private static readonly ILog SessionLog = LogManager.GetLogger("SessionLogger");
        private SIPApp _app;
        private static Address _localparty;
        private SQLiteDatabase _db;
        private DateTime _lastupdate;

        public SIPStack CreateStack(SIPApp app, string proxyIp = null, int proxyPort = -1)
        {
            SIPStack myStack = new SIPStack(app, "USPS");
            if (proxyIp != null)
            {
                myStack.ProxyHost = proxyIp;
                myStack.ProxyPort = (proxyPort == -1) ? 5060 : proxyPort;
            }
            return myStack;
        }

        public TransportInfo CreateTransport(string listenIp, int listenPort)
        {
            return new TransportInfo(IPAddress.Parse(listenIp), listenPort, System.Net.Sockets.ProtocolType.Udp);
        }

        void AppResponseRecvEvent(object sender, SipMessageEventArgs e)
        {
            Log.Info("Response Received:" + e.Message);
            Message response = e.Message;
            string requestType = response.First("CSeq").ToString().Trim().Split()[1].ToUpper();
            switch (requestType)
            {
                case "INVITE":
                case "REGISTER":
                case "BYE":
                default:
                    Log.Info("Response for Request Type " + requestType + " is unhandled ");
                    break;
            }
        }

        void AppRequestRecvEvent(object sender, SipMessageEventArgs e)
        {
            Log.Info("Request Received:" + e.Message);
            Message request = e.Message;
            switch (request.Method.ToUpper())
            {
                case "INVITE":
                case "BYE":
                case "ACK":
                case "CANCEL":
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
                case "OPTIONS":
                case "REFER":
                case "SUBSCRIBE":
                case "NOTIFY":
                case "PUBLISH":
                case "INFO":
                default:
                    {
                        Log.Info("Request with method " + request.Method.ToUpper() + " is unhandled");
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
                xmlDoc.LoadXml(messageBody);
                XmlNodeList guids = xmlDoc.GetElementsByTagName("GUID");
                string serviceGUID = guids[0].InnerText;
                xmlDoc.Save(Server.MapPath("Resources\\Services\\") + serviceGUID + ".xml");
            }
            ServiceManager.LoadServices(Server.MapPath("Resources\\Services\\"));
        }

        private void StartTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(SendServicesChains);
            aTimer.Interval = 30000;
            aTimer.Enabled = true;
        }

        private DataTable readServiceFlowDB()
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
                error += fail.Message.ToString() + "\n\n";
                Log.Error(error);
            }
            return null;
        }

        private void SendServicesChains(object sender, ElapsedEventArgs e)
        {
            DataTable serviceFlows = readServiceFlowDB();
            bool update_needed = false;
            if (serviceFlows == null) return;
            Dictionary<string,List<ServiceFlow>> changedFlows = new Dictionary<string, List<ServiceFlow>>();
            foreach (DataRow r in serviceFlows.Rows)
            {
                DateTime t2 = Convert.ToDateTime(r["LastUpdatedDate"].ToString());
                if (_lastupdate >= t2) continue;
                changedFlows[r["email"].ToString()] = r["PropertyValuesString"].ToString().Deserialize<List<ServiceFlow>>();
                update_needed = true;
            }
            if (update_needed)
            {
                UserAgent serviceChainUA = new UserAgent(_app.Stack) { RemoteParty = new Address("<sip:scim@open-ims.test>"), LocalParty = _localparty };
                Message request = serviceChainUA.CreateRequest("MESSAGE");
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
            _app.RequestRecvEvent += new EventHandler<SipMessageEventArgs>(AppRequestRecvEvent);
            _app.ResponseRecvEvent += new EventHandler<SipMessageEventArgs>(AppResponseRecvEvent);
            const string scscfIP = "scscf.open-ims.test";
            const int scscfPort = 6060;
            SIPStack stack = CreateStack(_app, scscfIP, scscfPort);
            stack.Uri = new SIPURI("usps@open-ims.test");
            _localparty = new Address("<sip:usps@open-ims.test>");
            StartTimer();
        }
    }
}
