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
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetZeroCore.Net;

namespace MyCompanyName.AbpZeroTemplate.Projects
{
    public class ProjectsClientAppService : AbpZeroTemplateAppServiceBase, IProjectsClientAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IProjectsExcelExporter _projectsExcelExporter;
        private readonly IRepository<ProjectStatu, Guid> _lookup_projectStatuRepository;
        private readonly IRepository<ProjectIndustrie, Guid> _lookup_projectIndustrieRepository;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public ProjectsClientAppService(IRepository<Project, Guid> projectRepository, IProjectsExcelExporter projectsExcelExporter, IRepository<ProjectStatu, Guid> lookup_projectStatuRepository, IRepository<ProjectIndustrie, Guid> lookup_projectIndustrieRepository, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _projectRepository = projectRepository;
            _projectsExcelExporter = projectsExcelExporter;
            _lookup_projectStatuRepository = lookup_projectStatuRepository;
            _lookup_projectIndustrieRepository = lookup_projectIndustrieRepository;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public async Task<ProjectIndexOutput> GetDataProjectIndex()
        {

            var result = new ProjectIndexOutput();

            result.PROMOTED_PROJECTS = await _projectRepository.GetAll()
                .Where(c => c.IsActive == true && c.IsPormoted == true)
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .Select(PROMOTED_PROJECTS => new ProjectIndexDto
                {
                    Id = PROMOTED_PROJECTS.Id,
                    Logo = PROMOTED_PROJECTS.Logo.ToString(),
                    ProjectName = PROMOTED_PROJECTS.ProjectName,
                    ProjectSummary = PROMOTED_PROJECTS.ProjectSummary.ToString(),
                }).ToListAsync();

            result.POPULAR_PROJECTS = await _projectRepository.GetAll()
                .Where(c => c.IsActive == true)
                .OrderBy(x => Guid.NewGuid())
                .ThenByDescending(c => c.CountSee)
                .Take(4)
               .Select(PROMOTED_PROJECTS => new ProjectIndexDto
               {
                   Id = PROMOTED_PROJECTS.Id,
                   Logo = PROMOTED_PROJECTS.Logo.ToString(),
                   ProjectName = PROMOTED_PROJECTS.ProjectName,
                   ProjectSummary = PROMOTED_PROJECTS.ProjectSummary.ToString(),
               }).ToListAsync();


            result.RECENTLY_ADDED = await _projectRepository.GetAll()
              .Where(c => c.IsActive == true)
              .OrderBy(x => Guid.NewGuid())
              .ThenByDescending(c => c.CreationTime)
              .Take(4)
             .Select(PROMOTED_PROJECTS => new ProjectIndexDto
             {
                 Id = PROMOTED_PROJECTS.Id,
                 Logo = PROMOTED_PROJECTS.Logo.ToString(),
                 ProjectName = PROMOTED_PROJECTS.ProjectName,
                 ProjectSummary = PROMOTED_PROJECTS.ProjectSummary.ToString(),
             }).ToListAsync();

            return result;
        }

        public async Task<List<GetAllEcosystemProjectDto>> GetDataEcosystemProject()
        {
            var result = new List<GetAllEcosystemProjectDto>();
            var listIndustres = await _lookup_projectIndustrieRepository.GetAll().ToListAsync();

            foreach (var itemIndustre in listIndustres)
            {
                var listProject = await _projectRepository.GetAll()
                .Where(c => c.IsActive == true && c.ProjectIndustrieFk.Id == itemIndustre.Id).ToListAsync();
                var modelRecord = new GetAllEcosystemProjectDto();
                modelRecord.NameIndustres = itemIndustre.NameIndustries;

                if (listProject.Count() > 0)
                {
                    foreach (var itemPrject in listProject)
                    {
                        var ProjectImportRecord = new PrjectInfor
                        {
                            Logo = itemPrject.Logo + ".png",
                            NameProject = itemPrject.ProjectName
                        };
                        modelRecord.ListProject.Add(ProjectImportRecord);
                    }

                    result.Add(modelRecord);
                }
            }
            return result;
        }
    }
}