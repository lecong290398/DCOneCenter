using Abp.Web.Models;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.Web.Models.ProductIndustres
{
    public class ProjectIndustresIndexCacheModel : ErrorInfo
    {

        public List<GetProjectIndustrieForViewDto> DataProjectsIndustres { get; set; }

        public ProjectIndustresIndexCacheModel(List<GetProjectIndustrieForViewDto> dataProjectsIndustres)
        {
            DataProjectsIndustres = new List<GetProjectIndustrieForViewDto>(dataProjectsIndustres);
        }

        public ProjectIndustresIndexCacheModel(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}
