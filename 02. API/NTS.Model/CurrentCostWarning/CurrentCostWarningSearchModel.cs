using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CurrentCostWarning
{
    public class CurrentCostWarningSearchModel : SearchCommonModel
    {
        public string ProjectId { get; set; }
    }

    public class CurrentCostWarningModel
    {
        public string ProjectId { get; set; }
        public string ProjectProductModuleId { get; set; }
        public string ProjectProductModuleGroupId { get; set; }
        public string ErrorModuleId { get; set; }
        public string ErrorModuleGroupId { get; set; }
    }

    public class AddModel
    {
        public string Id { get; set; }
        public string ModuleGroupCode { get; set; }
        public string ModuleGroupName { get; set; }
        public decimal Price { get; set; }
        public decimal Percent { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class GroupAddModel
    {
        public string ModuleGroupCode { get; set; }
        public string ModuleGroupName { get; set; }
        public decimal QuantityModule { get; set; }
        public decimal PriceProjectProduct { get; set; }
        public decimal PercentProjectProduct { get; set; }
        public decimal AveragePriceProjectProduct { get; set; }
        public decimal PriceError { get; set; }
        public decimal PercentError { get; set; }
        public decimal AverageError { get; set; }
    }
}
