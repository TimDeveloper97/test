using NTS.Model.DataDistributionFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DataDistribution
{
    public class DataDistributionFileSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public List<string> ListSelectId { get; set; }

        public DataDistributionFileSearchModel()
        {
            ListSelectId = new List<string>();
        }
    }
}
