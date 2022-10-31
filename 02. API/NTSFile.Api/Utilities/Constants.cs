using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NTSFile.Api
{
    public class Constants
    {
        /// <summary>
        /// Chuỗi connect database
        /// </summary>
        public static string ENERGY_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["QLTKConnectionString"].ConnectionString;

        /// <summary>
        /// Thư mục export
        /// </summary>
        public const string FolderExport = "Export/";

        /// <summary>
        /// 
        /// </summary>
        public const string FolderImageCustomer = "ImageCustomer";
        /// <summary>
        /// Format ngày tháng YYYY-MM-DD
        /// </summary>
        public const string DATE_FORMAT_YYYYMMDD = "yyyy-MM-dd";

        /// <summary>
        /// Format ngày tháng DD/MM/YYYY
        /// </summary>
        public const string DATE_FORMAT_DDMMYYYY = "dd/MM/yyyy";

        /// <summary>
        /// Format ngày tháng DD/MM/YYYY HH:mm
        /// </summary>
        public const string DATE_FORMAT_DDMMYYYY_HHMM = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// Format ngày tháng DD/MM
        /// </summary>
        public const string DATE_FORMAT_DDMM = "dd/MM";

        /// <summary>
        /// Tình trạng xe còn trong bãi
        /// </summary>
        public const string PARKING_STATUS_IN = "0";

        /// <summary>
        /// Tình trạng xe đã ra
        /// </summary>
        public const string PARKING_STATUS_OUT = "1";

        /// <summary>
        /// Loại vé ngày
        /// </summary>
        public const string PARKING_TICKET_DAY = "1";

        /// <summary>
        /// Loại vé tháng
        /// </summary>
        public const string PARKING_TICKET_MONTH = "2";

        /// <summary>
        /// Kiểu thống kê theo ngày hiện tại
        /// </summary>
        public const bool PARKING_STATISTICS_DAY = true;

        /// <summary>
        /// Kiểu thống kê theo 7 ngày gần đây
        /// </summary>
        public const bool PARKING_STATISTICS_7_DAY = false;

        /// <summary>
        /// Tình trạng khu vực đã có xe
        /// </summary>
        public const string PARKING_AREA_VEHICLE_STATUS_IN = "1";

        /// <summary>
        /// Tình trạng khu vực chưa có xe
        /// </summary>
        public const string PARKING_AREA_VEHICLE_STATUS_OUT = "0";

        /// <summary>
        /// Tình trạng đặt vé chưa xác nhận
        /// </summary>
        public const string PARKING_BOOKING_STATUS_NOT_VERIFIED = "0";

        /// <summary>
        /// Tình trạng đặt vé đã xác nhận
        /// </summary>
        public const string PARKING_BOOKING_STATUS_VERIFIED = "1";

        /// <summary>
        /// Tình trạng đặt vé đã đến
        /// </summary>
        public const string PARKING_BOOKING_STATUS_ARRIVED = "2";

        /// <summary>
        /// Tình trạng đặt vé không đến
        /// </summary>
        public const string PARKING_BOOKING_STATUS_NOT_ARRIVE = "3";

        /// <summary>
        /// Tình trạng đặt vé không đến
        /// </summary>
        public const string PARKING_BOOKING_STATUS_CANCEL = "4";

        /// <summary>
        /// Tình trạng đồng bộ đặt vé Không làm gì
        /// </summary>
        public const string PARKING_BOOKING_SYNC_STATUS_DO_NOTHING = "0";

        /// <summary>
        /// Tình trạng đồng bộ đặt vé Thêm mới
        /// </summary>
        public const string PARKING_BOOKING_SYNC_STATUS_CREATE = "1";

        /// <summary>
        /// Tình trạng đồng bộ đặt vé Chỉnh sửa
        /// </summary>
        public const string PARKING_BOOKING_SYNC_STATUS_EDIT = "2";

        /// <summary>
        /// Tình trạng đồng bộ đặt vé Xóa
        /// </summary>
        public const string PARKING_BOOKING_SYNC_STATUS_DELETE = "3";

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

        #region TICKET REGISTER

        /// <summary>
        /// Tình trạng đăng ký vé tháng: Chưa xác nhận
        /// </summary>
        public const string TICKETREGISTER_STATUS_UNCONFIRM = "0";

        /// <summary>
        /// Tình trạng đăng ký vé tháng: Đã xác nhận
        /// </summary>
        public const string TICKETREGISTER_STATUS_CONFIRM = "1";

        /// <summary>
        /// Tình trạng đăng ký vé tháng: Đã thanh toán
        /// </summary>
        public const string TICKETREGISTER_STATUS_PAY = "2";

        /// <summary>
        /// Tình trạng đăng ký vé tháng: Không mua nữa
        /// </summary>
        public const string TICKETREGISTER_STATUS_NOPAY = "3";

        /// <summary>
        /// Tình trạng đăng ký vé tháng: Hủy đăng ký
        /// </summary>
        public const string TICKETREGISTER_STATUS_CANCEL = "4";

        #endregion

        #region SYNC STATUS

        /// <summary>
        /// Tình trạng đồng bộ: Không làm gì
        /// </summary>
        public const string SYNC_STATUS_NORMAL = "0";

        /// <summary>
        ///  Tình trạng đồng bộ: Thêm mới
        /// </summary>
        public const string SYNC_STATUS_NEW = "1";

        /// <summary>
        ///  Tình trạng đồng bộ: Chỉnh sửa
        /// </summary>
        public const string SYNC_STATUS_EDIT = "2";

        /// <summary>
        ///  Tình trạng đồng bộ: Xóa
        /// </summary>
        public const string SYNC_STATUS_DELETE = "3";

        #endregion

        /// <summary>
        /// Tình trạng khóa
        /// </summary>
        public const string STATUS_LOCK = "1";

        /// <summary>
        /// Tình trạng chưa khóa
        /// </summary>
        public const string STATUS_NOT_LOCK = "0";

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

        #region Ticket month

        /// <summary>
        /// Vé tháng hết hạn
        /// </summary>
        public const string TICKET_MONTH_EXPIRED = "1";

        /// <summary>
        ///  Vé tháng còn hạn
        /// </summary>
        public const string TICKET_MONTH_UNEXPIRED = "0";

        #endregion

        #region Vehicle

        /// <summary>
        /// Chưa kích hoạt
        /// </summary>
        public const string VEHICLE_STATUS_UNACTIVE = "0";

        /// <summary>
        /// Đã kích hoạt
        /// </summary>
        public const string VEHICLE_STATUS_ACTIVE = "1";

        #endregion

        #region ParkingLot

        /// <summary>
        /// Hoạt động
        /// </summary>
        public const string PARKINGLOT_STATUS_WORK = "0";

        /// <summary>
        /// Dừng hoạt động
        /// </summary>
        public const string PARKINGLOT_STATUS_STOPWORK = "1";

        #endregion

        #region Group

        /// <summary>
        /// Không sử dụng
        /// </summary>
        public const string GROUP_STATUS_UNUSE = "0";

        /// <summary>
        /// Có sử dụng
        /// </summary>
        public const string GROUP_STATUS_USE = "1";

        #endregion

        #region Position

        /// <summary>
        /// Chưa kích hoạt
        /// </summary>
        public const string POSITION_STATUS_UNACTIVE = "0";

        /// <summary>
        /// Đã kích hoạt
        /// </summary>
        public const string POSITION_STATUS_ACTIVE = "1";

        #endregion

        /// <summary>
        /// MOBILE CLIENTID
        /// </summary>
        public const string MOBILE_CLIENTID = "QLTKMobile";
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

        #endregion

        #region NOTIFICATION_TYPE

        /// <summary>
        /// Thông báo bình thường
        /// </summary>
        public const string NOTIFICATION_TYPE_NORMAL = "0";

        #endregion
        #region

        /// <summary>
        /// Trạng thái relay 
        /// </summary>
        public const string CONSUME_RELAYSTATUS_ON = "1";

        /// <summary>
        /// Trạng thái relay 
        /// </summary>
        public const string CONSUME_RELAYSTATUS_OFF = "0";
        /// <summary>
        /// commad cảnh báo 1
        /// </summary>
        public const string COMMAND_AMPERE_ALARM1 = "CMD=25";
        /// <summary>
        /// commad cảnh báo 2
        /// </summary>
        public const string COMMAND_AMPERE_ALARM2 = "CMD=26";
        /// <summary>
        /// commad tắt 1
        /// </summary>
        public const string COMMAND_AMPERE_OFF1 = "CMD=23";
        /// <summary>
        /// commad tắt 2
        /// </summary>
        public const string COMMAND_AMPERE_OFF2 = "CMD=24";
        /// <summary>
        /// commad Set thời gian chu kỳ 2
        /// </summary>
        public const string COMMAND_CYCLE_TIME = "CMD=30";
        #endregion
        #region REGISTER_TYPE

        /// <summary>
        /// Kiểu đăng ký vé tháng
        /// </summary>
        public const string REGISTER_TYPE_TICKET = "1";

        /// <summary>
        /// Kiểu đăng ký đặt chỗ
        /// </summary>
        public const string REGISTER_TYPE_BOOKING = "2";

        #endregion

        #region COMMANDS
        /// <summary>
        /// SET RELAY
        /// </summary>
        public const string COMMAND_SET_RELAY1_Y = "{CMD=21:Y!**}";
        public const string COMMAND_SET_RELAY1_N = "{CMD=21:N!**}";
        public const string COMMAND_SET_RELAY2_N = "{CMD=22:N!**}";
        public const string COMMAND_SET_RELAY2_Y = "{CMD=22:Y!**}";

        /// <summary>
        /// GET RELAY
        /// </summary>
        public const string COMMAND_GET_RELAY = "{CMD=20!**}";

        #endregion
        /// <summary>
        /// Đầu đọc tiền lỗi 
        /// </summary>
        public const string READER_TYPE_STATUS = "0";
        /// <summary>
        /// đèn les đang bật
        /// </summary>
        public const string Device_LED_TYPE_STATUS = "0";
        /// <summary>
        ///ĐẦU ĐỌC TIỀN
        /// </summary>
        public const string Device_Money_TYPE_STATUS = "0";
        /// <summary>
        /// Connect thiế bị Connect
        /// </summary>
        public const string CONNECT_TYPE_STATUS = "0";
        /// <summary>
        /// Ngăn chứa tiền
        /// </summary>
        public const string Device_CONNECT_TYPE_STATUS = "0";

        /// <summary>
        ///  Trang thai thoi gian thuc tren may
        /// </summary>
        public const string Device_Time_TYPE_STATUS = "0";
        
        /// <summary>
        ///  Trang thai cửa mở
        /// </summary>
        public const string Device_Door_TYPE_0_STATUS = "0";
        /// <summary>
        ///  Trang thai cửa đóng
        /// </summary>
        public const string Device_Door_TYPE_1_STATUS = "1";

        /// <summary>
        ///  Trang thai cảm biến rơi
        /// </summary>
        public const string Device_Sensor_TYPE_STATUS = "0";

        /// <summary>
        ///  Trang thai cảm biến rơi lỗi
        /// </summary>
        public const string Device_Sensor_Error_STATUS = "0";
        #region Product type
        //Loại sản phẩm chè
        public const string Product_Type_Tea = "1";
        //Loại sản phẩm bao bì
        public const string Product_Type_Bag = "2";
        #endregion

        public const string fileTypeFile = "File";
        public const string fileTypeForder = "Forder";
        public const string fileUpload = "fileUpload/";
        public const string fileZip = "fileZip";



    }
}
