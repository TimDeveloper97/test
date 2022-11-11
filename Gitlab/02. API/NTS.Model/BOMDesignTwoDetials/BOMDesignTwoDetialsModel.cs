using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.BOMDesignTwoDetials
{
    public class BOMDesignTwoDetialsModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string BOMDesignTwoId { get; set; }
        public int MaterialType { get; set; }
        public string MaterialId { get; set; }
        public string Specification { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string FileIndex { get; set; }
        public int SAPIndex { get; set; }
        public string ModuleIndex { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string ManufactureCode { get; set; }
        public string UnitName { get; set; }
        public string GroupCode { get; set; }
    }
}
