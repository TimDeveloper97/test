using NTS.Model.ProjectGeneralDesignMaterials;
using NTS.Model.ProjectGeneralDesignModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesign
{
    public class ProjectGeneralDesignModel : BaseModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string SBURequestId { get; set; }
        public string DepartmentRequestId { get; set; }
        public string SBUCreateId { get; set; }
        public string DepartmentCreateId { get; set; }
        public DateTime RequestDate { get; set; }
        public string ProjectProductId { get; set; }
        public string ProjectId { get; set; }
        public int CreateIndex { get; set; }
        public string DesignBy { get; set; }
        public string DepartmentId { get; set; }
        public bool IsUpdate { get; set; }
        public int ApproveStatus { get; set; }
        public string ModuleId { get; set; }
        public List<ProjectGeneralDesignModuleModel> ListModule { get; set; }
        public List<ProjectGeneralDesignMaterialsModel> ListMaterial { get; set; }
        public ProjectGeneralDesignModel()
        {
            ListModule = new List<ProjectGeneralDesignModuleModel>();
            ListMaterial = new List<ProjectGeneralDesignMaterialsModel>();
        }
    }
}
