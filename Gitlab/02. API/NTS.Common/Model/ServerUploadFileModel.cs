using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
   public class ServerUploadFileModel
    {
        /// <summary>
        /// Tên/ IP server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Thư mục lưu ảnh
        /// </summary>
        public string FoldelName { get; set; }

        /// <summary>
        /// Key cho phép upload file lên server
        /// </summary>
        public string KeyAuthorize { get; set; }

        /// <summary>
        /// Thời gian Sleep đồng bộ ảnh lên server
        /// </summary>
        public int Timeout { get; set; }
    }
}
