using NTS.Model.MaterialParameter;
using NTS.Model.ModuleMaterials;
using NTS.Model.ModulePart;
using NTS.Model.ProductAccessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MaterialGroupId { get; set; }
        public string UnitId { get; set; }
        public string ManufactureId { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public string UnitName { get; set; }
        public string ManufactureName { get; set; }
        public string ManufactureCode { get; set; }
        public string Note { get; set; }
        public decimal Pricing { get; set; }
        public decimal PriceHistory { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public int DeliveryDays { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public System.Nullable<DateTime> LastBuyDate { get; set; }
        public DateTime? InputPriceDate { get; set; }
        public bool? IsUsuallyUse { get; set; }
        public string RawMaterialId { get; set; }
        public string RawMaterial { get; set; }
        public bool? Is3D { get; set; }
        public bool? IsDataSheet { get; set; }
        public string MaterialType { get; set; }
        public string MechanicalType { get; set; }
        public string Status { get; set; }
        public bool IsSendSale { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string Weight { get; set; }
        public string FileName { get; set; }
        public bool IsExport { get; set; }
        public bool IsSetup { get; set; }
        public int Index { get; set; }
        public int Type { get; set; }

        public List<string> ListModuleGroupId = new List<string>();

        public List<MaterialParameterModel> ListMaterialParameter { get; set; }
        public List<Design3DModel> ListFileDesign3D { get; set; }
        public List<DataSheetModel> ListFileDataSheet { get; set; }
        public List<ModuleMaterialResultModel> ListMaterialPart { get; set; }
        public List<MaterialImageModel> ListImage { get; set; }
        public List<MaterialModel> listSelect { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public string RawMaterialCode { get; set; }
        public string PracticeId { get; set; }
        public int Leadtime { get; set; }
        public string Parameter { get; set; }
        public string Specification { get; set; }
        public bool IsRedundant  { get; set; }
        public int RedundantAmount  { get; set; }
        public string RedundantDescription { get; set; }
        public string RedundantDeliveryNote { get; set; }

        public class ImportMaterial
        {
            public List<MaterialModel> ListExist { get; set; }
            public string Message { get; set; }
            public ImportMaterial()
            {
                ListExist = new List<MaterialModel>();
            }
        }
    }
}
