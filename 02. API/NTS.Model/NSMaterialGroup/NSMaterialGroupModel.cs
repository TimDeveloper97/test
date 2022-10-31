using NTS.Model.NSMaterialGroupAttach;
using NTS.Model.NSMaterialParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.NSMaterialGroup
{
    public class NSMaterialGroupModel
    {
        public string Id { get; set; }
        public string ManufactureId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ManufactureName { get; set; }
        public string TypeName { get; set; }

        public List<NSMaterialParameterModel> ListParameter { get; set; }
        public List<NSMaterialGroupAttachModel> ListFile { get; set; }

        public NSMaterialGroupModel()
        {
            ListParameter = new List<NSMaterialParameterModel>();
            ListFile = new List<NSMaterialGroupAttachModel>();
        }
        
    }
    
}
