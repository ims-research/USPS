#region

using System;
using System.Linq;
using System.Web.UI;
using LibServiceInfo;

#endregion

namespace USPS
{
    public partial class ViewAvailableServices : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            serviceGrid.DataSource = ServiceManager.ServiceList;
            serviceGrid.DataBind();
        }

        private void GetServices(string match)
        {
            if (match == "")
            {
                serviceGrid.DataSource = ServiceManager.ServiceList;
            }
            else
            {
                var services = from entry in ServiceManager.ServiceList
                               where (entry.Value.ServiceInformation["Name"].ToLower().Contains(match.ToLower()))
                               select entry;
                serviceGrid.DataSource = services;
            }
            serviceGrid.DataBind();
        }

        protected void TextBox1TextChanged(object sender, EventArgs e)
        {
            GetServices(TextBox1.Text);
        }
    }
}