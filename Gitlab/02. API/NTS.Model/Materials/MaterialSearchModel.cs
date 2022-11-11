using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ManufactureId { get; set; }
        public string MaterialType { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupName { get; set; }
        public bool IsExport { get; set; }

        public string IsAllFile { get; set; }
        public string Status { get; set; }
        public string Status3D { get; set; }
        public string StatusDatasheed { get; set; }
        public string ImagePath { get; set; }
        public string MechanicalType { get; set; }
        public string RawMaterialName { get; set; }
        public int MaterialPriceType { get; set; }
        public int MaterialHistoryPriceType { get; set; }
        public int DeliveryDaysType { get; set; }
        public int? DeliveryDays { get; set; }
        public int? LastDelivery{ get; set; }
        public int LastDeliveryType { get; set; }
        public decimal? Pricing { get; set; }
        public decimal? HistoryPrice { get; set; }
        public bool? IsSendSale { get; set; }
        public string RedundantStatus { get; set; }
        public List<string> ListIdSelect { get; set; }
        public MaterialSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
