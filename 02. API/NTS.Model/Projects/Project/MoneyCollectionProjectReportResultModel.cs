using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class MoneyCollectionProjectReportResultModel
    {
        public List<string> Departments { get; set; }
        public List<ReportMoneyCollectionResultLabelModel> Months { get; set; }
        public List<ReportMoneyCollectionResultLabelModel> Inventory { get; set; }

        public MoneyCollectionProjectReportResultModel()
        {
            Departments = new List<string>();
            Months = new List<ReportMoneyCollectionResultLabelModel>();
            Inventory = new List<ReportMoneyCollectionResultLabelModel>();
        }
        public class ReportMoneyCollectionResultObjectModel
        {
            public string DepartmentId { get; set; }
            public int TotalProject { get; set; }
            public decimal TotalAmount { get; set; } //đã thu
            public decimal TotalCollected { get; set; } //Tổng thực tế đã thu phải thu

            public decimal TotalReceivables { get; set; } //Tổng còn phải thu
            public decimal TotalPlanAmout { get; set; } //Tổng cần thu

            public bool IsExist { get; set; }
            public int Month { get; set; }
            public List<string> ListProject { get; set; }
            public ReportMoneyCollectionResultObjectModel()
            {
                ListProject = new List<string>();
            }
        }

        public class ReportMoneyCollectionResultLabelModel
        {
            public string Title { get; set; }

            public int AllProjects { get; set; } // Cộng dự án cần phải thu theo hàng
            public decimal AllPlanAmouts { get; set; } // Cộng giá trị phải thu theo hàng
            public decimal AllReceivables { get; set; } // Cộng giá trị còn phải thu theo hàng

            public List<ReportMoneyCollectionResultObjectModel> Result { get; set; }
            public ReportMoneyCollectionResultLabelModel()
            {

                Result = new List<ReportMoneyCollectionResultObjectModel>();
            }
        }
    }
}
