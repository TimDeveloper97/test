using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectAttch
{
    public class ProjectAttchTabTypeModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentId { get; set; }
        public string ProjectId { get; set; }
    }
}
