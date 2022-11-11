using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardsResultModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Note { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }
        public List<ProductStandardsModel> ListProductStandard { get; set; }

        public string ProductStandardGroupName { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public string CreateByName { get; set; }
    }
}
