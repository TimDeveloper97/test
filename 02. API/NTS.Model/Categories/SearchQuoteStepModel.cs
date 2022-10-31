using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Categories
{
    public class SearchQuoteStepModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public bool IsEnable { get; set; }
        public string Id { get; set; }
        public List<QuoteStepModel> ListQuote { get; set; }
        public string SBUId { get; set; }
        

    }
}
