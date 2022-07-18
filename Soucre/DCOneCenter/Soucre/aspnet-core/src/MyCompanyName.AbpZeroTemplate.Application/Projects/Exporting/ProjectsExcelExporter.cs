using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Projects.Exporting
{
    public class ProjectsExcelExporter : NpoiExcelExporterBase, IProjectsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectForViewDto> projects)
        {
            return CreateExcelPackage(
                "Projects.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Projects"));

                    AddHeader(
                        sheet,
                        L("ProjectName"),
                        L("TokenShortname"),
                        L("TotalTokenSupply"),
                        L("ReleaseYear"),
                        L("ProjectSummary"),
                        L("ProjectDescription"),
                        L("WebsiteURL"),
                        L("Whitepaper_URL_FAQ"),
                        L("TwitterURL"),
                        L("Discord"),
                        L("Reddit"),
                        L("Facebook"),
                        L("Telegram"),
                        L("YourName"),
                        L("YourEmailaddress"),
                        L("IsPormoted"),
                        L("IsActive"),
                        L("Logo"),
                        (L("ProjectStatu")) + L("NameStatus"),
                        (L("ProjectIndustrie")) + L("NameIndustries")
                        );

                    AddObjects(
                        sheet, projects,
                        _ => _.Project.ProjectName,
                        _ => _.Project.TokenShortname,
                        _ => _.Project.TotalTokenSupply,
                        _ => _.Project.ReleaseYear,
                        _ => _.Project.ProjectSummary,
                        _ => _.Project.ProjectDescription,
                        _ => _.Project.WebsiteURL,
                        _ => _.Project.Whitepaper_URL_FAQ,
                        _ => _.Project.TwitterURL,
                        _ => _.Project.Discord,
                        _ => _.Project.Reddit,
                        _ => _.Project.Facebook,
                        _ => _.Project.Telegram,
                        _ => _.Project.YourName,
                        _ => _.Project.YourEmailaddress,
                        _ => _.Project.IsPormoted,
                        _ => _.Project.IsActive,
                        _ => _.Project.LogoFileName,
                        _ => _.ProjectStatuNameStatus,
                        _ => _.ProjectIndustrieNameIndustries
                        );

                });
        }
    }
}