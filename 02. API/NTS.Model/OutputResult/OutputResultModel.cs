using NTS.Model.Document;
using NTS.Model.FlowStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.OutputResult
{
    public class OutputResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FlowStageId { get; set; }
        public string FlowStageName { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string FlowStagesCode { get; set; }

        public List<FlowStageModel> FlowStages { get; set; }
        public List<DocumentSearchResultModel> Documents { get; set; }
        public OutputResultModel()
        {
            FlowStages = new List<FlowStageModel>();
            Documents = new List<DocumentSearchResultModel>();
        }
    }
}
