using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfileProblemExist
{
    public class ImportProfileProblemExistSearchModel : SearchCommonModel
    {
        public string TimeType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Quarter { get; set; }
        public Nullable<int> Month { get; set; }
        public string PRCode { get; set; }
        public string ProjectCode { get; set; }
        public string EmployeeId { get; set; }
        public string SupplierId { get; set; }
        public int? Status { get; set; }
    }
}
