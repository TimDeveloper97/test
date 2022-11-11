using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class CustomerCreateModel
    {
        public string Id { get; set; }
        public string CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string SBUId { get; set; }
        public string Address { get; set; }
        public int? Index { get; set; }
    }
}
