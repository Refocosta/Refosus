﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class TracingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
