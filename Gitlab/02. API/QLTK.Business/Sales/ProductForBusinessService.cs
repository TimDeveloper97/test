using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Bussiness;
using NTS.Model.Bussiness.Application;
using NTS.Model.Combobox;
using NTS.Model.Job;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Sale.ProductForBusiness
{
    public class ProductForBusinessService
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<SaleProductModel> SearchSaleProduct(SaleProductSearchModel model, bool isPermission, bool isPermissionViewAll, List<string> listIdGroup)
        {
            List<string> listParentId = new List<string>();
            SearchResultModel<SaleProductModel> searchResult = new SearchResultModel<SaleProductModel>();

            List<string> productIds = new List<string>();

            if (!string.IsNullOrEmpty(model.JobId))
            {
                productIds.AddRange(db.SaleProductJobs.AsNoTracking().Where(r => r.JobId.Equals(model.JobId)).GroupBy(g => g.SaleProductId).Select(s => s.Key).ToList());
            }

            if (!string.IsNullOrEmpty(model.ApplicationId))
            {
                productIds.AddRange(db.SaleProductApplications.AsNoTracking().Where(r => r.ApplicationId.Equals(model.ApplicationId)).GroupBy(g => g.SaleProductId).Select(s => s.Key).ToList());
            }

            var dataQuery = (from a in db.SaleProducts.AsNoTracking()
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             join c in db.SaleProductTypes.AsNoTracking() on a.SaleProductTypeId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             select new SaleProductModel
                             {
                                 Id = a.Id,
                                 EName = a.EName,
                                 VName = a.VName,
                                 Model = a.Model,
                                 GroupCode = a.GroupCode,
                                 ChildGroupCode = a.ChildGroupCode,
                                 ManufactureName = da.Name,
                                 ManufactureId = a.ManufactureId,
                                 CountryName = a.CountryName,
                                 Specifications = a.Specifications,
                                 CustomerSpecifications = a.CustomerSpecifications,
                                 DeliveryTime = a.DeliveryTime,
                                 EXWTPADate = a.EXWTPADate,
                                 EXWTPAPrice = a.EXWTPAPrice,
                                 VAT = a.VAT,
                                 ExpireDateFrom = a.ExpireDateFrom,
                                 ExpireDateTo = a.ExpireDateTo,
                                 SpecificationDate = a.SpecificationDate,
                                 AvailableQuantity = a.AvailableQuantity,
                                 ExportQuantity = a.ExportQuantity,
                                 Inventory = a.Inventory,
                                 InventoryDate = a.InventoryDate,
                                 MaterialPrice = a.MaterialPrice,
                                 PublicPrice = a.PublicPrice,
                                 ProductType = a.SaleProductTypeId,
                                 ProductTypeName = acn != null ? acn.Name : string.Empty,
                                 Status = a.Status,
                                 IsSync = a.IsSync,
                                 ExistSolution = a.ExistSolution,
                                 ExistCatalog = a.ExistCatalog,
                                 ExistTrainingTechnique = a.ExistTrainingTechnique,
                                 ExistTrainingSale = a.ExistTrainingSale,
                                 ExistUserManual = a.ExistUserManual,
                                 ExistFixBug = a.ExistFixBug,
                             }).AsQueryable();
            if (productIds.Count > 0)
            {
                productIds = productIds.GroupBy(g => g).Select(s => s.Key).ToList();

                dataQuery = dataQuery.Where(r => productIds.Contains(r.Id));
            }

            var nearestImport = db.ImportInventories.AsNoTracking().FirstOrDefault(t => t.Type.Equals(Constants.ImportInventory_Type_SaleProduct));
            if (nearestImport != null)
            {
                searchResult.Date = nearestImport.Date;
            }

            var lastModifiedImport = db.ImportInventories.AsNoTracking().FirstOrDefault(t => t.Type.Equals(Constants.ImportInventory_Type_LastModified));
            if (lastModifiedImport != null)
            {
                searchResult.LastModifiedDate = lastModifiedImport.Date;
            }

            if (!isPermission)
            {
                dataQuery = dataQuery.Where(t => t.Status == true);

            }
            if (!isPermissionViewAll)
            {
                if (listIdGroup.Count > 0)
                {
                    var result = (from b in db.SaleGroupProducts.AsNoTracking()
                                  where listIdGroup.Contains(b.SaleGroupId)
                                  group b by b.SaleGroupId into g
                                  select g.Key).ToList();

                    dataQuery = dataQuery.Where(s => result.Contains(s.Id));
                }
            }

            if (!string.IsNullOrEmpty(model.SaleProductTypeId))
            {
                var saleTypeProduct = db.SaleProductTypes.FirstOrDefault(o => o.Id.Equals(model.SaleProductTypeId));
                if (model.SaleProductTypeId.Equals("CPL"))
                {
                    dataQuery = dataQuery.Where(s => string.IsNullOrEmpty(s.ProductType));
                }
                else if (model.SaleProductTypeId.Equals("SPB"))
                {
                    dataQuery = dataQuery.Where(t => t.Status == false);
                }
                else
                if (model.SaleProductTypeId.Contains("_pending"))
                {
                    var id = model.SaleProductTypeId.Replace("_pending", "");

                    dataQuery = dataQuery.Where(t => t.Status == false);
                    var saleProductTypes = db.SaleProductTypes.AsNoTracking().ToList();
                    var saleProductType = saleProductTypes.FirstOrDefault(i => i.Id.Equals(id));
                    if (saleProductType != null)
                    {
                        listParentId.Add(saleProductType.Id);
                    }

                    listParentId.AddRange(GetListParent(id, saleProductTypes));

                    var listModuleGroup = listParentId.AsQueryable();
                    dataQuery = (from a in dataQuery
                                 join b in listModuleGroup.AsQueryable() on a.ProductType equals b
                                 select a).AsQueryable();

                }
                else
                {
                    dataQuery = dataQuery.Where(t => t.Status == true);

                    var saleProductTypes = db.SaleProductTypes.AsNoTracking().ToList();
                    var saleProductType = saleProductTypes.FirstOrDefault(i => i.Id.Equals(model.SaleProductTypeId));
                    if (saleProductType != null)
                    {
                        listParentId.Add(saleProductType.Id);
                    }

                    listParentId.AddRange(GetListParent(model.SaleProductTypeId, saleProductTypes));

                    var listModuleGroup = listParentId.AsQueryable();
                    dataQuery = (from a in dataQuery
                                 join b in listModuleGroup.AsQueryable() on a.ProductType equals b
                                 select a).AsQueryable();
                }

            }
            else
            {
                dataQuery = dataQuery.Where(t => t.Status == true);
            }


            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(s => s.EName.Trim().Contains(model.Name) || s.VName.Trim().Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.Model))
            {
                dataQuery = dataQuery.Where(s => s.Model.Trim().Contains(model.Model));
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode.Trim().Contains(model.GroupCode));
            }
            if (!string.IsNullOrEmpty(model.ChildGroupCode))
            {
                dataQuery = dataQuery.Where(s => s.ChildGroupCode.Trim().Contains(model.ChildGroupCode));
            }
            if (!string.IsNullOrEmpty(model.ManufactureId))
            {
                dataQuery = dataQuery.Where(s => s.ManufactureId == model.ManufactureId);
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode == model.GroupCode);
            }
            if (!string.IsNullOrEmpty(model.CountryName))
            {
                dataQuery = dataQuery.Where(s => s.CountryName == model.CountryName);
            }
            if (!string.IsNullOrEmpty(model.Specifications))
            {
                dataQuery = dataQuery.Where(s => s.Specifications.Trim().Contains(model.Specifications) || s.CustomerSpecifications.Trim().Contains(model.Specifications));
            }
            if (model.SpecificationDate != null)
            {
                dataQuery = dataQuery.Where(s => s.SpecificationDate == model.SpecificationDate);
            }
            if (model.VAT.HasValue)
            {
                if (model.VATType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.VAT == model.VAT.Value);
                }
                else if (model.VATType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.VAT > model.VAT.Value);
                }
                else if (model.VATType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.VAT >= model.VAT.Value);
                }
                else if (model.VATType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.VAT < model.VAT.Value);
                }
                else if (model.VATType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.VAT <= model.VAT.Value);
                }
            }
            if (model.MaterialPrice.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice == model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice > model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice >= model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice < model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice <= model.MaterialPrice.Value);
                }
            }
            if (model.EXWTPAPrice.HasValue)
            {
                if (model.EXWTPAPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice == model.EXWTPAPrice.Value);
                }
                else if (model.EXWTPAPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice > model.EXWTPAPrice.Value);
                }
                else if (model.EXWTPAPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice >= model.EXWTPAPrice.Value);
                }
                else if (model.EXWTPAPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice < model.EXWTPAPrice.Value);
                }
                else if (model.EXWTPAPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice <= model.EXWTPAPrice.Value);
                }
            }
            if (model.PublicPrice.HasValue)
            {
                if (model.PublicPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice == model.PublicPrice.Value);
                }
                else if (model.PublicPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice > model.PublicPrice.Value);
                }
                else if (model.PublicPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice >= model.PublicPrice.Value);
                }
                else if (model.PublicPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice < model.PublicPrice.Value);
                }
                else if (model.PublicPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice <= model.PublicPrice.Value);
                }
            }
            if (model.EXWTPADateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate >= model.EXWTPADateFrom.Value);
            }
            if (model.EXWTPADateTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate <= model.EXWTPADateTo.Value);
            }
            if (model.ExpiredDateFromFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom >= model.ExpiredDateFromFrom.Value);
            }
            if (model.ExpiredDateFromTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom <= model.ExpiredDateFromTo.Value);
            }
            if (model.ExpiredDateToFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo >= model.ExpiredDateToFrom.Value);
            }
            if (model.ExpiredDateToTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo <= model.ExpiredDateToTo.Value);
            }
            if (model.InventoryDateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.InventoryDate >= model.InventoryDateFrom.Value);
            }
            if (model.InventoryDateTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.InventoryDate >= model.InventoryDateTo.Value);
            }
            if (model.Inventory.HasValue)
            {
                if (model.InventoryType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory == model.Inventory.Value);
                }
                else if (model.InventoryType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory > model.Inventory.Value);
                }
                else if (model.InventoryType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory >= model.Inventory.Value);
                }
                else if (model.InventoryType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory < model.Inventory.Value);
                }
                else if (model.InventoryType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory <= model.Inventory.Value);
                }
            }
            if (model.ExportQuantity.HasValue)
            {
                if (model.ExportQuantityType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity == model.ExportQuantity.Value);
                }
                else if (model.ExportQuantityType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity > model.ExportQuantity.Value);
                }
                else if (model.ExportQuantityType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity >= model.ExportQuantity.Value);
                }
                else if (model.ExportQuantityType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity < model.ExportQuantity.Value);
                }
                else if (model.ExportQuantityType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity <= model.ExportQuantity.Value);
                }
            }
            if (!string.IsNullOrEmpty(model.DeliveryTime))
            {
                dataQuery = dataQuery.Where(s => s.DeliveryTime == model.DeliveryTime);
            }
            if (model.Status.HasValue)
            {
                dataQuery = dataQuery.Where(t => t.Status == model.Status.Value);
            }
            if (model.IsSync.HasValue)
            {
                dataQuery = dataQuery.Where(t => t.IsSync == model.IsSync.Value);
            }
            if (model.DocStatus.HasValue)
            {
                if (model.DocStatus.Value)
                {
                    dataQuery = dataQuery.Where(t => t.ExistCatalog && t.ExistSolution && t.ExistTrainingTechnique && t.ExistTrainingSale && t.ExistUserManual && t.ExistFixBug);
                }
                else
                {
                    dataQuery = dataQuery.Where(t => !t.ExistCatalog || !t.ExistSolution || !t.ExistTrainingTechnique || !t.ExistTrainingSale || !t.ExistUserManual || !t.ExistFixBug);
                }
            }

            searchResult.TotalItem = dataQuery.Count();
            if (model.IsExport)
            {
                searchResult.ListResult = dataQuery.OrderBy(i => i.Model).ToList();
            }
            else
            {
                var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
                searchResult.ListResult = listResult;

                SaleProductMedia saleProductMedia;
                foreach (var item in searchResult.ListResult)
                {
                    saleProductMedia = db.SaleProductMedias.AsNoTracking().Where(s => s.SaleProductId == item.Id && s.Type == 1).FirstOrDefault();

                    if (saleProductMedia != null)
                    {
                        item.ImagePath = saleProductMedia.Path;
                    }
                }
            }
            return searchResult;
        }
        public List<string> GetListParent(string id, List<SaleProductType> saleProductTypes)
        {
            List<string> listChild = new List<string>();
            var moduleGroup = saleProductTypes.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item, saleProductTypes));
                }
            }
            return listChild;
        }

        public SearchResultModel<SaleProductModel> GetListAccessory(string idSaleProduct, SaleProductSearchModel model)
        {
            List<string> listParentId = new List<string>();
            SearchResultModel<SaleProductModel> searchResult = new SearchResultModel<SaleProductModel>();
            var dataQuery = (from i in db.SaleProductAccessories.AsNoTracking().Where(s => s.SaleProductId == idSaleProduct)
                             join a in db.SaleProducts.AsNoTracking() on i.ObjectId equals a.Id
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             join c in db.SaleProductTypes.AsNoTracking() on a.SaleProductTypeId equals c.Id
                             select new SaleProductModel
                             {
                                 Id = a.Id,
                                 EName = a.EName,
                                 VName = a.VName,
                                 Model = a.Model,
                                 GroupCode = a.GroupCode,
                                 ChildGroupCode = a.ChildGroupCode,
                                 ManufactureName = da.Name,
                                 ManufactureId = a.ManufactureId,
                                 CountryName = a.CountryName,
                                 Specifications = a.Specifications,
                                 CustomerSpecifications = a.CustomerSpecifications,
                                 DeliveryTime = a.DeliveryTime,
                                 EXWTPADate = a.EXWTPADate,
                                 EXWTPAPrice = a.EXWTPAPrice,
                                 VAT = a.VAT,
                                 ExpireDateFrom = a.ExpireDateFrom,
                                 ExpireDateTo = a.ExpireDateTo,
                                 SpecificationDate = a.SpecificationDate,
                                 //ExportPerson = a.ExportPerson,
                                 //ExportQuantity = a.ExportQuantity,
                                 Inventory = a.Inventory,
                                 InventoryDate = a.InventoryDate,
                                 MaterialPrice = a.MaterialPrice,
                                 PublicPrice = a.PublicPrice,
                                 //ImagePath = db.SaleProductMedias.AsNoTracking().Where(s => s.SaleProductId == a.Id && s.Type == 1).FirstOrDefault().Path,
                                 ProductType = a.SaleProductTypeId,
                                 ProductTypeName = c.Name,
                             }).AsQueryable();

            foreach (var item in dataQuery.ToList())
            {
                var path = db.SaleProductMedias.AsNoTracking().Where(s => s.SaleProductId == item.Id && s.Type == 1).FirstOrDefault();
                if (path != null)
                {
                    item.ImagePath = path.Path;
                }
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(s => s.EName.Trim().Contains(model.Name) || s.VName.Trim().Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.Model))
            {
                dataQuery = dataQuery.Where(s => s.Model.Trim().Contains(model.Model));
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode.Trim().Contains(model.GroupCode));
            }
            if (!string.IsNullOrEmpty(model.ChildGroupCode))
            {
                dataQuery = dataQuery.Where(s => s.ChildGroupCode.Trim().Contains(model.ChildGroupCode));
            }
            if (!string.IsNullOrEmpty(model.ManufactureId))
            {
                dataQuery = dataQuery.Where(s => s.ManufactureId == model.ManufactureId);
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode == model.GroupCode);
            }
            if (!string.IsNullOrEmpty(model.CountryName))
            {
                dataQuery = dataQuery.Where(s => s.CountryName == model.CountryName);
            }
            if (!string.IsNullOrEmpty(model.Specifications))
            {
                dataQuery = dataQuery.Where(s => s.Specifications.Trim().Contains(model.Specifications) || s.CustomerSpecifications.Trim().Contains(model.Specifications));
            }
            if (model.SpecificationDate != null)
            {
                dataQuery = dataQuery.Where(s => s.SpecificationDate == model.SpecificationDate);
            }
            if (model.VAT.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.VAT == model.VAT.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.VAT > model.VAT.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.VAT >= model.VAT.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.VAT < model.VAT.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.VAT <= model.VAT.Value);
                }
            }
            if (model.MaterialPrice.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice == model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice > model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice >= model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice < model.MaterialPrice.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice <= model.MaterialPrice.Value);
                }
            }
            if (model.EXWTPAPrice.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice == model.EXWTPAPrice.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice > model.EXWTPAPrice.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice >= model.EXWTPAPrice.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice < model.EXWTPAPrice.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice <= model.EXWTPAPrice.Value);
                }
            }
            if (model.PublicPrice.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice == model.PublicPrice.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice > model.PublicPrice.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice >= model.PublicPrice.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice < model.PublicPrice.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice <= model.PublicPrice.Value);
                }
            }
            if (model.EXWTPADateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate >= model.EXWTPADateFrom.Value);
            }
            if (model.EXWTPADateTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate <= model.EXWTPADateTo.Value);
            }
            if (model.ExpiredDateFromFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom >= model.ExpiredDateFromFrom.Value);
            }
            if (model.ExpiredDateFromTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom <= model.ExpiredDateFromTo.Value);
            }
            if (model.ExpiredDateToFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo >= model.ExpiredDateToFrom.Value);
            }
            if (model.ExpiredDateToTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo <= model.ExpiredDateToTo.Value);
            }
            if (model.InventoryDateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.InventoryDate >= model.InventoryDateFrom.Value);
            }
            if (model.InventoryDateTo.HasValue)
            {
                dataQuery = dataQuery.Where(s => s.InventoryDate >= model.InventoryDateTo.Value);
            }
            if (model.Inventory.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory == model.Inventory.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory > model.Inventory.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory >= model.Inventory.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory < model.Inventory.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory <= model.Inventory.Value);
                }
            }
            if (model.ExportQuantity.HasValue)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity == model.ExportQuantity.Value);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity > model.ExportQuantity.Value);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity >= model.ExportQuantity.Value);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity < model.ExportQuantity.Value);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity <= model.ExportQuantity.Value);
                }
            }
            if (!string.IsNullOrEmpty(model.DeliveryTime))
            {
                dataQuery = dataQuery.Where(s => s.DeliveryTime == model.DeliveryTime);
            }
            if (model.Status.HasValue)
            {
                dataQuery = dataQuery.Where(t => t.Status == model.Status.Value);
            }
            if (model.IsSync.HasValue)
            {
                dataQuery = dataQuery.Where(t => t.IsSync == model.IsSync.Value);
            }
            searchResult.TotalItem = dataQuery.Count();

            var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        //Chưa lm xong
        public string ExportExcel(SaleProductSearchModel model, bool isPermission, bool isPermissionViewAll, List<string> listIdGroup)
        {
            model.IsExport = true;
            var data = SearchSaleProduct(model, isPermission, isPermissionViewAll, listIdGroup);
            var list = data.ListResult.ToList();

            var listData = db.SaleProducts.AsNoTracking().ToList();
            SaleProduct saleProduct;
            foreach (var item in data.ListResult)
            {
                saleProduct = new SaleProduct();
                saleProduct = listData.FirstOrDefault(i => i.Id.Equals(item.Id));

            }

            //ListModel listModel = new ListModel();

            if (list.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                application.EnableIncrementalFormula = true;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/sp_cho_kinh_doanh.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = list.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var listExport = list.Select((a, i) => new
                {
                    Index = i + 1,
                    a.EName,
                    a.VName,
                    a.Model,
                    a.GroupCode,
                    a.ChildGroupCode,
                    a.ManufactureName,
                    a.CountryName,
                    a.ProductTypeName,
                    a.Specifications,
                    a.CustomerSpecifications,
                    a.SpecificationDate,
                    a.VAT,
                    a.MaterialPrice,
                    a.EXWTPAPrice,
                    a.EXWTPADate,
                    a.PublicPrice,
                    a.ExpireDateFrom,
                    a.ExpireDateTo,
                    a.DeliveryTime,
                    a.Inventory,
                    a.ExportQuantity,
                    a.AvailableQuantity,
                    a.InventoryDate,
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 142].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách sản phẩm kinh doanh" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách sản phẩm kinh doanh" + ".xlsx";
                db.SaveChanges();
                return resultPathClient;

            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }
        public void CreateSaleProduct(SaleProductCreateModel model, string userId, string sbuId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (model.ExpireDateFrom != null && model.ExpireDateTo != null)
                {
                    if (model.ExpireDateFrom > model.ExpireDateTo)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0012, TextResourceKey.SaleProducts);
                    }
                }

                if (!string.IsNullOrEmpty(model.SaleProductTypeId))
                {
                    string sbuTypeId = GetSBUIdByTypeId(model.SaleProductTypeId);

                    if (!string.IsNullOrEmpty(sbuTypeId) && !sbuTypeId.Equals(sbuId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0082, TextResourceKey.SaleProductType);
                    }
                }

                try
                {
                    SaleProduct saleProduct = new SaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EName = model.EName,
                        VName = model.VName,
                        Model = model.Model,
                        GroupCode = model.GroupCode,
                        ChildGroupCode = model.ChildGroupCode,
                        ManufactureId = model.ManufactureId,
                        CountryName = model.CountryName,
                        SpecificationDate = model.SpecificationDate,
                        Specifications = model.Specifications,
                        CustomerSpecifications = model.CustomerSpecifications,
                        VAT = model.VAT.HasValue ? model.VAT.Value : 0,
                        MaterialPrice = model.MaterialPrice.HasValue ? model.MaterialPrice.Value : 0,
                        EXWTPAPrice = model.EXWTPAPrice.HasValue ? model.EXWTPAPrice.Value : 0,
                        PublicPrice = model.PublicPrice,
                        ExpireDateFrom = model.ExpireDateFrom,
                        ExpireDateTo = model.ExpireDateTo,
                        DeliveryTime = model.DeliveryTime,
                        EXWTPADate = model.EXWTPADate,
                        SaleProductTypeId = model.SaleProductTypeId,
                        Status = true,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    AddMedia(saleProduct.Id, model.SaleProductMediaModels);
                    AddApp(saleProduct.Id, model.SaleProductAppModels);
                    AddAccessory(saleProduct.Id, model.SaleProductAccessoryModels);
                    AddDocument(saleProduct, model.SaleProductDocumnetModels);
                    AddJob(saleProduct.Id, model.SaleProductJobModels);
                    AddGroupProduct(saleProduct.Id, model.SaleGroupProduct);
                    db.SaleProducts.Add(saleProduct);
                    UserLogUtil.LogHistotyAdd(db, userId, saleProduct.Model, saleProduct.Id, Constants.LOG_SaleProduct);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        private string GetSBUIdByTypeId(string id)
        {
            string sbuId = string.Empty;
            var saleProductType = db.SaleProductTypes.AsNoTracking().FirstOrDefault(r => r.Id.Equals(id));

            if (saleProductType != null)
            {
                if (!string.IsNullOrEmpty(saleProductType.SBUId))
                {
                    sbuId = saleProductType.SBUId;
                }
                else
                {
                    sbuId = GetSBUIdByTypeId(saleProductType.ParentId);
                }
            }

            return sbuId;
        }


        private void AddJob(string id, List<string> saleProductJobModels)
        {
            var listJOb = new List<SaleProductJob>();
            var listOld = db.SaleProductJobs.Where(s => s.SaleProductId == id).ToList();
            db.SaleProductJobs.RemoveRange(listOld);
            foreach (var item in saleProductJobModels)
            {
                var saleProduce = new SaleProductJob
                {
                    Id = Guid.NewGuid().ToString(),
                    JobId = item,
                    SaleProductId = id,
                };
                listJOb.Add(saleProduce);
            }
            db.SaleProductJobs.AddRange(listJOb);
        }

        private void AddDocument(SaleProduct saleProduct, List<SaleProductDocumentModel> saleProductDocumnetModels)
        {
            var listProductDocument = new List<SaleProductDocument>();
            var listOldDocumnet = db.SaleProductDocuments.Where(s => s.SaleProductId.Equals(saleProduct.Id)).ToList();
            db.SaleProductDocuments.RemoveRange(listOldDocumnet);
            foreach (var item in saleProductDocumnetModels)
            {
                var type = new SaleProductDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleProductId = saleProduct.Id,
                    CreateDate = DateTime.Now,
                    Path = item.Path,
                    FileName = item.FileName,
                    FileSize = item.FileSize,
                    Type = item.Type,
                };
                listProductDocument.Add(type);

                if (item.Type.Equals(Constants.SaleProductDocument_Type_Solution))
                {
                    saleProduct.ExistSolution = true;
                }
                else if (item.Type.Equals(Constants.SaleProductDocument_Type_Catalog))
                {
                    saleProduct.ExistCatalog = true;
                }
                else if (item.Type.Equals(Constants.SaleProductDocument_Type_TechnicalTraining))
                {
                    saleProduct.ExistTrainingTechnique = true;
                }
                else if (item.Type.Equals(Constants.SaleProductDocument_Type_SaleTraining))
                {
                    saleProduct.ExistTrainingSale = true;
                }
                else if (item.Type.Equals(Constants.SaleProductDocument_Type_UserManual))
                {
                    saleProduct.ExistUserManual = true;
                }
                else
                {
                    saleProduct.ExistFixBug = true;
                }
            }
            db.SaleProductDocuments.AddRange(listProductDocument);
        }

        private void AddAccessory(string id, List<string> saleProductAccessoryModels)
        {
            var listAccessory = new List<SaleProductAccessory>();
            var listOld = db.SaleProductAccessories.Where(s => s.SaleProductId == id).ToList();
            db.SaleProductAccessories.RemoveRange(listOld);
            foreach (var item in saleProductAccessoryModels)
            {
                var saleProductAccessory = new SaleProductAccessory
                {
                    Id = Guid.NewGuid().ToString(),
                    ObjectId = item,
                    SaleProductId = id,
                };
                listAccessory.Add(saleProductAccessory);
            }
            if (listAccessory.Count > 0)
            {
                db.SaleProductAccessories.AddRange(listAccessory);
            }
        }

        private void AddApp(string id, List<string> saleProductAppModels)
        {
            var listApp = new List<SaleProductApplication>();
            var listOld = db.SaleProductApplications.Where(s => s.SaleProductId == id).ToList();
            db.SaleProductApplications.RemoveRange(listOld);
            foreach (var item in saleProductAppModels)
            {
                var saleProductApp = new SaleProductApplication
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationId = item,
                    SaleProductId = id,
                };
                listApp.Add(saleProductApp);
            }
            db.SaleProductApplications.AddRange(listApp);
        }


        private void AddGroupProduct(string id, List<string> saleGroupProduct)
        {
            var listProductGroup = new List<SaleGroupProduct>();
            var listOldGroup = db.SaleGroupProducts.Where(s => s.SaleProductId == id).ToList();
            db.SaleGroupProducts.RemoveRange(listOldGroup);
            foreach (var item in saleGroupProduct)
            {
                var groupProduct = new SaleGroupProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleProductId = id,
                    SaleGroupId = item
                };
                listProductGroup.Add(groupProduct);
            }
            db.SaleGroupProducts.AddRange(listProductGroup);
        }

        private void AddMedia(string id, List<SaleProductMediaModel> saleProductMediaModels)
        {
            var listProductMedia = new List<SaleProductMedia>();
            var listOldMedia = db.SaleProductMedias.Where(s => s.SaleProductId == id).ToList();
            db.SaleProductMedias.RemoveRange(listOldMedia);
            foreach (var item in saleProductMediaModels)
            {
                var type = new SaleProductMedia
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleProductId = id,
                    CreateDate = DateTime.Now,
                    Path = item.Path,
                    FileName = item.FileName,
                    FileSize = item.FileSize,
                    Type = item.Type,
                };
                listProductMedia.Add(type);
            }
            db.SaleProductMedias.AddRange(listProductMedia);
        }
        /// <summary>
        /// Get list job
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public object SearchJob(JobSearchModel modelSearch)
        {
            SearchResultModel<JobModel> searchResult = new SearchResultModel<JobModel>();
            try
            {
                var data = (from a in db.Jobs.AsNoTracking()
                            where !modelSearch.ListIdSelect.Contains(a.Id)
                            join b in db.JobGroups.AsNoTracking() on a.JobGroupId equals b.Id
                            join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
                            join d in db.JobSubjects.AsNoTracking() on a.Id equals d.JobId into da
                            from d in da.DefaultIfEmpty()
                            join e in db.Subjects.AsNoTracking() on d.SubjectsId equals e.Id into ea
                            from e in ea.DefaultIfEmpty()
                            orderby a.Code
                            select new JobModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                JobGroupId = b.Id,
                                JobGroupName = b.Name,
                                DegreeId = c.Id,
                                DegreeName = c.Name,
                                Description = a.Description,
                                SubjectId = e.Id,
                                SubjectName = e.Name,
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    data = data.Where(t => t.Code.Contains(modelSearch.Code));
                }
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    data = data.Where(t => t.Name.Contains(modelSearch.Name));
                }
                if (!string.IsNullOrEmpty(modelSearch.JobGroupId))
                {
                    data = data.Where(t => t.JobGroupId.Contains(modelSearch.JobGroupId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DegreeId))
                {
                    data = data.Where(t => t.DegreeId.Contains(modelSearch.DegreeId));
                }
                if (!string.IsNullOrEmpty(modelSearch.SubjectName))
                {
                    data = data.Where(t => t.SubjectName.Contains(modelSearch.SubjectName));
                }
                searchResult.TotalItem = data.Count();
                searchResult.ListResult = data.ToList(); ;
            }
            catch (Exception ex)
            {
                throw;
            }
            return searchResult;
        }
        public SearchResultModel<ApplicationModel> SearchApplication(ApplicationSearchModel modelSearch)
        {
            SearchResultModel<ApplicationModel> searchResult = new SearchResultModel<ApplicationModel>();

            var dataQuery = (from a in db.Applications.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Index
                             select new ApplicationModel
                             {
                                 Index = a.Index,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Id = a.Id,
                             }).AsQueryable();

            // Tìm kiếm theo tên khóa học
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public SearchResultModel<SaleProductModel> GetAllSaleProduct(SaleProductSearchModel model, bool isPermission, bool isPermissionViewAll, List<string> listIdGroup)
        {
            List<string> listParentId = new List<string>();
            SearchResultModel<SaleProductModel> searchResult = new SearchResultModel<SaleProductModel>();
            var dataQuery = (from a in db.SaleProducts.AsNoTracking()
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             join c in db.SaleProductTypes.AsNoTracking() on a.SaleProductTypeId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             select new SaleProductModel
                             {
                                 Id = a.Id,
                                 EName = a.EName,
                                 VName = a.VName,
                                 Model = a.Model,
                                 GroupCode = a.GroupCode,
                                 ChildGroupCode = a.ChildGroupCode,
                                 ManufactureName = da.Name,
                                 ManufactureId = a.ManufactureId,
                                 CountryName = a.CountryName,
                                 Specifications = a.Specifications,
                                 CustomerSpecifications = a.CustomerSpecifications,
                                 DeliveryTime = a.DeliveryTime,
                                 EXWTPADate = a.EXWTPADate,
                                 EXWTPAPrice = a.EXWTPAPrice,
                                 VAT = a.VAT,
                                 ExpireDateFrom = a.ExpireDateFrom,
                                 ExpireDateTo = a.ExpireDateTo,
                                 SpecificationDate = a.SpecificationDate,
                                 //ExportPerson = a.ExportPerson,
                                 //ExportQuantity = a.ExportQuantity,
                                 Inventory = a.Inventory,
                                 InventoryDate = a.InventoryDate,
                                 MaterialPrice = a.MaterialPrice,
                                 PublicPrice = a.PublicPrice,
                                 ImagePath = a.AvatarPath,
                                 ProductType = a.SaleProductTypeId,
                                 ProductTypeName = acn != null ? acn.Name : string.Empty,
                                 Status = a.Status,
                                 IsSync = a.IsSync,
                                 ExistSolution = a.ExistSolution,
                                 ExistCatalog = a.ExistCatalog,
                                 ExistTrainingTechnique = a.ExistTrainingTechnique,
                                 ExistTrainingSale = a.ExistTrainingSale,
                                 ExistUserManual = a.ExistUserManual,
                                 ExistFixBug = a.ExistFixBug
                             }).AsQueryable();
            if (!isPermission)
            {
                dataQuery = dataQuery.Where(t => t.Status == true);
            }
            if (!isPermissionViewAll)
            {
                if (listIdGroup.Count > 0)
                {
                    var result = (from b in db.SaleGroupProducts.AsNoTracking()
                                  where listIdGroup.Contains(b.SaleGroupId)
                                  group b by b.SaleGroupId into g
                                  select g.Key).ToList();
                    dataQuery = dataQuery.Where(s => result.Contains(s.Id));

                }
            }
            if (!string.IsNullOrEmpty(model.SaleProductTypeId))
            {
                var saleProductTypes = db.SaleProductTypes.AsNoTracking().ToList();
                var saleProductType = saleProductTypes.FirstOrDefault(i => i.Id.Equals(model.SaleProductTypeId));
                if (saleProductType != null)
                {
                    listParentId.Add(saleProductType.Id);
                }

                listParentId.AddRange(GetListParent(model.SaleProductTypeId, saleProductTypes));

                var listModuleGroup = listParentId.AsQueryable();
                dataQuery = (from a in dataQuery
                             join b in listModuleGroup.AsQueryable() on a.ProductType equals b
                             select a).AsQueryable();
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(s => s.EName.Trim().Contains(model.Name) || s.VName.Trim().Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.Model))
            {
                dataQuery = dataQuery.Where(s => s.Model.Trim().Contains(model.Model));
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode.Trim().Contains(model.GroupCode));
            }
            if (!string.IsNullOrEmpty(model.ChildGroupCode))
            {
                dataQuery = dataQuery.Where(s => s.ChildGroupCode.Trim().Contains(model.ChildGroupCode));
            }
            if (!string.IsNullOrEmpty(model.ManufactureId))
            {
                dataQuery = dataQuery.Where(s => s.ManufactureId == model.ManufactureId);
            }
            if (!string.IsNullOrEmpty(model.GroupCode))
            {
                dataQuery = dataQuery.Where(s => s.GroupCode == model.GroupCode);
            }
            if (!string.IsNullOrEmpty(model.CountryName))
            {
                dataQuery = dataQuery.Where(s => s.CountryName == model.CountryName);
            }
            if (!string.IsNullOrEmpty(model.Specifications))
            {
                dataQuery = dataQuery.Where(s => s.Specifications.Trim().Contains(model.Specifications) || s.CustomerSpecifications.Trim().Contains(model.Specifications));
            }
            if (model.SpecificationDate != null)
            {
                dataQuery = dataQuery.Where(s => s.SpecificationDate == model.SpecificationDate);
            }
            if (model.VAT != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.VAT == model.VAT);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.VAT > model.VAT);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.VAT >= model.VAT);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.VAT < model.VAT);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.VAT <= model.VAT);
                }
            }
            if (model.MaterialPrice != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice == model.MaterialPrice);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice > model.MaterialPrice);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice >= model.MaterialPrice);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice < model.MaterialPrice);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.MaterialPrice <= model.MaterialPrice);
                }
            }
            if (model.EXWTPAPrice != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice == model.EXWTPAPrice);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice > model.EXWTPAPrice);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice >= model.EXWTPAPrice);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice < model.EXWTPAPrice);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.EXWTPAPrice <= model.EXWTPAPrice);
                }
            }
            if (model.PublicPrice != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice == model.PublicPrice);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice > model.PublicPrice);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice >= model.PublicPrice);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice < model.PublicPrice);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.PublicPrice <= model.PublicPrice);
                }
            }
            if (model.DateFrom != null)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate >= model.DateFrom);
            }
            if (model.DateTo != null)
            {
                dataQuery = dataQuery.Where(s => s.EXWTPADate <= model.DateTo);
            }
            if (model.DateFrom != null)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom >= model.DateFrom);
            }
            if (model.DateTo != null)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateFrom <= model.DateTo);
            }
            if (model.DateFrom != null)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo >= model.DateFrom);
            }
            if (model.DateTo != null)
            {
                dataQuery = dataQuery.Where(s => s.ExpireDateTo <= model.DateTo);
            }
            if (model.Inventory != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory == model.Inventory);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory > model.Inventory);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory >= model.Inventory);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory < model.Inventory);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.Inventory <= model.Inventory);
                }
            }
            if (model.ExportQuantity != null)
            {
                if (model.MaterialPriceType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity == model.ExportQuantity);
                }
                else if (model.MaterialPriceType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity > model.ExportQuantity);
                }
                else if (model.MaterialPriceType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity >= model.ExportQuantity);
                }
                else if (model.MaterialPriceType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity < model.ExportQuantity);
                }
                else if (model.MaterialPriceType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.ExportQuantity <= model.ExportQuantity);
                }
            }
            if (!string.IsNullOrEmpty(model.DeliveryTime))
            {
                dataQuery = dataQuery.Where(s => s.DeliveryTime == model.DeliveryTime);
            }
            searchResult.TotalItem = dataQuery.Count();

            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void UpdateSaleProduct(string Id, SaleProductCreateModel model, string userId, string sbuId)
        {
            SaleProduct oldSaleProduct = db.SaleProducts.FirstOrDefault(t => t.Id.Equals(Id));

            if (oldSaleProduct == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProducts);
            }

            if (!string.IsNullOrEmpty(oldSaleProduct.SaleProductTypeId))
            {
                string sbuTypeId = GetSBUIdByTypeId(oldSaleProduct.SaleProductTypeId);

                //if (!string.IsNullOrEmpty(sbuTypeId) && !sbuTypeId.Equals(sbuId))
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0031, TextResourceKey.SaleProductType);
                //}
            }

            else
            {
                //var jsonBefore = AutoMapperConfig.Mapper.Map<SaleProductModel>(oldSaleProduct);
                using (var trans = db.Database.BeginTransaction())
                {
                    if (model.ExpireDateFrom != null && model.ExpireDateTo != null)
                    {
                        if (model.ExpireDateFrom > model.ExpireDateTo)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0012, TextResourceKey.SaleProducts);
                        }
                    }
                    try
                    {
                        if (model.SaleProductDocumnetModels.Count > 0)
                        {
                            var countDocument = from a in model.SaleProductDocumnetModels
                                                group a by a.Type into g
                                                select new { Type = g.Key, Count = g.Count() };
                            foreach (var item in countDocument)
                            {
                                if (item.Type == 1 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistSolution = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistSolution = false;
                                }
                                if (item.Type == 2 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistCatalog = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistCatalog = false;
                                }
                                if (item.Type == 3 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistTrainingTechnique = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistTrainingTechnique = false;
                                }
                                if (item.Type == 4 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistTrainingSale = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistTrainingSale = false;
                                }
                                if (item.Type == 5 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistUserManual = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistUserManual = false;
                                }
                                if (item.Type == 6 && item.Count > 0)
                                {
                                    oldSaleProduct.ExistFixBug = true;
                                }
                                else
                                {
                                    oldSaleProduct.ExistFixBug = false;
                                }
                            }
                        }
                        oldSaleProduct.EName = model.EName;
                        oldSaleProduct.VName = model.VName;
                        oldSaleProduct.Model = model.Model;
                        oldSaleProduct.GroupCode = model.GroupCode;
                        oldSaleProduct.ChildGroupCode = model.ChildGroupCode;
                        oldSaleProduct.ManufactureId = model.ManufactureId;
                        oldSaleProduct.CountryName = model.CountryName;
                        oldSaleProduct.SpecificationDate = model.SpecificationDate;
                        oldSaleProduct.Specifications = model.Specifications;
                        oldSaleProduct.CustomerSpecifications = model.CustomerSpecifications;
                        if (model.VAT.HasValue)
                        {
                            oldSaleProduct.VAT = model.VAT.Value;
                        }
                        if (model.MaterialPrice.HasValue)
                        {
                            oldSaleProduct.MaterialPrice = model.MaterialPrice.Value;
                        }
                        if (model.EXWTPAPrice.HasValue)
                        {
                            oldSaleProduct.EXWTPAPrice = model.EXWTPAPrice.Value;
                        }
                        oldSaleProduct.PublicPrice = model.PublicPrice;
                        oldSaleProduct.ExpireDateFrom = model.ExpireDateFrom;
                        oldSaleProduct.ExpireDateTo = model.ExpireDateTo;
                        oldSaleProduct.DeliveryTime = model.DeliveryTime;
                        oldSaleProduct.EXWTPADate = model.EXWTPADate;
                        oldSaleProduct.SaleProductTypeId = model.SaleProductTypeId;

                        oldSaleProduct.UpdateBy = userId;
                        oldSaleProduct.UpdateDate = DateTime.Now;

                        AddMedia(Id, model.SaleProductMediaModels);
                        AddApp(Id, model.SaleProductAppModels);
                        AddAccessory(Id, model.SaleProductAccessoryModels);
                        AddDocument(oldSaleProduct, model.SaleProductDocumnetModels);
                        AddJob(Id, model.SaleProductJobModels);
                        AddGroupProduct(Id, model.SaleGroupProduct);
                        //var jsonAfter = AutoMapperConfig.Mapper.Map<SaleProductModel>(oldSaleProduct);
                        //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_SaleProduct, oldSaleProduct.Id, oldSaleProduct.Model, jsonBefore, jsonAfter);
                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new NTSLogException(model, ex);
                    }
                }
            }

        }
        /// <summary>
        /// Chi tiết sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SaleProductDetailModel GetInfoById(string Id)
        {
            var saleProductDetail = (from a in db.SaleProducts.AsNoTracking()
                                     where a.Id.Equals(Id)
                                     select new SaleProductDetailModel
                                     {
                                         Id = Id,
                                         EName = a.EName,
                                         VName = a.VName,
                                         GroupCode = a.GroupCode,
                                         ChildGroupCode = a.ChildGroupCode,
                                         CountryName = a.CountryName,
                                         DeliveryTime = a.DeliveryTime,
                                         PublicPrice = a.PublicPrice,
                                         CustomerSpecifications = a.CustomerSpecifications,
                                         SpecificationDate = a.SpecificationDate,
                                         Specifications = a.Specifications,
                                         VAT = a.VAT,
                                         ExpireDateFrom = a.ExpireDateFrom,
                                         ExpireDateTo = a.ExpireDateTo,
                                         //ExportPerson = a.ExportPerson,
                                         ExportQuantity = a.ExportQuantity,
                                         EXWTPADate = a.EXWTPADate,
                                         EXWTPAPrice = a.EXWTPAPrice,
                                         InventoryDate = a.InventoryDate,
                                         Inventory = a.Inventory,
                                         ManufactureId = a.ManufactureId,
                                         MaterialPrice = a.MaterialPrice,
                                         Model = a.Model,
                                         SaleProductTypeId = a.SaleProductTypeId,
                                     }).FirstOrDefault();
            saleProductDetail.ListImage = (from m in db.SaleProductMedias.AsNoTracking()
                                           select new SaleProductMediaModel
                                           {
                                               SaleProductId = m.SaleProductId,
                                               Id = m.Id,
                                               FileName = m.FileName,
                                               FileSize = m.FileSize,
                                               Path = m.Path,
                                               CreateDate = m.CreateDate,
                                               Type = m.Type,
                                           }).Where(s => s.SaleProductId.Equals(Id) && s.Type == 1).ToList();
            return saleProductDetail;
        }
        /// <summary>
        /// dánh sách ứng dụng theo Id sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<ApplicationInfoModel> GetAppById(string Id)
        {
            var saleProductApplication = (from a in db.SaleProductApplications.AsNoTracking()
                                          where a.SaleProductId.Equals(Id)
                                          join b in db.Applications.AsNoTracking()
                                          on a.ApplicationId equals b.Id
                                          select new ApplicationInfoModel
                                          {
                                              Id = b.Id,
                                              Name = b.Name,
                                              Note = b.Note,
                                          }).ToList();
            return saleProductApplication;
        }
        /// <summary>
        /// Dánh sách nghề nghiệp theo id sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<CareeInfoModel> GetCareeById(string Id)
        {
            var saleProductJob = (from a in db.SaleProductJobs.AsNoTracking()
                                  where a.SaleProductId.Equals(Id)
                                  join b in db.Jobs.AsNoTracking()
                                  on a.JobId equals b.Id
                                  select new CareeInfoModel
                                  {
                                      Id = b.Id,
                                      Name = b.Name,
                                      Code = b.Code,
                                  }).ToList();
            return saleProductJob;
        }
        /// <summary>
        /// Danh sách media theo id sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<MediaInfoModel> GetMediaById(string Id)
        {
            var saleProductMedia = (from a in db.SaleProductMedias.AsNoTracking()
                                    where a.SaleProductId.Equals(Id)
                                    select new MediaInfoModel
                                    {
                                        Id = a.Id,
                                        FileName = a.FileName,
                                        FileSize = a.FileSize,
                                        CreatDate = a.CreateDate,
                                        Path = a.Path,
                                        Type = a.Type
                                    }).ToList();
            return saleProductMedia;
        }
        /// <summary>
        /// Danh sách tài liệu theo id sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<DocumentInfoModel> GetDocumentById(string Id)
        {
            var saleProductDocument = (from a in db.SaleProductDocuments.AsNoTracking()
                                       where a.SaleProductId.Equals(Id)
                                       select new DocumentInfoModel
                                       {
                                           Id = a.Id,
                                           FileName = a.FileName,
                                           FileSize = a.FileSize,
                                           CreatDate = a.CreateDate,
                                           Path = a.Path,
                                           Type = a.Type
                                       }).ToList();
            return saleProductDocument;
        }
        /// <summary>
        /// Danh sách phụ kiện theo Id sp
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<AccessoryInfoModel> GetAccessoryById(string Id)
        {
            var data = (from i in db.SaleProductAccessories.AsNoTracking().Where(s => s.SaleProductId == Id)
                        join a in db.SaleProducts.AsNoTracking() on i.ObjectId equals a.Id
                        join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                        from adc in ad.DefaultIfEmpty()
                        join c in db.SaleProductTypes.AsNoTracking() on a.SaleProductTypeId equals c.Id into ac
                        from acc in ac.DefaultIfEmpty()
                        select new AccessoryInfoModel
                        {
                            Id = a.Id,
                            EName = a.EName,
                            VName = a.VName,
                            Model = a.Model,
                            GroupCode = a.GroupCode,
                            ChildGroupCode = a.ChildGroupCode,
                            ManufactureName = adc.Name,
                            ManufactureId = a.ManufactureId,
                            CountryName = a.CountryName,
                            Specifications = a.Specifications,
                            CustomerSpecifications = a.CustomerSpecifications,
                            DeliveryTime = a.DeliveryTime,
                            EXWTPADate = a.EXWTPADate,
                            EXWTPAPrice = a.EXWTPAPrice,
                            VAT = a.VAT,
                            ExpireDateFrom = a.ExpireDateFrom,
                            ExpireDateTo = a.ExpireDateTo,
                            SpecificationDate = a.SpecificationDate,
                            //ExportPerson = a.ExportPerson,
                            //ExportQuantity = a.ExportQuantity,
                            Inventory = a.Inventory,
                            InventoryDate = a.InventoryDate,
                            MaterialPrice = a.MaterialPrice,
                            PublicPrice = a.PublicPrice,
                            ImagePath = a.AvatarPath,
                            ProductType = a.SaleProductTypeId,
                            ProductTypeName = acc.Name,
                        }).ToList();
            return data;
        }

        public void DeleteSaleProduct(string Id, string userId, string sbuId)
        {
            var oldSaleProduct = db.SaleProducts.FirstOrDefault(t => t.Id.Equals(Id));
            if (oldSaleProduct == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProducts);
            }

            if (!string.IsNullOrEmpty(oldSaleProduct.SaleProductTypeId))
            {
                string sbuTypeId = GetSBUIdByTypeId(oldSaleProduct.SaleProductTypeId);

                if (!string.IsNullOrEmpty(sbuTypeId) && !sbuTypeId.Equals(sbuId))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0036, TextResourceKey.SaleProductType);
                }
            }

            var saleExportDetail = db.SaleProductExportDetails.Where(s => s.SaleProductId == Id).ToList();
            if (saleExportDetail.Count > 0 && saleExportDetail != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SaleProducts);
            }
            else
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var saleProductJob = db.SaleProductJobs.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleProductJobs.RemoveRange(saleProductJob);
                        var saleProductApp = db.SaleProductApplications.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleProductApplications.RemoveRange(saleProductApp);
                        var saleGroupProduct = db.SaleGroupProducts.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleGroupProducts.RemoveRange(saleGroupProduct);
                        var saleProductMedia = db.SaleProductMedias.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleProductMedias.RemoveRange(saleProductMedia);
                        var saleProductDocument = db.SaleProductDocuments.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleProductDocuments.RemoveRange(saleProductDocument);
                        var saleProductAccessory = db.SaleProductAccessories.Where(s => s.SaleProductId == Id).ToList();
                        db.SaleProductAccessories.RemoveRange(saleProductAccessory);
                        db.SaleProducts.Remove(oldSaleProduct);

                        if (!string.IsNullOrEmpty(oldSaleProduct.SourceId))
                        {
                            if (oldSaleProduct.Type == 1)
                            {
                                var product = db.Products.FirstOrDefault(t => oldSaleProduct.SourceId.Equals(t.Id));
                                if (product != null)
                                {
                                    product.IsSendSale = false;
                                    product.SyncDate = null;
                                }
                            }
                            else if (oldSaleProduct.Type == 2)
                            {
                                var module = db.Modules.FirstOrDefault(t => oldSaleProduct.SourceId.Equals(t.Id));
                                if (module != null)
                                {
                                    module.IsSendSale = false;
                                    module.SyncDate = null;
                                }
                            }
                            else if (oldSaleProduct.Type == 3)
                            {
                                var material = db.Materials.FirstOrDefault(t => oldSaleProduct.SourceId.Equals(t.Id));
                                if (material != null)
                                {
                                    material.IsSendSale = false;
                                    material.SyncDate = null;
                                }
                            }
                            else if (oldSaleProduct.Type == 4)
                            {
                                var productTPA = db.ProductStandardTPAs.FirstOrDefault(t => oldSaleProduct.SourceId.Equals(t.Id));
                                if (productTPA != null)
                                {
                                    productTPA.IsSendSale = false;
                                    productTPA.SyncDate = null;
                                }
                            }
                        }

                        //var jsonBefore = AutoMapperConfig.Mapper.Map<SaleProductModel>(oldSaleProduct);
                        //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SaleProduct, oldSaleProduct.Id, oldSaleProduct.Model, jsonBefore);
                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new NTSLogException(null, ex);
                    }
                }
            }

        }

        public void UpdateStatus(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProduct = db.SaleProducts.FirstOrDefault(t => t.Id.Equals(id));
                    //var jsonBefore = AutoMapperConfig.Mapper.Map<SaleProductModel>(saleProduct);
                    if (saleProduct != null)
                    {
                        saleProduct.Status = !saleProduct.Status;
                        var saleProductType = db.SaleProductTypes.FirstOrDefault(o => o.Id.Equals(saleProduct.SaleProductTypeId));
                        //if (saleProductType != null && saleProduct.Status == false)
                        //{
                        //    var saleProductTypes = db.SaleProductTypes.FirstOrDefault(o => o.Id.Equals(saleProductType.ParentId));

                        //    saleProductType.OldParentId = saleProductType.ParentId;
                        //    saleProductType.ParentId = "SBP";
                        //    db.SaveChanges();

                        //}
                        //if (saleProductType != null && saleProduct.Status == true)
                        //{

                        //    saleProductType.ParentId = saleProductType.OldParentId;
                        //    db.SaveChanges();


                        //}
                    }
                    //var jsonAfter = AutoMapperConfig.Mapper.Map<SaleProductModel>(saleProduct);
                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_SaleProduct, saleProduct.Id, saleProduct.Model, jsonBefore, jsonAfter);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }

        }

        public object SearchGroupProduct(SearchSaleGroupModel modelSearch)
        {
            try
            {
                SearchResultModel<SaleGroupResultModel> searchResult = new SearchResultModel<SaleGroupResultModel>();

                var dataQuery = (from a in db.SaleGroups.AsNoTracking()
                                 where !modelSearch.ListIdSelect.Contains(a.Id)
                                 orderby a.Name
                                 select new SaleGroupResultModel()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Note = a.Note,
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.Note))
                {
                    dataQuery = dataQuery.Where(u => u.Note.ToUpper().Contains(modelSearch.Note.ToUpper()));
                }

                searchResult.TotalItem = dataQuery.Count();
                searchResult.ListResult = dataQuery.ToList();

                return searchResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// dánh sách nhóm kinh doanh theo Id sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<GroupProductInfoModel> GetGroupById(string Id)
        {
            var saleGroupProduct = (from a in db.SaleGroupProducts.AsNoTracking()
                                    where a.SaleProductId.Equals(Id)
                                    join b in db.SaleGroups.AsNoTracking()
                                    on a.SaleGroupId equals b.Id
                                    select new GroupProductInfoModel
                                    {
                                        Id = b.Id,
                                        Name = b.Name,
                                        Note = b.Note,
                                    }).ToList();
            return saleGroupProduct;
        }
        public List<EmployeeModel> getEmployee(string Id)
        {
            var saleGroupUser = (from a in db.SaleGroupUsers.AsNoTracking()
                                 where a.SaleGroupId.Equals(Id)
                                 join b in db.Users.AsNoTracking()
                                 on a.UserId equals b.Id
                                 join c in db.Employees.AsNoTracking()
                                 on b.EmployeeId equals c.Id
                                 join d in db.Departments.AsNoTracking()
                                 on c.DepartmentId equals d.Id
                                 select new EmployeeModel
                                 {
                                     Id = b.Id,
                                     Name = c.Name,
                                     Email = c.Email,
                                     Code = c.Code,
                                     DepartmentName = d.Name,
                                     ImagePath = c.ImagePath,
                                     PhoneNumber = c.PhoneNumber
                                 }).ToList();
            return saleGroupUser;
        }

        public void DefaultType(List<string> productIds, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SaleProduct saleProduct;
                    SaleProductType saleProductType;
                    List<ModuleGroup> moduelGroups;
                    List<ProductGroup> productGroups;
                    List<MaterialGroup> materialGroups;
                    List<ProductStandardTPAType> productStandardTPATypes;
                    Module module;
                    Product product;
                    Material material;
                    ProductStandardTPA productStandardTPA;
                    Dictionary<string, string> saleProductTypeNew = new Dictionary<string, string>();

                    string paretnId = string.Empty; ;
                    foreach (var id in productIds)
                    {
                        saleProduct = db.SaleProducts.FirstOrDefault(r => r.Id.Equals(id));

                        if (saleProduct != null)
                        {
                            if (saleProduct.Type == Constants.SaleProductDevice)
                            {
                                product = db.Products.AsNoTracking().FirstOrDefault(r => r.Id.Equals(saleProduct.SourceId));

                                if (product != null)
                                {
                                    productGroups = new List<ProductGroup>();

                                    GetProductGroup(productGroups, product.ProductGroupId);

                                    foreach (var moduleGroup in productGroups)
                                    {
                                        saleProductType = db.SaleProductTypes.AsNoTracking().FirstOrDefault(r => moduleGroup.Code.Equals(r.Code));

                                        if (saleProductType != null)
                                        {
                                            paretnId = saleProductType.Id;
                                        }
                                        else
                                        {
                                            if (!saleProductTypeNew.ContainsKey(moduleGroup.Code))
                                            {
                                                saleProductType = new SaleProductType
                                                {
                                                    Code = moduleGroup.Code,
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    Name = moduleGroup.Name,
                                                    Id = Guid.NewGuid().ToString(),
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now
                                                };

                                                if (!string.IsNullOrEmpty(paretnId))
                                                {
                                                    saleProductType.ParentId = paretnId;
                                                }

                                                db.SaleProductTypes.Add(saleProductType);

                                                paretnId = saleProductType.Id;

                                                saleProductTypeNew.Add(moduleGroup.Code, saleProductType.Id);
                                            }
                                            else
                                            {
                                                paretnId = saleProductTypeNew[moduleGroup.Code];
                                            }
                                        }
                                    }

                                }
                            }
                            else if (saleProduct.Type == Constants.SaleProductModule)
                            {
                                module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(saleProduct.SourceId));

                                if (module != null)
                                {
                                    moduelGroups = new List<ModuleGroup>();

                                    GetModuleGroup(moduelGroups, module.ModuleGroupId);

                                    foreach (var moduleGroup in moduelGroups)
                                    {
                                        saleProductType = db.SaleProductTypes.AsNoTracking().FirstOrDefault(r => moduleGroup.Code.Equals(r.Code));

                                        if (saleProductType != null)
                                        {
                                            paretnId = saleProductType.Id;
                                        }
                                        else
                                        {
                                            if (!saleProductTypeNew.ContainsKey(moduleGroup.Code))
                                            {
                                                saleProductType = new SaleProductType
                                                {
                                                    Code = moduleGroup.Code,
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    Name = moduleGroup.Name,
                                                    Id = Guid.NewGuid().ToString(),
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now
                                                };

                                                if (!string.IsNullOrEmpty(paretnId))
                                                {
                                                    saleProductType.ParentId = paretnId;
                                                }

                                                db.SaleProductTypes.Add(saleProductType);

                                                paretnId = saleProductType.Id;

                                                saleProductTypeNew.Add(moduleGroup.Code, saleProductType.Id);
                                            }
                                            else
                                            {
                                                paretnId = saleProductTypeNew[moduleGroup.Code];
                                            }
                                        }
                                    }
                                }
                            }
                            else if (saleProduct.Type == Constants.SaleProductMaterial)
                            {
                                material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Id.Equals(saleProduct.SourceId));

                                if (material != null)
                                {
                                    materialGroups = new List<MaterialGroup>();

                                    GetMaterialGroup(materialGroups, material.MaterialGroupId);

                                    foreach (var moduleGroup in materialGroups)
                                    {
                                        saleProductType = db.SaleProductTypes.AsNoTracking().FirstOrDefault(r => moduleGroup.Code.Equals(r.Code));

                                        if (saleProductType != null)
                                        {
                                            paretnId = saleProductType.Id;
                                        }
                                        else
                                        {
                                            if (!saleProductTypeNew.ContainsKey(moduleGroup.Code))
                                            {
                                                saleProductType = new SaleProductType
                                                {
                                                    Code = moduleGroup.Code,
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    Name = moduleGroup.Name,
                                                    Id = Guid.NewGuid().ToString(),
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now
                                                };

                                                if (!string.IsNullOrEmpty(paretnId))
                                                {
                                                    saleProductType.ParentId = paretnId;
                                                }

                                                db.SaleProductTypes.Add(saleProductType);

                                                paretnId = saleProductType.Id;
                                                saleProductTypeNew.Add(moduleGroup.Code, saleProductType.Id);
                                            }
                                            else
                                            {
                                                paretnId = saleProductTypeNew[moduleGroup.Code];
                                            }
                                        }
                                    }

                                }
                            }
                            else if (saleProduct.Type == Constants.SaleProductStandTPA)
                            {
                                productStandardTPA = db.ProductStandardTPAs.AsNoTracking().FirstOrDefault(r => r.Id.Equals(saleProduct.SourceId));

                                if (productStandardTPA != null)
                                {
                                    productStandardTPATypes = new List<ProductStandardTPAType>();

                                    GetProductStandardTPAType(productStandardTPATypes, productStandardTPA.ProductStandardTPATypeId);

                                    foreach (var moduleGroup in productStandardTPATypes)
                                    {
                                        saleProductType = db.SaleProductTypes.AsNoTracking().FirstOrDefault(r => moduleGroup.Name.Equals(r.Name));

                                        if (saleProductType != null)
                                        {
                                            paretnId = saleProductType.Id;
                                        }
                                        else
                                        {
                                            if (!saleProductTypeNew.ContainsKey(moduleGroup.Name))
                                            {
                                                saleProductType = new SaleProductType
                                                {
                                                    //Code = moduleGroup.Code,
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    Name = moduleGroup.Name,
                                                    Id = Guid.NewGuid().ToString(),
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now
                                                };

                                                if (!string.IsNullOrEmpty(paretnId))
                                                {
                                                    saleProductType.ParentId = paretnId;
                                                }

                                                db.SaleProductTypes.Add(saleProductType);

                                                paretnId = saleProductType.Id;
                                                saleProductTypeNew.Add(moduleGroup.Name, saleProductType.Id);
                                            }
                                            else
                                            {
                                                paretnId = saleProductTypeNew[moduleGroup.Name];
                                            }
                                        }
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(paretnId))
                            {
                                saleProduct.SaleProductTypeId = paretnId;
                            }
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);

                }
            }
        }

        private void GetModuleGroup(List<ModuleGroup> moduelGroups, string groupId)
        {
            var moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(groupId));

            if (moduleGroup != null)
            {
                if (!string.IsNullOrEmpty(moduleGroup.ParentId))
                {
                    GetModuleGroup(moduelGroups, moduleGroup.ParentId);
                }

                moduelGroups.Add(moduleGroup);
            }
        }

        private void GetProductGroup(List<ProductGroup> productGroups, string groupId)
        {
            var moduleGroup = db.ProductGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(groupId));

            if (moduleGroup != null)
            {
                if (!string.IsNullOrEmpty(moduleGroup.ParentId))
                {
                    GetProductGroup(productGroups, moduleGroup.ParentId);
                }

                productGroups.Add(moduleGroup);
            }
        }

        private void GetMaterialGroup(List<MaterialGroup> materialGroups, string groupId)
        {
            var moduleGroup = db.MaterialGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(groupId));

            if (moduleGroup != null)
            {
                if (!string.IsNullOrEmpty(moduleGroup.ParentId))
                {
                    GetMaterialGroup(materialGroups, moduleGroup.ParentId);
                }

                materialGroups.Add(moduleGroup);
            }
        }

        private void GetProductStandardTPAType(List<ProductStandardTPAType> productStandardTPATypes, string groupId)
        {
            var moduleGroup = db.ProductStandardTPATypes.AsNoTracking().FirstOrDefault(r => r.Id.Equals(groupId));

            if (moduleGroup != null)
            {
                if (!string.IsNullOrEmpty(moduleGroup.ParentId))
                {
                    GetProductStandardTPAType(productStandardTPATypes, moduleGroup.ParentId);
                }

                productStandardTPATypes.Add(moduleGroup);
            }
        }
    }
}