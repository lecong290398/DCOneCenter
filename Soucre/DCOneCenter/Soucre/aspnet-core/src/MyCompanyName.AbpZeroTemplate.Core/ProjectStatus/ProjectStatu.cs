using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus
{
    [Table("ProjectStatus")]
    public class ProjectStatu : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string NameStatus { get; set; }

        public virtual string SumaryStatus { get; set; }

    }
}