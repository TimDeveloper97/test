using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductModuleUpdate
{
    public class ProductModuleUpdateModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UpdateByName { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
