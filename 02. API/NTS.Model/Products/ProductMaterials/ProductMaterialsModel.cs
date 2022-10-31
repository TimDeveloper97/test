using NTS.Model.Datasheet;
using NTS.Model.ProductDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductMaterials
{
    public class ProductMaterialsModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductModuleMaterialId { get; set; }
        public string MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        public string RawMaterialCode { get; set; }
        public string RawMaterial { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Weight { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufacturerCode { get; set; }
        public string Note { get; set; }
        public string UnitName { get; set; }
        public string SetupFilePath { get; set; }
        public string DatasheetPath { get; set; }
        public string IsSetup { get; set; }
        public bool IsExport { get; set; }
        public List<FileSetupModel> ListFileSetup { get; set; }
        public List<DatasheetModel> ListFileDatasheet { get; set; }
        public List<ProductDocumentModel> ListFielDocument { get; set; }
    }

    public class ResultDownload
    {
        public string PathZip { get; set; }
        public string Error { get; set; }
    }
}
