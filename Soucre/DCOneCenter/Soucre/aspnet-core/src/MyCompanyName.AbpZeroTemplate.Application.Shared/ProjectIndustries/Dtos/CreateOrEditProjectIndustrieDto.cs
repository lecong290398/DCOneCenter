using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos
{
    public class CreateOrEditProjectIndustrieDto : EntityDto<Guid?>
    {

        [Required]
        public string NameIndustries { get; set; }

        public string SumaryIndustries { get; set; }

        public bool IsActive { get; set; }

        public Guid? Logo { get; set; }

        public string LogoToken { get; set; }

    }
}