using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeMaterialConsumable
{
    public class PracticeMaterialConsumableModel : BaseModel
    {
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string PracticeId { get; set; }
        public int Quantity { get; set; }
        public Decimal Pricing { get; set; }
        public Decimal TotalPrice { get; set; }
        public int? Leadtime { get; set; }
        public string ManufactureName { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public List<MaterialModel> listSelect { get; set; }

    }
}
