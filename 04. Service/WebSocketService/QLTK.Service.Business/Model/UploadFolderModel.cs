using QLTK.Service.Business.Common;
using QLTK.Service.Model.Checkbox;
using QLTK.Service.Model.Definitions;
using QLTK.Service.Model.Materials;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class UploadFolderModel
    {
        public string ApiUrl { get; set; }
        public string ModuleId { get; set; }
        public string Token { get; set; }
        public int DesignType { get; set; }
        public string Path { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleGroupCode { get; set; }
        public string ModuleGroupId { get; set; }
        public List<MaterialFromDBModel> ListMaterialModel { get; set; }
        public List<RawMaterialModel> ListRawMaterialsModel { get; set; } 
        public List<ManufactureResultModel> ListManufacturerModel { get; set; }
        public List<FolderDefinitionModel> ListFolderDefinition { get; set; }
        public List<FileDefinitionModel> ListFileDefinition { get; set; }
        public List<MaterialGroupFromDBModel> ListMaterialGroupModel { get; set; }
        public List<UnitModel> ListUnitModel { get; set; }
        public List<CodeRuleModel> ListCodeRule { get; set; }

        public List<ModuleDesignDocumentModel> ListFileModuleDesignDoc { get; set; }
        public UploadFolderModel()
        {
            ListMaterialModel = new List<MaterialFromDBModel>();
            ListRawMaterialsModel = new List<RawMaterialModel>();
            ListManufacturerModel = new List<ManufactureResultModel>();
            ListFolderDefinition = new List<FolderDefinitionModel>();
            ListFileDefinition = new List<FileDefinitionModel>();
            ListMaterialGroupModel = new List<MaterialGroupFromDBModel>();
            ListUnitModel = new List<UnitModel>();
            ListCodeRule = new List<CodeRuleModel>();
            ListDesignStrcture = new List<DesignStrctureModel>();
            ListFileModuleDesignDoc = new List<ModuleDesignDocumentModel>();
        }

        public List<DesignStrctureModel> ListDesignStrcture { get; set; }
        public CheckModel CheckModel { get; set; }
    }
}
