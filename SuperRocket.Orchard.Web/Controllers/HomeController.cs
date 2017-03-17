using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace SuperRocket.Orchard.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : OrchardControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}