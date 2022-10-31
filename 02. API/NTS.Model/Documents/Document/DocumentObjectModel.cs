using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentObjectModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string ObjectId { get; set; }
        public int ObjectType { get; set; }
    }
}
