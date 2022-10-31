using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportProfileSearchResultModel<T> : SearchResultModel<T>
    {
        public int PayWarningTotal { get; set; }
        public int PayExpiredTotal { get; set; }
        public int ProductionWarningTotal { get; set; }
        public int ProductionExpiredTotal { get; set; }
        public int ProductionExpiredWeekTotal { get; set; }
    }
}
