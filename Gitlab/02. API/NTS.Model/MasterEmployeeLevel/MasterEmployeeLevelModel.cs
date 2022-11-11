using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MasterEmployeeLevel
{
    public class MasterEmployeeLevelModel: SearchCommonModel
    {
        public string WorkTypeName { set; get; }
        public string WorkSkillName { set; get; }
        public string CouseName { set; get; }
        public string CouseCode { set; get; }
        public string DeviceForCourse { set; get; }
    }
}
