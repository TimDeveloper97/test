using NTS.Model.Datasheet;
using NTS.Model.Designer;
using NTS.Model.Employees;
using NTS.Model.Module;
using NTS.Model.ModuleManualDocument;
using NTS.Model.ModuleOldVersion;
using NTS.Model.ModulePart;
using NTS.Model.ModuleProductionTime;
using NTS.Model.ModuleTestCriteria;
using NTS.Model.ProductMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKMODULE
{
    public class ModuleModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleGroupId { get; set; }
        public string ProjectId { get; set; }
        public string DepartermentIdByRequest { get; set; }
        public string ModuleGroupName { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public int State { get; set; }
        public string Specification { get; set; }
        public bool FileElectric { get; set; }
        public bool FileElectronic { get; set; }
        public bool FileMechanics { get; set; }
        public bool IsSoftware { get; set; }
        public decimal Pricing { get; set; }
        public bool IsHMI { get; set; }
        public bool IsPLC { get; set; }
        public int CurrentVersion { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsFilm { get; set; }
        public bool IsDMTV { get; set; }
        public string Description { get; set; }
        public string EditContent { get; set; }
        public List<ModulePartModel> ListModulePartModel { get; set; }
        // Extent
        public bool ElectricExist { get; set; }
        public bool ElectronicExist { get; set; }
        public bool MechanicsExist { get; set; }
        public bool SoftwareExist { get; set; }
        public bool HMIExist { get; set; }
        public bool PLCExist { get; set; }
        public bool FilmExist { get; set; }
        /// <summary>
        /// Tình trạng dữ liệu
        /// </summary>
        public bool DataStatus { get; set; }
        public decimal Quantity { get; set; }

        public List<ModuleProductionTimeModel> ListStage { get; set; }
        public List<ModuleManualDocumentModel> ListModuleManualDocument { get; set; }
        public List<ModuleImageModel> ListImage { get; set; }
        public List<ModuleOldVersionModel> ListHistory { get; set; }
        public List<string> ListTestCriteriaModule { set; get; }
        public List<ModuleModel> ListModule { get; set; }
        public string ModuleId { get; set; }
        public string MaterialId { get; set; }
        public List<FileSetupModel> ListFileSetup { get; set; }
        public List<DatasheetModel> ListFileDatasheet { get; set; }
        public int LeadTime { get; set; }
        public string Creator { get; set; }
        public string DepartmentCreated { get; set; }
        public string ModuleGroupCode { get; set; }

        public bool IsManual { get; set; }
        public bool ManualExist { get; set; }
        public ModuleModel()
        {
            ListStage = new List<ModuleProductionTimeModel>();
            ListTestCriteriaModule = new List<string>();
        }
    }
    public class ModuleTestCriteriaModule : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string TestCriteriaId { get; set; }
    }
}
