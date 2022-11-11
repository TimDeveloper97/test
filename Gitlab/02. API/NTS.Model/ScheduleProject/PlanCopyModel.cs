using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class PlanCopyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ContractIndex { get; set; }
        public int DataType { get; set; }
        public List<PlanCopyCheckModel> ListCheck { get; set; }
        public PlanCopyModel()
        {
            ListCheck = new List<PlanCopyCheckModel>();
        }
    }

    public class PlanCopyCheckModel
    {
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public string StageId { get; set; }
        public bool Checked { get; set; }
    }
}
