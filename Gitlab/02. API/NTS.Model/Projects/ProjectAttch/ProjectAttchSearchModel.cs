using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectAttch
{
    public class ProjectAttchSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string UserId { get; set; }
        public string ParentId { get; set; }
    }
}
