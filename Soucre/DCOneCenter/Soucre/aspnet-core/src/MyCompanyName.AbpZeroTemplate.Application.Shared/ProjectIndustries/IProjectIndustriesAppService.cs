using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries
{
    public interface IProjectIndustriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProjectIndustrieForViewDto>> GetAll(GetAllProjectIndustriesInput input);

        Task<GetProjectIndustrieForViewDto> GetProjectIndustrieForView(Guid id);

        Task<GetProjectIndustrieForEditOutput> GetProjectIndustrieForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProjectIndustrieDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProjectIndustriesToExcel(GetAllProjectIndustriesForExcelInput input);

        Task RemoveLogoFile(EntityDto<Guid> input);

    }
}