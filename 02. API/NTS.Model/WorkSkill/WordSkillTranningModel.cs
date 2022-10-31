using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkSkill
{
    public class WordSkillTranningModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WorkSkillId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Max { get; set; }
        public decimal Grade { get; set; }
        public decimal Mark { get; set; }
        public int OldMark { get; set; }
    }
}
