using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class UserChangeViewModel : UserEntity
    {
        [Display(Name = "Compañia")]
        public int CompanyId { get; set; }

        [Display(Name = "Tipo de Documento")]
        public int DocumentTypeId { get; set; }


        [Display(Name = "Foto")]
        public IFormFile PictureFile { get; set; }

        [Display(Name = "Foto")]
        public string PicturePath { get; set; }

        [Display(Name = "Foto")]
        public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
            ? "~/Images/Users/User.jpg"
            : $"~/Images/Users/User.jpg";

        public IEnumerable<SelectListItem> Companies { get; set; }

        public IEnumerable<SelectListItem> DocumentTypes { get; set; }
    }
}
