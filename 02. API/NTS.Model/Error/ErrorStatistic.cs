using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorStatistic
    {
        public int TotalIssues { get; set; }
        public int TotalLated { get; set; }
        public int TotalNeedClose { get; set; }
        public int TotalNeedConfirm { get; set; }
    }
}
