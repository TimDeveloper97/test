using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportTemplate
{
    public class GeneralElectronicModel : BaseModel
    {
        // tên module
        public string ProductName { get; set; }
        // mã module
        public string ProductCode { get; set; }
        public string DateNow { get; set; }
        public string UserName { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public bool IsExport { get; set; }
        public string Paint { get; set; }
        //tên vật tư
        public string Name { get; set; }
        // mã vật tư
        public string Code { get; set; }
        public string Check { get; set; }
        public string Approve { get; set; }
        public string Designer { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string ManufactureName { get; set; }
        public string Specification { get; set; }
        public string RawMaterial { get; set; }
        public decimal Weight { get; set; }
        public string Note { get; set; }
    }
}
