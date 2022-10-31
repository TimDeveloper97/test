using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SimilarMaterialConfig
{
    public class SimilarMaterialConfigModel : BaseModel
    {
        public string Id { get; set; }
        public string SimilarMaterialId { get; set; }
        public string SimilarMaterialName { get; set; }
        public string MaterialId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MaterialGroupName { get; set; }
        public string ManufactureName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Pricing { get; set; }
        public decimal TotalPrice { get; set; }
        public string Leadtime { get; set; }
        public string UnitName { get; set; }
        public string Note { get; set; }
        public string Parameter { get; set; }
        public List<MaterialModel> ListSimilarMaterialConfig { get; set; }
        public bool Check { get; set; }
    }
}
