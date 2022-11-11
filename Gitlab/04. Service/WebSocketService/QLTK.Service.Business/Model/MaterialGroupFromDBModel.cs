using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class MaterialGroupFromDBModel
    {
        public string Id { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
    }

}
