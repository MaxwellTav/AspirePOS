using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class CashRegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
