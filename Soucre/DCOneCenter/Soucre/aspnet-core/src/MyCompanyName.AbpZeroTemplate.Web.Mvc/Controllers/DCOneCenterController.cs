using Abp.AspNetZeroCore.Net;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries;
using MyCompanyName.AbpZeroTemplate.Projects;
using MyCompanyName.AbpZeroTemplate.Storage;
using MyCompanyName.AbpZeroTemplate.Web.Models.Ecosystem;
using MyCompanyName.AbpZeroTemplate.Web.Models.ProductIndustres;
using MyCompanyName.AbpZeroTemplate.Web.Models.Project;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Web.Controllers
{
    public class DCOneCenterController : AbpZeroTemplateControllerBase
    {

        private readonly IProjectsClientAppService _projectsClientAppService;
        private readonly IProjectIndustriesClientAppService _projectIndustriesClientAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IHostEnvironment _env;

        public DCOneCenterController(IProjectsClientAppService projectsClientAppService, IBinaryObjectManager binaryObjectManager, IHostEnvironment env, IProjectIndustriesClientAppService projectIndustriesClientAppService)
        {
            _env = env;
            _projectsClientAppService = projectsClientAppService;
            _binaryObjectManager = binaryObjectManager;
            _projectIndustriesClientAppService = projectIndustriesClientAppService;
        }
        public async Task<IActionResult> Index()
        {

            var getProjectForViewDto = await _projectsClientAppService.GetDataProjectIndex();
            ViewData["DataProject"] = getProjectForViewDto;
            ViewData["DataPROMOTED_PROJECTS"] = getProjectForViewDto.PROMOTED_PROJECTS;
            return View();
        }

        [HttpPost]
        public async Task<ProjectIndexCacheModel> LoadDataProjectIndex()
        {
            try
            {
                var getProjectForViewDto = await _projectsClientAppService.GetDataProjectIndex();

                var dir = "/view-resources/Views/DCOneCenter/ImagesProject/";
                foreach (var item in getProjectForViewDto.RECENTLY_ADDED)
                {
                    item.Logo = Path.Combine(dir, item.Logo + ".png");
                }
                foreach (var item in getProjectForViewDto.PROMOTED_PROJECTS)
                {
                    item.Logo = Path.Combine(dir, item.Logo + ".png");
                }
                foreach (var item in getProjectForViewDto.POPULAR_PROJECTS)
                {
                    item.Logo = Path.Combine(dir, item.Logo + ".png");
                }


                return new ProjectIndexCacheModel(getProjectForViewDto);
            }
            catch (UserFriendlyException ex)
            {
                return new ProjectIndexCacheModel(new ErrorInfo(ex.Message));
            }
        }


        [HttpPost]
        public async Task<ProjectIndustresIndexCacheModel> LoadDataProjectIndustreIndex()
        {
            try
            {
                var getProjectForViewDto = await _projectIndustriesClientAppService.GetDataProjectIndustresIndex();

                return new ProjectIndustresIndexCacheModel(getProjectForViewDto);
            }
            catch (UserFriendlyException ex)
            {
                return new ProjectIndustresIndexCacheModel(new ErrorInfo(ex.Message));
            }
        }


        public async Task<IActionResult> Ecosystem()
        {
            return View();
        }


        [HttpPost]
        public async Task<EcosystemCacheModel> LoadDataEcosystemPage()
        {
            try
            {
                var DataEcosystem = await _projectsClientAppService.GetDataEcosystemProject();

                return new EcosystemCacheModel(DataEcosystem);
            }
            catch (UserFriendlyException ex)
            {
                return new EcosystemCacheModel(new ErrorInfo(ex.Message));
            }
        }

    }
}
