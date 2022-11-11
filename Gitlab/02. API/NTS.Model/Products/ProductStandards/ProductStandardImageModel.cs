using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Products.ProductStandards
{
    public class ProductStandardImageModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Note { get; set; }
    }
}
