using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.LLADGS
{
    public class LLADGS1000Model : BaseModel
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Note { get; set; }
        public string ImageLink { get; set; }
        
    }
}
