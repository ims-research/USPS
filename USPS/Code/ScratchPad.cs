using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace USPS.Code
{
    public class ScratchPad
    {
        //    public void scratch()
        //    {
        //        TypeNode serviceNode = new TypeNode(ServiceBlock.Types.Service);
        //        Service initialService = ServiceManager.ServiceList["0"];
        //        serviceNode.Text = initialService.ServiceInformation["Name"];
        //        serviceNode.SelectAction = TreeNodeSelectAction.Select;
        //        serviceNode.Name = initialService.ServiceInformation["Name"];
        //        serviceNode.GlobalGUID = initialService.ServiceConfig["GUID"];
        //        serviceNode.InstanceGUID = Guid.NewGuid().ToString();
        //        serviceNode.Value = serviceNode.InstanceGUID;

        //        if (lstbxSelRes.Items.Count > 0)
        //        {
        //            ValueNode tempNode = new ValueNode(ValueNode.Types.ServiceResponse) { SelectAction = TreeNodeSelectAction.Select, NavigateUrl = "", Value = Guid.NewGuid().ToString() };
        //            for (int i = 0; i < lstbxSelRes.Items.Count; i++)
        //            {
        //                tempNode.Values.Add(lstbxSelRes.Items[i].Text);
        //            }
        //            tempNode.Text = string.Join("<br/>", tempNode.Values);
        //            ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
        //            serviceNode.ChildNodes.Add(tempNode);
        //            if (tvServiceFlow.SelectedNode != null)
        //            {
        //                tvServiceFlow.SelectedNode.ChildNodes.Add(serviceNode);
        //                ((Dictionary<string, object>)Session["nodes"]).Add(serviceNode.InstanceGUID, serviceNode);
        //            }
        //            else
        //            {
        //                tvServiceFlow.Nodes.Add(serviceNode);
        //                ((Dictionary<string, object>)Session["nodes"]).Add(serviceNode.InstanceGUID, serviceNode);
        //            }
        //            tvServiceFlow.ExpandAll();
        //        }
        //    }

        //    public void scratch2()
        //    {
        //        TypeNode conditionNode = new TypeNode(ServiceBlock.Types.Condition);
        //        Condition condition = ServiceManager.ConditionList[ddlstCondition.SelectedValue];

        //        conditionNode.Text = condition.Name;
        //        conditionNode.InstanceGUID = Guid.NewGuid().ToString();
        //        conditionNode.Value = conditionNode.InstanceGUID;
        //        conditionNode.GlobalGUID = condition.GUID;
        //        conditionNode.SelectAction = TreeNodeSelectAction.Select;

        //        conditionNode.Name = condition.Name;


        //        if (lstbxSelValues.Items.Count > 0)
        //        {
        //            ValueNode tempNode = new ValueNode(ValueNode.Types.ConditionValue) {Value = Guid.NewGuid().ToString()};
        //            ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
        //            tempNode.SelectAction = TreeNodeSelectAction.Select;
        //            tempNode.NavigateUrl = "";
        //            for (int i = 0; i < lstbxSelValues.Items.Count; i++)
        //            {
        //                tempNode.Values.Add(lstbxSelValues.Items[i].Text);

        //            }
        //            tempNode.Value = Guid.NewGuid().ToString();
        //            tempNode.Text = string.Join("<br/>", tempNode.Values);
        //            conditionNode.ChildNodes.Add(tempNode);
        //            ((Dictionary<string, object>)Session["nodes"]).Add(tempNode.Value, tempNode);
        //            if (tvServiceFlow.SelectedNode != null)
        //            {
        //                tvServiceFlow.SelectedNode.ChildNodes.Add(conditionNode);
        //                ((Dictionary<string, object>)Session["nodes"]).Add(conditionNode.Value, conditionNode);
        //            }
        //            else
        //            {
        //                tvServiceFlow.Nodes.Add(conditionNode);
        //                ((Dictionary<string, object>)Session["nodes"]).Add(conditionNode.Value, conditionNode);
        //            }
        //            tvServiceFlow.ExpandAll();

        //    }
        //}
    }
}