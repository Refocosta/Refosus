using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class UserPhotoInfoModel
    {
        [Display(Name = "Fotografia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhotoPath { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MinLength(6)]
        [Display(Name = "Usuario")]
        public string Name { get; set; }
    }
}
