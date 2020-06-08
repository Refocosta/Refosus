using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class UserViewModel : UserEntity
    {
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Confirmar Contraseña")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
    }
}
