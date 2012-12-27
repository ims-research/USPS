using System;
using System.Collections.Generic;
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
                    ListItem condition = new ListItem
                    {
                        Text = kvp.Value.Name,
                        Value = kvp.Value.GUID
                    };
                    ddlstCondition.Items.Add(condition);
                }
            }
        }
    }
}