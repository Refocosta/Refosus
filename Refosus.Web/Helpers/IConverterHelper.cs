using Refosus.Web.Data.Entities;
using Refosus.Web.Data.EntitiesTE;
using Refosus.Web.Models;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IConverterHelper
    {
        #region Companies
        CompanyEntity ToCompanyEntity(CompanyViewModel model, string path, bool isNew);
        CompanyViewModel ToCompanyViewModel(CompanyEntity teamEntity);
        #endregion

        #region Departments
        Task<DepartmentEntity> ToDepartmentEntityAsync(DepartmentViewModel model, bool isNew);
        DepartmentViewModel ToDepartmentViewModel(DepartmentEntity departmentEntity);
        #endregion

        #region Cities
        Task<CityEntity> ToCityEntityAsync(CityViewModel model, bool isNew);
        CityViewModel ToCityViewModel(CityEntity cityEntity);
        #endregion

        #region Campus
        Task<CampusEntity> ToCampusEntityAsync(CampusViewModel model, bool isNew);
        CampusViewModel ToCampusViewModel(CampusEntity campusEntity);
        #endregion

        #region CampusDetaills
        Task<CampusDetailsEntity> ToCampusDetailsEntityAsync(CampusDetailsViewModel model, bool isNew);
        CampusDetailsViewModel ToCampusDetailsViewModel(CampusDetailsEntity campusDetailsEntity);
        #endregion

        #region Menus
        Task<MenuEntity> ToMenuEntityAsync(MenuViewModel model, string path, bool isNew);
        MenuViewModel ToMenuViewModel(MenuEntity menuEntity);
        #endregion

        #region RoleMenu
        Task<RoleMenuEntity> ToRoleMenuEntityAsync(RoleMenusViewModel model, bool isNew);
        RoleMenusViewModel ToRoleMenuViewModel(RoleMenuEntity roleMenuEntity);
        #endregion

        #region News
        NewEntity ToNewEntity(NewViewModel model, string path, bool isNew);
        NewViewModel ToNewViewModel(NewEntity teamEntity);
        #endregion

        #region Message
        Task<MessageEntity> ToMessageEntityAsync(MessageViewModel model, bool isNew);
        Task<MessagetransactionEntity> ToMessageTransactionEntityAsync(MessageViewModel model);
        MessageViewModel ToMessageViewModel(MessageEntity messagentity);
        MessageViewModel ToMessageViewModelNone(MessageEntity messagentity);
        MessageViewModel ToMessageViewModelAut(MessageEntity messagentity);

        #endregion

        #region Users
        Task<UserEntity> ToUserEntityAsync(UserViewModel model, bool isNew);
        UserViewModel ToUserViewModel(UserEntity model);
        UserChangeViewModel ToUserChangeViewModelAsync(UserEntity user);
        #endregion
        //#region Iniciativas
        //IniciativasEntity ToIniciativasEntity(WorkStreamViewModel model, bool isNew);
        //IniciativasViewModel ToIniciativasViewModel(IniciativasEntity modelEntity);

        //#endregion
        #region Shopping
        ShoppingEntity ToShoppingEntity(ShoppingViewModel model, bool isNew);
        ShoppingViewModel ToShoppingViewModel(ShoppingEntity model);
        #endregion
    }
}
