using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomMaterial
{
    public class PriceMaterialModel
    {
        public string ObjectId { get; set; }
        public int Type { get; set; }
        public string ClassRoomId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
