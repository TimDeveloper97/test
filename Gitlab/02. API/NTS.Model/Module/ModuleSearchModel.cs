using NTS.Model.Combobox;
using NTS.Model.TestCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKMODULE
{
    public class ModuleSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ModuleGroupId { get; set; }
        public string ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }
        public int State { get; set; }
        public string Specification { get; set; }
        public bool FileElectric { get; set; }
        public bool FileElectronic { get; set; }
        public bool FileMechanics { get; set; }
        public bool IsSoftware { get; set; }
        public string Pricing { get; set; }
        public bool IsHMI { get; set; }
        public bool IsPLC { get; set; }
        public int CurrentVersion { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public Nullable<int> IsUpdateFilm { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsExport { get; set; }
        public int? IsEnought { get; set; }
        public bool? IsEnought1 { get; set; }
        public string File { get; set; }
        public bool IsSendSale { get; set; }
        // Extent
        public Nullable<bool> ElectricExist { get; set; }
        public Nullable<bool> ElectronicExist { get; set; }
        public Nullable<bool> MechanicsExist { get; set; }
        public Nullable<bool> SoftwareExist { get; set; }
        public Nullable<bool> HMIExist { get; set; }
        public Nullable<bool> PLCExist { get; set; }
        public Nullable<bool> FilmExist { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }

        public ModuleSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
