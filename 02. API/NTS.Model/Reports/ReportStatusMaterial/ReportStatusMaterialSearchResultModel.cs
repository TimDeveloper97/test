using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusMaterial
{
    public class ReportStatusMaterialSearchResultModel<T> : SearchResultModel<T>
    {
        public int TotalModule { get; set; }
        public decimal TotalMaterial { get; set; }
    }
}
