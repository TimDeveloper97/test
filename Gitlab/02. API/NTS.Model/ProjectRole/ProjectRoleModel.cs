using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectRole
{
    public class ProjectRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Descipton { get; set; }
        public bool IsDisable { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<PlanFunctionResultModel> PlanFunctions { get; set; }
    }
}
