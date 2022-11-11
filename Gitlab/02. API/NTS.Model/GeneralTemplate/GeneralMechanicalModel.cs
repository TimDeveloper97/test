using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class GeneralMechanicalModel : BaseModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Responsible { get; set; } // phụ trách chính
        public string TechnologicalScheme { get; set; } // sơ đồ công nghệ
        public string FaceModule { get; set; } // module mặt
        public string RelationshipClusters { get; set; } // mối liên hệ
        public bool IsExport { get; set; }
        public string Designer { get; set; }
        public bool FileElectric { get; set; }
        public bool FileElectronic { get; set; }
        public bool FileMechanics { get; set; }
        public string UserName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string ManufactureName { get; set; }
        public string Specification { get; set; }
        public string RawMaterial { get; set; }
        public decimal Weight { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public List<MaterialModel> Materials { get; set; }
        public string UserNamePC { get; set; }

    }
    public class MaterialModel
    {
        public string Id { get; set; }
        public string MaterialName { get; set; }
        public string RawMaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public int DeliveryDays { get; set; }
        public string ManufacturerName { get; set; }
        public decimal Pricing { get; set; }
        public string Note { get; set; }
    }
    public class MaterialSearchModel
    {
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

    }
}
