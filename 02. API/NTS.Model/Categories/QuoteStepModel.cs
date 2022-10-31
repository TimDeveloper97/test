using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Categories
{
    public class QuoteStepModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string SBUId { get; set; }
        public string SBUCode { get; set; }
        public string SBUName { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsEnable { get; set; }
        public int Index { get; set; }
        public int SuccessRadio { get; set; }
    }
}
