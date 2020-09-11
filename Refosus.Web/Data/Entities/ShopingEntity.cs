using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ShopingEntity
    {
        [Display(Name = "Consecutivo")]
        public int Id { get; set; }

        [Display(Name = "Creado Por")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public UserEntity UserCreate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDateLocal => CreateDate.ToLocalTime();

        [Display(Name = "Asignado A")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public UserEntity UserAssigned { get; set; }

        [Display(Name = "Articulos")]
        public IEnumerable<ShoppingItemsEntity> Items { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDateLocal => UpdateDate.ToLocalTime();

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ShoppingStateEntity State { get; set; }
    }
}
