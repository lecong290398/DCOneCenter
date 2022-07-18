using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries
{
    public interface IProjectIndustriesClientAppService : IApplicationService
    {

        Task<List<GetProjectIndustrieForViewDto>> GetDataProjectIndustresIndex();
    }
}