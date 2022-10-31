using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ConverUnit
{
    public class ConverUnitModel : BaseModel
    {
        public string Id { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal ConvertQuantity { get; set; }
        public List<ConverUnitModel> ListConverUnit { get; set; }

        public string MaterialCode { get; set; }
        /// <summary>
        /// tỷ lệ tiêu hao
        /// </summary>
        public decimal LossRate { get; set; }
    }
}
