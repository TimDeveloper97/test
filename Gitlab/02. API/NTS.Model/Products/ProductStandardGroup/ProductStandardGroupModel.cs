using System;
using System.Linq;

namespace NTS.Model.ProductStandardGroup
{
    public class ProductStandardGroupModel : BaseModel
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Note { get; set; }
    }
}
