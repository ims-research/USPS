using System;
using System.Collections.Generic;
using LibServiceInfo;
using LibServiceInfo;
using System.Web.Security;
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

        private bool GetServiceFlows(out List<ServiceFlow> flows, out String errorMessage)
        {
            if (Session != null)
            {
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                Dictionary<String, List<ServiceFlow>> dict_flows;
                String email = Membership.GetUser(User.Identity.Name).Email;
                try
                {
                    dict_flows = profile.ServiceFlows.Deserialize<Dictionary<String, List<ServiceFlow>>>();
                    flows = dict_flows[email];
                    errorMessage = "Success";
                    return true;
                }
                catch (Exception)
                {
                    errorMessage = "Could not retrieve list of service flows from profile";
                    dict_flows = new Dictionary<String, List<ServiceFlow>>();
                    dict_flows[email] = flows = new List<ServiceFlow>();
                    profile.ServiceFlows = dict_flows.Serialize();
                    profile.Save();
                    
                    return false;
                }
            }
            errorMessage = "Error - Could not ID session - please login again";
            flows = new List<ServiceFlow>();
            return false;
        }

        [WebMethod(EnableSession = true)]
        public string SaveChain(Object GUID, String Chain, String Name)
        {
            var jss = new JavaScriptSerializer();
            D3Node rootD3Node = jss.Deserialize<D3Node>(Chain);
            string newServiceGuid = Guid.NewGuid().ToString();
            Node rootNode = new Node(rootD3Node);
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                ServiceFlow serviceflow = new ServiceFlow(rootNode, Name);
                return jss.Serialize(SaveServiceFlow(sfs, serviceflow));
            }
            else
            {
                return jss.Serialize(errorMessage);
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
                String email = Membership.GetUser(User.Identity.Name).Email;
                serviceFlows.Add(serviceflow);
                Dictionary<String,List<ServiceFlow>> dict_flows = new Dictionary<String,List<ServiceFlow>>();
                dict_flows[email] = serviceFlows;
                profile.ServiceFlows = dict_flows.Serialize();
                profile.Save();
                return "Chain Saved Successfully";
            }
            catch (Exception exception)
            {
                return "Problem saving chain - " + exception.Message;
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
                if (DetectExistingConditionStartPoint(serviceFlows, startblock))
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

        [WebMethod(EnableSession = true)]
        public string ListExistingChains()
        {
            var jss = new JavaScriptSerializer();
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                return jss.Serialize(sfs);
            }
            else
            {
                return jss.Serialize(errorMessage);
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetExistingChain(String FirstBlockGUID)
        {
            var jss = new JavaScriptSerializer();
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                foreach (ServiceFlow serviceFlow in sfs)
                {
                    if (serviceFlow.FirstBlockGUID == FirstBlockGUID)
                    {
                        return jss.Serialize(serviceFlow.RootNode);
                    }
                }
                return jss.Serialize("Error - Chain not found");
            }
            else
            {
                return jss.Serialize(errorMessage);
            }
        }

        [WebMethod(EnableSession = true)]
        public string DeleteExistingChain(String FirstBlockGUID)
        {
            var jss = new JavaScriptSerializer();
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                foreach (ServiceFlow serviceFlow in sfs)
                {
                    if (serviceFlow.FirstBlockGUID == FirstBlockGUID)
                    {
                        try
                        {
                            UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                            List<ServiceFlow> newServiceFlows = new List<ServiceFlow>(sfs);
                            String email = Membership.GetUser(User.Identity.Name).Email;
                            newServiceFlows.Remove(serviceFlow);
                            Dictionary<String, List<ServiceFlow>> dict_flows = new Dictionary<String, List<ServiceFlow>>();
                            dict_flows[email] = newServiceFlows;
                            profile.ServiceFlows = dict_flows.Serialize();
                            profile.Save();
                            return jss.Serialize("Chain Deleted Successfully");
                        }
                        catch (Exception exception)
                        {
                            return jss.Serialize("Problem deleting chain - " + exception.Message);
                        }
                    }
                }
                return jss.Serialize("Error - Chain not found");
            }
            else
            {
                return jss.Serialize(errorMessage);
            }
        }

    }
}
