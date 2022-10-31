using NTS.Model.ProductCatalog;
using NTS.Model.ProductDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductDocumentAttach
{
    public class UpdateProductDocumentModel
    {
        public string Id { get; set; }
        public List<ProductCatalogModel> ListFileCatalog { get; set; }
        public List<ProductDocumentModel> ListFileDocument { get; set; }
        public string UserId { get; set; }
    }
}
