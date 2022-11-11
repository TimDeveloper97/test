using NTS.Model.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class OverallProjectModel
    {
        public string ProjectId { get; set; }
        public string Id { get; set; }
        public string ContractName { get; set; }
        public int DataType { get; set; }
        public string ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleId { get; set; }
        public List<string> listStage { get; set; }
        public int Status { get; set; }
        public string Issue { get; set; }
        public DateTime? DateTo { get; set; }
        public int Index { get; set; }

        public OverallProjectModel()
        {
            listStage = new List<string>();
        }

    }
}
