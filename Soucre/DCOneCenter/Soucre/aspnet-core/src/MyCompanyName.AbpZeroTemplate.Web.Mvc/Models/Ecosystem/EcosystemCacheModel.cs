using Abp.Web.Models;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.Web.Models.Ecosystem
{
    public class EcosystemCacheModel : ErrorInfo
    {

        public List<GetAllEcosystemProjectDto> DataProjectsIndustres { get; set; }

        public EcosystemCacheModel(List<GetAllEcosystemProjectDto> dataProjectsIndustres)
        {
            DataProjectsIndustres = new List<GetAllEcosystemProjectDto>(dataProjectsIndustres);
        }

        public EcosystemCacheModel(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}
