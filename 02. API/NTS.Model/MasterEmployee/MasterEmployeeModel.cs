using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MasterEmployee
{
    public class MasterEmployeeModel: SearchCommonModel
    {
        public string EmployeeName { set; get; }
        public string EmployeeCode { set; get; }
        // Kĩ năng hiện tại
        public string WorkSkillName { set; get; }
        // Mục tiêu tương lai so sánh điểm đánh giá <= điểm điều kiện 
        public string FutureGoals { set; get; }
        // khóa học đã học so sánh điểm đánh giá => điểm điều kiện 
        public string CouseNameOld { set; get; }
        // Khóa học cần học so sánh điểm đánh giá <= điểm điều kiện 
        public string CouseNameNew { set; get; }
        public string CouseCode { set; get; }
        public string Note { set; get; }
        public string Mark { set; get; }
        public string Grade { set; get; }

    }
}
