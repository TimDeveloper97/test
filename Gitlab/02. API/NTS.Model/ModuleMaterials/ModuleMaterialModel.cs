using NTS.Model.Datasheet;
using NTS.Model.ProductMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterials
{
    public class ModuleMaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string ModuleId { get; set; }
        public string MaterialId { get; set; }
        public string Code { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        public string RawMaterialCode { get; set; }
        public string RawMaterial { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalModule { get; set; }
        public decimal Total { get; set; }
        public decimal TotalInProject { get; set; }
        public decimal Weight { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal PriceHistory { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufacturerCode { get; set; }
        public string Note { get; set; }
        public string UnitName { get; set; }
        public bool IsSetup { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime? LastBuyDate { get; set; }
        public DateTime? InputPriceDate { get; set; }
        public List<DatasheetModel> ListDatashet { get; set; }

        public List<FileSetUpModel> ListFileSetup { get; set; }
    }
}
