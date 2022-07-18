using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.ProjectStatus;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.ProjectStatus;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProjectStatus)]
    public class ProjectStatusController : AbpZeroTemplateControllerBase
    {
        private readonly IProjectStatusAppService _projectStatusAppService;

        public ProjectStatusController(IProjectStatusAppService projectStatusAppService)
        {
            _projectStatusAppService = projectStatusAppService;

        }

        public ActionResult Index()
        {
            var model = new ProjectStatusViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProjectStatus_Create, AppPermissions.Pages_ProjectStatus_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetProjectStatuForEditOutput getProjectStatuForEditOutput;

            if (id.HasValue)
            {
                getProjectStatuForEditOutput = await _projectStatusAppService.GetProjectStatuForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getProjectStatuForEditOutput = new GetProjectStatuForEditOutput
                {
                    ProjectStatu = new CreateOrEditProjectStatuDto()
                };
            }

            var viewModel = new CreateOrEditProjectStatuModalViewModel()
            {
                ProjectStatu = getProjectStatuForEditOutput.ProjectStatu,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProjectStatuModal(Guid id)
        {
            var getProjectStatuForViewDto = await _projectStatusAppService.GetProjectStatuForView(id);

            var model = new ProjectStatuViewModel()
            {
                ProjectStatu = getProjectStatuForViewDto.ProjectStatu
            };

            return PartialView("_ViewProjectStatuModal", model);
        }

    }
}