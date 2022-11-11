using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class ProduceStageModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public List<string> ListIdSelect { get; set; }
        public ProduceStageModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
