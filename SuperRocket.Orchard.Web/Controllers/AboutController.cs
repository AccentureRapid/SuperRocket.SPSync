using SuperRocket.Orchard.Services;
using System.Web.Mvc;

namespace SuperRocket.Orchard.Web.Controllers
{
    public class AboutController : OrchardControllerBase
    {
        private readonly ITestService _testService;

        public AboutController(ITestService testService)
        {
            _testService = testService;
        }
        public ActionResult Index()
        {
            _testService.Test();
            return View();
        }
	}
}