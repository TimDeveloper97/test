using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialImageModel:BaseModel
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
