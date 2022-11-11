using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductCatalog
{
    public class ProductCatalogModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public decimal? FileSize { get; set; }
        public string Note { get; set; }
        public bool IsDocument { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
