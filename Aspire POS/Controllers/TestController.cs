using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
