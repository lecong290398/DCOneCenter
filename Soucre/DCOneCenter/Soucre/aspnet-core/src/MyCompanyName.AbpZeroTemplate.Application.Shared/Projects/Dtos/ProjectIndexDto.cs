using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public class ProjectIndexDto : EntityDto<Guid>
    {
        public string ProjectName { get; set; }
        public string ProjectSummary { get; set; }
        public string Logo { get; set; }

    }

    public class ProjectIndexOutput
    {
        public ProjectIndexOutput()
        {
            RECENTLY_ADDED = new List<ProjectIndexDto>();
            POPULAR_PROJECTS = new List<ProjectIndexDto>();
            PROMOTED_PROJECTS = new List<ProjectIndexDto>();
        }
        public List<ProjectIndexDto> PROMOTED_PROJECTS { get; set; }
        public List<ProjectIndexDto> POPULAR_PROJECTS { get; set; }
        public List<ProjectIndexDto> RECENTLY_ADDED { get; set; }

    }
}
