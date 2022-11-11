using NTS.Model.ErrorAttach;
using NTS.Model.ErrorImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorFixModel
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeFixId { get; set; }
        public string EmployeeName { get; set; }
        public string Solution { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Status { get; set; }
        public string SupportId { get; set; }
        public string ApproveId { get; set; }
        public string AdviseId { get; set; }
        public string NotifyId { get; set; }
        public int Deadline { get; set; }
        public bool IsDelete { get; set; }
        public bool IsChange { get; set; }
        public string Reason { get; set; }
        public int Done { get; set; }
        public decimal? EstimateTime { get; set; }
        public List<ErrorFixAttachModel> FixAttachs { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ActualStartDate { get; set; }

        public ErrorFixModel()
        {
            FixAttachs = new List<ErrorFixAttachModel>();
        }
    }
}
