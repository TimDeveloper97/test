using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FunctionHistory
{
    public class FunctionHistoryModel
    {
        public string Id { get; set; }
        public string FunctionGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TechnicalRequire { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }

    }
}
