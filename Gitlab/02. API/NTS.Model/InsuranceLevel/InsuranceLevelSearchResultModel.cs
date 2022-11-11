using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.InsuranceLevel
{
    public class InsuranceLevelSearchResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Money { get; set; }
        public string Note { get; set; }
    }
}
