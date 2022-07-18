using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class GetProjectForEditOutput
    {
        public CreateOrEditProjectDto Project { get; set; }

        public string ProjectStatuNameStatus { get; set; }

        public string ProjectIndustrieNameIndustries { get; set; }

        public string LogoFileName { get; set; }

    }
}