using NTS.Model.ModuleError;
using NTS.Model.ModuleMaterials;
using NTS.Model.TestCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKMODULE
{
    public class ModuleResultModel
    {
        public string Id { get; set; }
        public string ModuleGroupId { get; set; }
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
        public string Pricing { get; set; }
        public bool IsHMI { get; set; }
        public bool IsPLC { get; set; }
        public bool IsDMTV { get; set; }
        public int CurrentVersion { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsUpdateFilm { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        // join mudule group
        public string ModuleGroupName { get; set; }
        public string ModuleGroupCode { get; set; }
        // list lỗi
        public int ListModelError { get; set; }
        public bool IsEnought { get; set; }
        // Extent
        public Nullable<bool> ElectricExist { get; set; }
        public Nullable<bool> ElectronicExist { get; set; }
        public Nullable<bool> MechanicsExist { get; set; }
        public Nullable<bool> SoftwareExist { get; set; }
        public Nullable<bool> HMIExist { get; set; }
        public Nullable<bool> PLCExist { get; set; }
        public Nullable<bool> FilmExist { get; set; }
        public decimal? Quantity { get; set; }
        public List<ModuleMaterialResultModel> ListMaterial { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal Price { get; set; }
        public int Leadtime { get; set; }
        public bool IsManual { get; set; }
        public bool ManualExist { get; set; }
        public bool IsSendSale { get; set; }
        public DateTime? SyncDate { get; set; }
        public ModuleResultModel()
        {
            ListMaterial = new List<ModuleMaterialResultModel>();
        }

        public List<TestCriteriaModel> ListTestCeiteriaModel { get; set; }

        public bool IsSpecification { get; set; }
    }
}
