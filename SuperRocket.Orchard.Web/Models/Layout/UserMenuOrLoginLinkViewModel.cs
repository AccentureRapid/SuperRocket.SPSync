using SuperRocket.Orchard.Sessions.Dto;

namespace SuperRocket.Orchard.Web.Models.Layout
{
    public class UserMenuOrLoginLinkViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        public bool IsMultiTenancyEnabled { get; set; }

        public string GetShownLoginName()
        {
            var userName = "<span id=\"HeaderCurrentUserName\">" + LoginInformations.User.UserName + "</span>";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return LoginInformations.Tenant == null
                ? ".\\" + userName
                : LoginInformations.Tenant.TenancyName + "\\" + userName;
        }
    }
}