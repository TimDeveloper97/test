using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class WorkDiaryModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string SBUId { get; set; }
        public int ObjectType { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeId { get; set; }
        public string ObjectId { get; set; }
        public string EmployeeName { get; set; }
        public string Note { get; set; }
        public string EmployeeCode { get; set; }
        public int Done { get; set; }
        public System.DateTime WorkDate { get; set; }
        public decimal TotalTime { get; set; }
        public string Address { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
