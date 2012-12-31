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

        public string ListConditions()
        {
            StringBuilder sb = new StringBuilder();
            var jss = new JavaScriptSerializer();
            Dictionary<String, String> conditions = new Dictionary<String, String>();
            foreach (KeyValuePair<string, Condition> kvp in ServiceManager.ConditionList)
            {
                conditions.Add(kvp.Value.Name, kvp.Value.GUID);
            }
            return jss.Serialize(conditions);
        }

        [WebMethod]
        public string ListServiceResponses(String ServiceGUID)
        {
            Service service = ServiceManager.ServiceList[ServiceGUID];
            var jss = new JavaScriptSerializer();
            return jss.Serialize(service.SIPResponses);
        }

        [WebMethod]
        public string ListConditionOptions(String ConditionGUID)
        {
            Condition condition = ServiceManager.ConditionList[ConditionGUID];
            var jss = new JavaScriptSerializer();
            return jss.Serialize(condition.PossibleValues);
        }

        [WebMethod(EnableSession = true)]
        public string SaveChain(Object GUID, String Chain)
        {
            var jss = new JavaScriptSerializer();
            D3Node root_node = jss.Deserialize<D3Node>(Chain);
            string new_service_guid = Guid.NewGuid().ToString();
            if (Session != null)
            {
                Session["service"] = new ServiceFlow();
                List<ServiceFlow> sf = new List<ServiceFlow>();
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                sf.Add((ServiceFlow)Session["service"]);
                profile.ServiceFlows = sf.Serialize();
                profile.Save();
                return jss.Serialize("Chain Saved Successfully");
            }
            else
            {
                return jss.Serialize("Chain Not Saved - Please make sure you are logged in");
            }
        }

    }
}
