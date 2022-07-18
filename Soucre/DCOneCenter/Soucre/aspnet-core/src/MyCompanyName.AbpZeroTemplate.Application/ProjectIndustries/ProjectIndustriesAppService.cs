using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Exporting;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.Storage;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries
{
    [AbpAuthorize(AppPermissions.Pages_ProjectIndustries)]
    public class ProjectIndustriesAppService : AbpZeroTemplateAppServiceBase, IProjectIndustriesAppService
    {
        private readonly IRepository<ProjectIndustrie, Guid> _projectIndustrieRepository;
        private readonly IProjectIndustriesExcelExporter _projectIndustriesExcelExporter;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IHostEnvironment _env;

        public ProjectIndustriesAppService(IHostEnvironment env , IRepository<ProjectIndustrie, Guid> projectIndustrieRepository, IProjectIndustriesExcelExporter projectIndustriesExcelExporter, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _env = env;
            _projectIndustrieRepository = projectIndustrieRepository;
            _projectIndustriesExcelExporter = projectIndustriesExcelExporter;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public async Task<PagedResultDto<GetProjectIndustrieForViewDto>> GetAll(GetAllProjectIndustriesInput input)
        {

            var filteredProjectIndustries = _projectIndustrieRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameIndustries.Contains(input.Filter) || e.SumaryIndustries.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameIndustriesFilter), e => e.NameIndustries == input.NameIndustriesFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredProjectIndustries = filteredProjectIndustries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var projectIndustries = from o in pagedAndFilteredProjectIndustries
                                    select new
                                    {

                                        o.NameIndustries,
                                        o.SumaryIndustries,
                                        o.IsActive,
                                        o.Logo,
                                        Id = o.Id
                                    };

            var totalCount = await filteredProjectIndustries.CountAsync();

            var dbList = await projectIndustries.ToListAsync();
            var results = new List<GetProjectIndustrieForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProjectIndustrieForViewDto()
                {
                    ProjectIndustrie = new ProjectIndustrieDto
                    {

                        NameIndustries = o.NameIndustries,
                        SumaryIndustries = o.SumaryIndustries,
                        IsActive = o.IsActive,
                        Logo = o.Logo,
                        Id = o.Id,
                    }
                };
                res.ProjectIndustrie.LogoFileName = o.Logo + ".png";

                results.Add(res);
            }

            return new PagedResultDto<GetProjectIndustrieForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProjectIndustrieForViewDto> GetProjectIndustrieForView(Guid id)
        {
            var projectIndustrie = await _projectIndustrieRepository.GetAsync(id);

            var output = new GetProjectIndustrieForViewDto { ProjectIndustrie = ObjectMapper.Map<ProjectIndustrieDto>(projectIndustrie) };

            output.ProjectIndustrie.LogoFileName = projectIndustrie.Logo + ".png";

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProjectIndustries_Edit)]
        public async Task<GetProjectIndustrieForEditOutput> GetProjectIndustrieForEdit(EntityDto<Guid> input)
        {
            var projectIndustrie = await _projectIndustrieRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProjectIndustrieForEditOutput { ProjectIndustrie = ObjectMapper.Map<CreateOrEditProjectIndustrieDto>(projectIndustrie) };

            output.LogoFileName = projectIndustrie.Logo.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProjectIndustrieDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProjectIndustries_Create)]
        protected virtual async Task Create(CreateOrEditProjectIndustrieDto input)
        {
            var projectIndustrie = ObjectMapper.Map<ProjectIndustrie>(input);

            if (AbpSession.TenantId != null)
            {
                projectIndustrie.TenantId = (int?)AbpSession.TenantId;
            }

            await _projectIndustrieRepository.InsertAsync(projectIndustrie);
            projectIndustrie.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_ProjectIndustries_Edit)]
        protected virtual async Task Update(CreateOrEditProjectIndustrieDto input)
        {
            var projectIndustrie = await _projectIndustrieRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, projectIndustrie);
            projectIndustrie.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_ProjectIndustries_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _projectIndustrieRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProjectIndustriesToExcel(GetAllProjectIndustriesForExcelInput input)
        {

            var filteredProjectIndustries = _projectIndustrieRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameIndustries.Contains(input.Filter) || e.SumaryIndustries.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameIndustriesFilter), e => e.NameIndustries == input.NameIndustriesFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredProjectIndustries
                         select new GetProjectIndustrieForViewDto()
                         {
                             ProjectIndustrie = new ProjectIndustrieDto
                             {
                                 NameIndustries = o.NameIndustries,
                                 SumaryIndustries = o.SumaryIndustries,
                                 IsActive = o.IsActive,
                                 Logo = o.Logo,
                                 Id = o.Id
                             }
                         });

            var projectIndustrieListDtos = await query.ToListAsync();

            return _projectIndustriesExcelExporter.ExportToFile(projectIndustrieListDtos);
        }

        private async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);

            string ext = Path.GetExtension(fileCache.FileName);
            var uniqueFileName = storedFile.Id + ".png";

            var dir = Path.Combine(_env.ContentRootPath, @"wwwroot\view-resources\Views\DCOneCenter\ImagesProjectIndustries");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, uniqueFileName);
            SaveByteArrayToFileWithFileStream(fileCache.File, filePath); // Requires System.IO
            return storedFile.Id;
        }

        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
        }

        private async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_ProjectIndustries_Edit)]
        public async Task RemoveLogoFile(EntityDto<Guid> input)
        {
            var projectIndustrie = await _projectIndustrieRepository.FirstOrDefaultAsync(input.Id);
            if (projectIndustrie == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!projectIndustrie.Logo.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(projectIndustrie.Logo.Value);
            projectIndustrie.Logo = null;
        }

    }
}