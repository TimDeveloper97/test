using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Applys
{
    public class ApplyInterviewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewBy { get; set; }
        public string Comment { get; set; }
        public string WorkTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string IdWorkType { get; set; }

        public ApplyInterviewModel()
        {

        }

    }
}
