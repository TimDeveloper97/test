using QLTK.Service.Model.Commons;
using QLTK.Service.Model.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Solution
{
    public class UploadFolderSolutionResultModel
    {
        public List<CheckUploadEntity> ListError { get; set; }
        public List<string> LstError { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public bool Status { get; set; }
        public bool IsUploadSuccess { get; set; }
        public bool Design3DExist { get; set; }
        public bool Design2DExist { get; set; }
        public bool ExplanExist { get; set; }
        public bool DMVTExist { get; set; }
        public bool FCMExist { get; set; }
        public bool TSTKExist { get; set; }
        public string PathImage { get; set; }
        public string NameImage { get; set; }
        public UploadFolderSolutionResultModel()
        {
            LstError = new List<string>();
            DesignDocuments = new List<FolderUploadModel>();
        }
    }
}
