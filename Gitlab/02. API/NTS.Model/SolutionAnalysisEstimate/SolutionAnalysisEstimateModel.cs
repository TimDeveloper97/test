using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionAnalysisEstimate
{
    public class SolutionAnalysisEstimateModel : BaseModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string MaterialId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string Note { get; set; }
        public string MaterialGroupName { get; set; }
        public string ManufactureCode { get; set; }
        public string Code { get; set; }
        public String Name { get; set; }
        public decimal Pricing { get; set; }
        public int Quantity { get; set; }

    }
}
