using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Categories
{
    public class SearchTechnologySolutionModel : SearchCommonModel
    {
        public int Index { get; set; }
        public bool IsEnable { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string SupplierId { get; set; }
        public string ManufactureId { get; set; }
        public List<TechnologySolutionModel> ListTech { get; set; }
    }
}
