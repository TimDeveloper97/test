using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.InsuranceLevel
{
    public class InsuranceLevelModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Money { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
