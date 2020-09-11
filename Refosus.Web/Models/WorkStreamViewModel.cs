using Refosus.Web.Data.EntitiesTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class WorkStreamViewModel : WorkStreamEntity
    {
        public IEnumerable<IniciativasEntity> Iniciativas { get; set; }
    }
}
