using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentChannels
{
    public class RecruitmentChannelSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public bool? Status { get; set; }
    }
}
