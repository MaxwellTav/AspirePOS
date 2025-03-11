using Aspire_POS.Models;
using Aspire_POS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class InventoryController : BaseController
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);

            var model = new InventoryModel
            {
                Products = await _inventoryService.GetInventoryAsync()
            };

            return View(model);
        }
    }
}
