using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Report
{
    public class ReportErrorProgressResultModel
    {
        public List<ReportErrorProgressResultObjectModel> ErrorFixs { get; set; }
        public List<ReportErrorProgressResultObjectModel> ErrorFixBys { get; set; }        
        public List<ReportErrorProgressResultLabelModel> Weeks { get; set; }
        public List<ReportErrorProgressResultLabelDayModel> Days { get; set; }
        public List<ReportErrorProgressResultObjectModel> ErrorChangePlans { get; set; }
        public List<ReportErrorProgressResultECPLabelModel> Lables { get; set; }
        public ReportErrorProgressResultModel()
        {
            ErrorFixs = new List<ReportErrorProgressResultObjectModel>();
            ErrorChangePlans = new List<ReportErrorProgressResultObjectModel>();
            Weeks = new List<ReportErrorProgressResultLabelModel>();
            Days = new List<ReportErrorProgressResultLabelDayModel>();
            ErrorFixBys = new List<ReportErrorProgressResultObjectModel>();
        }
    }

    public class ReportErrorProgressResultObjectModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal FinishPercent { get; set; }
        public decimal OpeningTotal { get; set; }
        public decimal DelayTotal { get; set; }
        public decimal FinishTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


        public List<ReportErrorProgressResultValueModel> DelayValues { get; set; }
        public List<ReportErrorProgressResultValueModel> PlanValues { get; set; }

        public ReportErrorProgressResultObjectModel()
        {
            DelayValues = new List<ReportErrorProgressResultValueModel>();
            PlanValues = new List<ReportErrorProgressResultValueModel>();
        }
    }

    public class ReportErrorProgressResultValueModel
    {
        public int Value { get; set; }
        public bool IsToDay { get; set; }
        public bool IsLessToDay { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class ReportErrorProgressResultLabelModel
    {
        public string Title { get; set; }
        public bool IsToDay { get; set; }
    }

    public class ReportErrorProgressResultLabelDayModel
    {
        public string TitleECP { get; set; }
        public bool IsToDayECP { get; set; }
    }
    public class ReportErrorProgressResultECPLabelModel
    {
        public string Title { get; set; }
    }

}
