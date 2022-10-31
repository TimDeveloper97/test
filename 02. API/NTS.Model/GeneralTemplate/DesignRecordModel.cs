using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class DesignRecordModel : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public bool check_1_3 { get; set; }
        public bool check_2_3 { get; set; }
        public bool check_3_3 { get; set; }
        public bool check_4_3 { get; set; }
        public bool check_5_3 { get; set; }
        public bool check_6_3 { get; set; }
        public bool check_7_3 { get; set; }
        public bool check_8_M { get; set; }
        public bool check_8_C { get; set; }
        public bool check_9_C { get; set; }
        public bool check_9_M { get; set; }
        public bool check_10_3 { get; set; }
        public bool check_11a_3 { get; set; }
        public bool check_11b_3 { get; set; }
        public bool check_12a_3 { get; set; }
        public bool check_12b_3 { get; set; }
        public bool IsExport { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string DateNow { get; set; }

    }
}
