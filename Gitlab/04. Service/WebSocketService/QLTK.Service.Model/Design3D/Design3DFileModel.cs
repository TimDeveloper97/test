using QLTK.Service.Model.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Design3D
{
  public  class Design3DFileModel
    {
        public string ModuleCode { get; set; }
        public List<FileModel> Files { get; set; }

        public Design3DFileModel()
        {
            Files = new List<FileModel>();
        }
    }
}
