using NTS.Model.Projects.ProjectProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProducts
{
    public class ProductsItemModel : BaseModel
    {
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public int QCStatus { get; set; }
        public string ProjectProductId { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public List<string> ImageLink { get; set; }
        public int TotalNG { get; set; }
        public int Total { get; set; }
        public List<ProductStandardGroupModel> ListStandardsGroup { get; set; }
        public string SerialNumber { get; set; }

        public ProductsItemModel()
        {
            ListStandardsGroup = new List<ProductStandardGroupModel>();
        }
    }
}
