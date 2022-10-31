using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Classification
{
    public class ClassIficationModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public string Note { get; set; }
    }
}
