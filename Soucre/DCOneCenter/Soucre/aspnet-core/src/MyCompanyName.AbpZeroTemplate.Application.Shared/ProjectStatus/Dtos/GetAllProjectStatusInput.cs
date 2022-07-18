using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos
{
    public class GetAllProjectStatusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameStatusFilter { get; set; }

    }
}