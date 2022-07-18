using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.ProjectIndustries.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}