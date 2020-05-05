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
        Task<DepartmentEntity> ToDepartmentEntityAsync(DepartmentViewModel model, bool isNew);
        DepartmentViewModel ToDepartmentViewModel(DepartmentEntity departmentEntity);
        Task<CityEntity> ToCityEntityAsync(CityViewModel model, bool isNew);
        CityViewModel ToCityViewModel(CityEntity cityEntity);
        Task<CampusEntity> ToCampusEntityAsync(CampusViewModel model, bool isNew);
        CampusViewModel ToCampusViewModel(CampusEntity campusEntity);

        Task<CampusDetailsEntity> ToCampusDetailsEntityAsync(CampusDetailsViewModel model, bool isNew);
        CampusDetailsViewModel ToCampusDetailsViewModel(CampusDetailsEntity campusDetailsEntity);

    }
}
