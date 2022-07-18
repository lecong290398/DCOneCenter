using MyCompanyName.AbpZeroTemplate.ProjectStatus;
using MyCompanyName.AbpZeroTemplate.ProjectIndustries;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Projects
{
    [Table("Projects")]
    public class Project : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxProjectNameLength, MinimumLength = ProjectConsts.MinProjectNameLength)]
        public virtual string ProjectName { get; set; }

        public virtual string TokenShortname { get; set; }

        public virtual string TotalTokenSupply { get; set; }

        public virtual string ReleaseYear { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxProjectSummaryLength, MinimumLength = ProjectConsts.MinProjectSummaryLength)]
        public virtual string ProjectSummary { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxProjectDescriptionLength, MinimumLength = ProjectConsts.MinProjectDescriptionLength)]
        public virtual string ProjectDescription { get; set; }

        public virtual string WebsiteURL { get; set; }

        public virtual string Whitepaper_URL_FAQ { get; set; }

        public virtual string TwitterURL { get; set; }

        public virtual string Discord { get; set; }

        public virtual string Reddit { get; set; }

        public virtual string Facebook { get; set; }

        public virtual string Telegram { get; set; }

        public virtual string YourName { get; set; }

        public virtual string YourEmailaddress { get; set; }

        public virtual bool IsPormoted { get; set; }

        public virtual int CountSee { get; set; }

        public virtual bool IsActive { get; set; }
        //File

        public virtual Guid? Logo { get; set; } //File, (BinaryObjectId)

        public virtual Guid ProjectStatuId { get; set; }

        [ForeignKey("ProjectStatuId")]
        public ProjectStatu ProjectStatuFk { get; set; }

        public virtual Guid ProjectIndustrieId { get; set; }

        [ForeignKey("ProjectIndustrieId")]
        public ProjectIndustrie ProjectIndustrieFk { get; set; }

    }
}