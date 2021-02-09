using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class TM_User_GroupEntity
    {
        public int Id { get; set; }
        [Display(Name = "Usuario")]
        public UserEntity User { get; set; }
        [Display(Name = "Grupo")]
        public TP_GroupEntity Group { get; set; }
    }
}
