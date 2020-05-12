using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Data.Entities
{
    public class MenuEntity
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Controlador")]
        public string Controller { get; set; }

        [Display(Name = "Acción")]
        public string Action { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        [Display(Name = "Activa?")]
        public bool IsActive { get; set; }

        [Display(Name = "Dependencia")]
        public MenuEntity Menu { get; set; }

        public ICollection<RoleMenuEntity> roleMenus { get; set; }
    }
}
