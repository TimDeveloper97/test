using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SkillEmployee
{
    public class SkillEmployeeModel : SearchCommonModel
    {
        public string Id { set; get; }
        public string EmployeeId { set; get; }
        public string WorkSkillId { set; get; }
        public string Name { set; get; }
        public decimal Mark { set; get; }
        public decimal Grade { set; get; }
        public decimal Score { set; get; }
        public string Description { set; get; }
        public string WorkTypeName { get; set; }
        public List<SkillEmployeeModel> ListResult { get; set; }
    }
}
