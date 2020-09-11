using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data.EntitiesTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    [Authorize(Roles = "Administrator,TEAdmininistrator")]
    public class TEController : Controller
    {

        private readonly RefocostaContext _context;

        public TEController(RefocostaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .WorkStream
                .Include(t => t.IdResponsableNavigation)
                .Include(t => t.Iniciativas)
                .ToListAsync());
        }
        public async Task<IActionResult> DetailsWorkStream(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            WorkStreamEntity workStreamEntity = await _context.WorkStream
                .Include(t => t.Iniciativas)
                .ThenInclude(t => t.IdResponsableNavigation)
                .FirstOrDefaultAsync(m => m.IdWorkStream == id);
            //workStreamEntity.L0 = _context.WorkStream
            //    .Include(t => t.Iniciativas.Where(q=>q.Etapa=="L0"))
            //    .ThenInclude(t=>t.Etapa.Where(q=>q))
            //    .ToList().Count();




            if (workStreamEntity == null)
            {
                return NotFound();
            }
            return View(workStreamEntity);
        }
    }
}
