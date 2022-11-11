using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class UploadModel
    {
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string KeyAuthorize { get; set; }
        public bool isCreateThumb { get; set; }
    }
    public class UploadResultModel
    {
        public string FileName { get; set; }
        public string Link { get; set; }
        public string LinkThumb { get; set; }
        public int? Size { get; set; }
        

    }
}
