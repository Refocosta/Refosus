﻿using Microsoft.AspNetCore.Http;
using Refosus.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Refosus.Web.Models
{
    public class CompanyViewModel : CompanyEntity
    {
        [Display(Name = "Logo")]
        public IFormFile LogoFile { get; set; }
        public string Logo { get; internal set; }
    }
}
