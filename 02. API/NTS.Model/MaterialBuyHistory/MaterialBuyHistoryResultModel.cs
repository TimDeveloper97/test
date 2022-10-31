using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialBuyHistory
{
    public class MaterialBuyHistoryResultModel
    {
        public List<MaterialBuyHistoryModel> ListMaterialBuyHistory { get; set; }
        public int TotalItem { get; set; }
    }
}
