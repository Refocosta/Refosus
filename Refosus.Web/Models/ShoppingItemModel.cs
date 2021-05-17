using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class ShoppingTempItemModel : ShoppingTempItems
    {
        public int IdShoping { get; set; }
        public int IdCategory { get; set; }
        public int IdSubCategory { get; set; }
        public int IdUnit { get; set; }
        public int IdMeasure { get; set; }
        public int IdState { get; set; }
        public IEnumerable<SelectListItem> states { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> SubCategories { get; set; }
        public IEnumerable<SelectListItem> Unities { get; set; }
        public IEnumerable<SelectListItem> Measures { get; set; }
    }
}
