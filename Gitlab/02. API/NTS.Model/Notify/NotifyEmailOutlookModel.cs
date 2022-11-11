using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Notify
{
    public class NotifyEmailOutlookModel
    {
        /// <summary>
        /// ID Process
        /// </summary>
        public ulong ProcessId { get; set; }

        /// <summary>
        /// Tự động gửi
        /// </summary>
        public bool AutoSend { get; set; }

        /// <summary>
        /// Email nhận
        /// Có thể gửi nhiều email, mỗi email cách nhau bởi dấu ; hoặc ,
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// Tiêu đề thư
        /// </summary>
        public string SubjectTitle { get; set; }

        /// <summary>
        /// Nội dung thư
        /// </summary>
        public string BodyContent { get; set; }

        /// <summary>
        /// Danh sách file đính kèm
        /// </summary>
        public List<string> Attachments { get; set; }
    }
}
