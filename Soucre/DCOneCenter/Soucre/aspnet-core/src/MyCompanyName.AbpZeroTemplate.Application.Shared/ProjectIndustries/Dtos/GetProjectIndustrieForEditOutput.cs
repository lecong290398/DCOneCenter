using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos
{
    public class GetProjectIndustrieForEditOutput
    {
        public CreateOrEditProjectIndustrieDto ProjectIndustrie { get; set; }

        public string LogoFileName { get; set; }

    }
}