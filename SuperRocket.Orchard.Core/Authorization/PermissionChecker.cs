using Abp.Authorization;
using SuperRocket.Orchard.Authorization.Roles;
using SuperRocket.Orchard.MultiTenancy;
using SuperRocket.Orchard.Users;

namespace SuperRocket.Orchard.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
