using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductMaterials
{
    public class ProductMaterialSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public List<string> ListModule { get; set; }
    }
}
