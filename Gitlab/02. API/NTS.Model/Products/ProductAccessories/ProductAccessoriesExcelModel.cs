using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductAccessories
{
    public class ProductAccessoriesExcelModel
    {
        public string ProductId { get; set; }
        public List<ProductAccessoriesModel> ListData { get; set; }
        public ProductAccessoriesExcelModel()
        {
            ListData = new List<ProductAccessoriesModel>();
        }
    }
}
