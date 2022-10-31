using NTS.Model.Practice;
using NTS.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeProduct
{
    public class PracticeProductSearchResultModel
    {
        public List<ProductResultModel> Products { get; set; }
        public List<ModuleInPracticeModel> Modules { get; set; }

        public PracticeProductSearchResultModel()
        {
            Products = new List<ProductResultModel>();
            Modules = new List<ModuleInPracticeModel>();
        }
    }
}
