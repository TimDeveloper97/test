using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Materials
{
    public class MaterialDesign3DModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public decimal Size { get; set; }

        public string CreateByName { get; set; }
    }
}
