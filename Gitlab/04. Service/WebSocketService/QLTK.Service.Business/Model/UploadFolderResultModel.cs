using QLTK.Service.Business.Common;
using QLTK.Service.Model.Commons;
using QLTK.Service.Model.Files;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class UploadFolderResultModel
    {
        public List<CheckUploadEntity> ListError { get; set; }
        public List<ModuleMaterialModel> Materials { get; set; }
        public List<string> LstError { get; set; }
        public List<string> ListFolder { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public List<ModuleDesignerModel> Designers { get; set; }
        public bool Status { get; set; }
        public bool IsUploadSuccess { get; set; }
        public bool SoftwareExist { get; set; }
        public bool HMIExist { get; set; }
        public bool PLCExist { get; set; }
        public bool FilmExist { get; set; }
        public string ModuleId { get; set; }

        public UploadFolderResultModel()
        {
            LstError = new List<string>();
            DesignDocuments = new List<FolderUploadModel>();
            Materials = new List<ModuleMaterialModel>();
            ListFolder = new List<string>();
            Designers = new List<ModuleDesignerModel>();
        }
    }
}
