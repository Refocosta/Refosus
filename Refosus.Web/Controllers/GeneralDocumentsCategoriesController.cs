using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;

namespace Refosus.Web.Controllers
{
    [Authorize(Roles = "generalDocumentsAdministrator")]
    public class GeneralDocumentsCategoriesController : Controller
    {
        DataContext ctx;
        public GeneralDocumentsCategoriesController(DataContext _context)
        {
            ctx = _context;
        }
        public IActionResult Index()
        {
            return View(ctx.GeneralDocumentCategoryEntity.Where(x => x.Status == 1).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [BindProperty]
        public GeneralDocumentCategoryEntity generalDocumentCategory { get; set; }
        public IActionResult Store()
        {
            generalDocumentCategory.Status = 1;
            ctx.Add(generalDocumentCategory);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var edit = ctx.GeneralDocumentCategoryEntity.Find(id);
            return View(edit);
        }

        [BindProperty]
        public GeneralDocumentCategoryEntity generalDocumentCategoryUpdate { get; set; }
        public IActionResult Update()
        {
            var update = ctx.GeneralDocumentCategoryEntity.Find(generalDocumentCategoryUpdate.Id);
            if (update != null)
            {
                update.Name = generalDocumentCategoryUpdate.Name;
                update.Description = generalDocumentCategoryUpdate.Description;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var delete = ctx.GeneralDocumentCategoryEntity.Find(id);
            if (delete != null)
            {
                delete.Status = 0;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
