using Microsoft.AspNetCore.Identity;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IUserHelper
    {
        #region Usuarios
        Task<IdentityResult> AddUserAsync(UserEntity user, string password);
        Task<UserEntity> GetUserByEmailAsync(string email);
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
        Task< List<RoleEntity>> GetRoles();
        Task CheckRoleAsync(string roleName);
        Task<RoleEntity> GetRoleByIdAsync(string id);
        Task RemoveRoleAsync(RoleEntity role);
        #endregion


    }
}
