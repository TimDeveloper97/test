using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Resource
{
    public class ErrorResourceKey
    {
        /// <summary>
        /// Có lỗi phát sinh trong quá trình xử lý!
        /// </summary>
        public const string ERR0001 = "ERR0001";

        /// <summary>
        /// Thiết kế chưa hoàn thành
        /// </summary>
        public const string ERR0002 = "ERR0002";

        /// <summary>
        /// Không lấy được dữ liệu từ server
        /// </summary>
        public const string ERR0003 = "ERR0003";

        /// <summary>
        /// Thư mục {0} không tồn tại
        /// </summary>
        public const string ERR0004 = "ERR0004";

        /// <summary>
        /// Không đọc được dữ liệu file {0}
        /// </summary>
        public const string ERR0005 = "ERR0005";
    }
}
