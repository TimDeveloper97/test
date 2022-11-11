using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace NTS.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert string yyyy-MM-dd to dd/MM/yyyy
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>dd/MM/yyyy</returns>
        public static string ConvertDateYMDToDMY(this string stringValue)
        {
            try
            {
                DateTime datetTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                return datetTime.ToString("dd/MM/yyyy");
            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static int DateYMDDiffNowToDay(this string stringValue)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                if (DateTime.Now.Date <= dateTime.Date)
                {
                    return (dateTime - DateTime.Now).Days;
                }

                return 0;

            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        public static decimal ConvertToDecimal(this string stringValue)
        {
            try
            {
                if (string.IsNullOrEmpty(stringValue)|| stringValue.Trim().Equals("-"))
                {
                    return 0;
                }

                return decimal.Parse(stringValue);

            }
            catch
            {
                return 0;
            }
        }
    }
}