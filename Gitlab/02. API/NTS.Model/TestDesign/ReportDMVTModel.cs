using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestDesign
{
    public class ReportDMVTModel
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string CreateBy { get; set; }
        public List<MaterialModel> ListMaterial { get; set; }

        public ReportDMVTModel()
        {
            ListMaterial = new List<MaterialModel>();
        }
    }

    public class MaterialModel
    {
        public string Stt { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Code { get; set; }
        public string RawMaterialCode { get; set; }
        public string DV { get; set; }
        public string SL { get; set; }
        public string VL { get; set; }
        public string KL { get; set; }
        public string ManufactureCode { get; set; }
        public string Note { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }

    }
}
