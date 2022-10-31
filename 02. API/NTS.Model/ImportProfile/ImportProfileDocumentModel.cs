using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportProfileDocumentModel
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public int Step { get; set; }
        public string Name { get; set; }
        public string SupplierName { get; set; }
        public bool IsRequired { get; set; }
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
