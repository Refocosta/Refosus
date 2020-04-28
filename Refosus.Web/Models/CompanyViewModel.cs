using Microsoft.AspNetCore.Http;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class CompanyViewModel : CompanyEntity
    {
    [Display(Name ="Logo")]
    public IFormFile LogoFile { get; set; }
        public string Logo { get; internal set; }
    }
}
