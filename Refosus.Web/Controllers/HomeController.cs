using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ISecurityHelper _securityHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public HomeController(ILogger<HomeController> logger,
            IUserHelper userHelper,
            DataContext context,
            ISecurityHelper securityHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
            _context = context;
            _securityHelper = securityHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.News.ToListAsync());
        }

        public async Task<IActionResult> IndexNews()
        {
            return View(await _context.News.ToListAsync());
        }

        public IActionResult CreateNew()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(NewViewModel newViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (newViewModel.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(newViewModel.LogoFile, "News");
                }
                NewEntity newEntity = _converterHelper.ToNewEntity(newViewModel, path, true);

                _context.Add(newEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(IndexNews));
                }
                catch (Exception ex)
                {
                        ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(newViewModel);
        }

        public async Task<IActionResult> DetailsNew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewEntity newEntity = await _context.News
                .FirstOrDefaultAsync(g => g.Id == id);

            if (newEntity == null)
            {
                return NotFound();
            }
            return View(newEntity);
        }

        public async Task<IActionResult> EditNew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewEntity newEntity = await _context.News
                .FirstOrDefaultAsync(g => g.Id == id);
            if (newEntity == null)
            {
                return NotFound();
            }
            NewViewModel newViewModel = _converterHelper.ToNewViewModel(newEntity);
            return View(newViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNew(int id, NewViewModel model)
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
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "News");
                }

                NewEntity newEntity =  _converterHelper.ToNewEntity(model, path, false);

                _context.Update(newEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(IndexNews));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteNew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewEntity newEntity = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newEntity == null)
            {
                return NotFound();
            }

            _context.News.Remove(newEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexNews));
        }

        public async Task<IActionResult> _MenuAsync()
        {
            string url = Request.HttpContext.Request.GetDisplayUrl();
            if (User.Identity.IsAuthenticated == true)
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                List<RoleMenuEntity> menus = await _securityHelper.GetMenusRoleAsync(user);
                return PartialView("_menu", menus);
            }
            return NotFound();
        }
        public async Task<IActionResult> _UserPhotoInfoAsync()
        {
            string url = Request.HttpContext.Request.GetDisplayUrl();
            if (User.Identity.IsAuthenticated == true)
            {
                UserEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                UserPhotoInfoModel info=new UserPhotoInfoModel();
                info.PhotoPath = user.PhotoPath;
                info.Name = user.FullName;
                return PartialView("_UserPhotoInfo", info);
            }
            return NotFound();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
