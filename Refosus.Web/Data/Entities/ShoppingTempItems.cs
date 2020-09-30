using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingTempItems
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDateLocal => CreateDate.ToLocalTime();

        [Display(Name = "Creado Por")]
        public UserEntity UserCreate { get; set; }

        public string CodSAP { get; set; }
        public ShoppingCategoryEntity Category { get; set; }
        public ShoppingCategoryEntity SubCategory { get; set; }
        public ShoppingUnitEntity Unit { get; set; }
        public ShoppingMeasureEntity Measure { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Refence { get; set; }
        public string Mark { get; set; }
        public string InternalOrder { get; set; }
        public string NumInternalOrder { get; set; }
    }
}
