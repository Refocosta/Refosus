using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class TP_GroupEntity
    {
        public int? Id { get; set; }
        public CompanyEntity Company { get; set; }
        public string Name { get; set; }
        public bool Stade { get; set; }
        public ICollection<TM_User_GroupEntity> Users { get; set; }
    }
}
