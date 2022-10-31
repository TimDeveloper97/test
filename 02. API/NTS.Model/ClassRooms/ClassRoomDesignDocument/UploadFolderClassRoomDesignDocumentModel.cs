using NTS.Model.DesignDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomDesignDocument
{
    public class UploadFolderClassRoomDesignDocumentModel
    {
        public string ClassRoomId { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public string CreateBy { get; set; }
        public int DesignType { get; set; }
        public UploadFolderClassRoomDesignDocumentModel()
        {
            DesignDocuments = new List<FolderUploadModel>();
        }
    }
}
