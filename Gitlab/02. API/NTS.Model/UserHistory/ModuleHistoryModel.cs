using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleHistory
{
    public class ModuleHistoryModel
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
        public bool ElectricExist { get; set; }
        public bool FileElectronic { get; set; }
        public bool ElectronicExist { get; set; }
        public bool FileMechanics { get; set; }
        public bool MechanicsExist { get; set; }
        public bool IsSoftware { get; set; }
        public bool SoftwareExist { get; set; }
        public decimal Pricing { get; set; }
        public bool IsHMI { get; set; }
        public bool HMIExist { get; set; }
        public bool IsPLC { get; set; }
        public bool PLCExist { get; set; }
        public int CurrentVersion { get; set; }
        public string ThumbnailPath { get; set; }
        public bool IsFilm { get; set; }
        public bool IsDMTV { get; set; }
        public bool FilmExist { get; set; }
        public string Description { get; set; }
        public string EditContent { get; set; }
        public int Leadtime { get; set; }
        public string DepartmentId { get; set; }
        public bool IsManual { get; set; }
        public bool ManualExist { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
