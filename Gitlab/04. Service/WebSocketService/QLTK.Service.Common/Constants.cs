using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Common
{
    public class Constants
    {
        #region Materials
        /// <summary>
        /// Dang sử dụng
        /// </summary>
        public const string Material_Status_Use = "0";

        /// <summary>
        /// Tạm dừng
        /// </summary>
        public const string Material_Status_Pause = "1";

        /// <summary>
        /// Ngừng sản xuất
        /// </summary>
        public const string Material_Status_Stop = "2"; 


        #endregion

        public const string Sheet_PartProduct = "DMVT";
        public static string CheckElectron = "TPAT.";

        /// <summary>
        /// Thư mục export
        /// </summary>
        public const string FolderExport = "Export/";

        /// <summary>
        /// Danh sách các Định dạng kiểu số
        /// </summary>
        public enum NumberFormatType { INTEREGER = 1, FLOAT = 2, DECIMAL = 3 };

        public const string TPA = "TPA";

        public const string HAN = "HÀN";

        public const string NO_CAD_SHARE = "0";

        #region Manufaceture
        public const string manufacetureUse = "0"; // Còn sử dụng
        public const string manufacetureStop = "1"; // Không sử dụng
        #endregion

        public const string MODULE_DESIGN_DOCUMENT_ERROR_NAME = "1";
        public const string MODULE_DESIGN_DOCUMENT_ERROR_SIZE = "2";
        public const string MODULE_DESIGN_DOCUMENT_ERROR_OK = "0";

        /// <summary>
        /// Thiết kế cơ khi
        /// </summary>
        public const int Design_Type_CK = 1;

        /// <summary>
        /// Thiết kế điện
        /// </summary>
        public const int Design_Type_DN = 2;

        /// <summary>
        /// Thiết kế điện tử
        /// </summary>
        public const int Design_Type_DT = 3;

        /// <summary>
        /// Loại đối tượng: MOdule
        /// </summary>
        public const int Definition_ObjectType_Module = 1;

        /// <summary>
        /// Loại đối tượng: Thiết bị
        /// </summary>
        public const int Definition_ObjectType_Product = 2;


        #region File Definition
        /// <summary>
        /// Định nghĩa Danh mục vật tư
        /// </summary>
        public const int FileDefinition_FileType_ListMaterial = 1;

        /// <summary>
        /// Dịnh nghĩa file 2D tổng
        /// </summary>
        public const int FileDefinition_FileType_IDW = 2;

        /// <summary>
        /// Dịnh nghĩa file Bản vẽ thiết kế điện dạng PDF
        /// </summary>
        public const int FileDefinition_FileType_DnPDF = 3;

        /// <summary>
        /// File lấy ảnh giải pháp
        /// </summary>
        public const int FileDefinition_FileType_Image = 4;

        /// <summary>
        /// File bản giải trình
        /// </summary>
        public const int FileDefinition_FileType_Explanation = 5;

        /// <summary>
        /// File FCM
        /// </summary>
        public const int FileDefinition_FileType_FCM = 6;

        /// <summary>
        /// File TSTK
        /// </summary>
        public const int FileDefinition_FileType_Parameter = 7;
        #endregion

        #region Folder
        /// <summary>
        /// Ổ cứng để thiết kế
        /// </summary>
        public const string Disk_Start_Design = "D:";

        /// <summary>
        /// Ổ cứng để thiết kế
        /// </summary>
        public const string Disk_Design= "D:\\";
        
        /// <summary>
        /// Dịnh nghĩa folder upload
        /// </summary>
        public const int FolderDefinition_FolderType_Upload = 1;

        /// <summary>
        /// Dịnh nghĩa folder chứa bản cứng CAD
        /// </summary>
        public const int FolderDefinition_FolderType_BCCAD = 2;

        /// <summary>
        /// Dịnh nghĩa folder chứa file CAD
        /// </summary>
        public const int FolderDefinition_FolderType_FileCAD = 3;

        /// <summary>
        /// Dịnh nghĩa folder MAT
        /// </summary>
        public const int FolderDefinition_FolderType_MAT = 4;

        /// <summary>
        /// Định nghĩa folder bản cứng cơ khí
        /// </summary>
        public const int FolderDefinition_FolderType_BCCk = 5;

        /// <summary>
        /// Định nghĩa folder 3D
        /// </summary>
        public const int FolderDefinition_FolderType_3D = 6;

        /// <summary>
        /// Định nghĩa folder IGS
        /// </summary>
        public const int FolderDefinition_FolderType_IGS = 7;

        /// <summary>
        /// Định nghĩa folder Download TPA
        /// </summary>
        public const int FolderDefinition_FolderType_TPA = 8;

        /// <summary>
        /// Định nghĩa folder HMI
        /// </summary>
        public const int FolderDefinition_FolderType_HMI = 9;

        /// <summary>
        /// Định nghĩa folder PLC
        /// </summary>
        public const int FolderDefinition_FolderType_PLC= 10;

        /// <summary>
        /// Định nghĩa folder Software
        /// </summary>
        public const int FolderDefinition_FolderType_Software = 11;
        #endregion

        #region
        /// <summary>
        /// Loại download: server
        /// </summary>
        public const int Type_DownLoad_Server = 1;

        /// <summary>
        /// Loại download: ftp
        /// </summary>
        public const int Type_DownLoad_Ftp = 2;
        #endregion
    }
}
