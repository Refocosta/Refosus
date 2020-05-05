using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class CampusDetailsEntity
    {
        public int Id { get; set; }
        public CampusEntity Campus { get; set; }
        public CompanyEntity Company { get; set; }

    }
}
