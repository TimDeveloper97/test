using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class WorkDiarySearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string WorkTimeId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string IndustryName { get; set; }
        public string IndustryCode { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsExport { get; set; }
    }
}
