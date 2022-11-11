using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkTypeInterview
{
    public class WorkTypeInterviewSearchResultModel
    {
        public int Id { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
    }
}
