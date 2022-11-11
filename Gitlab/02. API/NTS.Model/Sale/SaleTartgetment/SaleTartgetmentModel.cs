using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleTartgetment
{
    public class SaleTartgetmentModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int Year { get; set; }
        public long? SaleTarget { get; set; }
        public string DomainId { get; set; }
        public string IndustryId { get; set; }
        public string ApplicationId { get; set; }
        public DateTime PlanContractDate { get; set; }
        public string UpdateBy { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string DomainName { get; set; }
        public string ApplicationName { get; set; }
        public string IndustryName { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
