using NTS.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectProducts
{
    public class CopyQCCheckListModel
    {
        public string ProjectProductId { get; set; }
        public string ProjectId { get; set; }
        public List<string> ListProjectProductId { get; set; }
        public List<QCCheckListModel> ListCheck { get; set; }
    }
}
