using NTS.Model.Combobox;
using System;
using System.Linq;

namespace NTS.Model.ProductStandardGroup
{
    public class ProductStandardGroupSearchModel: SearchCommonModel
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
