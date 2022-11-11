using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class SearchResultErrorModel
    {
        public int TotalItem { get; set; }
        public List<ErrorModel> Errors = new List<ErrorModel>();
        public List<ErrorFixResultModel> ErrorFixs = new List<ErrorFixResultModel>();
        public int ProblemStatus1 { get; set; }
        public int ProblemStatus2 { get; set; }
        public int ProblemStatus3 { get; set; }
        public int ProblemStatus4 { get; set; }
        public int ProblemStatus5 { get; set; }
        public int ProblemStatus6 { get; set; }
        public int ProblemStatus7 { get; set; }
        public int ProblemStatus8 { get; set; }
        public int ProblemStatus9 { get; set; }
        public int ProblemStatus10 { get; set; }

        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public int TotalError { get; set; }

        public int ErrorStatus1 { get; set; }
        public int ErrorStatus2 { get; set; }
        public int ErrorStatus3 { get; set; }
        public int ErrorStatus4 { get; set; }
        public int ErrorStatus5 { get; set; }
        public int ErrorStatus6 { get; set; }
        public int ErrorStatus7 { get; set; }
        public int ErrorStatus8 { get; set; }
        public int ErrorStatus9 { get; set; }
        public int ErrorStatus10 { get; set; }

    }
}
