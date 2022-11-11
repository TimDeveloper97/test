using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanWorkModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string SupplierId { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractDueDate { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanDueDate { get; set; }
    }

    public class PlanWorkEmpoyeeModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public string NowDate { get; set; }
        public Monthly Monthly { get; set; }
        public List<TaskWorkTimeModel> ListWorkTime { get; set; }
        public List<TaskAssign> taskAssigns { get; set; }
        public PlanWorkEmpoyeeModel()
        {
            ListWorkTime = new List<TaskWorkTimeModel>();
            taskAssigns = new List<TaskAssign>();
        }
    }
    public class Monthly
    {
        public string TimeRange { get; set; }
        public List<DataMonthlys> DataMonthlys { get; set; }

        public Monthly()
        {
            DataMonthlys = new List<DataMonthlys>();
        }
    }

    public class DataMonthlys
    {
        public int Date { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime DateTime { get; set; }
        public decimal LogTime { get; set; }
    }

    public class TaskWorkTimeModel
    {
        public string TaskName { get; set; }
        public List<DataMonthlys> TaskWorkTime { get; set; }
        public TaskWorkTimeModel()
        {
            TaskWorkTime = new List<DataMonthlys>();
        }
    }
    public class TaskAssign
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string TaskName { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal NumberTime { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Note { get; set; }
        public string ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string EmpoyeeId { get; set; }
    }

    public class TaskAssignModel
    {
        public string employeeId { get; set; }
        public List<TaskAssign> TaskAssign { get; set; }
        public TaskAssignModel()
        {
            TaskAssign = new List<TaskAssign>();
        }
    }
}
