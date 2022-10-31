using NTS.Model.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Mobile
{
    public class ErrorSearchResult
    {
        public int TotalItem { get; set; }
        public List<ErrorResultModel> Errors = new List<ErrorResultModel>();
    }
}
