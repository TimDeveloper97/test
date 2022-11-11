using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialHistoryModel
    {
        public string Id { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string UnitId { get; set; }
        public string ManufactureId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Pricing { get; set; }
        public decimal PriceHistory { get; set; }
        public int DeliveryDays { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public Nullable<System.DateTime> LastBuyDate { get; set; }
        public Nullable<System.DateTime> InputPriceDate { get; set; }
        public Nullable<bool> IsUsuallyUse { get; set; }
        public string MaterialType { get; set; }
        public string MechanicalType { get; set; }
        public string RawMaterial { get; set; }
        public string Status { get; set; }
        public Nullable<bool> Is3D { get; set; }
        public bool Is3DExist { get; set; }
        public Nullable<bool> IsDataSheet { get; set; }
        public bool IsDatasheetExist { get; set; }
        public string Weight { get; set; }
        public string RawMaterialId { get; set; }
        public string ModuleGroupId { get; set; }
        public bool IsSetup { get; set; }
        public string Specification { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
