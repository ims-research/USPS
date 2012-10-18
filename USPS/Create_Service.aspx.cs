using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.UI.WebControls;
using USPS.Code;
namespace USPS
{
    public partial class CreateService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlstService.Items.Clear();
                foreach (KeyValuePair<string, Service> kvp in ServiceManager.ServiceList)
                {
                    ListItem service = new ListItem
                                           {
                                               Text = kvp.Value.ServiceInformation["Name"],
                                               Value = kvp.Value.ServiceConfig["GUID"]
                                           };
                    ddlstService.Items.Add(service);
                }

                ddlstCondition.Items.Clear();
                foreach (KeyValuePair<string, Condition> kvp in ServiceManager.ConditionList)
                {
                    ListItem condition = new ListItem {Text = kvp.Value.Name, Value = kvp.Value.Name};
                    ddlstCondition.Items.Add(condition);
                }
                Session["service"] = new ServiceFlow();
                Session["nodes"] = new Dictionary<string, object>();
            }
            else
            {
            }
        }

        protected void ddlstServiceSelectedIndexChanged(object sender, EventArgs e)
        {
            lstbxAvalRes.Items.Clear();
            lstbxSelRes.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in ServiceManager.ServiceList[ddlstService.SelectedValue].SIPResponses)
            {
                ListItem service = new ListItem {Value = kvp.Value, Text = kvp.Key};
                service.Attributes.Add("Title", kvp.Value);
                lstbxAvalRes.Items.Add(service);
            }

        }

        protected void rdblstChoice_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (rdblstChoice.SelectedIndex == 0)
            {
                ServiceTable.Visible = true;
                ConditionTable.Visible = false;
                ConditionPanel.Visible = false;
                pnlInstruction.Visible = false;

            }
            else if (rdblstChoice.SelectedIndex == 1)
            {
                ConditionTable.Visible = true;
                ServiceTable.Visible = false;
                ConditionPanel.Visible = false;
                pnlInstruction.Visible = false;
            }
            else if (rdblstChoice.SelectedIndex == 2)
            {
                string guid = Guid.NewGuid().ToString();
                TypeNode tn = new TypeNode(ServiceBlock.Types.Terminating)
                                  {
                                      Text = "Service Terminated",
                                      NavigateUrl = "",
                                      SelectAction = TreeNodeSelectAction.Select,
                                      Value = guid,
                                      InstanceGUID = Guid.NewGuid().ToString()
                                  };
                
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(tn);
                    ((Dictionary<string, object>)Session["nodes"]).Add(guid,tn);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(tn);
                    ((Dictionary<string, object>)Session["nodes"]).Add(guid,tn);
                }
                tvServiceFlow.ExpandAll();
            }
            else if (rdblstChoice.SelectedIndex == 3)
            {
                if (tvServiceFlow.SelectedNode != null)
                {
                    if (tvServiceFlow.SelectedNode is TypeNode)
                    {
                        TypeNode temp_node = (TypeNode)tvServiceFlow.SelectedNode;
                    }
                }

            }
            rdblstChoice.ClearSelection();
        }

        protected void ddlstConditionSelectedIndexChanged(object sender, EventArgs e)
        {
            lstbxAvaValues.Items.Clear();
            lstbxSelValues.Items.Clear();
            foreach (string possibleValue in ServiceManager.ConditionList[ddlstCondition.SelectedValue].PossibleValues)
            {
                ListItem service = new ListItem {Value = possibleValue, Text = possibleValue};
                service.Attributes.Add("Title", ServiceManager.ConditionList[ddlstCondition.SelectedValue].Description);
                lstbxAvaValues.Items.Add(service);
            }

        }

        protected void btnAddResponses_OnClick(object sender, EventArgs e)
        {
            List<ListItem> itemList = new List<ListItem>();

            if (lstbxAvalRes.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxAvalRes.Items.Count; i++)
                {
                    if (lstbxAvalRes.Items[i].Selected)
                    {
                        if (!itemList.Contains(lstbxAvalRes.Items[i]))
                            itemList.Add(lstbxAvalRes.Items[i]);
                    }
                }
                foreach (ListItem t in itemList)
                {
                    if (!lstbxSelRes.Items.Contains(t))
                        lstbxSelRes.Items.Add(t);
                    lstbxAvalRes.Items.Remove(t);
                }
            }
            lstbxAvalRes.ClearSelection();
            lstbxSelRes.ClearSelection();
        }

        protected void btnDelResponses_OnClick(object sender, EventArgs e)
        {
            List<ListItem> itemList = new List<ListItem>();

            if (lstbxSelRes.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxSelRes.Items.Count; i++)
                {
                    if (lstbxSelRes.Items[i].Selected)
                    {
                        if (!itemList.Contains(lstbxSelRes.Items[i]))
                            itemList.Add(lstbxSelRes.Items[i]);
                    }
                }
                foreach (ListItem t in itemList)
                {
                    if (!lstbxAvalRes.Items.Contains(t))
                        lstbxAvalRes.Items.Add(t);
                    lstbxSelRes.Items.Remove(t);
                }
            }
            lstbxAvalRes.ClearSelection();
            lstbxSelRes.ClearSelection();
        }

        protected void btnAddValue_OnClick(object sender, EventArgs e)
        {
            List<ListItem> itemList = new List<ListItem>();

            if (lstbxAvaValues.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxAvaValues.Items.Count; i++)
                {
                    if (lstbxAvaValues.Items[i].Selected)
                    {
                        if (!itemList.Contains(lstbxAvaValues.Items[i]))
                            itemList.Add(lstbxAvaValues.Items[i]);
                    }
                }
                foreach (ListItem t in itemList)
                {
                    if (!lstbxSelValues.Items.Contains(t))
                        lstbxSelValues.Items.Add(t);
                    lstbxAvaValues.Items.Remove(t);
                }
            }
            lstbxAvaValues.ClearSelection();
            lstbxSelValues.ClearSelection();
        }

        protected void btnDelValue_OnClick(object sender, EventArgs e)
        {
            List<ListItem> itemList = new List<ListItem>();

            if (lstbxSelValues.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxSelValues.Items.Count; i++)
                {
                    if (lstbxSelValues.Items[i].Selected)
                    {
                        if (!itemList.Contains(lstbxSelValues.Items[i]))
                            itemList.Add(lstbxSelValues.Items[i]);
                    }
                }
                foreach (ListItem t in itemList)
                {
                    if (!lstbxAvaValues.Items.Contains(t))
                        lstbxAvaValues.Items.Add(t);
                    lstbxSelValues.Items.Remove(t);
                }
            }
            lstbxAvaValues.ClearSelection();
            lstbxSelValues.ClearSelection();
        }

        protected void btnAddService_OnClick(object sender, EventArgs e)
        {
            ServiceTable.Visible = false;
            lblChooseAnother.Visible = true;
            TypeNode serviceNode = new TypeNode(ServiceBlock.Types.Service);
            Service initialService = ServiceManager.ServiceList[ddlstService.SelectedValue];
            serviceNode.Text = initialService.ServiceInformation["Name"];
            serviceNode.Value = initialService.ServiceConfig["GUID"];
            serviceNode.SelectAction = TreeNodeSelectAction.Select;

            serviceNode.Name = initialService.ServiceInformation["Name"];
            serviceNode.GlobalGUID = initialService.ServiceConfig["GUID"];
            serviceNode.InstanceGUID = Guid.NewGuid().ToString();

            if (lstbxSelRes.Items.Count > 0)
            {
                ValueNode tempNode = new ValueNode(ValueNode.Types.ServiceResponse) { SelectAction = TreeNodeSelectAction.Select, NavigateUrl = "", Value = Guid.NewGuid().ToString() };
                for (int i = 0; i < lstbxSelRes.Items.Count; i++)
                {
                    tempNode.Values.Add(lstbxSelRes.Items[i].Text);
                }
                tempNode.Text = string.Join("<br/>", tempNode.Values);
                ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
                serviceNode.ChildNodes.Add(tempNode);
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(serviceNode);
                    ((Dictionary<string, object>)Session["nodes"]).Add(initialService.ServiceConfig["GUID"], serviceNode);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(serviceNode);
                    ((Dictionary<string, object>)Session["nodes"]).Add(initialService.ServiceConfig["GUID"], serviceNode);
                }
                tvServiceFlow.ExpandAll();
            }
        }

        protected void btnAddCondition_OnClick(object sender, EventArgs e)
        {
            ConditionTable.Visible = false;
            lblChooseAnother.Visible = true;
            TypeNode conditionNode = new TypeNode(ServiceBlock.Types.Condition);
            Condition condition = ServiceManager.ConditionList[ddlstCondition.SelectedValue];
            conditionNode.Text = condition.Name;
            conditionNode.Value = Guid.NewGuid().ToString();

            conditionNode.SelectAction = TreeNodeSelectAction.Select;

            conditionNode.Name = condition.Name;
            conditionNode.InstanceGUID = Guid.NewGuid().ToString();

            if (lstbxSelValues.Items.Count > 0)
            {
                ValueNode tempNode = new ValueNode(ValueNode.Types.ConditionValue) {Value = Guid.NewGuid().ToString()};
                ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
                tempNode.SelectAction = TreeNodeSelectAction.Select;
                tempNode.NavigateUrl = "";
                for (int i = 0; i < lstbxSelValues.Items.Count; i++)
                {
                    tempNode.Values.Add(lstbxSelValues.Items[i].Text);

                }
                tempNode.Value = Guid.NewGuid().ToString();
                tempNode.Text = string.Join("<br/>", tempNode.Values);
                conditionNode.ChildNodes.Add(tempNode);
                ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(conditionNode);
                    ((Dictionary<string, object>)Session["nodes"]).Add(conditionNode.Value, conditionNode);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(conditionNode);
                    ((Dictionary<string, object>)Session["nodes"]).Add(conditionNode.Value, conditionNode);
                }
                tvServiceFlow.ExpandAll();

            }
        }

        protected void TvServiceFlowNodeChange(object sender, EventArgs e)
        {
            ConditionTable.Visible = false;
            ServiceTable.Visible = false;
            lblChooseAnother.Visible = false;
            ConditionPanel.Visible = true;
        }

        protected void BtnSaveFlowClick(object sender, EventArgs e)
        {
            Session["service"] = new ServiceFlow();
            TreeNodeCollection nodes = tvServiceFlow.Nodes;
            foreach (TreeNode n in nodes)
            {
                AddNode(n);
            }
            foreach (KeyValuePair<string, ServiceBlock> kvp in ((ServiceFlow)Session["service"]).Blocks)
            {
                string key = kvp.Key;
                ServiceBlock value = kvp.Value;
            }
            UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
            List<ServiceFlow> sf;
            try
            {
                 sf = profile.ServiceFlows.Deserialize<List<ServiceFlow>>();
            }
            catch (Exception)
            {
                sf = new List<ServiceFlow>();
            }
            sf.Add((ServiceFlow) Session["service"]);
            profile.ServiceFlows = sf.Serialize();
            profile.Save();
            //TODO Add service flow to user profile
        }

        private void AddNode(TreeNode n)
        {
            Dictionary<string, object> nodes = (Dictionary<string, object>) Session["nodes"];
            Object node = nodes[n.Value];
            if (node is TypeNode)
            {
                AddTypeNode((TypeNode)node);
            }
            else if (node is ValueNode)
            {
                AddValueNode((ValueNode)node, n);
            }
            foreach (TreeNode tn in n.ChildNodes)
            {
                AddNode(tn);
            }

        }

        private void AddTypeNode(TypeNode typeNode)
        {
            ServiceBlock sb = new ServiceBlock
                                  {
                                      Name = typeNode.Name,
                                      GlobalGUID = typeNode.GlobalGUID,
                                      InstanceGUID = typeNode.InstanceGUID,
                                      BlockType = typeNode.BlockType
                                  };
            ((ServiceFlow)Session["service"]).Blocks.Add(typeNode.InstanceGUID, sb);
        }

        private void AddValueNode(ValueNode valueNode, TreeNode tn)
        {
            Dictionary<string, object> nodes = (Dictionary<string, object>)Session["nodes"];
            Object parent = nodes[tn.Parent.Value];
            Object child = null;
            if (tn.ChildNodes.Count > 0)
            {
                child = nodes[tn.ChildNodes[0].Value];    
            }
            ServiceBlock sb = ((ServiceFlow)Session["service"]).Blocks[((TypeNode)parent).InstanceGUID];
            if (child !=null)
            {
                sb.NextBlock.Add(valueNode.Values[0].ToString(), ((TypeNode)child).InstanceGUID);    
            }
            ((ServiceFlow)Session["service"]).Blocks[((TypeNode)parent).InstanceGUID] = sb;
        }

        private void AddTypeNodes(TreeNode treeNode)
        {
            if (treeNode is TypeNode)
            {
                TypeNode tempNode = (TypeNode)treeNode;
                ServiceBlock sb = new ServiceBlock
                                      {
                                          Name = tempNode.Name,
                                          GlobalGUID = tempNode.GlobalGUID,
                                          InstanceGUID = tempNode.InstanceGUID,
                                          BlockType = tempNode.BlockType
                                      };
                ((ServiceFlow)Session["service"]).Blocks.Add(tempNode.InstanceGUID, sb);
            }
            foreach (TreeNode tn in treeNode.ChildNodes)
            {
                AddTypeNodes(tn);
            }
        }

        private void AddValueNodes(TreeNode treeNode)
        {
           if (treeNode is ValueNode)
            {
               ValueNode tempNode = (ValueNode)treeNode;
               TypeNode parent = (TypeNode)treeNode.Parent;
               TypeNode child = (TypeNode)treeNode.ChildNodes[0];
               ServiceBlock sb = ((ServiceFlow)Session["service"]).Blocks[parent.InstanceGUID];
               sb.NextBlock.Add(tempNode.Values.ToString(), child.InstanceGUID);
               ((ServiceFlow)Session["service"]).Blocks[parent.InstanceGUID] = sb;
            }
            foreach (TreeNode tn in treeNode.ChildNodes)
            {
                AddValueNodes(tn);
            }
        }
    }
}