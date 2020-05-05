using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Models
{
    public class CampusDetailsViewModel : CampusDetailsEntity
    {
        public int CampusId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Compañia")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una compañia.")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
    }
}
