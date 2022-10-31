using NTS.Model.ModuleDesignDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class TestDesignStructureModel
    {
        public string ApiUrl { get; set; }
        public string PathFile { get; set; }
        public string SelectedPath { get; set; }
        public string ModuleCode { get; set; }
        public List<Design3DModel> List3D { get; set; }
        public List<MaterialModel> ListMaterialDB { get; set; }
        public List<ModuleDesignDocumentModel> ListModuleDesignDocument { get; set; }
        public List<RawMaterialModel> ListRawMaterial { get; set; }
        public List<ErrorModel> ListModuleError { get; set; }
        public List<ConverUnitModel> ListConvertUnit { get; set; }
        public List<DesignStructureModel> ListDesignStructure { get; set; }
        public List<DesignStructureFileModel> ListDesignStructureFile { get; set; }
        public List<ModuleModel> Module { get; set; }
    }
}
