using System;
using System.Collections.Generic;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class EstadoHitos
    {
        public EstadoHitos()
        {
            Hitos = new HashSet<Hitos>();
        }

        public int IdEstadoHito { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Hitos> Hitos { get; set; }
    }
}
