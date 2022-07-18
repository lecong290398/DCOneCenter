using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.ProjectIndustries;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
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
    [AbpMvcAuthorize(AppPermissions.Pages_ProjectIndustries)]
    public class ProjectIndustriesController : AbpZeroTemplateControllerBase
    {
        private readonly IProjectIndustriesAppService _projectIndustriesAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const long MaxLogoLength = 5242880; //5MB
        private const string MaxLogoLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] LogoAllowedFileTypes = { "jpeg", "jpg", "png" };

        public ProjectIndustriesController(IProjectIndustriesAppService projectIndustriesAppService, ITempFileCacheManager tempFileCacheManager)
        {
            _projectIndustriesAppService = projectIndustriesAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new ProjectIndustriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProjectIndustries_Create, AppPermissions.Pages_ProjectIndustries_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProjectIndustrieForEditOutput getProjectIndustrieForEditOutput;

            if (id.HasValue)
            {
                getProjectIndustrieForEditOutput = await _projectIndustriesAppService.GetProjectIndustrieForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProjectIndustrieForEditOutput = new GetProjectIndustrieForEditOutput
                {
                    ProjectIndustrie = new CreateOrEditProjectIndustrieDto()
                };
            }

            var viewModel = new CreateOrEditProjectIndustrieModalViewModel()
            {
                ProjectIndustrie = getProjectIndustrieForEditOutput.ProjectIndustrie,
                LogoFileName = getProjectIndustrieForEditOutput.LogoFileName,
            };

            foreach (var LogoAllowedFileType in LogoAllowedFileTypes)
            {
                viewModel.LogoFileAcceptedTypes += "." + LogoAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProjectIndustrieModal(Guid id)
        {
            var getProjectIndustrieForViewDto = await _projectIndustriesAppService.GetProjectIndustrieForView(id);

            var model = new ProjectIndustrieViewModel()
            {
                ProjectIndustrie = getProjectIndustrieForViewDto.ProjectIndustrie
            };

            return PartialView("_ViewProjectIndustrieModal", model);
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