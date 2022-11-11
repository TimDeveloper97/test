using NTS.Model.ModuleMaterials;
using NTS.Model.PracticeFile;
using NTS.Model.PracticeMaterial;
using NTS.Model.PracticeOldVersion;
using NTS.Model.PracticeSkill;
using NTS.Model.ProductDocument;
using NTS.Model.QLTKMODULE;
using NTS.Model.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Practice
{
    public class PracticeModel : BaseModel
    {
        public string Id { get; set; }
        public string PracticeGroupId { get; set; }
        public string PracticeGroupName { get; set; }
        public string PracticeGroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CurentVersion { get; set; }
        public string Note { get; set; }
        public int TrainingTime { get; set; }
        public string SkillName { get; set; }
        public string SkillId { get; set; }
        public decimal LessonPrice { get; set; }
        public decimal HardwarePrice { get; set; }
        public string UnitId { get; set; }
        public int Quantity { get; set; }
        public int LeadTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Content { get; set; }
        public string EditContent { get; set; }
        public bool IsExport { get; set; }
        public List<PracticeSkillModel> listSelect { get; set; }
        public List<PracticeOldVersionModel> ListHistory { get; set; }
        public List<ModuleInPracticeModel> ListModuleInPractice { get; set; }
        public List<SkillsModel> ListSkill { get; set; }
        public List<string> ListSkillId { get; set; }
        public bool? MaterialConsumableExist { get; set; }
        public bool? MaterialConsumable { get; set; } // vật tư tiêu hao
        public int IsMaterialConsumable { get; set; }
        public bool? SupMaterialExist { get; set; }
        public bool? SupMaterial { get; set; }  // thiết bị phụ trợ
        public int IsSupMaterial { get; set; }
        public bool? PracticeExist { get; set; }
        public bool? PracticeFile { get; set; } // tài liệu
        public int IsPracticeFile { get; set; }
        public bool HasDocument { get; set; }
        public bool Check { get; set; }
        public List<object> ListCheckExist { get; set; }
        public List<string> PracaticeId { get; set; }
        public List<string> ListCheckSkill { get; set; }
        public int MaxTotalLeadtime { get; set; }

        public List<ProductDocumentModel> ListGuidePractice { get; set; }
        public List<PracticeFileModel> ListPracticeFile { get; set; }

        public PracticeModel()
        {
            ListModuleInPractice = new List<ModuleInPracticeModel>();
            ListSkillId = new List<string>();
            ListCheckSkill = new List<string>();
            ListGuidePractice = new List<ProductDocumentModel>();
            ListPracticeFile = new List<PracticeFileModel>();
        }
    }

    public class ModuleInPracticeModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleGroupId { get; set; }
        public string ModuleGroupCode { get; set; }
        public string PracticeId { get; set; }
        public int Qty { get; set; }
        public int Version { get; set; }
        public string ModuleName { get; set; }
        public string Specification { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string ModuleCode { get; set; }
        public decimal Pricing { get; set; }
        public decimal Amount { get; set; }
        public bool IsNoPrice { get; set; }
        public List<ModuleMaterialResultModel> ListMaterial { get; set; }
        public ModuleInPracticeModel()
        {
            ListMaterial = new List<ModuleMaterialResultModel>();
        }
        public string Index { get; set; }
        public int LeadTime { get; set; }
        public int MaxQtyByPractice { get; set; }
    }

    public class ExportPracticeModel
    {
        public string Index { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CurentVersion { get; set; }
        public string PracticeFile { get; set; }
        public string SupMaterial { get; set; }
        public string MaterialConsumable { get; set; }
        public int TrainingTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Note { get; set; }
    }
}