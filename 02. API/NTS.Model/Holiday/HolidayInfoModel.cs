using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Holiday
{
    public class HolidayInfoModel
    {
        public long Id { get; set; }
        public int Year { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
