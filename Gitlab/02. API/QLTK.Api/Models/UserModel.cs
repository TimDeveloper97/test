// <copyright company="nhantinsoft.vn">
// Author: Vũ Văn Văn
// Created Date: 09/08/2016 12:08
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Api.Models
{
    public class UserModel
    {
        /// <summary> 
        /// id 
        /// </summary> 
        public string UserId { get; set; }
        /// <summary> 
        /// Chức vụ 
        /// </summary> 
        public string UserRoleId { get; set; }
        /// <summary> 
        /// Nhóm người dùng
        /// </summary> 
        public string GroupId { get; set; }

        /// <summary> 
        /// Tên đăng nhập 
        /// </summary> 
        public string Name { get; set; }
        /// <summary> 
        /// Họ tên 
        /// </summary> 
        public string FullName { get; set; }
        /// <summary> 
        /// Ngày sinh 
        /// </summary> 
        public Nullable<DateTime> BirthDay { get; set; }
        public string Aliass { get; set; }
        /// <summary>
        /// Cơ quan
        /// </summary>
        public string Agency { get; set; }
        /// <summary> 
        /// Email 
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// Cấp bậc
        /// </summary>
        //public string PoliceLevel { get; set; }
        /// <summary>
        /// Chức vụ
        /// </summary>
        public string Role { get; set; }
        /// <summary> 
        /// Điện thoại 
        /// </summary> 
        public string PhoneNumber { get; set; }
        /// <summary> 
        /// Mật khẩu 
        /// </summary> 
        public string Password { get; set; }
        /// <summary> 
        /// Mã hóa 
        /// </summary> 
        public string PasswordHash { get; set; }
        /// <summary> 
        /// Trạng thái 
        /// </summary> 
        public Nullable<int> Status { get; set; }
        /// <summary> 
        /// Mô tả 
        /// </summary> 
        public string Description { get; set; }

        public string ImageLink { get; set; }
        /// <summary> 
        /// CreateBy 
        /// </summary> 
        public string CreateBy { get; set; }
        /// <summary> 
        /// CreateDate 
        /// </summary> 
        public Nullable<DateTime> CreateDate { get; set; }
        /// <summary> 
        /// UpdateBy 
        /// </summary> 
        public string UpdateBy { get; set; }
        /// <summary> 
        /// UpdateDate 
        /// </summary> 
        public Nullable<DateTime> UpdateDate { get; set; }
        /// <summary> 
        /// Trạng thái xóa
        /// </summary> 
        public int DeleteFlg { get; set; }
        public decimal Delay;
        public decimal? DelayOld;
        public decimal? DelayNew;
        public decimal Work;
        public decimal WorkOld;
        public decimal WorkNew;
        
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string UserType { get; set; }

        public string ConfirmNewPassword { get; set; }

        ///// <summary>
        ///// Dánh sách quyền người dùng
        ///// </summary>
        //public List<FunctionModel> ListPermission { get; set; }
        //public List<PatientsModel> listApp { get; set; }

        //Cho tìm kiếm
        public string ManagementUnitId { get; set; }
        //
        public int Count;
        public int CountOld;
        public int CountNew;
        //Log người dùng
        public string LogUserId { get; set; }
        public string ViolationEventId { get; set; }
        public int LogGroup { get; set; }
        public string AssistantId { get;  set; }

        public string DoctorLocation;
        public int TherapyIndex;
    }
}
