using NTS.Model.Document;
using NTS.Model.Materials;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkTypeSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkType
{
    public class WorkTypeModel
    {
        public string Id { get; set; }
        public decimal Quantity { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string FlowStageId { get; set; }
        public string SalaryGroupId { get; set; }
        public string SalaryTypeId { get; set; }
        public string SalaryLevelMinId { get; set; }
        public string SalaryLevelMaxId { get; set; }
        public decimal Value { get; set; }

        public List<WorkTypeSkillModel> ListWorkTypeSkill { get; set; }

        public List<MaterialResultModel> Materials { get; set; }
        public List<TaskFlowStageModel> Tasks { get; set; }
        public List<WorkTypeDocumentModel> Documents { get; set; }

        public WorkTypeModel()
        {
            ListWorkTypeSkill = new List<WorkTypeSkillModel>();
            Materials = new List<MaterialResultModel>();
            Tasks = new List<TaskFlowStageModel>();
            Documents = new List<WorkTypeDocumentModel>();
        }
    }
}
