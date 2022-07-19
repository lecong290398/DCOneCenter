using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Projects.Dtos
{
    public  class GetAllEcosystemProjectDto
    {
        public string NameIndustres { get; set; }
        public List<PrjectInfor> ListProject { get; set; }

        public GetAllEcosystemProjectDto()
        {
            ListProject = new List<PrjectInfor>();
        }
    }



    public class PrjectInfor
    {
        public string NameProject { get; set; }
        public string Logo { get; set; }
    }
}
