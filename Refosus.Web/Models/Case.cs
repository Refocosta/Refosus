using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Refosus.Web.Data.Entities;

namespace Refosus.Web.Models
{
    [Table("Cases")]
    public class Case
    {
        [Key]
        public int Id { get; set; }
        public string Issue { get; set; }
        public string Description { get; set; }
        public string Sender { get; set; }
        public int TypesCasesId { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public string Code { get; set; }
        public string Responsable { get; set; }
        public int BusinessUnitsId { get; set; }
        public string Ubication { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
