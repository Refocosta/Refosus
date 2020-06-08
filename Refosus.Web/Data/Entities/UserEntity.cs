using Microsoft.AspNetCore.Identity;
using Refosus.Common.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El Campo {0} No puede tener mas de {1} Caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El Campo {0} No puede tener mas de {1} Caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El Campo {0} No puede tener mas de {1} Caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Direccion")]
        [MaxLength(500, ErrorMessage = "El Campo {0} No puede tener mas de {1} Caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public string PicturePath { get; set; }

        [Display(Name = "Foto")]
        public string PictureFullPath { get; set; }

        [Display(Name = "Sede")]
        public CampusEntity Campus { get; set; }

        [Display(Name = "Compañia")]
        public CompanyEntity Company { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        [Display(Name = "Activo?")]
        public bool IsActive { get; set; }

        
    }
}
