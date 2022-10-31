using NTS.Model.Document;
using NTS.Model.GeneralTemplate;
using NTS.Model.Materials;
using NTS.Model.OutputResult;
using NTS.Model.Skills;
using NTS.Model.Task;
using NTS.Model.WorldSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskFlowStage
{
    public class TaskFlowStageModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsDesignModule { get; set; }
        public Nullable<decimal> TimeStandard { get; set; }
        public string DegreeId { get; set; }
        public string Specialization { get; set; }
        public string SpecializeId { get; set; }
        public string WorkTypeRId { get; set; }
        public string WorkTypeAId { get; set; }
        public string WorkTypeSId { get; set; }
        public string WorkTypeCId { get; set; }
        public string WorkTypeIId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string FlowStageId { get; set; }
        public bool IsProjectWork { get; set; }
        public string WorkTypeId { get; set; }

        public List<TaskWorkTypeModel> TaskWorkTypes { get; set; }
        public Nullable<decimal> PercentValue { get; set; }

        public List<WorkSkillModel> Skills { get; set; }
        public List<DocumentSearchResultModel> Documents { get; set; }
        public List<MaterialResultModel> Materials { get; set; }

        public List<OutputResultModel> OutputResults { get; set; }
        public List<OutputResultModel> InputResults { get; set; }

        public TaskFlowStageModel()
        {
            Skills = new List<WorkSkillModel>();
            Documents = new List<DocumentSearchResultModel>();
            Materials = new List<MaterialResultModel>();
            OutputResults = new List<OutputResultModel>();
            InputResults = new List<OutputResultModel>();
        }

    }
}
