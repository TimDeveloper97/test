using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionAnalysisEstimate
{
    public class SolutionAnalysisSupplierModel : BaseModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public String PhoneNumber { get; set; }
        public string Country { get; set; }
        public string CodeProduct { get; set; }
        public string NameProduct { get; set; }
    }
}
