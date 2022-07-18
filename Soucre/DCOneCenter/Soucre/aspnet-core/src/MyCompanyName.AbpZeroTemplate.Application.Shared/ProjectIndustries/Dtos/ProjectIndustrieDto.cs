using System;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos
{
    public class ProjectIndustrieDto : EntityDto<Guid>
    {
        public string NameIndustries { get; set; }

        public string SumaryIndustries { get; set; }

        public bool IsActive { get; set; }

        public Guid? Logo { get; set; }

        public string LogoFileName { get; set; }

    }
}