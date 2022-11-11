using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SurveyMaterial
{
    public class SurveyMaterialCreateModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Quantity { get; set; }
        public string SurvayId { get; set; }
    }
}
