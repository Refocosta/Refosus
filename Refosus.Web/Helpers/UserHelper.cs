using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
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

        public UserHelper(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        #region Usuarios
        public async Task<IdentityResult> AddUserAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
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
            return await _userManager.Users.Include(t=>t.TypeDocument).ToListAsync();
        }

        public Task<List<RoleEntity>> GetRoles()
        {
            throw new System.NotImplementedException();
        }
    }
}
