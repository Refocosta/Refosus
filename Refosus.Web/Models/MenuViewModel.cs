using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Models
{
    public class MenuViewModel : MenuEntity
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Menu")]
        [Range(0, int.MaxValue, ErrorMessage = "Error en el rango de Menus")]
        public int MenuId { get; set; }

        public IEnumerable<SelectListItem> Menus { get; set; }

        [Display(Name = "Logo")]
        public IFormFile LogoFile { get; set; }
        public string Logo { get; internal set; }
    }
}
