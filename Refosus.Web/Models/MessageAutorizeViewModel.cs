using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class MessageAutorizeViewModel : MessageEntity
    {
        [Display(Name = "Autorizar")]
        public bool Auto { get; set; }
        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }
        [Display(Name = "Compañia")]
        public IEnumerable<SelectListItem> Companies { get; set; }
        [Display(Name = "Usuario")]
        public string UserId { get; set; }
        [Display(Name = "Usuario")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
