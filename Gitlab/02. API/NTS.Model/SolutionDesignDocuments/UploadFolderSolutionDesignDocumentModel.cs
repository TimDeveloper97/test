using NTS.Model.DesignDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionDesignDocuments
{
    public class UploadFolderSolutionDesignDocumentModel
    {
        public string SolutionId { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public string CreateBy { get; set; }
        public int DesignType { get; set; }
        public int CurentVersion { get; set; }
        public string EditContent { get; set; }
        public string PathImage { get; set; }
        public string NameImage { get; set; }
        public bool Design3DExist { get; set; }
        public bool Design2DExist { get; set; }
        public bool ExplanExist { get; set; }
        public bool DMVTExist { get; set; }
        public bool FCMExist { get; set; }
        public bool TSTKExist { get; set; }
        public UploadFolderSolutionDesignDocumentModel()
        {
            DesignDocuments = new List<FolderUploadModel>();
        }
    }
}
