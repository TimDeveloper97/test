using NTS.Model.MaterialParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialGroup
{
    public class UpdateMaterialGroupModel
    {
        public string Id { get; set; }
        public string CreateBy { get; set; }
        public List<MaterialParameterModel> ListParameter { get; set; }

        public UpdateMaterialGroupModel()
        {
            ListParameter = new List<MaterialParameterModel>();
        }
    }

    public class MaterialGroupModel
    {
        public string Id { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
        public List<MaterialParameterModel> ListParameter { get; set; }
        public List<string> ListParentId { get; set; }
        public List<string> ListParentName { get; set; }
        public List<string> ListChildId { get; set; }
        public MaterialGroupModel()
        {
            ListParameter = new List<MaterialParameterModel>();
            ListChildId = new List<string>();
        }
    }

}
