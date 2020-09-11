using System;
using System.Collections.Generic;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class IniciativasEntity
    {
        public IniciativasEntity()
        {
            Hitos = new HashSet<Hitos>();
        }

        public int IdIniciativa { get; set; }
        public string NombreIniciativa { get; set; }
        public string DescripIniciativa { get; set; }
        public string Etapa { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdWorkStream { get; set; }
        public string TipoIniciativa { get; set; }
        public decimal? BeneficiosRecurrente { get; set; }
        public decimal? BeneficiosNoRecurrentes { get; set; }
        public decimal? CostoImplementación { get; set; }
        public DateTime? FechaL2 { get; set; }
        public DateTime? FechaL3 { get; set; }
        public DateTime? FechaL4 { get; set; }
        public DateTime? FechaL5 { get; set; }
        public string Medicion { get; set; }
        public string RangoBenefRecurrentes { get; set; }
        public string Unidad { get; set; }
        public string SupuestosImpacto { get; set; }
        public string Observacion { get; set; }

        public virtual Responsables IdResponsableNavigation { get; set; }
        public virtual WorkStreamEntity IdWorkStreamNavigation { get; set; }
        public virtual ICollection<Hitos> Hitos { get; set; }
    }
}
