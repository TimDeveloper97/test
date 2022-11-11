using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Mobile.Error
{
    public class ErrorSearchCondition: SearchCommonModel
    {
        public string NameCode { get; set; }
        public string ErrorGroupId { get; set; }
        public string ErrorGroupName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentProcessId { get; set; }
        public string ErrorBy { get; set; }
        public string ErrorName { get; set; }
        public string FixBy { get; set; }
        public string StageId { get; set; }
        public Nullable<System.DateTime> DateOpen { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public int Type { get; set; }
        public bool IsExport { get; set; }
        public int Status { get; set; }
        public string ProjectId { get; set; }
        public string ObjectId { get; set; }
        public string ProjectName { get; set; }
        public string SBUId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public int? FixStatus { get; set; }
        public string DepartmentManageId { get; set; }
        public int? ErrorAffectId { get; set; }
        public int? PlanType { get; set; }
        public string AuthorDepartmentId { get; set; }
        public bool IsLate { get; set; }
        public bool IsAllPermission { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
