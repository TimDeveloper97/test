using NTS.Model.Function;
using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sketches
{
    public class SketchesModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string Note { get; set; }
        public string EditContent { get; set; }
        public List<MaterialModel> ListSketchMaterialElectronic { get; set; }
        public List<MaterialModel> ListSketchMaterialMechanical { get; set; }
        public List<SketchAttachModel> ListFileTypeTwo { get; set; }

        public string CurrentVersion { get; set; }

    }

    public class SketchesFunctionModel : BaseModel
    {
        public string Id { get; set; }
        public string SketchId { get; set; }
        public string FunctionId { get; set; }
    }

    public class SketchAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string SketchId { get; set; }
        public string FileName { get; set; }
        public decimal? FileSize { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }

    }

    public class SketcheHistoryModel : BaseModel
    {
        public string Id { get; set; }
        public string SketchId { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }

        public string CreateByName { get; set; }
    }

    public class ImportSketchesMaterial
    {
        public List<SketchesModel> ListExist { get; set; }
        public string Message { get; set; }
        public ImportSketchesMaterial()
        {
            ListExist = new List<SketchesModel>();
        }
    }
}
