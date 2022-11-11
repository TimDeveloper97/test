using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialBuyHistory
{
    public class MaterialBuyHistorySearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Filter { get; set; }
    }
}
