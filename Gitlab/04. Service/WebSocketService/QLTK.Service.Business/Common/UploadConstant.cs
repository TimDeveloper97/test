using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Common
{
    public class UploadConstant
    {
        public const string PathProjectProject = "/DA";
        public const string PathProjectContract = "/HD";
        public const string PathProjectBids = "/HSDT";
        public const string PathProjectProductOut = "/TBMN";
        public const string PathProjectProduct = "/TBSX";

        /// <summary>
        /// .DIN
        /// </summary>
        public const string ElectricExtension = ".DIN";
        public const string ErrorExtension = ".Error";

        /// <summary>
        /// .Image
        /// </summary>
        public const string ImageExtention = ".Image";

        /// <summary>
        /// Chức năng upload tài liệu
        /// </summary>
        public enum FunctionType
        {
            Project = 1,
            Contract,
            Product,
            ProjectProductOut,
            Part,
            ProductStandard,
            ProductCode,
            Bids,
            ProductOutSide,
            ProductError,
            ProductElectron
        }

        /// <summary>
        /// Folder home trên server
        /// </summary>
        public enum FolderHost
        {
            Project = 1,
            Part,
            Module,
            SourceCode,
            ProductOut,
            BackupRestor
        }

        /// <summary>
        /// Loại upload
        /// </summary>
        public enum UploadType
        {
            Upload = 0,
            Download
        }

        /// <summary>
        /// Loại download
        /// </summary>
        public enum DownloadType
        {
            File = 0,
            Folder
        }

        /// <summary>
        /// Loại upload tài liệu
        /// </summary>
        public enum UploadDocumentType
        {
            File = 0,
            Folder
        }

        /// <summary>
        /// Loại thiết kế
        /// </summary>
        public enum DesignType
        {
            Electric = 0,
            Mechanic
        }

        /// <summary>
        /// 
        /// </summary>
        public enum FolderType
        {
            DrawPrint = 1,
            Draw2D,
            Draw3D,
            ProductElectric,
            ModuleElectric,
            Product,
            Source,
            Normal
        }
    }
}
