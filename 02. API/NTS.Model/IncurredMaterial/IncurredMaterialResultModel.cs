using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.IncurredMaterial
{
    public class IncurredMaterialResultModel
    {
        public decimal Total { get; set; }
        public List<ModuleMaterialResultModel> LitsMaterial { get; set; }
        public IncurredMaterialResultModel()
        {
            LitsMaterial = new List<ModuleMaterialResultModel>();
        }
    }
}
