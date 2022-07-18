using System.Collections.Generic;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Exporting
{
    public interface IProjectIndustriesExcelExporter
    {
        FileDto ExportToFile(List<GetProjectIndustrieForViewDto> projectIndustries);
    }
}