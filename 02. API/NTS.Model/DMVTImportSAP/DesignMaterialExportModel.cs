using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DMVTImportSAP
{
    public class DesignMaterialExportModel
    {
        public string ProjectCode { get; set; }
        public string IndexDesign { get; set; }
        public string IndexSAP { get; set; }
        //public string IndexStandard { get; set; }
        public string MaterialName { get; set; }
        public string Prameter { get; set; }
        public string MaterialCode { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; }
        public string RawMaterial { get; set; }
        public string Manufacturer { get; set; }
        public string Note { get; set; }
        public decimal? Price { get; set; }
        public string GroupCode { get; set; }
        public string StoreCode { get; set; }
        public int LeadTime { get; set; }
        public string MakeBuy { get; set; }
        public decimal ModuleQuantity { get; set; }
    }
    public class ListDataModel
    {
        public List<string> ListDatashet { get; set; }
    }
}
