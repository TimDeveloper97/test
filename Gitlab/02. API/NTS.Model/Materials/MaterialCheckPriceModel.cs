using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialCheckPriceModel
    {
        public string Index { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        public string MaterialCode { get; set; }
        public string RawMaterialCode { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
        public string RawMaterial { get; set; }
        public decimal Weight { get; set; }
        public string ManufacturerCode { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public MaterialCheckPriceModel()
        {
        }
    }
}

