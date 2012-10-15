using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USPS.Code;
namespace USPS
{
    public partial class Create_Service : System.Web.UI.Page
    {
        ServiceFlow sf;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlstService.Items.Clear();
                foreach (KeyValuePair<string, Service> kvp in Service_Manager.service_list)
                {
                    ListItem service = new ListItem();
                    service.Text = kvp.Value.Service_Information["Name"];
                    service.Value = kvp.Value.Service_Config["GUID"];
                    ddlstService.Items.Add(service);
                }

                ddlstCondition.Items.Clear();
                foreach (KeyValuePair<string, Condition> kvp in Service_Manager.condition_list)
                {
                    ListItem condition = new ListItem();
                    condition.Text = kvp.Value.Name;
                    condition.Value = kvp.Value.Name;
                    ddlstCondition.Items.Add(condition);
                }

                sf = new ServiceFlow();

            }
            else
            {

            }
        }

        protected void ddlstService_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstbxAvalRes.Items.Clear();
            lstbxSelRes.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in Service_Manager.service_list[ddlstService.SelectedValue].SIP_Responses)
            {
                ListItem service = new ListItem();
                service.Value = kvp.Value;
                service.Text = kvp.Key;
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
                TypeNode tn = new TypeNode(ServiceBlock.Types.Terminating);
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

                    if (tvServiceFlow.SelectedNode is TypeNode)
                    {
                        TypeNode temp_node = (TypeNode)tvServiceFlow.SelectedNode;

                    }
                }

            }
            rdblstChoice.ClearSelection();
        }

        protected void ddlstCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstbxAvaValues.Items.Clear();
            lstbxSelValues.Items.Clear();
            foreach (string possible_value in Service_Manager.condition_list[ddlstCondition.SelectedValue].possible_values)
            {
                ListItem service = new ListItem();
                service.Value = possible_value;
                service.Text = possible_value;
                service.Attributes.Add("Title", Service_Manager.condition_list[ddlstCondition.SelectedValue].Description);
                lstbxAvaValues.Items.Add(service);
            }

        }

        protected void btnAddResponses_OnClick(object sender, EventArgs e)
        {
            List<ListItem> item_list = new List<ListItem>();

            if (lstbxAvalRes.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxAvalRes.Items.Count; i++)
                {
                    if (lstbxAvalRes.Items[i].Selected)
                    {
                        if (!item_list.Contains(lstbxAvalRes.Items[i]))
                            item_list.Add(lstbxAvalRes.Items[i]);
                    }
                }
                for (int i = 0; i < item_list.Count; i++)
                {
                    if (!lstbxSelRes.Items.Contains((ListItem)item_list[i]))
                        lstbxSelRes.Items.Add((ListItem)item_list[i]);
                    lstbxAvalRes.Items.Remove((ListItem)item_list[i]);
                }
            }
            lstbxAvalRes.ClearSelection();
            lstbxSelRes.ClearSelection();
        }

        protected void btnDelResponses_OnClick(object sender, EventArgs e)
        {
            List<ListItem> item_list = new List<ListItem>();

            if (lstbxSelRes.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxSelRes.Items.Count; i++)
                {
                    if (lstbxSelRes.Items[i].Selected)
                    {
                        if (!item_list.Contains(lstbxSelRes.Items[i]))
                            item_list.Add(lstbxSelRes.Items[i]);
                    }
                }
                for (int i = 0; i < item_list.Count; i++)
                {
                    if (!lstbxAvalRes.Items.Contains((ListItem)item_list[i]))
                        lstbxAvalRes.Items.Add((ListItem)item_list[i]);
                    lstbxSelRes.Items.Remove((ListItem)item_list[i]);
                }
            }
            lstbxAvalRes.ClearSelection();
            lstbxSelRes.ClearSelection();
        }

        protected void btnAddValue_OnClick(object sender, EventArgs e)
        {
            List<ListItem> item_list = new List<ListItem>();

            if (lstbxAvaValues.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxAvaValues.Items.Count; i++)
                {
                    if (lstbxAvaValues.Items[i].Selected)
                    {
                        if (!item_list.Contains(lstbxAvaValues.Items[i]))
                            item_list.Add(lstbxAvaValues.Items[i]);
                    }
                }
                for (int i = 0; i < item_list.Count; i++)
                {
                    if (!lstbxSelValues.Items.Contains((ListItem)item_list[i]))
                        lstbxSelValues.Items.Add((ListItem)item_list[i]);
                    lstbxAvaValues.Items.Remove((ListItem)item_list[i]);
                }
            }
            lstbxAvaValues.ClearSelection();
            lstbxSelValues.ClearSelection();
        }

        protected void btnDelValue_OnClick(object sender, EventArgs e)
        {
            List<ListItem> item_list = new List<ListItem>();

            if (lstbxSelValues.SelectedIndex >= 0)
            {
                for (int i = 0; i < lstbxSelValues.Items.Count; i++)
                {
                    if (lstbxSelValues.Items[i].Selected)
                    {
                        if (!item_list.Contains(lstbxSelValues.Items[i]))
                            item_list.Add(lstbxSelValues.Items[i]);
                    }
                }
                for (int i = 0; i < item_list.Count; i++)
                {
                    if (!lstbxAvaValues.Items.Contains((ListItem)item_list[i]))
                        lstbxAvaValues.Items.Add((ListItem)item_list[i]);
                    lstbxSelValues.Items.Remove((ListItem)item_list[i]);
                }
            }
            lstbxAvaValues.ClearSelection();
            lstbxSelValues.ClearSelection();
        }

        protected void btnAddService_OnClick(object sender, EventArgs e)
        {
            ServiceTable.Visible = false;
            lblChooseAnother.Visible = true;
            TypeNode service_node = new TypeNode(ServiceBlock.Types.Service);
            Service initial_service = Service_Manager.service_list[ddlstService.SelectedValue];
            service_node.Text = initial_service.Service_Information["Name"];
            service_node.Value = initial_service.Service_Config["GUID"];
            service_node.SelectAction = TreeNodeSelectAction.Select;

            service_node.Name = initial_service.Service_Information["Name"];
            service_node.GlobalGUID = initial_service.Service_Config["GUID"];
            service_node.InstanceGUID = Guid.NewGuid().ToString();

            if (lstbxSelRes.Items.Count > 0)
            {
                ValueNode temp_node = new ValueNode(ValueNode.Types.ServiceResponse);
                temp_node.SelectAction = TreeNodeSelectAction.Select;
                temp_node.NavigateUrl = "";
                for (int i = 0; i < lstbxSelRes.Items.Count; i++)
                {
                    temp_node.values.Add(lstbxSelRes.Items[i].Text);
                }
                temp_node.Text = string.Join("<br/>", temp_node.values);

                service_node.ChildNodes.Add(temp_node);
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(service_node);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(service_node);
                }
                tvServiceFlow.ExpandAll();
            }
        }

        protected void btnAddCondition_OnClick(object sender, EventArgs e)
        {
            ConditionTable.Visible = false;
            lblChooseAnother.Visible = true;
            TypeNode condition_node = new TypeNode(ServiceBlock.Types.Condition);
            Condition condition = Service_Manager.condition_list[ddlstCondition.SelectedValue];
            condition_node.Text = condition.Name;
            condition_node.Value = condition.Name;

            condition_node.SelectAction = TreeNodeSelectAction.Select;

            condition_node.Name = condition.Name;
            condition_node.InstanceGUID = Guid.NewGuid().ToString();

            if (lstbxSelValues.Items.Count > 0)
            {
                ValueNode temp_node = new ValueNode(ValueNode.Types.ConditionValue);
                temp_node.SelectAction = TreeNodeSelectAction.Select;
                temp_node.NavigateUrl = "";
                for (int i = 0; i < lstbxSelValues.Items.Count; i++)
                {
                    temp_node.values.Add(lstbxSelValues.Items[i].Text);

                }
                temp_node.Text = string.Join("<br/>", temp_node.values);
                condition_node.ChildNodes.Add(temp_node);
                if (tvServiceFlow.SelectedNode != null)
                {
                    tvServiceFlow.SelectedNode.ChildNodes.Add(condition_node);
                }
                else
                {
                    tvServiceFlow.Nodes.Add(condition_node);
                }
                tvServiceFlow.ExpandAll();

            }
        }

        protected void tvServiceFlow_NodeChange(object sender, EventArgs e)
        {
            ConditionTable.Visible = false;
            ServiceTable.Visible = false;
            lblChooseAnother.Visible = false;
            ConditionPanel.Visible = true;
        }

        protected void btnSaveFlow_Click(object sender, EventArgs e)
        {
            sf = new ServiceFlow();
            TreeNodeCollection nodes = tvServiceFlow.Nodes;
            foreach (TreeNode n in nodes)
            {
                AddTypeNodes(n);
            }
            foreach (TreeNode n in nodes)
            {
                AddValueNodes(n);
            }
            foreach (KeyValuePair<string,ServiceBlock> kvp  in sf.blocks)
            {
                string key = kvp.Key;
                ServiceBlock value = kvp.Value;
            }
        }

        private void AddTypeNodes(TreeNode treeNode)
        {
            if (treeNode is TypeNode)
            {
                TypeNode temp_node = (TypeNode)treeNode;
                ServiceBlock sb = new ServiceBlock();
                sb.Name = temp_node.Name;
                sb.GlobalGUID = temp_node.GlobalGUID;
                sb.InstanceGUID = temp_node.InstanceGUID;
                sb.block_type = temp_node.block_type;
                sf.blocks.Add(temp_node.InstanceGUID, sb);
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
               ValueNode temp_node = (ValueNode)treeNode;
               TypeNode parent = (TypeNode)treeNode.Parent;
               TypeNode child = (TypeNode)treeNode.ChildNodes[0];
               ServiceBlock sb = sf.blocks[parent.InstanceGUID];
               sb.nextBlock.Add(temp_node.values.ToString(), child.InstanceGUID);
               sf.blocks[parent.InstanceGUID] = sb;
            }
            foreach (TreeNode tn in treeNode.ChildNodes)
            {
                AddValueNodes(tn);
            }
        }
    }
}