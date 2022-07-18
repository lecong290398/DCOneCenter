using System;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos
{
    public class ProjectStatuDto : EntityDto<Guid>
    {
        public string NameStatus { get; set; }

        public string SumaryStatus { get; set; }

    }
}