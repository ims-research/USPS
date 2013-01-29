﻿#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SIPLib.SIP;
using SIPLib.Utils;
using log4net;
using log4net.Config;
using Timer = SIPLib.SIP.Timer;

#endregion

namespace USPS.Code
{
    public class SIPApp : SIPLib.SIPApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (SIPApp));

        public SIPApp(TransportInfo transport)
        {
            XmlConfigurator.Configure();
            TempBuffer = new byte[4096];
            transport.Socket = transport.Type == ProtocolType.Tcp
                                   ? new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                                   : new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localEP = new IPEndPoint(transport.Host, transport.Port);
            transport.Socket.Bind(localEP);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint sendEP = sender;
            transport.Socket.BeginReceiveFrom(TempBuffer, 0, TempBuffer.Length, SocketFlags.None, ref sendEP,
                                              ReceiveDataCB, sendEP);
            Transport = transport;
            Useragents = new List<UserAgent>();
        }

        public override SIPStack Stack { get; set; }
        private byte[] TempBuffer { get; set; }
        public override sealed TransportInfo Transport { get; set; }
        public List<UserAgent> Useragents { get; set; }
        private Address PublicServiceIdentity { get; set; }

        public event EventHandler<RawEventArgs> RawRecvEvent;
        public event EventHandler<RawEventArgs> RawSentEvent;
        public override event EventHandler<RawEventArgs> ReceivedDataEvent;
        public event EventHandler<SipMessageEventArgs> RequestRecvEvent;
        public event EventHandler<SipMessageEventArgs> ResponseRecvEvent;
        public event EventHandler<SipMessageEventArgs> SipSentEvent;
        public event EventHandler<StackErrorEventArgs> ErrorEvent;

        private void ReceiveDataCB(IAsyncResult asyncResult)
        {
            try
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint sendEP = sender;
                int bytesRead = Transport.Socket.EndReceiveFrom(asyncResult, ref sendEP);
                string data = Encoding.ASCII.GetString(TempBuffer, 0, bytesRead);
                string remoteHost = ((IPEndPoint) sendEP).Address.ToString();
                string remotePort = ((IPEndPoint) sendEP).Port.ToString();
                ThreadPool.QueueUserWorkItem(delegate
                    {
                        if (RawRecvEvent != null)
                        {
                            RawRecvEvent(this, new RawEventArgs(data, new[] {remoteHost, remotePort}, false));
                        }
                        if (ReceivedDataEvent != null)
                        {
                            ReceivedDataEvent(this, new RawEventArgs(data, new[] {remoteHost, remotePort}, false));
                        }
                    }, null);
                Transport.Socket.BeginReceiveFrom(TempBuffer, 0, TempBuffer.Length, SocketFlags.None, ref sendEP,
                                                  ReceiveDataCB, sendEP);
            }
            catch (Exception ex)
            {
                if (ErrorEvent != null)
                {
                    ErrorEvent(this, new StackErrorEventArgs("Receive Data Callback", ex));
                }
            }
        }

        public override void Send(string data, string ip, int port, SIPStack stack)
        {
            IPAddress[] addresses = Dns.GetHostAddresses(ip);
            IPEndPoint dest = new IPEndPoint(addresses[0], port);
            EndPoint destEP = dest;
            byte[] sendData = Encoding.ASCII.GetBytes(data);
            string remoteHost = ((IPEndPoint) destEP).Address.ToString();
            string remotePort = ((IPEndPoint) destEP).Port.ToString();

            stack.Transport.Socket.BeginSendTo(sendData, 0, sendData.Length, SocketFlags.None, destEP, SendDataCB,
                                               destEP);

            ThreadPool.QueueUserWorkItem(delegate
                {
                    if (RawSentEvent != null)
                    {
                        RawSentEvent(this, new RawEventArgs(data, new[] {remoteHost, remotePort}, true));
                    }
                    if (SipSentEvent != null)
                    {
                        SipSentEvent(this, new SipMessageEventArgs(new Message(data)));
                    }
                }, null);
        }

        private void SendDataCB(IAsyncResult asyncResult)
        {
            try
            {
                Stack.Transport.Socket.EndSend(asyncResult);
            }
            catch (Exception ex)
            {
                Log.Error("Error in sendDataCB", ex);
                if (ErrorEvent != null)
                {
                    ErrorEvent(this, new StackErrorEventArgs("Send Data Callback", ex));
                }
            }
        }

        public override UserAgent CreateServer(Message request, SIPURI uri, SIPStack stack)
        {
            if (request.Method == "INVITE")
            {
                return new UserAgent(Stack, request);
            }
            return null;
        }

        public override void Sending(UserAgent ua, Message message, SIPStack stack)
        {
            if (Helpers.IsRequest(message))
            {
                Log.Info("Sending request with method " + message.Method);
            }
            else
            {
                Log.Info("Sending response with code " + message.ResponseCode);
            }
            Log.Debug("\n\n" + message);
            //TODO: Allow App to modify message before it gets sent?;
        }

        public override void Cancelled(UserAgent ua, Message request, SIPStack stack)
        {
            throw new NotImplementedException();
        }

        public override string[] Authenticate(UserAgent ua, Header header, SIPStack sipStack)
        {
            throw new NotImplementedException();
        }

        public override void DialogCreated(Dialog dialog, UserAgent ua, SIPStack stack)
        {
            Useragents.Remove(ua);
            Useragents.Add(dialog);
            Log.Info("New dialog created");
        }

        public override Timer CreateTimer(UserAgent app, SIPStack stack)
        {
            return new Timer(app);
        }

        public override void ReceivedResponse(UserAgent ua, Message response, SIPStack stack)
        {
            Log.Info("Received response with code " + response.ResponseCode + " " + response.ResponseText);
            Log.Debug("\n\n" + response);
            if (ResponseRecvEvent != null)
            {
                ResponseRecvEvent(this, new SipMessageEventArgs(response));
            }
        }

        public override void ReceivedRequest(UserAgent ua, Message request, SIPStack stack)
        {
            Log.Info("Received request with method " + request.Method.ToUpper());
            Log.Debug("\n\n" + request);
            if (RequestRecvEvent != null)
            {
                RequestRecvEvent(this, new SipMessageEventArgs(request, ua));
            }
        }
    }
}