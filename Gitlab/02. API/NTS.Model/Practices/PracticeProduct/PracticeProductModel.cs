using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeProduct
{
    public class PracticeProductModel
    {
        public string Id { get; set; }
        public string PracticeId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public int Version { get; set; }
    }
}
