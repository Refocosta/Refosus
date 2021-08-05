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
using Microsoft.EntityFrameworkCore;

namespace Refosus.Web.Controllers
{
    [Authorize(Roles = "maintenanceAdministrator")]
    public class CasesController : Controller
    {
        DataContext ctx;
        public CasesController(DataContext _context)
        {
            ctx = _context;
        }
        public IActionResult Index()
        {
            return View(ctx.CaseEntity.Where(x => x.Status == 1).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.typesCases = ctx.TypeCaseEntity.ToList();
            ViewBag.businessUnits = ctx.BusinessUnitEntity.ToList();
            ViewBag.usersList = ctx.Users.ToList();
            return View();
        }

        [BindProperty]
        public CaseEntity caseEntity { get; set; }
        public IActionResult Store()
        {
            caseEntity.Sender = User.Identity.Name;
            caseEntity.Status = 1;
            caseEntity.Code = this.Random();
            caseEntity.Solution = "Este caso aún no tiene respuesta";
            caseEntity.CreatedAt = System.DateTime.Now.ToUniversalTime();
            ctx.Add(caseEntity);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            CaseEntity details = ctx.CaseEntity.Where(x => x.Id == id).Where(x => x.Status == 1).Include(t => t.TypesCases).Include(t => t.BusinessUnits).FirstOrDefault();
            if (details == null)
            {
                RedirectToAction("Index");
            }
            return View(details);
        }

        public IActionResult Edit(int id)
        {
            CaseEntity edit = ctx.CaseEntity.Where(x => x.Id == id).Include(t => t.TypesCases).Include(t => t.BusinessUnits).FirstOrDefault();
            ViewBag.typesCases = ctx.TypeCaseEntity.ToList();
            ViewBag.businessUnits = ctx.BusinessUnitEntity.ToList();
            ViewBag.usersList = ctx.Users.ToList();
            return View(edit);
        }

        [BindProperty]
        public CaseEntity caseEntityUpdate { get; set; }
        public IActionResult Update()
        {
            CaseEntity update = ctx.CaseEntity.Find(caseEntityUpdate.Id);
            if (update != null)
            {
                update.Issue = caseEntityUpdate.Issue;
                update.Description = caseEntityUpdate.Description;
                update.Sender = User.Identity.Name;
                update.TypesCasesId = caseEntityUpdate.TypesCasesId;
                update.Priority = caseEntityUpdate.Priority;
                update.Responsable = caseEntityUpdate.Responsable;
                update.BusinessUnitsId = caseEntityUpdate.BusinessUnitsId;
                update.Ubication = caseEntityUpdate.Ubication;
                ctx.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = caseEntityUpdate.Id });
        }

        public IActionResult Solution(int id)
        {
            return View();
        }

        public IActionResult StoreSolution()
        {
            return Json(true);
        }

        public IActionResult Delete(int id)
        {
            var delete = ctx.CaseEntity.Find(id);
            if (delete == null)
            {
                return RedirectToAction("Index");
            }
            delete.Status = 0;
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Thrash()
        {
            return View();
        }

        public IActionResult Enable()
        {
            return Json(true);
        }

        private String Random()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
    }
}
