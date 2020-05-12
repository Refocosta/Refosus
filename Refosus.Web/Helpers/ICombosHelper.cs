using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Refosus.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboCampus();
        IEnumerable<SelectListItem> GetComboCompany();
        IEnumerable<SelectListItem> GetComboMenus();
    }
}
