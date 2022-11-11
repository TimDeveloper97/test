using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomProduct
{
    public class ClassRoomProductModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string PracticeGroupName { get; set; }
        public string Name { get; set; }
        public string ClassRoomId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Pricing { get; set; }
    }
}
