using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Exporting;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus
{
    [AbpAuthorize(AppPermissions.Pages_ProjectStatus)]
    public class ProjectStatusAppService : AbpZeroTemplateAppServiceBase, IProjectStatusAppService
    {
        private readonly IRepository<ProjectStatu, Guid> _projectStatuRepository;
        private readonly IProjectStatusExcelExporter _projectStatusExcelExporter;

        public ProjectStatusAppService(IRepository<ProjectStatu, Guid> projectStatuRepository, IProjectStatusExcelExporter projectStatusExcelExporter)
        {
            _projectStatuRepository = projectStatuRepository;
            _projectStatusExcelExporter = projectStatusExcelExporter;

        }

        public async Task<PagedResultDto<GetProjectStatuForViewDto>> GetAll(GetAllProjectStatusInput input)
        {

            var filteredProjectStatus = _projectStatuRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameStatus.Contains(input.Filter) || e.SumaryStatus.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameStatusFilter), e => e.NameStatus == input.NameStatusFilter);

            var pagedAndFilteredProjectStatus = filteredProjectStatus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var projectStatus = from o in pagedAndFilteredProjectStatus
                                select new
                                {

                                    o.NameStatus,
                                    o.SumaryStatus,
                                    Id = o.Id
                                };

            var totalCount = await filteredProjectStatus.CountAsync();

            var dbList = await projectStatus.ToListAsync();
            var results = new List<GetProjectStatuForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProjectStatuForViewDto()
                {
                    ProjectStatu = new ProjectStatuDto
                    {

                        NameStatus = o.NameStatus,
                        SumaryStatus = o.SumaryStatus,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProjectStatuForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProjectStatuForViewDto> GetProjectStatuForView(Guid id)
        {
            var projectStatu = await _projectStatuRepository.GetAsync(id);

            var output = new GetProjectStatuForViewDto { ProjectStatu = ObjectMapper.Map<ProjectStatuDto>(projectStatu) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProjectStatus_Edit)]
        public async Task<GetProjectStatuForEditOutput> GetProjectStatuForEdit(EntityDto<Guid> input)
        {
            var projectStatu = await _projectStatuRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProjectStatuForEditOutput { ProjectStatu = ObjectMapper.Map<CreateOrEditProjectStatuDto>(projectStatu) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProjectStatuDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ProjectStatus_Create)]
        protected virtual async Task Create(CreateOrEditProjectStatuDto input)
        {
            var projectStatu = ObjectMapper.Map<ProjectStatu>(input);

            if (AbpSession.TenantId != null)
            {
                projectStatu.TenantId = (int?)AbpSession.TenantId;
            }

            await _projectStatuRepository.InsertAsync(projectStatu);

        }

        [AbpAuthorize(AppPermissions.Pages_ProjectStatus_Edit)]
        protected virtual async Task Update(CreateOrEditProjectStatuDto input)
        {
            var projectStatu = await _projectStatuRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, projectStatu);

        }

        [AbpAuthorize(AppPermissions.Pages_ProjectStatus_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _projectStatuRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProjectStatusToExcel(GetAllProjectStatusForExcelInput input)
        {

            var filteredProjectStatus = _projectStatuRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameStatus.Contains(input.Filter) || e.SumaryStatus.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameStatusFilter), e => e.NameStatus == input.NameStatusFilter);

            var query = (from o in filteredProjectStatus
                         select new GetProjectStatuForViewDto()
                         {
                             ProjectStatu = new ProjectStatuDto
                             {
                                 NameStatus = o.NameStatus,
                                 SumaryStatus = o.SumaryStatus,
                                 Id = o.Id
                             }
                         });

            var projectStatuListDtos = await query.ToListAsync();

            return _projectStatusExcelExporter.ExportToFile(projectStatuListDtos);
        }

    }
}