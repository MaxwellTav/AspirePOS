using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
