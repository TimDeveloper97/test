using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ErrorImage
{
    public class ErrorImageModel
    {
        public string Id { get; set; }
        public string ErrorId { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
