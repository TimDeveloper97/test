using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Reports.ReportError
{
    public class ReportErorrWorkChangePlanModel
    {
        public string ErrorId { get; set; }
        public string departmentId { get; set; }
        public string index { get; set; }
        public string ErrorFixId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ErrorCode { get; set; }
        public string Subject { get; set; }
        public string DepartmentName { get; set; }
        public string FixByName { get; set; }
        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Deadline { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int TotalChange { get; set; }
        public DateTime? NewStartDate { get; set; }
        public DateTime? NewFinishDate { get; set; }
        public string Reason { get; set; }
        public string Solution { get; set; }

        public List<InforChange> ListChange { get; set; }


        public List<DateChange> Dates { get; set; }
        public class DateChange
        {
            public string Title { get; set; }
        }

        public ReportErorrWorkChangePlanModel()
        {
            ListChange = new List<InforChange>();
        }
        //
    }

    public class InforChange
    {
        public string Id { get; set; }
        public DateTime? NewStartDate { get; set; }
        public DateTime? NewFinishDate { get; set; }
        public string Reason { get; set; }
    }
}
