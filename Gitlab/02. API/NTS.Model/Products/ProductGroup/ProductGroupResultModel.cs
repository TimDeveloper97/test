using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductGroup
{
    public class ProductGroupResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }


        public string Index { get; set; }

        public string IndexView { get; set; }
    }
}
