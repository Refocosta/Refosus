using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context,ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        #region Companies
        public CompanyEntity ToCompanyEntity(CompanyViewModel model, string path, bool isNew)
        {
            return new CompanyEntity
            {
                Id = isNew ? 0 : model.Id,
                LogoPath = path,
                Name = model.Name,
                Code = model.Code
            };
        }

        public CompanyViewModel ToCompanyViewModel(CompanyEntity companyEntity)
        {
            return new CompanyViewModel
            {
                Id = companyEntity.Id,
                LogoPath = companyEntity.LogoPath,
                Name = companyEntity.Name,
                Code = companyEntity.Code
            };
        }
        #endregion

        #region Departments
        public async Task<DepartmentEntity> ToDepartmentEntityAsync(DepartmentViewModel model, bool isNew)
        {
            return new DepartmentEntity
            {
                Cities = model.Cities,
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                Country = await _context.Countries.FindAsync(model.CountryId)
            };
        }

        public DepartmentViewModel ToDepartmentViewModel(DepartmentEntity departmentEntity)
        {
            return new DepartmentViewModel
            {
                Cities = departmentEntity.Cities,
                Id = departmentEntity.Id,
                Name = departmentEntity.Name,
                IsActive = departmentEntity.IsActive,
                Country = departmentEntity.Country,
                CountryId = departmentEntity.Country.Id
            };
        }
        #endregion

        #region Cities
        public async Task<CityEntity> ToCityEntityAsync(CityViewModel model, bool isNew)
        {
            return new CityEntity
            {
                Campus = model.Campus,
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                Department = await _context.Deparments.FindAsync(model.DepartmentId)
            };
        }

        public CityViewModel ToCityViewModel(CityEntity cityEntity)
        {
            return new CityViewModel
            {
                Campus = cityEntity.Campus,
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                IsActive = cityEntity.IsActive,
                Department = cityEntity.Department,
                DepartmentId = cityEntity.Department.Id
            };
        }
        #endregion

        #region Campus
        public async Task<CampusEntity> ToCampusEntityAsync(CampusViewModel model, bool isNew)
        {
            return new CampusEntity
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Address = model.Address,
                IsActive = model.IsActive,
                CreateDate = model.CreateDateLocal.ToUniversalTime(),
                City = await _context.Cities.FindAsync(model.CityId),
                CampusDetails = model.CampusDetails
            };
        }

        public CampusViewModel ToCampusViewModel(CampusEntity campusEntity)
        {
            return new CampusViewModel
            {
                Id = campusEntity.Id,
                Name = campusEntity.Name,
                Address = campusEntity.Address,
                IsActive = campusEntity.IsActive,
                CreateDate = campusEntity.CreateDate,
                City = campusEntity.City,
                CityId = campusEntity.City.Id,

            };
        }
        #endregion

        #region CampusDetaills
        public async Task<CampusDetailsEntity> ToCampusDetailsEntityAsync(CampusDetailsViewModel model, bool isNew)
        {
            return new CampusDetailsEntity
            {
                Id = isNew ? 0 : model.Id,
                Campus = await _context.Campus.FindAsync(model.CampusId),
                Company = await _context.Companies.FindAsync(model.CompanyId)
            };

        }

        public CampusDetailsViewModel ToCampusDetailsViewModel(CampusDetailsEntity campusDetailsEntity)
        {
            return new CampusDetailsViewModel
            {
                Id = campusDetailsEntity.Id,
                Campus = campusDetailsEntity.Campus,
                CampusId = campusDetailsEntity.Campus.Id,
                Company = campusDetailsEntity.Company,
                CompanyId = campusDetailsEntity.Company.Id,
                Companies = _combosHelper.GetComboCompany()
            };
        }
        #endregion

        #region Menus
        public async Task<MenuEntity> ToMenuEntityAsync(MenuViewModel model, string path, bool isNew)
        {
            if (model.Menus == null)
            {
                return new MenuEntity
                {
                    Id = isNew ? 0 : model.Id,
                    Name = model.Name,
                    Controller = model.Controller,
                    Action = model.Action,
                    LogoPath = path,
                    IsActive = model.IsActive,
                    Menu = await _context.Menus.FindAsync(model.MenuId)
                };
            }
            else
            {
                return new MenuEntity
                {
                    Id = isNew ? 0 : model.Id,
                    Name = model.Name,
                    Controller = model.Controller,
                    Action = model.Action,
                    LogoPath = path,
                    IsActive = model.IsActive,
                    Menu = null
                };
            }

        }

        public MenuViewModel ToMenuViewModel(MenuEntity menuEntity)
        {
            MenuViewModel model = new MenuViewModel();

            if (menuEntity.Menu == null)
            {
                return new MenuViewModel
                {
                    Id = menuEntity.Id,
                    Name = menuEntity.Name,
                    Controller = menuEntity.Controller,
                    Action = menuEntity.Action,
                    IsActive = menuEntity.IsActive,
                    LogoPath = menuEntity.LogoPath,
                    Menus = _combosHelper.GetComboMenus()
                };
            }
            else
            {
                return new MenuViewModel
                {
                    Id = menuEntity.Id,
                    Name = menuEntity.Name,
                    Controller = menuEntity.Controller,
                    Action = menuEntity.Action,
                    IsActive = menuEntity.IsActive,
                    MenuId = menuEntity.Menu.Id,
                    LogoPath = menuEntity.LogoPath,
                    Menus = _combosHelper.GetComboMenus()
                };
            }


        }
        #endregion

        #region RoleMenu
        public async Task<RoleMenuEntity> ToRoleMenuEntityAsync(RoleMenusViewModel model, bool isNew)
        {
            return new RoleMenuEntity
            {
                Id = isNew ? 0 : model.Id,
                Role = await _context.Roles.FindAsync(model.RoleId),
                Menu = await _context.Menus.FindAsync(model.MenuId)
            };
        }

        public RoleMenusViewModel ToRoleMenuViewModel(RoleMenuEntity roleMenuEntity)
        {
            return new RoleMenusViewModel
            {
                Id = roleMenuEntity.Id,
                Role = roleMenuEntity.Role,
                RoleId = roleMenuEntity.Role.Id,
                Menu = roleMenuEntity.Menu,
                MenuId = roleMenuEntity.Menu.Id,
                Menus = _combosHelper.GetComboMenus()
            };
        }
        #endregion

        #region News

        NewEntity IConverterHelper.ToNewEntity(NewViewModel model, string path, bool isNew)
        {
            return new NewEntity
            {
                Id = isNew ? 0 : model.Id,
                LogoPath = path,
                Colour = model.Colour,
                Content = model.Content,
                Size=model.Size,
                Title=model.Title,
                Public=model.Public
            };
        }

        NewViewModel IConverterHelper.ToNewViewModel(NewEntity newEntity)
        {
            return new NewViewModel
            {
                Id = newEntity.Id,
                LogoPath = newEntity.LogoPath,
                Colour = newEntity.Colour,
                Content = newEntity.Content,
                Size = newEntity.Size,
                Title = newEntity.Title,
                Public = newEntity.Public
            };
        }
        #endregion
    }
}
