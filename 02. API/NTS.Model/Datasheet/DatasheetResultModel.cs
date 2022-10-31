using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Datasheet
{
    public class DatasheetResultModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
