using System;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class ProjectDto : EntityDto<Guid>
    {
        public string ProjectName { get; set; }

        public string TokenShortname { get; set; }

        public string TotalTokenSupply { get; set; }

        public string ReleaseYear { get; set; }

        public string ProjectSummary { get; set; }

        public string ProjectDescription { get; set; }

        public string WebsiteURL { get; set; }

        public string Whitepaper_URL_FAQ { get; set; }

        public string TwitterURL { get; set; }

        public string Discord { get; set; }

        public string Reddit { get; set; }

        public string Facebook { get; set; }

        public string Telegram { get; set; }

        public string YourName { get; set; }

        public string YourEmailaddress { get; set; }

        public bool IsPormoted { get; set; }

        public bool IsActive { get; set; }

        public Guid? Logo { get; set; }

        public string LogoFileName { get; set; }

        public Guid ProjectStatuId { get; set; }

        public Guid ProjectIndustrieId { get; set; }

    }
}