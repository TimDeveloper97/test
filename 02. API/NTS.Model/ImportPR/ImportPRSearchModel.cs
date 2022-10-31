using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportPR
{
    public class ImportPRSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public bool? Status { get; set; }
        public string Name { get; set; }
        public string ProjectCode { get; set; }
        public string PRCode { get; set; }
        public string ManufactureId { get; set; }
        public string EmployeeId { get; set; }
        public List<string> ListId { get; set; }
        public ImportPRSearchModel()
        {
            ListId = new List<string>();
        }
    }
}
