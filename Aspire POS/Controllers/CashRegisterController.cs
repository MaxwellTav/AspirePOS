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

        [HttpPost]
        public async Task<IActionResult> ProcesarOrden([FromBody] OrderRequestModel orden)
        {
            if (orden == null || orden.Productos.Count == 0)
            {
                Console.WriteLine("⚠️ La orden está vacía o malformada.");
                return BadRequest("La orden no puede estar vacía.");
            }

            Console.WriteLine($"📥 Orden recibida con estado: {orden.Estado}");  // <-- Verifica que llega "on-hold"

            foreach (var item in orden.Productos)
            {
                Console.WriteLine($"🆔 Producto ID: {item.ProductId}, Cantidad: {item.Quantity}");
            }

            bool resultado = await _cashRegister.ProcesarOrdenAsync(orden);

            if (!resultado)
            {
                Console.WriteLine("❌ Error al enviar la orden a WooCommerce.");
                return StatusCode(500, "Error al guardar la orden en WooCommerce");
            }

            Console.WriteLine("✅ Orden enviada a WooCommerce correctamente.");
            return Ok(new { mensaje = "Orden guardada en WooCommerce correctamente" });
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
