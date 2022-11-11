using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanImplementReality
    {
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string StageCode { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? RealDate { get; set; }
        public int? DateDelay { get; set; }
        public bool IsDelay { get; set; }
        public string PlanId { get; set; }
        public decimal Width { get; set; } 
        public DateTime? ContactDate { get; set; }
        public DateTime? PlanDate { get; set; }
        public int Status { get; set; }

    }

    public class GeneralPlan
    {
        public string  DoneRatio { get; set; }
        public List<PlanImplementReality> PlanImplementRealities = new List<PlanImplementReality>();
        public decimal TotalWitdh { get; set; }
    }

    public class TotalTypePlan
    {
        public int WorkEmptyStart { get; set; }
        public int WorkEmptyEnd { get; set; }
        public int WorkLate { get; set; }
        public int WorkIncurred { get; set; }

        public int StageEmptyStart { get; set; }
        public int StageEmptyEnd { get; set; }
        public int StageLate { get; set; }

        public int ParentProductEmptyStart { get; set; }
        public int ParentProductEmptyEnd { get; set; }
        public int ParentProductIncurred { get; set; }

        public int ChildProductEmptyStart { get; set; }
        public int ChildProductEmptyEnd { get; set; }
        public int ChildProductIncurred { get; set; }

        public int ThieuNhaThau { get; set; }
        public int ThieuNguoiPhutrach { get; set; }

    }

    public class StageModel
    {
        public string StageId { get; set; }
        public string PlanId { get; set; }
        public string StageName { get; set; }
        public int Status { get; set; }
        public decimal DoneRatio { get; set; }
        public DateTime? Date { get; set; }
        public int Weight { get; set; }

    }
    public class ProductPlan
    {
        public string ProductName { get; set; }
        public List<StageModel> StageModels { get; set; }
        public DateTime? HDEndDate { get; set; }
        public DateTime? TKEndDate { get; set; }
        public string PlanId { get; set; }
        public int Weight { get; set; }
        public ProductPlan()
        {
            StageModels = new List<StageModel>();
        }

    }
    public class ProjectPlan
    {
        public List<ProductPlan> ProductPlans { get; set; }
        public List<StageModel> StageModels { get; set; }
        public ProjectPlan()
        {
            ProductPlans = new List<ProductPlan>();
            StageModels = new List<StageModel>();
        }
    }

    public class StageDelay
    {
        public int DateDelay { get; set; }
        public string Id { get; set; }

        public DateTime? ContractDate { get; set; }
        public DateTime? PlanDate { get; set; }
    }

}
