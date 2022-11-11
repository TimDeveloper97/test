using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Stage
{
    public class StageModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public decimal Time { get; set; }
        public string CreateBy { get; set; }
        public string StageId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string Color { get; set; }
        public bool IsEnable { get; set; }
        public int index { get; set; }
    }
}
