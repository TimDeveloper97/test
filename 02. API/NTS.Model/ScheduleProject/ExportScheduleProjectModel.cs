using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class ExportScheduleProjectModel
    {
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string ModuleCode { get; set; }
        public string Design { get; set; }
        public string Supplies { get; set; }
        public string Assembly { get; set; }
        public string Delivery { get; set; }
        public string Installation { get; set; }
        public string IO { get; set; }
        public string ManualAjust { get; set; }
        public string Teaching { get; set; }
        public string DryCicle { get; set; }
        public string TestWithE34 { get; set; }
        public string TestWithE35 { get; set; }
        public string TestWithE36 { get; set; }
        public string TrialProduct1st { get; set; }
        public string TrialProduct2st { get; set; }
        public string TrialProduct3st { get; set; }
        public string ActureProduct { get; set; }
    }
}
