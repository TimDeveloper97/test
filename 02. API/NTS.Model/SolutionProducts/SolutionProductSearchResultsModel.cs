using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionProducts
{
    public class SolutionProductSearchResultsModel 
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ObjectName { get; set; }
        public string ObjectCode { get; set; }
        public int ObjectType { get; set; }
        public string Specification { get; set; }
    }
}
