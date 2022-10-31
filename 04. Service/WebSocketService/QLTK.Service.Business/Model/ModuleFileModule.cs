using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ModuleFileModule
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

        public List<TestCriteriaModel> ListTestCeiteriaModel { get; set; }
    }
    public class TestCriteriaModel
    {
        public string Id { get; set; }
        public string TestCriteriaGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TechnicalRequirements { get; set; }
        public string Note { get; set; }
        public string TestCriteriaGroupName { get; set; }
        public bool ResuldStatusTest { get; set; }
        public bool IsExport { get; set; }
        public string NoteResuld { get; set; }
    }
}
