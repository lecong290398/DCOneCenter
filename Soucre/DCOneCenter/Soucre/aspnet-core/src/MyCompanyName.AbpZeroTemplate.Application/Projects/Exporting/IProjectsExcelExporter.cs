using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.Projects.Exporting
{
    public interface IProjectsExcelExporter
    {
        FileDto ExportToFile(List<GetProjectForViewDto> projects);
    }
}