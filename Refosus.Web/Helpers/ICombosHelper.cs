using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Refosus.Web.Helpers
{
    public interface ICombosHelper
    {
        #region Parameters
        IEnumerable<SelectListItem> GetComboProject();
        #endregion
        IEnumerable<SelectListItem> GetComboCampus();
        IEnumerable<SelectListItem> GetComboCompany();
        IEnumerable<SelectListItem> GetComboMenus();
        IEnumerable<SelectListItem> GetComboMessageType();
        IEnumerable<SelectListItem> GetComboMessageState();
        IEnumerable<SelectListItem> GetComboMessageBillState();
        IEnumerable<SelectListItem> GetComboUser();
        IEnumerable<SelectListItem> GetComboUserActive();
        IEnumerable<SelectListItem> GetComboCeCo(int comp);
        IEnumerable<SelectListItem> GetComboDocumentType();
        IEnumerable<SelectListItem> GetGroupsUser(string user);
        IEnumerable<SelectListItem> GetGroupsActive();
        IEnumerable<SelectListItem> GetGroups();
        #region Shopping
        IEnumerable<SelectListItem> GetComboShoppingCategory();
        IEnumerable<SelectListItem> GetComboShoppingCategory(int sub);
        IEnumerable<SelectListItem> GetComboShoppingUnit();
        IEnumerable<SelectListItem> GetComboShoppingMeasure(int uni);
        IEnumerable<SelectListItem> GetComboShoppingState();



        #endregion
    }
}
