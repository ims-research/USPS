#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using LibServiceInfo;
using USPS.Code;

#endregion

namespace USPS
{
    /// <summary>
    ///     Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class WebServices : WebService
    {
        [WebMethod]
        public string ListServices()
        {
            var jss = new JavaScriptSerializer();
            Dictionary<String, String> services = ServiceManager.ServiceList.ToDictionary(kvp => kvp.Value.ServiceInformation["Name"], kvp => kvp.Value.ServiceConfig["GUID"]);
            return jss.Serialize(services);
        }

        [WebMethod]
        public string ListConditions()
        {
            var jss = new JavaScriptSerializer();
            Dictionary<String, String> conditions = ServiceManager.ConditionList.ToDictionary(kvp => kvp.Value.Name, kvp => kvp.Value.GUID);
            return jss.Serialize(conditions);
        }

        [WebMethod]
        public string ListServiceResponses(String serviceGUID)
        {
            Service service = ServiceManager.ServiceList[serviceGUID];
            var jss = new JavaScriptSerializer();
            return jss.Serialize(service.SIPResponses);
        }

        [WebMethod]
        public string ListConditionOptions(String conditionGUID)
        {
            Condition condition = ServiceManager.ConditionList[conditionGUID];
            var jss = new JavaScriptSerializer();
            return jss.Serialize(condition.PossibleValues);
        }

        private bool GetServiceFlows(out List<ServiceFlow> flows, out String errorMessage)
        {
            if (Session != null)
            {
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                try
                {
                    flows = profile.ServiceFlows.Deserialize<List<ServiceFlow>>();
                    errorMessage = "Success";
                    return true;
                }
                catch (Exception)
                {
                    errorMessage = "Could not retrieve list of service flows from profile";
                    flows = new List<ServiceFlow>();
                    profile.ServiceFlows = flows.Serialize();
                    profile.Save();

                    return false;
                }
            }
            errorMessage = "Error - Could not ID session - please login again";
            flows = new List<ServiceFlow>();
            return false;
        }

        [WebMethod(EnableSession = true)]
        public string SaveChain(Object guid, String chain, String name)
        {
            var jss = new JavaScriptSerializer();
            D3Node rootD3Node = jss.Deserialize<D3Node>(chain);
            Node rootNode = new Node(rootD3Node);
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                ServiceFlow serviceflow = new ServiceFlow(rootNode, name);
                return jss.Serialize(SaveServiceFlow(sfs, serviceflow));
            }
            return jss.Serialize(errorMessage);
        }

        private string SaveServiceFlow(List<ServiceFlow> serviceFlows, ServiceFlow serviceflow)
        {
            string errorMessage;
            //TODO: Re-enable conflict detection
            //if (DetectConflict(serviceFlows, serviceflow, out errorMessage))
            //    return "Conflict Detected - " + errorMessage;
            try
            {
                UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                serviceFlows.Add(serviceflow);
                profile.ServiceFlows = serviceFlows.Serialize();
                profile.Save();
                return "Chain Saved Successfully";
            }
            catch (Exception exception)
            {
                return "Problem saving chain - " + exception.Message;
            }
        }

        private bool DetectConflict(IEnumerable<ServiceFlow> serviceFlows, ServiceFlow serviceFlow, out string errorMessage)
        {
            errorMessage = "Success";
            Block startblock = serviceFlow.Blocks[serviceFlow.FirstBlockGUID];
            if (startblock.BlockType == Block.BlockTypes.Service)
            {
                if (DetectExistingServiceStartPoint(serviceFlows))
                {
                    errorMessage = "Cannot have more than one chain with a service as the starting point";
                    return true;
                }
            }
            else if (startblock.BlockType == Block.BlockTypes.Condition)
            {
                if (DetectExistingConditionStartPoint(serviceFlows, startblock))
                {
                    errorMessage = "Cannot have more than one chain with the same condition as a starting point";
                    return true;
                }
            }
            return false;
        }

        private bool DetectExistingConditionStartPoint(IEnumerable<ServiceFlow> serviceFlows, Block newStartblock)
        {
            bool conflictFound = false;
            foreach (ServiceFlow currentserviceFlow in serviceFlows)
            {
                Block startblock = currentserviceFlow.Blocks[currentserviceFlow.FirstBlockGUID];
                if (startblock.BlockType == Block.BlockTypes.Condition)
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

        private bool DetectExistingServiceStartPoint(IEnumerable<ServiceFlow> serviceFlows)
        {
            int numberServiceStartBlocks = 0;
            foreach (ServiceFlow currentserviceFlow in serviceFlows)
            {
                Block startblock = currentserviceFlow.Blocks[currentserviceFlow.FirstBlockGUID];
                if (startblock.BlockType == Block.BlockTypes.Service)
                {
                    numberServiceStartBlocks++;
                }
            }
            if (numberServiceStartBlocks >= 1)
            {
                return true;
            }
            return false;
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
            return jss.Serialize(errorMessage);
        }

        [WebMethod(EnableSession = true)]
        public string GetExistingChain(String firstBlockGUID)
        {
            var jss = new JavaScriptSerializer();
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                foreach (ServiceFlow serviceFlow in sfs)
                {
                    if (serviceFlow.FirstBlockGUID == firstBlockGUID)
                    {
                        return jss.Serialize(serviceFlow.RootNode);
                    }
                }
                return jss.Serialize("Error - Chain not found");
            }
            return jss.Serialize(errorMessage);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteExistingChain(String firstBlockGUID)
        {
            var jss = new JavaScriptSerializer();
            List<ServiceFlow> sfs;
            String errorMessage;
            if (GetServiceFlows(out sfs, out errorMessage))
            {
                foreach (ServiceFlow serviceFlow in sfs)
                {
                    if (serviceFlow.FirstBlockGUID == firstBlockGUID)
                    {
                        try
                        {
                            UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                            List<ServiceFlow> newServiceFlows = new List<ServiceFlow>(sfs);
                            newServiceFlows.Remove(serviceFlow);
                            profile.ServiceFlows = newServiceFlows.Serialize();
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
            return jss.Serialize(errorMessage);
        }
    }
}