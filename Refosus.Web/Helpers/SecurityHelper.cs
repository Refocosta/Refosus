using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        public SecurityHelper(IUserHelper userHelper,
            DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }
        public async Task<List<RoleMenuEntity>> GetMenusRoleAsync(UserEntity userEntity)
        {
            if (userEntity != null)
            {
                Task<IList<string>> roles =  _userHelper.GetUserRolesAsync(userEntity);
                if (roles != null)
                {
                    
                    
                        List<RoleMenuEntity> Salida = new List<RoleMenuEntity>();
                        for (int i = 0; i < roles.Result.Count; i++)
                        {
                            if (i == 0)
                            {
                                Salida = (_context
                            .RoleMenus
                            .Include(t => t.Menu)
                            .ThenInclude(t => t.Menu)
                            .Include(t => t.Role)
                            .Where(t => t.Role.Name == roles.Result[i])
                            .Where(t => t.Menu.IsActive == true)
                            .Where(t => t.Menu.Id != 1)
                            .OrderBy(t => t.Menu.Menu.Id)
                            .ToList());
                            }
                            else
                            {
                                List<RoleMenuEntity> temp = (_context
                            .RoleMenus
                            .Include(t => t.Menu)
                            .ThenInclude(t => t.Menu)
                            .Include(t => t.Role)
                            .Where(t => t.Role.Name == roles.Result[i])
                            .Where(t => t.Menu.IsActive == true)
                            .Where(t => t.Menu.Id != 1)
                            .OrderBy(t => t.Menu.Menu.Id)
                            .ToList());
                                Salida.AddRange(temp);
                            }
                        }
                        List<RoleMenuEntity> listAgrupada = new List<RoleMenuEntity>();
                        List<int> menu = new List<int>();
                        for (int i = 0; i < Salida.Count; i++)
                        {
                            int count = 0;
                            for (int j = 0; j < Salida.Count; j++)
                            {
                                if (Salida[i].Menu.Id == Salida[j].Menu.Id && i != j)
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                listAgrupada.Add(Salida[i]);
                            }
                            else
                            {
                                if (!menu.Contains(Salida[i].Menu.Id))
                                {
                                    listAgrupada.Add(Salida[i]);
                                    menu.Add(Salida[i].Menu.Id);
                                }
                                count = 0;
                            }
                        }
                        listAgrupada = listAgrupada.OrderBy(o => o.Menu.Name).ToList();
                        listAgrupada = listAgrupada.OrderBy(o => o.Menu.Menu.Id).ToList();
                        return listAgrupada;
                    

                }
            }
            return null;
        }
    }
}
