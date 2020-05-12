using Microsoft.AspNetCore.Identity;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IUserHelper
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(UserEntity user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(UserEntity user, string roleName);
        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);
        Task<IList<string>> GetUserRolesAsync(UserEntity user);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
