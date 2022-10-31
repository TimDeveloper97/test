using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;
using NTS.Model.JobSupjects;

namespace NTS.Model.Job
{
    public class JobSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DegreeId { get; set; }
        public string JobGroupId { get; set; }
        public string SubjectId { set; get; }
        public string DegreeName { get; set; }
        public string JobGroupName { get; set; }
        public string SubjectName { set; get; }
        public bool IsExport { get; set; }
        //public List<JobSubjectsModel> Subject { get; set; }

        public List<string> ListIdSelect { get; set; }

        public JobSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
