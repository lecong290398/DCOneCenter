using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Projects
{
    public class CreateOrEditProjectModalViewModel
    {
        public CreateOrEditProjectDto Project { get; set; }

        public string ProjectStatuNameStatus { get; set; }

        public string ProjectIndustrieNameIndustries { get; set; }

        public List<ProjectProjectStatuLookupTableDto> ProjectProjectStatuList { get; set; }

        public List<ProjectProjectIndustrieLookupTableDto> ProjectProjectIndustrieList { get; set; }

        public string LogoFileName { get; set; }
        public string LogoFileAcceptedTypes { get; set; }

        public bool IsEditMode => Project.Id.HasValue;
    }
}