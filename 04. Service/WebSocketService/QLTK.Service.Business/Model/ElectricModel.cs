using QLTK.Service.Business.Common;
using QLTK.Service.Model.Commons;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ElectricModel
    {
        public List<CheckUploadEntity> ListError { get; set; }
        public List<string> LstError { get; set; }
        public List<string> ListFolder { get; set; }
        public bool Message { get; set; }
        public ElectricModel()
        {
            ListError = new List<CheckUploadEntity>();
            LstError = new List<string>();
            ListFolder = new List<string>();
            ListCheckFile = new List<ElectricModel>();

            ListDesignStructure = new List<DesignStrctureModel>();
            ListDesignStructureFile = new List<DesignStrctureFileModel>();

            ListModule = new List<ModuleModel>();
        }
        public List<ElectricModel> ListCheckFile { get; set; }
        public string Name { get; set; }
        public string PathLocal { get; set; }
        public string Type { get; set; }
        public decimal Size { get; set; }
        public string ApiUrl { get; set; }
        public List<DesignStrctureModel> ListDesignStructure { get; set; }
        public List<DesignStrctureFileModel> ListDesignStructureFile { get; set; }
        public List<ModuleModel> ListModule { get; set; }
        public List<ModuleDesignDocumentModel> ListModuleDesignDoc { get; set; }

    }
}
