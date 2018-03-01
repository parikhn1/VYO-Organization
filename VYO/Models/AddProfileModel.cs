using System.Web.Profile;
using System.Web.Security;

namespace VYO.Models
{
    public class AddProfileModel:ProfileBase
    {
        static public AddProfileModel CurrentUser
        {
            get
            {
                return (AddProfileModel)
                       (Create(Membership.GetUser().UserName));
            }
        }

        public new string UserName
        {
            get { return ((string)(base["UserName"])); }
            set { base["UserName"] = value; Save(); }
        }
    }
}