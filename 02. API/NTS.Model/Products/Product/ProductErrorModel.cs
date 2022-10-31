using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class ProductErrorModel : BaseModel
    {
        public string Id { get; set; }
        public string ErrorGroupId { get; set; }
        public string ErrorGroupName { get; set; }
        public string DepartmentProcessId { get; set; }
        public string DepartmentProcessName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Subject { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ErrorBy { get; set; }
        public string ErrorByName { get; set; }
        public string QuickSolution { get; set; }
        public string Solution { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string FixBy { get; set; }
        public string FixByName { get; set; }
        public decimal ErrorCost { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanFinishDate { get; set; }
        public Nullable<System.DateTime> ActualStartDate { get; set; }
        public Nullable<System.DateTime> ActualFinishDate { get; set; }
        public Nullable<int> IsKCSConfirmed { get; set; }
        public Nullable<int> IsManagerConfirmed { get; set; }
        public Nullable<int> TempConfirmed { get; set; }
        public Nullable<System.DateTime> DesignCompleteTime { get; set; }
        public Nullable<System.DateTime> KCSCompleteTime { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string PersonConfirmMail { get; set; }
        public string PersonProcessMail { get; set; }
    }
}
