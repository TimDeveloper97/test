using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class WorkingReportModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ContractName { get; set; }
        public string StageName { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int NumberDate { get; set; }
        public string ContractorName { get; set; }
        public string PlanUser { get; set; }
        public int Status { get; set; }
        public int DoneRatio { get; set; }
        public string Comment { get; set; }
        public DateTime? DateFromActualStartDate { get; set; }
        public DateTime? DateToActualStartDate { get; set; }
        public DateTime? DateFromActualEndDate { get; set; }
        public DateTime? DateToActualEndDate { get; set; }
        public string ProjectId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public string Types { get; set; }
        public string Statuss { get; set; }
    }
}
