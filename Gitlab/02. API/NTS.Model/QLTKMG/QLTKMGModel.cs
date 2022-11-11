using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKMG
{
    public class QLTKMGModel : BaseModel
    {
        public string IsOpen { get; set; }
        public string IsParent { get; set; }
        public int Level { get; set; }
        public string IsShow { get; set; }
        public string Index { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string MaterialGroupTPACode { get; set; }
        public string MaterialGroupTPAName { get; set; }
        public List<QLTKMGModel> SupplierList { get; set; }
    }
}
