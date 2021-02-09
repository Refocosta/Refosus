using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class ChangePasswordAllUserViewModel
    {
        

        [Display(Name = "Usuario")]
        [Range(1,int.MaxValue,ErrorMessage ="Debe seleccionar un usuario")]
        public string UserId { get; set; }

        [Display(Name = "Contraseña nueva")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe contener entre {2} y {1} caracteres.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirme la Contraseña")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe contener entre {2} y {1} caracteres.")]
        [Compare("NewPassword")]
        public string Confirm { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
