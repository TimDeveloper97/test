using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.NSMaterialGroupAttach
{
    public class NSMaterialGroupAttachModel
    {
        public string Id { get; set; }
        public string NSMaterialGroupId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal FileSize { get; set; }
        public bool IsDelete { get; set; }
    }
}
