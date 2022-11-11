using NTS.Common.Resource;
using QLTK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Common
{
    public class CheckItem
    {
        /// <summary>
        /// kiểm tra chuỗi rỗng hoặc null và có độ dài lớn nhất
        /// </summary>
        /// <param name="text">Chuỗi cần check</param>
        /// <param name="maxLength">Đô dài lớn nhất</param>
        /// <param name="paramete">Chuỗi cần hiển thị. Viết chữ hoa đầu câu</param>
        /// <returns></returns>
        public static bool CheckNullOrMaxLength(string text, int maxLength, string paramete, out string message)
        {
            message = string.Empty;

            // Trường hợp chuỗi rỗng hoặc null
            if (text == null || string.IsNullOrEmpty(text.Trim()))
            {
                message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0007, paramete);
                return false;
            }
            // Quá độ dài cho phép
            else if (text.Length > maxLength)
            {
                message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0008, paramete, maxLength);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra chuỗi null hoặc là kiểu số hay không
        /// </summary>
        /// <param name="text">Chuỗi cần kiểm tra</param>
        /// <param name="paramete">Chuỗi cần hiển thị</param>
        /// <param name="numberType">Kiểu số 1: int; 2: Float; 3: Decimal;</param>
        /// <returns></returns>
        public static bool CheckIsNumber(string text, string paramete, Constants.NumberFormatType numberType, out string message)
        {
            message = string.Empty;
            try
            {
                // Kiểm tra có là kiểu số hay không
                switch (numberType)
                {
                    case Constants.NumberFormatType.INTEREGER:// Kiểu int
                        {
                            int value = int.Parse(text);
                            return true;
                        }
                    case Constants.NumberFormatType.FLOAT:
                        {
                            float value = float.Parse(text);
                            return true;
                        }
                    case Constants.NumberFormatType.DECIMAL:
                        {
                            decimal value = decimal.Parse(text);
                            return true;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                switch (numberType)
                {
                    case Constants.NumberFormatType.INTEREGER:
                        {
                            message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0009, paramete);
                            break;
                        }
                    case Constants.NumberFormatType.FLOAT:
                        {
                            message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0010, paramete);
                            break;
                        }
                    case Constants.NumberFormatType.DECIMAL:
                        {
                            message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0010, paramete);
                            break;
                        }
                }
                return false;
            }
        }

        /// <summary>
        /// Check MaxLenght
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <param name="paramete"></param>
        /// <returns></returns>
        public static bool CheckMaxLength(string text, int maxLength, string paramete, out string message)
        {
            message = string.Empty;
            // Quá độ dài cho phép
            if (text.Length > maxLength)
            {
                message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0008, paramete);
                return false;
            }
            return true;
        }
    }
}
