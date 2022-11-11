using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterialFinishDesign
{
    public class ModuleMaterialFinishDesignModel
    {
        public string Id { get; set; }
        public string ProjectProductId { get; set; }
        public string MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
