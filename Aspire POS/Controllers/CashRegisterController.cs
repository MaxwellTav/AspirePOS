using Aspire_POS.Models;
using Aspire_POS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class CashRegisterController : BaseController
    {
        private readonly CashRegisterService _cashRegister;
        private readonly IMemoryCache _cache;

        public CashRegisterController(IMemoryCache cache, CashRegisterService cashRegister)
        {
            _cashRegister = cashRegister;
            _cache = cache;
        }

        #region Create

        #endregion

        #region Read
        public async Task<IActionResult> Index()
        {
            InitializeViewBags(true, true, true);

            var products = await _cashRegister.GetProductsAsync();

            UserModel user = new()
            {
                //Aquí se debería obtener el usuario actual, se quedará pendiente.
            };

            var model = new CashRegisterMainModel
            {
                CurrentUser = user,
                CashRegisters = products
            };

            return View(model);
        }

        public IActionResult AdminCashier()
        {
            InitializeViewBags(false, false, false);
            return View();
        }

        public IActionResult SeeSesions()
        {
            InitializeViewBags(false, false, false);
            return View();
        }
        #endregion

        #region Update

        #endregion

        #region Delete

        #endregion
    }
}
