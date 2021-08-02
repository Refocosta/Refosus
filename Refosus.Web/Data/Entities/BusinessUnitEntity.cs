using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refosus.Web.Data.Entities
{
    [Table("BusinessUnits")]
    public class BusinessUnitEntity
    {
        public int Id { get; set; }
        [Display(Name = "Negocio")]
        public string Business { get; set; }
        [Display(Name = "Estado")]
        public int Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateAt { get; set; }
    }
}
