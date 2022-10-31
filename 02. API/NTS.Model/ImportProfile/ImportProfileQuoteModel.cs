using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportProfileQuoteModel
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Code { get; set; }
        public DateTime? QuoteDate { get; set; }
        public int Effect { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string UploadName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
