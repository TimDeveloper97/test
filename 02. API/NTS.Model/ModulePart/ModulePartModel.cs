using System;

namespace NTS.Model.ModulePart
{

    public class ModulePartModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleItemId { get; set; }
        public int Qty { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
    }

}