using NTS.Model.BOMDesignTwoAttach;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.BOMDesignTwo
{
    public class BOMDesignTwoModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ProjectProductId { get; set; }
        public int Version { get; set; }
        public int Index { get; set; }
        public string CreateByName { get; set; }
        public List<BOMDesignTwoAttachModel> ListFile { get; set; }
        public string Content { get; set; }
        public BOMDesignTwoModel()
        {
            ListFile = new List<BOMDesignTwoAttachModel>();
        }
    }
    public class GetDateImportManufacture
    {
        public List<BOMDesignTwoDetial> ListData { get; set; }
        public List<Material> ListMaterial { get; set; }
        public GetDateImportManufacture()
        {
            ListData = new List<BOMDesignTwoDetial>();
            ListMaterial = new List<Material>();
        }
    }

    public class IndexModel
    {
        public string NewIndex { get; set; }
        public string OldIndex { get; set; }
    }
}
