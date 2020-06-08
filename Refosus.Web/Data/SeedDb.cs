using Refosus.Common.Enum;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper
            )
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckMessageBillState();
            await CheckMessageTypes();
            await CheckMessageState();
            await CheckUserAsync("1010", "Administrador", "Refosus", "didneyn@refocosta.com", "3133366284", "Refocosta Principal");
            await CheckUserAsync("1010", "Usuario", "Refosus", "didneyn@gmail.com", "3133366284", "Refocosta Principal");

            await CheckCompaniesAsync();
            await CheckCountriesAsync();
            await CheckMenusAsync();
            await CheckMenusRoleAsync();
        }
        private async Task CheckMenusAsync()
        {
            if (!_context.Menus.Any())
            {
                await AddMenuAsync("Principal", "", "", 0);
                await AddMenuAsync("Parametros", "", "", 1);
                await AddMenuAsync("Seguridad", "", "", 1);
                await AddMenuAsync("Mensajeria", "", "", 1);
                await AddMenuAsync("Noticias", "Home", "IndexNews", 1);

                await AddMenuAsync("Compañias", "Companies", "Index", 2);
                await AddMenuAsync("Paises", "Countries", "Index", 2);

                await AddMenuAsync("Menus", "Menus", "Index", 3);
                await AddMenuAsync("Roles", "Account", "IndexRoles", 3);
                await AddMenuAsync("Usuarios", "Account", "IndexUsers", 3);

                await AddMenuAsync("Mensajes", "Messages", "Index", 4);
                await AddMenuAsync("Mis Mensajes", "Messages", "IndexMe", 4);
            }
        }

        private async Task AddMenuAsync(string name, string controller, string action, int id)
        {
            MenuEntity menu = _context.Menus.FirstOrDefault(o => o.Id == id);
            _context.Menus.Add(new MenuEntity { Name = name, Controller = controller, Action = action, LogoPath = $"~/Images/Menus/{name}.jpg", Menu = menu, IsActive = true });
            await _context.SaveChangesAsync();
        }


        private async Task CheckMenusRoleAsync()
        {
            if (!_context.RoleMenus.Any())
            {
                AddMenusRole();
                await _context.SaveChangesAsync();
            }
        }
        private void AddMenusRole()
        {
            List<MenuEntity> menus = _context.Menus.ToList();
            RoleEntity role = _context.Roles.FirstOrDefault(o => o.Name == "Administrador");
            foreach (MenuEntity item in menus)
            {
                _context.RoleMenus.Add(new RoleMenuEntity { Menu = item, Role = role });
            }
        }
        private async Task CheckMessageTypes()
        {
            if (!_context.MessagesTypes.Any())
            {
                foreach (string type in Enum.GetNames(typeof(MessageType)))
                {
                    _context.MessagesTypes.Add(
                        new MessageTypeEntity
                        {
                            Name = type,
                            Active = true
                        }
                        );
                }
            }
        }
        private async Task CheckMessageState()
        {
            if (!_context.MessagesStates.Any())
            {
                foreach (string state in Enum.GetNames(typeof(MessageState)))
                {
                    _context.MessagesStates.Add(
                        new MessageStateEntity
                        {
                            Name = state,
                            Active = true
                        }
                        );
                }
            }
        }
        private async Task CheckMessageBillState()
        {
            if (!_context.MessagesBillState.Any())
            {
                foreach (string state in Enum.GetNames(typeof(MessageBillState)))
                {
                    _context.MessagesBillState.Add(
                        new MessageBillStateEntity
                        {
                            Name = state,
                            Active = true
                        }
                        );
                }
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                DateTime createDate = DateTime.Today.ToUniversalTime();
                _context.Countries.Add(
                    new CountryEntity
                    {
                        Name = "Colombia",
                        IsActive = true,
                        Departments = new List<DepartmentEntity>
                {
                    new DepartmentEntity{
                        Name="Cundinamarca",
                        IsActive = true,
                        Cities =new List<CityEntity>
                        {
                            new CityEntity
                            {
                                Name="Bogota",
                                IsActive = true,
                                Campus =new List<CampusEntity>
                                {
                                    new CampusEntity
                                    {
                                        Name="Principal",
                                        IsActive = true,
                                        Address=null,
                                        CreateDate=createDate,
                                        CampusDetails= new List<CampusDetailsEntity>
                                        {
                                            new CampusDetailsEntity
                                            {
                                                Company=_context.Companies.FirstOrDefault()
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }
                }
                    });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckCompaniesAsync()
        {
            if (!_context.Companies.Any())
            {
                AddCompany("Refocosta", "4A00");
                AddCompany("Refomass", null);
                AddCompany("Refoenergy", null);
                AddCompany("Refopanel", null);
                AddCompany("Canalclima", null);
                await _context.SaveChangesAsync();
            }
        }
        private void AddCompany(string name, string code)
        {
            _context.Companies.Add(new CompanyEntity { Name = name, LogoPath = $"~/Images/Companies/{name}.jpg", Code = code, IsActive = true });
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Administrador.ToString());
            await _userHelper.CheckRoleAsync(UserType.Usuario.ToString());
        }

        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address)
        {
            UserEntity user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Campus = _context.Campus.FirstOrDefault(),
                    Company = _context.Companies.FirstOrDefault(),
                    IsActive = true
                };
                await _userHelper.AddUserAsync(user, "123456789");
                await _userHelper.AddUserToRoleAsync(user, "Administrador");
            }
            return user;
        }

    }
}
