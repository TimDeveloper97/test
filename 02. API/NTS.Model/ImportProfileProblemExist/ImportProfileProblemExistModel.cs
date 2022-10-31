using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfileProblemExist
{
    public class ImportProfileProblemExistModel
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Note { get; set; }
        public string Plan { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string PRCode { get; set; }
        public string ProjectCodeList { get; set; }
        public string ProjectCode { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string Code { get; set; }
        public int LateDay { get; set; }
        public decimal AmountVND { get; set; }
        public int CurrencyUnit { get; set; }
    }
}
