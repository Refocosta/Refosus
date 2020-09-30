using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Refosus.Web.Controllers
{
    public class BSCController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult Info()
        {
            return View();
        }
    }
}