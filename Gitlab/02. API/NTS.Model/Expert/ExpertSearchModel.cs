using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.Expert
{
    public class ExpertSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string BankAccountName { get; set; }

        public string SpecializeId { get; set; }
        //public string SpecializeName { get; set; }
        //public string DegreeName { get; set; }
        public string DegreeId { get; set; }
        //public string WorkplaceName { get; set; }
        public string WorkPlaceId { get; set; }
        public bool IsExport { get; set; }
        public List<string> ListIdSelect { get; set; }
    }
}
