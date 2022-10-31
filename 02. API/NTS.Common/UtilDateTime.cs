using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NTS.Common
{
    public static class UtilDateTime
    {
        public static Nullable<DateTime> SubDateTimeInString(string containingDate, bool isNullReturnNow)
        {
            try
            {
                Regex rx = new Regex(@"([0-9]+[-/. ]([0-9]+|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)[-/. ][0-9]+)");
                Match m = rx.Match(containingDate);
                if (m.Success)
                {
                    var plain = m.Value;
                    return DateTime.Parse(plain + " " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
            }
            return (isNullReturnNow ? DateTime.Now : default(DateTime));
        }
    }
}
