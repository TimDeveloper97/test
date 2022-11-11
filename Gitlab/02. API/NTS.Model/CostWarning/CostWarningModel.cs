using NTS.Model.Cost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CostWarning
{
    public class CostWarningModel
    {
        public decimal Year { get; set; }
        public List<CostModel> ListCost { get; set; }
        public CostWarningModel()
        {
            ListCost = new List<CostModel>();
        }
    }

    public class TargetCostModel
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}
