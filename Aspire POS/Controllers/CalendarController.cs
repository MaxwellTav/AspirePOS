﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspire_POS.Controllers
{
    [Authorize]
    public class CalendarController : BaseController
    {
        public IActionResult Index()
        {
            InitializeViewBags(false, false, false);
            return View();
        }
    }
}
