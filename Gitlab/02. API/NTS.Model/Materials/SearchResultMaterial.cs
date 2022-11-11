using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class SearchResultMaterial<T> : SearchResultModel<T>
    {
        public int TotalItemExten { get; set; }
        public int TotalNoFile { get; set; }
        public int TotalFile { get; set; }

        public static implicit operator SearchResultMaterial<T>(SearchResultMaterial<GeneralTemplate.MaterialModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
