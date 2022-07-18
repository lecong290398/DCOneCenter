using Abp.Application.Services.Dto;
using System;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class GetAllProjectsForExcelInput
    {
        public string Filter { get; set; }

        public string ProjectNameFilter { get; set; }

        public string TokenShortnameFilter { get; set; }

        public string TotalTokenSupplyFilter { get; set; }

        public string ReleaseYearFilter { get; set; }

        public int? IsPormotedFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string ProjectStatuNameStatusFilter { get; set; }

        public string ProjectIndustrieNameIndustriesFilter { get; set; }

    }
}