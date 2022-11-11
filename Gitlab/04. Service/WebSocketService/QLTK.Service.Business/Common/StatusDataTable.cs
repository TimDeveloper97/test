using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Common
{
    public class StatusDataTable
    {
        /// <summary>
        /// Loại linh kiện
        /// </summary>
        public enum PartType
        {
            Part = 1, // Linh kiện
            PartMake, // Linh kiện sản xuất
            Product // TBSX
        }

        /// <summary>
        /// Trạng thái thiết bị mua ngoài trong dự án
        /// </summary>
        public enum StatusPProductOut
        {
            Unrealized = 1,//Chưa thực hiện
            Ongoing// Đang thực hiện            
        }

        /// <summary>
        /// Trạng thái thiết bị mua ngoài trong dự án
        /// </summary>
        public enum StatusProjectModulePart
        {
            Unrealized = 1,//Chưa thực hiện
            Ongoing,// Đang thực hiện   
            Complate
        }

        /// <summary>
        /// Trạng thái yêu cầu thiết bị mua ngoài
        /// </summary>
        public enum StatusRequireProductOut
        {
            Unrealized = 1, // Chưa thực hiện
            Ongoing, // Đang thực hiện  
            Test, // Đang KCS
            Complete // Hoàn thành
        }

        /// <summary>
        /// Trạng thái yêu cầu linh kiện
        /// </summary>
        public enum StatusRequirePart
        {
            Unrealized = 1, // Chưa thực hiện
            Ongoing, // Đang thực hiện
            Test, // Đang KCS
            Complete // Hoàn thành
        }

        /// <summary>
        /// Trạng thái yêu cầu linh kiện
        /// </summary>
        public enum StatusRequireMaterial
        {
            Unrealized = 1, // Chưa thực hiện
            Ongoing, // Đang thực hiện
            Test, // Đang KCS
            Complete // Hoàn thành
        }

        /// <summary>
        /// Trạng thái tài liệu module
        /// </summary>
        public enum StatusModuleFile
        {
            Create = 1, // Đang tạo
            WaitConfirm, // Chưa duyệt duyệt
            Confirm, // Đã duyệt
            NotConfirm // Không duyệt
        }

        /// <summary>
        /// Trang thái sản thiết bị xuất
        /// </summary>
        public enum StatusProjectProducts
        {
            Unrealized = 1, // Chưa thực hiện
            Design, // Đang thiết kế
            Request, // Đang yêu cầu vật tư
            Assembly, // Đang lắp ráp
            AssemblyComplete, // Đang lắp ráp
            Test, // Đang KCS
            TestComplete, // Đang KCS
            Complete,// Hoàn thành
            ImportComplete // Nhập kho
        }

        /// <summary>
        /// Trang thái kiểm tra thông tin
        /// </summary>
        public enum StatusProjectProductsCheckDesign
        {
            Check = 1,
            NoCheck
        }

        /// <summary>
        /// Trang thái xác nhận thiết bị sản xuất
        /// </summary>
        public enum StatusConfirmProjectProducts
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        /// <summary>
        /// Trạng thái xác nhận báo giá
        /// </summary>
        public enum StatusConfirmQuotations
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        /// <summary>
        /// Loại thiết bị báo giá
        /// </summary>
        public enum QuotationProductType
        {
            ProductOut = 1,
            Product,
            QuotationsProduct
        }

        /// <summary>
        /// Loại nhóm
        /// </summary>
        public enum GroupType
        {
            Product = 1, //Thiết bị          
            Parts = 2, // Linh kiện
            File = 3, // Tài liệu            
            Error = 4, // Lỗi
            Item = 5 //Hạng mục
        }
        /// <summary>
        /// Loại nhóm
        /// </summary>
        public enum GroupPartType
        {
            All = 0, //Thiết bị          
            PartBuy = 1, // Linh kiện
            PartProduct = 2 // Tài liệu            

        }

        /// <summary>
        /// Tình trạng xác nhận của bảng tài liệu hồ sơ dự thầu
        /// </summary>
        public enum StatusBidsFile
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        /// <summary>
        /// Trạng thái vật tư trong module
        /// </summary>
        public enum ProjectModulePartsStatusUpdate
        {
            /// <summary>
            ///  Bình thường
            /// </summary>
            Nomarl = 1,
            /// <summary>
            /// Cập nhật
            /// </summary>
            Update,
            /// <summary>
            /// Trạng thái cập nhật số lượng
            /// </summary>
            UpdateTotal,
            /// <summary>
            /// Xóa
            /// </summary>
            Remove,
            /// <summary>
            /// Hủy phải nhập kho
            /// </summary>
            Cancel
        }

        /// <summary>
        /// Loại thiết bị trong vận chuyển
        /// </summary>
        public enum DeliveryProductType
        {
            ProductOut = 1,// Thiết bị mua ngoài
            Product// Thiết bị sản xuất
        }

        /// <summary>
        /// Trang thái vận chuyển
        /// </summary>
        public enum StatusConfirmDelivery
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        /// <summary>
        ///  Kiểu dự án
        /// </summary>
        public enum ProjectType
        {
            External = 1,// Bên ngoài
            Internal = 2 // Nội bộ
        }

        // Trạng thái dự án
        public enum StatusProject
        {
            Nomarl = 1,// Đang thực hiện           
            Pause, // Tạm dừng
            Cancel, // Hủy
            Finish // Kết thúc
        }

        /// <summary>
        /// Loại module
        /// </summary>
        public enum ModuleType
        {
            StandardBase = 1, // Module chuẩn
            Standard // Thiết bị chuẩn
        }

        /// <summary>
        /// Trang thái tài liệu thiết bị sản xuất
        /// </summary>
        public enum StatusProjectModuleFile
        {
            Create = 1, // Đang tạo
            WaitConfirm, // Đang chờ duyệt
            Confirm, // Đã duyệt
            NotConfirm // Không duyệt
        }

        /// <summary>
        /// Loại của hạn mục công việc
        /// </summary>
        public enum ItemType
        {
            Product = 1,
            Arising
        }

        /// <summary>
        /// Trạng thái bảng tiêu chí thiết bị sản xuất
        /// </summary>
        public enum StatusProjectModuleCriteria
        {
            NG = 0, // KHông đạt
            OK // Đạt
        }

        /// <summary>
        /// Trạng thái bảng chi tiết tiêu chí
        /// </summary>
        public enum StausItemsDetails
        {
            Uncomplete = 1, // Chưa hoàn thành
            Complete = 2// Hoàn thành
        }

        /// <summary>
        /// Loại yêu cầu vật tư
        /// </summary>
        public enum RequestType
        {
            ProductOut = 1,// yêu cầu thiết bị mua ngoài
            Product, // yêu cầu thiết bị sản xuất
            Free, // Thiết bị tự do
            ProductMake // TBSX
        }

        /// <summary>
        /// Trạng thái khách hàng của báo giá
        /// </summary>
        public enum StatusCustomerQuotation
        {
            CustomerWait = 1,
            CustomerNotConfirm,
            CustomerConfirm
        }

        /// <summary>
        /// Trạng thái yêu cầu vật tư
        /// </summary>
        public enum StatusConfirmRequest
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        public enum MaterialType
        {
            Part = 1, // Vật tư trong kho
            NotPart // Vật tư ngoài kho
        }

        /// <summary>
        /// Trạng thái yêu cầu vật tư
        /// </summary>
        public enum StatusRequest
        {
            Uncomplete = 1,
            Complete
        }

        /// <summary>
        /// Trạng thái bảng yêu cầu mua
        /// </summary>
        public enum StatusProposalBuy
        {
            Uncomplete = 1, // Chưa thực hiện
            Ongoing, // Đang thực hiện
            Complete // Đã hoàn thành
        }

        /// <summary>
        /// Loại phiếu mua
        /// </summary>
        public enum ProductTypeProposalBuy
        {
            ProductOut = 1,//Thiết bị mua ngoài
            Product,//Linh kiện
            Free
        }

        /// <summary>
        /// trạng thái vật tư mua
        /// </summary>
        public enum StatusBuyProductParts
        {
            OnBuy = 1,//Đang mua
            Complete,//Đang nhập kho

        }

        /// <summary>
        /// trạng thái vật tư nhập kho
        /// </summary>
        public enum StatusProductPartsImport
        {
            OnWarehousing = 1,//Đang nhập kho
            Complete,//Xong
        }

        /// <summary>
        /// trạng thái tiêu chí nhập kho
        /// </summary>
        public enum StatusCriteriaImport
        {

            NG = 0,//NG
            OK,//Ok
        }

        /// <summary>
        /// Trạng thai bảng lỗi thiết bị sản xuất
        /// </summary>
        public enum StatusProjectModuleError
        {
            Create = 1,
            OnFix,// Đang sửa
            Fixed,// Đã sửa
            Complete// Đã xác nhận
        }

        /// <summary>
        /// Trạng thái bảng 
        /// </summary>
        public enum StatusRequestImport
        {
            Create = 1, // Đang tạo
            WaitKCS, // Yêu cầu KCS
            KCSConfirm, // Đã được KCS
            WaitImport,// Yêu cầu nhập kho
            ConfirmImport // Đã nhập kho
        }

        /// <summary>
        /// Trạng thái bảng 
        /// </summary>
        public enum StatusRequestProductImport
        {
            Create = 1, // Đang tạo  
            WaitKCS, // Yêu cầu KCS
            KCSConfirm, // Đã được KCS
            WaitImport = 4,// Yêu cầu nhập kho
            ConfirmImport = 5 // Đã nhập kho
        }

        /// <summary>
        /// Trạng thái bảng 
        /// </summary>
        public enum ProductImportType
        {
            Part = 1, // linh kiện 
            Product, // thiết bị
        }

        /// <summary>
        /// Trạng phiếu yêu cầu đề nghị nhập kho
        /// </summary>
        public enum StatusConfirmProposalBuy
        {
            Create = 1, // Đang tạo
            WaitManagerConfirm, // Đang chờ trưởng phòng duyệt           
            ManagerNotConfirm, // Trưởng phòng  Không duyệt
            WaitDirectorConfirm, // Đang chờ giám đốc duyệt
            DirectorConfirm, // Giám đốc đã duyệt
            DirectorNotConfirm // Giám đốc không xác
        }

        /// <summary>
        /// Khách hàng xác nhận báo giá
        /// </summary>
        public enum CustomerConfirm
        {
            CustomerWaitConfirm = 1,//Chưa đồng ý
            CustomerConfirm,//Đồng ý
            CustomerNotConfirm//Không đồng ý
        }

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public enum ConfirmContentObjectType
        {
            Product = 1, // Thiết bị sản xuất
            Request = 2, // Yêu cầu vật tư
            Bid = 3,// Hồ sơ dự thầu
            Delivery = 4,//Vận chuyển
            Quotation = 5, //Báo giá
            DeleveryOrder// Yêu cầu xuất kho
        }

        /// <summary>
        /// Loại xác nhận
        /// </summary>
        public enum ConfirmContentConfirmType
        {
            NotConfirm = 1, // Không duyệt
            Confirm = 2, // Duyệt
            RequestConfirm, // Yêu cầu duyệt
            /// <summary>
            /// Yêu cầu xuất kho
            /// </summary>
            DeliveryOrder
        }

        /// <summary>
        /// Trạng thái xuất kho
        /// </summary>
        public enum StatusConfirmDeliveryOrder
        {
            Create = 1,// Đang tạo
            WaitingExport,//Đang chờ xuất kho
            WaitingConfirmExport,
            NotConfirmExport,
            /// <summary>
            /// Đã xuất kho
            /// </summary>
            Exported, // Đã xuất kho
            //Complete
        }

        /// <summary>
        /// Loại function
        /// </summary>
        public enum FunctionType
        {
            Project = 1,
            ProductMake,
            Users,
            Customer,
            Parts,
            GroupType,
            Supllier,
            ProductOut,
            Quotations,
            Module,
            ProductStandard,
            Request,
            RequestBuy,
            RequestImport,
            Summary,
            Items,
            File,
            DeliveryOrder,
            SourceCode,
            Setting,
            Orders,
            Invoice,
            ReserveFund
        }

        /// <summary>
        /// Trạng thái đăng nhập
        /// </summary>
        public enum StatusLoginUser
        {
            /// <summary>
            ///  Cho phép login
            /// </summary>
            StartLogin = 0,

            /// <summary>
            /// Đang login
            /// </summary>
            Logging,

            /// <summary>
            /// // Log Out
            /// </summary>
            LogOut
        }

        /// <summary>
        /// Trạng thái nguồn
        /// </summary>
        public enum StatusSourceCode
        {
            /// <summary>
            /// Bình thường
            /// </summary>
            Nomarl = 1,
            /// <summary>
            /// Đăng đợi thiết kế
            /// </summary>
            Design,
            /// <summary>
            /// Xóa
            /// </summary>
            Delete

        }

        /// <summary>
        /// Loại chức năng cần định nghĩa
        /// </summary>
        public enum TypeFunctionDefinition
        {
            /// <summary>
            /// Thiết kế
            /// </summary>
            Design = 1,
            /// <summary>
            /// Sản xuất
            /// </summary>
            Production
        }

        /// <summary>
        /// Loại thư mục đinh nghĩ
        /// </summary>
        public enum TypeFolderName
        {
            /// <summary>
            /// Không theo loại
            /// </summary>
            None = 0,
            /// <summary>
            /// Mã nhóm
            /// </summary>
            GroupCode,
            /// <summary>
            /// Mã thiết bị
            /// </summary>
            ProductCode,
            /// <summary>
            /// Ma nhóm cha
            /// </summary>
            ParentGroupCode,
            /// <summary>
            /// Bỏ PCB. cho cấu hình điện tử
            /// </summary>
            ExceptPCB,
            /// <summary>
            /// Mã module nguồn
            /// </summary>
            ModuleCode
        }

        /// <summary>
        /// Loại thư mục đinh nghĩ
        /// </summary>
        public enum TypeFolderIndex
        {
            /// <summary>
            /// Không theo loại
            /// </summary>
            None = 0,
            /// <summary>
            /// Mã nhóm
            /// </summary>
            Number,
            /// <summary>
            /// Ký tự
            /// </summary>
            Characters
        }

        /// <summary>
        /// 
        /// </summary>
        public enum StatusFolderUpload
        {
            Nomarl = 0,
            Upload
        }

        /// <summary>
        /// Loại tìm kiếm thiết kế điện
        /// </summary>
        public enum SearchElectronType
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// Tên
            /// </summary>
            Name,
            /// <summary>
            /// Mã
            /// </summary>
            Model
        }

        /// <summary>
        /// Loại tài liệu thiết kế
        /// </summary>
        public enum FileDesignType
        {
            Mechanical = 1,
            Electric = 2,
            Electron = 3
        }

        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// Đang tạo
            /// </summary>
            Create = 1,
            /// <summary>
            /// Đang thực hiện
            /// </summary>
            Ongoing,
            /// <summary>
            /// Đã hoàn thành
            /// </summary>
            Complete
        }

        /// <summary>
        /// Trạng thái vật tư trong yêu cầu mua
        /// </summary>
        public enum BuyOrderStatus
        {
            UnOrder = 1,// Chưa tạo đơn hàng
            Order // Đã tạo đơn hàng
        }

        /// <summary>
        /// Tình trạng xác nhận của bảng tài liệu hồ sơ dự thầu
        /// </summary>
        public enum StatusIEnter
        {
            Error = -1, // Lỗi
            Normal = 0, // Bình thường
            OK = 1, // Đồng ý
            AddNew = 2, // Thêm mới           
            Edit = 3, // Chỉnh sửa 
            Delete = 4 // Xóa           
        }

        /// <summary>
        /// Tình trạng xác nhận của bảng tài liệu hồ sơ dự thầu
        /// </summary>
        public enum StatusNTS
        {
            Error = -1, // Lỗi
            Normal = 0, // Bình thường
            OK = 1, // Đồng ý
            AddNew = 2, // Thêm mới           
            Edit = 3, // Chỉnh sửa 
            Delete = 4 // Xóa           
        }

        /// <summary>
        /// Loại đối tượng nhập kho
        /// </summary>
        public enum ProductPartsImportType
        {
            Parts = 1,// Vật tư
            Product,// Thiết bị sản xuất
            ProductOut// Thiết bị mua ngoài
        }

        /// <summary>
        /// Loại xuất kho
        /// </summary>
        public enum DeliveryOrderType
        {
            PartsMake = 1,// Vật tư
            ProjectProduct,
        }

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public enum MessageType
        {
            ProjectProduct = 1,
            Request = 2,
            ProposalBuy = 3,
            Quotation = 4,
            RequestImport = 5,
            RequestExport = 6,
            Order = 7
        }

        public enum RequestPaidStatus
        {
            /// <summary>
            /// Bình thường
            /// </summary>
            Nomarl = 1,
            /// <summary>
            /// Khẩn cấp
            /// </summary>
            Emergency
        }

        public enum RequestPaidStatusTransfer
        {
            /// <summary>
            /// Chưa chuyển
            /// </summary>
            None = 1,
            /// <summary>
            /// Đã chuyển
            /// </summary>
            Transfer
        }
    }
}
