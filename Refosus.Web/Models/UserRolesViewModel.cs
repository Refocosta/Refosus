using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class UserRolesViewModel
    {
        public UserEntity User { get; set; }
        public RoleEntity Rol { get; set; }
        [DisplayName("Usuario ")]
        public string userId { get; set; }
        [DisplayName("Rol")]
        public string rolesId { get; set; }
        [DisplayName("Usuarios")]
        public IEnumerable<UserEntity> Users { get; set; }
        [DisplayName("Roles")]
        public IEnumerable<RoleEntity> Roles { get; set; }
        [DisplayName("Usuarios")]
        public IEnumerable<SelectListItem> ListUsers { get; set; }
        [DisplayName("Roles")]
        public IEnumerable<SelectListItem> ListRoles { get; set; }
    }
}
