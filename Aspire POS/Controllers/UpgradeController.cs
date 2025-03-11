using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class UpgradeController : BaseController
    {
        public IActionResult Index()
        {
            InitializeViewBags();
            return View();
        }
    }
}
