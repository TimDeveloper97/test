using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectAttch
{
    public class ProjectAttchSearchResultModel
    {
        public int JuridicalTotal { get; set; }
        public List<ProjectAttchResultModel> JuridicalFiles { get; set; }
        public int TechnicalTotal { get; set; }
        public List<ProjectAttchResultModel> TechnicalFiles { get; set; }
        public int OtherTotal { get; set; }
        public List<ProjectAttchResultModel> OtherFiles { get; set; }

        public ProjectAttchSearchResultModel()
        {
            OtherFiles = new List<ProjectAttchResultModel>();
            JuridicalFiles = new List<ProjectAttchResultModel>();
            TechnicalFiles = new List<ProjectAttchResultModel>();
        }
    }
}
