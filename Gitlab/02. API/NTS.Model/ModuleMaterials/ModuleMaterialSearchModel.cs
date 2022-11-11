using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterials
{
    public class ModuleMaterialSearchModel : SearchCommonModel
    {
        //public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ProductProjectId { get; set; }
        //public string MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string ManufacturerCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectProductId { get; set; }
        public bool IsParent { get; set; }
        //public string Specification { get; set; }
        //public string RawMaterialCode { get; set; }
        //public string RawMaterial { get; set; }
        //public string Price { get; set; }
        //public string Quantity { get; set; }
        //public string Amount { get; set; }
        //public string Weight { get; set; }
        //public string ManufacturerCode { get; set; }
        //public string Note { get; set; }
        //public string UnitName { get; set; }
        //public System.Nullable<DateTime> LastBuyDate { get; set; }
    }
}
