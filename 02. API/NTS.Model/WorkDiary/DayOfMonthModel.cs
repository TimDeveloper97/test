using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class DayOfMonthModel
    {
        public bool IsHoliday { get; set; }
        public decimal TotalTime { get; set; }
        public decimal TotalTimeDay { get; set; }
        public decimal TimeDay{ get; set; }
        public int Day { get; set; }
        public bool ExitDay { get; set; }
        public DateTime DateTime { get; set; }

        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }

    }
}
