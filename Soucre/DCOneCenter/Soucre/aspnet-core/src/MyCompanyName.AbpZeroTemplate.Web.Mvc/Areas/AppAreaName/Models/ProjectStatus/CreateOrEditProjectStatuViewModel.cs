using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;

using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.ProjectStatus
{
    public class CreateOrEditProjectStatuModalViewModel
    {
        public CreateOrEditProjectStatuDto ProjectStatu { get; set; }

        public bool IsEditMode => ProjectStatu.Id.HasValue;
    }
}