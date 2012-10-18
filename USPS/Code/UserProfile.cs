using System;
using System.Web.Profile;
using System.Web.Security;
using System.Collections.Generic;
namespace USPS.Code
{
    [Serializable]
    public class UserProfile : ProfileBase
    {
        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }
        public static UserProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as UserProfile;
        }

        [SettingsAllowAnonymous(false)]
        public string ServiceFlows
        {
            get { return base["ServiceFlows"] as string; }
            set { base["ServiceFlows"] = value; }
        }

    }
}