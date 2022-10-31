using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Payment
{
    public class PaymentModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime? PlanPaymentDate { get; set; }
        public decimal? PlanAmount { get; set; }
        public string PaymentCondition { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime? ActualPaymentDate1 { get; set; }
        public decimal ActualAmount1 { get; set; }
        public DateTime? ActualPaymentDate2 { get; set; }
        public decimal ActualAmount2 { get; set; }
        public DateTime? ActualPaymentDate3 { get; set; }
        public decimal ActualAmount3 { get; set; }
        public DateTime? ActualPaymentDate4 { get; set; }
        public decimal ActualAmount4 { get; set; }
        public DateTime? ActualPaymentDate5 { get; set; }
        public decimal ActualAmount5 { get; set; }
        public DateTime? PaymentMilestone { get; set; }
        public int MoneyCollectionTime { get; set; }

        public DateTime? CollectionDate { get; set; }
        public int AverageDoneRatio { get; set; }

        public int TotalPlan { get; set; }
        public int UnfinishedPlan { get; set; }
        public int LatePlan { get; set; }

    }

    public class TotalWithPaymentModel
    {
        public List<PaymentModel> PaymentModels { get; set; }
        public decimal? TotalPlanAmount { get; set; }
        public decimal? ActualPlanAmount { get; set; }

    }
    public class PlanPaymentModel
    {
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string PaymentId { get; set; }

    }
    public class PlanPaymentDate
    {
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string PaymentId { get; set; }
        public DateTime? PlanDate { get; set; }

    }
    public class PlanPayment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndtDate { get; set; }
        public int Status { get; set; }

    }
}
