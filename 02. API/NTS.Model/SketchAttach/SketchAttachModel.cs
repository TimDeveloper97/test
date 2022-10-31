using NTS.Model.Function;
using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SketchAttach
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
        public string ModuleId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public List<SketchAttachModel> ListFileSketches { get; set; }
        public List<SketchAttachModel> ListDelete { get; set; }

    }

    public class SketchAttachHistoryModel : BaseModel
    {
        public string Id { get; set; }
        public string SketchAttachId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public List<SketchAttachHistoryModel> ListHistory { get; set; }
    }
}
