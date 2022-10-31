using NTS.Model.Projects.ProjectAttch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectAttch
{
    public class ProjectAttchInfoModel
    {
        public string ProjectId { get; set; }
        public string CreateBy { get; set; }
        public List<ProjectAttchModel> JuridicalFiles { get; set; }
        public List<ProjectAttchModel> TechnicalFiles { get; set; }
        public List<ProjectAttchModel> OtherFiles { get; set; }
        public ProjectAttchInfoModel()
        {
            OtherFiles = new List<ProjectAttchModel>();
            JuridicalFiles = new List<ProjectAttchModel>();
            TechnicalFiles = new List<ProjectAttchModel>();
        }
    }
}
