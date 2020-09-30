using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingMeasureEntity
    {
        public int Id { get; set; }

        [Display(Name = "Medida")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Unidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ShoppingUnitEntity Unit { get; set; }

    }
}
