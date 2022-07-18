using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus
{
    public interface IProjectStatusAppService : IApplicationService
    {
        Task<PagedResultDto<GetProjectStatuForViewDto>> GetAll(GetAllProjectStatusInput input);

        Task<GetProjectStatuForViewDto> GetProjectStatuForView(Guid id);

        Task<GetProjectStatuForEditOutput> GetProjectStatuForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProjectStatuDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProjectStatusToExcel(GetAllProjectStatusForExcelInput input);

    }
}