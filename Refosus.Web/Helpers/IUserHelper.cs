using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IUserHelper
    {
        #region Usuarios
        Task<List<UserEntity>> GetUsersAsync();


        Task<IdentityResult> AddUserAsync(UserEntity user, string password);
        Task<UserEntity> GetUserAsync(string email);
        Task<UserEntity> GetUserAsync(Guid userId);
        Task<UserEntity> GetUserByIdAsync(string id);
        Task<IdentityResult> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword);
        Task<IdentityResult> ChangePasswordAllUserAsync(UserEntity user, string token, string newPassword);
        Task<string> GenerateTokenChangePasswordAllAsync(UserEntity user);
        Task<IdentityResult>  UpdateUserAsync(UserEntity user);
        #endregion

        #region Cuenta





        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);
        Task<IList<string>> GetUserRolesAsync(UserEntity user);
        Task AddUserToRoleAsync(UserEntity user, string roleName);
        Task RemoveUserToRoleAsync(UserEntity user, string roleName);
        #endregion

        #region Roles
        Task<List<RoleEntity>> GetRoles();
        IEnumerable<SelectListItem> GetRolesCombo();
        Task CheckRoleAsync(string roleName);
        Task<RoleEntity> GetRoleByIdAsync(string id);
        Task RemoveRoleAsync(RoleEntity role);
        #endregion


    }
}
