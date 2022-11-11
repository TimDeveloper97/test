using NTS.Model.CodeRule;
using NTS.Model.FileDefinition;
using NTS.Model.FolderDefinition;
using NTS.Model.Manufacture;
using NTS.Model.MaterialGroup;
using NTS.Model.Materials;
using NTS.Model.QLTKMODULE;
using NTS.Model.RawMaterial;
using NTS.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DataCheckModuleUpload
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
        public List<DataCheckModuleModel> Modules { get; set; }
        public List<CodeRuleModel> ListCodeRule { get; set; }
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
            Modules = new List<DataCheckModuleModel>();
        }
    }

    public class DataCheckModuleModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
