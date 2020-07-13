using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class MenusController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public MenusController(DataContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Menus
                .Include(t => t.Menu)
                .OrderBy(t => t.Name)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            MenuViewModel model = new MenuViewModel
            {
                Menus = _combosHelper.GetComboMenus()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (model.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Menus");
                }
                MenuEntity menuEntity = await _converterHelper.ToMenuEntityAsync(model, path, true);
                _context.Add(menuEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "Menus", action = "Index" }));
            }
            model.Menus = _combosHelper.GetComboMenus();
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuEntity menuEntity = await _context.Menus
                .Include(g => g.Menu)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (menuEntity == null)
            {
                return NotFound();
            }
            MenuViewModel menuViewEntity = _converterHelper.ToMenuViewModel(menuEntity);
            return View(menuViewEntity);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuEntity menuEntity = await _context.Menus
                .Include(g => g.Menu)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (menuEntity == null)
            {
                return NotFound();
            }
            MenuViewModel menuViewModel = _converterHelper.ToMenuViewModel(menuEntity);
            return View(menuViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string path = model.LogoPath;
                if (model.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Menus");
                }
                MenuEntity menuEntity = await _converterHelper.ToMenuEntityAsync(model, path, false);
                _context.Update(menuEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        if (ex.InnerException.Message.Contains("Name"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe un Menu con el nombre: {menuEntity.Name}");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            model.Menus = _combosHelper.GetComboMenus();
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            MenuEntity menuEntity = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuEntity == null)
            {
                return NotFound();
            }
            _context.Menus.Remove(menuEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "Menus", action = "Index" }));
        }
    }
}