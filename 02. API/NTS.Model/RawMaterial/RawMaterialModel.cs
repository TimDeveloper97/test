using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RawMaterial
{
    public class RawMaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Index { get; set; }
        public string Note { get; set; }
        public string MaterialId { get; set; }
        public string MaterialCode { get; set; }
    }
}
