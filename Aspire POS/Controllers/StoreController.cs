using Aspire_POS.Models;
using Aspire_POS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private readonly StoreService _storeService;

        public StoreController(StoreService storeService)
        {
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);

            var model = new StoreMainModel
            {
                CurrentProducts = await _storeService.GetProductCountAsync()
            };

            return View(model);
        }
    }
}
