#region

using System;
using System.Web.Profile;
using System.Web.Security;

#endregion

namespace USPS.Code
{
    [Serializable]
    public class UserProfile : ProfileBase
    {
        [SettingsAllowAnonymous(false)]
        public string ServiceFlows
        {
            get { return base["ServiceFlows"] as string; }
            set { base["ServiceFlows"] = value; }
        }

        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }

        public static UserProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as UserProfile;
        }
    }
}