using System;
using System.Collections.Generic;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class Hitos
    {
        public int IdHito { get; set; }
        public string NombreHito { get; set; }
        public string DescripcionHito { get; set; }
        public int IdResponsable { get; set; }
        public string Tipo { get; set; }
        public int IdEstado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? IdIniciativa { get; set; }

        public virtual EstadoHitos IdEstadoNavigation { get; set; }
        public virtual IniciativasEntity IdIniciativaNavigation { get; set; }
    }
}
