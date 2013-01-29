#region

using System;
using System.Threading;
using System.Web;
using Juice.Framework;
using LibServiceInfo;
using USPS.Code;

#endregion

namespace USPS
{
    public class Global : HttpApplication
    {
        private Thread _sipThread;

        private void Application_Start(object sender, EventArgs e)
        {
            CssManager.CssResourceMapping.AddDefinition("juice-ui", new CssResourceDefinition
                {
                    Path = "~/Content/themes/Supercharged/jquery-ui-1.9.0.custom.css",
                });
            ServiceManager.LoadServices(Server.MapPath("Resources\\Services\\"));
            ServiceManager.LoadConditions(Server.MapPath("Resources\\Conditions\\Conditions.xml"));
            SIPHandler sh = new SIPHandler(Server.MapPath("Resources\\"));
            _sipThread = new Thread(sh.Start);
            _sipThread.Start();
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            _sipThread.Abort();
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            //  Code that runs after the page code is executed
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        private void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        private void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}