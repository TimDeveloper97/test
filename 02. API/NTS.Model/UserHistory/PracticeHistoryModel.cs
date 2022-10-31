using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeHistory
{
    public class PracticeHistoryModel
    {
        public string Id { get; set; }
        public string PracticeGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CurentVersion { get; set; }
        public string EditContent { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public int TrainingTime { get; set; }
        public decimal LessonPrice { get; set; }
        public decimal HardwarePrice { get; set; }
        public string UnitId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int LeadTime { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public Nullable<bool> PracticeFile { get; set; }
        public Nullable<bool> PracticeExist { get; set; }
        public Nullable<bool> SupMaterial { get; set; }
        public Nullable<bool> SupMaterialExist { get; set; }
        public Nullable<bool> MaterialConsumable { get; set; }
        public Nullable<bool> MaterialConsumableExist { get; set; }
    }
}
