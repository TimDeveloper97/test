using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class SaleProductExportModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ExpiredDate { get; set; }
        public int Status { get; set; }
    }
}
