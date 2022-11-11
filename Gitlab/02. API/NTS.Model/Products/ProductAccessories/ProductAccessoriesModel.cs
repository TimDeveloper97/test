using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductAccessories
{
    public class ProductAccessoriesModel : BaseModel
    {
        public string Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProductId { get; set; }
        public string MaterialId { get; set; }
        public string ProjectGeneralDesignId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Manafacture { get; set; }
        public decimal Quantity { get; set; }
        public decimal OldQuantity { get; set; }
        public decimal Inventoty { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string UnitName { get; set; }
        public bool IsExport { get; set; }
        public int TotalError { get; set; }
        public bool IsDelete { get; set; }
        public int Type { get; set; }
        public string StatusUse { get; set; }
        public int ModuleStatusUse { get; set; }
    }
}
