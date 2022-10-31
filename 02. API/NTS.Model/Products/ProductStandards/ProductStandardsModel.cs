using NTS.Model.Combobox;
using NTS.Model.Products.ProductStandards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardsModel : BaseModel
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string ModuleGroupId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Note { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }

        public string ProductStandardGroupName { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public string CreateByName { get; set; }
        public int DataType { get; set; }
        public string OK_Images { get; set; }
        public string NG_Images { get; set; }
        public List<ProductStandardAttachModel> ListFile { get; set; }
        public List<AttachmentModel> ListAttach { get; set; }
        public List<ProductStandardHistoryModel> ListHistory { get; set; }
        public List<ProductStandardImageModel> ListImage { get; set; }
        public List<ProductStandardImageModel> ListImageV { get; set; }
        public string ProjectProductId { get; set; }

        public ProductStandardsModel()
        {
            ListAttach = new List<AttachmentModel>();
            ListFile = new List<ProductStandardAttachModel>();
            ListHistory = new List<ProductStandardHistoryModel>();
            ListImage = new List<ProductStandardImageModel>();
            ListImageV = new List<ProductStandardImageModel>();
        }

    }
}
