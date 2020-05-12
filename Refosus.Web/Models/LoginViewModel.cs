using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Display (Name ="Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MinLength(6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }
    }
}
