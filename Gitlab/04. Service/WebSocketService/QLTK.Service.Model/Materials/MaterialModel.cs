using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Materials
{
    public class MaterialModel
    {
        public string Id { get; set; }
        public string Stt { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Code { get; set; }
        public string RawMaterialCode { get; set; }
        public string DV { get; set; }
        public string SL { get; set; }
        public string VL { get; set; }
        public string KL { get; set; }
        public string ManufactureName { get; set; }
        public string ManufactureCode { get; set; }
        public string Note { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public string CreateBy { get; set; }
        public bool IsUsuallyUse { get; set; }
        public string Status { get; set; }
        public bool Is3DExist { get; set; }

    }
}
