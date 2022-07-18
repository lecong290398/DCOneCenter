using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Exporting
{
    public class ProjectStatusExcelExporter : NpoiExcelExporterBase, IProjectStatusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectStatusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectStatuForViewDto> projectStatus)
        {
            return CreateExcelPackage(
                "ProjectStatus.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProjectStatus"));

                    AddHeader(
                        sheet,
                        L("NameStatus"),
                        L("SumaryStatus")
                        );

                    AddObjects(
                        sheet, projectStatus,
                        _ => _.ProjectStatu.NameStatus,
                        _ => _.ProjectStatu.SumaryStatus
                        );

                });
        }
    }
}