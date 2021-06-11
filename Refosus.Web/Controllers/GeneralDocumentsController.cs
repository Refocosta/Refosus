using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System.IO;

namespace Refosus.Web.Controllers
{
    public class GeneralDocumentsController : Controller
    {
        DataContext ctx;
        public GeneralDocumentsController(DataContext _context)
        {
            ctx = _context;
        }
        public IActionResult Index()
        {
            return View(ctx.GeneralDocumentEntity.ToList());
        }
        public IActionResult Create()
        {
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
            generalDocumentEntity.Name = generalDocument.Name;
            generalDocumentEntity.Ext = ext;
            generalDocumentEntity.Path = generalDocument.Path;
            generalDocumentEntity.Alias = generalDocument.Alias;
            ctx.Add(generalDocumentEntity);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
