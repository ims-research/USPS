using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text;
using USPS.Code;

namespace USPS
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class WebServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string ListServices()
        {
            StringBuilder sb = new StringBuilder();
            var jss = new JavaScriptSerializer();
            Dictionary<String, String> services = new Dictionary<String,String>();
            foreach (KeyValuePair<string, Service> kvp in ServiceManager.ServiceList)
            {
                services.Add(kvp.Value.ServiceInformation["Name"],kvp.Value.ServiceConfig["GUID"]);
            }
            return jss.Serialize(services);
        }

        [WebMethod]
        public string ListServiceResponses(String ServiceGUID)
        {
            Service service = ServiceManager.ServiceList[ServiceGUID];
            var jss = new JavaScriptSerializer();
            return jss.Serialize(service.SIPResponses);
        }

    }
}
