using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace Refosus.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCampus()
        {
            List<SelectListItem> list = _context.Campus.Where(t => t.IsActive == true).Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"

              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciones una sede]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboCompany()
        {
            List<SelectListItem> list = _context.Companies.Where(t => t.IsActive == true).Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una Compañia]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetComboMenus()
        {
            List<SelectListItem> list = _context.Menus.Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Menu]",
                Value = "0"
            });
            return list;
        }
    }
}
