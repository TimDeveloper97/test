using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialDocumentDownloadModel
    {
        public string ModuleCode { get; set; }
        public string MaterialId { get; set; }
        public string MaterialCodeNotFound { get; set; }
        public string MaterialManufatureError { get; set; }

        public List<string> Document3DId { get; set; }
    }
}
