using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class ScheduleProjectSearchModel : SearchCommonModel
    {
        public string ProjectId { get; set; }
        public bool IsHistory { get; set; }
        public DateTime? ContractStartDateToV { get; set; }
        public DateTime? ContractStartDateFromV { get; set; }
        public DateTime? ContractDueDateToV { get; set; }
        public DateTime? ContractDueDateFromV { get; set; }
        public DateTime? PlanStartDateToV { get; set; }
        public DateTime? PlanStartDateFromV { get; set; }
        public DateTime? PlanDueDateToV { get; set; }
        public DateTime? PlanDueDateFromV { get; set; }
        public string EmployeeId { get; set; }
        public int WorkProgress { get; set; }
        public string StageId { get; set; }
        public string ImplementingAgenciesCode { get; set; }
        public string DepartmentId { get; set; }
        public int WorkClassify { get; set; }
        public int PlanStatus { get; set; }
        public int WorkStatus { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string PlanHistoryId { get; set; }

        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }

        public int ErrorStage { get; set; } 
        public int ErrorModule { get; set; }
        public int ErrorProduct { get; set; }
        public string NameProduct { get; set; }
        public string SupplierId { get; set; }
        public int Operator { get; set; }
        public decimal Percen { get; set; }


    }
}
