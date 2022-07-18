using Abp.Web.Models;
using MyCompanyName.AbpZeroTemplate.Projects.Dtos;

namespace MyCompanyName.AbpZeroTemplate.Web.Models.Project
{
    public class ProjectIndexCacheModel : ErrorInfo
    {
        public ProjectIndexOutput DataProjects { get; set; }

        public ProjectIndexCacheModel(ProjectIndexOutput dataProjects)
        {
            DataProjects = dataProjects;
        }

        public ProjectIndexCacheModel(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}
