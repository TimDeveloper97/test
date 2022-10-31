using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeSupMaterial
{
    public class PracticeSupMaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MaterialId { get; set; }
        public string PracticeId { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public string ManufactureName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public decimal Pricing { get; set; }
        public decimal TotalPrice { get; set; }
        public int Leadtime { get; set; }
        public int Type { get; set; }
        public System.Nullable<DateTime> LastBuyDate { get; set; }
        public DateTime? InputPriceDate { get; set; }
        public decimal PriceHistory { get; set; }
    }

    public class PracticeSupMaterialInfoModel
    {
        public List<PracticeSupMaterialModel> Materials { get; set; }
        public string PracticeId { get; set; }
    }
}
