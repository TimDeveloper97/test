using NTS.Model.ProductCatalog;
using NTS.Model.ProductDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductDocumentAttach
{
    public class ProductDocumentAttachModel
    {
        public List<ProductDocumentModel> ListFileAttach { get; set; }
        public List<ProductDocumentModel> ListGuidePractice { get; set; }
        public List<ProductDocumentModel> ListQuotation { get; set; }
        public List<ProductDocumentModel> ListDrawingLayout { get; set; }
        public List<ProductDocumentModel> ListDMVT { get; set; }
        public List<ProductDocumentModel> ListDMBTH { get; set; }
        public List<ProductDocumentModel> ListGuideMaintenance { get; set; }
        public List<ProductCatalogModel> ListFileCatalog { get; set; }
    }
}
