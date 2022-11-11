using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Common
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Convert datetime to HH:mm dd/MM/yyyy
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringHHMMDDMMYY(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("HH:mm dd/MM/yyyy");
        }

        /// <summary>
        /// Convert datetime to dd/MM/yyyy
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringDDMMYY(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Convert datetime to yyyy-MM-dd
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringYYMMDD(this DateTime? dateTimeValue)
        {
            return dateTimeValue.HasValue? dateTimeValue.Value.ToString("yyyy-MM-dd"):"";
        }

        /// <summary>
        /// Convert datetime to yyyy-MM-dd
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringYYMMDD(this DateTime dateTimeValue)
        {
            return  dateTimeValue.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Convert datetime về cuối ngày
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static DateTime ToEndDate(this DateTime dateTimeValue)
        {
            return DateTime.ParseExact(dateTimeValue.ToStringDDMMYY() + " 23:59:59", "dd/MM/yyyy HH:mm:ss", null);
        }

        /// <summary>
        /// Convert datetime về đầu ngày
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static DateTime ToStartDate(this DateTime dateTimeValue)
        {
            return DateTime.ParseExact(dateTimeValue.ToStringDDMMYY() + " 00:00:00", "dd/MM/yyyy HH:mm:ss", null);
        }
    }
}