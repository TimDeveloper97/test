using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuoteDocumentModel : BaseModel
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal Size { get; set; }
        public string Extention { get; set; }
        public string Thumbnail { get; set; }
        public string HashValue { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
