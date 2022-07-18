using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Exporting
{
    public class ProjectIndustriesExcelExporter : NpoiExcelExporterBase, IProjectIndustriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectIndustriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectIndustrieForViewDto> projectIndustries)
        {
            return CreateExcelPackage(
                "ProjectIndustries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProjectIndustries"));

                    AddHeader(
                        sheet,
                        L("NameIndustries"),
                        L("SumaryIndustries"),
                        L("IsActive"),
                        L("Logo")
                        );

                    AddObjects(
                        sheet, projectIndustries,
                        _ => _.ProjectIndustrie.NameIndustries,
                        _ => _.ProjectIndustrie.SumaryIndustries,
                        _ => _.ProjectIndustrie.IsActive,
                        _ => _.ProjectIndustrie.LogoFileName
                        );

                });
        }
    }
}