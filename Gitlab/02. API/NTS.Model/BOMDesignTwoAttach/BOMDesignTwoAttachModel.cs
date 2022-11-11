using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.BOMDesignTwoAttach
{
    public class BOMDesignTwoAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string BOMDesignTwoId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public decimal FileSize { get; set; }
    }
}
