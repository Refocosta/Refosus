using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System.IO;

namespace Refosus.Web.Controllers
{
    [Authorize(Roles = "generalDocumentsAdministrator")]
    public class GeneralDocumentsController : Controller
    {
        DataContext ctx;
        public GeneralDocumentsController(DataContext _context)
        {
            ctx = _context;
        }
        public IActionResult Index()
        {
            return View(ctx.GeneralDocumentEntity.Where(x => x.Status == 1).ToList());
        }
        public IActionResult Create(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        
        [BindProperty]
        public GeneralDocument generalDocument { get; set; }
        public IActionResult Store()
        {
            string random = Guid.NewGuid().ToString();
            string ext = Path.GetExtension(generalDocument.File.FileName);
            string file = $"{random}" + ext;
            string current = Directory.GetCurrentDirectory();
            string path = Path.Combine(current, $"wwwroot\\generalDocuments", file);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                generalDocument.File.CopyTo(stream);
                stream.Flush();
            }
            generalDocument.Name = generalDocument.File.FileName;
            generalDocument.Path = $"~/GeneralDocuments/{file}";
            GeneralDocumentEntity generalDocumentEntity = new GeneralDocumentEntity();
            generalDocumentEntity.Alias = generalDocument.Alias;
            generalDocumentEntity.Name = generalDocument.Name;
            generalDocumentEntity.Path = generalDocument.Path;
            generalDocumentEntity.Ext = ext;
            generalDocumentEntity.Status = 1;
            generalDocumentEntity.GeneralDocumentsCategoriesId = generalDocument.GeneralDocumentsCategoriesId;
            generalDocumentEntity.CreateAt = System.DateTime.Now.ToUniversalTime();
            ctx.Add(generalDocumentEntity);
            ctx.SaveChanges();
            return RedirectToAction("ByCategorie", new { id = generalDocument.GeneralDocumentsCategoriesId });
        }

        public IActionResult Edit(int id)
        {
            var edit = ctx.GeneralDocumentEntity.Find(id);
            ViewBag.IdCate = edit.GeneralDocumentsCategoriesId;
            return View(edit);
        }

        [BindProperty]
        public GeneralDocument GeneralDocumentUpdate { get; set; }
        public IActionResult Update()
        {
            var update = ctx.GeneralDocumentEntity.Find(GeneralDocumentUpdate.Id);
            if (update != null)
            {
                update.Alias = GeneralDocumentUpdate.Alias;
                ctx.SaveChanges();
            }
            return RedirectToAction("ByCategorie", new { id = update.GeneralDocumentsCategoriesId });
        }

        public IActionResult Delete(int id)
        {
            var delete = ctx.GeneralDocumentEntity.Find(id);
            if (delete != null)
            {
                delete.Status = 0;
                ctx.SaveChanges();
            }
            return RedirectToAction("ByCategorie", new { id = delete.GeneralDocumentsCategoriesId });
        }

        public IActionResult ByCategorie(int id)
        {
            ViewBag.Id = id;
            return View(ctx.GeneralDocumentEntity.Where(s => s.GeneralDocumentsCategoriesId == id).Where(x => x.Status == 1).ToList());
        }
    }
}
