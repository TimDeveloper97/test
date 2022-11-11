using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class CustomerRequirementModel
    {
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerRequirementId { get; set; }
        public string CustomerName { get; set; }
        public string NumberRequire { get; set; } //Số YCKH
        public string ContentRequire { get; set; } //Nội dung yêu cầu

        public string QuotesId { get; set; }
        public string QuotesCode { get; set; }
        public string QuotesName { get; set; }
        public int SuccessRatio { get; set; }
        public int Index { get; set; }

    }
}
