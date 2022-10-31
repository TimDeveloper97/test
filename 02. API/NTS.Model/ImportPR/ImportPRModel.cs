using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportPR
{
    public class ImportPRModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public string UnitName { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureCode { get; set; }
        public int Quantity { get; set; }
        public DateTime RequireDate { get; set; }
        public DateTime? PODueDate { get; set; }
        public string SalesBy { get; set; }
        public string SalesName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string PurchaseRequestCode { get; set; }
        public bool Status { get; set; }
        public decimal QuotaPrice { get; set; }
    }
}
