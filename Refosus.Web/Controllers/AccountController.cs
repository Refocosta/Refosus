using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;

namespace Refosus.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IUserHelper userHelper,
            DataContext dataContext,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _context = dataContext;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "Usuario o contraseña incorrectos.");
            }
            return View(model);
        } 
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }

        #region Roles
        public async Task<IActionResult> IndexRoles()
        {
            return View(await _context.
                Roles
                .Include(t =>t.roleMenus)
                .OrderBy(t => t.Name)
                .ToListAsync());
        }
        public async Task<IActionResult> DetailsRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoleEntity roleEntity = await _context.Roles
                .Include(g => g.roleMenus)
                .ThenInclude(t => t.Menu)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (roleEntity == null)
            {
                return NotFound();
            }
            return View(roleEntity);
        }
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleEntity role)
        {
            if (ModelState.IsValid)
            {
                await _userHelper.CheckRoleAsync(role.Name);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexRoles", new RouteValueDictionary(
                new { controller = "Account", action = "IndexRoles" }));
            }
            return View(role);
        }
        public async Task<IActionResult> DeleteRole(String id)
        {
            if (id == null)
            {
                NotFound();
            }
            RoleEntity roleEntity = await _userHelper.GetRoleByIdAsync(id);
            if (roleEntity == null)
            {
                return NotFound();
            }
            await _userHelper.RemoveRoleAsync(roleEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexRoles", new RouteValueDictionary(
                new { controller = "Account", action = "IndexRoles" }));
        }
        #endregion



        #region RoleMenu
        public async Task<IActionResult> AddRoleMenus(string id)
        {
            if (id == null)
            {
                NotFound();
            }
            RoleEntity roleEntity = await _userHelper.GetRoleByIdAsync(id);
            if (roleEntity == null)
            {
                return NotFound();
            }
            RoleMenusViewModel model = new RoleMenusViewModel
            {
                Role = roleEntity,
                RoleId = roleEntity.Id,
                Menus = _combosHelper.GetComboMenus()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleMenus(RoleMenusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                RoleMenuEntity roleMenuEntity = await _converterHelper.ToRoleMenuEntityAsync(model, true);
                _context.Add(roleMenuEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsRole", new RouteValueDictionary(
                new { controller = "Account", action = "DetailsRole", Id = model.RoleId }));
            }
            model.Role = await _context.Roles.FindAsync(model.RoleId);
            model.Menus = _combosHelper.GetComboMenus();
            return View(model);
        }
        public async Task<IActionResult> DeleteRoleMenu(int? id)
        {
            {
                if (id == null)
                {
                    NotFound();
                }
                RoleMenuEntity roleMenuEntity = await _context.RoleMenus
                    .Include(g => g.Role)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (roleMenuEntity == null)
                {
                    return NotFound();
                }
                _context.RoleMenus.Remove(roleMenuEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsRole", new RouteValueDictionary(
                        new { controller = "Account", action = "DetailsRole"}));
            }
        }
        #endregion
    }
}