using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using Refosus.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Refosus.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ISecurityHelper _securityHelper;
        public AccountController(IUserHelper userHelper,
            DataContext dataContext,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            ISecurityHelper securityHelper)
        {
            _userHelper = userHelper;
            _context = dataContext;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _securityHelper = securityHelper;
        }
        #region System
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
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {

                    UserEntity user = await _userHelper.GetUserAsync(model.UserName);
                    if (user.IsActive == true)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await _userHelper.LogoutAsync();
                        ModelState.AddModelError(string.Empty, "Usuario no activo");
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
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
        #endregion
        #region Roles
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> IndexRoles()
        {
            return View(await _context.
                Roles
                .Include(t => t.roleMenus)
                .OrderBy(t => t.Name)
                .ToListAsync());
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public IActionResult AddRole()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRole(string id)
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
        #region Users
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> IndexUsers()
        {
            return View(await _userHelper.GetUsersAsync());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AddUserAsync()
        {
            UserViewModel model = new UserViewModel
            {

                DocumentTypes = _combosHelper.GetComboDocumentType(),
                Companies = _combosHelper.GetComboCompany()
            };
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    user = await _converterHelper.ToUserEntityAsync(model, true);
                    await _userHelper.AddUserAsync(user, model.Password);
                }
                return RedirectToAction("IndexUsers", new RouteValueDictionary(
                new { controller = "Account", action = "IndexUsers" }));
            }
            model.DocumentTypes = _combosHelper.GetComboDocumentType();
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DetailsUser(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserRolesViewModel modelView = new UserRolesViewModel
            {
                User = await _context.Users
                .Include(t => t.TypeDocument)
                .FirstOrDefaultAsync(g => g.Id == id)
            };

            modelView.Roles = await _securityHelper.GetRoleByUserAsync(modelView.User);

            if (modelView == null)
            {
                return NotFound();
            }
            return View(modelView);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserEntity userEntity =
                userEntity = await _context.Users
                .Include(t => t.TypeDocument)
                .Include(t => t.Company)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (userEntity == null)
            {
                return NotFound();
            }
            UserViewModel userViewModel = _converterHelper.ToUserViewModel(userEntity);
            return View(userViewModel);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserByIdAsync(model.Id);
                user.TypeDocument = await _context.DocumentTypes.FirstOrDefaultAsync(t => t.Id == model.DocumentTypeId);
                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.IsActive = model.IsActive;
                user.Company = await _context.Companies.FirstOrDefaultAsync(t => t.Id == model.CompanyId);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexUsers", new RouteValueDictionary(
                        new { controller = "Account", action = "IndexUsers" }));
            }
            return View(model);
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUserRole(string email, string name)
        {
            {
                if (email == null && name == null)
                {
                    NotFound();
                }
                UserEntity modelUser = await _context.Users
                .FirstOrDefaultAsync(g => g.Email == email)
                ;
                if (modelUser == null)
                {
                    return NotFound();
                }
                await _userHelper.RemoveUserToRoleAsync(modelUser, name);
                return RedirectToAction("DetailsUser", new RouteValueDictionary(
                        new { controller = "Account", action = "DetailsUser", Id = modelUser.Id }));

            }
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Contraseña Cambiada con exito");
                    return View(new ChangePasswordViewModel { });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangePasswordAllUser()
        {
            ChangePasswordAllUserViewModel model = new ChangePasswordAllUserViewModel
            {
                Users = _combosHelper.GetComboUser()
            };
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAllUser(ChangePasswordAllUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
                string token = await _userHelper.GenerateTokenChangePasswordAllAsync(user);
                Microsoft.AspNetCore.Identity.IdentityResult result = await _userHelper.ChangePasswordAllUserAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Contraseña Cambiada con exito");
                    return View(new ChangePasswordAllUserViewModel { Users = _combosHelper.GetComboUser() });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
            }
            model.Users = _combosHelper.GetComboUser();
            return View(model);
        }





        [Authorize]
        public async Task<IActionResult> ChangeUser()
        {
            UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
            UserChangeViewModel model = _converterHelper.ToUserChangeViewModelAsync(user);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeUser(UserChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model.Companies = _combosHelper.GetComboCompany();
                model.DocumentTypes = _combosHelper.GetComboDocumentType();
                user.Company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == model.CompanyId);
                user.TypeDocument = await _context.DocumentTypes.FirstOrDefaultAsync(d => d.Id == model.DocumentTypeId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;

                string path = string.Empty;
                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Users", model.Email);
                    user.PhotoPath = path;
                }
                _context.Update(user);
                await _context.SaveChangesAsync();
                ModelState.AddModelError(string.Empty, "Se actualizaron los datos correctamente.");
                model.PhotoPath = user.PhotoPath;
                return View(model);
            }
            return View(model);
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUserRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserEntity user = await _userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserRolesViewModel model = new UserRolesViewModel
            {
                User = user,
                userId = user.Id,
                ListRoles = _userHelper.GetRolesCombo()
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUserRole(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.User = await _userHelper.GetUserByIdAsync(model.userId);
                model.Rol = await _userHelper.GetRoleByIdAsync(model.rolesId);
                await _userHelper.AddUserToRoleAsync(model.User, model.Rol.Name);
                return RedirectToAction("DetailsUser", new RouteValueDictionary(
    new { controller = "Account", action = "DetailsUser", Id = model.userId }));
            }
            return View(model);
        }

        #endregion
        #region RoleMenu
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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
                        new { controller = "Account", action = "DetailsRole" }));
            }
        }
        #endregion
    }
}