using Aspire_POS.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    public class BaseController : Controller
    {
        #region Show Message
        /// <summary>
        /// Función que muestra un mensaje en la página.
        /// </summary>
        /// <param name="title">Título del mensaje</param>
        /// <param name="body">Cuerpo del mensaje</param>
        /// <param name="response">El tipo de mensaje que es.</param>
        /// <param name="duration">Segundos para que se desaparezca.</param>
        protected void ShowMessage(string title, string body, MessageResponse response, int duration)
        {
            TempData["ToastrTitle"] = title;
            TempData["ToastrMessage"] = body;
            TempData["ToastrResponse"] = response.ToString();
            TempData["ToastrDuration"] = duration * 1000;
        }

        protected void ShowMessage(string title, string body, MessageResponse response)
        {
            ShowMessage(title, body, response, 3000);
        }

        protected void ShowMessage(string title, string body)
        {
            ShowMessage(title, body, MessageResponse.Success, 3000);
        }

        protected void ShowMessage()
        {
            ShowMessage("Título", "¡Hola Mundo!", MessageResponse.Success, 3000);
        }
        #endregion

        #region Hide Page Elements
        /// <summary>
        /// Automáticamente oculta o muestra un componente de la página.
        /// </summary>
        /// <param name="_navbar">Esconderá la barra de navegación superior.</param>
        /// <param name="_sidebar">Esconderá la barra lateral, la cuál está el menú y demás opciones.</param>
        /// <param name="_footer">Esconderá el pie de página.</param>
        public void InitializeViewBags(bool _navbar, bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = _navbar;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }

        /// <summary>
        /// Automáticamente oculta o muestra un componente de la página, la barra de navegación se oculta siempre.
        /// </summary>
        /// <param name="_sidebar"></param>
        /// <param name="_footer"></param>
        public void InitializeViewBags(bool _sidebar, bool _footer)
        {
            ViewBag.HideNavbar = true;
            ViewBag.HideSidebar = _sidebar;
            ViewBag.HideFooter = _footer;
        }

        /// <summary>
        /// Automáticamente oculta o muestra un componente de la página, la barra de navegación y el pie de página se ocultan siempre.
        /// </summary>
        /// <param name="_sidebar"></param>
        public void InitializeViewBags(bool _sidebar)
        {
            ViewBag.HideNavbar = true;
            ViewBag.HideFooter = true;
            ViewBag.HideSidebar = _sidebar;
        }

        /// <summary>
        /// Automáticamente oculta todos los componentes de la página, la barra de navegación, la barra lateral y el pie de página.
        /// </summary>
        public void InitializeViewBags()
        {
            ViewBag.HideNavbar = true;
            ViewBag.HideFooter = true;
            ViewBag.HideSidebar = true;
        }
        #endregion
    }

    public enum MessageResponse
    {
        Success,
        Warning,
        Info,
        Error
    }
}
