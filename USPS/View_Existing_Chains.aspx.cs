using System;
using System.Collections.Generic;
using USPS.Code;

namespace USPS
{
    public partial class View_Existing_Chains : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
            List<ServiceFlow> sf;
            try
            {
                sf = profile.ServiceFlows.Deserialize<List<ServiceFlow>>();
                sf[0].ToString();
            }
            catch (Exception)
            {
                sf = new List<ServiceFlow>();
            }
            //rpt1.DataSource = sf;
            ServiceFlow sf1 = sf[0];
            //rpt1.DataBind();
        }
    }
}