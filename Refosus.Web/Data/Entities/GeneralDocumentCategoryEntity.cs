using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refosus.Web.Data.Entities
{
    [Table("GeneralDocumentsCategories")]
    public class GeneralDocumentCategoryEntity
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
    }
}
