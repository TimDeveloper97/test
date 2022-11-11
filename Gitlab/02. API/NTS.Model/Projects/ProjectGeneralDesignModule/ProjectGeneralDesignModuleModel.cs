using NTS.Model.ProductAccessories;
using NTS.Model.ProjectGeneralDesign;
using NTS.Model.ProjectProducts;
using NTS.Model.QLTKMODULE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesignModule
{
    public class ProjectGeneralDesignModuleModel : ModuleModel
    {
        public string ProjectGeneralDesignId { get; set; }
        public string ProjectProductId { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal ContractQuantity {get;set;}
        public decimal ErrorQuantity { get; set; }
        public decimal Amount { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal ModulePrice { get; set; }
        public int ModuleStatus { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Manufacture { get; set; }
        public int TotalError { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public int CreateIndex { get; set; }
        public int Index { get; set; }
        public bool IsNoPrice { get; set; }
        public int ApproveStatus { get; set; }
    }

    public class GetDataModel
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalPriceContract { get; set; }
        public ProjectProductsResuldModel Data { get; set; }
        public int CreateIndex { get; set; }
        public List<ProjectProductsResuldModel> ListModule { get; set; }
        public List<ProjectProductsResuldModel> ListModuleProduct { get; set; }
        public List<ProjectProductsResuldModel> ListModuleProductFalse { get; set; }
        public List<ProductAccessoriesModel> ListMaterial { get; set; }
        public ProjectGeneralDesignModel Models { get; set; }
        public bool CheckVersion { get; set; }
        public GetDataModel()
        {
            Data = new ProjectProductsResuldModel();
            ListModule = new List<ProjectProductsResuldModel>();
            ListModuleProduct = new List<ProjectProductsResuldModel>();
            ListModuleProductFalse = new List<ProjectProductsResuldModel>();
            ListMaterial = new List<ProductAccessoriesModel>();
            Models = new ProjectGeneralDesignModel();
        }
    }
}
