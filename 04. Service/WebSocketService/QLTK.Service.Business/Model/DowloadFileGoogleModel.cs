using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class DowloadFileGoogleModel
    {
        public string ModuleId { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }
        public List<ModuleFileModule> ListSelect { get; set; }
        public List<ModuleDesignDocumentModel> ListModuleDesignDocument { get; set; }
        public List<DataDistributionModel> ListStrucFile { get; set; }
        public List<ModuleMaterialModel> ListModuleMaterial { get; set; }
    }
}
