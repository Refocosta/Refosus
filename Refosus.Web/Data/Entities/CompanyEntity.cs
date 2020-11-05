using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Data.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        [Display(Name = "Codigo")]
        public string Code { get; set; }

        [Display(Name = "Activa?")]
        public bool IsActive { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}
