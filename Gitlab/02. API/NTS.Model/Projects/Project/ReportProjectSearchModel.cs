using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class ReportProjectSearchModel
    {
        public string DepartmentId { get; set; }
        public bool OrderType { get; set; }
        public string OrderBy { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool IsExist { get; set; }



        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PaymentName { get; set; }
        public string CustomerId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public decimal Collected { get; set; } //Đã thu
        public decimal? Exist { get; set; } //Còn phải thu
        public decimal? Receivables { get; set; } //Phải thu

        public string Name { get; set; } //Loại thanh toán

        public decimal ActualAmount1 { get; set; } 
        public decimal ActualAmount2 { get; set; } 
        public decimal ActualAmount3 { get; set; }
        public decimal ActualAmount4 { get; set; }
        public decimal ActualAmount5 { get; set; }

        public DateTime? ActualPaymentDate1 { get; set; }
        public DateTime? ActualPaymentDate2 { get; set; }
        public DateTime? ActualPaymentDate3 { get; set; }
        public DateTime? ActualPaymentDate4 { get; set; }
        public DateTime? ActualPaymentDate5 { get; set; }
        public List<string> ListProject { get; set; }
        public ReportProjectSearchModel()
        {
            ListProject = new List<string>();
        }
    }
}
