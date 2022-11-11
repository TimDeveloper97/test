using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class UploadSolutionModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public string ParentGroupCode { get; set; }
        public string ParentGroupId { get; set; }
    }
}
