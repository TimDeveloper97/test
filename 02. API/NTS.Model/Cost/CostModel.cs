using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Cost
{
    public class CostModel
    {
        public string Id { get; set; }
        public int Month { get; set; }
        public decimal Year { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? CheckEstimatedCost { get; set; }
        public decimal? RealCost { get; set; }
        public decimal? CheckRealCost { get; set; }
        public decimal? StatusCost { get; set; }
        public double NextMonthCost { get; set; }
    }
}
