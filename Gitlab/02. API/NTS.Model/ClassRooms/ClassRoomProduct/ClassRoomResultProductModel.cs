using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomResultProductModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Pricing { get; set; }
        public string ProductGroupId { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public string ProductGroupName { get; set; }
        public string Description { get; set; }
    }
}
