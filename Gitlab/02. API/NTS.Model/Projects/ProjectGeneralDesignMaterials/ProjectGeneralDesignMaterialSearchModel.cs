using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesignMaterials
{
    public class ProjectGeneralDesignMaterialSearchModel
    {
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public string Name { get; set; }
        public List<string> ListIdSelect { get; set; }

        public ProjectGeneralDesignMaterialSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
