using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadModule
{
    public class DownloadModuleSearchResultModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsEnought { get; set; }
        public bool FileElectric { get; set; }
        public bool FileElectronic { get; set; }
        public bool FileMechanics { get; set; }
        public bool ElectricExist { get; set; }
        public bool ElectronicExist { get; set; }
        public bool MechanicsExist { get; set; }
    }
}
