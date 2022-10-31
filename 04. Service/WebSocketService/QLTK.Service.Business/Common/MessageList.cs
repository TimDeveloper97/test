using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Common
{
    public class MessageList
    {
        #region Message
        /// <summary>
        /// {0}: chỉ được phép nhập các giá trị số
        /// </summary>
        public const string MSG001 = "MSG001";

        /// <summary>
        /// Không có người dùng nào.
        /// </summary>
        public const string MSG002 = "MSG002";

        /// <summary>
        /// Lưu thông tin {0} thành công.
        /// </summary>
        public const string MSG004 = "MSG004";

        /// <summary>
        /// {0}: chỉ cho phép nhập các giá trị số nguyên
        /// </summary>
        public const string MSG005 = "MSG005";

        /// <summary>
        /// Bạn có muốn xóa {0} này?
        /// </summary>
        public const string MSG006 = "MSG006";

        /// <summary>
        /// Làm mới mật khẩu thành công
        /// </summary>
        public const string MSG007 = "MSG007";

        /// <summary>
        /// {0}: không được để trống.
        /// </summary>
        public const string MSG008 = "MSG008";

        /// <summary>
        /// {0} đăng nhập không đúng
        /// </summary>
        public const string MSG009 = "MSG009";

        /// <summary>
        /// 
        /// </summary>
        public const string MSG010 = "MSG010";

        /// <summary>
        /// Bạn có muốn thoát khỏi giao diện {0} không?
        /// </summary>
        public const string MSG017 = "MSG017";

        /// <summary>
        /// {0} đã bị xóa.
        /// </summary>
        public const string MSG018 = "MSG018";

        /// <summary>
        /// Mật khẩu cũ không đúng
        /// </summary>
        public const string MSG019 = "MSG019";

        /// <summary>
        /// Mật khẩu xác nhận không đúng
        /// </summary>
        public const string MSG020 = "MSG020";

        /// <summary>
        /// Không có log thao tác người dùng nào
        /// </summary>
        public const string MSG021 = "MSG021";

        /// <summary>
        /// Thay đổi {0} thành công
        /// </summary>
        public const string MSG022 = "MSG022";

        /// <summary>
        /// Tên truy cập đã có người sử dụng. {0} Bạn hãy chọn tên truy cập khác.
        /// </summary>
        public const string MSG023 = "MSG023";

        /// <summary>
        /// Thêm mới {0} thành công.
        /// </summary>
        public const string MSG024 = "MSG024";

        /// <summary>
        /// {0} phải có độ dài từ {1} đến {2} ký tự.
        /// </summary>
        public const string MSG025 = "MSG025";

        /// <summary>
        /// Xóa {0} thành công.
        /// </summary>
        public const string MSG026 = "MSG026";

        /// <summary>
        /// {0}: không được nhập nhiều hơn {1} ký tự
        /// </summary>
        public const string MSG027 = "MSG027";

        /// <summary>
        /// {0}: không được nhập ít hơn {1} ký tự
        /// </summary>
        public const string MSG128 = "MSG128";

        /// <summary>
        /// Từ ngày không được lớn hơn ngày đến.
        /// </summary>
        public const string MSG028 = "MSG028";

        /// <summary>
        /// Không có kết quả thỏa mãn điều kiện
        /// </summary>
        public const string MSG029 = "MSG029";

        /// <summary>
        /// Không tồn tại Sheet theo quy định.\nQuy đinh tên Sheet là {0}
        /// </summary>
        public const string MSG030 = "MSG030";

        /// <summary>
        /// Có {0}  {1} bị lỗi.
        /// </summary>
        public const string MSG031 = "MSG031";

        /// <summary>
        /// Bạn không có quyền {0}
        /// </summary>
        public const string MSG032 = "MSG032";

        /// <summary>
        /// Chỉnh sửa {0} thành công.
        /// </summary>
        public const string MSG033 = "MSG033";

        /// <summary>
        /// {0} đã tồn tại. Bạn hãy nhập {1} khác
        /// </summary>
        public const string MSG034 = "MSG034";

        /// <summary>
        /// Bạn chưa nhập thông tin {0}. Không xem được {1}
        /// </summary>
        public const string MSG035 = "MSG035";

        /// <summary>
        /// Không có file nào
        /// </summary>
        public const string MSG036 = "MSG036";

        /// <summary>
        /// Bạn chưa nhập thông tin {0}. Không thêm được {1}.
        /// </summary>
        public const string MSG037 = "MSG037";

        /// <summary>
        /// Không có hạn mục nào
        /// </summary>
        public const string MSG038 = "MSG038";

        /// <summary>
        /// Bạn chưa nhập giá trị thực tế. Không đánh gía được tiêu chí.
        /// </summary>
        public const string MSG039 = "MSG039";

        /// <summary>
        /// Phòng hiện tại đang có nhận viên chịu trách nhiệm. {0} Không thay đổi được phòng ban.
        /// </summary>
        public const string MSG040 = "MSG040";

        /// <summary>
        /// Bạn chưa chọn {0}.
        /// </summary>
        public const string MSG041 = "MSG041";

        /// <summary>
        /// {0} không được phép chứa các ký tự đặc biêt. {1} @,  
        /// [, \\,  /, ?, :, <, >, |, * , \ ," , ], '
        /// </summary>
        public const string MSG042 = "MSG042";

        /// <summary>
        /// Bạn có chắc muốn yêu cầu vật tư {0}.
        /// </summary>
        public const string MSG043 = "MSG043";

        /// <summary>
        /// Thiết bị chưa thiết kế xong không được phép yêu cầu vật tư
        /// </summary>
        public const string MSG044 = "MSG044";

        /// <summary>
        /// Thiết bị đã chuyển sang bước khác, không hủy duyệt thiết kế được.
        /// </summary>
        public const string MSG045 = "MSG045";

        /// <summary>
        /// Chọn thành công {0}
        /// </summary>
        public const string MSG046 = "MSG046";

        /// <summary>
        /// Bạn chưa chọn {0} cần {1}.
        /// </summary>
        public const string MSG047 = "MSG047";

        /// <summary>
        /// Bạn chưa chọn {0}, không được phép thêm {1}
        /// </summary>
        public const string MSG048 = "MSG048";

        /// <summary>
        /// Không có {0} nào
        /// </summary>
        public const string MSG049 = "MSG049";

        /// <summary>
        /// Bạn phải chọn cột {0}
        /// </summary>
        public const string MSG050 = "MSG050";

        /// <summary>
        /// Bạn không được chọn 1 lúc nhiều cột
        /// </summary>
        public const string MSG051 = "MSG051";

        /// <summary>
        /// Email không đúng định dạng. Bạn hãy kiểm tra lại
        /// </summary>
        public const string MSG052 = "MSG052";

        /// <summary>
        /// Không được phép chọn nhiều {0}
        /// </summary>
        public const string MSG053 = "MSG053";

        /// <summary>
        /// Import thành công {0}
        /// </summary>
        public const string MSG054 = "MSG054";

        /// <summary>
        /// Chưa nhập {0} không được phép {1}
        /// </summary>
        public const string MSG055 = "MSG055";

        /// <summary>
        /// Thông tin kết nối không chính xác.
        /// </summary>
        public const string MSG056 = "MSG056";

        /// <summary>
        /// Có {0} {1} và {2} module bị lỗi
        /// </summary>
        public const string MSG057 = "MSG057";

        /// <summary>
        /// Bạn không có quyền upload file lên server
        /// </summary>     
        public const string MSG058 = "MSG058";

        /// <summary>
        /// {0} không được lớn hơn {1}
        /// </summary>
        public const string MSG059 = "MSG059";

        /// <summary>
        /// Tài liệu đã được upload nội dung lên server. {0}Bạn có muốn thay thế nội dung hiện tại không?
        /// </summary>
        public const string MSG060 = "MSG060";

        /// <summary>
        /// Tài liệu chưa có nội dung trên server.
        /// </summary>
        public const string MSG061 = "MSG061";

        /// <summary>
        /// {0} không hợp lệ.
        /// </summary>
        public const string MSG062 = "MSG062";

        /// <summary>
        /// {0} chưa có ảnh trên server.
        /// </summary>
        public const string MSG063 = "MSG063";

        /// <summary>
        /// Bạn nhập số lượng {0} vượt quá số lượng cần mua.
        /// </summary>
        public const string MSG064 = "MSG064";

        /// <summary>
        /// {0} đã được yêu cầu không được phép xóa.
        /// </summary>
        public const string MSG065 = "MSG065";

        /// <summary>
        /// Bạn chưa chọn {0} hoặc {1} đã có trong {2}.
        /// </summary>
        public const string MSG066 = "MSG066";

        /// <summary>
        /// Thông tin {0} chưa được lưu vào cơ sở dữ liệu.Nên bạn không {1} được.
        /// </summary>
        public const string MSG067 = "MSG067";

        /// <summary>
        /// {0} dữ liệu thành công
        /// </summary>
        public const string MSG068 = "MSG068";

        /// <summary>
        /// {0} chưa được {1} duyệt hoặc bạn không có quyền duyệt {2} này.
        /// </summary>
        public const string MSG069 = "MSG069";

        /// <summary>
        /// chưa có yêu cầu duyệt{0}
        /// </summary>
        public const string MSG070 = "MSG070";

        /// <summary>
        /// {0} đã được {1} duyệt.
        /// </summary>
        public const string MSG071 = "MSG071";

        /// <summary>
        /// Chọn {0} thành module chuẩn thành công
        /// </summary>
        public const string MSG072 = "MSG072";

        /// <summary>
        /// {0} đã được chọn làm module chuẩn
        /// </summary>
        public const string MSG073 = "MSG073";


        /// <summary>
        /// {0} chưa được {1} duyệt
        /// </summary>
        public const string MSG074 = "MSG074";

        /// <summary>
        /// bạn chưa nhập {0} không được phép {1}.
        /// </summary>
        public const string MSG075 = "MSG075";

        /// <summary>
        /// chưa có thông tin {0}
        /// </summary>
        public const string MSG076 = "MSG076";

        /// <summary>
        /// Chyển {0} sang  {1} thành công
        /// </summary>
        public const string MSG077 = "MSG077";

        /// <summary>
        /// {0} đã xuất kho.Không thể {1}.
        /// </summary>
        public const string MSG078 = "MSG078";

        /// <summary>
        /// {0} đang chờ xuất kho.Không thể {1}.
        /// </summary>
        public const string MSG079 = "MSG079";



        /// <summary>
        /// Chưa có yêu cầu {0}
        /// </summary>
        public const string MSG080 = "MSG080";

        /// <summary>
        /// Vật tư đã đươc {0} nhập kho.
        /// </summary>
        public const string MSG081 = "MSG081";

        /// <summary>
        /// Vật tư đã đươc {0} nhập kho,không được phép xóa
        /// </summary>
        public const string MSG082 = "MSG082";

        /// <summary>
        /// {0} chưa được đồng bộ,không thể {1}.
        /// </summary>
        public const string MSG083 = "MSG083";

        /// <summary>
        /// {0} không được nhập chữ có dấu
        /// </summary>
        public const string MSG084 = "MSG084";

        /// <summary>
        /// Vật tư {0}.Không thể {1}
        /// </summary>
        public const string MSG085 = "MSG085";

        /// <summary>
        /// {0} đang mua.Không thể {1}.
        /// </summary>
        public const string MSG086 = "MSG086";

        /// <summary>
        /// {0} không tồn tại trên server.
        /// </summary>
        public const string MSG087 = "MSG087";

        /// <summary>
        /// {0} đang duyệt nên không thể thay đổi thông tin.
        /// </summary>
        public const string MSG088 = "MSG088";

        /// <summary>
        /// {0} chưa được duyệt,không thể hoàn thành.
        /// </summary>
        public const string MSG089 = "MSG089";

        /// <summary>
        /// Bạn chưa nhập thông tin {0}. Không {1}.
        /// </summary>
        public const string MSG090 = "MSG090";

        /// <summary>
        /// {0} đang khóa.Bạn cần mở khóa mới lưu được {1}.
        /// </summary>
        public const string MSG091 = "MSG091";

        /// <summary>
        /// Chưa có {0} nào. Không được phép hoàn thành thiết kế.
        /// </summary>
        public const string MSG092 = "MSG092";

        /// <summary>
        /// Thiết bị mua ngoài đã được {0},không được phép {1}.
        /// </summary>
        public const string MSG093 = "MSG093";

        /// <summary>
        /// {0} đã {1} .Nên không thể chỉnh sửa thông tin
        /// </summary>
        public const string MSG094 = "MSG094";

        /// <summary>
        /// Chưa có {0} nào. Không được yêu cầu duyệt
        /// </summary>
        public const string MSG095 = "MSG095";

        /// <summary>
        /// {0}: chỉ cho phép nhập các giá trị số nguyên dương
        /// </summary>
        public const string MSG096 = "MSG096";

        /// <summary>
        /// Dự án {0}. Bạn không thể thay đổi thông tin được.
        /// </summary>
        public const string MSG097 = "MSG097";

        /// <summary>
        /// Bạn chắc chắn muốn {1} dự án ?
        /// </summary>
        public const string MSG098 = "MSG098";

        /// <summary>
        /// {0} không tồn tại.Bạn hãy nhập {1} khác.
        /// </summary>
        public const string MSG099 = "MSG099";

        /// <summary>
        /// {0} không được nhập số âm
        /// </summary>
        public const string MSG100 = "MSG100";

        /// <summary>
        /// {0} .Không thể {1}.
        /// </summary>
        public const string MSG101 = "MSG101";

        /// <summary>
        /// {0} đã được {1}. Không được hủy duyệt.
        /// </summary>
        public const string MSG102 = "MSG102";

        /// <summary>
        /// {0} đang được {1}. Không được {2}.
        /// </summary>
        public const string MSG103 = "MSG103";



        /// <summary>
        /// {0} không được lớn hơn {1}.
        /// </summary>
        public const string MSG104 = "MSG104";

        /// <summary>
        /// Thông tin {0} của module chưa có. Không được phép hoàn thành thiết bị.
        /// </summary>
        public const string MSG105 = "MSG105";

        /// <summary>
        /// Bạn nhập {0} vượt quá {1}.
        /// </summary>
        public const string MSG106 = "MSG106";

        /// <summary>
        /// {0} đang KCS không được phép thêm.
        /// </summary>
        public const string MSG107 = "MSG107";

        /// <summary>
        /// Sản phẩm báo giá là {0} nên bạn không thể {1}.
        /// </summary>
        public const string MSG108 = "MSG108";

        /// <summary>
        /// {0} không được nhỏ hơn {1}.
        /// </summary>
        public const string MSG109 = "MSG109";

        /// <summary>
        /// Chương trình cập nhật phiên bản không tồn tại bạn hãy kiểm tra lại.
        /// </summary>
        public const string MSG110 = "MSG110";

        /// <summary>
        /// Tài liệu đã được upload cho tài liệu chuẩn khác. Bạn không được phép tải lên cho tài liệu chuẩn này.
        /// </summary>
        public const string MSG111 = "MSG111";

        /// <summary>
        /// Tạo tài liệu mẫu {0} thành công
        /// </summary>
        public const string MSG112 = "MSG112";

        /// <summary>
        /// Bạn không thuộc phòng thiết kế thiết bị này. Không được phép {0}.
        /// </summary>
        public const string MSG113 = "MSG113";

        /// <summary>
        /// Bạn không thuộc phòng tạo thiết bị này. Không được phép xóa.
        /// </summary>
        public const string MSG115 = "MSG115";

        /// <summary>
        /// Không có {0} cần import
        /// </summary>
        public const string MSG114 = "MSG114";

        /// <summary>
        /// Không được phép chọn {0}.
        /// </summary>
        public const string MSG116 = "MSG116";

        /// <summary>
        /// Chuyển danh sách vật tư thiết bị {0} thành công.
        /// </summary>
        public const string MSG117 = "MSG117";

        /// <summary>
        /// Bạn không thuộc phong thiết kế thiết bị này. Không được phép xóa.
        /// </summary>
        public const string MSG118 = "MSG118";

        /// <summary>
        /// Lỗi xóa tài liệu trên server. Bạn hãy kiểm tra lại
        /// </summary>
        public const string MSG119 = "MSG119";

        /// <summary>
        /// {0} chưa  được đồng bộ.{1} Không được phép {2}.
        /// 1. \n
        /// </summary>
        public const string MSG120 = "MSG120";

        /// <summary>
        /// Có thể có {0} thừa,bạn có muốn xóa không ?
        /// </summary>
        public const string MSG121 = "MSG121";

        /// <summary>
        /// Hạng mục nhỏ hơn chưa được hoàn thành.{0}Không được phép hoàn thành hạng mục này.
        /// </summary>
        public const string MSG122 = "MSG122";

        /// <summary>
        ///{0} này không phải của phòng bạn.{1}Không được phép {2}.
        /// </summary>
        public const string MSG123 = "MSG123";

        /// <summary>
        /// Xuất đề nghị {0} thành công. 
        /// </summary>
        public const string MSG124 = "MSG124";

        /// <summary>
        ///Bạn có muốn hoàn thành {0}? 
        /// </summary>
        public const string MSG125 = "MSG125";

        /// <summary>
        /// Chưa KCS hêt vật tư. Không được phép xác nhận KCS.
        /// </summary>
        public const string MSG126 = "MSG126";

        /// <summary>
        /// {0} thành công.
        /// </summary>
        public const string MSG127 = "MSG127";

        /// <summary>
        /// Title thông báo
        /// </summary>
        public const string TITLE = "TITLE";

        #endregion

        #region Error Message
        /// <summary>
        /// Không kết nối được cơ sở dữ liệu. {0}Bạn có muốn cái đặt lại kết nối hay không?
        /// </summary>
        public const string ERR001 = "ERR001";

        /// <summary>
        /// Linh kiện đã được yêu cầu vật tư không được phép xóa
        /// </summary>
        public const string ERR002 = "ERR002";

        /// <summary>
        /// Lỗi thiết bị có tài liệu chứng minh không được phép xóa
        /// </summary>
        public const string ERR003 = "ERR003";

        /// <summary>
        /// Lỗi Database : {0}
        /// </summary>
        public const string ERR004 = "ERR004";

        /// <summary>
        /// Tài liệu đang được sử dụng không được phép xóa
        /// </summary>
        public const string ERR005 = "ERR005";

        /// <summary>
        /// Hạng mục đang được sử dụng không được phép xóa
        /// </summary>
        public const string ERR006 = "ERR006";

        /// <summary>
        /// Đang có tiêu chí không được phép xóa
        /// </summary>
        public const string ERR007 = "ERR007";

        /// <summary>
        /// {0} đang sử dụng không được phép xoá
        /// </summary>
        public const string ERR008 = "ERR008";

        /// <summary>
        /// {0} đã có thông tin liên quan không được phép xóa.
        /// </summary>
        public const string ERR009 = "ERR009";

        /// <summary>
        /// Thông tin kết nối không chính xác. Vui lòng kiểm tra lại thông tin kết nối
        /// </summary>
        public const string ERR010 = "ERR010";

        /// <summary>
        /// Không kết nối được máy chủ. Vui lòng thử lại sau.
        /// </summary>
        public const string ERR011 = "ERR011";
        /// <summary>
        /// Service của máy chủ đang tạm dừng. Vui lòng kích hoạt lại Service của máy chủ.
        /// </summary>
        public const string ERR012 = "ERR012";

        /// <summary>
        /// Service của máy chủ đang tạm dừng. Vui lòng kích hoạt lại Service của máy chủ.
        /// </summary>
        public const string ERR013 = "ERR013";

        /// <summary>
        /// Tài khoản đang đăng nhập ở một máy tính khác
        /// </summary>
        public const string ERR014 = "ERR014";

        /// <summary>
        /// Đọc file lỗi. Bạn hãy kiểm tra lại file.
        /// </summary>
        public const string ERR015 = "ERR015";

        /// <summary>
        /// Tài khoản đang đăng nhập trên một máy {0}.
        /// </summary>
        public const string ERR016 = "ERR016";

        /// <summary>
        /// Lỗi thêm dữ liệu bạn hãy thêm lại.
        /// </summary>
        public const string ERR017 = "ERR017";
        #endregion

        #region About

        /// <summary>
        /// Version của phần mềm
        /// </summary>
        public const string VERSION = "VERSION";

        /// <summary>
        /// Ngày build
        /// </summary>
        public const string BUILDDATE = "BUILDDATE";

        #endregion
    }
}
