
using NTS.Model.GroupModule;
using NTS.Model.ProductStandards;
using System.Collections.Generic;

namespace NTS.Model.QLTKGROUPMODUL
{
    public class GroupModuleModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string DepartementId { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Index { get; set; }

        public List<ProductStandardsModel> ListProductStandard { get; set; }
        public List<GroupModuleStageModel> ListStage { get; set; }
        public List<ModuleGroupTestCriteiaModel> ListTestCriteri { get; set; }
    }
}
