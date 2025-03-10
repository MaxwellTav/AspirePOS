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
            //CashRegisterMainModel model = await _cashRegister.GetCashRegisterAsync();
            return View();
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
