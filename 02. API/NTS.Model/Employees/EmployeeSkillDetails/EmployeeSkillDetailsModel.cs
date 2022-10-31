using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeSkillDetails
{
    public class EmployeeSkillDetailsModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int LevelMax { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string LevelRate { get; set; }
        public DateTime RateDate { get; set; }
        public string LevelRate1 { get; set; }
        public DateTime RateDate1 { get; set; }
        public string LevelRate2 { get; set; }
        public DateTime RateDate2 { get; set; }
        public string Note { get; set; }
        public List<EmployeeSkillDetailsModel> ListData { get; set; }

        public string SkillGroupId { get; set; }
    }
}
