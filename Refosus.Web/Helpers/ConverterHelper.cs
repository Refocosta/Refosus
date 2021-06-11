using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using Refosus.Web.Data.Entities;
using Refosus.Web.Data.EntitiesTE;
using Refosus.Web.Models;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper, IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
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
                Size = model.Size,
                Title = model.Title,
                Public = model.Public
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

        #region Message
        public async Task<MessageEntity> ToMessageEntityAsync(MessageViewModel model, bool isNew)
        {
            return new MessageEntity
            {
                Id = isNew ? 0 : model.Id,
                Company = await _context.Companies.FindAsync(model.CompanyId),
                Type = await _context.MessagesTypes.FindAsync(model.TypeId),
                Sender = model.Sender,
                Reference = model.Reference,
                CreateDate = model.CreateDateLocal.ToUniversalTime(),
                UpdateDate = model.UpdateDateLocal.ToUniversalTime(),
                UserCreate = await _context.Users.FindAsync(model.CreateUser),
                UserSender = await _context.Users.FindAsync(model.UserTrn),
                User = await _context.Users.FindAsync(model.UserRec),
                State = await _context.MessagesStates.FindAsync(model.StateId),
                StateBill = await _context.MessagesBillState.FindAsync(model.StateBillId),
                Ceco = await _context.CeCos.Include(c => c.UserResponsible).FirstOrDefaultAsync(c => c.Id == model.CecoId),
                UserAut = await _context.Users.FindAsync(model.AutUser),
                DateAut = model.DateAutLocal.ToUniversalTime(),
                UserPros = await _context.Users.FindAsync(model.ProUser),
                DateProcess = model.DateProcessLocal.ToUniversalTime(),
                NumberBill = model.NumberBill,
                Value = model.Value,
                Observation = model.Observation
            };
        }
        public MessageViewModel ToMessageViewModel(MessageEntity messagentity)
        {
            MessageViewModel model = new MessageViewModel
            {
                Id = messagentity.Id,
                Company = messagentity.Company,
                CompanyId = messagentity.Company.Id,
                Type = messagentity.Type,
                TypeId = messagentity.Type.Id,
                Sender = messagentity.Sender,
                Reference = messagentity.Reference,
                CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                UserCreate = messagentity.UserCreate,
                CreateUser = messagentity.UserCreate.Id,
                UserSender = messagentity.UserSender,
                UserTrn = messagentity.UserSender.Id,
                User = messagentity.User,
                UserRec = messagentity.User.Id,
                State = messagentity.State,
                StateId = messagentity.State.Id,
                StateBill = messagentity.StateBill,
                StateBillId = messagentity.StateBill.Id,
                NumberBill = messagentity.NumberBill,
                Value = messagentity.Value,
                Observation = messagentity.Observation
            };
            if (messagentity.Ceco != null)
            {
                model.Ceco = messagentity.Ceco;
                model.CecoId = messagentity.Ceco.Id;
            }
            if (messagentity.UserAut != null)
            {
                model.UserAut = messagentity.UserAut;
                model.AutUser = messagentity.UserAut.Id;
                model.DateAut = messagentity.DateAutLocal.ToUniversalTime();
            }
            if (messagentity.UserPros != null)
            {
                model.UserPros = messagentity.UserPros;
                model.ProUser = messagentity.UserPros.Id;
                model.DateProcess = messagentity.DateProcessLocal.ToUniversalTime();
            }
            model.MessageType = _combosHelper.GetComboMessageType();
            model.MessageState = _combosHelper.GetComboMessageState();
            model.Users = _combosHelper.GetComboUser();
            model.MessageBillState = _combosHelper.GetComboMessageBillState();
            model.Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id);
            model.Companies = _combosHelper.GetComboCompany();
            return model;
        }
        public async Task<MessagetransactionEntity> ToMessageTransactionEntityAsync(MessageViewModel model)
        {
            return new MessagetransactionEntity
            {
                Id = 0,
                UserCreate = model.UserSender,
                UserUpdate = model.User,
                UpdateDate = model.UpdateDateLocal.ToUniversalTime(),
                StateCreate = model.Transaction.StateCreate,
                StateUpdate = model.State,
                Observation = model.Transaction.Observation
            };
        }
        public MessageAutorizeViewModel ToMessageAutorizeViewModel(MessageEntity messagentity)
        {
            MessageAutorizeViewModel model = new MessageAutorizeViewModel
            {
                Id = messagentity.Id,
                Company = messagentity.Company,
                Type = messagentity.Type,
                Sender = messagentity.Sender,
                Reference = messagentity.Reference,
                CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                UserCreate = messagentity.UserCreate,
                UserSender = messagentity.UserSender,
                User = messagentity.User,
                State = messagentity.State,
                StateBill = messagentity.StateBill,
                NumberBill = messagentity.NumberBill,
                Ceco = messagentity.Ceco,
                UserAut = messagentity.UserAut,
                DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                UserPros = messagentity.UserPros,
                DateProcess = messagentity.DateProcessLocal.ToUniversalTime(),
                Value = messagentity.Value,
                Observation = messagentity.Observation
            };
            return model;
        }
        #endregion

        #region Users
        public async Task<UserEntity> ToUserEntityAsync(UserViewModel model, bool isNew)
        {
            return new UserEntity
            {
                TypeDocument = await _context.DocumentTypes.FindAsync(model.DocumentTypeId),
                Document = model.Document,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = model.IsActive,
                Company = await _context.Companies.FindAsync(model.CompanyId),
                CreateDate = System.DateTime.Now.ToUniversalTime(),
                ActiveDate = System.DateTime.Now.ToUniversalTime(),
                PhotoPath = model.PhotoPath
            };
        }
        public UserViewModel ToUserViewModel(UserEntity model)
        {
            return new UserViewModel
            {
                DocumentTypes = _combosHelper.GetComboDocumentType(),
                Companies = _combosHelper.GetComboCompany(),
                Id = model.Id,
                UserName = model.UserName,
                TypeDocument = model.TypeDocument,
                DocumentTypeId = model.TypeDocument.Id,
                Document = model.Document,
                CompanyId = model.Company.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = model.IsActive,
                Email = model.Email
            };
        }


        public UserChangeViewModel ToUserChangeViewModelAsync(UserEntity user)
        {
            return new UserChangeViewModel
            {
                Companies = _combosHelper.GetComboCompany(),
                DocumentTypes = _combosHelper.GetComboDocumentType(),
                PhotoPath = user.PhotoPath,
                DocumentTypeId = user.TypeDocument.Id,
                Document = user.Document,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                CompanyId = user.Company.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }



        #endregion

        #region Shopping
        public async Task<ShoppingEntity> ToShoppingEntityAsync(ShoppingViewModel model, bool isNew)
        {
            return new ShoppingEntity
            {
                Id = isNew ? 0 : model.Id,
                Company = await _context.Companies.SingleOrDefaultAsync(p => p.Id == model.IdCompany),
                CreateGroup = await _context.TP_Groups.FirstOrDefaultAsync(p => p.Id == model.IdGroupCreate),
                UserCreate = await _context.Users.FirstOrDefaultAsync(p => p.Id == model.IdUserCreate),

                UserAssigned = await _userHelper.GetUserByIdAsync(model.IdUserAssign),
                AssignedGroup = await _context.TP_Groups.FirstOrDefaultAsync(p => p.Id == model.IdGroupAssigned),

                State = await _context.ShoppingStates.FirstOrDefaultAsync(p => p.Id == model.IdState),

                CreateDate = model.CreateDateLocal.ToUniversalTime(),
                UpdateDate = model.UpdateDateLocal.ToUniversalTime(),

                Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.IdProject),
                UserProjectBoss = await _context.Users.FirstOrDefaultAsync(p => p.Id == model.IdUserProjectBoss),





                observations = model.observations,
                TotalValue = model.TotalValue
            };
        }

        public async Task<ShoppingViewModel> ToShoppingViewModelAsync(ShoppingEntity model)
        {
            ShoppingViewModel modelView = new ShoppingViewModel();
            modelView.Id = model.Id;
            modelView.IdUserCreate = model.UserCreate.Id;
            modelView.UserCreate = model.UserCreate;
            modelView.CreateDate = model.CreateDate;
            modelView.UpdateDate = model.UpdateDate;
            if (model.Company != null)
            {
                modelView.Company = model.Company;
                modelView.IdCompany = model.Company.Id;
            }
            if (model.UserAssigned != null)
            {
                modelView.UserAssigned = model.UserAssigned;
                modelView.IdUserAssign = model.UserAssigned.Id;
            }
            else
            {
                modelView.UserAssigned = null;
                modelView.IdUserAssign = null;
            }
            modelView.State = await _context.ShoppingStates.FirstOrDefaultAsync(s => s.Id == model.State.Id);
            modelView.IdState = model.State.Id;

            if (model.AssignedGroup != null)
            {
                modelView.AssignedGroup = model.AssignedGroup;
                modelView.IdGroupAssigned = (int)model.AssignedGroup.Id;
            }
            if (model.CreateGroup != null)
            {
                modelView.CreateGroup = model.CreateGroup;
                modelView.IdGroupCreate = (int)model.CreateGroup.Id;
            }
            if (model.Project != null)
            {
                modelView.Project = await _context.Projects.FirstOrDefaultAsync(s => s.Id == model.Project.Id);
                modelView.IdProject = model.Project.Id;
                modelView.IdUserProjectBoss = model.UserProjectBoss.Id;
                modelView.UserProjectBoss = model.UserProjectBoss;
            }

            modelView.ShoppingUnits = _combosHelper.GetComboShoppingUnit();
            modelView.ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(0);
            modelView.Categories = _combosHelper.GetComboShoppingCategory();
            modelView.SubCategories = _combosHelper.GetComboShoppingCategory(0);
            modelView.Users = _combosHelper.GetComboUser();
            modelView.ShoppingStates = _combosHelper.GetComboShoppingState();
            modelView.Projects = _combosHelper.GetComboProject();
            modelView.Groups = _combosHelper.GetGroups();
            modelView.TotalValue = model.TotalValue;
            modelView.observations = model.observations;
            return modelView;
        }
        #endregion

        //#region Iniciativas
        ////public IniciativasEntity ToIniciativasEntity(WorkStreamViewModel model, bool isNew)
        ////{
        ////    throw new System.NotImplementedException();
        ////}

        ////public IniciativasViewModel ToIniciativasViewModel(IniciativasEntity modelEntity)
        ////{
        ////    return new MenuEntity
        ////    {
        ////        Id = isNew ? 0 : model.Id,
        ////        Name = model.Name,
        ////        Controller = model.Controller,
        ////        Action = model.Action,
        ////        LogoPath = path,
        ////        IsActive = model.IsActive,
        ////        Menu = await _context.Menus.FindAsync(model.MenuId)
        ////    };
        ////    return model;
        ////}
        //#endregion
    }
}
