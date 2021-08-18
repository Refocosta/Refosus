using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refosus.Web.Data.Entities;
using Refosus.Web.Data;

namespace Refosus.Web.Controllers
{
    public class TypesCasesController : Controller
    {

        private DataContext ctx;

        public TypesCasesController(DataContext dataContext)
        {
            this.ctx = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [BindProperty]
        public TypeCaseEntity typeCaseEntity { get; set; }
        public IActionResult Store()
        {
            typeCaseEntity.Status = 1;
            typeCaseEntity.CreateAt = DateTime.Now.ToUniversalTime();
            ctx.Add(typeCaseEntity);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            return null;
        }

        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}
