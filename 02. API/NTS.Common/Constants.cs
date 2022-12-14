using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common
{
    public class Constants
    {
        public static IniFile iniFile;

        /// <summary>
        /// Thư mục export
        /// </summary>
        public const string FolderExport = "Export/";

        /// <summary>
        /// Tình trạng làm việc
        /// </summary>
        public const int STATUS_WORKING = 1;

        /// <summary>
        /// Tình trạng nghỉ việc
        /// </summary>
        public const int STATUS_NOTWORKING = 0;

        /// <summary>
        /// Tình trạng khóa
        /// </summary>
        public const int STATUS_LOCK = 1;

        /// <summary>
        /// Tình trạng chưa khóa
        /// </summary>
        public const int STATUS_NOT_LOCK = 0;

        /// <summary>
        /// Config vật tư phụ
        /// </summary>
        public const string BOM_VATTUPHU_INDEX = "BOM_VATTUPHU_INDEX";

        public const string TPASupplierId = "6876ace9-3948-4b4b-a4e9-46e9d3b16537";

        #region Notification
        /// <summary>
        /// Thông báo bình thường
        /// </summary>
        public const string NOTIFICATION_TYPE_NORMAL = "0";

        /// <summary>
        /// Loại điện thoại IOS
        /// </summary>
        public const string PHONE_TYPE_IOS = "1";

        /// <summary>
        /// Loại điện thoại ANDROID
        /// </summary>
        public const string PHONE_TYPE_ANDROID = "0";

        /// <summary>
        /// API KEY FIREBASE
        /// </summary>
        public const string FIREBASE_SERVER_KEY = "key=AAAA00GSbgE:APA91bGsA3oroPu29KbDVfYiVnkCnbJrZGNThUyi-Cf3ABs--qHXcxO6j-OH3C_nQuYfej0SjnJ0N9JoCN1BWOUxLdOCEwHLAZouTpA0KOr2lfAflOBdtpqRMu-_R_mVvvSNvmS-Adyt";

        /// <summary>
        /// FIREBASE URL
        /// </summary>
        public const string FIREBASE_URL = "https://fcm.googleapis.com/fcm/send";

        /// <summary>
        /// Nội dung tiêu đề thông báo notify
        /// </summary>
        public const string NOTIFY_TITLE = "THÔNG BÁO";

        /// <summary>
        /// Độ ưu tiên thông báo notify
        /// </summary>
        public const string NOTIFY_PRIORITY = "high";
        #endregion

        #region User
        /// <summary>
        /// Kiểu nhân viên
        /// </summary>
        public const string USER_TYPE_EMPLOYEE = "1";

        /// <summary>
        /// Kiểu khách hàng
        /// </summary>
        public const string USER_TYPE_CUSTOMER = "2";

        /// <summary>
        /// Nội dung thông báo đăng ký thành viên thành công
        /// </summary>
        public const string MESSAGE_REGISTER_ACCOUNT_SUCCESS = "Đăng ký thành viên thành công";

        public const int RESPONSE_LOGIN_STATUS_LOCK = -6;
        public const int RESPONSE_LOGIN_STATUS_WRONG_PASSWORD = -5;
        public const int RESPONSE_LOGIN_STATUS_NOT_EXITS_USER = -4;
        public const int RESPONSE_LOGIN_STATUS_NOT_EXITS_CUSTOMER = -41;
        public const int RESPONSE_LOGIN_STATUS_NOT_PERMISSION = -20;
        /// <summary>
        /// Lỗi server khi đăng nhập
        /// </summary>
        public const int RESPONSE_LOGIN_STATUS_SERVER_ERROR = -41;

        /// <summary>
        /// Nhóm quyền của người dùng
        /// </summary>
        public const string GROUP_FUNCTION_TYPE_USER = "1";

        /// <summary>
        /// Nhóm quyền của bãi xe
        /// </summary>
        public const string GROUP_FUNCTION_TYPE_PARKINGLOT = "2";

        /// <summary>
        /// Mật khẩu mặc định
        /// </summary>
        public const string DEFAULT_PASSWORD = "123456";
        #endregion

        public const string EmailPass = "EmailPass";
        public const string EmailValue = "EmailValue";

        #region UserHistory

        /// <summary>
        /// Log in & Log out system
        /// </summary>
        public const int LOGTYPE_LOGIN_LOGOUT = 1;

        /// <summary>
        /// Thêm sửa xóa khách hàng
        /// </summary>
        public const int LOG_CUSTOMMER = 2;

        /// <summary>
        /// Thêm sửa xóa quản lí nhân viên
        /// </summary>
        public const int LOG_EMPLOYEE = 3;

        /// <summary>
        /// Thêm sửa xóa quản lí quyền
        /// </summary>
        public const int LOG_FUNCTION_GROUPFUNCTION = 4;

        /// <summary>
        /// Thêm sửa xóa quản lí nhóm người dùng
        /// </summary>
        public const int LOG_GROUP = 5;

        /// <summary>
        /// Thêm sửa xóa quản lí bãi xe
        /// </summary>
        public const int LOG_PARKING = 6;

        /// <summary>
        /// Thêm sửa xóa quản lí vị trí
        /// </summary>
        public const int LOG_POSITION = 7;

        /// <summary>
        /// Thêm sửa xóa quản lí vé
        /// </summary>
        public const int LOG_TICKET = 8;

        /// <summary>
        /// Thêm sửa xóa quản lí người dùng
        /// </summary>
        public const int LOG_USER = 9;

        /// <summary>
        /// Thêm sửa xóa quản lí loại xe
        /// </summary>
        public const int LOG_VEHICLE = 10;

        /// <summary>
        /// Thêm sửa xóa vật tư
        /// </summary>
        public const int LOG_Material = 1;

        /// <summary>
        /// Thêm sửa xóa vật tư theo chức năng
        /// </summary>
        public const int LOG_MaterialGroup = 2;

        /// <summary>
        /// Thêm sửa xóa quản lý nhóm vật tư tpa
        /// </summary>
        public const int LOG_MaterialGroupTPa = 3;


        /// <summary>
        /// Thêm sửa xóa quản lý cấu hình vật tư
        /// </summary>
        public const int LOG_ConfigMaterial = 4;

        /// <summary>
        /// Thêm sửa xóa quản lý nhóm vật tư tiêu chuẩn
        /// </summary>
        public const int LOG_NSMaterialGrtoup = 5;

        /// <summary>
        /// Thêm sửa xóa hãng sản xuất
        /// </summary>
        public const int LOG_Manufacture = 6;

        /// <summary>
        /// Thêm sửa xóa quản lý đơn vị tính
        /// </summary>
        public const int LOG_Unit = 7;

        /// <summary>
        /// Thêm sửa xóa quản lý vật liệu
        /// </summary>
        public const int LOG_RawMaterial = 8;

        /// <summary>
        /// Thêm sửa xóa nhà cung cấp
        /// </summary>
        public const int LOG_Supplier = 9;

        /// <summary>
        /// Thêm sửa xóa quản lý vật tư tương tự
        /// </summary>
        public const int LOG_SimilarMaterial = 10;

        /// <summary>
        /// Thêm sửa xóa đinh nghĩa vật tư phần mềm tạo
        /// </summary>
        public const int LOG_CodeRule = 11;

        /// <summary>
        /// Thêm sửa xóa định nghĩa module
        /// </summary>
        public const int LOG_Module = 12;

        /// <summary>
        /// Thêm sửa xóa tiêu chuẩn sản phẩm
        /// </summary>
        public const int LOG_ProductStandard = 14;

        /// <summary>
        /// Thêm sửa xóa tiêu chí kiểm tra
        /// </summary>
        public const int LOG_TestCriteria = 13;

        /// <summary>
        /// Thêm sửa xóa tính năng
        /// </summary>
        public const int LOG_Function = 15;

        /// <summary>
        /// Thêm sửa xóa nghành
        /// </summary>
        public const int LOG_Industry = 16;

        /// <summary>
        /// Thêm sửa xóa bài thực hành
        /// </summary>
        public const int LOG_Practice = 18;

        /// <summary>
        /// Thêm sửa xóa kỹ năng bài thực hành
        /// </summary>
        public const int LOG_Practice_Skill = 19;

        /// <summary>
        /// Thêm sửa xóa trình độ
        /// </summary>
        public const int LOG_Degree = 20;

        /// <summary>
        /// Thêm sửa xóa chuyên môn
        /// </summary>
        public const int LOG_Specialize = 21;

        /// <summary>
        /// Thêm sửa xóa đơn vị công tác
        /// </summary>
        public const int LOG_WorkPlace = 22;

        /// <summary>
        /// Thêm sửa xóa chuyên gia
        /// </summary>
        public const int LOG_Expert = 23;

        /// <summary>
        /// Thêm sửa xóa môn học
        /// </summary>
        public const int LOG_Subject = 24;

        /// <summary>
        /// Thêm sửa xóa chương trình đào tạo
        /// </summary>
        public const int LOG_Education = 25;

        /// <summary>
        /// Thêm sửa xóa phòng học
        /// </summary>
        public const int LOG_ClassRoom = 26;

        /// <summary>
        /// Thêm sửa xóa nghề
        /// </summary>
        public const int LOG_Job = 27;

        /// <summary>
        /// Thêm sửa xóa dự án
        /// </summary>
        public const int LOG_Project = 28;

        /// <summary>
        /// Thêm sửa xóa nhóm khách hàng
        /// </summary>
        public const int LOG_CustomerType = 29;

        /// <summary>
        /// Thêm sửa xóa  khách hàng
        /// </summary>
        public const int LOG_Customer = 30;

        /// <summary>
        /// Thêm sửa xóa  Vấn đề
        /// </summary>
        public const int LOG_Error = 31;

        /// <summary>
        /// Thêm sửa xóa công việc theo nhóm module 
        /// </summary>
        public const int LOG_Task = 32;

        /// <summary>
        /// Thêm sửa xóa thời gian tiêu chuẩn cho từng công việc
        /// </summary>
        public const int LOG_TaskTimeStand = 33;

        /// <summary>
        /// Thêm sửa xóa kế hoạch thiết kế
        /// </summary>
        public const int LOG_Plan = 34;

        /// <summary>
        /// Thêm sửa xóa cấu hình thông tin ngày nghỉ
        /// </summary>
        public const int LOG_Holiday = 35;

        /// <summary>
        /// Thêm sửa xóa thời gian làm việc
        /// </summary>
        public const int LOG_WorkingTime = 36;

        /// <summary>
        /// Thêm sửa xóa giải pháp
        /// </summary>
        public const int LOG_Solution = 37;

        /// <summary>
        /// Thêm sửa xóa chương trình đào tạo
        /// </summary>
        public const int LOG_EmployeeTraining = 38;

        /// <summary>
        /// Thêm sửa xóa nhật ký công việc
        /// </summary>
        public const int LOG_WorkDiary = 39;

        /// <summary>
        /// Thêm sửa xóa nhóm quyền
        /// </summary>
        public const int LOG_GroupUser = 40;

        /// <summary>
        /// Thêm sửa xóa thời gian làm việc
        /// </summary>
        public const int LOG_TimeWorking = 41;

        /// <summary>
        /// Thêm sửa xóa quản lý phòng ban
        /// </summary>
        public const int LOG_Department = 42;

        /// <summary>
        /// Thêm sửa xóa quản lý chức vụ
        /// </summary>
        public const int LOG_JobPosition = 43;

        /// <summary>
        /// Thêm sửa xóa quản lý SBU
        /// </summary>
        public const int LOG_SBU = 44;

        /// <summary>
        /// Thêm sửa xóa quản lý nhân viên
        /// </summary>
        public const int LOG_Employee = 45;

        /// <summary>
        /// Thêm sửa xóa quản lý nhóm  nhân viên
        /// </summary>
        public const int LOG_GroupEmployee = 46;

        /// <summary>
        /// Thêm sửa xóa quản lý khóa học
        /// </summary>
        public const int LOG_Course = 47;

        /// <summary>
        /// Thêm sửa xóa quản lý kỹ năng nhân viên
        /// </summary>
        public const int LOG_WorkSkill = 48;

        /// <summary>
        /// Thêm sửa xóa vị trí công việc
        /// </summary>
        public const int LOG_WorkType = 49;

        /// <summary>
        /// Tạo cấu trúc thư mục
        /// </summary>
        public const int LOG_DesignStructure = 50;

        /// <summary>
        /// Cài đặt định nghĩa thư mục
        /// </summary>
        public const int LOG_FolderDefinition = 51;

        /// <summary>
        /// Đổi tên file scan
        /// </summary>
        public const int LOG_FileScan = 52;

        /// <summary>
        /// Phân bổ dữ liệu
        /// </summary>
        public const int LOG_DataDistribution = 53;

        /// <summary>
        /// Hồ sơ thiết kế mạch điện tử
        /// </summary>
        public const int LOG_GeneralProfileElectronicDesign = 54;

        /// <summary>
        /// Tạo file hồ sơ điện
        /// </summary>
        public const int LOG_GeneralElectronicRecordn = 55;

        /// <summary>
        /// Lập bảng tín hiệu I/O
        /// </summary>
        public const int LOG_GeneralProgramableData = 56;

        /// <summary>
        /// Quá trình thử nghiệm
        /// </summary>
        public const int LOG_GeneralTestProcess = 57;

        /// <summary>
        /// Vẽ sơ đồ thuật toán điều khiển
        /// </summary>
        public const int LOG_GeneralDrawControlAlgorithmModel = 58;


        /// <summary>
        /// Danh mục thiết bị theo chức năng
        /// </summary>
        public const int LOG_GeneralEquipmentByFunction = 59;

        /// <summary>
        /// Hạng mục thiết kế
        /// </summary>
        public const int LOG_DesignArticleElectric = 60;

        /// <summary>
        /// Danh sách hàm chức năng
        /// </summary>
        public const int LOG_GeneralListFunction = 61;


        /// <summary>
        /// Dữ liệu cài đă
        /// </summary>
        public const int LOG_GeneralDataProgramElectric = 62;

        /// <summary>
        /// Kiểm tra nguyên lý
        /// </summary>
        public const int LOG_GeneralCheckPrinciplesElectric = 63;

        /// <summary>
        /// Biểu mẫu kiểm tra nguyên lý
        /// </summary>
        public const int LOG_GeneralPrinciples = 64;

        /// <summary>
        /// Phương án thiết kế - Danh mục khối chức năng
        /// </summary>
        public const int LOG_GeneralFunctionDesignOptions = 65;


        /// <summary>
        /// Kiểm tra chất lượng sản phẩm mạch điện tử
        /// </summary>
        public const int LOG_GeneralCheckElectronic = 66;


        /// <summary>
        /// Phương án thiết kế - Mô tả chung - Sơ đồ khối
        /// </summary>
        public const int LOG_GeneralDesignOptions = 67;

        /// <summary>
        /// Biểu mẫu kiểm tra bản vẽ mạch in
        /// </summary>
        public const int LOG_GeneralElectronic = 68;

        /// <summary>
        /// Biểu mẫu bản vẽ nguyên lý - Bảng tính toán
        /// </summary>
        public const int LOG_GeneralPrinciplesCalculate = 69;

        /// <summary>
        /// Phương án thiết kế - Linh kiện chính,thông số mạch
        /// </summary>
        public const int LOG_GeneralFunctionDesignMaterial = 70;

        /// <summary>
        /// Biểu mẫu hướng dẫn lắp ráp mạch điện tử
        /// </summary>
        public const int LOG_GeneralElectronicCircuitAssembly = 71;

        /// <summary>
        /// Biểu mẫu vật tư
        /// </summary>
        public const int LOG_GeneralMaterial = 72;

        /// <summary>
        /// Tạo phác thảo thiết kế
        /// </summary>
        public const int LOG_GeneralDegignMechanical = 73;

        /// <summary>
        /// Kiểm tra phương án thiết kế
        /// </summary>
        public const int LOG_CheckDesignPlan = 74;


        /// <summary>
        /// Dự toán sơ bộ
        /// </summary>
        public const int LOG_GeneralPreliminaryEstimate = 75;

        /// <summary>
        /// Lập thông số kỹ thuật
        /// </summary>
        public const int LOG_GeneralSetUpSpecification = 76;

        /// <summary>
        /// Tạo danh mục
        /// </summary>
        public const int LOG_GeneralCheckList = 77;

        /// <summary>
        /// Download danh mục vật tư sap
        /// </summary>
        public const int LOG_GenerateMaterialSAP = 78;

        /// <summary>
        /// Thêm sửa xóa quản lý xuất giữ
        /// </summary>
        //public const int LOG_SaleProduct = 79;
        public const int LOG_SaleProductExport = 79;

        /// <summary>
        /// So sánh sai khác thư viện sản phẩm với nguồn
        /// </summary>
        public const int LOG_Compare = 81;

        /// <summary>
        /// Đồng bộ vật tư
        /// </summary>
        public const int LOG_SyncMaterial = 82;

        /// <summary>
        /// Đồng bộ module
        /// </summary>
        public const int LOG_SyncModule = 83;

        /// <summary>
        /// Đồng bộ thiết bị
        /// </summary>
        public const int LOG_SyncProduct = 84;

        /// <summary>
        /// Đồng bộ thiết bị tiêu chuẩn
        /// </summary>
        public const int LOG_SyncProductTPA = 85;

        /// <summary>
        /// Chủng loại hàng hóa
        /// </summary>
        public const int LOG_ProductStandardTPATypes = 86;

        /// <summary>
        /// Thêm sửa xóa quản lý sản phẩm kinh doanh
        /// </summary>
        public const int LOG_SaleProduct = 87;

        /// <summary>
        /// Thêm sửa xóa ứng dụng
        /// </summary>
        public const int LOG_Application = 88;

        /// <summary>
        /// Thêm sửa xóa nhóm kinh doanh
        /// </summary>
        public const int LOG_SaleGroup = 89;

        /// <summary>
        /// Thêm sửa xóa hồ sơ nhập khẩu
        /// </summary>
        public const int LOG_ImportProfileDocumentConfig = 90;

        /// <summary>
        /// Chủng loại hàng hóa kinh doanh
        /// </summary>
        public const int LOG_SaleProductTypes = 91;

        /// <summary>
        /// Hồ sơ nhập khẩu
        /// </summary>
        public const int LOG_ImportProfile = 92;

        /// <summary>
        /// Báo cáo vấn đề tồn đọng
        /// </summary>
        public const int LOG_ReportProblemExist = 93;

        /// <summary>
        /// Thêm sửa xóa quản lý lý do nghỉ việc
        /// </summary>
        public const int LOG_Reason = 94;

        /// <summary>
        /// Thêm sửa xóa quản lý ngân hàng
        /// </summary>
        public const int LOG_BankAccount = 95;

        /// <summary>
        /// Thêm sửa xóa quản lý mức đóng bảo hiểm
        /// </summary>
        public const int LOG_InsuranceLevel = 96;

        /// <summary>
        /// Thêm sửa xóa quản lý lý do thay đổi thu nhập
        /// </summary>
        public const int LOG_ReasonChangeIncome = 97;

        /// <summary>
        /// Thêm sửa xóa quản lý lý do thay đổi bảo hiểm
        /// </summary>
        public const int LOG_ReasonChangeInsurance = 99;

        /// <summary>
        /// Thêm sửa xóa quản lý nơi làm việc
        /// </summary>
        public const int LOG_WorkLocation = 100;

        /// <summary>
        /// Thêm sửa xóa hợp đồng lao động
        /// </summary>
        public const int LOG_LaborContract = 101;

        /// <summary>
        /// Thêm sửa xóa nhóm câu hỏi
        /// </summary>
        public const int LOG_QuestionGroup = 102;

        /// <summary>
        /// Thêm sửa xóa câu hỏi
        /// </summary>
        public const int LOG_Question = 103;

        /// <summary>
        /// Thêm sửa nhóm tài liệu
        /// </summary>
        public const int LOG_DocumentGroup = 104;

        /// <summary>
        /// Thêm sửa tài liệu
        /// </summary>
        public const int LOG_Document = 105;

        /// <summary>
        /// Thêm sửa loại tài liệu
        /// </summary>
        public const int LOG_DocumentType = 106;

        /// <summary>
        /// Thêm sửa loại ban hành
        /// </summary>
        public const int LOG_DocumentPromulgate = 107;

        /// <summary>
        /// Thêm sửa loại ban hành
        /// </summary>
        public const int LOG_DocumentProblem = 108;

        /// <summary>
        /// Thêm sửa dòng chảy
        /// </summary>
        public const int LOG_FlowStage = 109;

        /// <summary>
        /// Thêm sửa dòng chảy
        /// </summary>
        public const int LOG_OutputResult = 110;

        /// <summary>
        /// Thêm sửa xóa mức lương
        /// </summary>
        public const int LOG_SalaryLevel = 111;

        /// <summary>
        /// Thêm sửa xóa phỏng vấn vị trí công việc
        /// </summary>
        public const int LOG_WorkTypeInterview = 112;

        /// <summary>
        /// Thêm sửa xóa Hồ sơ ứng viên
        /// </summary>
        public const int LOG_Candidate = 113;

        /// <summary>
        /// Thêm sửa xóa Nhóm lương
        /// </summary>
        public const int LOG_SalaryGroup = 114;

        /// <summary>
        /// Thêm sửa xóa Ngạch lương
        /// </summary>
        public const int LOG_SalaryType = 115;

        /// <summary>
        /// Thêm sửa xóa Yêu cầu tuyển dụng
        /// </summary>
        public const int LOG_RecruitmentRequest = 116;

        /// <summary>
        /// Thêm sửa xóa dụng cụ khảo sát
        /// </summary>
        public const int LOG_SurveyMaterial = 117;

        /// <summary>
        /// Thêm sửa xóa nội dung khảo sát
        /// </summary>
        public const int LOG_SurveyContent = 118;

        #endregion

        #region GroupFunction
        /// <summary>
        /// Loại quyền của khach hàng cấp 2
        /// </summary>
        public const string GROUP_FUNCTION_TYPE_CUSTOME_2 = "3";
        /// <summary>
        /// Loại quyền của khach hàng cấp 1
        /// </summary>
        public const string GROUP_FUNCTION_TYPE_CUSTOME_1 = "2";
        /// <summary>
        /// Loại quyền của admin
        /// </summary>
        public const string GROUP_FUNCTION_TYPE_ADMIN = "1";






        #endregion

        #region GoodsReceipt

        /// <summary>
        /// Loại nhập kho lô hàng
        /// </summary>
        public const string GOOD_RECEIPT_TYPE_NKLH = "1";

        /// <summary>
        /// Loại nhập kho sản xuất
        /// </summary>
        public const string GOOD_RECEIPT_TYPE_PRODUCTION = "2";

        /// <summary>
        /// Loại nhập kho khác
        /// </summary>
        public const string GOOD_RECEIPT_TYPE_OTHER = "3";

        #endregion

        #region GoodsIssue

        /// <summary>
        /// Loại xuất kho sản xuất
        /// </summary>
        public const string GOOD_ISSUE_TYPE_PRODUCTION = "1";

        /// <summary>
        /// Loại xuất kho khác
        /// </summary>
        public const string GOOD_ISSUE_TYPE_OTHER = "2";

        #endregion

        #region CUSTOMER_STATUS
        /// <summary>
        /// khách hàng mở khóa
        /// </summary>
        public const string CUSTOMER_STATUS_USE = "0";

        public const int CUSTOMER_SALEPRODUCT = 1;
        #endregion

        #region GoodsReceipt
        /// <summary>
        /// Loại phiếu nhập kho
        /// </summary>
        public const string GOODSRECEIPT_TYPE_USE = "1";

        #endregion

        #region ProductionProcess
        /// <summary>
        /// Tình trạng Đang sản xuất
        /// </summary>
        public const string PRODUCTIONPROCESS_STATUS_PRODUCING = "0";
        /// <summary>
        /// Tình trạng Đã sản xuất xong
        /// </summary>
        public const string PRODUCTIONPROCESS_STATUS_COMPLETE = "1";

        #endregion

        #region Step_StepId
        /// <summary>
        /// Công đoạn sàng cắt (sieve cutter)
        /// </summary>
        public const int STEP_STEPID_SIEVECUTTER = 1;

        /// <summary>
        /// Công đoạn sàng  (sieve)
        /// </summary>
        public const int STEP_STEPID_SIEVE = 2;

        /// <summary>
        /// Công đoạn tách cẫng
        /// </summary>
        public const int STEP_STEPID_STRAINED = 3;

        /// <summary>
        /// Công đoạn đấu trộn
        /// </summary>
        public const int STEP_STEPID_MIX = 4;

        #endregion

        #region Product output
        /// <summary>
        /// Loại thành phẩm
        /// </summary>
        public const string PRODUCTOUPT_OUTPUTTYPE_FINISH = "1";

        /// <summary>
        /// Loại khác
        /// </summary>
        public const string PRODUCTOUPT_OUTPUTTYPE_OTHER = "2";
        #endregion

        #region Product type
        //Loại sản phẩm chè
        public const string Product_Type_Tea = "1";
        //Loại sản phẩm bao bì
        public const string Product_Type_Bag = "2";
        #endregion

        #region File Config Setting
        /// <summary>
        /// File cấu hình database
        /// </summary>
        public const string File_Congfig_DbConnections = "DbConnections.xml";
        /// <summary>
        /// File cấu hình ListRadar
        /// </summary>
        public const string File_Congfig_ListRadar = "ListRadar.xml";
        /// <summary>
        /// File cấu hình ListCamera
        /// </summary>
        public const string File_Congfig_ListCamera = "ListCamera.xml";
        /// <summary>
        /// File cấu hình QueueRadar
        /// </summary>
        public const string File_Congfig_RabbitQueueRadar = "RabbitQueueRadar.xml";
        /// <summary>
        /// File cấu hình QueueCamera
        /// </summary>
        public const string File_Congfig_RabbitQueueCamera = "RabbitQueueCamera.xml";
        /// <summary>
        /// File cấu hình TCP
        /// </summary>
        public const string File_Congfig_TCPSettings = "TCPSettings.xml";
        /// <summary>
        /// File cấu hình API
        /// </summary>
        public const string File_Congfig_ConnectionsAPI = "ConnectionsAPI.xml";
        /// <summary>
        /// File cấu hình Redis
        /// </summary>
        public const string File_Congfig_RedisSettings = "RedisSettings.xml";
        /// <summary>
        /// File cấu hình ServerUploadFile
        /// </summary>
        public const string File_Congfig_ServerUploadFile = "ServerUploadFile.xml";
        /// <summary>
        /// File cấu hình CleanServerFile
        /// </summary>
        public const string File_Congfig_CleanServerFile = "CleanServerFile.xml";

        /// <summary>
        /// File cấu hình Congfig_SyncNew
        /// </summary>
        public const string File_Congfig_SyncNew = "ListConfigSyncNews.xml";

        /// <summary>
        /// File cấu hình SettingSyncNews
        /// </summary>
        public const string File_SettingSyncNews = "SettingSyncNews.xml";

        /// <summary>
        /// File cấu hình File_Congfig_ServiceQueueRadar
        /// </summary>
        public const string File_Congfig_ServiceQueueRadar = "QLTK.ServiceQueueRadar.exe.config";

        /// <summary>
        /// File cấu hình File_Congfig_ServiceQueueRadar
        /// </summary>
        public const string File_Congfig_ServiceSyncNews = "QLTK.ServiceSyncNews.exe.config";
        #endregion

        //tên cache tiện ích
        public const string Forgotpassword = "QLTK:Forgotpassword";
        public const string LoginUser = "QLTK:LoginUser";
        //cấu hình mã lỗi
        public const string CodeModuleError = "CodeModuleError";
        //loại lỗi
        public const int ErrorVisual = 1;
        public const int ErrorReason = 2;
        public const int ErrorCost = 3;

        //hoạt động
        public const int Active = 1;
        public const int NotActive = 0;
        //tin tức
        public const string NewHot = "1";

        #region TimeType
        /// <summary>
        /// Ngày hôm nay
        /// </summary>
        public const string TimeType_Today = "1";

        /// <summary>
        /// Ngày qua
        /// </summary>
        public const string TimeType_Yesterday = "2";

        /// <summary>
        /// Tuần này
        /// </summary>
        public const string TimeType_ThisWeek = "3";

        /// <summary>
        /// Tuần trước
        /// </summary>
        public const string TimeType_LastWeek = "4";

        /// <summary>
        /// 7 ngày gần đây
        /// </summary>
        public const string TimeType_SevenDay = "5";

        /// <summary>
        /// Tháng này
        /// </summary>
        public const string TimeType_ThisMonth = "6";

        /// <summary>
        /// Tháng trước
        /// </summary>
        public const string TimeType_LastMonth = "7";

        /// <summary>
        /// Tháng
        /// </summary>
        public const string TimeType_Month = "8";

        /// <summary>
        /// Quý
        /// </summary>
        public const string TimeType_Quarter = "9";

        /// <summary>
        /// Năm nay
        /// </summary>
        public const string TimeType_ThisYear = "10";

        /// <summary>
        /// Năm trước
        /// </summary>
        public const string TimeType_LastYear = "11";

        /// <summary>
        /// Năm 
        /// </summary>
        public const string TimeType_Year = "12";

        /// <summary>
        /// Khoảng thời gian
        /// </summary>
        public const string TimeType_Between = "13";
        #endregion

        #region TypeFunctionDefinitionId
        /// <summary>
        ///  Loại tài liệu : cơ khí module
        /// </summary>
        public const int TypeFunctionDefinition_Mechanical = 1;

        /// <summary>
        /// Thiết kế điện module
        /// </summary>
        public const int TypeFunctionDefinition_Electricity = 2;
        /// <summary>
        /// Thiết kế điện tư Module
        /// </summary>
        public const int TypeFunctionDefinition_Electronic = 3;

        /// <summary>
        /// Thiết kế thiết bị
        /// </summary>
        public const int TypeFunctionDefinition_Product = 4;

        /// <summary>
        /// Loại đối tượng: Module
        /// </summary>
        public const int Definition_ObjectType_Module = 1;

        /// <summary>
        /// Loại đối tượng: Thiết bị
        /// </summary>
        public const int Definition_ObjectType_Product = 2;

        /// <summary>
        /// Loại đối tượng: Phòng học
        /// </summary>
        public const int Definition_ObjectType_ClassRoom = 3;

        /// <summary>
        /// Loại đối tượng: Giải pháp
        /// </summary>
        public const int Definition_ObjectType_Solution = 4;
        #endregion

        /// <summary>
        /// Webservice return
        /// </summary>
        public const int StatusCodeSuccess = 1;
        public const int StatusCodeError = 2;

        #region ProblemExist_Type
        /// <summary>
        /// Loại vấn đề: Lỗi
        /// </summary>
        public const int Error_Type_Error = 1;

        /// <summary>
        /// Loại vấn đề: Vấn đề
        /// </summary>
        public const int Error_Type_Issue = 2;
        #endregion

        public const string DatatypeNamePractice = "Bài thực hành";
        public const string DatatypeNameProduct = "Sản phẩm";
        public const string DatatypeNameParadigm = "Mô hình";
        public const string DatatypeNameModule = "Module";

        #region Project
        /// <summary>
        /// Tình trạng: Chưa bắt đầu
        /// </summary>
        public const string Prooject_Status_NotStartedYet = "1";

        /// <summary>
        /// Tình trạng: Đang triển khai
        /// </summary>
        public const string Prooject_Status_Processing = "2";

        /// <summary>
        /// Tình trạng: Hoàn thành dự án
        /// </summary>
        public const string Prooject_Status_Finish = "3";

        /// <summary>
        /// Tình trạng: Tạm dừng
        /// </summary>
        public const string Prooject_Status_Stop = "4";

        /// <summary>
        /// Tình trạng: Đang lắp đặt tại khách hàng
        /// </summary>
        public const string Prooject_Status_Setup = "5";

        /// <summary>
        /// Tình trạng: Đang xử lý vấn đề tồn đọng
        /// </summary>
        public const string Prooject_Status_Problem = "6";

        /// <summary>
        /// Chưa có tài liệu
        /// </summary>
        public const int Project_DocumentStatus_No = 1;

        /// <summary>
        /// Chưa đủ tài liệu
        /// </summary>
        public const int Project_DocumentStatus_NotEnough = 2;

        /// <summary>
        /// Đã đủ tài liệu
        /// </summary>
        public const int Project_DocumentStatus_Full = 3;
        #endregion

        #region ProjectProduct_DataType
        /// <summary>
        /// Kiểu dữ liệu: Bài thực hành
        /// </summary>
        public const int ProjectProduct_DataType_Practice = 1;

        /// <summary>
        /// Kiểu dữ liệu: Sản phẩm
        /// </summary>
        public const int ProjectProduct_DataType_ProjectProduct = 2;

        /// <summary>
        /// Kiểu dữ liệu: Mô hình
        /// </summary>
        public const int ProjectProduct_DataType_Paradigm = 3;

        /// <summary>
        /// Kiểu dữ liệu: Module
        /// </summary>
        public const int ProjectProduct_DataType_Module = 4;
        #endregion

        #region ProjectProduct_DesignStatus
        /// <summary>
        /// Tình trạng thiết kế: Thiết kế mới
        /// </summary>
        public const int ProjectProduct_DesignStatus_NewDesign = 1;

        /// <summary>
        /// Tình trạng thiết kế: Sửa thiết kế cũ
        /// </summary>
        public const int ProjectProduct_DesignStatus_UpdateDesign = 2;

        /// <summary>
        /// Tình trạng thiết kế: Tận dụng
        /// </summary>
        public const int ProjectProduct_DesignStatus_Use = 3;

        /// <summary>
        /// Tình trạng thiết kế: Hàng bán thẳng
        /// </summary>
        public const int ProjectProduct_DesignStatus_DesignStatus = 4;
        #endregion

        #region ProjectProduct_DesignStatus_String
        /// <summary>
        /// Tình trạng thiết kế: Thiết kế mới
        /// </summary>
        public const string ProjectProduct_DesignStatus_NewDesign_String = "mới";

        /// <summary>
        /// Tình trạng thiết kế: Sửa thiết kế cũ
        /// </summary>
        public const string ProjectProduct_DesignStatus_UpdateDesign_String = "cũ";

        /// <summary>
        /// Tình trạng thiết kế: Tận dụng
        /// </summary>
        public const string ProjectProduct_DesignStatus_Use_String = "tận dụng";
        #endregion

        #region Task
        /// <summary>
        /// loại thiết kế
        /// </summary>
        public const int Task_Design = 1;
        /// <summary>
        /// loại tài liệu
        /// </summary>
        public const int Task_Doc = 2;
        /// <summary>
        /// loại chuyển giao
        /// </summary>
        public const int Task_Transfer = 3;
        #endregion

        #region DataDistributionFile

        /// <summary>
        /// Số lượng nhiều file
        /// </summary>
        public const int DataDistributionFile_GetType_MultiFile = 0;

        /// <summary>
        /// Số lượng một file
        /// </summary>
        public const int DataDistributionFile_GetType_OneFile = 1;
        #endregion

        #region ModuleDesignDocuments

        /// <summary>
        /// Loại tài liệu là File
        /// </summary>
        public const string ModuleDesignDocument_FileType_File = "1";

        /// <summary>
        /// Loại tài liệu là Folder
        /// </summary>
        public const string ModuleDesignDocument_FileType_Folder = "2";

        /// <summary>
        /// Tài liệu MAT không khác nhau
        /// </summary>
        public const string ModuleDesignDocument_FileMAT_Status_None = "0";

        /// <summary>
        /// Tài liệu MAT khác tên
        /// </summary>
        public const string ModuleDesignDocument_FileMAT_Status_Name = "1";

        /// <summary>
        /// Tài liệu MAT khác size
        /// </summary>
        public const string ModuleDesignDocument_FileMAT_Status_Size = "2";

        /// <summary>
        /// Tài liệu IGS không khác nhau
        /// </summary>
        public const string ModuleDesignDocument_FileIGS_Status_None = "0";

        /// <summary>
        /// Tài liệu IGS khác tên
        /// </summary>
        public const string ModuleDesignDocument_FileIGS_Status_Name = "1";

        /// <summary>
        /// Tài liệu IGS khác size
        /// </summary>
        public const string ModuleDesignDocument_FileIGS_Status_Size = "2";
        #endregion

        /// <summary>
        /// Tình trạng module trong bảng projectproduct là Dự án
        /// </summary>
        public const int ProjectProduct_ModuleStatus_Project = 1;

        /// <summary>
        /// Tình trạng module trong bảng projectproduct là Bổ sung tính phí
        /// </summary>
        public const int ProjectProduct_ModuleStatus_Additional = 2;

        /// <summary>
        /// Tình trạng module trong bảng projectproduct là Bổ sung không tính phí
        /// </summary>
        public const int ProjectProduct_ModuleStatus_AdditionalNoPrice = 3;

        /// <summary>
        /// Trạng thái Plan: Chưa thực hiện
        /// </summary>
        public const int Plan_Status_DoNot = 1;

        /// <summary>
        /// Trạng thái Plan: Đang thực hiện
        /// </summary>
        public const int Plan_Status_Doing = 2;

        /// <summary>
        /// Trạng thái Plan: Đã hoàn thành
        /// </summary>
        public const int Plan_Status_Done = 3;

        /// <summary>
        /// Thời gian dự kiến: chưa có
        /// </summary>
        public const int Plan_StartDate_Status_No_Exist = 1;

        #region Materials
        /// <summary>
        /// Tình trạng VT: Đang sử dụng
        /// </summary>
        public const string Material_Status_Use = "0";

        /// <summary>
        /// Loại VT: VT tiêu chuẩn
        /// </summary>
        public const string Material_Type_Standard = "1";

        /// <summary>
        /// Vật tư thông số Hàn
        /// </summary>

        public const string Material_Specification_HAN = "HÀN";
        #endregion

        #region Manufactures
        /// <summary>
        /// Tình trạng HSX: Đang sử dụng
        /// </summary>
        public const string Manufacture_Status_Use = "0";

        /// <summary>
        /// Hãng sản xuất TPA
        /// </summary>
        public const string Manufacture_TPA = "TPA";

        /// <summary>
        /// Thông số TPA
        /// </summary>
        public const string Parameter_TPA = "TPA";
        #endregion

        #region Units
        /// <summary>
        /// Đơn vị KG
        /// </summary>
        public const string Unit_Kg = "KG";

        /// <summary>
        /// Đơn vị Bộ
        /// </summary>
        public const string Unit_Bo = "Bộ";
        #endregion        

        #region Employee
        /// <summary>
        /// Nv đang làm việc
        /// </summary>
        public const int Employee_Status_Use = 1;
        /// <summary>
        /// NV đã nghỉ việc
        /// </summary>
        public const int Employee_Status_Not_Use = 2;
        /// <summary>
        /// nv cơ khí
        /// </summary>
        public const int Employee_WorkType_CK = 1;
        /// <summary>
        /// nv điện
        /// </summary>
        public const int Employee_WorkType_Dn = 2;
        /// <summary>
        /// nv điện tử
        /// </summary>
        public const int Employee_WorkType_Dt = 3;
        #endregion

        #region Course Training 
        /// <summary>
        /// Chưa đào tạo
        /// </summary>
        public const int Course_Training_Status_Not_Learn = 0;
        /// <summary>
        /// Đã đào tạo
        /// </summary>
        public const int Course_Training_Status_Finsh = 1;

        /// <summary>
        /// Chưa chấm điểm
        /// </summary>
        public const int Employee_Course_Training_Status_Not_Finsh = 0;

        /// <summary>
        /// Đã chấm điểm
        /// </summary>
        public const int Employee_Course_Training_Status_Finsh = 1;
        #endregion

        #region SolutionAttach_Type
        /// <summary>
        /// 3D giải pháp
        /// </summary>
        public const int SolutionAttach_Type_Has3DSolution = 1;

        /// <summary>
        /// Bản vẽ tổng 2D
        /// </summary>
        public const int SolutionAttach_Type_Has2D = 2;

        /// <summary>
        /// Bản giải trình
        /// </summary>
        public const int SolutionAttach_Type_HasExplan = 3;

        /// <summary>
        /// DMVT
        /// </summary>
        public const int SolutionAttach_Type_HasDMVT = 4;

        /// <summary>
        /// FCM
        /// </summary>
        public const int SolutionAttach_Type_HasFCM = 5;

        /// <summary>
        /// Thông số kỹ thuật
        /// </summary>
        public const int SolutionAttach_Type_HasTSKT = 6;
        #endregion

        #region ImportProjectProduct
        /// <summary>
        /// Kiểu dữ liệu: Bài thực hành
        /// </summary>
        public const string ImportProjectProduct_DataType_Practice = "BTH";

        /// <summary>
        /// Kiểu dữ liệu: Sản phẩm
        /// </summary>
        public const string ImportProjectProduct_DataType_ProjectProduct = "SP";

        /// <summary>
        /// Kiểu dữ liệu: Mô hình
        /// </summary>
        public const string ImportProjectProduct_DataType_Paradigm = "MH";

        /// <summary>
        /// Kiểu dữ liệu: Module
        /// </summary>
        public const string ImportProjectProduct_DataType_Module = "MD";
        #endregion

        #region mobile
        public const string ResponseSuccess = "1";
        public const string ResponseError = "0";

        /// <summary>
        /// Chưa đọc
        /// </summary>
        public const string Notify_Status_UnRead = "0";

        /// <summary>
        /// Đã đọc
        /// </summary>
        public const string Notify_Status_Read = "1";
        #endregion

        #region Error_Status
        /// <summary>
        /// Đang tạo
        /// </summary>
        public const int Error_Status_Creating = 1;

        /// <summary>
        /// Đang chờ xác nhận
        /// </summary>
        public const int Error_Status_Pending = 2;

        /// <summary>
        /// Đang chờ xác nhận
        /// </summary>
        public const int Error_Status_No_Plan = 3;

        /// <summary>
        /// Đang xử lý
        /// </summary>
        public const int Error_Status_Processing = 5;

        /// <summary>
        /// Đã xử lý
        /// </summary>
        //public const int Error_Status_Processed = 4;

        /// <summary>
        /// Đóng
        /// </summary>
        public const int Error_Status_Close = 9;

        /// <summary>
        /// Đang chờ QC
        /// </summary>
        public const int Error_Status_Waiting_QC = 6;

        /// <summary>
        /// QC Đạt
        /// </summary>
        public const int Error_Status_Done_QC = 7;

        /// <summary>
        /// QC không đạt
        /// </summary>
        public const int Error_Status_Fail_QC = 8;

        /// <summary>
        /// QC không đạt
        /// </summary>
        public const int Error_Status_Overcome = 9;

        /// <summary>
        /// Đã xong
        /// </summary>
        public const int ErrorFix_Status_Finish = 2;

        #endregion

        #region ProjectProduct_ModuleStatus
        /// <summary>
        /// Dự án
        /// </summary>
        public const string ProjectProduct_ModuleStatus_Project_String = "dự án";

        /// <summary>
        /// Bổ sung
        /// </summary>
        public const string ProjectProduct_ModuleStatus_Additional_String = "bổ sung";
        #endregion

        #region Soulution
        /// <summary>
        /// Giải pháp đang triển khai
        /// </summary>
        public const int Solution_Status_Use = 1;
        /// <summary>
        /// Giáp pháp thành dự án
        /// </summary>
        public const int Solution_Status_To_Project = 2;
        /// <summary>
        /// Giải pháp không thành dự án
        /// </summary>
        public const int Solution_Status_Not_To_Project = 3;
        /// <summary>
        /// Giải pháp tạm dừng
        /// </summary>
        public const int Solution_Status_Stop = 4;
        /// <summary>
        /// Giải pháp hủy
        /// </summary>
        public const int Solution_Status_Cancel = 5;

        /// <summary>
        /// Loại đối tượng tham khảo - Module
        /// </summary>
        public const int Solution_ObjectType_Module = 1;

        /// <summary>
        /// Loại đối tượng tham khảo - Thiết bị
        /// </summary>
        public const int Solution_ObjectType_Product = 2;
        #endregion

        #region ProductDesignDocuments

        /// <summary>
        /// Loại tài liệu là File
        /// </summary>
        public const string ProductDesignDocument_FileType_File = "1";

        /// <summary>
        /// Loại tài liệu là Folder
        /// </summary>
        public const string ProductDesignDocument_FileType_Folder = "2";
        #endregion

        #region SolutionDesignDocuments

        /// <summary>
        /// Loại tài liệu là File
        /// </summary>
        public const string SolutionDesignDocument_FileType_File = "1";

        /// <summary>
        /// Loại tài liệu là Folder
        /// </summary>
        public const string SolutionDesignDocument_FileType_Folder = "2";
        #endregion

        #region Import BOM 
        /// <summary>
        /// Vật tư điện
        /// </summary>
        public const int BOM_Electric = 1;

        /// <summary>
        /// Vật tư hãng
        /// </summary>
        public const int BOM_Manufacture = 2;

        /// <summary>
        /// Vật tư TPA
        /// </summary>
        public const int BOM_TPA = 3;

        /// <summary>
        /// Bu lông
        /// </summary>
        public const int BOM_Bulong = 4;

        /// <summary>
        /// Vật tư khác
        /// </summary>
        public const int BOM_Other = 5;
        #endregion

        #region Import BOM 
        /// <summary>
        /// Vật tư điện
        /// </summary>
        public const string BOM_Electrics = "1";

        /// <summary>
        /// Vật tư hãng
        /// </summary>
        public const string BOM_Manufactures = "2";

        /// <summary>
        /// Vật tư TPA
        /// </summary>
        public const string BOM_TPAs = "3";

        /// <summary>
        /// Bu lông
        /// </summary>
        public const string BOM_Bulongs = "4";

        /// <summary>
        /// Vật tư khác
        /// </summary>
        public const string BOM_Others = "5";
        #endregion

        #region Trạng thái vấn đề tồn đọng
        /// <summary>
        /// Đang tạo
        /// </summary>
        public const int Problem_Status_Creating = 1;
        /// <summary>
        /// Đang chờ xác nhận
        /// </summary>
        public const int Problem_Status_Awaiting_Confirm = 2;

        /// <summary>
        /// Chưa có kế hoạch
        /// </summary>
        public const int Problem_Status_NoPlan = 3;
        public const int Problem_Status_Pending = 4; // đang chờ xử lý
        /// <summary>
        /// Đang xử lý
        /// </summary>
        public const int Problem_Status_Processed = 5;
        /// <summary>
        /// Đang chờ QC
        /// </summary>
        public const int Problem_Status_Awaiting_QC = 6;
        public const int Problem_Status_Ok_QC = 7; // QC đạt
        public const int Problem_Status_NotOk_QC = 8; // QC không đạt
        public const int Problem_Status_Close = 9; // đóng vấn đề
        public const int Problem_Status_Done = 10; // đã khắc phục triệt để
        #endregion

        #region Quyền phần mềm

        /// <summary>
        /// Xem thông tin nhật ký công việc của người khác
        /// </summary>
        public const string Permission_Code_F080805 = "F080805";

        /// <summary>
        /// Xuất danh sách nhật ký công việc của người khác
        /// </summary>
        public const string Permission_Code_F080807 = "F080807";

        /// <summary>
        /// Xem thông tin module phòng ban khác
        /// </summary>
        public const string Permission_Code_F020105 = "F020105";

        /// <summary>
        /// Xuất thông tin dự án của SBU khác
        /// </summary>
        public const string Permission_Code_F060005 = "F060005";

        /// <summary>
        /// Xuất thông tin phòng ban khác
        /// </summary>
        public const string Permission_Code_F030000 = "F030000";

        /// <summary>
        /// Xem thời gian làm việc của phòng ban khác
        /// </summary>
        public const string Permission_Code_F060802 = "F060802";

        /// <summary>
        /// Xem tổng hợp kế hoạch thiết kế của SBU khác
        /// </summary>
        public const string Permission_Code_F060902 = "F060902";

        /// <summary>
        /// Xem Doashboard nhân sự của Phòng ban khác
        /// </summary>
        public const string Permission_Code_F061002 = "F061002";

        /// <summary>
        /// Xem Dashboard dự án của SBU khác
        /// </summary>
        public const string Permission_Code_F061102 = "F061102";

        /// <summary>
        /// Xem thông tin kế hoạch của phòng ban khác
        /// </summary>
        public const string Permission_Code_F060705 = "F060705";

        /// <summary>
        /// Xem công viêc của phòng ban
        /// </summary>
        public const string Permission_Code_F110707 = "F110707";

        /// <summary>
        /// Xem công viêc của tất cả phòng ban
        /// </summary>
        public const string Permission_Code_F110708 = "F110708";

        /// <summary>
        /// Xem thông tin kế hoạch của phòng ban khác
        /// </summary>
        public const string Permission_Code_F110702 = "F110702";

        /// <summary>
        /// Xem Thiết bị phòng ban khác
        /// </summary>
        public const string Permission_Code_F030405 = "F030405";
        /// <summary>
        /// Xem thông tin khách hàng của sbu khác
        /// </summary>
        public const string Permission_Code_F060205 = "F060205";
        /// <summary>
        /// Xem thông tin tất cả các lỗi
        /// </summary>
        public const string Permission_Code_F060431 = "F060431";
        /// <summary>
        /// Xem thông tin giải pháp của Phòng ban khác
        /// </summary>
        public const string Permission_Code_F070105 = "F070105";

        /// <summary>
        /// Xem thông tin cài đặt định nghĩa thư mục phòng ban khác
        /// </summary>
        public const string Permission_Code_F090608 = "F090608";

        /// <summary>
        /// Xem thông tin nhóm quyền của phòng ban khác
        /// </summary>
        public const string Permission_Code_F080905 = "F080905";

        /// <summary>
        /// Thêm nhóm quyền của phòng ban khác
        /// </summary>
        public const string Permission_Code_F080906 = "F080906";

        /// <summary>
        /// Chỉnh sửa nhóm quyền của phòng ban khác
        /// </summary>
        public const string Permission_Code_F080907 = "F080907";

        /// <summary>
        /// Xóa nhóm quyền của phòng ban khác
        /// </summary>
        public const string Permission_Code_F080908 = "F080908";

        /// <summary>
        /// Download tài liệu tiết kế của SBU khác
        /// </summary>
        public const string Permission_Code_F090902 = "F090902";

        /// <summary>
        /// Xem cấu trúc thư mục phòng ban khác
        /// </summary>
        public const string Permission_Code_F090006 = "F090006";

        /// <summary>
        /// Xem thời gian làm việc của nhân viên khác
        /// </summary>
        public const string Permission_Code_F110102 = "F110102";

        /// <summary>
        /// Xem thời gian làm việc của sbu khác
        /// </summary>
        public const string Permission_Code_F110103 = "F110103";


        /// <summary>
        /// Sửa công đoạn phòng ban khác
        /// </summary>
        public const string Permission_Code_F110305 = "F110305";

        /// <summary>
        /// Xóa công đoạn phòng ban khác
        /// </summary>
        public const string Permission_Code_F110306 = "F110306";

        /// <summary>
        /// Xem công đoạn phòng ban khác
        /// </summary>
        public const string Permission_Code_F110307 = "F110307";

        /// <summary>
        /// Xem lịch sử phòng ban khác
        /// </summary>
        public const string Permission_Code_F110502 = "F110502";

        /// <summary>
        /// Thêm mới nhật ký công việc khi quá hạn
        /// </summary>
        public const string Permission_Code_F080809 = "F080809";

        /// <summary>
        /// Chỉnh sửa nhật ký công việc khi quá hạn
        /// </summary>
        public const string Permission_Code_F080810 = "F080810";

        /// <summary>
        /// Xóa nhật ký công việc khi quá hạn
        /// </summary>
        public const string Permission_Code_F080811 = "F080811";

        /// <summary>
        /// Cập nhật thông tin giá TPA
        /// </summary>
        public const string Permission_Code_F110811 = "F110811";

        /// <summary>
        /// Tìm kiếm sản phẩm kinh doanh
        /// </summary>
        public const string Permission_Code_F120000 = "F120000";
        public const string Permission_Code_View_ALl_F120007 = "F120007";

        /// <summary>
        /// Xem vật tư mua của người khác
        /// </summary>
        public const string Permission_Code_F120753 = "F120753";

        /// <summary>
        /// Xem hồ sơ mua của người khác
        /// </summary>
        public const string Permission_Code_F120804 = "F120804";

        /// <summary>
        /// Tìm kiếm xuất giữ của người khác
        /// </summary>
        public const string Permission_Code_F120120 = "F120120";

        /// <summary>
        /// Xem chi tiết lịch sử xuất giữ của người khác
        /// </summary>
        public const string Permission_Code_F120202 = "F120202";

        /// <summary>
        /// Chỉnh sửa xuất giữ của người khác
        /// </summary>
        public const string Permission_Code_F120106 = "F120106";

        /// <summary>
        /// Xóa xuất giữ của người khác
        /// </summary>
        public const string Permission_Code_F120107 = "F120107";

        /// <summary>
        /// Xem tài liệu của phòng ban khác
        /// </summary>
        public const string Permission_Code_F121353 = "F121353";

        /// <summary>
        /// Download tài liệu của phòng ban khác
        /// </summary>
        public const string Permission_Code_F121354 = "F121354";
        #endregion

        #region Thiết bị phụ trợ BTH
        /// <summary>
        /// Thiết bị phụ trợ là vật tư
        /// </summary>
        public const int PracticeSupMaterial_Type_Material = 1;

        /// <summary>
        /// Thiết bị phụ trợ là module
        /// </summary>
        public const int PracticeSupMaterial_Type_Module = 2;
        #endregion

        #region Vật tư phụ tổng hợp thiết kế
        /// <summary>
        /// Vật tư phụ là vật tư
        /// </summary>
        public const int ProjectGaneralDesignMaterial_Type_Material = 1;

        /// <summary>
        /// Vật tư phụ là module
        /// </summary>
        public const int ProjectGaneralDesignMaterial_Type_Module = 2;

        /// <summary>
        /// Vât tự phụ là module hàng bán thẳng
        /// </summary>
        public const int ProjectGaneralDesignMaterial_Type_Module_DirectSales = 3;
        #endregion

        #region View Detail Design
        /// <summary>
        /// Công việc chưa có kế hoạch theo module 
        /// </summary>
        public const int ViewDesign_Module_TaskNotPlan = 1;

        /// <summary>
        /// Công việc chưa có kế hoạch theo mô hình
        /// </summary>
        public const int ViewDesign_Paradigm_TaskNotPlan = 2;

        /// <summary>
        /// Công việc hoàn thành theo module 
        /// </summary>
        public const int ViewDesign_Module_finish = 3;

        /// <summary>
        /// Công việc chưa(đang) hoàn thành theo module
        /// </summary>
        public const int ViewDesign_Module_makeDesign = 4;

        /// <summary>
        /// Công việc hoàn thành theo mô hình
        /// </summary>
        public const int ViewDesign_Paradigm_finish = 5;

        /// <summary>
        /// Công việc chưa(đang) hoàn thành theo mô hình
        /// </summary>
        public const int ViewDesign_Paradigm_makeDesign = 6;

        /// <summary>
        /// Công việc chậm theo module
        /// </summary>
        public const int ViewDesign_Module_delay = 7;

        /// <summary>
        /// Công việc chậm theo mô hình
        /// </summary>
        public const int ViewDesign_Paradigm_delay = 8;

        /// <summary>
        /// công việc đến deadline kickoff nhưng chưa hoàn thành của thiết kế
        /// </summary>
        public const int ViewDesign_Task_Design_delay = 9;
        #endregion

        #region View Detail Document
        /// <summary>
        /// Công việc chưa có kế hoạch - Module
        /// </summary>
        public const int ViewDocumnet_Module_TaskNotPlan_Doc = 1;

        /// <summary>
        /// Công việc chư có kế hoạch - Mô hình
        /// </summary>
        public const int ViewDocumnet_Paradigm_TaskNotPlan_Doc = 2;

        /// <summary>
        /// Công việc hoàn thành theo module (Tài liệu)
        /// </summary>
        public const int ViewDocumnet_Module_finish_Doc = 3;

        /// <summary>
        ///  Công việc chưa(đang) hoàn thành theo module (Tài liệu)
        /// </summary>
        public const int ViewDocumnet_Module_makeDesign_Doc = 4;

        /// <summary>
        /// Công việc hoàn thành theo mô hình (Tài liệu)
        /// </summary>
        public const int ViewDocumnet_Paradigm_finish_Doc = 5;

        /// <summary>
        /// Công việc chưa(đang) hoàn thành theo mô hình (Tài liệu)
        /// </summary>
        public const int ViewDocumnet_Paradigm_makeDesign_Doc = 6;

        /// <summary>
        /// Công việc chậm theo module
        /// </summary>
        public const int ViewDocumnet_Module_delay_Doc = 7;

        /// <summary>
        ///  Công việc chậm theo mô hình
        /// </summary>
        public const int ViewDocumnet_Paradigm_delay_Doc = 8;

        /// <summary>
        /// công việc đến deadline kickoff nhưng chưa hoàn thành của tài liệu
        /// </summary>
        public const int ViewDocumnet_Total_task_Doc_delay = 9;
        #endregion

        #region View Detail Tranfer
        /// <summary>
        /// Công việc chưa có kế hoạch
        /// </summary>
        public const int ViewTranfer_ProjectIsNotPlan_Transfer = 1;

        /// <summary>
        /// Công việc đến deadline kickoff nhưng chưa hoàn thành của chuyển giao
        /// </summary>
        public const int ViewTranfer_Task_Transfer_delay = 2;

        #endregion

        #region Nhật ký công việc
        public const int WorkDiary_ObjectType_None = 0;

        /// <summary>
        /// Loại đối tượng: Kế hoạch công việc
        /// </summary>
        public const int WorkDiary_ObjectType_Plan = 1;
        #endregion

        #region
        /// <summary>
        /// Tình trạng phê duyệt: Chưa duyệt
        /// </summary>
        public const int ProjectGeneralDesigns_ApproveStatus_NotApproved = 0;

        /// <summary>
        /// Tình trạng phê duyệt: Đã duyệt
        /// </summary>
        public const int ProjectGeneralDesigns_ApproveStatus_Approved = 1;
        #endregion

        #region Cấu hình
        /// <summary>
        /// Cấu hình: số ngày mua
        /// </summary>
        public const string Config_Material_LastBuy = "VATTU_GIALICHSU_SONGAY";

        /// <summary>
        /// Cấu hình: Hạn nhập nhật ký công việc
        /// </summary>
        public const string Config_WorkDiary_Day = "NHATKY_HANNHAP_NGAY";
        #endregion

        #region
        /// <summary>
        /// Loại lịch sử: Module
        /// </summary>
        public const int HistoryVersion_Type_Module = 1;

        /// <summary>
        /// Loại lịch sử: Thiết bị
        /// </summary>
        public const int HistoryVersion_Type_Product = 2;

        /// <summary>
        /// Loại lịch sử: Bài thực hành
        /// </summary>
        public const int HistoryVersion_Type_Practice = 3;
        #endregion

        #region Folder Definition
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int FolderDefinitionBetween_Group = 1;

        /// <summary>
        /// Cấu hình theo mã đối tương
        /// </summary>
        public const int FolderDefinitionBetween_Object = 2;

        /// <summary>
        /// Cấu hình theo mã nhóm cha
        /// </summary>
        public const int FolderDefinitionBetween_GroupParent = 3;

        /// <summary>
        /// Cấu hình theo mã module nguồn
        /// </summary>
        public const int FolderDefinitionBetween_Module = 5;

        #endregion

        #region Product File Type
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_FileAttach = 0;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_GuidePractive = 1;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_Quotation = 2;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_DrawingLayout = 3;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_DMVT = 4;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_DMBTH = 5;
        /// <summary>
        /// Cấu hình theo mã nhóm
        /// </summary>
        public const int ProductDocumentAttach_GuideMaintenance = 6;
        #endregion

        #region Upload
        /// <summary>
        /// Ảnh giải pháp: Upload thường
        /// </summary>
        public const int SolutionImage_Type_Image = 1;

        /// <summary>
        /// Ảnh giải pháp: Upload tài liệu thiết kế
        /// </summary>
        public const int SolutionImage_Type_Thumbnail = 2;
        #endregion

        #region
        public const int ProductStandardTPAFile_Type_Catolog = 1;

        public const int ProductStandardTPAFile_Type_File = 2;

        public const int ProductStandardTPAFile_Type_COCQ = 3;
        #endregion

        #region ProductStandardTPA
        /// <summary>
        /// Chủng loại hàng hóa: linh kiện
        /// </summary>
        public const string ProductStandardTPA_TypeMerchandise_Accessories = "Linh kiện";
        public const int ProductStandardTPA_TypeMerchandise_AccessoriesInt = 1;

        /// <summary>
        /// Chủng loại hàng hóa: máy
        /// </summary>
        public const string ProductStandardTPA_TypeMerchandise_Machine = "Máy";
        public const int ProductStandardTPA_TypeMerchandise_MachineInt = 2;

        /// <summary>
        /// Loại tiền tệ: USD
        /// </summary>
        public const string ProductStandardTPA_Currency_USD = "USD";
        public const int ProductStandardTPA_Currency_USDInt = 1;

        /// <summary>
        /// Loại tiền tệ: EUR
        /// </summary>
        public const string ProductStandardTPA_Currency_EUR = "EUR";
        public const int ProductStandardTPA_Currency_EURInt = 2;

        /// <summary>
        /// Loại tiền tệ: CNY
        /// </summary>
        public const string ProductStandardTPA_Currency_CNY = "CNY";
        public const int ProductStandardTPA_Currency_CNYInt = 3;
        #endregion

        #region Module
        /// <summary>
        /// Module đang ngừng sử dụng
        /// </summary>
        public const int Module_Status_Not_Use = 3;

        /// <summary>
        /// Module chỉ dùng 1 lần
        /// </summary>
        public const int Module_Status_Use_One = 1;
        #endregion

        #region saleproduct

        /// <summary>
        /// Sản phẩm kinh doanh thiết bị
        /// </summary>
        public const int SaleProductDevice = 1;

        /// <summary>
        /// Sản phẩm kinh doanh module
        /// </summary>
        public const int SaleProductModule = 2;

        /// <summary>
        /// Sản phẩm kinh doanh vật tư
        /// </summary>
        public const int SaleProductMaterial = 3;

        /// <summary>
        /// Sản phẩm kinh doanh THIẾT BỊ TIÊU CHUẨN
        /// </summary>
        public const int SaleProductStandTPA = 4;

        /// <summary>
        /// Sản phẩm kinh doanh THIẾT BỊ TIÊU CHUẨN
        /// </summary>
        public const bool SaleProduct_Status_UnLock = true;

        #endregion

        #region Xuất giữ
        /// <summary>
        /// Trạng thái đã bán
        /// </summary>
        public const int SaleProductExport_Status_daban = 1;

        /// <summary>
        /// Trạng thái đang giữ
        /// </summary>
        public const int SaleProductExport_Status_danggiu = 2;

        /// <summary>
        /// Trạng thái trả về kho
        /// </summary>
        public const int SaleProductExport_Status_travekho = 3;

        /// <summary>
        /// Trạng thái quá hạn
        /// </summary>
        public const int SaleProductExport_Status_quahan = 4;
        #endregion

        #region SaleProductExport
        /// <summary>
        /// Tình trạng thanh toán: Chưa thanh toán
        /// </summary>
        public const int SaleProductExport_PayStatus_Unpaid = 1;

        /// <summary>
        /// Tình trạng thanh toán: Đã thanh toán
        /// </summary>
        public const int SaleProductExport_PayStatus_Paid = 2;
        #endregion

        #region ImportInventory_Type
        public const string ImportInventory_Type_SaleProduct = "SaleProduct";
        public const string ImportInventory_Type_LastModified = "LastModified";
        #endregion

        #region SyncSaleProduct_Type
        public const string SyncSaleProduct_Type_SaleProduct = "Product";

        public const string SyncSaleProduct_Type_SaleProductStandardTPA = "SaleProductStandardTPA";

        public const string SyncSaleProduct_Type_SaleProductModule = "Module";

        public const string SyncSaleProduct_Type_SaleProductMaterial = "Material";
        #endregion

        #region Sync
        public const string ManufactureId = "63515c04-d1fb-40b9-aa2f-edfb22066594";

        public const string CountryName = "Việt Nam";
        #endregion

        #region SaleProductMedia
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public const int SaleProductMedia_Type_Image = 1;

        /// <summary>
        /// Thư viện ảnh
        /// </summary>
        public const int SaleProductMedia_Type_LibraryImage = 2;

        #endregion

        #region SaleProductDocument_Type
        /// <summary>
        /// Catalog
        /// </summary>
        public const int SaleProductDocument_Type_Solution = 1;

        /// <summary>
        /// Catalog
        /// </summary>
        public const int SaleProductDocument_Type_Catalog = 2;

        /// <summary>
        /// Catalog
        /// </summary>
        public const int SaleProductDocument_Type_TechnicalTraining = 3;

        /// <summary>
        /// Tài liệu đào tạo kỹ thuật
        /// </summary>
        public const int SaleProductDocument_Type_SaleTraining = 4;

        /// <summary>
        /// Catalog
        /// </summary>
        public const int SaleProductDocument_Type_UserManual = 5;

        /// <summary>
        /// Tài liệu hướng dẫn thực hành
        /// </summary>
        public const int SaleProductDocument_Type_Fixbug = 6;
        #endregion

        #region DocumentTemplate
        #endregion

        #region Import Profile
        /// <summary>
        /// Xác định nhà cung cấp
        /// </summary>
        public const int ImportProfile_Step_ConfirmSupplier = 1;

        /// <summary>
        /// Làm HĐ
        /// </summary>
        public const int ImportProfile_Step_Contract = 2;

        /// <summary>
        /// Thanh toán
        /// </summary>
        public const int ImportProfile_Step_Payment = 3;

        /// <summary>
        /// Tiến độ sản xuất
        /// </summary>
        public const int ImportProfile_Step_Production = 4;

        /// <summary>
        /// Vận chuyển
        /// </summary>
        public const int ImportProfile_Step_Transport = 5;

        /// <summary>
        /// Hải quan
        /// </summary>
        public const int ImportProfile_Step_Customs = 6;

        /// <summary>
        /// Nhập kho
        /// </summary>
        public const int ImportProfile_Step_Import = 7;

        /// <summary>
        /// Kết thúc
        /// </summary>
        public const int ImportProfile_Step_Finish = 8;

        /// <summary>
        /// Chưa xác định
        /// </summary>
        public const int ImportProfile_Status_None = 0;

        /// <summary>
        /// Đúng tiến độ
        /// </summary>
        public const int ImportProfile_Status_Ongoing = 1;

        /// <summary>
        /// Chậm tiến độ
        /// </summary>
        public const int ImportProfile_Status_Slow = 2;

        /// <summary>
        /// Chưa kết thúc
        /// </summary>
        public const int ImportProfile_WorkStatus_UnFinish = 0;

        /// <summary>
        /// Đã Chưa kết thúc
        /// </summary>
        public const int ImportProfile_WorkStatus_Finish = 1;

        /// <summary>
        /// Chưa thanh toán
        /// </summary>
        public const int ImportProfilePayment_Status_UnPay = 1;

        /// <summary>
        /// Đã thanh toán
        /// </summary>
        public const int ImportProfilePayment_Status_Pay = 2;

        /// <summary>
        /// Cảnh báo thanh toán
        /// </summary>
        public const int ImportProfilePayment_Status_Warning = 3;

        /// <summary>
        /// Cảnh báo thanh toán
        /// </summary>
        public const int ImportProfilePayment_Status_Expired = 4;

        /// <summary>
        /// Đúng tiến độ
        /// </summary>
        public const int ImportProfilePayment_StepStatus_Ongoing = 1;

        /// <summary>
        /// Chậm tiến độ
        /// </summary>
        public const int ImportProfilePayment_StepStatus_Slow = 2;
        #endregion

        #region TPA info
        public const string TPAName = "CÔNG TY CỔ PHẦN TỰ ĐỘNG HÓA TÂN PHÁT";

        public const string PhoneNumber = "024 3685 7776";

        public const string Address = "Số 189 Đường Phan Trọng Tuệ, Xã Thanh Liệt, H.Thanh Trì, TP. Hà Nội";
        #endregion

        #region ReportProblem
        public const int ReportProblem_Status_Processed = 1;
        public const int ReportProblem_Status_NoProcessed = 2;

        public const int CurrencyUnit_VNĐ = 3;
        #endregion

        #region Tài liệu

        /// <summary>
        /// Thiết bị
        /// </summary>
        public const int ObjectType_Devide = 1;

        /// <summary>
        /// Module
        /// </summary>
        public const int ObjectType_Module = 2;

        /// <summary>
        /// Công việc
        /// </summary>
        public const int ObjectType_Work = 3;

        /// <summary>
        /// Vị trí công việc
        /// </summary>
        public const int ObjectType_WorkType = 4;

        /// <summary>
        /// Phòng ban
        /// </summary>
        public const int ObjectType_Department = 5;

        /// <summary>
        /// Bài thực hành
        /// </summary>
        public const int ObjectType_Practice = 6;

        /// <summary>
        /// Thiết bị nhập khẩu
        /// </summary>
        public const int ObjectType_Device_Imported = 6;
        #endregion

        #region Project Attach

        /// <summary>
        /// Loại tài liệu: Pháp lý
        /// </summary>
        public const int ProjectAttach_Type_Juridical = 1;

        /// <summary>
        ///  Loại tài liệu: Kỹ thuật
        /// </summary>
        public const int ProjectAttach_Type_Technical = 2;

        /// <summary>
        ///  Loại tài liệu: Khác
        /// </summary>
        public const int ProjectAttach_Type_Other = 3;

        /// <summary>
        /// Loại ban hành: Khách hàng
        /// </summary>
        public const int ProjectAttach_PromulgateType_Customer = 1;

        /// <summary>
        /// Loại ban hành: Nhà cung cấp
        /// </summary>
        public const int ProjectAttach_PromulgateType_Supplier = 2;
        #endregion

        #region Documents
        /// <summary>
        /// Trạng thái tài liệu: Đang sử dụng
        /// </summary>
        public const int Document_Status_Used = 1;

        /// <summary>
        /// Trạng thái tài liệu: Hủy
        /// </summary>
        public const int Document_Status_Cancel = 2;

        /// <summary>
        /// Trạng thái tài liệu: Đang review
        /// </summary>
        public const int Document_Status_Review = 3;
        #endregion

        #region  DocumentGroup
        /// <summary>
        /// Mã nhóm tài liệu: Hướng dẫn thực hành
        /// </summary>
        public const string DocumentGroup_Code_Product_GuidePractive = "D01.02.02";

        /// <summary>
        /// Mã nhóm tài liệu: Danh mục bài thực hành
        /// </summary>
        public const string DocumentGroup_Code_Product_DMBTH = "D01.02.03";

        /// <summary>
        /// Mã nhóm tài liệu: Tài liệu hướng dẫn bảo trì
        /// </summary>
        public const string DocumentGroup_Code_Product_GuideMaintenance = "D01.02.04";

        /// <summary>
        /// Mã nhóm tài liệu: Tài liệu Catelog
        /// </summary>
        public const string DocumentGroup_Code_Product_Catelog = "D01.02.05";

        /// <summary>
        /// Tài liệu hướng dẫn sử dụng
        /// </summary>
        public const string DocumentGroup_Code_Module_DocumentHDSD = "D01.02.01";
        #endregion

        #region  DocumentGroup
        /// <summary>
        /// Tinh trang ho so: Đạt
        /// </summary>
        public const int CandidateApply_ProfileStatus = 2;


        #endregion

        #region Hợp đồng
        /// <summary>
        /// Hợp đồng nguyên tắc
        /// </summary>
        public const int Contract_Rule = 2;

        /// <summary>
        /// Hợp đồng lao động
        /// </summary>
        public const int Contract_Labor = 1;
        #endregion

        #region Departments
        /// <summary>
        ///Tình trạng sử dụng
        /// </summary>
        public const string Department_Status_Use = "0";
        #endregion

        #region Projects

        /// <summary>
        /// Dự án đã hoàn thành
        /// </summary>
        public const string Project_Status_Finish = "3";
        #endregion

        #region Customer Requirement
        /// <summary>
        /// Đang phân tích
        /// </summary>
        public const string CustomerRequirement_Status_analysis = "1";

        /// <summary>
        /// Đang phân tích
        /// </summary>
        public const string CustomerRequirement_Status_survey = "2";

        /// <summary>
        /// Đang làm giải pháp kỹ thuật
        /// </summary>
        public const string CustomerRequirement_Status_solution = "3";

        /// <summary>
        /// Đang lập dự toán
        /// </summary>
        public const string CustomerRequirement_Status_estimates = "4";

        /// <summary>
        /// chốt giải pháp
        /// </summary>
        public const string CustomerRequirement_Status_agree = "5";

        /// <summary>
        /// Hủy giải pháp
        /// </summary>
        public const string CustomerRequirement_Status_cancel = "6";

        /// <summary>
        /// Thêm sửa xóa Yêu cầu khach hang
        /// </summary>
        public const int CustomerRequirement_Status = 7;

        /// <summary>
        /// Thêm sửa xóa khảo sát
        /// </summary>
        public const int Survey_Status = 8;

        #endregion

        #region Customer Requirement Attach

        /// <summary>
        /// Thông tin sản phẩm
        /// </summary>
        public const string CustomerRequirementAyyach_Type_Product = "1";

        /// <summary>
        /// Yêu cầu tốc độ đầu ra 
        /// </summary>
        public const string CustomerRequirementAyyach_Type_outputSpeed = "2";

        /// <summary>
        /// Quy trình công nghệ
        /// </summary>
        public const string CustomerRequirementAyyach_Type_process = "3";

        /// <summary>
        /// Phạm vi cung cấp
        /// </summary>
        public const string CustomerRequirementAyyach_Type_Limit = "4";

        /// <summary>
        /// Timeline triển khai
        /// </summary>
        public const string CustomerRequirementAyyach_Type_Timeline = "5";

        /// <summary>
        /// Budget
        /// </summary>
        public const string CustomerRequirementAyyach_Type_Budget = "6";

        /// <summary>
        /// Yêu cầu về vât tư sử dụng trong máy
        /// </summary>
        public const string CustomerRequirementAyyach_Type_MaterialRequirement = "7";

        /// <summary>
        /// Thời gian chạy đánh giá nghiẹm thu
        /// </summary>
        public const string CustomerRequirementAyyach_Type_EvaluationTime = "8";

        /// <summary>
        /// Tiêu chuẩn đánh giá chất lượng sản phẩm
        /// </summary>
        public const string CustomerRequirementAyyach_Type_ = "9";

        /// <summary>
        /// Yêu cầu về an toàn
        /// </summary>
        public const string CustomerRequirementAyyach_Type_safetyRequirement = "10";

        /// <summary>
        /// Tiêu chuẩn thiết kế máy
        /// </summary>
        public const string CustomerRequirementAyyach_Type_designStandard = "11";

        /// <summary>
        /// yêu cầu về thu thập và quản lý dữ liẹu, quản lý vận hành 
        /// </summary>
        public const string CustomerRequirementAyyach_Type_dataManagement = "12";

        /// <summary>
        /// Layout lắp đặt
        /// </summary>
        public const string CustomerRequirementAyyach_Type_Layout = "13";




        #endregion

        #region Meeting
        public const int Meeting_Status_NoPlan = 1;
        public const int Meeting_Status_HasPlan = 2;
        public const int Meeting_Status_Cancel = 3;
        public const int Meeting_Status_Finish = 4;

        public const int Meeting_Step_Create = 0;
        public const int Meeting_Step_Confirm = 1;
        public const int Meeting_Step_DoMeeting = 2;
        public const int Meeting_Step_Finish = 3;
        #endregion


        #region ProjectPhase
        /// <summary>
        /// Thêm sửa xóa giai đoạn dự án
        /// </summary>
        public const int LOG_ProjectPhase = 1;
        #endregion

        #region Plan Type
        public const int Plan_Type_Project = 1;
        public const int Plan_Type_Additional = 2;
        public const int Plan_Type_AdditionalNoPrice = 3;
        #endregion

        public enum ScheduleStatus
        {
            Open = 1,
            Ongoing = 2,
            Closed = 3,
            Stop = 4,
            Cancel = 5,
        };

        public enum PLanType
        {
            Plan = 1,
            Error = 2,
            Quotation = 3,
        };
    }
}
