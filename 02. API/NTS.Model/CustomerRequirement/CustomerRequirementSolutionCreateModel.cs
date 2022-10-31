using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementSolutionCreateModel
    {
        public string CustomerRequirementId { get; set; }
        public int? EstimateState { get; set; }
        public List<CustomerRequirementSolutionModel> ListSolution { get; set; }
        public List<ProductNeedPriceModel> ListProductNeedPrice { get; set; }
        public List<ProductNeedSolutionModel> ListProductNeedSolution { get; set; }
        public string ProductNeedSolutionId { get; set; }
        public List<string> SolutionIds { get; set; }
        public string SolutionId { get; set; }
        public List<ProtectSolutionModel> ListProtectSolution { get; set; }

        public CustomerRequirementSolutionCreateModel()
        {
            ListSolution = new List<CustomerRequirementSolutionModel>();
            ListProductNeedPrice = new List<ProductNeedPriceModel>();
            ListProductNeedSolution = new List<ProductNeedSolutionModel>();
            ListProtectSolution = new List<ProtectSolutionModel>();
        }
    }
}
