using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class RoleEntity : IdentityRole
    {
        [Display(Name = "Activo?")]
        public bool IsActive { get; set; }
        public ICollection<RoleMenuEntity> roleMenus { get; set; }
    }
}
