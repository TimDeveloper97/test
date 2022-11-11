using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class ProductStandardTPATypeModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Index { get; set; }
        public string Note { get; set; }
        public List<ProductStandardTPATypeModel> ListProductStandardTPATypeParent { get; set; }

        public ProductStandardTPATypeModel()
        {
            ListProductStandardTPATypeParent = new List<ProductStandardTPATypeModel>();
        }
    }
}
