using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Models
{
    public class RoleMenusViewModel : RoleMenuEntity
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Menu")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Menu.")]
        public int MenuId { get; set; }

        public IEnumerable<SelectListItem> Menus { get; set; }
    }
}
