using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using USPS.Code;
namespace USPS
{
    public partial class View_Available_Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            serviceGrid.DataSource = Service_Manager.service_list;
            serviceGrid.DataBind();
        }

        private void Get_Services(string match)
        {
            if (match == "")
            {
                serviceGrid.DataSource = Service_Manager.service_list;
            }
            else
            {
                var services = from entry in Service_Manager.service_list
                               where (entry.Value.Service_Information["Name"].ToLower().Contains(match.ToLower()))
                               select entry;
                serviceGrid.DataSource = services;
            }
          serviceGrid.DataBind();
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Get_Services(TextBox1.Text);
        }
    }
}