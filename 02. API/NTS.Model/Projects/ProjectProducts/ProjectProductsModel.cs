using NTS.Model.Projects.ProjectProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProducts
{
    public class ProjectProductsModel : BaseModel
    {
        public bool listId;

        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ParentId { get; set; }
        public string ModuleId { get; set; }
        public string ParentModuleId { get; set; }
        public string ModuleGroupId { get; set; }
        public string ProductId { get; set; }
        public string Code { get; set; }
        public string CodeView { get; set; }
        public string Name { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string Specifications { get; set; }
        public string SpecificationModule { get; set; }
        public int DataType { get; set; }
        public int ParentDataType { get; set; }
        public string DatatypeName { get; set; }
        public int ModuleStatus { get; set; }
        public string ModuleStatusName { get; set; }
        public int DesignStatus { get; set; }
        public string DesignStatusName { get; set; }
        public DateTime? DesignFinishDate { get; set; }
        public DateTime? MakeFinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountTHTK { get; set; }
        public decimal AmountIncurred { get; set; }
        public string Note { get; set; }
        public string Index { get; set; }
        public int SaveIndex { get; set; }
        public string ModuleName { get; set; }
        public string EmployeeId { get; set; }
        public string ContractIndex { get; set; }
        public int CreateIndex { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal Pricing { get; set; }
        public string THTK { get; set; }
        public bool CheckTHTK { get; set; }
        public bool Checked { get; set; }
        public decimal QuantityTHTK { get; set; }
        public decimal RealQuantityTHTK { get; set; }
        public decimal PriceTHTK { get; set; }
        public bool IsGeneralDesign { get; set; }
        public bool DesignWorkStatus { get; set; }
        public bool IsProductGeneralDesign { get; set; }
        public bool ColorGeneralDesign { get; set; }
        public decimal ModulePrice { get; set; }
        public string SBUId { get; set; }
        public int ApproveStatus { get; set; }
        public string DesignWorkStatusName { get; set; }
        public DateTime? DesignCloseDate { get; set; }
        public DateTime? GeneralDesignLastDate { get; set; }
        public bool MaterialExist { get; set; }
        public bool IsMaterial { get; set; }
        public bool IsIncurred { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public int StageIndex { get; set; }
        public int PlanStatus { get; set; }
        public int PlanWeight { get; set; }
        public int PlanDoneRatio { get; set; }
        public string ProjectProductId { get; set; }
        public DateTime? ContactEnDate { get; set; }
        public CheckedPublications ListPublications { get; set; }
        public string ProductName { get; set; }
        public bool IsCantalogs { get; set; }
        public bool IsMarketingPractice { get; set; }
        public bool IsMarketingDevice { get; set; }
        public bool IsMarketingMaintenance { get; set; }
        public bool MarketingPractice { get; set; }
        public bool MarketingDevice { get; set; }
        public bool MarketingMaintenance { get; set; }
        public bool MarketingCatalogs { get; set; }
        public bool IsNeedQC { get; set; }
        public int QCQuantity { get; set; }
        public string SerialNumber { get; set; }
        public int QCStatus { get; set; }
        public string CatalogRequireNote { get; set; }
        public string UserGuideRequireNote { get; set; }
        public string MaintenaineGuideRequireNote { get; set; }
        public string PracticeGuideRequireNote { get; set; }
        public string QCStatusName { get; set; }
    }

    public class ExportCompareModel
    {
        public string ContractIndex { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        public string ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public string Compare { get; set; }
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string Specifications { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
    }

    public class ImportModel
    {
        public string Index { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public decimal RealQuantity { get; set; }
    }

    public class ReturnModel
    {
        public string Specification { get; set; }
        public int Index { get; set; }
    }
}
