using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries
{
    [Table("ProjectIndustries")]
    public class ProjectIndustrie : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string NameIndustries { get; set; }

        public virtual string SumaryIndustries { get; set; }

        public virtual bool IsActive { get; set; }
        //File

        public virtual Guid? Logo { get; set; } //File, (BinaryObjectId)

    }
}