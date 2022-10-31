using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Report
{
    public class ReportErrorInfoModel
    {
        public string Id { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string StageId { get; set; }
        public string ProjectId { get; set; }
        public int Status { get; set; }
        public int? AffectId { get; set; }
        public int Deadline { get; set; }
        public string ProjectStatus { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeFixId { get; set; }
        public string DepartmentId { get; set; }
        public string FixDepartmentId { get; set; }
        public string FixDepartmentName { get; set; }
        public int FixStatus { get; set; }
        public string DepartmentManageId { get; set; }
        public string SBUManageId { get; set; }
        public string DepartmentManageName { get; set; }
        public decimal ProjectAmount { get; set; }
        public string CustomerId { get; set; }
        public string CustomerFinalId { get; set; }
        public string ErrorId { get; set; }
        public string ErrorFixId { get; set; }
        public string ErrorChangePlanId { get; set; }
        public int TotalECP { get; set; }
    }
}
