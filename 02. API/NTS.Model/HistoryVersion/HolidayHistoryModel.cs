using NTS.Model.Holiday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class HolidayHistoryModel
    {
        public int Year { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime FullDate { get; set; }
        public int Month { get; set; }
        public List<HolidayModel> ListDayOfMonth { get; set; }
        public bool IsChecked { get; set; }
        public HolidayHistoryModel()
        {
            ListDayOfMonth = new List<HolidayModel>();
        }
    }
}
