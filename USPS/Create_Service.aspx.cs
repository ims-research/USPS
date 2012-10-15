using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using USPS.Code;
namespace USPS
{
    public partial class CreateService : System.Web.UI.Page
    {
        ServiceFlow _sf;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
            }
            else
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

                _sf = new ServiceFlow();
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
                TypeNode typenode = new TypeNode(ServiceBlock.Types.Terminating);
                TagTreeNode tn = new TagTreeNode();
                tn.Tag = typenode;
                tn.Text = "Service Terminated";
                tn.NavigateUrl = "";
                tn.SelectAction = TreeNodeSelectAction.Select;
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(tn);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(tn);
                }
                tvServiceFlow.ExpandAll();
            }
            else if (rdblstChoice.SelectedIndex == 3)
            {
                if (tvServiceFlow.SelectedNode != null)
                {
                    if (((TagTreeNode)tvServiceFlow.SelectedNode).Tag is TypeNode)
                    {
                        TypeNode temp_node = (TypeNode)((TagTreeNode)tvServiceFlow.SelectedNode).Tag;

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
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (!lstbxSelRes.Items.Contains((ListItem)itemList[i]))
                        lstbxSelRes.Items.Add((ListItem)itemList[i]);
                    lstbxAvalRes.Items.Remove((ListItem)itemList[i]);
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
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (!lstbxAvalRes.Items.Contains((ListItem)itemList[i]))
                        lstbxAvalRes.Items.Add((ListItem)itemList[i]);
                    lstbxSelRes.Items.Remove((ListItem)itemList[i]);
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
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (!lstbxSelValues.Items.Contains((ListItem)itemList[i]))
                        lstbxSelValues.Items.Add((ListItem)itemList[i]);
                    lstbxAvaValues.Items.Remove((ListItem)itemList[i]);
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
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (!lstbxAvaValues.Items.Contains((ListItem)itemList[i]))
                        lstbxAvaValues.Items.Add((ListItem)itemList[i]);
                    lstbxSelValues.Items.Remove((ListItem)itemList[i]);
                }
            }
            lstbxAvaValues.ClearSelection();
            lstbxSelValues.ClearSelection();
        }

        protected void btnAddService_OnClick(object sender, EventArgs e)
        {
            ServiceTable.Visible = false;
            lblChooseAnother.Visible = true;
            TagTreeNode serviceNode = new TagTreeNode();
            TypeNode serviceNode2 = new TypeNode(ServiceBlock.Types.Service);
            Service initialService = ServiceManager.ServiceList[ddlstService.SelectedValue];
            
            serviceNode.Text = initialService.ServiceInformation["Name"];
            serviceNode.Value = initialService.ServiceConfig["GUID"];
            serviceNode.SelectAction = TreeNodeSelectAction.Select;

            serviceNode2.Name = initialService.ServiceInformation["Name"];
            serviceNode2.GlobalGUID = initialService.ServiceConfig["GUID"];
            serviceNode2.InstanceGUID = Guid.NewGuid().ToString();
            serviceNode.Tag = serviceNode2;

            if (lstbxSelRes.Items.Count > 0)
            {
                TagTreeNode tempNode = new TagTreeNode();
                ValueNode tempNode2 = new ValueNode(ValueNode.Types.ServiceResponse);

                tempNode.SelectAction = TreeNodeSelectAction.Select;
                tempNode.NavigateUrl = "";
                for (int i = 0; i < lstbxSelRes.Items.Count; i++)
                {
                    tempNode2.Values.Add(lstbxSelRes.Items[i].Text);
                }
                tempNode.Text = string.Join("<br/>", tempNode2.Values);
                tempNode.Tag = tempNode2;
                serviceNode.ChildNodes.Add(tempNode);

                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(serviceNode);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(serviceNode);
                }
                tvServiceFlow.ExpandAll();
            }
        }

        protected void btnAddCondition_OnClick(object sender, EventArgs e)
        {
            ConditionTable.Visible = false;
            lblChooseAnother.Visible = true;
            TagTreeNode conditionNode = new TagTreeNode();
            TypeNode conditionNode2 = new TypeNode(ServiceBlock.Types.Condition);
            Condition condition = ServiceManager.ConditionList[ddlstCondition.SelectedValue];
            conditionNode.Text = condition.Name;
            conditionNode.Value = condition.Name;

            conditionNode.SelectAction = TreeNodeSelectAction.Select;

            conditionNode2.Name = condition.Name;
            conditionNode2.InstanceGUID = Guid.NewGuid().ToString();

            conditionNode.Tag = conditionNode2;

            if (lstbxSelValues.Items.Count > 0)
            {
                TagTreeNode tempNode = new TagTreeNode();
                ValueNode tempNode2 = new ValueNode(ValueNode.Types.ConditionValue);
                tempNode.SelectAction = TreeNodeSelectAction.Select;
                tempNode.NavigateUrl = "";
                for (int i = 0; i < lstbxSelValues.Items.Count; i++)
                {
                    tempNode2.Values.Add(lstbxSelValues.Items[i].Text);

                }
                tempNode.Text = string.Join("<br/>", tempNode2.Values);
                tempNode.Tag = tempNode2;
                conditionNode.ChildNodes.Add(tempNode);
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(conditionNode);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(conditionNode);
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
            _sf = new ServiceFlow();
            TreeNodeCollection nodes = tvServiceFlow.Nodes;
            foreach (object n in nodes)
            {
                AddNode(n);
            }
            foreach (KeyValuePair<string,ServiceBlock> kvp  in _sf.Blocks)
            {
                string key = kvp.Key;
                ServiceBlock value = kvp.Value;
            }

            //TODO Add service flow to user profile
        }

        private void AddNode(object n)
        {
            TagTreeNode t = (TagTreeNode) n;
            if (t.Tag is TypeNode)
            {
                AddTypeNode((TypeNode)t.Tag);
            }
            else if (t.Tag is ValueNode)
            {
                AddValueNode((ValueNode)t.Tag,t);
            }
            foreach (TagTreeNode tn in ((TagTreeNode)(n)).ChildNodes)
            {
                AddNode(tn);
            }
            
        }

        private void AddTypeNode(TypeNode typeNode)
        {
                ServiceBlock sb = new ServiceBlock();
                sb.Name = typeNode.Name;
                sb.GlobalGUID = typeNode.GlobalGUID;
                sb.InstanceGUID = typeNode.InstanceGUID;
                sb.block_type = typeNode.BlockType;
                _sf.Blocks.Add(typeNode.InstanceGUID, sb);
        }

        private void AddValueNode(ValueNode valueNode,TagTreeNode tn)
        {
            TagTreeNode parent = (TagTreeNode) tn.Parent;
            TagTreeNode child = (TagTreeNode) tn.ChildNodes[0];
               ServiceBlock sb = _sf.Blocks[((TypeNode)parent.Tag).InstanceGUID];
               sb.nextBlock.Add(valueNode.Values.ToString(), ((TypeNode)child.Tag).InstanceGUID);
               _sf.Blocks[((TypeNode)parent.Tag).InstanceGUID] = sb;
        }
    }
}