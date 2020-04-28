using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public CompaniesController(
            DataContext context,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CompanyEntity companyEntity = await _context.Companies.FindAsync(id);
            //CompanyEntity companyEntity = await _context.Companies.FirstOrDefaultAsync(m => m.Id == id);
            if (companyEntity == null)
            {
                return NotFound();
            }
            CompanyViewModel teamViewModel = _converterHelper.ToCompanyViewModel(companyEntity);
            return View(teamViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (companyViewModel.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(companyViewModel.LogoFile, "Companies");
                }
                CompanyEntity companyEntity = _converterHelper.ToCompanyEntity(companyViewModel, path, true);

                _context.Add(companyEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe una compañia con el codigo: {companyEntity.Code}");
                        }
                        if (ex.InnerException.Message.Contains("Name"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe una compañia con el nombre: {companyEntity.Name}");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View(companyViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyEntity companyEntity = await _context.Companies.FindAsync(id);
            if (companyEntity == null)
            {
                return NotFound();
            }
            CompanyViewModel teamViewModel = _converterHelper.ToCompanyViewModel(companyEntity);
            return View(teamViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyViewModel companyViewModel)
        {
            if (id != companyViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string path = companyViewModel.LogoPath;
                if (companyViewModel.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(companyViewModel.LogoFile, "Companies");
                }
                CompanyEntity companyEntity = _converterHelper.ToCompanyEntity(companyViewModel, path, false);
                _context.Update(companyEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe una compañia con el codigo: {companyEntity.Code}");
                        }
                        if (ex.InnerException.Message.Contains("Name"))
                        {
                            ModelState.AddModelError(string.Empty, $"Ya existe una compañia con el nombre: {companyEntity.Name}");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View(companyViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyEntity companyEntity = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyEntity == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(companyEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
