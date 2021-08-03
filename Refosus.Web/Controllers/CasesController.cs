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

        public IActionResult Details(int id)
        {
            //var show = ctx.CaseEntity.Find(id);
            var details = ctx.CaseEntity.Where(x => x.Id == id).Include(t => t.TypesCases).ToList();
            //return View(details);
            return Json(details);

        }
    }
}
