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

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
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
            //if (model.CecoId != null && model.Type.Name == "Factura")
            //{
                return new MessageEntity
                {
                    Id = isNew ? 0 : model.Id,
                    Type = await _context.MessagesTypes.FindAsync(model.TypeId),
                    Sender = model.Sender,
                    Reference = model.Reference,
                    CreateDate = model.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = model.UpdateDateLocal.ToUniversalTime(),
                    State = await _context.MessagesStates.FindAsync(model.StateId),
                    User = await _context.Users.FindAsync(model.CreateUser),
                    StateBill = await _context.MessagesBillState.FindAsync(model.StateBillId),
                    UserSender = await _context.Users.FindAsync(model.SenderUser),
                    UserAut = await _context.Users.FindAsync(model.AutUser),
                    UserPros = await _context.Users.FindAsync(model.ProUser),
                    UserCreate = await _context.Users.FindAsync(model.CreateUser),
                    DateAut = model.DateAutLocal.ToUniversalTime(),
                    DateProcess = model.DateProcessLocal.ToUniversalTime(),
                    Ceco = await _context.CeCos.FindAsync(model.CecoId),
                    NumberBill = model.NumberBill,
                    Company = await _context.Companies.FindAsync(model.CompanyId),
                };
            //}
            //else
            //{
            //    return new MessageEntity
            //    {
            //        Id = isNew ? 0 : model.Id,
            //        Type = await _context.MessagesTypes.FindAsync(model.TypeId),
            //        Sender = model.Sender,
            //        Reference = model.Reference,
            //        CreateDate = model.CreateDate.ToUniversalTime(),
            //        UpdateDate = model.UpdateDate.ToUniversalTime(),
            //        State = await _context.MessagesStates.FindAsync(model.StateId),
            //        User = await _context.Users.FindAsync(model.CreateUser),
            //        StateBill = await _context.MessagesBillState.FindAsync(model.StateBillId),
            //        UserSender = await _context.Users.FindAsync(model.SenderUser),

            //        Ceco = await _context.CeCos.FindAsync(model.CecoId),
            //        NumberBill = model.NumberBill
            //    };
            //}
        }
        public async Task<MessagetransactionEntity> ToMessageTransactionEntityAsync(MessageViewModel model)
        {
            return new MessagetransactionEntity
            {
                Id = 0,
                Message = model,
                UserCreate = model.User,
                UserUpdate = model.User,
                UpdateDate = model.Transaction.UpdateDateLocal.ToUniversalTime(),
                StateCreate = model.Transaction.StateCreate,
                StateUpdate = model.State,
                Observation = model.Transaction.Observation
            };
        }

        public MessageViewModel ToMessageViewModel(MessageEntity messagentity)
        {
            if (messagentity.Ceco == null)
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies=_combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    AutUser = messagentity.UserAut.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    ProUser = messagentity.UserPros.Id,
                    DateProcess = messagentity.DateProcessLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId=messagentity.Company.Id
                };
            }
            else
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies = _combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Ceco = messagentity.Ceco,
                    CecoId = messagentity.Ceco.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    AutUser = messagentity.UserAut.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    ProUser = messagentity.UserPros.Id,
                    DateProcess = messagentity.DateProcessLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId = messagentity.Company.Id
                };
            }
        }
        public MessageViewModel ToMessageViewModelNone(MessageEntity messagentity)
        {
            if (messagentity.Ceco == null)
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies = _combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    DateProcess = messagentity.DateProcessLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId = messagentity.Company.Id
                };
            }
            else
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies = _combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Ceco = messagentity.Ceco,
                    CecoId = messagentity.Ceco.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    DateProcess = messagentity.DateProcessLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId = messagentity.Company.Id
                };
            }
        }
        public MessageViewModel ToMessageViewModelAut(MessageEntity messagentity)
        {
            if (messagentity.Ceco == null)
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies = _combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    AutUser = messagentity.UserAut.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId = messagentity.Company.Id
                };
            }
            else
            {
                return new MessageViewModel
                {
                    MessageType = _combosHelper.GetComboMessageType(),
                    MessageState = _combosHelper.GetComboMessageState(),
                    Users = _combosHelper.GetComboUser(),
                    MessageBillState = _combosHelper.GetComboMessageBillState(),
                    Cecos = _combosHelper.GetComboCeCo(messagentity.Company.Id),
                    Companies = _combosHelper.GetComboCompany(),
                    Id = messagentity.Id,
                    Type = messagentity.Type,
                    TypeId = messagentity.Type.Id,
                    Ceco = messagentity.Ceco,
                    CecoId = messagentity.Ceco.Id,
                    Sender = messagentity.Sender,
                    Reference = messagentity.Reference,
                    CreateDate = messagentity.CreateDateLocal.ToUniversalTime(),
                    UpdateDate = messagentity.UpdateDateLocal.ToUniversalTime(),
                    State = messagentity.State,
                    StateId = messagentity.State.Id,
                    StateBill = messagentity.StateBill,
                    StateBillId = messagentity.StateBill.Id,
                    User = messagentity.User,
                    CreateUser = messagentity.User.Id,
                    AutUser = messagentity.UserAut.Id,
                    DateAut = messagentity.DateAutLocal.ToUniversalTime(),
                    UserSender = messagentity.UserSender,
                    SenderUser = messagentity.UserSender.Id,
                    NumberBill = messagentity.NumberBill,
                    CompanyId = messagentity.Company.Id
                };
            }
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
                Id = model.Id,
                UserName = model.UserName,

                TypeDocument = model.TypeDocument,
                DocumentTypeId = model.TypeDocument.Id,
                Document = model.Document,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = model.IsActive,
                Email = model.Email
            };
        }






        #endregion

        #region Shopping
        public ShoppingEntity ToShoppingEntity(ShoppingViewModel model, bool isNew)
        {
            return new ShoppingEntity
            {
                Id = isNew ? 0 : model.Id
            };
        }

        public ShoppingViewModel ToShoppingViewModel(ShoppingEntity model)
        {
            return new ShoppingViewModel
            {
                Id = model.Id,


                ShoppingUnits = _combosHelper.GetComboShoppingUnit(),
                ShoppingMeasures = _combosHelper.GetComboShoppingMeasure(0),
                Categories = _combosHelper.GetComboShoppingCategory(),
                SubCategories = _combosHelper.GetComboShoppingCategory(0),
                Users = _combosHelper.GetComboUser(),
                ShoppingStates = _combosHelper.GetComboShoppingState(),
                Projects = _combosHelper.GetComboProject()
            };
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
