using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeMaterial
{
    public class PracticeMaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MaterialId { get; set; }
        public string PracticeId { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupName { get; set; }
        public string ManufactureName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Pricing { get; set; }
        public decimal TotalPrice { get; set; }
        public string Leadtime { get; set; }
        public string UnitName { get; set; }
        public List<MaterialModel> listSelect { get; set; }
    }
}
