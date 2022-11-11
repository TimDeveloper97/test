using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MaterialGroupId { get; set; }
        public string UnitId { get; set; }
        public string ManufactureId { get; set; }
        public string MaterialGroupName { get; set; }
        public string UnitName { get; set; }
        public string ManufactureCode { get; set; }
        public string Note { get; set; }
        public decimal Pricing { get; set; }
        public decimal PriceHistory { get; set; }
        public decimal PriceNearest { get; set; }
        public int? DeliveryDays { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public System.Nullable<DateTime> LastBuyDate { get; set; }
        public System.Nullable<DateTime> InputPriceDate { get; set; }
        public bool? IsUsuallyUse { get; set; }
        public bool IsSendSale { get; set; }

        public double LastDelivery { get; set; }
        public string RawMaterialId { get; set; }
        public string RawMaterial { get; set; }
        public bool? Is3D { get; set; }
        public bool? IsDataSheet { get; set; }
        public bool? IsSetup { get; set; }
        public int IsDocument3D { get; set; }
        public int IsDocumentDataSheet { get; set; }
        public string MaterialType { get; set; }
        public string MechanicalType { get; set; }
        public string Status { get; set; }
        public string RawMaterialName { get; set; }

        public string MaterialGroupTPAId { get; set; }
        public string Weight { get; set; }
        public string ModuleGroupId { get; set; }

        public int Quantity { get; set; }
        public int Index { get; set; }

        public bool Is3DExist { get; set; }
        public bool IsDataSheetExist { get; set; }
        public DateTime? SyncDate { get; set; }
        public bool IsRedundant { get; set; }

    }
}
