using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Contraseña Actual")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe contener entre {2} y {1} caracteres.")]
        public string OldPassword { get; set; }

        [Display(Name = "Contraseña nueva")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe contener entre {2} y {1} caracteres.")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirme la Contraseña")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="La contraseña debe ser igual.")]
        public string Confirm { get; set; }
    }
}
