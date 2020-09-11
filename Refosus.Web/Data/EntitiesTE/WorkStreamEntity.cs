using System;
using System.Collections.Generic;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class WorkStreamEntity
    {
        public WorkStreamEntity()
        {
            Iniciativas = new HashSet<IniciativasEntity>();
        }

        public int IdWorkStream { get; set; }
        public string WorkStream1 { get; set; }
        public string Descripcion { get; set; }
        public int? IdResponsable { get; set; }
        public string Colour { get; set; }
        public virtual Responsables IdResponsableNavigation { get; set; }
        public ICollection<IniciativasEntity> Iniciativas { get; set; }
    }
}
