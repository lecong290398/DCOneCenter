using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectStatus.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}