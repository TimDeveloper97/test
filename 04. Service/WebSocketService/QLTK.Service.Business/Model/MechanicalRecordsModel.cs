using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class MechanicalRecordsModel
    {
        public string ApiUrl { get; set; }

        /// <summary>
        /// Lấy tên file name
        /// </summary>
        public string FileName { get; set; }
        public int Type { get; set; }
        public string ModuleCode { get; set; }
        public string Path { get; set; }
        public string PathFolder { get; set; }
        public string ProductName { get; set; }
        // mã module
        public string ProductCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public bool check_14_3 { get; set; }
        public bool check_15_3 { get; set; }
        public bool check_16_3 { get; set; }
        public bool check_17_3 { get; set; }
        public bool check_18_3 { get; set; }
        public bool check_19_3 { get; set; }
        public bool check_20_3 { get; set; }
        public bool check_21_3 { get; set; }
        public bool check_22_3 { get; set; }
        public bool check_23_3 { get; set; }
        public bool check_24_3 { get; set; }
        public bool check_25_3 { get; set; }
        public bool check_26_3 { get; set; }
        public bool check_27_3 { get; set; }
        public bool check_28_3 { get; set; }
        public bool check_29_3 { get; set; }
        public bool check_30_3 { get; set; }

        public List<ProductStandModel> ListProduct { get; set; }
    }
}
