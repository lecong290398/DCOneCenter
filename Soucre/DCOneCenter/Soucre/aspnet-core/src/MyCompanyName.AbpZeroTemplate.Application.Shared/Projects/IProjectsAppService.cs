using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.Projects
{
    public interface IProjectsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input);

        Task<GetProjectForViewDto> GetProjectForView(Guid id);

        Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProjectDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input);

        Task<List<ProjectProjectStatuLookupTableDto>> GetAllProjectStatuForTableDropdown();

        Task<List<ProjectProjectIndustrieLookupTableDto>> GetAllProjectIndustrieForTableDropdown();

        Task RemoveLogoFile(EntityDto<Guid> input);

    }
}