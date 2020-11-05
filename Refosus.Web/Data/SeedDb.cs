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
            await CheckCompaniesAsync();
            await CheckCountriesAsync();


            await CheckRoles();

            await CheckDocumentTypeAsync();
            await CheckUserAsync("1010", "Administrator", "Nativa", "nativa@refocosta.com", "", "Refocosta Principal");
            await CheckRolesUser();


            #region Message
            await CheckMessageTypesAsync();
            await CheckMessageStateAsync();
            await CheckMessageBillStateAsync();
            #endregion




            await CheckMenusAsync();
            await CheckMenusRoleAsync();
        }
        private async Task CheckDocumentTypeAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                await AddDocumentTypes("Cédula de Ciudadanía", "CC");
            }
        }
        private async Task AddDocumentTypes(string name, string nom)
        {
            _context.DocumentTypes.Add(new DocumentTypeEntity { Name = name, Nom = nom });
            await _context.SaveChangesAsync();
        }

        private async Task CheckMenusAsync()
        {
            if (!_context.Menus.Any())
            {
                await AddMenuAsync("Principal", "", "", 0);


                await AddMenuAsync("Parametros", "", "", 1);
                await AddMenuAsync("Seguridad", "", "", 1);
                await AddMenuAsync("Correspondencia", "", "", 1);
                await AddMenuAsync("Noticias", "Home", "IndexNews", 1);
                await AddMenuAsync("Compras", "Shopping", "Index", 1);
                await AddMenuAsync("Reportes", "", "", 1);
                await AddMenuAsync("Iniciativas TE", "TE", "Index", 1);
                await AddMenuAsync("BSC", "", "", 1);


                await AddMenuAsync("Compañias", "Companies", "Index", 2);
                await AddMenuAsync("Paises", "Countries", "Index", 2);

                await AddMenuAsync("Menus", "Menus", "Index", 3);
                await AddMenuAsync("Roles", "Account", "IndexRoles", 3);
                await AddMenuAsync("Usuarios", "Account", "IndexUsers", 3);

                await AddMenuAsync("Mensajes", "Messages", "Index", 4);
                await AddMenuAsync("Mis Mensajes", "Messages", "IndexMe", 4);
                await AddMenuAsync("Mi Historial", "Messages", "IndexMeHistory", 4);
                await AddMenuAsync("Facturacion Pendiente", "Messages", "IndexBillPending", 4);
                await AddMenuAsync("Historial de Facturación", "Messages", "IndexBillHistory", 4);
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
                await AddMenusRole();
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddMenusRole()
        {
            List<MenuEntity> menus = _context.Menus.ToList();
            RoleEntity role = _context.Roles.FirstOrDefault(o => o.Name == "Administrator");
            foreach (MenuEntity item in menus)
            {
                _context.RoleMenus.Add(new RoleMenuEntity { Menu = item, Role = role });
            }

        }
        #region Message
        private async Task CheckMessageTypesAsync()
        {
            if (!_context.MessagesTypes.Any())
            {
                await AddMessageTypeAsync("Carta", true);
                await AddMessageTypeAsync("Factura", true);
                await AddMessageTypeAsync("Paquete", true);
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddMessageTypeAsync(string name, bool active)
        {
            _context.MessagesTypes.Add(new MessageTypeEntity { Name = name, Active = active });
        }

        private async Task CheckMessageStateAsync()
        {
            if (!_context.MessagesStates.Any())
            {
                await AddMessageStateAsync("Ingresado", true);
                await AddMessageStateAsync("Recibido", true);
                await AddMessageStateAsync("En Transito", true);
                await AddMessageStateAsync("En Proceso", true);
                await AddMessageStateAsync("Tramitado", true);
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddMessageStateAsync(string name, bool active)
        {
            _context.MessagesStates.Add(new MessageStateEntity { Name = name, Active = active });
        }

        private async Task CheckMessageBillStateAsync()
        {
            if (!_context.MessagesBillState.Any())
            {
                await AddMessagesBillStateAsync("Nuevo", true);
                await AddMessagesBillStateAsync("Otro", true);
                await AddMessagesBillStateAsync("Aprobado", true);
                await AddMessagesBillStateAsync("Rechazado", true);
                await AddMessagesBillStateAsync("Procesado", true);
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddMessagesBillStateAsync(string name, bool active)
        {
            _context.MessagesBillState.Add(new MessageBillStateEntity { Name = name, Active = active });
        }
        #endregion
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
                await AddCompany("Refocosta", "4A00");
                await AddCompany("Refomass", null);
                await AddCompany("Refoenergy", null);
                await AddCompany("Refopanel", null);
                await AddCompany("Canalclima", null);
                await _context.SaveChangesAsync();
            }
        }
        private async Task AddCompany(string name, string code)
        {
            _context.Companies.Add(new CompanyEntity { Name = name, LogoPath = $"~/Images/Companies/{name}.jpg", Code = code, IsActive = true });
        }






        private async Task CheckRoles()
        {
            if (!_context.Roles.Any())
            {
                await _userHelper.CheckRoleAsync("Administrator");
                #region Messages
                await _userHelper.CheckRoleAsync("MessageAdministrator");
                await _userHelper.CheckRoleAsync("MessageMeMessage");
                await _userHelper.CheckRoleAsync("MessageMeHistory");
                await _userHelper.CheckRoleAsync("MessageBillPending");
                await _userHelper.CheckRoleAsync("MessageBillHistory");
                await _userHelper.CheckRoleAsync("MessageCreator");
                await _userHelper.CheckRoleAsync("MessageBillProcesator");
                await _userHelper.CheckRoleAsync("MessageBillAutorizador");
                await _userHelper.CheckRoleAsync("MessageBillChecker");
                await _userHelper.CheckRoleAsync("MessageBillChecker");
                #endregion
                #region Shopping

                #endregion

                #region TE
                await _userHelper.CheckRoleAsync("TEAdmininistrator");
                #endregion


            }
        }
        private async Task CheckRolesUser()
        {
            UserEntity user = await _userHelper.GetUserAsync("nativa@refocosta.com");
            int count = _userHelper.GetUserRolesAsync(user).Result.Count;
            if (count == 0)
            {
                await _userHelper.AddUserToRoleAsync(user, "Administrator");
            }
        }
        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address)
        {
            UserEntity user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    TypeDocument = _context.DocumentTypes.FirstOrDefault(),
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    IsActive = true,
                    CreateDate = System.DateTime.Now.ToUniversalTime(),
                    ActiveDate = System.DateTime.Now.ToUniversalTime(),
                    PhotoPath = $"~/Images/Users/{email}.jpg",
                    Company = _context.Companies.FirstOrDefault()
                };
                await _userHelper.AddUserAsync(user, "123456789");
            }
            return user;
        }

    }
}
