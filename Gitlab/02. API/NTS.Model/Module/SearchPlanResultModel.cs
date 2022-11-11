using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Module
{
    public class SearchPlanResultModel<T> : SearchResultModel<T>
    {
        public bool ElectronicExist{get; set; }
        public bool ElectricExist { get; set; }
        public bool MechanicsExist { get; set; }
        public bool FileElectronic { get; set; }
        public bool FileElectric { get; set; }
        public bool FileMechanics { get; set; }
    }
}
