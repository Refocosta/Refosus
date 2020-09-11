using System;
using System.Collections.Generic;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class Responsables
    {
        public Responsables()
        {
            Iniciativas = new HashSet<IniciativasEntity>();
            WorkStream = new HashSet<WorkStreamEntity>();
        }

        public int IdResponsable { get; set; }
        public string Nombre { get; set; }
        public string IdRol { get; set; }

        public virtual ICollection<IniciativasEntity> Iniciativas { get; set; }
        public virtual ICollection<WorkStreamEntity> WorkStream { get; set; }
    }
}
