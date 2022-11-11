using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    public static class DataTableHelper
    {
        public static DataTable SortData(this DataTable data, string columnNameSort)
        {
            try
            {
                int maxLen = data.AsEnumerable().Select(s => s[columnNameSort].ToString().Length).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                return data.AsEnumerable()
                        .Select(s =>
                            new
                            {
                                OrgStr = s,
                                SortStr = Regex.Replace(s[columnNameSort].ToString(), @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                            })
                        .OrderBy(x => x.SortStr)
                        .Select(x => x.OrgStr).CopyToDataTable();
            }
            catch
            {
                return data;
            }
        }

    }
}
