using Refosus.Web.Data.Entities;
using Refosus.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Helpers
{
    public interface IConverterHelper
    {
        CompanyEntity ToCompanyEntity(CompanyViewModel model, string path, bool isNew);
        CompanyViewModel ToCompanyViewModel(CompanyEntity teamEntity);
    }
}
