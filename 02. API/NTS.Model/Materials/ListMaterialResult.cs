using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class ListMaterialResult
    {
        public List<ModuleMaterialModel> ListModuleMaterial { get; set; }
        public List<MaterialFromDBModel> ListMaterial { get; set; }
        public ListMaterialResult()
        {
            ListModuleMaterial = new List<ModuleMaterialModel>();
            ListMaterial = new List<MaterialFromDBModel>();
        }
    }
}
