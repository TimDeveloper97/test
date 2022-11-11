using NTS.Model.Common;
using NTS.Model.Materials;
using NTS.Model.QLTKMODULE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SketchMaterialElectronic
{
    public class SketchMaterialElectronicModel 
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string ModuleId { get; set; }
        public int Quantity { get; set; }
        public int? Leadtime { get; set; }
        public string Note { get; set; }
        public string Name { get; set; } // đổ dữ liệu của material
        public string Code { get; set; } // đổ dữ liệu của material
        public List<MaterialModel> ListSketchMaterialElectronic { get; set; }
        public List<ModuleModel> ListSketchModuleElectronic { get; set; }
        public List<SketchMaterialElectronicModel> ListSketchMaterialElectronicModel { get; set; }

    }

    public class ImportSketchesMaterialElectronic
    {
        public List<SketchMaterialElectronicModel> ListExist { get; set; }

        public List<int> RowMaterialEmpty { get; set; }
        public string Message { get; set; }
        public ImportSketchesMaterialElectronic()
        {
            ListExist = new List<SketchMaterialElectronicModel>();
            RowMaterialEmpty = new List<int>();
        }
    }

    public class UploadResultModelElectronic : UploadResultModel
    {
        public string ModuleId { get; set; }
    }
    public class MaterialElectronicMechanical
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string ModuleId { get; set; }
        public int Quantity { get; set; }
        public int? Leadtime { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }

        public string Code { get; set; }
    }
}
