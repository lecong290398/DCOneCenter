using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos
{
    public class GetAllProjectIndustriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameIndustriesFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}