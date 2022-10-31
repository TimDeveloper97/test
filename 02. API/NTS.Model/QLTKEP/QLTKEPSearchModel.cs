using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKEP
{
    public class QLTKEPSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string JobPositionId { get; set; }
        public string PhoneNumber { get; set; }
        public int? Gender { get; set; }
    }
}
