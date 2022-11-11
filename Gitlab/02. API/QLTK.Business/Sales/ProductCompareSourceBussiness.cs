using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.ProductCompareSource;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.Sale.ProductCompareSource;
using QLTK.Business.AutoMappers;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ProductCompareSource
{
    public class ProductCompareSourceBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm sản phẩm sai khác so với nguồn
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public SearchResultModel<ProductCompareSourceResultModel> SearchProductCompareSource(ProductCompareSourceSearchModel modelSearch)
        {
            SearchResultModel<ProductCompareSourceResultModel> searchResult = new SearchResultModel<ProductCompareSourceResultModel>();
            ProductCompareSourceResultModel sourceResultModel = new ProductCompareSourceResultModel();
            List<ProductCompareSourceResultModel> listSourceResult = new List<ProductCompareSourceResultModel>();

            // Lấy danh sách sản phẩm trong thư viện kinh doanh từ nguồn cập nhật sang
            var listSaleProduct = db.SaleProducts.AsNoTracking().Where(a => a.Type != 0 && a.IsSync == true).ToList();

            // Tìm kiếm theo tên hoặc code
            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                listSaleProduct = listSaleProduct.Where(a => a.VName.ToLower().Contains(modelSearch.NameCode.ToLower()) && a.Model.ToLower().Contains(modelSearch.NameCode.ToLower())).ToList();
            }

            // Tìm kiếm theo nguồn
            if (modelSearch.Source.HasValue)
            {
                listSaleProduct = listSaleProduct.Where(a => a.Type.Equals(modelSearch.Source)).ToList();
            }

            // danh sách thiết bị
            var listSaleProductDevice = (from a in db.Products.AsNoTracking()
                                         join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id into ab
                                         from ba in ab.DefaultIfEmpty()
                                         select new
                                         {
                                             Id = a.Id,
                                             Name = a.Name,
                                             Code = a.Code,
                                             ProductGroupCode = ba.Code,
                                             Pricing = a.Pricing,
                                             Description = a.Description,
                                             ParentId = ba.ParentId,
                                         }).ToList();

            // Danh sách module
            var listSaleProductModule = (from a in db.Modules.AsNoTracking()
                                         join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id into ab
                                         from ba in ab.DefaultIfEmpty()
                                         select new ModuleCompareSourceModel
                                         {
                                             Id = a.Id,
                                             Name = a.Name,
                                             Code = a.Code,
                                             ModuleGroupCode = ba.Code,
                                             Pricing = a.Pricing,
                                             Specification = a.Specification,
                                             ParentId = ba.ParentId,
                                             UpdateDate = a.UpdateDate
                                         }).ToList();

            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            foreach (var module in listSaleProductModule)
            {
                module.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(module.Id, 0);
            }

            // Danh sách vật tư
            var listSaleProductMaterial = (from a in db.Materials.AsNoTracking()
                                           join b in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals b.Id into ab
                                           from ba in ab.DefaultIfEmpty()
                                           select new
                                           {
                                               Id = a.Id,
                                               Name = a.Name,
                                               Code = a.Code,
                                               MaterialGroupCode = ba.Code,
                                               Pricing = a.Pricing,
                                               ManufactureId = a.ManufactureId,
                                               Description = a.Note,
                                               ParentId = ba.ParentId,
                                               UpdateDate = a.UpdateDate,
                                               LastBuyDate = a.LastBuyDate,
                                               PriceHistory = a.PriceHistory,
                                               InputPriceDate = a.InputPriceDate,
                                           }).ToList();

            //// Danh sách vật tư tiêu chuẩn
            var listSaleProductStandTPA = db.ProductStandardTPAs.AsNoTracking().ToList();

            //// Danh sách nhóm thiết bị
            //var productGroups = db.ProductGroups.AsNoTracking().ToList();

            //// Danh sách nhóm module
            //var moduleGroups = db.ModuleGroups.AsNoTracking().ToList();

            //// Danh sách nhóm vật tư
            //var materialGroups = db.MaterialGroups.AsNoTracking().ToList();


            foreach (var item in listSaleProduct)
            {
                //thiết bị
                if (item.Type == Constants.SaleProductDevice)
                {
                    var deviceInfo = (from a in listSaleProductDevice
                                      where a.Id.Equals(item.SourceId)
                                      select new
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Code = a.Code,
                                          ProductGroupCode = a.ProductGroupCode,
                                          Pricing = a.Pricing,
                                          Description = a.Description,
                                          ParentId = a.ParentId,
                                      }).FirstOrDefault();


                    if (deviceInfo != null)
                    {
                        //var childProductGroupCode = productGroups.Where(a => a.Id.Equals(deviceInfo.ParentId)).Select(a => a.Code).FirstOrDefault();

                        if (item.Model != deviceInfo.Code || item.VName != deviceInfo.Name || item.GroupCode != deviceInfo.ProductGroupCode || item.MaterialPrice != deviceInfo.Pricing || item.Specifications != deviceInfo.Description)
                        {
                            sourceResultModel = new ProductCompareSourceResultModel()
                            {
                                IdSaleProduct = item.Id,
                                Code = item.Model,
                                IdSource = deviceInfo.Id,
                                Name = item.VName,
                                Source = Constants.SaleProductDevice,
                            };
                            listSourceResult.Add(sourceResultModel);
                        }

                    }
                }

                // Module
                if (item.Type == Constants.SaleProductModule)
                {
                    var moduleInfo = (from a in listSaleProductModule
                                      where a.Id.Equals(item.SourceId)
                                      select new
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Code = a.Code,
                                          ModuleGroupCode = a.ModuleGroupCode,
                                          Pricing = a.Pricing,
                                          Specification = a.Specification,
                                          ParentId = a.ParentId,
                                          UpdateDate = a.UpdateDate,

                                      }).FirstOrDefault();



                    if (moduleInfo != null)
                    {
                        //var childProductGroupCode = moduleGroups.Where(a => a.Id.Equals(moduleInfo.ParentId)).Select(a => a.Code).FirstOrDefault();

                        if (item.Model != moduleInfo.Code || item.VName != moduleInfo.Name || item.SpecificationDate != moduleInfo.UpdateDate
                            || item.GroupCode != moduleInfo.ModuleGroupCode || item.MaterialPrice != moduleInfo.Pricing
                            || item.Specifications != moduleInfo.Specification)
                        {
                            sourceResultModel = new ProductCompareSourceResultModel()
                            {
                                IdSaleProduct = item.Id,
                                Code = item.Model,
                                IdSource = moduleInfo.Id,
                                Name = item.VName,
                                Source = Constants.SaleProductModule,
                            };
                            listSourceResult.Add(sourceResultModel);
                        }

                    }
                }

                // Vật tư
                if (item.Type == Constants.SaleProductMaterial)
                {
                    MaterialBusiness materialBusiness = new MaterialBusiness();
                    var day = materialBusiness.GetConfigMaterialLastByDate();


                    var materialInfo = (from a in listSaleProductMaterial
                                        where a.Id.Equals(item.SourceId)
                                        select new
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Code = a.Code,
                                            MaterialGroupCode = a.MaterialGroupCode,
                                            Pricing = a.Pricing,
                                            ManufactureId = a.ManufactureId,
                                            Specifications = a.Description,
                                            ParentId = a.ParentId,
                                            UpdateDate = a.UpdateDate,
                                            LastBuyDate = a.LastBuyDate,
                                            PriceHistory = a.PriceHistory,
                                            InputPriceDate = a.InputPriceDate,
                                        }).FirstOrDefault();


                    if (materialInfo != null)
                    {
                        //var childProductGroupCode = materialGroups.Where(a => a.Id.Equals(materialInfo.ParentId)).Select(a => a.Code).FirstOrDefault();
                        var price = materialInfo.Pricing;
                        if (materialInfo.LastBuyDate.HasValue)
                        {
                            TimeSpan timeSpan = DateTime.Now.Subtract(materialInfo.LastBuyDate.Value);

                            if (timeSpan.Days <= day)
                            {
                                price = materialInfo.PriceHistory;
                            }
                            else if (!materialInfo.InputPriceDate.HasValue || materialInfo.InputPriceDate.Value.Date < materialInfo.LastBuyDate.Value.Date)
                            {
                                price = 0;
                            }
                        }
                        if (item.Model != materialInfo.Code || item.VName != materialInfo.Name || item.SpecificationDate != materialInfo.UpdateDate || item.GroupCode != materialInfo.MaterialGroupCode || item.MaterialPrice != price || item.ManufactureId != materialInfo.ManufactureId || item.Specifications != materialInfo.Specifications)
                        {
                            sourceResultModel = new ProductCompareSourceResultModel()
                            {
                                IdSaleProduct = item.Id,
                                Code = item.Model,
                                IdSource = materialInfo.Id,
                                Name = item.VName,
                                Source = Constants.SaleProductMaterial,
                            };
                            listSourceResult.Add(sourceResultModel);
                        }

                    }
                }

                // Thiết bị tiêu chuẩn
                if (item.Type == Constants.SaleProductStandTPA)
                {
                    var productStandardInfo = listSaleProductStandTPA.Where(a => a.Id.Equals(item.SourceId)).FirstOrDefault();

                    if (productStandardInfo != null)
                    {
                        var check = false;
                        if (!string.IsNullOrEmpty(item.VName) && !string.IsNullOrEmpty(productStandardInfo.VietNamName) && !item.VName.Equals(productStandardInfo.VietNamName))
                        {
                            check = true;
                        }

                        if (!string.IsNullOrEmpty(item.EName) && !string.IsNullOrEmpty(productStandardInfo.EnglishName) && !item.EName.Equals(productStandardInfo.EnglishName))
                        {
                            check = true;
                        }

                        if (!string.IsNullOrEmpty(item.Model) && !string.IsNullOrEmpty(productStandardInfo.Model) && !item.Model.Equals(productStandardInfo.Model))
                        {
                            check = true;
                        }

                        if (!string.IsNullOrEmpty(item.Model) && !string.IsNullOrEmpty(productStandardInfo.Model) && !item.Model.Equals(productStandardInfo.Model))
                        {
                            check = true;
                        }


                        if (productStandardInfo.PriceEXW_TPA.HasValue && item.EXWTPAPrice != productStandardInfo.PriceEXW_TPA)
                        {
                            check = true;
                        }

                        if (productStandardInfo.VAT.HasValue && item.VAT != productStandardInfo.VAT.Value)
                        {
                            check = true;
                        }

                        if (productStandardInfo.VAT.HasValue && item.EXWTPADate.HasValue && item.EXWTPADate.Value != productStandardInfo.UpdateDatePrice_TPA.Value)
                        {
                            check = true;
                        }

                        if (!string.IsNullOrEmpty(item.CountryName) && !string.IsNullOrEmpty(productStandardInfo.Country_NCC_SX) && !item.CountryName.Equals(productStandardInfo.Country_NCC_SX))
                        {
                            check = true;
                        }

                        if (!string.IsNullOrEmpty(item.DeliveryTime) && !string.IsNullOrEmpty(productStandardInfo.DeliveryTime) && !item.DeliveryTime.Equals(productStandardInfo.DeliveryTime))
                        {
                            check = true;
                        }

                        if (check)
                        {
                            sourceResultModel = new ProductCompareSourceResultModel()
                            {
                                IdSaleProduct = item.Id,
                                Code = item.Model,
                                IdSource = productStandardInfo.Id,
                                Name = item.VName,
                                Source = Constants.SaleProductStandTPA,
                            };
                            listSourceResult.Add(sourceResultModel);
                        }
                    }
                }
            }



            searchResult.TotalItem = listSourceResult.Count();
            searchResult.ListResult = listSourceResult;
            return searchResult;
        }

        /// <summary>
        /// Lấy thông tin sản phẩm sai khác trong kinh doanh
        /// </summary>
        /// <param name="id">Id sản phẩm trong thư viện sản phẩm</param>
        /// <returns></returns>
        public object GetProductCompareSourceById(string id)
        {
            ProductCompareSourcModel productCompare = new ProductCompareSourcModel();
            var saleProductInfo = (from a in db.SaleProducts.AsNoTracking()
                                   where a.Id.Equals(id)
                                   select new
                                   {
                                       Type = a.Type,
                                       Model = a.Model,
                                       EName = a.EName,
                                       VName = a.VName,
                                       VAT = a.VAT,
                                       Specifications = a.Specifications,
                                       GroupCode = a.GroupCode,
                                       MaterialPrice = a.MaterialPrice,
                                       //ProductStandTPATypeName = ac.Name,
                                       EXWTPAPrice = a.EXWTPAPrice,
                                       EXWTPADate = a.EXWTPADate,
                                       DeliveryTime = a.DeliveryTime,
                                       CountryName = a.CountryName,
                                       ChildGroupCode = a.ChildGroupCode,
                                       SpecificationDate = a.SpecificationDate,
                                       SourceId = a.SourceId,

                                   }).FirstOrDefault();

            if (saleProductInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProducts);
            }

            // Thiết bị
            if (saleProductInfo.Type == Constants.SaleProductDevice)
            {
                productCompare = (from a in db.Products.AsNoTracking()
                                  where a.Id.Equals(saleProductInfo.SourceId)
                                  join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id into ab
                                  from ba in ab.DefaultIfEmpty()
                                  select new ProductCompareSourcModel()
                                  {
                                      VName = a.Name,
                                      Model = a.Code,
                                      MaterialPrice = a.Pricing,
                                      Specifications = a.Description,
                                      GroupCode = ba.Code,
                                      ChildGroupId = ba.ParentId,
                                      CountryName = "Việt Nam",

                                  }).FirstOrDefault();

                productCompare.ChildGroupCode = db.ProductGroups.AsNoTracking().Where(a => a.Id.Equals(productCompare.ChildGroupId)).Select(a => a.Code).FirstOrDefault();

            }

            // Module
            if (saleProductInfo.Type == Constants.SaleProductModule)
            {
                productCompare = (from a in db.Modules.AsNoTracking()
                                  where a.Id.Equals(saleProductInfo.SourceId)
                                  join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id into ab
                                  from ba in ab.DefaultIfEmpty()
                                  select new ProductCompareSourcModel()
                                  {
                                      VName = a.Name,
                                      Model = a.Code,
                                      MaterialPrice = a.Pricing,
                                      Specifications = a.Description,
                                      GroupCode = ba.Code,
                                      ChildGroupId = ba.ParentId,
                                      CountryName = "Việt Nam",
                                      SpecificationDate = a.UpdateDate,
                                  }).FirstOrDefault();
                productCompare.ChildGroupCode = db.ModuleGroups.AsNoTracking().Where(a => a.Id.Equals(productCompare.ChildGroupId)).Select(a => a.Code).FirstOrDefault();

            }

            // Vật tư
            if (saleProductInfo.Type == Constants.SaleProductMaterial)
            {
                MaterialBusiness materialBusiness = new MaterialBusiness();
                var day = materialBusiness.GetConfigMaterialLastByDate();

                productCompare = (from a in db.Materials.AsNoTracking()
                                  where a.Id.Equals(saleProductInfo.SourceId)
                                  join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id into ab
                                  from ba in ab.DefaultIfEmpty()
                                  join d in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals d.Id into ad
                                  from da in ad.DefaultIfEmpty()
                                  select new ProductCompareSourcModel()
                                  {
                                      VName = a.Name,
                                      Model = a.Code,
                                      MaterialPrice = a.Pricing,
                                      Specifications = a.Note,
                                      ManufactureName = ba.Name,
                                      GroupCode = da.Code,
                                      ChildGroupId = da.ParentId,
                                      SpecificationDate = a.UpdateDate,
                                      LastBuyDate = a.LastBuyDate,
                                      PriceHistory = a.PriceHistory,
                                      InputPriceDate = a.InputPriceDate,

                                      //MaterialParameterValue = db.MaterialParameterValues.AsNoTracking().Where(a => a.MaterialParameterId.Equals(c.Id)).Select(b => b.Value).ToList(),
                                  }).FirstOrDefault();

                productCompare.ChildGroupCode = db.MaterialGroups.AsNoTracking().Where(a => a.Id.Equals(productCompare.ChildGroupId)).Select(a => a.Code).FirstOrDefault();

                if (productCompare.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(productCompare.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        productCompare.MaterialPrice = productCompare.PriceHistory;
                    }
                    else if (!productCompare.InputPriceDate.HasValue || productCompare.InputPriceDate.Value.Date < productCompare.LastBuyDate.Value.Date)
                    {
                        productCompare.MaterialPrice = 0;
                    }
                }

            }

            // Thiết bị tiêu chuẩn
            if (saleProductInfo.Type == Constants.SaleProductStandTPA)
            {
                productCompare = (from a in db.ProductStandardTPAs.AsNoTracking()
                                  where a.Id.Equals(saleProductInfo.SourceId)
                                  join b in db.ProductStandardTPATypes.AsNoTracking() on a.ProductStandardTPATypeId equals b.Id into ab
                                  from ba in ab.DefaultIfEmpty()
                                  select new ProductCompareSourcModel()
                                  {
                                      VName = a.VietNamName,
                                      EName = a.EnglishName,
                                      Model = a.Model,
                                      ProductStandTPATypeName = ba.Name,
                                      EXWTPAPrice = a.PriceEXW_TPA,
                                      EXWTPADate = a.UpdateDatePrice_TPA,
                                      DeliveryTime = a.DeliveryTime,
                                      VAT = a.VAT,
                                      CountryName = a.Country_NCC_SX,
                                      Specifications = a.Specifications,

                                  }).FirstOrDefault();

                if (!productCompare.EXWTPAPrice.HasValue)
                {
                    productCompare.EXWTPAPrice = 0;
                }
            }

            // trả về giá trị

            return new
            {
                SaleProductInfo = saleProductInfo,
                ProductCompare = productCompare,
            };
        }

        public void UpdateListSaleProduct(UpdateSaleProductModel model, string userId)
        {
            foreach (var item in model.ListIdSaleProduct)
            {

                UpdateSaleProduct(item, userId);

            }
            db.SaveChanges();


        }

        public void UpdateSaleProduct(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProductInfo = db.SaleProducts.Where(a => a.Id.Equals(id)).FirstOrDefault();

                    if (saleProductInfo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProducts);
                    }

                    //var jsonApter = AutoMapperConfig.Mapper.Map<NTS.Model.Sale.SaleProduct.SaleProductModel>(saleProductInfo);

                    // Nguồn thiết bị
                    if (saleProductInfo.Type == Constants.SaleProductDevice)
                    {
                        var deviceInfo = (from a in db.Products.AsNoTracking()
                                          where a.Id.Equals(saleProductInfo.SourceId)
                                          join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id
                                          select new
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              Code = a.Code,
                                              ProductGroupCode = b.Code,
                                              Pricing = a.Pricing,
                                              Description = a.Description,
                                              ParentId = b.ParentId,
                                          }).FirstOrDefault();


                        if (deviceInfo != null)
                        {
                            var childProductGroupCode = db.ProductGroups.AsNoTracking().Where(a => a.Id.Equals(deviceInfo.ParentId)).Select(a => a.Code).FirstOrDefault();

                            saleProductInfo.VName = deviceInfo.Name;
                            saleProductInfo.Model = deviceInfo.Code;
                            saleProductInfo.GroupCode = deviceInfo.ProductGroupCode;
                            saleProductInfo.ChildGroupCode = childProductGroupCode;
                            saleProductInfo.MaterialPrice = deviceInfo.Pricing;
                            saleProductInfo.Specifications = deviceInfo.Description;
                            saleProductInfo.CountryName = "Việt Nam";
                        }
                    }

                    // Nguồn module
                    if (saleProductInfo.Type == Constants.SaleProductModule)
                    {
                        var moduleInfo = (from a in db.Modules.AsNoTracking()
                                          where a.Id.Equals(saleProductInfo.SourceId)
                                          join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                                          select new
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              Code = a.Code,
                                              ModuleGroupCode = b.Code,
                                              Pricing = a.Pricing,
                                              Description = a.Description,
                                              ParentId = b.ParentId,
                                              UpdateDate = a.UpdateDate,
                                          }).FirstOrDefault();


                        if (moduleInfo != null)
                        {
                            var childProductGroupCode = db.ModuleGroups.AsNoTracking().Where(a => a.Id.Equals(moduleInfo.ParentId)).Select(a => a.Code).FirstOrDefault();

                            saleProductInfo.VName = moduleInfo.Name;
                            saleProductInfo.Model = moduleInfo.Code;
                            saleProductInfo.GroupCode = moduleInfo.ModuleGroupCode;
                            saleProductInfo.ChildGroupCode = childProductGroupCode;
                            saleProductInfo.MaterialPrice = moduleInfo.Pricing;
                            saleProductInfo.Specifications = moduleInfo.Description;
                            saleProductInfo.CountryName = "Việt Nam";
                            saleProductInfo.SpecificationDate = moduleInfo.UpdateDate;
                        }
                    }

                    // Nguồn vật tư
                    if (saleProductInfo.Type == Constants.SaleProductMaterial)
                    {
                        List<SaleProductMedia> listSaleProductMedia = new List<SaleProductMedia>();
                        MaterialBusiness materialBusiness = new MaterialBusiness();
                        var day = materialBusiness.GetConfigMaterialLastByDate();
                        var material = db.Materials.FirstOrDefault(i => i.Id.Equals(saleProductInfo.SourceId));
                        if (material != null)
                        {
                            material.SyncDate = DateTime.Now;
                            if (material.LastBuyDate.HasValue)
                            {
                                TimeSpan timeSpan = DateTime.Now.Subtract(material.LastBuyDate.Value);

                                if (timeSpan.Days <= day)
                                {
                                    material.Pricing = material.PriceHistory;
                                }
                                else if (!material.InputPriceDate.HasValue || material.InputPriceDate.Value.Date < material.LastBuyDate.Value.Date)
                                {
                                    material.Pricing = 0;
                                }
                            }

                            var groupCode = db.MaterialGroups.FirstOrDefault(i => i.Id.Equals(material.MaterialGroupId))?.Code;

                            var check = db.SaleProducts.FirstOrDefault(i => i.Type == Constants.SaleProductMaterial && i.SourceId.Equals(material.Id));

                            if (check != null)
                            {
                                check.EName = string.Empty;
                                check.VName = material.Name;
                                check.Model = material.Code;
                                check.GroupCode = groupCode;
                                check.ManufactureId = material.ManufactureId;
                                //check.CountryName = Constants.CountryName;
                                check.Specifications = material.Note;
                                check.SpecificationDate = material.UpdateDate;
                                check.MaterialPrice = material.Pricing;
                                check.DeliveryTime = material.DeliveryDays.ToString();
                                check.SourceId = material.Id;
                                check.IsSync = true;
                                check.Status = true;
                                check.Type = Constants.SaleProductMaterial;
                                check.UpdateBy = userId;
                                check.UpdateDate = DateTime.Now;

                                var syncSaleProduct = new SyncSaleProduct()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Type = Constants.SyncSaleProduct_Type_SaleProductMaterial,
                                    Date = DateTime.Now
                                };

                                db.SyncSaleProducts.Add(syncSaleProduct);
                            }
                        }
                    }

                    // Nguồn vật tư tiêu chuẩn
                    if (saleProductInfo.Type == Constants.SaleProductStandTPA)
                    {
                        var productStandardInfo = db.ProductStandardTPAs.FirstOrDefault(i => i.Id.Equals(saleProductInfo.SourceId));
                        var groupCode = db.ProductStandardTPATypes.FirstOrDefault(i => i.Id.Equals(productStandardInfo.ProductStandardTPATypeId))?.Name;
                        var manufacureId = db.Manufactures.FirstOrDefault(i => i.Name.ToUpper().Equals(productStandardInfo.Manufacture_NCC_SX.ToUpper()))?.Id;
                        if (productStandardInfo != null)
                        {
                            saleProductInfo.Model = productStandardInfo.Model;
                            saleProductInfo.EName = productStandardInfo.EnglishName;
                            saleProductInfo.VName = productStandardInfo.VietNamName;
                            saleProductInfo.GroupCode = groupCode;
                            saleProductInfo.ManufactureId = manufacureId;
                            saleProductInfo.CountryName = productStandardInfo.Country_NCC_SX;
                            saleProductInfo.Specifications = productStandardInfo.Specifications;
                            saleProductInfo.CustomerSpecifications = productStandardInfo.Specifications;
                            if (productStandardInfo.VAT.HasValue)
                            {
                                saleProductInfo.VAT = productStandardInfo.VAT.Value;
                            }
                            if (productStandardInfo.PriceEXW_TPA.HasValue)
                            {
                                saleProductInfo.EXWTPAPrice = productStandardInfo.PriceEXW_TPA.Value;
                            }
                            //check.MaterialPrice = productStandardTPA.PriceProduct_TPA;
                            saleProductInfo.EXWTPADate = productStandardInfo.UpdateDatePrice_TPA;
                            //check.PublicPrice;
                            saleProductInfo.DeliveryTime = productStandardInfo.DeliveryTime;
                            saleProductInfo.SourceId = productStandardInfo.Id;
                            saleProductInfo.Type = Constants.SaleProductStandTPA;
                            saleProductInfo.IsSync = true;
                            saleProductInfo.Status = true;
                            saleProductInfo.UpdateBy = userId;
                            saleProductInfo.UpdateDate = DateTime.Now;

                            var syncSaleProduct = new SyncSaleProduct()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = Constants.SyncSaleProduct_Type_SaleProductStandardTPA,
                                Date = DateTime.Now
                            };
                            db.SyncSaleProducts.Add(syncSaleProduct);
                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<NTS.Model.Sale.SaleProduct.SaleProductModel>(saleProductInfo);
                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Compare, saleProductInfo.Id, saleProductInfo.Model, jsonBefor, jsonApter);

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
}
