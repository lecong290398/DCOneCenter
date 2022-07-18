using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.ProjectIndustries
{
    public class CreateOrEditProjectIndustrieModalViewModel
    {
        public CreateOrEditProjectIndustrieDto ProjectIndustrie { get; set; }

        public string LogoFileName { get; set; }
        public string LogoFileAcceptedTypes { get; set; }

        public bool IsEditMode => ProjectIndustrie.Id.HasValue;
    }
}