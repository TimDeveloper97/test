using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class SearchResultReportProjectModel <T>
    {
        public int TotalItem { get; set; }
        public List<T> ListResult = new List<T>();
       
    }
}
