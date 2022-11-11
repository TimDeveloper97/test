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
    public class DataCheckModuleUploadModel
    {
        public bool SuccessStatus { get; set; }
        public string Message { get; set; }
        public List<MaterialFromDBModel> ListMaterialModel { get; set; }
        public List<RawMaterialModel> ListRawMaterialsModel { get; set; }
        public List<ManufactureResultModel> ListManufacturerModel { get; set; }
        public List<FolderDefinitionModel> ListFolderDefinition { get; set; }
        public List<FileDefinitionModel> ListFileDefinition { get; set; }
        public List<MaterialGroupFromDBModel> ListMaterialGroupModel { get; set; }
        public List<UnitModel> ListUnitModel { get; set; }
        public List<CodeRuleModel> ListCodeRule { get; set; }
        public List<MaterialModel> ListMaterial { get; set; }
        public List<DataCheckModuleModel> Modules { get; set; }
        public DataCheckModuleUploadModel()
        {
            ListMaterialModel = new List<MaterialFromDBModel>();
            ListRawMaterialsModel = new List<RawMaterialModel>();
            ListManufacturerModel = new List<ManufactureResultModel>();
            ListFolderDefinition = new List<FolderDefinitionModel>();
            ListFileDefinition = new List<FileDefinitionModel>();
            ListMaterialGroupModel = new List<MaterialGroupFromDBModel>();
            ListUnitModel = new List<UnitModel>();
            ListCodeRule = new List<CodeRuleModel>();
            ListMaterial = new List<MaterialModel>();
            Modules = new List<DataCheckModuleModel>();
        }
    }
}
