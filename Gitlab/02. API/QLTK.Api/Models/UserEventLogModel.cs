// <copyright company="nhantinsoft.vn">
// Author: Vũ Văn Văn
// Created Date: 10/08/2016 17:08
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Api.Models
{
    public class UserEventLogModel
    {
        /// <summary> 
         /// UserEventLogId 
         /// </summary> 
         public string UserEventLogId { get; set; } 
         /// <summary> 
         /// Lỗi vi phạm 
         /// </summary> 
         public string ViolationEventId { get; set; } 
         /// <summary> 
         /// Id nguười dùng
         /// </summary> 
         public string UserId { get; set; } 
         /// <summary> 
         /// Mô tả
         /// </summary> 
         public string Description { get; set; } 
         /// <summary> 
         /// Loại
         /// </summary> 
         public Nullable<int> LogType { get; set; } 
         /// <summary> 
         /// LogGroup 
         /// </summary> 
         public Nullable<int> LogGroup { get; set; } 
         /// <summary> 
         /// CreateDate 
         /// </summary> 
         public Nullable<DateTime> CreateDate { get; set; } 

        public Nullable<DateTime> LogDateFrom { get; set; }

        public Nullable<DateTime> LogDateTo { get; set; }
        public string LogTypeName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        /// <summary> 
        /// Id nguười dùng tìm kiếm
        /// </summary> 
        public string UserIdSearch { get; set; }
    }
}
