using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Repositories;
using NTS.Model.UnitHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Users
{
    public class UserLogUtil
    {
        /// <summary>
        /// Log lịch sử khi thêm mới
        /// Nội dung log: Thêm mới {objectName} Tên/Mã: {nameOrCode}
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <param name="nameOrCode">Nội dung đối tượng (Mã/Tên)</param>
        /// <param name="objectType">Loại đối tượng lưu lịch sử</param>
        public static void LogHistotyAdd(QLTKEntities db, string userId, string nameOrCode, string objectId, int objectType)
        {
            try
            {
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0053, GetObjectName(objectType), nameOrCode),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                db.UserHistories.Add(adduser);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Log thay đổi thông tin chung của đối tượng
        /// Nội dung log: Cập nhật {subName} của {objectName} Tên/Mã: {nameOrCode}
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId">Id tài khoản đăng nhập</param>
        /// <param name="objectType">Loại đối tượng </param>
        /// <param name="objectId">Id bản ghi</param>
        /// <param name="nameOrCode">Nội dung bản ghi (Mã/Tên)</param>
        /// <param name="dataBefor">Dữ liệu trước khi sửa</param>
        /// <param name="dataAfter">Dữ liệu sau khi sửa</param>
        public static void LogHistotyUpdateInfo(QLTKEntities db, string userId, int objectType, string objectId, string nameOrCode, object dataBefor, object dataAfter)
        {
            try
            {
                var a = JsonConvert.SerializeObject(dataAfter);
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0054, GetObjectName(objectType), nameOrCode),
                    DataAfter = JsonConvert.SerializeObject(dataAfter),
                    DataBefor = JsonConvert.SerializeObject(dataBefor),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                if (string.IsNullOrEmpty(nameOrCode))
                {
                    adduser.Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, GetObjectName(objectType));
                }
                else
                {
                    adduser.Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0054, GetObjectName(objectType), nameOrCode);
                }

                db.UserHistories.Add(adduser);

                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin phụ của đối tượng
        /// Nội dung log: Cập nhật {subName} của {objectName} Tên/Mã: {nameOrCode}
        /// </summary>  
        /// <param name="db"></param>
        /// <param name="userId">Id tài khoản đăng nhập</param>
        /// <param name="objectType">Loại đối tượng </param>
        /// <param name="objectId">Id bản ghi</param>
        /// <param name="nameOrCode">Mã/Tên Đối tượng cập nhật</param
        /// <param name="subName">Tên thông tin phụ</param>
        public static void LogHistotyUpdateSub(QLTKEntities db, string userId, int objectType, string objectId, string nameOrCode, string subName)
        {

            try
            {
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0055, subName, GetObjectName(objectType), nameOrCode),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                db.UserHistories.Add(adduser);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin phụ của đối tượng
        /// Nội dung log: {content} của {objectName} Tên/Mã: {nameOrCode}
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId">Id tài khoản đăng nhập</param>
        /// <param name="objectType">Loại đối tượng </param>
        /// <param name="objectId">Id bản ghi</param>
        /// <param name="nameOrCode">Mã/Tên Đối tượng cập nhật</param
        /// <param name="content">Nội dung thay đổi</param>
        public static void LogHistotyUpdateOther(QLTKEntities db, string userId, int objectType, string objectId, string nameOrCode, string content)
        {

            try
            {
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0058, content, GetObjectName(objectType), nameOrCode),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                db.UserHistories.Add(adduser);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Log xóa đối tượng
        /// Nội dung log: Xóa {objectName} Tên/Mã: {nameOrCode}
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId">Id tài khoản đăng nhập</param>
        /// <param name="objectType">Loại đối tượng </param>
        /// <param name="objectId">Id bản ghi</param>
        /// <param name="nameOrCode">Nội dung bản ghi (Mã/Tên)</param>
        public static void LogHistotyDelete(QLTKEntities db, string userId, int objectType, string objectId, string nameOrCode, object dataBefor)
        {
            try
            {
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0056, GetObjectName(objectType), nameOrCode),
                    DataBefor = JsonConvert.SerializeObject(dataBefor),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                db.UserHistories.Add(adduser);

                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Log xóa đối tượng phụ của đối tượng
        /// Nội dung log: Xóa {objectSubName} Tên/mã: {value} của {objectName} Tên/Mã: {nameOrCode}
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId">Id tài khoản đăng nhập</param>
        /// <param name="objectType">Loại đối tượng </param>
        /// <param name="objectId">Id bản ghi</param>
        /// <param name="nameOrCode">Mã/Tên đối tượng</param>
        /// <param name="objectSubName">Tên đối tượng phụ</param>
        /// <param name="value">Giá trị xóa (Tên/Mã)</param>
        public static void LogHistotyDeleteSub(QLTKEntities db, string userId, int objectType, string objectId, string nameOrCode, string objectSubName, string value)
        {
            try
            {
                NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ObjectType = objectType,
                    ObjectId = objectId,
                    Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0057, objectSubName, value, GetObjectName(objectType), nameOrCode),
                    CreateDate = DateTime.Now,
                    CreateBy = userId,
                };

                db.UserHistories.Add(adduser);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        private static string GetObjectName(int objectType)
        {
            switch (objectType)
            {
                case 1:
                    {
                        return "Vật tư";
                    }
                case 2:
                    {
                        return "Nhóm vật tư chức năng";
                    }
                case 3:
                    {
                        return "Nhóm vật tư TPA";
                    }
                case 4:
                    {
                        return "Cấu hình thông số vật tư";
                    }
                case 5:
                    {
                        return "Nhóm vật tư phi tiêu chuẩn";
                    }
                case 6:
                    {
                        return "Hãng sản xuất";
                    }
                case 7:
                    {
                        return "Đơn vị tính";
                    }
                case 8:
                    {
                        return "Vật liệu";
                    }
                case 9:
                    {
                        return "Nhà cung cấp";
                    }
                case 10:
                    {
                        return "Vật tư tương tự";
                    }
                case 11:
                    {
                        return "Định nghĩa mã vật tư phần mềm tạo";
                    }
                case 12:
                    {
                        return "Module";
                    }
                case 13:
                    {
                        return "Tiêu chí kiểm tra";
                    }
                case 14:
                    {
                        return "Tiêu chuẩn sản phẩm";
                    }
                case 15:
                    {
                        return "Tính năng";
                    }
                case 16:
                    {
                        return "Nghành";
                    }
                case 18:
                    {
                        return "bBi thực hành";
                    }
                case 19:
                    {
                        return "Kỹ năng bài thực hành";
                    }
                case 20:
                    {
                        return "T độ";
                    }
                case 21:
                    {
                        return "Chuyên môn";
                    }
                case 22:
                    {
                        return "Đơn vị công tác";
                    }
                case 23:
                    {
                        return "Chuyên gia";
                    }
                case 24:
                    {
                        return "Môn học";
                    }
                case 25:
                    {
                        return "Chương trình đào tạo";
                    }
                case 26:
                    {
                        return "Phòng học";
                    }
                case 27:
                    {
                        return "Nghề";
                    }
                case 28:
                    {
                        return "Dự án";
                    }
                case 29:
                    {
                        return "Nhóm khách hàng";
                    }
                case 30:
                    {
                        return "Khách hàng";
                    }
                case 31:
                    {
                        return "Vấn đề";
                    }
                case 32:
                    {
                        return "Công việc theo nhóm module ";
                    }
                case 33:
                    {
                        return "Thời gian tiêu chuẩn cho từng công việc";
                    }
                case 34:
                    {
                        return "Kế hoạch thiết kế";
                    }
                case 35:
                    {
                        return "Cấu hình thông tin ngày nghỉ";
                    }
                case 36:
                    {
                        return "Thời gian làm việc";
                    }
                case 37:
                    {
                        return "Giải pháp";
                    }
                case 38:
                    {
                        return "Chương trình đào tạo";
                    }
                case 39:
                    {
                        return "Nhật ký công việc";
                    }
                case 40:
                    {
                        return "Nhóm quyền";
                    }
                case 41:
                    {
                        return "Thời gian làm việc";
                    }
                case 42:
                    {
                        return "Phòng ban";
                    }
                case 43:
                    {
                        return "Chức vụ";
                    }
                case 44:
                    {
                        return "SBU";
                    }
                case 45:
                    {
                        return "Nhân viên";
                    }
                case 46:
                    {
                        return "Nhóm nhân viên";
                    }
                case 47:
                    {
                        return "Khóa học";
                    }
                case 48:
                    {
                        return "Kỹ năng nhân viên";
                    }
                case 49:
                    {
                        return "Vị trí công việc";
                    }
                case 50:
                    {
                        return "Tạo cấu trúc thư mục";
                    }
                case 51:
                    {
                        return "Cài đặt định nghĩa thư mục";
                    }
                case 52:
                    {
                        return "Đổi tên file scan";
                    }
                case 53:
                    {
                        return "Phân bổ dữ liệu";
                    }

                case 54:
                    {
                        return "Hồ sơ thiết kế mạch điện tử";
                    }

                case 55:
                    {
                        return "Tạo file hồ sơ điện";
                    }

                case 56:
                    {
                        return "Lập bảng tín hiệu I/O";
                    }

                case 57:
                    {
                        return "Quá trình thử nghiệm";
                    }

                case 58:
                    {
                        return "Vẽ sơ đồ thuật toán điều khiển";
                    }

                case 59:
                    {
                        return "Danh mục thiết bị theo chức năng";
                    }

                case 60:
                    {
                        return "Hạng mục thiết kế";
                    }

                case 61:
                    {
                        return "Danh sách hàm chức năng";
                    }

                case 62:
                    {
                        return "Dữ liệu cài đặt";
                    }

                case 63:
                    {
                        return "Kiểm tra nguyên lý";
                    }
                case 64:
                    {
                        return "Biểu mẫu kiểm tra nguyên lý";
                    }
                case 65:
                    {
                        return "Phương án thiết kế - Danh mục khối chức năng";
                    }
                case 66:
                    {
                        return "Kiểm tra chất lượng sản phẩm mạch điện tử";
                    }
                case 67:
                    {
                        return "Phương án thiết kế - Mô tả chung - Sơ đồ khối";
                    }
                case 68:
                    {
                        return "Biểu mẫu kiểm tra bản vẽ mạch in";
                    }
                case 69:
                    {
                        return "Biểu mẫu bản vẽ nguyên lý - Bảng tính toán";
                    }
                case 70:
                    {
                        return "Phương án thiết kế - Linh kiện chính,thông số mạch";
                    }
                case 71:
                    {
                        return "Biểu mẫu hướng dẫn lắp ráp mạch điện tử";
                    }
                case 72:
                    {
                        return "Biểu mẫu vật tư";
                    }
                case 73:
                    {
                        return "Tạo phác thảo thiết kế";
                    }
                case 74:
                    {
                        return "Kiểm tra phương án thiết kế";
                    }
                case 75:
                    {
                        return "Dự toán sơ bộ";
                    }
                case 76:
                    {
                        return "Lập thông số kỹ thuật";
                    }
                case 77:
                    {
                        return "Tạo danh mục";
                    }
                case 78:
                    {
                        return "Download danh mục vật tư sap";
                    }
                case 79:
                    {
                        return "Quản lý xuất giữ";
                    }
                case 80:
                    {
                        return "Lịch sử xuất giữ";
                    }
                case 81:
                    {
                        return "So sánh sai khác thư viện sản phẩm với nguồn";
                    }
                case 82:
                    {
                        return "Đồng bộ vật tư";
                    }
                case 83:
                    {
                        return "Đồng bộ module";
                    }
                case 84:
                    {
                        return "Đồng bộ thiết bị";
                    }
                case 85:
                    {
                        return "Đồng bộ thiết bị nhập khẩu";
                    }
                case 86:
                    {
                        return "Chủng loại hàng hóa";
                    }
                case 87:
                    {
                        return "Sản phẩm kinh doanh";
                    }
                case 88:
                    {
                        return "Ứng dụng";
                    }
                case 89:
                    {
                        return "Nhóm kinh doanh";
                    }
                case 90:
                    {
                        return "Cấu hình tài liệu HSNK";
                    }
                case 91:
                    {
                        return "Chủng loại hàng hóa kinh doanh";
                    }
                case 92:
                    {
                        return "Hồ sơ nhập khẩu";
                    }
                case 93:
                    {
                        return "Cấu hình tài liệu HSNK";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

    }
}
