using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAFile
{
    public class ProductStandardTPAFileModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductStandardTPAId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTemplateId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<decimal> FileSize { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
        public bool IsDocument { get; set; }
        public string DocumentGroupCode { get; set; }
    }
}
