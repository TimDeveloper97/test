using NTS.Model.Specialize;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTS.Model.PracticeGroup
{
    public class PracticeGroupModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Index { get; set; }
        public string Description { get; set; }
        public List<SpecializeResultModel> ListSpecialize { get; set; }

        public PracticeGroupModel()
        {
            ListSpecialize = new List<SpecializeResultModel>();
        }
    }
}
