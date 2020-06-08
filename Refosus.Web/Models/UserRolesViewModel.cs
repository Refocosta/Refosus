using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class UserRolesViewModel
    {
        public UserEntity user { get; set; }
        public List<RoleEntity> roles { get; set; }
    }
}
