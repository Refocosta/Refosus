using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
