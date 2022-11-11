using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusProduct
{
    public class ReportStatusProductSearchModel : SearchCommonModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProjectId { get; set; }
        public bool IsExccel { get; set; }
    }
}
