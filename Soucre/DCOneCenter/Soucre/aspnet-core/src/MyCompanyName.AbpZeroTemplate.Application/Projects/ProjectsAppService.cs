using MyCompanyName.AbpZeroTemplate.ProjectStatus;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Projects.Exporting;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
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

namespace MyCompanyName.AbpZeroTemplate.Projects
{
    [AbpAuthorize(AppPermissions.Pages_Projects)]
    public class ProjectsAppService : AbpZeroTemplateAppServiceBase, IProjectsAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IProjectsExcelExporter _projectsExcelExporter;
        private readonly IRepository<ProjectStatu, Guid> _lookup_projectStatuRepository;
        private readonly IRepository<ProjectIndustrie, Guid> _lookup_projectIndustrieRepository;
        private readonly IHostEnvironment _env;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public ProjectsAppService(IRepository<Project, Guid> projectRepository, IProjectsExcelExporter projectsExcelExporter
            , IRepository<ProjectStatu, Guid> lookup_projectStatuRepository, IRepository<ProjectIndustrie
            , Guid> lookup_projectIndustrieRepository
            , ITempFileCacheManager tempFileCacheManager
            , IBinaryObjectManager binaryObjectManager
            , IHostEnvironment env
            )
        {
            _env = env;
            _projectRepository = projectRepository;
            _projectsExcelExporter = projectsExcelExporter;
            _lookup_projectStatuRepository = lookup_projectStatuRepository;
            _lookup_projectIndustrieRepository = lookup_projectIndustrieRepository;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
        {

            var filteredProjects = _projectRepository.GetAll()
                        .Include(e => e.ProjectStatuFk)
                        .Include(e => e.ProjectIndustrieFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProjectName.Contains(input.Filter) || e.TokenShortname.Contains(input.Filter) || e.TotalTokenSupply.Contains(input.Filter) || e.ReleaseYear.Contains(input.Filter) || e.ProjectSummary.Contains(input.Filter) || e.ProjectDescription.Contains(input.Filter) || e.WebsiteURL.Contains(input.Filter) || e.Whitepaper_URL_FAQ.Contains(input.Filter) || e.TwitterURL.Contains(input.Filter) || e.Discord.Contains(input.Filter) || e.Reddit.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.Telegram.Contains(input.Filter) || e.YourName.Contains(input.Filter) || e.YourEmailaddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectName == input.ProjectNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TokenShortnameFilter), e => e.TokenShortname == input.TokenShortnameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalTokenSupplyFilter), e => e.TotalTokenSupply == input.TotalTokenSupplyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReleaseYearFilter), e => e.ReleaseYear == input.ReleaseYearFilter)
                        .WhereIf(input.IsPormotedFilter.HasValue && input.IsPormotedFilter > -1, e => (input.IsPormotedFilter == 1 && e.IsPormoted) || (input.IsPormotedFilter == 0 && !e.IsPormoted))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectStatuNameStatusFilter), e => e.ProjectStatuFk != null && e.ProjectStatuFk.NameStatus == input.ProjectStatuNameStatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectIndustrieNameIndustriesFilter), e => e.ProjectIndustrieFk != null && e.ProjectIndustrieFk.NameIndustries == input.ProjectIndustrieNameIndustriesFilter);

            var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var projects = from o in pagedAndFilteredProjects
                           join o1 in _lookup_projectStatuRepository.GetAll() on o.ProjectStatuId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_projectIndustrieRepository.GetAll() on o.ProjectIndustrieId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {

                               o.ProjectName,
                               o.TokenShortname,
                               o.TotalTokenSupply,
                               o.ReleaseYear,
                               o.ProjectSummary,
                               o.ProjectDescription,
                               o.WebsiteURL,
                               o.Whitepaper_URL_FAQ,
                               o.TwitterURL,
                               o.Discord,
                               o.Reddit,
                               o.Facebook,
                               o.Telegram,
                               o.YourName,
                               o.YourEmailaddress,
                               o.IsPormoted,
                               o.IsActive,
                               o.Logo,
                               Id = o.Id,
                               ProjectStatuNameStatus = s1 == null || s1.NameStatus == null ? "" : s1.NameStatus.ToString(),
                               ProjectIndustrieNameIndustries = s2 == null || s2.NameIndustries == null ? "" : s2.NameIndustries.ToString()
                           };

            var totalCount = await filteredProjects.CountAsync();

            var dbList = await projects.ToListAsync();
            var results = new List<GetProjectForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProjectForViewDto()
                {
                    Project = new ProjectDto
                    {

                        ProjectName = o.ProjectName,
                        TokenShortname = o.TokenShortname,
                        TotalTokenSupply = o.TotalTokenSupply,
                        ReleaseYear = o.ReleaseYear,
                        ProjectSummary = o.ProjectSummary,
                        ProjectDescription = o.ProjectDescription,
                        WebsiteURL = o.WebsiteURL,
                        Whitepaper_URL_FAQ = o.Whitepaper_URL_FAQ,
                        TwitterURL = o.TwitterURL,
                        Discord = o.Discord,
                        Reddit = o.Reddit,
                        Facebook = o.Facebook,
                        Telegram = o.Telegram,
                        YourName = o.YourName,
                        YourEmailaddress = o.YourEmailaddress,
                        IsPormoted = o.IsPormoted,
                        IsActive = o.IsActive,
                        Logo = o.Logo,
                        Id = o.Id,
                    },
                    ProjectStatuNameStatus = o.ProjectStatuNameStatus,
                    ProjectIndustrieNameIndustries = o.ProjectIndustrieNameIndustries
                };
                res.Project.LogoFileName = o.Logo + ".png";

                results.Add(res);
            }

            return new PagedResultDto<GetProjectForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProjectForViewDto> GetProjectForView(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            var output = new GetProjectForViewDto { Project = ObjectMapper.Map<ProjectDto>(project) };

            if (output.Project.ProjectStatuId != null)
            {
                var _lookupProjectStatu = await _lookup_projectStatuRepository.FirstOrDefaultAsync((Guid)output.Project.ProjectStatuId);
                output.ProjectStatuNameStatus = _lookupProjectStatu?.NameStatus?.ToString();
            }

            if (output.Project.ProjectIndustrieId != null)
            {
                var _lookupProjectIndustrie = await _lookup_projectIndustrieRepository.FirstOrDefaultAsync((Guid)output.Project.ProjectIndustrieId);
                output.ProjectIndustrieNameIndustries = _lookupProjectIndustrie?.NameIndustries?.ToString();
            }

            output.Project.LogoFileName = project.Logo + ".png";

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto<Guid> input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProjectForEditOutput { Project = ObjectMapper.Map<CreateOrEditProjectDto>(project) };

            if (output.Project.ProjectStatuId != null)
            {
                var _lookupProjectStatu = await _lookup_projectStatuRepository.FirstOrDefaultAsync((Guid)output.Project.ProjectStatuId);
                output.ProjectStatuNameStatus = _lookupProjectStatu?.NameStatus?.ToString();
            }

            if (output.Project.ProjectIndustrieId != null)
            {
                var _lookupProjectIndustrie = await _lookup_projectIndustrieRepository.FirstOrDefaultAsync((Guid)output.Project.ProjectIndustrieId);
                output.ProjectIndustrieNameIndustries = _lookupProjectIndustrie?.NameIndustries?.ToString();
            }

            output.LogoFileName = project.Logo.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProjectDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Projects_Create)]
        protected virtual async Task Create(CreateOrEditProjectDto input)
        {
            var project = ObjectMapper.Map<Project>(input);

            if (AbpSession.TenantId != null)
            {
                project.TenantId = (int?)AbpSession.TenantId;
            }

            await _projectRepository.InsertAsync(project);
            project.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        protected virtual async Task Update(CreateOrEditProjectDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, project);
            project.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _projectRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input)
        {

            var filteredProjects = _projectRepository.GetAll()
                        .Include(e => e.ProjectStatuFk)
                        .Include(e => e.ProjectIndustrieFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProjectName.Contains(input.Filter) || e.TokenShortname.Contains(input.Filter) || e.TotalTokenSupply.Contains(input.Filter) || e.ReleaseYear.Contains(input.Filter) || e.ProjectSummary.Contains(input.Filter) || e.ProjectDescription.Contains(input.Filter) || e.WebsiteURL.Contains(input.Filter) || e.Whitepaper_URL_FAQ.Contains(input.Filter) || e.TwitterURL.Contains(input.Filter) || e.Discord.Contains(input.Filter) || e.Reddit.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.Telegram.Contains(input.Filter) || e.YourName.Contains(input.Filter) || e.YourEmailaddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectNameFilter), e => e.ProjectName == input.ProjectNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TokenShortnameFilter), e => e.TokenShortname == input.TokenShortnameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalTokenSupplyFilter), e => e.TotalTokenSupply == input.TotalTokenSupplyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReleaseYearFilter), e => e.ReleaseYear == input.ReleaseYearFilter)
                        .WhereIf(input.IsPormotedFilter.HasValue && input.IsPormotedFilter > -1, e => (input.IsPormotedFilter == 1 && e.IsPormoted) || (input.IsPormotedFilter == 0 && !e.IsPormoted))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectStatuNameStatusFilter), e => e.ProjectStatuFk != null && e.ProjectStatuFk.NameStatus == input.ProjectStatuNameStatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProjectIndustrieNameIndustriesFilter), e => e.ProjectIndustrieFk != null && e.ProjectIndustrieFk.NameIndustries == input.ProjectIndustrieNameIndustriesFilter);

            var query = (from o in filteredProjects
                         join o1 in _lookup_projectStatuRepository.GetAll() on o.ProjectStatuId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_projectIndustrieRepository.GetAll() on o.ProjectIndustrieId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProjectForViewDto()
                         {
                             Project = new ProjectDto
                             {
                                 ProjectName = o.ProjectName,
                                 TokenShortname = o.TokenShortname,
                                 TotalTokenSupply = o.TotalTokenSupply,
                                 ReleaseYear = o.ReleaseYear,
                                 ProjectSummary = o.ProjectSummary,
                                 ProjectDescription = o.ProjectDescription,
                                 WebsiteURL = o.WebsiteURL,
                                 Whitepaper_URL_FAQ = o.Whitepaper_URL_FAQ,
                                 TwitterURL = o.TwitterURL,
                                 Discord = o.Discord,
                                 Reddit = o.Reddit,
                                 Facebook = o.Facebook,
                                 Telegram = o.Telegram,
                                 YourName = o.YourName,
                                 YourEmailaddress = o.YourEmailaddress,
                                 IsPormoted = o.IsPormoted,
                                 IsActive = o.IsActive,
                                 Logo = o.Logo,
                                 Id = o.Id
                             },
                             ProjectStatuNameStatus = s1 == null || s1.NameStatus == null ? "" : s1.NameStatus.ToString(),
                             ProjectIndustrieNameIndustries = s2 == null || s2.NameIndustries == null ? "" : s2.NameIndustries.ToString()
                         });

            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Projects)]
        public async Task<List<ProjectProjectStatuLookupTableDto>> GetAllProjectStatuForTableDropdown()
        {
            return await _lookup_projectStatuRepository.GetAll()
                .Select(projectStatu => new ProjectProjectStatuLookupTableDto
                {
                    Id = projectStatu.Id.ToString(),
                    DisplayName = projectStatu == null || projectStatu.NameStatus == null ? "" : projectStatu.NameStatus.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Projects)]
        public async Task<List<ProjectProjectIndustrieLookupTableDto>> GetAllProjectIndustrieForTableDropdown()
        {
            return await _lookup_projectIndustrieRepository.GetAll()
                .Select(projectIndustrie => new ProjectProjectIndustrieLookupTableDto
                {
                    Id = projectIndustrie.Id.ToString(),
                    DisplayName = projectIndustrie == null || projectIndustrie.NameIndustries == null ? "" : projectIndustrie.NameIndustries.ToString()
                }).ToListAsync();
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

            var dir = Path.Combine(_env.ContentRootPath, @"wwwroot\view-resources\Views\DCOneCenter\ImagesProject");
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

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        public async Task RemoveLogoFile(EntityDto<Guid> input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);
            if (project == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!project.Logo.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(project.Logo.Value);
            project.Logo = null;
        }

    }
}