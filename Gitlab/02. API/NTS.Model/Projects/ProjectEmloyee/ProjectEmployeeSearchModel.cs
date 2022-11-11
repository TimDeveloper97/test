using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class ProjectEmployeeSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
    }
}
