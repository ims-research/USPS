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
            Dictionary<String, String> services = new Dictionary<String, String>();
            foreach (KeyValuePair<string, Service> kvp in ServiceManager.ServiceList)
            {
                services.Add(kvp.Value.ServiceInformation["Name"], kvp.Value.ServiceConfig["GUID"]);
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
            D3Node rootD3Node = jss.Deserialize<D3Node>(Chain);
            string newServiceGuid = Guid.NewGuid().ToString();
            Node rootNode = new Node(rootD3Node);
            if (Session != null)
            {
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                List<ServiceFlow> sfs;
                try
                {
                    sfs = profile.ServiceFlows.Deserialize<List<ServiceFlow>>();
                    // Use below line to reset user's list of services;
                    //sfs = new List<ServiceFlow>();
                }
                catch (Exception)
                {
                    sfs = new List<ServiceFlow>();
                }
                ServiceFlow serviceflow = new ServiceFlow(rootNode);
                return jss.Serialize(SaveServiceFlow(sfs, serviceflow));
            }
            else
            {
                return jss.Serialize("Chain Not Saved - Please make sure you are logged in");
            }
        }

        private string SaveServiceFlow(List<ServiceFlow> serviceFlows, ServiceFlow serviceflow)
        {
            string errorMessage;
            if (DetectConflict(serviceFlows, serviceflow, out errorMessage))
                return "Conflict Detected - " + errorMessage;
            try
            {
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                serviceFlows.Add(serviceflow);
                profile.ServiceFlows = serviceFlows.Serialize();
                profile.Save();
                return "Chain Saved Successfully";
            }
            catch (Exception)
            {
                return "Problem saving chain";
            }
        }

        private bool DetectConflict(List<ServiceFlow> serviceFlows, ServiceFlow serviceFlow, out string errorMessage)
        {
            errorMessage = "Success";
            ServiceBlock startblock = serviceFlow.Blocks[serviceFlow.FirstBlockGUID];
            if (startblock.BlockType == ServiceBlock.BlockTypes.Service)
            {
                if (DetectExistingServiceStartPoint(serviceFlows))
                {
                    errorMessage = "Cannot have more than one chain with a service as the starting point";
                    return true;
                }
            }
            else if (startblock.BlockType == ServiceBlock.BlockTypes.Condition)
            {
                if (DetectExistingConditionStartPoint(serviceFlows,startblock))
                {
                    errorMessage = "Cannot have more than one chain with the same condition as a starting point";
                    return true;
                }
            }
            return false;
        }

        private bool DetectExistingConditionStartPoint(List<ServiceFlow> serviceFlows, ServiceBlock newStartblock)
        {
            bool conflictFound = false;
            foreach (ServiceFlow currentserviceFlow in serviceFlows)
            {
                ServiceBlock startblock = currentserviceFlow.Blocks[currentserviceFlow.FirstBlockGUID];
                if (startblock.BlockType == ServiceBlock.BlockTypes.Condition)
                {
                    if (newStartblock.GlobalGUID == startblock.GlobalGUID)
                    {
                        foreach (String key in newStartblock.NextBlocks.Keys)
                        {
                           if (startblock.NextBlocks.ContainsKey(key))
                           {
                               conflictFound = true;
                           }
                        }
                    }
                }
            }
            return conflictFound;
        }

        private bool DetectExistingServiceStartPoint(List<ServiceFlow> serviceFlows)
        {
            int numberServiceStartBlocks = 0;
            foreach (ServiceFlow currentserviceFlow in serviceFlows)
            {
                ServiceBlock startblock = currentserviceFlow.Blocks[currentserviceFlow.FirstBlockGUID];
                if (startblock.BlockType == ServiceBlock.BlockTypes.Service)
                {
                    numberServiceStartBlocks++;
                }
            }
            if (numberServiceStartBlocks >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
