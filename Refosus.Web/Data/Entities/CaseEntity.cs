using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refosus.Web.Data.Entities
{
    [Table("Cases")]
    public class CaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "Tema")]
        public string Issue { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Remitente")]
        public string Sender { get; set; }
        [Display(Name = "TipoCaso")]
        public int TypesCasesId { get; set; }
        [Display(Name = "Estado")]
        public int Status { get; set; }
        [Display(Name = "Prioridad")]
        public int Priority { get; set; }
        [Display(Name = "Codigo")]
        public string Code { get; set; }
        [Display(Name = "Solucion")]
        public string Solution { get; set; }
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }
        [Display(Name = "Cumplimiento")]
        public int Fulfillment { get; set; }
        [Display(Name = "Horas")]
        public double Hours { get; set; }
        [Display(Name = "TipoUnidadNegocio")]
        public int BusinessUnitsId { get; set; }
        [Display(Name = "Ubicacion")]
        public string Ubication { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime DeadLine { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime ClosingDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreatedAt { get; set; }


    }
}
