using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos
{
    public class GetProjectStatuForEditOutput
    {
        public CreateOrEditProjectStatuDto ProjectStatu { get; set; }

    }
}