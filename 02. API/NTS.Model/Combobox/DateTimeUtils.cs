using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Combobox
{
    public static class DateTimeUtils
    {
        public static DateTime ConvertDateFrom(DateTime? date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 00:00:00");

            return returnValue;
        }

        public static DateTime ConvertDateTo(DateTime? date)
        {
            if (date == null)
            {
                return new DateTime();
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 23:59:59");

            return returnValue;
        }

        public static DateTime? ConvertStartDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 00:00:00");

            return returnValue;
        }

        public static DateTime? ConvertEndDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            DateTime returnValue = DateTime.Parse(date.Value.ToShortDateString() + " 23:59:59");

            return returnValue;
        }
    }
}