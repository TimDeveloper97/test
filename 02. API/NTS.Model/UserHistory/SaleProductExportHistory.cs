using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
  public  class SaleProductExportHistory
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public int Status { get; set; }
        public int Index { get; set; }
        public System.DateTime ExpiredDate { get; set; }
        public int ProductQuantity { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
