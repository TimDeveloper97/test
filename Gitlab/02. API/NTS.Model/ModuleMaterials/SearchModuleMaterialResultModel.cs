using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterials
{
    public class SearchModuleMaterialResultModel<T> : SearchResultModel<T>
    {
        public decimal TotalAmount { get; set; }
    }
}
