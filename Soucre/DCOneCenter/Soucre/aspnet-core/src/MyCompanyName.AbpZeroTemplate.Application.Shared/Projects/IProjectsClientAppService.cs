using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.Projects
{
    public interface IProjectsClientAppService : IApplicationService
    {

        Task<ProjectIndexOutput> GetDataProjectIndex();
        Task<List<GetAllEcosystemProjectDto>> GetDataEcosystemProject();

    }
}