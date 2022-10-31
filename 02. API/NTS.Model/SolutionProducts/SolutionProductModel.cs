using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SolutionProducts
{
    public class SolutionProductModel : BaseModel
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string Specification { get; set; }

    }

    public class ModulePriceModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Pricing { get; set; }
    }

    public class SolutionProductResult
    {
        public string SolutionId { get; set; }
        public List<SolutionProductModel> ListSolutionProduct { get; set; }
    }
}
