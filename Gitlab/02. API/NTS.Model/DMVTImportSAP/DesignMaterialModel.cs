using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DMVTImportSAP
{
    public class DesignMaterialModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string MaterialName { get; set; }
        public string Parameter { get; set; }
        public string MaterialCode { get; set; }

        /// <summary>
        /// Mã Vật liệu
        /// </summary>
        public string RawMaterialCode { get; set; }

        public string Unit { get; set; }
        public decimal Quantity { get; set; }

        /// <summary>
        /// Vật liệu
        /// </summary>
        public string RawMaterial { get; set; }

        public decimal Weight { get; set; }
        public string Manufacturer { get; set; }
        public string Note { get; set; }
        public decimal ModuleQuantity { get; set; }
        public string ModuleIndex { get; set; }
        public decimal Price{ get; set; }
        public string ConvertUnit { get; set; }
        public string GroupCode { get; set; }
        public DateTime? DueDate { get; set; }
        public string PCBCode { get; set; }
    }
}
