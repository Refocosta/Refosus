using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refosus.Web.Models
{
    [Table("TypesCases")]
    public class TypeCase
    {
        [Key]
        public int Id { get; set; }
        public string Case { get; set; }
        public int Status { get; set; }
    }
}
