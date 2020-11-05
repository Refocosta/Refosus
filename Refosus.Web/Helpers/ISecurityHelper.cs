using Refosus.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface ISecurityHelper
    {
        Task<List<RoleMenuEntity>> GetMenusRoleAsync(UserEntity userEntity);
        Task<List<RoleEntity>> GetRoleByUserAsync(UserEntity userEntity);

    }
}
