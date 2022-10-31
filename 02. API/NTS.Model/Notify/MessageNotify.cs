using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Notify
{
    public class MessageNotify
    {
        /// <summary>
        /// ID Process
        /// </summary>
        public ulong ProcessId { get; set; }

        /// <summary>
        /// Ngày gửi notify
        /// </summary>
        public DateTime SentDate { get; set; }

        /// <summary>
        /// Loại notify
        /// </summary>
        public NotifyType Type { get; set; }

        /// <summary>
        /// Object thông tin notify
        /// </summary>
        public object MessageContent { get; set; }
    }

    public enum NotifyType
    {
        /// <summary>
        /// Thông báo lên thiết bị mobile
        /// </summary>
        Mobile = 0,
        /// <summary>
        /// Thông báo lên web
        /// </summary>
        SignalR = 1,
        /// <summary>
        /// Gửi email
        /// </summary>
        Email = 2,
        /// <summary>
        /// Gửi SMS
        /// </summary>
        SMS = 3,
        /// <summary>
        /// Gửi email outlook
        /// </summary>
        EmailOutlook = 4,
    }
}
