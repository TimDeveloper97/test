using NTS.Common.Helpers;
using NTS.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common
{
    public class NTSLogException : Exception
    {
        /// <summary>
        /// Thông tin lỗi
        /// </summary>
        public ExceptionInfoModel ExceptionInfo { get; set; }

        /// <summary>
        /// Contructor with error message and inner exception.
        /// </summary>
        /// <param name="message">the error message</param>
        /// <param name="innerException">the inner exception</param>
        public NTSLogException(object param, Exception innerException)
            : base(innerException.Message, innerException)
        {
            ExceptionInfo = innerException.GetExceptionInfo();
            ExceptionInfo.Data = param;
        }
    }
}
