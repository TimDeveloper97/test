using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.FTP
{
    public class NtsFTPResult
    {
        public FtpStatus FtpStatus { get; set; }
        public string Message { get; set; }
    }

    public enum FtpStatus
    {
        /// <summary>
        /// Xử lý lỗi
        /// </summary>
        Failed = 0,

        /// <summary>
        /// Thành công
        /// </summary>
        Success = 1
    }
}
