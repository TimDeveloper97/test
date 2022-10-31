using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Exceptions
{
    public class ExceptionInfoModel
    {
        /// <summary>
        /// Nội dung lỗi
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Method phát sinh lỗi
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Dong lỗi
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// File lỗi
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Link đến lỗi
        /// </summary>
        public string Router { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Dữ liệu gây lỗi
        /// </summary>
        public object Data { get; set; }
    }
}
