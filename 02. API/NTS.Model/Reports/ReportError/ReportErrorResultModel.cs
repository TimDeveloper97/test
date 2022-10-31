using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Report
{
    public class ReportErrorResultModel
    {
        public List<ReportErrorResultObjectModel> ErrorFixs { get; set; }
        public List<ReportErrorResultObjectModel> ErrorFixBys { get; set; }
        public List<ReportErrorResultDepartmentManageModel> ErrorProjects { get; set; }
        public List<string> Stages { get; set; }
        public List<string> Departments { get; set; }

        public ReportErrorResultModel()
        {
            ErrorFixs = new List<ReportErrorResultObjectModel>();
            ErrorFixBys = new List<ReportErrorResultObjectModel>();
            ErrorProjects = new List<ReportErrorResultDepartmentManageModel>();
            Stages = new List<string>();
            Departments = new List<string>();
        }
    }

    public class ReportErrorResultObjectModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Deadline { get; set; }
        public int ErrorQuantity { get; set; }
        public int WorkQuantity { get; set; }
        public int WorkDelay { get; set; }
        public int TotalWorkToDo { get; set; }
        public int TotalWorkDelay { get; set; }

        public List<ReportErrorResultSumModel> Values { get; set; }

        public ReportErrorResultObjectModel()
        {
            Values = new List<ReportErrorResultSumModel>();
        }
    }

    public class ReportErrorResultSumModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Deadline { get; set; }
        public int ErrorQuantity { get; set; }
        public int WorkQuantity { get; set; }
        public int WorkDelay { get; set; }
        public int WorkToDo { get; set; }
    }

    public class ReportErrorResultDepartmentManageModel
    {
        public string Id { get; set; }
        public string SBUId { get; set; }
        public string Name { get; set; }
        public int ErrorNoPlan1 { get; set; }
        public int ErrorNoPlan2 { get; set; }
        public int ErrorNoPlan3 { get; set; }
        public int ErrorQuantity { get; set; }
        public int ProjectQuantity { get; set; }
        public int ErrorDelay { get; set; }
        public decimal ProjectAmount { get; set; }
    }
}
