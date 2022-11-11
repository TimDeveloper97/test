using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Common
{
    public class UploadEntity
    {
        private List<string> pathClientFileNotDefinition;

        public List<string> PathClientFileNotDefinition
        {
            get { return pathClientFileNotDefinition; }
            set { pathClientFileNotDefinition = value; }
        }

        /// <summary>
        /// Loại upload
        /// </summary>
        public UploadConstant.UploadType UploadType
        {
            get;
            set;
        }

        /// <summary>
        /// Chức năng
        /// </summary>
        public UploadConstant.FunctionType FunctionType
        {
            get;
            set;
        }

        /// <summary>
        /// Thư mục host
        /// </summary>
        public UploadConstant.FolderHost FolderHost
        {
            get;
            set;
        }

        /// <summary>
        /// Loại download
        /// </summary>
        public UploadConstant.DownloadType DownloadType
        {
            get;
            set;
        }

        /// <summary>
        /// Id đối tượng cha
        /// </summary>
        public string ParentObjectId
        {
            get;
            set;
        }


        /// <summary>
        /// Id đối tượng hiện tại
        /// </summary>
        public string ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Mã đối tượng hiện tại
        /// </summary>
        public string ObjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// Đường dẫn file client
        /// </summary>
        public string PathFileClient
        {
            get;
            set;
        }

        /// <summary>
        /// Linh upload
        /// </summary>
        public Uri UriUpload
        {
            get;
            set;
        }

        /// <summary>
        /// Thread chạy
        /// </summary>
        public BackgroundWorker Bgw
        {
            get;
            set;
        }

        /// <summary>
        /// Dung lượng file
        /// </summary>
        public double FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// Đường dẫn thư mục trên server
        /// </summary>
        public string PathFolderServer
        {
            get;
            set;
        }

        /// <summary>
        /// Đường dẫn thư mục trên server
        /// </summary>
        public string PathFolderClient
        {
            get;
            set;
        }

        /// <summary>
        /// Đường dẫn file trên server
        /// </summary>
        public string PathFileServer
        {
            get;
            set;
        }

        /// <summary>
        /// Cho phép replease tất cả các file đã tồn tại
        /// </summary>
        public bool IsSkips
        {
            get;
            set;
        }

        /// <summary>
        /// File cần xóa trên server
        /// </summary>
        public string PathFileToDelete
        {
            get;
            set;
        }

        /// <summary>
        /// Id loại tài liệu
        /// </summary>
        public string TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Loại thư mục upload
        /// </summary>
        public UploadConstant.FolderType FolderType
        {
            get;
            set;
        }

        /// <summary>
        /// Loại tài liệu
        /// </summary>
        public UploadConstant.UploadDocumentType UploadDocumentType
        {
            get;
            set;
        }

        public bool IsShowException
        {
            get;
            set;
        }

        /// <summary>
        /// Trạng thái ghi đè file
        /// </summary>
        public bool IsRelease
        {
            get;
            set;
        }

        /// <summary>
        /// Tên mới
        /// </summary>
        public string NewName
        {
            get;
            set;
        }

        /// <summary>
        /// Trạng thái có lấy UserFTP hay không
        /// </summary>
        public bool IsGetUserFTP
        {
            get;
            set;
        }

        /// <summary>
        /// User đăng nhập ftp
        /// </summary>
        public string UserFTPName
        {
            get;
            set;
        }

        /// <summary>
        /// Mật khẩu đăng nhập FTP
        /// </summary>
        public string PasswordFTP
        {
            get;
            set;
        }

        /// <summary>
        /// Đuôi file
        /// </summary>
        public string Extension
        {
            get;
            set;
        }

        /// <summary>
        /// Nhóm đối tượng
        /// </summary>
        public string GroupId
        {
            get;
            set;
        }

        private int totalFile;

        public int TotalFile
        {
            get { return totalFile; }
            set { totalFile = value; }
        }
    }
}
