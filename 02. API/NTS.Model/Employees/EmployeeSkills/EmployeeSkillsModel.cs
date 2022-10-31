using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeSkillsDetails
{
    public class EmployeeSkillsModel:BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string LevelMax { get; set; }
    }
}
