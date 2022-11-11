using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardHistoryModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }
        public string CreateByName { get; set; }
    }
}
