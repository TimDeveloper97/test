using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteriaHistory
{
    public class TestCriteriasHistoryModel
    {
        public string Id { get; set; }
        public string TestCriteriaGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TechnicalRequirements { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
