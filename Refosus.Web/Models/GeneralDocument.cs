using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refosus.Web.Models
{
    [Table("GeneralDocuments")]
    public class GeneralDocument
    {
        [Key]
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Ext { get; set; }
        public IFormFile File { get; set; }
        public int Status { get; set; }
        public int GeneralDocumentsCategoriesId { get; set; }
    }
}
