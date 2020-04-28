using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public CompanyEntity ToCompanyEntity(CompanyViewModel model, string path, bool isNew)
        {
            return new CompanyEntity
            {
                Id = isNew ? 0 : model.Id,
                LogoPath = path,
                Name = model.Name,
                Code = model.Code
            };
        }

        public CompanyViewModel ToCompanyViewModel(CompanyEntity companyEntity)
        {
            return new CompanyViewModel
            {
                Id = companyEntity.Id,
                LogoPath = companyEntity.LogoPath,
                Name = companyEntity.Name,
                Code = companyEntity.Code
            };
        }
    }
}
