using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class NewEntity
    {
        public int Id { get; set; }
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Title { get; set; }

        [Display(Name = "Tamaño")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Size { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Colour { get; set; }

        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        [Display(Name = "Publico")]
        public bool Public { get; set; }
    }
}
