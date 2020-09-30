using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string Name{get;set;}

        [DisplayName("Jefe del Proyecto")]
        public string IdUserProjectBoss { get; set; }

    }
}
