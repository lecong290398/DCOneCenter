using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}