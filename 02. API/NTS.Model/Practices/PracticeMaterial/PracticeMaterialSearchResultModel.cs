using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeMaterial
{
    public class PracticeMaterialSearchResultModel<T> : SearchResultModel<T>
    {
        public decimal MaxPricing { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
