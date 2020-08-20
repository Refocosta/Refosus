using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "Tipo de Documento")]
        public DocumentTypeEntity TypeDocument { get; set; }

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

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Creacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Creacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime CreateDateLocal => CreateDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Creacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime ActiveDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Creacion")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime ActiveDateLocal => ActiveDate.ToLocalTime();

        [Display(Name = "Foto")]
        public string PhotoPath { get; set; }

        [Display(Name = "Compañia")]
        public CompanyEntity Company { get; set; }

        [Display(Name = "Activo?")]
        public bool IsActive { get; set; }

        [Display(Name = "Direccion")]
        [MaxLength(500, ErrorMessage = "El Campo {0} No puede tener mas de {1} Caracteres.")]
        public string Address { get; set; }
    }
}
