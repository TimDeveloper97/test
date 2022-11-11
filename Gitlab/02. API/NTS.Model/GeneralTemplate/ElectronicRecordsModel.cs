using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class ElectronicRecordsModel : BaseModel
    {
        public string ProductName { get; set; }
        // mã module
        public string ProductCode { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool check_14_4 { get; set; }
        public bool check_15_4 { get; set; }
        public bool check_16_4 { get; set; }
        public bool check_17_4 { get; set; }
        public bool check_18_4 { get; set; }
        public bool check_20_4 { get; set; }
        public bool check_22_4 { get; set; }
        public bool check_24_4 { get; set; }
        public bool check_26_4 { get; set; }
    }
}
