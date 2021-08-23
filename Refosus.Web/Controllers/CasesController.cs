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
using FluentDateTime;

namespace Refosus.Web.Controllers
{
    public class CasesController : Controller
    {
        DataContext ctx;
        public CasesController(DataContext _context)
        {
            ctx = _context;
        }

        [Authorize(Roles = "maintenanceCreatorAdministrator, maintenanceAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Index(string? message)
        {
            ViewBag.message = message;
            List<CaseEntity> facade;
            if (User.IsInRole("maintenanceFilterAdministrator"))
            {
                facade = ctx.CaseEntity.Where(x => x.Status == 1 || x.Status == 2 || x.Status == 3).ToList();
            }
            else
            {
                facade = ctx.CaseEntity.Where(x => x.Status == 1 || x.Status == 2 || x.Status == 3).Where(x => x.Sender == User.Identity.Name || x.Responsable == User.Identity.Name).ToList();
            }

            return View(facade);
        }

        [Authorize(Roles = "maintenanceAdministrator, maintenanceCreatorAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Create()
        {
            ViewBag.typesCases = ctx.TypeCaseEntity.ToList();
            ViewBag.businessUnits = ctx.BusinessUnitEntity.ToList();
            ViewBag.usersList = ctx.Users.ToList();
            return View();
        }

        [BindProperty]
        public CaseEntity caseEntity { get; set; }
        [Authorize(Roles = "maintenanceCreatorAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Store()
        {
            caseEntity.Responsable = (String.IsNullOrEmpty(caseEntity.Responsable)) ? "filter@refocosta.com" : caseEntity.Responsable;
            caseEntity.Sender = (String.IsNullOrEmpty(caseEntity.Sender)) ? User.Identity.Name : caseEntity.Sender;
            caseEntity.Status = 1;
            caseEntity.Code = this.Random();
            caseEntity.Solution = "Este caso aún no tiene respuesta";
            caseEntity.CreatedAt = System.DateTime.Now.ToUniversalTime();
            caseEntity.DeadLine = (caseEntity.DeadLine == DateTime.MinValue) ? DateTime.Now.AddBusinessDays(1) : caseEntity.DeadLine;
            ctx.Add(caseEntity);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "maintenanceAdministrator, maintenanceCreatorAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Details(int id)
        {
            CaseEntity details = ctx.CaseEntity.Where(x => x.Id == id).Where(x => x.Status == 1 || x.Status == 2 || x.Status == 3).Include(t => t.TypesCases).Include(t => t.BusinessUnits).FirstOrDefault();
            if (details == null)
            {
                return RedirectToAction("Index");
            }
            return View(details);
        }

        [Authorize(Roles = "maintenanceCreatorAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Edit(int id)
        {
            CaseEntity edit = ctx.CaseEntity.Where(x => x.Id == id).Where(x => x.Status == 1 || x.Status == 2 || x.Status == 3).Include(t => t.TypesCases).Include(t => t.BusinessUnits).FirstOrDefault();
            if (edit == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.typesCases = ctx.TypeCaseEntity.ToList();
            ViewBag.businessUnits = ctx.BusinessUnitEntity.ToList();
            ViewBag.usersList = ctx.Users.ToList();
            return View(edit);
        }

        [BindProperty]
        public CaseEntity caseEntityUpdate { get; set; }
        [Authorize(Roles = "maintenanceCreatorAdministrator, maintenanceFilterAdministrator")]
        public IActionResult Update()
        {
            CaseEntity update = ctx.CaseEntity.Find(caseEntityUpdate.Id);
            if (update != null)
            {
                update.Issue = caseEntityUpdate.Issue;
                update.Description = caseEntityUpdate.Description;
                update.Sender = (!String.IsNullOrEmpty(update.Sender)) ? update.Sender : User.Identity.Name;
                update.TypesCasesId = caseEntityUpdate.TypesCasesId;
                update.Priority = caseEntityUpdate.Priority;
                update.Responsable = caseEntityUpdate.Responsable;
                update.BusinessUnitsId = caseEntityUpdate.BusinessUnitsId;
                update.Ubication = caseEntityUpdate.Ubication;
                ctx.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = caseEntityUpdate.Id });
        }

        [Authorize(Roles = "maintenanceAdministrator")]
        public IActionResult Solution(int id)
        {
            CaseEntity solution = ctx.CaseEntity.Where(x => x.Id == id).Where(x => x.Status == 1 || x.Status == 3).FirstOrDefault();
            if (solution == null)
            {
                return RedirectToAction("Index");
            }
            return View(solution);
        }

        [BindProperty]
        public CaseEntity caseEntitySolution { get; set; }
        [Authorize(Roles = "maintenanceAdministrator")]
        public IActionResult StoreSolution()
        {
            CaseEntity update = ctx.CaseEntity.Find(caseEntitySolution.Id);
            
            if (update != null)
            {
                update.Solution = caseEntitySolution.Solution;
                update.Hours = caseEntitySolution.Hours;
                update.ClosingDate = System.DateTime.Now.ToUniversalTime();
                update.Status = 2;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "maintenanceFilterAdministrator")]
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

        [Authorize(Roles = "maintenanceFilterAdministrator")]
        public IActionResult Thrash()
        {
            return View();
        }

        [Authorize(Roles = "maintenanceFilterAdministrator")]
        public IActionResult Enable()
        {
            return Json(true);
        }

        public IActionResult Atention(int id)
        {
            CaseEntity update = ctx.CaseEntity.Find(id);
            if (update != null)
            {
                update.Fulfillment = 2;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", new { message = "Se ha hecho el llamado de atención del caso " + update.Code });
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
