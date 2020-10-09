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
    public class UserChangeViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tipo de Documento")]
        public DocumentTypeEntity TypeDocument { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Direccion")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string PhoneNumber { get; set; }

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
