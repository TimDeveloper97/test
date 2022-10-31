using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SimilarMaterial
{
    public class SimilarMaterialModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialId { get; set; }
    }
}
