using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ErrorAttach
{
    public class ErrorFixAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string ErrorId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public bool IsDelete { get; set; }
    }
}
