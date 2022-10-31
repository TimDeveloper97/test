using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class EmployeeHistoryModel
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string QualificationId { get; set; }
        public string JobPositionId { get; set; }
        public string EmployeeGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public string BankAccount { get; set; }
        public string SocialInsurrance { get; set; }
        public string HealtInsurrance { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public Nullable<System.DateTime> StartWorking { get; set; }
        public Nullable<System.DateTime> EndWorking { get; set; }
        public string Carrer { get; set; }
        public int Status { get; set; }
        public string WorkType { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal Index { get; set; }
    }
}
