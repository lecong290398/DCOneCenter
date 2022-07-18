using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Exporting
{
    public interface IProjectStatusExcelExporter
    {
        FileDto ExportToFile(List<GetProjectStatuForViewDto> projectStatus);
    }
}