using NTS.Model.Common;
using NTS.Model.Materials;
using NTS.Model.QLTKMODULE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SketchMaterialMechanical
{
    public class SketchMaterialMechanicalModel
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string ModuleId { get; set; }
        public int Quantity { get; set; }
        public int? Leadtime { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<MaterialModel> ListSketchMaterialElectronic { get; set; }
        public List<ModuleModel> ListSketchModuleElectronic { get; set; }
        public List<SketchMaterialMechanicalModel> ListSketchMaterialMechanicalModel { get; set; }


        public class ImportSketchesMaterialMechanical
        {
            public List<SketchMaterialMechanicalModel> ListExist { get; set; }
            public string Message { get; set; }
            public ImportSketchesMaterialMechanical()
            {
                ListExist = new List<SketchMaterialMechanicalModel>();
            }
        }

        public class UploadResultModelMechanical : UploadResultModel
        {
            public string ModuleId { get; set; }
        }
    }
}
