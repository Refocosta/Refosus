using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;

namespace Refosus.Web.Controllers
{
    public class GeneralDocumentsCategoriesController : Controller
    {
        DataContext ctx;
        public GeneralDocumentsCategoriesController(DataContext _context)
        {
            ctx = _context;
        }
        public IActionResult Index()
        {
            return View(ctx.GeneralDocumentCategoryEntity.ToList());
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
    }
}
