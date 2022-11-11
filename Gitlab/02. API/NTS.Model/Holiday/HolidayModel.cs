using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Holiday
{
    public class HolidayModel
    {
        public int Year { get; set; }
        public int DayOfWeek { get; set; }
        public string CreateBy { get; set; }
        public DateTime FullDate { get; set; }
        public int Month { get; set; }
        public List<HolidayModel> ListDayOfMonth { get; set; }
        public bool IsChecked { get; set; }
        public HolidayModel()
        {
            ListDayOfMonth = new List<HolidayModel>();
        }
    }
}
