using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.ClassRoom
{
    public class UploadClassRoomFolderModel
    {
        public string ApiUrl { get; set; }
        public string ClassRoomId { get; set; }
        public string Token { get; set; }
        public string Path { get; set; }
        public int DesignType { get; set; }
    }
}
