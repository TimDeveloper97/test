using System;
using System.Linq;

namespace NTS.Model.PracticeGroup
{
    public class PracticeGroupResultModel
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public string Description { get; set; }
    }
}
