using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeSkillDetails
{
    public class EmployeeSkillDetailsResultModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeSkillId { get; set; }
        public string LevelRate { get; set; }
        public string RateDate { get; set; }
        public string LevelRate1 { get; set; }
        public string RateDate1 { get; set; }
        public string LevelRate2 { get; set; }
        public string RateDate2 { get; set; }
        public string Note { get; set; }
    }
}
