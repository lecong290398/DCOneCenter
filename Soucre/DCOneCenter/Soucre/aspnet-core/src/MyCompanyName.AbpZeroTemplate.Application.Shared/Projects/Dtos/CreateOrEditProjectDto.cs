using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(ProjectConsts.MaxProjectNameLength, MinimumLength = ProjectConsts.MinProjectNameLength)]
        public string ProjectName { get; set; }

        public string TokenShortname { get; set; }

        public string TotalTokenSupply { get; set; }

        public string ReleaseYear { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxProjectSummaryLength, MinimumLength = ProjectConsts.MinProjectSummaryLength)]
        public string ProjectSummary { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxProjectDescriptionLength, MinimumLength = ProjectConsts.MinProjectDescriptionLength)]
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

        public string LogoToken { get; set; }

        public Guid ProjectStatuId { get; set; }

        public Guid ProjectIndustrieId { get; set; }

    }
}