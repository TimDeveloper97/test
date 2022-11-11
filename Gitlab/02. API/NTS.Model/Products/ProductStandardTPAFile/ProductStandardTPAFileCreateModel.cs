using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAFile
{
    public class ProductStandardTPAFileCreateModel : BaseModel
    {
        public string ProductStandardTPAId { get; set; }
        public List<ProductStandardTPAFileModel> ListFile { get; set; }
        public ProductStandardTPAFileCreateModel()
        {
            ListFile = new List<ProductStandardTPAFileModel>();
        }
    }
}
