using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesign
{
    public class ProjectGeneralDesignResultModel : BaseModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string DepartmentRequestId { get; set; }
        public string DepartmentRequestName { get; set; }
        public string DepartmentCreateId { get; set; }
        public string DepartmentCreateName { get; set; }
        public DateTime RequestDate { get; set; }
        public string ProjectProductId { get; set; }
        public string ProjectId { get; set; }
        public int CreateIndex { get; set; }
        public string DesignBy { get; set; }
        public string ProjectProductName { get; set; }
        public string ProjectProductCode { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public int DataType { get; set; }
        public int ApproveStatus { get; set; }
    }
}
