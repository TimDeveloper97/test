using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class RawMaterialModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Index { get; set; }
        public string Note { get; set; }
    }
}
