using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Refosus.Web.Data.Entities
{
    [Table("GeneralDocuments")]
    public class GeneralDocumentEntity
    {
        public int Id { get; set; }
        [Display(Name = "Alias")]
        public string Alias { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Archivo")]
        public string Path { get; set; }
        [Display(Name = "Ext")]
        public string Ext { get; set; }
        [Display(Name = "Estado")]
        public int Status { get; set; }
        [Display(Name = "Categoria documento general")]
        public int GeneralDocumentsCategoriesId { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de importe")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateAt { get; set; }

    }
}
