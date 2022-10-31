using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Report
{
  public  class ReportErrorSearchModel : SearchCommonModel
    {
        public string ProjectId { get; set; }
        public string ProjectValue { get; set; }
        public string ProjectStatus { get; set; }
        public string DepartmentManageId { get; set; }
        public string DepartmentId { get; set; }
        public string FixDepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string StageId { get; set; }
        public int? AffectId { get; set; }
        public int WorkType { get; set; }
        public int Deadline { get; set; }
        public string CustomerId { get; set; }
        public string CustomerFinalId { get; set; }
        public bool IsAffect { get; set; }
    }
}
