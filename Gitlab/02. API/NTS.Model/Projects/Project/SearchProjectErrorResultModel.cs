using NTS.Model.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.Project
{
    public class SearchProjectErrorResultModel
    {
        public int TotalItem { get; set; }
        public List<ErrorModel> Errors = new List<ErrorModel>();
        public List<ErrorFixResultModel> ErrorFixs = new List<ErrorFixResultModel>();

        public int Status1 { get; set; }
        public int Status2 { get; set; }
        public int Status3 { get; set; }
        public int Status4 { get; set; }
        public int Status5 { get; set; }
        public int Status6 { get; set; }
        public int Status7 { get; set; }
        public int Status8 { get; set; }
        public int Status9 { get; set; }
        public int TotalError { get; set; }

        public int MaxDeliveryDay { get; set; }
        public int Status10 { get; set; }
        public int Status11 { get; set; }
        public int Status12 { get; set; }
        public int Status13 { get; set; }
    }
}