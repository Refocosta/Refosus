using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly DataContext _context;
        public UserHelper(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            SignInManager<UserEntity> signInManager,
            DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        #region Usuarios

        public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> ChangePasswordAllUserAsync(UserEntity user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<string> GenerateTokenChangePasswordAllAsync(UserEntity user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEntity user)
        {
            return await _userManager.UpdateAsync(user);
        }


        public async Task<UserEntity> GetUserAsync(string email)
        {
            return await _context.Users
                .Include(u => u.TypeDocument)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }



        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<UserEntity> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        #endregion


        #region Roles
        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new RoleEntity
                {
                    Name = roleName,
                    IsActive = true
                });
            }

        }
        public async Task<RoleEntity> GetRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
        public async Task RemoveRoleAsync(RoleEntity role)
        {
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        #endregion

        #region Cuenta
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            //Bloqueo en true
            return await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion


        public async Task AddUserToRoleAsync(UserEntity user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task RemoveUserToRoleAsync(UserEntity user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(UserEntity user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetUserRolesAsync(UserEntity user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<UserEntity>> GetUsersAsync()
        {
            return await _userManager.Users
                .Include(t => t.TypeDocument)
                .Include(t => t.Company)
                .ToListAsync();
        }

        public async Task<List<RoleEntity>> GetRoles()
        {
            return await _roleManager.Roles
                .ToListAsync();
        }

        public IEnumerable<SelectListItem> GetRolesCombo()
        {
            List<SelectListItem> list = _roleManager.Roles
                .Where(r => r.IsActive == true)
                .Select(t =>
              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .ToList();
            return list;
        }



    }
}
