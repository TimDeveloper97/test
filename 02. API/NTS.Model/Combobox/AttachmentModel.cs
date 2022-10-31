using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Combobox
{
    public class AttachmentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal? Size { get; set; }
        public string Extention { get; set; }
        public string Thumbnail { get; set; }
        public string HashValue { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
    }

   
}
