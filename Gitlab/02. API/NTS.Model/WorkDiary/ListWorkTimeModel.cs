using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class ListWorkTimeModel
    {
        public DateTime DateTime { set; get; }
        public decimal EstimateTime { get; set; }
        public bool IsHoliday { get; set; }
    }
}
