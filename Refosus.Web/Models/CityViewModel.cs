using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class CityViewModel : CityEntity
    {
        public int DepartmentId { get; set; }
    }
}
