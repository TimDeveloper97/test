using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Unit
{
    public class UnitModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
    }
}
