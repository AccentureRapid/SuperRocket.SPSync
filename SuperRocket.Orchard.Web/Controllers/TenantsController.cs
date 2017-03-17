using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using SuperRocket.Orchard.Authorization;
using SuperRocket.Orchard.MultiTenancy;

namespace SuperRocket.Orchard.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : OrchardControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}