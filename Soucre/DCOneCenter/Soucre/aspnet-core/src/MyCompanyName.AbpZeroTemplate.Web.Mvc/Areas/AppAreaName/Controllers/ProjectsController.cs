using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Projects;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Projects;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

using System.IO;
using System.Linq;
using Abp.Web.Models;
using Abp.UI;
using Abp.IO.Extensions;
using MyCompanyName.AbpZeroTemplate.Storage;
using Microsoft.AspNetCore.Http;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Projects)]
    public class ProjectsController : AbpZeroTemplateControllerBase
    {
        private readonly IProjectsAppService _projectsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxLogoLength = 5242880; //5MB
        private const string MaxLogoLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] LogoAllowedFileTypes = { "jpeg", "jpg", "png" };

        public ProjectsController(IProjectsAppService projectsAppService, ITempFileCacheManager tempFileCacheManager)
        {
            _projectsAppService = projectsAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new ProjectsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Projects_Create, AppPermissions.Pages_Projects_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProjectForEditOutput getProjectForEditOutput;

            if (id.HasValue)
            {
                getProjectForEditOutput = await _projectsAppService.GetProjectForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProjectForEditOutput = new GetProjectForEditOutput
                {
                    Project = new CreateOrEditProjectDto()
                };
            }

            var viewModel = new CreateOrEditProjectModalViewModel()
            {
                Project = getProjectForEditOutput.Project,
                ProjectStatuNameStatus = getProjectForEditOutput.ProjectStatuNameStatus,
                ProjectIndustrieNameIndustries = getProjectForEditOutput.ProjectIndustrieNameIndustries,
                ProjectProjectStatuList = await _projectsAppService.GetAllProjectStatuForTableDropdown(),
                ProjectProjectIndustrieList = await _projectsAppService.GetAllProjectIndustrieForTableDropdown(),
                LogoFileName = getProjectForEditOutput.LogoFileName,
            };

            foreach (var LogoAllowedFileType in LogoAllowedFileTypes)
            {
                viewModel.LogoFileAcceptedTypes += "." + LogoAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProjectModal(Guid id)
        {
            var getProjectForViewDto = await _projectsAppService.GetProjectForView(id);

            var model = new ProjectViewModel()
            {
                Project = getProjectForViewDto.Project
                ,
                ProjectStatuNameStatus = getProjectForViewDto.ProjectStatuNameStatus

                ,
                ProjectIndustrieNameIndustries = getProjectForViewDto.ProjectIndustrieNameIndustries

            };

            return PartialView("_ViewProjectModal", model);
        }

        public FileUploadCacheOutput UploadLogoFile(IFormFile file)
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                if (file.Length > MaxLogoLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxLogoLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (LogoAllowedFileTypes != null && LogoAllowedFileTypes.Length > 0 && !LogoAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", LogoAllowedFileTypes));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

    }
}