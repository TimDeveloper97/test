using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class GrantChartModel
    {
        public int Colspan { get; set; }
        public bool IsColspan { get; set; }
        public bool IsHoliday { get; set; }
        public bool DateNow { get; set; }
        public DateTime? Day { get; set; }
    }
}
