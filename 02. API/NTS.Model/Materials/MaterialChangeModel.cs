using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialChangeModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string RawMaterialCode { get; set; }
        public string RawMaterial { get; set; }
        public string UnitName { get; set; }
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
        public decimal Pricing { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureCode { get; set; }
        public decimal TotalPrice { get; set; }

        public string NewIndex { get; set; }
        public string NewName { get; set; }
        public string NewSpecification { get; set; }
        public string NewRawMaterialCode { get; set; }
        public string NewRawMaterial { get; set; }
        public string NewUnitName { get; set; }
        public decimal NewWeight { get; set; }
        public decimal NewQuantity { get; set; }
        public decimal NewPricing { get; set; }
        public string NewNote { get; set; }
        public decimal NewAmount { get; set; }
        public string NewManufactureId { get; set; }
        public string NewManufactureCode { get; set; }
        public decimal NewTotalPrice { get; set; }

        public string THTKIndex { get; set; }
        public string THTKName { get; set; }
        public string THTKSpecification { get; set; }
        public string THTKRawMaterialCode { get; set; }
        public string THTKRawMaterial { get; set; }
        public string THTKUnitName { get; set; }
        public decimal THTKWeight { get; set; }
        public decimal THTKQuantity { get; set; }
        public decimal THTKPricing { get; set; }
        public string THTKNote { get; set; }
        public decimal THTKAmount { get; set; }
        public string THTKManufactureId { get; set; }
        public string THTKManufactureCode { get; set; }
        public decimal THTKTotalPrice { get; set; }

        public bool DupplicateCode { get; set; }
        public bool DupplicateIndex { get; set; }
        public bool DupplicateItem { get; set; }

        public string ModuleId { get; set; }
        public string ModuleCode { get; set; }
    }
}
