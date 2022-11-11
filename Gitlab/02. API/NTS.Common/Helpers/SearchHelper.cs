using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    public class SearchHelper
    {
        public static void GetDateFromDateToByTimeType(string timeType, int year, int month, int quarter,
            ref DateTime dateFrom, ref DateTime dateTo)
        {
            List<string> labels = new List<string>();
            switch (timeType)
            {
                case Constants.TimeType_Today:
                    {
                        dateFrom = DateTime.Now.ToStartDate();
                        dateTo = DateTime.Now.ToEndDate();
                        break;
                    }
                case Constants.TimeType_Yesterday:
                    {
                        dateFrom = DateTime.Now.AddDays(-1).ToStartDate();
                        dateTo = DateTime.Now.AddDays(-1).ToEndDate();
                        break;
                    }
                case Constants.TimeType_ThisWeek:
                    {
                        DayOfWeek day = DateTime.Now.DayOfWeek;
                        int days = day - DayOfWeek.Monday;

                        dateFrom = DateTime.Now.AddDays(-days).ToStartDate();
                        dateTo = dateFrom.AddDays(6).ToEndDate();
                        break;
                    }
                case Constants.TimeType_LastWeek:
                    {
                        DateTime dayOfLastWeek = DateTime.Now.AddDays(-7);
                        if (dayOfLastWeek.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dateTo = dayOfLastWeek.ToEndDate();
                            dateFrom = dayOfLastWeek.AddDays(-6).ToStartDate();
                        }
                        else
                        {
                            dateFrom = dayOfLastWeek.AddDays(DayOfWeek.Monday - dayOfLastWeek.DayOfWeek).ToStartDate();
                            dateTo = dateFrom.AddDays(6).ToEndDate();
                        }
                        break;
                    }
                case Constants.TimeType_SevenDay:
                    {
                        dateFrom = DateTime.Now.AddDays(-6).ToStartDate();
                        dateTo = DateTime.Now.ToEndDate();
                        break;
                    }
                case Constants.TimeType_ThisMonth:
                    {
                        dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToStartDate();
                        dateTo = dateFrom.AddMonths(1).AddDays(-1).ToEndDate();
                        break;
                    }
                case Constants.TimeType_LastMonth:
                    {
                        DateTime lastMonthDate = DateTime.Now.AddMonths(-1);
                        dateFrom = new DateTime(lastMonthDate.Year, lastMonthDate.Month, 1).ToStartDate();
                        dateTo = dateFrom.AddMonths(1).AddDays(-1).ToEndDate();
                        break;
                    }
                case Constants.TimeType_Month:
                    {
                        dateFrom = new DateTime(year, month, 1).ToStartDate();
                        dateTo = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToEndDate();
                        break;
                    }
                case Constants.TimeType_Quarter:
                    {
                        if (quarter == 1)
                        {
                            dateFrom = new DateTime(year, 1, 1).ToStartDate();
                            dateTo = new DateTime(year, 3, DateTime.DaysInMonth(year, 3)).ToEndDate();
                        }
                        else if (quarter == 2)
                        {
                            dateFrom = new DateTime(year, 4, 1).ToStartDate();
                            dateTo = new DateTime(year, 6, DateTime.DaysInMonth(year, 6)).ToEndDate();
                        }
                        else if (quarter == 3)
                        {
                            dateFrom = new DateTime(year, 7, 1).ToStartDate();
                            dateTo = new DateTime(year, 9, DateTime.DaysInMonth(year, 9)).ToEndDate();

                        }
                        else if (quarter == 4)
                        {
                            dateFrom = new DateTime(year, 10, 1).ToStartDate();
                            dateTo = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToEndDate();
                        }
                        break;
                    }
                case Constants.TimeType_ThisYear:
                    {
                        dateFrom = new DateTime(DateTime.Now.Year, 1, 1).ToStartDate();
                        dateTo = new DateTime(DateTime.Now.Year, 12, 31).ToEndDate();
                        break;
                    }
                case Constants.TimeType_LastYear:
                    {
                        DateTime lastYearDate = DateTime.Now.AddYears(-1);
                        dateFrom = new DateTime(lastYearDate.Year, 1, 1).ToStartDate();
                        dateTo = new DateTime(lastYearDate.Year, 12, 31).ToEndDate();
                        break;
                    }
                case Constants.TimeType_Year:
                    {
                        dateFrom = new DateTime(year, 1, 1).ToStartDate();
                        dateTo = new DateTime(year, 12, 31).ToEndDate();
                        break;
                    }
                default:
                    {
                        dateFrom = DateTime.Now.ToStartDate();
                        dateTo = DateTime.Now.ToEndDate();
                        break;
                    }
            }
        }
    }
}
