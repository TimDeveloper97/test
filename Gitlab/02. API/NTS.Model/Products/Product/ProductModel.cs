using NTS.Model.Practice;
using NTS.Model.ProductAccessories;
using NTS.Model.ProductCatalog;
using NTS.Model.ProductDocument;
using NTS.Model.ProductModuleUpdate;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class ProductModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CurentVersion { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public int ProcedureTime { get; set; }
        public decimal Period { get; set; }
        public decimal Pricing { get; set; }
        public string Specification { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }

        public List<ProductImageModel> ListImage { get; set; }
        public bool IsManualExist { get; set; }
        public bool IsQuoteExist { get; set; }
        public bool IsPracticeExist { get; set; }
        public bool IsLayoutExist { get; set; }
        public bool IsMaterialExist { get; set; }
        public bool IsManualMaintenance { get; set; }
        public bool IsCatalog { get; set; }
        public string ProductGroupName { get; set; }
        public bool IsTestResult { get; set; }

        public List<PracticeModel> ListPractice { get; set; }
        public List<ModuleInProductModel> ListModuleProduct { get; set; }
        public List<ProductDocumentModel> ListFielDocument { get; set; }
        public List<ProductCatalogModel> ListFileCatalog { get; set; }
        public List<ProductModuleUpdateModel> ListProductModuleUpdate { get; set; }

        public List<FileTestAttachModel> ListFileTestAttach { get; set; }
        public List<ProductAccessoriesModel> ListSelect { get; set; }
        public string Creator { get; set; }
        public string DepartmentCreated { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CreateByName { get; set; }
        public bool IsSendSale { get; set; }
        public ProductModel()
        {
            ListPractice = new List<PracticeModel>();
            ListProductModuleUpdate = new List<ProductModuleUpdateModel>();
            ListSelect = new List<ProductAccessoriesModel>();
        }
    }

    public class ProductImageModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Note { get; set; }
    }

    public class FileTestAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public decimal? FileSize { get; set; }
        public string CreateByName { get; set; }
    }
    public class SearchFileTestAttachModel
    {
        public bool IsTestResult { get; set; }
        public string ProductId { get; set; }
        public List<FileTestAttachModel> ListFileTestAttach { get; set; }
    }

    public class CheckStatusModel
    {
        public int Status { get; set; }
        public string ProductId { get; set; }
    }
}
