using NTS.Model.Projects.ProjectProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProducts
{
    public class ProductStandardGroupModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProductStandardsGroupId { get; set; }
        public int Status { get; set; }
        public int TotalNG { get; set; }
        public int Total { get; set; }
    }
}
