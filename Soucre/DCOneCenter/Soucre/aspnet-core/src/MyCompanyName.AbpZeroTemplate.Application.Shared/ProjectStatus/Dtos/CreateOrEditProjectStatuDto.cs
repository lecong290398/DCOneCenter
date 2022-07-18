using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos
{
    public class CreateOrEditProjectStatuDto : EntityDto<Guid?>
    {

        [Required]
        public string NameStatus { get; set; }

        public string SumaryStatus { get; set; }

    }
}