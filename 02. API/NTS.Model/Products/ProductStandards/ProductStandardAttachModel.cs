using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public decimal FileSize { get; set; }
        public string CreateByName { get; set; }
    }
}
