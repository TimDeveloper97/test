using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RawMaterialHistory
{
    public class RawMaterialHistoryModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> Index { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public string MaterialId { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
