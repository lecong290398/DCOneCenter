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
    public class ProjectIndustriesClientAppService : AbpZeroTemplateAppServiceBase, IProjectIndustriesClientAppService
    {
        private readonly IRepository<ProjectIndustrie, Guid> _projectIndustrieRepository;
        private readonly IProjectIndustriesExcelExporter _projectIndustriesExcelExporter;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IHostEnvironment _env;

        public ProjectIndustriesClientAppService(IHostEnvironment env , IRepository<ProjectIndustrie, Guid> projectIndustrieRepository, IProjectIndustriesExcelExporter projectIndustriesExcelExporter, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _env = env;
            _projectIndustrieRepository = projectIndustrieRepository;
            _projectIndustriesExcelExporter = projectIndustriesExcelExporter;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public async Task<List<GetProjectIndustrieForViewDto>> GetDataProjectIndustresIndex()
        {
          var dbList = await _projectIndustrieRepository.GetAll()
                .OrderBy(x => x.NameIndustries)
                .ToListAsync();
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
            return results;
        }

    }
}