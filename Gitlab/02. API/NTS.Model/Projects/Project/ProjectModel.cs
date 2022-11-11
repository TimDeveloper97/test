using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Project
{
    public class ProjectModel : BaseModel
    {
        public string Id { get; set; }
        public string ProjectProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public System.DateTime? DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Status { get; set; }
        public DateTime search { get; set; }
        public string Parameter { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string ManageName { get; set; }
        public decimal CountModule { get; set; }
        public bool StatusTestCriteria { get; set; }
        public bool IsDelay { set; get; }
        public string ProjectId { set; get; }

        public string ModuleId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DesignPrice { get; set; }
        public decimal SaleNoVat { get; set; }
        public decimal FCMPrice { get; set; }
        /// <summary>
        /// Mã kho
        /// </summary>
        public string WarehouseCode { get; set; }
        public int Type { get; set; }
        public int? PaymentStatus { get; set; }

    }
}
