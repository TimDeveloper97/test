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
        public static string ToStringYYMMDDHHMMSSFFF(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("yyyyMMddHHmmssfff");
        }
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
            return dateTimeValue.HasValue ? dateTimeValue.Value.ToString("yyyy-MM-dd") : "";
        }

        /// <summary>
        /// Convert datetime to yyyy-MM-dd
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string ToStringYYMMDD(this DateTime dateTimeValue)
        {
            return dateTimeValue.ToString("yyyy-MM-dd");
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

        /// <summary>
        /// Lầy thứ trong tuần
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static string GetDayOfWWeek(this DateTime dateTimeValue)
        {
            switch (dateTimeValue.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "T2";
                case DayOfWeek.Tuesday:
                    return "T3";
                case DayOfWeek.Wednesday:
                    return "T4";
                case DayOfWeek.Thursday:
                    return "T5";
                case DayOfWeek.Friday:
                    return "T6";
                case DayOfWeek.Saturday:
                    return "T7";
                case DayOfWeek.Sunday:
                    return "CN";
                default:
                    return string.Empty;
            }
        }

        public static DateTime ConvertDateFromStr(string date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.ParseExact(date + " 00:00:00", "dd/MM/yyyy HH:mm:ss", null);

            return returnValue;
        }

        public static DateTime ConvertDateToStr(string date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.ParseExact(date + " 23:59:59", "dd/MM/yyyy HH:mm:ss", null);

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek">Ngày bắt đầu tuần</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}