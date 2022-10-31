using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportProblemExist
{
    public class ReportProblemExistSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime? DateToV { get; set; }
        public DateTime? DateFromV { get; set; }
    }
}
