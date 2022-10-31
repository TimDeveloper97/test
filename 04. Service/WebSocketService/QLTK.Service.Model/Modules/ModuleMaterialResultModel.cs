using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Modules
{
    public class ModuleMaterialResultModel
    {
        public string ModuleCode { get; set; }
        public List<string> Materials { get; set; }
        public string Id { get; set; }
        public string IndexExport { get; set; }
        public string ModuleMaterialId { get; set; }
        public string ProductModuleMaterialId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string MaterialId { get; set; }
        public string SimilarMaterialConfig { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        /// <summary>
        /// Mã vật liệu
        /// </summary>
        public string RawMaterialCode { get; set; }
        public string RawMaterial { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHistory { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Weight { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufactureName { get; set; }
        public string Note { get; set; }
        public string UnitName { get; set; }
        public decimal Pricing { get; set; }
        public decimal ParentPricing { get; set; }
        public bool IsExport { get; set; }
        public bool IsSetup { get; set; }
        public string Path { get; set; }
        public string FilePath { get; set; }
        public List<object> ListDatashet { get; set; }
        public List<object> ListFileSetup { get; set; }
        public System.Nullable<DateTime> LastBuyDate { get; set; }
        public System.Nullable<DateTime> InputPriceDate { get; set; }
        public string Check { get; set; }
        public string ModuleGroupName { get; set; }
        public decimal TotalQuantity { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public int DeliveryDays { get; set; }
        public string FileName { get; set; }
        public string Index { get; set; }
        /// <summary>
        /// Vật tư không có giá
        /// </summary>
        public bool IsNoPrice { get; set; }
        public ModuleMaterialResultModel()
        {
            ListDatashet = new List<object>();
            ListFileSetup = new List<object>();
        }
    }
}
