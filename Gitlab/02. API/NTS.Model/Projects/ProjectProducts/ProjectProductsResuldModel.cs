using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProducts
{
    public class ProjectProductsResuldModel : BaseModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string ProjectProductId { get; set; }
        public string ContractName { get; set; }
        public string ContractCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public int ModuleStatus { get; set; }
        public string DepartmentRequestId { get; set; }
        public string DepartmentPerformId { get; set; }
        public string Manufacture { get; set; }
        public decimal Quantity { get; set; }
        public decimal ContractQuantity { get; set; }
        public int? Categories { get; set; }
        public int TotalError { get; set; }
        public int TotalNoDone { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? RequestDate { get; set; }
        public int DataType { get; set; }
        public int DesignStatus { get; set; }
        public string Index { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal Pricing { get; set; }
        public int CreateIndex { get; set; }
        public decimal CheckQuantity { get; set; }
        public string Note { get; set; }
        public bool Checked { get; set; }
        public decimal Amount { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPriceContract { get; set; }
        public decimal RealQuantity { get; set; }
        public bool IsGeneralDesign { get; set; }
        public bool IsNoPrice { get; set; }
        public int StatusUse { get; set; }
        public int ApproveStatus { get; set; }
    }
}
