using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common
{
    public class NTSException : Exception
    {
        /// <summary>
        /// Declare Error Code.
        /// </summary>
        private string errorCode;

        /// <summary>
        /// Declare Error Message.
        /// </summary>
        private string message;

        /// <summary>
        /// Contructor.
        /// </summary>
        protected NTSException()
            : this(null) { }

        /// <summary>
        /// Contructor with error message.
        /// </summary>
        /// <param name="message">the error message</param>
        protected NTSException(string message)
            : this(message, null) { }

        /// <summary>
        /// Contructor with error message and inner exception.
        /// </summary>
        /// <param name="message">the error message</param>
        /// <param name="innerException">the inner exception</param>
        protected NTSException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
        }

        /// <summary>
        /// Create new Instance of business exception.
        /// </summary>
        /// <param name="errorCode">the error code</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>instance of business exception</returns>
        public static NTSException CreateInstance(string errorCode, params object[] parameters)
        {
            return CreateInstance(errorCode, null, parameters);
        }

        /// <summary>
        /// Create new Instance of business exception.
        /// </summary>
        /// <param name="errorCode">the error code</param>
        /// <param name="innerException">the inner exception</param>
        /// <param name="parameters">the parameters</param>
        /// <returns>instance of business exception</returns>
        public static NTSException CreateInstance(string errorCode, Exception innerException, params object[] parameters)
        {
            return new NTSException(ResourceUtil.GetResourcesNoLag(errorCode, parameters), innerException) { errorCode = errorCode };
        }

        /// <summary>
        /// The variable contains code string.
        /// </summary>
        /// <value>
        /// the error code.
        /// </value>
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// The variable contains message string.
        /// </summary>
        /// <value>
        /// the error message.
        /// </value>
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this.ErrorCode))
                {
                    return string.Empty;
                }

                return (base.Message);
            }
        }
    }
}
