using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UnitHistory
{
    public class ProductStandardTPATypeHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
