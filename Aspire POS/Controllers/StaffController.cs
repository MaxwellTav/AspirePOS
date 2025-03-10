using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aspire_POS.Services;
using Aspire_POS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class StaffController : BaseController
    {
        private readonly StaffService _staffService;
        private readonly IMemoryCache _cache;

        private readonly bool editByPage = true;

        public StaffController(StaffService staffService, IMemoryCache cache)
        {
            _staffService = staffService;
            _cache = cache;
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            InitializeViewBags(false, false, false);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            InitializeViewBags(false, false, false);
            //if (!ModelState.IsValid)
            //{
            //    TempData["ErrorMessage"] = "Por favor, completa todos los campos correctamente.";
            //    return View(model);
            //}

            model.Roles = new List<string> { model.Roles.FirstOrDefault() };

            bool result = await _staffService.CreateUserAsync(model);

            if (result)
            {
                ShowMessage("Éxito", "Se ha creado el usuario de manera exitosa.", MessageResponse.Success);
                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion

        #region Read
        public async Task<IActionResult> Index()
        {
            InitializeViewBags(false, false, false);
            StaffMainModel model = await _staffService.GetStaffAsync();
            return View(model);
        }
        #endregion

        #region Update
        public IActionResult Edit(int id)
        {
            InitializeViewBags(false, false, false);

            if (editByPage)
            {
                if (!_cache.TryGetValue("HostCredentials", out HostCredentialsModel credentials) || string.IsNullOrEmpty(credentials.ApiUrl))
                {
                    return NotFound();
                }

                string externalUrl = credentials.ApiUrl + $"wp-admin/user-edit.php?user_id={id}";
                return Redirect(externalUrl);
            }

            if (id <= 0)
                return NotFound();

            return View(new UserModel { Id = id });
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "❌ Verifica los datos ingresados.";
                return View(model);
            }

            bool result = await _staffService.UpdateUserAsync(model);

            if (result)
            {
                TempData["SuccessMessage"] = "✅ Usuario actualizado exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "❌ Error al actualizar el usuario.";
            }

            return View(model);
        }

        #endregion 

        #region Delete

        #endregion
    }
}
