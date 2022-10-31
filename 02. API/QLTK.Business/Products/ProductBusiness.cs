using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DesignDocuments;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.Practice;
using NTS.Model.PracticeFile;
using NTS.Model.PracticeSupMaterial;
using NTS.Model.Product;
using NTS.Model.ProductAccessories;
using NTS.Model.ProductCatalog;
using NTS.Model.ProductDesignDocuments;
using NTS.Model.ProductDocument;
using NTS.Model.ProductDocumentAttach;
using NTS.Model.ProductModuleUpdate;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Products
{
    public class ProductBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private MaterialBusiness materialBusiness = new MaterialBusiness();

        /// <summary>
        /// Tìm kiếm module
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ProductResultModel> SearchProduct(ProductSearchModel modelSearch, bool isExport)
        {
            SearchResultModel<ProductResultModel> searchResult = new SearchResultModel<ProductResultModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.Products.AsNoTracking()
                             join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.SBUs.AsNoTracking() on ca.SBUId equals d.Id into cad
                             from cnd in cad.DefaultIfEmpty()
                             orderby a.Code
                             select new ProductResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ProductGroupId = a.ProductGroupId,
                                 ProductGroupName = b.Name,
                                 ProductGroupCode = b.Code,
                                 Pricing = a.Price,
                                 IsManualExist = a.IsManualExist,
                                 IsQuoteExist = a.IsQuoteExist,
                                 IsPracticeExist = a.IsPracticeExist,
                                 IsLayoutExist = a.IsLayoutExist,
                                 IsMaterialExist = a.IsMaterialExist,
                                 IsManualMaintenance = a.IsManualMaintenance,
                                 IsCatalog = a.IsCatalog,
                                 ProcedureTime = a.ProcedureTime,
                                 CurentVersion = a.CurentVersion,
                                 Status = a.Status,
                                 IsEnoughtSearch = ((!a.IsManualExist) || (!a.IsQuoteExist) || (!a.IsPracticeExist) || (!a.IsLayoutExist) || (!a.IsMaterialExist)),
                                 DepartmentId = ca != null ? ca.Id : string.Empty,
                                 DepartmentName = ca != null ? ca.Name : string.Empty,
                                 SBUId = cnd != null ? cnd.Id : string.Empty,
                                 SBUName = cnd != null ? cnd.Name : string.Empty,
                                 IsTestResult = a.IsTestResult,
                                 IsSendSale = a.IsSendSale,
                                 SyncDate = a.SyncDate
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.TypeGuideMaintenance))
            {
                if (modelSearch.TypeGuideMaintenance.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsManualMaintenance == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsManualMaintenance == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeDMBTH))
            {
                if (modelSearch.TypeDMBTH.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsManualExist == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsManualExist == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeGuidePractice))
            {
                if (modelSearch.TypeGuidePractice.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsPracticeExist == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsPracticeExist == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeCatalogs))
            {
                if (modelSearch.TypeCatalogs.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsCatalog == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsCatalog == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(a => a.SBUId.Equals(modelSearch.SBUId));
            }
            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(a => a.DepartmentId.Equals(modelSearch.DepartmentId));
            }
            //// tìm kiếm theo tên
            //if (!string.IsNullOrEmpty(modelSearch.Name))
            //{
            //    dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.ProductGroupId))
            {
                //dataQuery = dataQuery.Where(r => r.ProductGroupId.ToUpper().Contains(modelSearch.ProductGroupId.ToUpper()));
                var productGroup = db.ProductGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.ProductGroupId));
                if (productGroup != null)
                {
                    listParentId.Add(productGroup.Id);
                }
                listParentId.AddRange(GetListParent(modelSearch.ProductGroupId));
                var listProductGroup = listParentId.AsQueryable();
                dataQuery = (from a in dataQuery
                             join b in listProductGroup.AsQueryable() on a.ProductGroupId equals b
                             orderby a.Code
                             select a).AsQueryable();
            }

            // tìm kiếm theo mã 
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || r.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            // tìm kiếm theo tình trạng sử dụng
            if (!string.IsNullOrEmpty(modelSearch.Status))
            {
                dataQuery = dataQuery.Where(r => r.Status.Contains(modelSearch.Status));
            }

            //if (modelSearch.IsSendSale != null)
            //{
            //    dataQuery = dataQuery.Where(r => r.IsSendSale.Equals(modelSearch.IsSendSale));
            //}

            if (modelSearch.IsEnought != null)
            {
                if (modelSearch.IsEnought == 1)
                {
                    dataQuery = dataQuery.Where(a => a.IsManualExist && a.IsQuoteExist && a.IsPracticeExist && a.IsLayoutExist && a.IsMaterialExist);
                }
                if (modelSearch.IsEnought == 0)
                {
                    dataQuery = dataQuery.Where(a => !a.IsManualExist && !a.IsQuoteExist && !a.IsPracticeExist && !a.IsLayoutExist && !a.IsMaterialExist);
                }
                if (modelSearch.IsEnought == 2)
                {
                    dataQuery = dataQuery.Where(a => !a.IsManualExist);
                }
                if (modelSearch.IsEnought == 3)
                {
                    dataQuery = dataQuery.Where(a => !a.IsQuoteExist);
                }
                if (modelSearch.IsEnought == 4)
                {
                    dataQuery = dataQuery.Where(a => !a.IsPracticeExist);
                }
                if (modelSearch.IsEnought == 5)
                {
                    dataQuery = dataQuery.Where(a => !a.IsLayoutExist);
                }
                if (modelSearch.IsEnought == 6)
                {
                    dataQuery = dataQuery.Where(a => !a.IsMaterialExist);
                }
            }

            searchResult.Status2 = dataQuery.Where(u => u.IsEnoughtSearch.Value).Count();
            searchResult.Status1 = dataQuery.Where(u => !u.IsEnoughtSearch.Value).Count();

            searchResult.TotalItem = dataQuery.Count();
            if (isExport)
            {
                searchResult.ListResult = dataQuery.ToList();
            }
            else
            {
                searchResult.Date = dataQuery.Max(i => i.SyncDate);
                searchResult.ListResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }

            //decimal moduleAmount = 0;
            //decimal price = 0;

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();

            foreach (var item in searchResult.ListResult)
            {
                var errorCount = (from a in db.Errors.AsNoTracking()
                                  where a.Type == Constants.Error_Type_Error && a.Status > Constants.Problem_Status_Awaiting_Confirm && a.ObjectType == 2 && a.ObjectId.Equals(item.Id)
                                  orderby a.Code
                                  select new
                                  {
                                      Id = a.Id,
                                  }).Count();

                item.ErrorCount = errorCount;

                //var modules = (from b in db.ProductModules.AsNoTracking()
                //               where b.ProductId.Equals(item.Id)
                //               select new
                //               {
                //                   b.ModuleId,
                //                   b.Quantity
                //               }).ToList();

                //moduleAmount = 0;
                //foreach (var module in modules)
                //{
                //    moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                //}

                //var accessories = (from a in db.ProductAccessories.AsNoTracking()
                //                   join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                //                   where a.ProductId.Equals(item.Id)
                //                   select new
                //                   {
                //                       a.Quantity,
                //                       c.Pricing,
                //                       c.LastBuyDate,
                //                       c.InputPriceDate,
                //                       c.PriceHistory
                //                   }).ToList();

                //foreach (var acces in accessories)
                //{
                //    price = acces.Pricing;
                //    if (acces.LastBuyDate.HasValue)
                //    {
                //        TimeSpan timeSpan = DateTime.Now.Subtract(acces.LastBuyDate.Value);

                //        if (timeSpan.Days <= day)
                //        {
                //            price = acces.PriceHistory;
                //        }
                //        else if (!acces.InputPriceDate.HasValue || acces.InputPriceDate.Value.Date < acces.LastBuyDate.Value.Date)
                //        {
                //            price = 0;
                //        }
                //    }

                //    moduleAmount += acces.Quantity * price;
                //}

                //item.Pricing = moduleAmount;
            }

            return searchResult;
        }

        /// <summary>
        /// Thêm mới thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        public void AddProduct(ProductModel productModel)
        {
            CheckInputAdd(productModel);
            productModel.Code = Util.RemoveSpecialCharacter(productModel.Code);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    NTS.Model.Repositories.Product newProduct = new NTS.Model.Repositories.Product()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductGroupId = productModel.ProductGroupId,
                        Code = productModel.Code,
                        Name = productModel.Name,
                        ProcedureTime = productModel.ProcedureTime,
                        CurentVersion = productModel.CurentVersion,
                        Description = productModel.Description,
                        Content = productModel.Content,
                        Status = productModel.Status,
                        DepartmentId = productModel.DepartmentId,
                        IsManualMaintenance = productModel.IsManualMaintenance,
                        CreateBy = productModel.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = productModel.CreateBy
                    };

                    if (productModel.ListFileCatalog.Count > 0)
                    {
                        newProduct.IsCatalog = true;
                    }

                    db.Products.Add(newProduct);

                    if (productModel.ListImage != null)
                    {
                        foreach (var item in productModel.ListImage)
                        {
                            ProductImage newImage = new ProductImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = newProduct.Id,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                ThumbnailPath = item.ThumbnailPath,
                                Note = item.Note,
                                CreateBy = productModel.CreateBy,
                                CreateDate = DateTime.Now,
                            };
                            db.ProductImages.Add(newImage);

                        }
                    }

                    //if (productModel.ListFielDocument.Count > 0)
                    //{
                    //    foreach (var item in productModel.ListFielDocument)
                    //    {
                    //        if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.Path))
                    //        {
                    //            ProductDocument productDocument = new ProductDocument()
                    //            {
                    //                Id = Guid.NewGuid().ToString(),
                    //                ProductId = newProduct.Id,
                    //                Path = item.Path,
                    //                FileName = item.FileName,
                    //                CreateDate = DateTime.Now,
                    //                FileSize = item.FileSize,
                    //                Note = item.Note,
                    //                FileType = item.FileType,
                    //            };
                    //            db.ProductDocuments.Add(productDocument);

                    //        }
                    //    }

                    //}

                    //if (productModel.ListFileCatalog.Count > 0)
                    //{
                    //    foreach (var item in productModel.ListFileCatalog)
                    //    {
                    //        if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FilePath))
                    //        {
                    //            ProductCatalog productCatalog = new ProductCatalog()
                    //            {
                    //                Id = Guid.NewGuid().ToString(),
                    //                ProductId = newProduct.Id,
                    //                FilePath = item.FilePath,
                    //                FileName = item.FileName,
                    //                CreateDate = DateTime.Now,
                    //                FileSize = item.FileSize,
                    //                Note = item.Note,
                    //                CreateBy = productModel.CreateBy,
                    //            };
                    //            db.ProductCatalogs.Add(productCatalog);

                    //        }
                    //    }

                    //}

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(productModel, ex);
                }
            }
            try
            {
                //luu Log lich su
                string decription = String.Empty;
                decription = "Thêm thiết bị có tên là: " + productModel.Name;
                LogBusiness.SaveLogEvent(db, productModel.CreateBy, decription);
            }
            catch (Exception) { }
        }

        public void UpdateNewPrice()
        {
            SearchResultModel<ProductResultModel> searchResult = new SearchResultModel<ProductResultModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = db.Products.ToList();

            searchResult.TotalItem = dataQuery.Count();

            decimal moduleAmount = 0;
            decimal price = 0;

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();

            foreach (var item in dataQuery)
            {
                var modules = (from b in db.ProductModules.AsNoTracking()
                               where b.ProductId.Equals(item.Id)
                               select new
                               {
                                   b.ModuleId,
                                   b.Quantity
                               }).ToList();

                moduleAmount = 0;
                foreach (var module in modules)
                {
                    moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                }

                var accessories = (from a in db.ProductAccessories.AsNoTracking()
                                   join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                   where a.ProductId.Equals(item.Id)
                                   select new
                                   {
                                       a.Quantity,
                                       c.Pricing,
                                       c.LastBuyDate,
                                       c.InputPriceDate,
                                       c.PriceHistory
                                   }).ToList();

                foreach (var acces in accessories)
                {
                    price = acces.Pricing;
                    if (acces.LastBuyDate.HasValue)
                    {
                        TimeSpan timeSpan = DateTime.Now.Subtract(acces.LastBuyDate.Value);

                        if (timeSpan.Days <= day)
                        {
                            price = acces.PriceHistory;
                        }
                        else if (!acces.InputPriceDate.HasValue || acces.InputPriceDate.Value.Date < acces.LastBuyDate.Value.Date)
                        {
                            price = 0;
                        }
                    }

                    moduleAmount += acces.Quantity * price;
                }

                item.Price = Math.Round(moduleAmount);
            }
            db.SaveChanges();
        }

        public List<ModuleInProductModel> GetModuleByProduct(ProductModel model)
        {
            var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                      join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                      join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                      where model.Id.Equals(a.ProductId)
                                      orderby b.Code
                                      select new ModuleInProductModel
                                      {
                                          Id = a.Id,
                                          ModuleId = b.Id,
                                          Qty = a.Quantity,
                                          ModuleName = b.Name,
                                          Specification = b.Specification,
                                          Note = b.Note,
                                          Code = b.Code,
                                          Price = b.Pricing,
                                          LeadTime = b.Leadtime,
                                          Status = b.Status,
                                      }).AsQueryable();
            var modules = queryProductModule.ToList();
            foreach (var m in modules)
            {
                m.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(m.ModuleId, 0);
            }

            return modules;
        }

        /// <summary>
        /// Kiểm tra thêm mới thiết bị Product
        /// </summary>
        /// <param name="productModel"></param>
        public void CheckInputAdd(ProductModel productModel)
        {
            productModel.Code = Util.RemoveSpecialCharacter(productModel.Code);
            var group = db.ProductGroups.AsNoTracking().FirstOrDefault(t => t.Id.Equals(productModel.ProductGroupId));
            if (group != null)
            {
                if (!productModel.Code.ToUpper().StartsWith(group.Code.ToUpper()))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0025, TextResourceKey.Product);
                }
            }
            else
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductGroup);
            }

            if (db.Products.AsNoTracking().Where(o => o.Name.Equals(productModel.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Product);
            }

            if (db.Products.AsNoTracking().Where(o => o.Code.Equals(productModel.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Product);
            }
        }

        /// <summary>
        /// Lấy thông tin thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        public ProductModel GetProductInfo(ProductModel model)
        {
            var productInfo = db.Products.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProductModel
            {
                Id = p.Id,
                ProductGroupId = p.ProductGroupId,
                Name = p.Name,
                Code = p.Code,
                ProcedureTime = p.ProcedureTime,
                CurentVersion = p.CurentVersion,
                Content = p.Content,
                Description = p.Description,
                Status = p.Status,
                IsLayoutExist = p.IsLayoutExist,
                IsManualExist = p.IsManualExist,
                IsMaterialExist = p.IsMaterialExist,
                IsPracticeExist = p.IsPracticeExist,
                IsManualMaintenance = p.IsManualMaintenance,
                IsCatalog = p.IsCatalog,
                IsQuoteExist = p.IsQuoteExist,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
                DepartmentId = p.DepartmentId,
                IsTestResult = p.IsTestResult,
                IsSendSale = p.IsSendSale,
            }).FirstOrDefault();

            if (productInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
            }
            productInfo.CreateByName = (from a in db.Users.AsNoTracking()
                                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id into ab
                                        from anb in ab.DefaultIfEmpty()
                                        where a.Id.Equals(productInfo.CreateBy)
                                        select anb.Name).FirstOrDefault();
            var department = db.Departments.AsNoTracking().Where(a => a.Id.Equals(productInfo.DepartmentId)).FirstOrDefault();
            model.DepartmentName = department != null ? department.Name : string.Empty;
            var listImage = db.ProductImages.AsNoTracking().Where(p => p.ProductId.Equals(model.Id)).Select(u => new ProductImageModel
            {
                Id = u.Id,
                FileName = u.FileName,
                FilePath = u.FilePath,
                ThumbnailPath = u.ThumbnailPath,
                Note = u.Note,
                CreateBy = u.CreateBy,
                CreateDate = u.CreateDate
            }).ToList();

            var modules = (from b in db.PracticInProducts.AsNoTracking()
                           join p in db.Practices.AsNoTracking() on b.PracticeId equals p.Id
                           join m in db.ModuleInPractices.AsNoTracking() on p.Id equals m.PracticeId
                           where b.ProductId.Equals(productInfo.Id)
                           group m by new { m.ModuleId } into g
                           select new
                           {
                               g.Key.ModuleId,
                               Quantity = g.Max(r => r.Qty)
                           }).ToList();

            decimal moduleAmount = 0;

            foreach (var module in modules)
            {
                moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
            }

            var accessories = (from a in db.ProductAccessories.AsNoTracking()
                               join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                               where a.ProductId.Equals(productInfo.Id)
                               select new
                               {
                                   a.Quantity,
                                   c.Pricing,
                                   c.LastBuyDate,
                                   c.InputPriceDate,
                                   c.PriceHistory
                               }).ToList();

            decimal accessorieAmount = 0;
            decimal price = 0;

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in accessories)
            {
                price = item.Pricing;
                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        price = 0;
                    }
                }

                accessorieAmount += item.Quantity * price;
            }

            var listProductModule = (from a in db.ProductModuleUpdates.AsNoTracking()
                                     join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                     join c in db.Users.AsNoTracking() on a.UpdateBy equals c.Id
                                     join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                     where a.ProductId.Equals(model.Id)
                                     orderby b.Code
                                     select new ProductModuleUpdateModel
                                     {
                                         Id = a.Id,
                                         Code = b.Code,
                                         Name = b.Name,
                                         UpdateByName = d.Name,
                                         UpdateDate = a.UpdateDate
                                     }).ToList();
            productInfo.ListProductModuleUpdate = listProductModule;

            productInfo.Creator = (from a in db.Users.AsNoTracking()
                                   join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id into ab
                                   from anb in ab.DefaultIfEmpty()
                                   where a.Id.Equals(productInfo.CreateBy)
                                   select anb.Name).FirstOrDefault();
            var departmentInfo = db.Departments.AsNoTracking().Where(a => a.Id.Equals(productInfo.DepartmentId)).FirstOrDefault();
            productInfo.DepartmentName = departmentInfo != null ? departmentInfo.Name : string.Empty;

            productInfo.Pricing = moduleAmount + accessorieAmount;
            productInfo.ListImage = listImage;

            return productInfo;
        }

        public ProductDocumentAttachModel GetProductDocumentAttachs(ProductModel model)
        {
            ProductDocumentAttachModel result = new ProductDocumentAttachModel();
            // Danh sách file catalog theo id
            var listFileCatalog = (from a in db.ProductCatalogs.AsNoTracking()
                                   where a.ProductId.Equals(model.Id)
                                   select new ProductCatalogModel
                                   {
                                       Id = a.Id,
                                       ProductId = a.ProductId,
                                       FilePath = a.FilePath,
                                       FileName = a.FileName,
                                       CreateDate = a.CreateDate,
                                       FileSize = a.FileSize,
                                       Note = a.Note,
                                   }).ToList();

            listFileCatalog.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                      join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                      join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                      where a.ObjectId.Equals(model.Id) && c.Code.ToUpper().Equals(Constants.DocumentGroup_Code_Product_Catelog.ToUpper()) && a.ObjectType == Constants.ObjectType_Devide
                                      select new ProductCatalogModel
                                      {
                                          Id = b.Id,
                                          FileName = b.Name,
                                          Code = b.Code,
                                          FilePath = b.Id,
                                          Note = b.Description,
                                          IsDocument = true,
                                      }).ToList());

            // Danh sách file đính kèm
            var listFileAttach = (from a in db.ProductDocuments.AsNoTracking()
                                  where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_FileAttach
                                  select new ProductDocumentModel
                                  {
                                      Id = a.Id,
                                      ProductId = a.ProductId,
                                      Path = a.Path,
                                      FileName = a.FileName,
                                      CreateDate = a.CreateDate,
                                      FileSize = a.FileSize,
                                      Note = a.Note,
                                      FileType = a.FileType
                                  }).ToList();

            // Danh sách file hướng dẫn thực hành
            var listGuidePractice = (from a in db.ProductDocuments.AsNoTracking()
                                     where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_GuidePractive
                                     select new ProductDocumentModel
                                     {
                                         Id = a.Id,
                                         ProductId = a.ProductId,
                                         Path = a.Path,
                                         FileName = a.FileName,
                                         CreateDate = a.CreateDate,
                                         FileSize = a.FileSize,
                                         Note = a.Note,
                                         FileType = a.FileType
                                     }).ToList();

            listGuidePractice.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                        join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                        join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                        where a.ObjectId.Equals(model.Id) && c.Code.ToUpper().Equals(Constants.DocumentGroup_Code_Product_GuidePractive.ToUpper()) && a.ObjectType == Constants.ObjectType_Devide
                                        select new ProductDocumentModel
                                        {
                                            Id = b.Id,
                                            FileName = b.Name,
                                            Code = b.Code,
                                            Path = b.Id,
                                            Note = b.Description,
                                            FileType = Constants.ProductDocumentAttach_GuidePractive,
                                            IsDocument = true
                                        }).ToList());

            // Danh sách file báo giá
            var listQuotation = (from a in db.ProductDocuments.AsNoTracking()
                                 where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_Quotation
                                 select new ProductDocumentModel
                                 {
                                     Id = a.Id,
                                     ProductId = a.ProductId,
                                     Path = a.Path,
                                     FileName = a.FileName,
                                     CreateDate = a.CreateDate,
                                     FileSize = a.FileSize,
                                     Note = a.Note,
                                     FileType = a.FileType
                                 }).ToList();

            // Danh sách file bản vẽ layout
            var listDrawingLayout = (from a in db.ProductDocuments.AsNoTracking()
                                     where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_DrawingLayout
                                     select new ProductDocumentModel
                                     {
                                         Id = a.Id,
                                         ProductId = a.ProductId,
                                         Path = a.Path,
                                         FileName = a.FileName,
                                         CreateDate = a.CreateDate,
                                         FileSize = a.FileSize,
                                         Note = a.Note,
                                         FileType = a.FileType
                                     }).ToList();

            // Danh sách file danh mục vật tư
            var listDMVT = (from a in db.ProductDocuments.AsNoTracking()
                            where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_DMVT
                            select new ProductDocumentModel
                            {
                                Id = a.Id,
                                ProductId = a.ProductId,
                                Path = a.Path,
                                FileName = a.FileName,
                                CreateDate = a.CreateDate,
                                FileSize = a.FileSize,
                                Note = a.Note,
                                FileType = a.FileType
                            }).ToList();

            // Danh sách file danh mục bài thực hành
            var listDMBTH = (from a in db.ProductDocuments.AsNoTracking()
                             where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_DMBTH
                             select new ProductDocumentModel
                             {
                                 Id = a.Id,
                                 ProductId = a.ProductId,
                                 Path = a.Path,
                                 FileName = a.FileName,
                                 CreateDate = a.CreateDate,
                                 FileSize = a.FileSize,
                                 Note = a.Note,
                                 FileType = a.FileType
                             }).ToList();

            listDMBTH.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                where a.ObjectId.Equals(model.Id) && c.Code.ToUpper().Equals(Constants.DocumentGroup_Code_Product_DMBTH.ToUpper()) && a.ObjectType == Constants.ObjectType_Devide
                                select new ProductDocumentModel
                                {
                                    Id = b.Id,
                                    FileName = b.Name,
                                    Code = b.Code,
                                    Path = b.Id,
                                    Note = b.Description,
                                    FileType = Constants.ProductDocumentAttach_DMBTH,
                                    IsDocument = true
                                }).ToList());

            // Danh sách file hướng dẫn bảo trì
            var listGuideMaintenance = (from a in db.ProductDocuments.AsNoTracking()
                                        where a.ProductId.Equals(model.Id) && a.FileType == Constants.ProductDocumentAttach_GuideMaintenance
                                        select new ProductDocumentModel
                                        {
                                            Id = a.Id,
                                            ProductId = a.ProductId,
                                            Path = a.Path,
                                            FileName = a.FileName,
                                            CreateDate = a.CreateDate,
                                            FileSize = a.FileSize,
                                            Note = a.Note,
                                            FileType = a.FileType
                                        }).ToList();

            listGuideMaintenance.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                           join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                           join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                           where a.ObjectId.Equals(model.Id) && c.Code.ToUpper().Equals(Constants.DocumentGroup_Code_Product_GuideMaintenance.ToUpper()) && a.ObjectType == Constants.ObjectType_Devide
                                           select new ProductDocumentModel
                                           {
                                               Id = b.Id,
                                               FileName = b.Name,
                                               Code = b.Code,
                                               Path = b.Id,
                                               Note = b.Description,
                                               FileType = Constants.ProductDocumentAttach_GuideMaintenance,
                                               IsDocument = true
                                           }).ToList());

            result.ListFileAttach = listFileAttach;
            result.ListGuidePractice = listGuidePractice;
            result.ListQuotation = listQuotation;
            result.ListDrawingLayout = listDrawingLayout;
            result.ListDMVT = listDMVT;
            result.ListDMBTH = listDMBTH;
            result.ListGuideMaintenance = listGuideMaintenance;
            result.ListFileCatalog = listFileCatalog;
            return result;
        }

        public bool UpdateProductDocument(UpdateProductDocumentModel model)
        {
            bool rs = false;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var product = db.Products.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    var productDocuments = db.ProductDocuments.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productDocuments.Count > 0)
                    {
                        db.ProductDocuments.RemoveRange(productDocuments);
                    }

                    var documentObjects = db.DocumentObjects.Where(a => a.ObjectId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_Devide).ToList();
                    if (documentObjects.Count > 0)
                    {
                        db.DocumentObjects.RemoveRange(documentObjects);
                    }

                    if (model.ListFileDocument.Count > 0)
                    {
                        List<ProductDocument> listProduct = new List<ProductDocument>();
                        foreach (var item in model.ListFileDocument)
                        {
                            if (!item.IsDocument)
                            {
                                if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.Path))
                                {
                                    ProductDocument productDocument = new ProductDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProductId = model.Id,
                                        Path = item.Path,
                                        FileName = item.FileName,
                                        CreateDate = DateTime.Now,
                                        FileSize = item.FileSize,
                                        Note = item.Note,
                                        FileType = item.FileType,
                                    };
                                    listProduct.Add(productDocument);
                                }
                            }
                            else
                            {
                                DocumentObject documentObject = new DocumentObject()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    DocumentId = item.Id,
                                    ObjectId = model.Id,
                                    ObjectType = Constants.ObjectType_Devide
                                };

                                db.DocumentObjects.Add(documentObject);
                            }
                        }
                        db.ProductDocuments.AddRange(listProduct);

                        var guidePractice = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_GuidePractive);
                        if (guidePractice.Count() > 0)
                        {
                            product.IsManualExist = true;
                        }
                        else
                        {
                            product.IsManualExist = false;
                        }

                        var quotation = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_Quotation);
                        if (quotation.Count() > 0)
                        {
                            product.IsQuoteExist = true;
                        }
                        else
                        {
                            product.IsQuoteExist = false;
                        }

                        var drawingLayout = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_DrawingLayout);
                        if (drawingLayout.Count() > 0)
                        {
                            product.IsLayoutExist = true;
                        }
                        else
                        {
                            product.IsLayoutExist = false;
                        }

                        var dmvt = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_DMVT);
                        if (dmvt.Count() > 0)
                        {
                            product.IsMaterialExist = true;
                        }
                        else
                        {
                            product.IsMaterialExist = false;
                        }

                        var dmbth = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_DMBTH);
                        if (dmbth.Count() > 0)
                        {
                            product.IsPracticeExist = true;
                        }
                        else
                        {
                            product.IsPracticeExist = false;
                        }

                        var guideMaintenance = listProduct.Where(t => t.FileType == Constants.ProductDocumentAttach_GuideMaintenance);
                        if (guideMaintenance.Count() > 0)
                        {
                            product.IsManualMaintenance = true;
                        }
                        else
                        {
                            product.IsManualMaintenance = false;
                        }
                    }

                    // Danh sách file catalog

                    var productCatalogs = db.ProductCatalogs.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productCatalogs.Count > 0)
                    {
                        db.ProductCatalogs.RemoveRange(productCatalogs);
                    }
                    if (model.ListFileCatalog.Count > 0)
                    {
                        List<ProductCatalog> listCatalog = new List<ProductCatalog>();
                        foreach (var item in model.ListFileCatalog)
                        {
                            if (!item.IsDocument)
                            {
                                if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FilePath))
                                {
                                    ProductCatalog productCatalog = new ProductCatalog()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProductId = model.Id,
                                        FilePath = item.FilePath,
                                        FileName = item.FileName,
                                        CreateDate = DateTime.Now,
                                        FileSize = item.FileSize,
                                        Note = item.Note,
                                        CreateBy = model.UserId
                                    };

                                    listCatalog.Add(productCatalog);
                                }
                            }
                            else
                            {
                                DocumentObject documentObject = new DocumentObject()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    DocumentId = item.Id,
                                    ObjectId = model.Id,
                                    ObjectType = Constants.ObjectType_Devide
                                };

                                db.DocumentObjects.Add(documentObject);
                            }
                        }
                        product.IsCatalog = true;
                        db.ProductCatalogs.AddRange(listCatalog);
                    }
                    db.SaveChanges();
                    trans.Commit();
                    rs = true;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return rs;
        }

        /// <summary>
        /// Kiểm tra cập nhật thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        private void CheckExistedForUpdate(ProductModel model)
        {
            if (db.Products.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Product);
            }

            if (db.Products.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Product);
            }
        }

        /// <summary>
        /// Cập nhật thiết bị
        /// </summary>
        /// <param name="productModel"></param>
        public void UpdateProduct(ProductModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            CheckExistedForUpdate(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var product = db.Products.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    product.ProductGroupId = model.ProductGroupId;
                    product.ProcedureTime = model.ProcedureTime;
                    product.CurentVersion = model.CurentVersion;
                    product.Content = model.Content;
                    product.Status = model.Status;
                    product.IsManualMaintenance = model.IsManualMaintenance;
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Code = model.Code;
                    product.UpdateBy = model.UpdateBy;
                    product.UpdateDate = DateTime.Now;

                    var listImageOld = db.ProductImages.Where(p => p.ProductId.Equals(model.Id)).ToList();
                    db.ProductImages.RemoveRange(listImageOld);

                    if (model.ListImage != null)
                    {
                        foreach (var item in model.ListImage)
                        {
                            ProductImage newImage = new ProductImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = model.Id,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                ThumbnailPath = item.ThumbnailPath,
                                Note = item.Note,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                            };
                            db.ProductImages.Add(newImage);
                        }
                    }


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

        /// <summary>
        /// Lấy danh sách chọn Module
        /// </summary>
        /// <param name="moduleModel"></param>
        public object GetModule(ModuleSearchModel modelSearch)
        {
            SearchResultModel<ModuleModel> searchResult = new SearchResultModel<ModuleModel>();

            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ModuleModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 ModuleGroupId = b.Id,
                                 ModuleGroupName = b.Name,
                                 Pricing = a.Pricing,
                                 Note = a.Note,
                                 Specification = a.Specification,
                                 LeadTime = a.Leadtime,
                                 ModuleGroupCode = b.Code,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
            {
                dataQuery = dataQuery.Where(a => a.ModuleGroupId.Equals(modelSearch.ModuleGroupId));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void DeleteProduct(ProductModel model, string departmentId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var product = db.Products.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (product == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                    }

                    var productVersion = db.ProductVersions.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productVersion.Count() > 0)
                    {
                        db.ProductVersions.RemoveRange(productVersion);
                    }
                    var productImage = db.ProductImages.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productImage.Count() > 0)
                    {
                        db.ProductImages.RemoveRange(productImage);
                    }
                    var productDocuments = db.ProductDocuments.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productDocuments.Count() > 0)
                    {
                        db.ProductDocuments.RemoveRange(productDocuments);
                    }

                    var productTestResult = db.ProductTestResultAttachs.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productTestResult.Count() > 0)
                    {
                        db.ProductTestResultAttachs.RemoveRange(productTestResult);
                    }

                    var producAccess = db.ProductAccessories.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (producAccess.Count() > 0)
                    {
                        db.ProductAccessories.RemoveRange(producAccess);
                    }

                    var practiceInProduct = db.PracticInProducts.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (practiceInProduct.Count() > 0)
                    {
                        db.PracticInProducts.RemoveRange(practiceInProduct);
                    }

                    var productMaterial = db.ProductMaterials.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (productMaterial.Count() > 0)
                    {
                        db.ProductMaterials.RemoveRange(productMaterial);
                    }
                    var projectProduct = db.ProjectProducts.Where(a => a.ProductId.Equals(model.Id)).ToList();
                    if (projectProduct.Count > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Product);

                    }
                    if (!string.IsNullOrEmpty(product.DepartmentId) && !product.DepartmentId.Equals(departmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.Product);
                    }

                    db.Products.Remove(product);
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

        public void DeleteProductModuleUpdate(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productModuleUpdate = db.ProductModuleUpdates.Where(u => u.ProductId.Equals(id)).ToList();
                    //if (productModuleUpdate == null)
                    //{
                    //    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                    //}

                    db.ProductModuleUpdates.RemoveRange(productModuleUpdate);
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

        /// <summary>
        /// Thêm mới phác thảo thiết kế Thiết bị
        /// </summary>
        /// <param name="practiceModel"></param>
        public void AddProductSkeches(ProductModel productModel)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var listPracticeInProduct = db.PracticInProducts.Where(a => a.ProductId.Equals(productModel.Id)).ToList();
                var listPracticeInProductId = db.PracticInProducts.Select(a => a.PracticeId).ToList();
                if (listPracticeInProduct.Count() > 0)
                {
                    db.PracticInProducts.RemoveRange(listPracticeInProduct);
                }

                //foreach (var item in listPracticeInProduct)
                //{
                //    var moduleInPractice = db.ModuleInPractices.Where(a => a.PracticeId.Equals(item.PracticeId)).ToList();
                //    db.ModuleInPractices.RemoveRange(moduleInPractice);
                //}

                var listProductModule = db.ProductModules.Where(a => a.ProductId.Equals(productModel.Id)).ToList();
                if (listProductModule.Count > 0)
                {
                    db.ProductModules.RemoveRange(listProductModule);
                }

                var productAccessories = db.ProductAccessories.Where(i => i.ProductId.Equals(productModel.Id)).ToList();
                if (productAccessories.Count > 0)
                {
                    db.ProductAccessories.RemoveRange(productAccessories);
                }

                try
                {
                    if (productModel.ListModuleProduct.Count > 0)
                    {
                        ProductModule productModule;
                        foreach (var item in productModel.ListModuleProduct)
                        {
                            productModule = new ProductModule();
                            productModule.Id = Guid.NewGuid().ToString();
                            productModule.ModuleId = item.ModuleId;
                            productModule.ProductId = productModel.Id;
                            productModule.Quantity = item.Qty;
                            db.ProductModules.Add(productModule);
                        }
                    }

                    if (productModel.ListPractice.Count() > 0)
                    {
                        foreach (var practice in productModel.ListPractice)
                        {
                            PracticInProduct newPractice = new PracticInProduct()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = productModel.Id,
                                PracticeId = practice.Id,
                                Qty = 1,
                                Version = 0
                            };
                            db.PracticInProducts.Add(newPractice);

                            //foreach (var module in practice.ListModuleInPractice)
                            //{
                            //    if (module.Qty != 0)
                            //    {
                            //        ModuleInPractice moduleInPractice = new ModuleInPractice()
                            //        {
                            //            Id = Guid.NewGuid().ToString(),
                            //            ModuleId = module.ModuleId,
                            //            PracticeId = practice.Id,
                            //            Qty = module.Qty,
                            //            Version = 0,
                            //        };
                            //        db.ModuleInPractices.Add(moduleInPractice);
                            //    }
                            //}
                        }
                    }

                    foreach (var item in productModel.ListSelect)
                    {
                        ProductAccessory newProductAccessory = new ProductAccessory
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = productModel.Id,
                            MaterialId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Amount = item.Quantity * item.Price,
                            Note = item.Note,
                            Type = item.Type
                        };
                        db.ProductAccessories.Add(newProductAccessory);
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(productModel, ex);
                }
            }
        }
        public object GetProductSketchesInfo(ProductModel model)
        {
            ProductModel product = new ProductModel();
            List<ModuleInPracticeModel> listModule = new List<ModuleInPracticeModel>();
            product.Id = model.Id;
            var listPractice = (from a in db.PracticInProducts.AsNoTracking()
                                join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                                where a.ProductId.Equals(model.Id)
                                orderby b.Code
                                select new PracticeModel
                                {
                                    Id = a.PracticeId,
                                    Name = b.Name,
                                    LeadTime = b.LeadTime,
                                    Code = b.Code,
                                    ListModuleInPractice = (from c in db.ModuleInPractices
                                                            join d in db.Modules on c.ModuleId equals d.Id
                                                            where c.PracticeId.Equals(a.PracticeId)
                                                            select new ModuleInPracticeModel
                                                            {
                                                                Id = c.Id,
                                                                ModuleId = c.ModuleId,
                                                                PracticeId = c.PracticeId,
                                                                Qty = c.Qty,
                                                                ModuleName = d.Name,
                                                                Specification = d.Specification,
                                                                Note = d.Note,
                                                                Code = d.Code,
                                                                Pricing = d.Pricing,
                                                                LeadTime = d.Leadtime,
                                                            }).ToList()
                                }).ToList();

            foreach (var practice in listPractice)
            {
                ModuleInPracticeModel[] a = new ModuleInPracticeModel[practice.ListModuleInPractice.Count];
                practice.ListModuleInPractice.CopyTo(a);
                listModule.AddRange(a.ToList());
            }

            var listModuleGroup = listModule.GroupBy(t => new { t.LeadTime, t.ModuleName, t.Pricing, t.ModuleId, t.Code, t.Specification, t.Note }).OrderBy(o => o.Key.Code).Select(b => new ModuleInPracticeModel
            {
                ModuleId = b.Key.ModuleId,
                Code = b.Key.Code,
                Pricing = b.Key.Pricing,
                Qty = 0,
                Specification = b.Key.Specification,
                Note = b.Key.Note,
                ModuleName = b.Key.ModuleName,
                LeadTime = b.Key.LeadTime,
                MaxQtyByPractice = b.Max(a => a.Qty)
            }).ToList();


            ModulePriceInfoModel modulePriceInfoModel;
            foreach (var item in listModuleGroup)
            {
                modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.ModuleId);
                item.Pricing = modulePriceInfoModel.Price;
                item.IsNoPrice = modulePriceInfoModel.IsNoPrice;
            }

            foreach (var practice in listPractice)
            {

                foreach (var item in listModuleGroup)
                {
                    item.Qty = 0;
                    foreach (var module in practice.ListModuleInPractice)
                    {
                        if (module.ModuleId.Equals(item.ModuleId))
                        {
                            item.Qty = module.Qty;
                            break;
                        }
                    }
                    practice.TotalPrice = practice.TotalPrice + item.Qty * item.Pricing;
                }

                practice.ListModuleInPractice = listModuleGroup.Select(a => new ModuleInPracticeModel
                {
                    ModuleId = a.ModuleId,
                    Code = a.Code,
                    PracticeId = practice.Id,
                    Pricing = a.Pricing,
                    Qty = a.Qty,
                    Specification = a.Specification,
                    Note = a.Note,
                    ModuleName = a.ModuleName,
                    LeadTime = a.LeadTime,
                    MaxQtyByPractice = a.MaxQtyByPractice
                }).ToList();

            }

            if (listPractice.Count() > 0)
            {
                foreach (var practice in listPractice)
                {
                    if (practice.ListModuleInPractice.Count() > 0)
                    {
                        int maxLeadTime;
                        //var max = practice.ListModuleInPractice.Where(b => b.Qty > 0).Max(a => a.LeadTime);
                        var listModuleQty = practice.ListModuleInPractice.Where(b => b.Qty > 0).ToList();
                        if (listModuleQty.Count() > 0)
                        {
                            maxLeadTime = listModuleQty.Max(a => a.LeadTime);
                            practice.MaxTotalLeadtime = practice.LeadTime + maxLeadTime;
                        }
                        else
                        {
                            practice.MaxTotalLeadtime = practice.LeadTime;
                        }

                    }
                }
            }

            var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                      join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                      join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                      where model.Id.Equals(a.ProductId)
                                      orderby b.Code
                                      select new ModuleInProductModel
                                      {
                                          Id = a.Id,
                                          ModuleId = b.Id,
                                          Qty = a.Quantity,
                                          ModuleName = b.Name,
                                          Specification = b.Specification,
                                          Note = b.Note,
                                          Code = b.Code,
                                          Price = b.Pricing,
                                          LeadTime = b.Leadtime,
                                      }).AsQueryable();
            foreach (var ite in listPractice)
            {

                ite.ListPracticeFile = (from a in db.PracticeFiles.AsNoTracking()
                                        where a.PracticeId.Equals(ite.Id)
                                        select new PracticeFileModel
                                        {
                                            Id = a.Id,
                                            PracticeId = a.PracticeId,
                                            FileName = a.FileName,
                                            FilePath = a.FilePath,
                                            Size = a.Size.ToString(),
                                            Description = a.Description,
                                            CreateBy = a.CreateBy,
                                            CreateByName = ite.Name,
                                            CreateDate = a.CreateDate,
                                        }).ToList();

                ite.HasDocument = ite.ListPracticeFile.Count > 0;
            }

            product.ListModuleProduct = queryProductModule.ToList();

            product.ListPractice = listPractice;

            return product;
        }

        public List<ProductAccessoriesModel> GetListMatarial(List<PracticeModel> practiceModels)
        {
            var practiceSubMaterials = (from a in db.PracticeSubMaterials.AsEnumerable()
                                        join c in practiceModels.AsEnumerable() on a.PracticeId equals c.Id
                                        select new PracticeSupMaterialModel
                                        {
                                            PracticeId = a.PracticeId,
                                            MaterialId = a.MaterialId,
                                            Quantity = a.Quantity,
                                            Type = a.Type,
                                        }).ToList();
            var practiceMaterialConsumables = (from a in db.PracticeMaterialConsumables.AsEnumerable()
                                               join c in practiceModels.AsEnumerable() on a.PracticeId equals c.Id
                                               select new PracticeSupMaterialModel
                                               {
                                                   PracticeId = a.PracticeId,
                                                   MaterialId = a.MaterialId,
                                                   Quantity = a.Quantity,
                                                   Type = 1
                                               }).ToList();
            List<PracticeSupMaterialModel> practiceSupMaterialModels = new List<PracticeSupMaterialModel>();
            practiceSupMaterialModels.AddRange(practiceSubMaterials);
            practiceSupMaterialModels.AddRange(practiceMaterialConsumables);

            var listId = (from a in practiceSupMaterialModels
                          group a by new { a.MaterialId, a.Type } into g
                          select new
                          {
                              //g.Key.PracticeId,
                              g.Key.MaterialId,
                              g.Key.Type,
                              Quantity = g.Max(i => i.Quantity)
                          }).ToList();

            var dataQuery = (from a in listId
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                             where a.Type == Constants.PracticeSupMaterial_Type_Material
                             orderby c.Code
                             select new
                             {
                                 c.Id,
                                 Code = c.Code,
                                 Name = c.Name,
                                 MaterialId = a.MaterialId,
                                 Manafacture = d.Name,
                                 Price = c.Pricing,
                                 c.LastBuyDate,
                                 c.InputPriceDate,
                                 c.PriceHistory,
                                 Type = a.Type,
                                 a.Quantity
                             }).ToList();
            List<ProductAccessoriesModel> listData = new List<ProductAccessoriesModel>();
            ProductAccessoriesModel productAccessoriesModel;
            int day = 0;
            foreach (var item in dataQuery)
            {
                productAccessoriesModel = new ProductAccessoriesModel()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialId = item.MaterialId,
                    Manafacture = item.Name,
                    Quantity = Convert.ToDecimal(item.Quantity),
                    Price = item.Price,
                    Type = item.Type
                };

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    day = materialBusiness.GetConfigMaterialLastByDate();
                    if (timeSpan.Days <= day)
                    {
                        productAccessoriesModel.Price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        productAccessoriesModel.Price = 0;
                    }
                }

                productAccessoriesModel.Amount = productAccessoriesModel.Quantity * productAccessoriesModel.Price;
                listData.Add(productAccessoriesModel);
            }

            var data = (from a in listId
                        join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                        join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                        where a.Type == Constants.PracticeSupMaterial_Type_Module
                        select new ProductAccessoriesModel
                        {
                            Id = b.Id,
                            MaterialId = a.MaterialId,
                            Name = b.Name,
                            Code = b.Code,
                            Manafacture = Constants.Manufacture_TPA,
                            Quantity = Convert.ToDecimal(a.Quantity),
                            Price = b.Pricing,
                            Type = a.Type
                        }).ToList();

            foreach (var item in data)
            {
                item.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
                item.Amount = item.Quantity * item.Price;
            }
            listData.AddRange(data);

            return listData;
        }

        public List<ModuleModel> ImportModuleProductSketches(string userId, HttpPostedFile file, string productId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string moduleCode, qty;

            #region
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<ModuleModel> listImports = new List<ModuleModel>();
            ModuleModel item;
            var listModule = db.Modules.AsNoTracking().ToList();
            List<int> rowModuleCode = new List<int>();
            List<int> rowCheckModuleCode = new List<int>();
            List<int> rowQty = new List<int>();
            List<int> rowCheckQty = new List<int>();
            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    item = new ModuleModel();
                    moduleCode = sheet[i, 2].Value;
                    qty = sheet[i, 3].Value;

                    //Code
                    if (!string.IsNullOrEmpty(moduleCode))
                    {
                        var data = listModule.FirstOrDefault(a => a.Code.ToUpper().Equals(moduleCode.ToUpper()));
                        if (data != null)
                        {
                            item.Id = data.Id;
                            item.Name = data.Name;
                            item.Code = data.Code;
                            item.Pricing = data.Pricing;
                            item.Note = data.Note;
                            item.Specification = data.Specification;
                            item.LeadTime = data.Leadtime;
                        }
                        else
                        {
                            rowCheckModuleCode.Add(i);
                        }
                    }
                    else
                    {
                        rowModuleCode.Add(i);
                    }

                    // Qty
                    try
                    {
                        if (!string.IsNullOrEmpty(qty))
                        {
                            item.Quantity = Convert.ToDecimal(qty);
                        }
                        else
                        {
                            rowQty.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckQty.Add(i);
                        continue;
                    }
                    listImports.Add(item);
                }

                #endregion

                if (rowModuleCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã Module dòng <" + string.Join(", ", rowModuleCode) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckModuleCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã Module dòng <" + string.Join(", ", rowCheckModuleCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowQty.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQty) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckQty.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQty) + "> không đúng định dạng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }
            }
            catch (Exception ex)
            {
                //fs.Close();
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

            //fs.Close();
            workbook.Close();
            excelEngine.Dispose();
            return listImports;
        }

        public List<ProductDesignDocumentModel> GetListFolderProduct(string productId)
        {
            List<ProductDesignDocumentModel> list = new List<ProductDesignDocumentModel>();
            list = db.ProductDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_Folder) && t.ProductId.Equals(productId))
                .OrderBy(o => o.Path)
                .Select(m => new ProductDesignDocumentModel
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            var root = db.ProductDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder) && string.IsNullOrEmpty(t.ProductId))
                .OrderBy(o => o.Path).Select(m => new ProductDesignDocumentModel
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    Path = m.Path,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            list.AddRange(root);
            return list;
        }

        public List<ProductDesignDocumentModel> GetListFileProduct(string folderId)
        {
            var list = db.ProductDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File) && t.ParentId.Equals(folderId))
                 .OrderBy(o => o.Path)
                .Select(m => new ProductDesignDocumentModel
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    FileSize = m.FileSize,
                    CreateDate = m.CreateDate,
                    UpdateDate = m.UpdateDate,
                    ServerPath = m.ServerPath
                }).ToList();

            return list;
        }

        /// <summary>
        /// Upload tài liệu thiết kế
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UploadDesignDocument(UploadFolderProductDesignDocumentModel model, string userId)
        {
            var product = db.Products.FirstOrDefault(t => t.Id.Equals(model.ProductId));

            if (product == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Có tài liệu thiết kế
                    if (model.DesignDocuments.Count > 0)
                    {
                        List<ProductDesignDocument> designDocuments = new List<ProductDesignDocument>();
                        ProductDesignDocument designDocument;
                        ProductDesignDocument designDocumentFile;

                        var documents = db.ProductDesignDocuments.Where(r => r.DesignType == model.DesignType && r.ProductId.Equals(product.Id)).ToList();

                        bool isDelete;
                        foreach (var document in documents)
                        {
                            isDelete = true;
                            foreach (var item in model.DesignDocuments)
                            {
                                if (item.LocalPath.Equals(document.Path))
                                {
                                    isDelete = false;
                                    break;
                                }

                                foreach (var file in item.Files)
                                {
                                    if (file.LocalPath.Equals(document.Path))
                                    {
                                        isDelete = false;
                                        break;
                                    }
                                }

                                if (!isDelete)
                                {
                                    break;
                                }
                            }

                            if (isDelete)
                            {
                                db.ProductDesignDocuments.Remove(document);
                            }

                        }

                        var folderRoor = db.ProductDesignDocuments.Where(r => r.Id.Equals(model.DesignType.ToString())).FirstOrDefault();
                        if (folderRoor == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0026);
                        }

                        FolderUploadModel parent;
                        foreach (var item in model.DesignDocuments)
                        {
                            designDocument = db.ProductDesignDocuments.FirstOrDefault(r => r.ProductId.Equals(model.ProductId) && r.Path.Equals(item.LocalPath));
                            if (designDocument == null)
                            {
                                designDocument = new ProductDesignDocument()
                                {
                                    Id = item.Id,
                                    ProductId = product.Id,
                                    ParentId = item.ParentId,
                                    ServerPath = item.ServerPath,
                                    Path = item.LocalPath,
                                    Name = item.Name,
                                    FileType = Constants.ModuleDesignDocument_FileType_Folder,
                                    DesignType = model.DesignType,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now
                                };

                                if (string.IsNullOrEmpty(designDocument.ParentId))
                                {
                                    designDocument.ParentId = folderRoor.Id;
                                }
                                else
                                {
                                    parent = model.DesignDocuments.FirstOrDefault(r => r.Id.Equals(item.ParentId));
                                    if (parent != null && !string.IsNullOrEmpty(parent.DBId))
                                    {
                                        designDocument.ParentId = parent.DBId;
                                    }
                                }

                                designDocuments.Add(designDocument);
                            }
                            else
                            {
                                item.DBId = designDocument.Id;
                                designDocument.UpdateBy = userId;
                                designDocument.UpdateDate = DateTime.Now;
                                designDocument.ServerPath = item.ServerPath;
                            }

                            foreach (var file in item.Files)
                            {
                                designDocumentFile = db.ProductDesignDocuments.FirstOrDefault(r => r.ProductId.Equals(model.ProductId) && r.Path.Equals(item.LocalPath));
                                if (designDocumentFile == null)
                                {
                                    designDocumentFile = new ProductDesignDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProductId = product.Id,
                                        ParentId = item.Id,
                                        ServerPath = file.ServerPath,
                                        Path = file.LocalPath,
                                        Name = file.Name,
                                        FileType = Constants.ModuleDesignDocument_FileType_File,
                                        FileSize = file.Size,
                                        DesignType = model.DesignType,
                                        CreateBy = userId,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = userId,
                                        UpdateDate = DateTime.Now
                                    };

                                    if (designDocument != null)
                                    {
                                        designDocumentFile.ParentId = designDocument.Id;
                                    }

                                    designDocuments.Add(designDocumentFile);
                                }
                                else
                                {
                                    designDocumentFile.UpdateBy = userId;
                                    designDocumentFile.UpdateDate = DateTime.Now;
                                    designDocumentFile.FileSize = file.Size;
                                    designDocumentFile.ServerPath = file.ServerPath;
                                }
                            }
                        }

                        db.ProductDesignDocuments.AddRange(designDocuments);
                    }


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    NtsLog.LogError(ex);
                    throw;
                }
            }
        }

        public List<ModuleInProductModel> GetModulePrice(List<string> listModuleId)
        {
            List<ModuleInProductModel> moduleAmounts = new List<ModuleInProductModel>();
            ModuleInProductModel moduleInProduct;
            ModulePriceInfoModel modulePriceInfoModel;
            foreach (var moduleId in listModuleId)
            {
                moduleInProduct = new ModuleInProductModel();
                modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(moduleId);
                moduleInProduct.Price = modulePriceInfoModel.Price;
                moduleInProduct.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                moduleInProduct.ModuleId = moduleId;

                moduleAmounts.Add(moduleInProduct);
            }

            return moduleAmounts;
        }

        public List<PracticeModel> SynchronizedPractice(ProductModel model)
        {
            List<PracticeModel> result = new List<PracticeModel>();
            var listAllPractice = (from a in db.Practices.AsNoTracking()
                                   orderby a.Code
                                   select new PracticeModel
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       LeadTime = a.LeadTime,
                                       Code = a.Code,
                                       ListModuleInPractice = (from c in db.ModuleInPractices
                                                               join d in db.Modules on c.ModuleId equals d.Id
                                                               where c.PracticeId.Equals(a.Id)
                                                               select new ModuleInPracticeModel
                                                               {
                                                                   Id = c.Id,
                                                                   ModuleId = c.ModuleId,
                                                                   PracticeId = c.PracticeId,
                                                                   Qty = c.Qty,
                                                                   ModuleName = d.Name,
                                                                   Specification = d.Specification,
                                                                   Note = d.Note,
                                                                   Code = d.Code,
                                                                   Pricing = d.Pricing,
                                                                   LeadTime = d.Leadtime,
                                                               }).ToList()
                                   }).ToList();

            var totalModuleInProduct = model.ListModuleProduct.Count();

            listAllPractice = listAllPractice.Where(a => a.ListModuleInPractice.Count() <= totalModuleInProduct && a.ListModuleInPractice.Count() > 0).ToList();

            if (listAllPractice.Count() > 0)
            {

                foreach (var practice in listAllPractice)
                {
                    bool isOk = true;
                    foreach (var module in practice.ListModuleInPractice)
                    {
                        var moduleCheck = model.ListModuleProduct.FirstOrDefault(a => a.ModuleId.Equals(module.ModuleId));
                        if (moduleCheck != null)
                        {
                            if (module.Qty > moduleCheck.Qty)
                            {
                                isOk = false;
                                break;
                            }
                        }
                        else
                        {
                            isOk = false;
                            break;
                        }
                    }
                    if (isOk)
                    {
                        result.Add(practice);
                    }
                }
            }

            List<ModuleInPracticeModel> listModule = new List<ModuleInPracticeModel>();
            foreach (var practice in result)
            {
                ModuleInPracticeModel[] a = new ModuleInPracticeModel[practice.ListModuleInPractice.Count];
                practice.ListModuleInPractice.CopyTo(a);
                listModule.AddRange(a.ToList());
            }

            var listModuleGroupBy = listModule.GroupBy(t => new { t.LeadTime, t.ModuleName, t.Pricing, t.ModuleId, t.Code, t.Specification, t.Note }).OrderBy(o => o.Key.Code).Select(b => new ModuleInPracticeModel
            {
                ModuleId = b.Key.ModuleId,
                Code = b.Key.Code,
                Pricing = b.Key.Pricing,
                Qty = 0,
                Specification = b.Key.Specification,
                Note = b.Key.Note,
                ModuleName = b.Key.ModuleName,
                LeadTime = b.Key.LeadTime,
                MaxQtyByPractice = b.Max(a => a.Qty)

            }).ToList();

            ModulePriceInfoModel modulePriceInfoModel;
            foreach (var item in listModuleGroupBy)
            {
                modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.ModuleId);
                item.Pricing = modulePriceInfoModel.Price;
                item.IsNoPrice = modulePriceInfoModel.IsNoPrice;
            }

            foreach (var practice in result)
            {

                foreach (var item in listModuleGroupBy)
                {
                    item.Qty = 0;
                    foreach (var module in practice.ListModuleInPractice)
                    {
                        if (module.ModuleId.Equals(item.ModuleId))
                        {
                            item.Qty = module.Qty;
                            break;
                        }
                    }
                    practice.TotalPrice = practice.TotalPrice + item.Qty * item.Pricing;
                }

                practice.ListModuleInPractice = listModuleGroupBy.Select(a => new ModuleInPracticeModel
                {
                    ModuleId = a.ModuleId,
                    Code = a.Code,
                    PracticeId = practice.Id,
                    Pricing = a.Pricing,
                    Qty = a.Qty,
                    Specification = a.Specification,
                    Note = a.Note,
                    ModuleName = a.ModuleName,
                    LeadTime = a.LeadTime,
                    MaxQtyByPractice = a.MaxQtyByPractice
                }).ToList();

            }

            foreach (var ite in result)
            {

                ite.ListPracticeFile = (from a in db.PracticeFiles.AsNoTracking()
                                        where a.PracticeId.Equals(ite.Id)
                                        select new PracticeFileModel
                                        {
                                            Id = a.Id,
                                            PracticeId = a.PracticeId,
                                            FileName = a.FileName,
                                            FilePath = a.FilePath,
                                            Size = a.Size.ToString(),
                                            Description = a.Description,
                                            CreateBy = a.CreateBy,
                                            CreateByName = ite.Name,
                                            CreateDate = a.CreateDate,
                                        }).ToList();
            }



            return result;
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var productGroup = db.ProductGroups.AsNoTracking().Where(i => i.ParentId.Equals(id)).Select(i => i.Id).ToList();
            listChild.AddRange(productGroup);
            if (productGroup.Count > 0)
            {
                foreach (var item in productGroup)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }

        public decimal GetProductPrice(string id)
        {
            decimal moduleAmount = 0;

            var modules = (from b in db.ProductModules.AsNoTracking()
                           where b.ProductId.Equals(id)
                           select new
                           {
                               b.ModuleId,
                               b.Quantity
                           }).ToList();

            moduleAmount = 0;
            foreach (var module in modules)
            {
                moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
            }

            var accessories = (from a in db.ProductAccessories.AsNoTracking()
                               join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                               where a.ProductId.Equals(id)
                               select new
                               {
                                   a.Quantity,
                                   c.Pricing,
                                   c.LastBuyDate,
                                   c.InputPriceDate,
                                   c.PriceHistory
                               }).ToList();

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();

            decimal price;
            foreach (var acces in accessories)
            {
                price = acces.Pricing;
                if (acces.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(acces.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        price = acces.PriceHistory;
                    }
                    else if (!acces.InputPriceDate.HasValue || acces.InputPriceDate.Value.Date < acces.LastBuyDate.Value.Date)
                    {
                        price = 0;
                    }
                }

                moduleAmount += acces.Quantity * price;
            }

            return moduleAmount;
        }

        /// <summary>
        /// Lấy danh sách thiết bị lỗi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public SearchResultModel<ProductErrorModel> SearchProductErrors(SearchErrorProductModel model)
        {
            SearchResultModel<ProductErrorModel> searchResult = new SearchResultModel<ProductErrorModel>();


            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Products.AsNoTracking() on a.ObjectId equals g.Id into ag
                             from g in ag.DefaultIfEmpty()
                             where a.Type == Constants.Error_Type_Error && a.Status != Constants.Problem_Status_Creating && a.Status != Constants.Problem_Status_Awaiting_Confirm && a.ObjectType == 2 && a.ObjectId.Equals(model.Id)
                             orderby a.Code
                             select new ProductErrorModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 ErrorGroupName = b.Name,
                                 AuthorId = a.AuthorId,
                                 AuthorName = c.Name,
                                 PlanStartDate = a.PlanStartDate,
                                 ObjectId = a.ObjectId,
                                 ProjectId = a.ProjectId,
                                 ProjectName = d.Name,
                                 ProductName = g.Name,
                                 Type = a.Type,
                                 Status = a.Status,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = e.Name,
                                 ErrorBy = a.ErrorBy,
                                 ErrorByName = c.Name,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 DepartmentProcessName = e.Name,
                                 StageId = a.StageId,
                                 StageName = f.Name,
                                 FixBy = a.FixBy,
                                 FixByName = c.Name,
                                 Note = a.Note,
                                 ErrorCost = a.ErrorCost,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Subject))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(model.Subject.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (model.Status != null)
            {
                dataQuery = dataQuery.Where(u => u.Status == model.Status);
            }

            if (!string.IsNullOrEmpty(model.DateFrom.ToString()))
            {
                if (model.DateTo.ToString() == "")
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom);
                }
                else
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom && u.PlanStartDate <= model.DateTo);
                }

            }

            if (!string.IsNullOrEmpty(model.DateTo.ToString()))
            {
                if (model.DateFrom.ToString() == "")
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate <= model.DateTo);
                }
                else
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom && u.PlanStartDate <= model.DateTo);
                }

            }

            searchResult.TotalItem = dataQuery.Count();

            searchResult.Status1 = dataQuery.Where(r => r.Status == 5).Count();
            searchResult.Status2 = dataQuery.Where(r => r.Status == 6).Count();
            searchResult.Status3 = dataQuery.Where(r => r.Status == 7).Count();
            searchResult.Status4 = dataQuery.Where(r => r.Status == 8).Count();
            searchResult.Status5 = dataQuery.Where(r => r.Status == 9).Count();

            searchResult.MaxDeliveryDay = dataQuery.Where(r => r.Status == Constants.Error_Status_Close).Count();
            var listResult = dataQuery.ToList();

            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchFileTestAttachModel GetListFileTestAttachByProductId(SearchFileTestAttachModel model)
        {
            try
            {
                var product = db.Products.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ProductId));

                if (product == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                }

                var data = (from a in db.ProductTestResultAttachs.AsNoTracking()
                            join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id into ac
                            from ca in ac.DefaultIfEmpty()
                            join d in db.Employees.AsNoTracking() on ca.EmployeeId equals d.Id into ad
                            from da in ad.DefaultIfEmpty()
                            where a.ProductId.Equals(model.ProductId)
                            orderby a.FileName
                            select new FileTestAttachModel
                            {
                                Id = a.Id,
                                ProductId = a.ProductId,
                                FileName = a.FileName,
                                FilePath = a.FilePath,
                                FileSize = a.FileSize,
                                CreateBy = a.CreateBy,
                                CreateByName = da.Name,
                                CreateDate = a.CreateDate,
                            }).ToList();

                model.ListFileTestAttach = data;
                model.IsTestResult = product.IsTestResult;

                return model;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void CreateFileTestAttach(SearchFileTestAttachModel model, string userId)
        {
            var productTestAttachs = db.ProductTestResultAttachs.Where(a => a.ProductId.Equals(model.ProductId)).ToList();
            if (productTestAttachs.Count > 0)
            {
                db.ProductTestResultAttachs.RemoveRange(productTestAttachs);
            }
            if (model.ListFileTestAttach.Count > 0)
            {
                List<ProductTestResultAttach> listFileEntity = new List<ProductTestResultAttach>();

                foreach (var item in model.ListFileTestAttach)
                {
                    if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FilePath))
                    {
                        ProductTestResultAttach productTestResult = new ProductTestResultAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = model.ProductId,
                            FilePath = item.FilePath,
                            FileName = item.FileName,
                            CreateDate = DateTime.Now,
                            FileSize = item.FileSize,
                            CreateBy = userId,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };
                        listFileEntity.Add(productTestResult);
                    }
                }
                db.ProductTestResultAttachs.AddRange(listFileEntity);
                db.SaveChanges();
            }
        }

        public void CheckStatusProduct(CheckStatusModel model)
        {
            var productInfor = db.Products.FirstOrDefault(a => a.Id.Equals(model.ProductId));
            if (productInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
            }
            if (model.Status == 1)
            {
                // Đã xác nhận
                productInfor.IsTestResult = true;
            }
            else
            {
                productInfor.IsTestResult = false;
            }
            db.SaveChanges();

        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(ProductSearchModel model)
        {

            var data = SearchProduct(model, true);
            List<ProductResultModel> products = data.ListResult;

            if (products.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Products.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = products.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = products.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                    a.ProductGroupCode,
                    a.Pricing,
                    a.ErrorCount,
                    IsManualExist = a.IsManualExist ? "Đã có" : "Chưa có",
                    IsQuoteExist = a.IsQuoteExist ? "Đã có" : "Chưa có",
                    IsPracticeExist = a.IsPracticeExist ? "Đã có" : "Chưa có",
                    IsLayoutExist = a.IsLayoutExist ? "Đã có" : "Chưa có",
                    IsMaterialExist = a.IsMaterialExist ? "Đã có" : "Chưa có",
                    IsCatalog = a.IsCatalog ? "Đã có" : "Chưa có",
                    a.ProcedureTime,
                    a.CurentVersion,
                    IsTestResult = a.IsTestResult ? "Đã test" : "Chưa test",
                    Status = a.Status == "1" ? "Chỉ sử dụng một lần" : a.Status == "2" ? "Module chuẩn" : a.Status == "3" ? "Module ngừng sử dụng" : "",
                    a.DepartmentName
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách thiết bị" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách thiết bị" + ".xlsx";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public string GetContentProduct(string productId)
        {
            string content = string.Empty;
            var data = db.Products.AsNoTracking().FirstOrDefault(i => i.Id.Equals(productId));
            if (data != null)
            {
                content = data.Content;
            }
            else
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
            }
            return content;
        }

        public void UpdateContent(ProductContentModel model)
        {
            var practice = db.Products.FirstOrDefault(i => i.Id.Equals(model.ProductId));
            if (practice == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
            }
            practice.Content = model.Content;
            db.SaveChanges();
        }

        public string ImportFile(HttpPostedFile file, bool isConfirm, string userId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            string code;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<int> rowCode = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<string> listId = new List<string>();
            string confirm = string.Empty;

            try
            {
                var list = db.Products.AsNoTracking().ToList();
                for (int i = 3; i <= rowCount; i++)
                {
                    code = sheet[i, 2].Value;

                    if (!string.IsNullOrEmpty(code))
                    {
                        var data = list.FirstOrDefault(a => a.Code.Equals(code));
                        if (data != null)
                        {
                            listId.Add(data.Id);
                        }
                        else
                        {
                            rowCheckCode.Add(i);
                        }
                    }
                    else
                    {
                        rowCode.Add(i);
                    }
                }

                #endregion

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã thiết bị dòng <" + string.Join(", ", rowCode) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã thiết bị dòng <" + string.Join(", ", rowCheckCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                confirm = SyncSaleProduct(false, isConfirm, listId, userId);
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            return confirm;
        }

        public string SyncSaleProduct(bool isAll, bool isConfirm, List<string> listProductId, string userId)
        {
            ProductModel model;
            //ProductModel product;
            SaleProduct saleProduct;
            SaleProduct check;
            SaleProductMedia saleProductMedia;
            List<SaleProductMedia> saleProductMedias;
            SaleProductDocument saleProductDocument;
            List<SaleProductDocument> saleProductDocuments;
            List<SaleProduct> listSaleProduct = new List<SaleProduct>();
            List<SaleProductDocument> listSaleProductDocument = new List<SaleProductDocument>();
            List<SaleProductMedia> listSaleProductMedia = new List<SaleProductMedia>();
            ProductDocumentAttachModel productDocumentAttachModel;
            SyncSaleProduct syncSaleProduct;
            List<SyncSaleProduct> syncSaleProducts = new List<SyncSaleProduct>();
            NTS.Model.Repositories.Product product;
            List<ProductImage> listImage;
            string groupCode = string.Empty;
            string conten = $"Đồng bộ sản phẩm kinh doanh";
            var listProductGroup = db.ProductGroups.AsNoTracking().ToList();
            List<string> listCodeError = new List<string>();

            if (isAll)
            {
                listProductId = new List<string>();
                listProductId = db.Products.AsNoTracking().Select(i => i.Id).ToList();
            }

            foreach (var id in listProductId)
            {
                model = new ProductModel()
                {
                    Id = id
                };
                product = new NTS.Model.Repositories.Product();
                product = db.Products.FirstOrDefault(i => i.Id.Equals(id));
                if (product == null)
                {
                    continue;
                }
                product.SyncDate = DateTime.Now;
                product.IsSendSale = true;
                listImage = new List<ProductImage>();
                listImage = db.ProductImages.AsNoTracking().Where(i => i.ProductId.Equals(product.Id)).ToList();

                productDocumentAttachModel = new ProductDocumentAttachModel();
                productDocumentAttachModel = GetProductDocumentAttachs(model);

                groupCode = listProductGroup.FirstOrDefault(i => i.Id.Equals(product.ProductGroupId))?.Code;

                // Kiểm tra thông tin thiết bị, nếu thiếu thì cảnh báo
                if (!isConfirm && (string.IsNullOrEmpty(groupCode) ||
                    string.IsNullOrEmpty(product.Description) ||
                    product.Pricing <= 0))
                {
                    listCodeError.Add(product.Code);
                    continue;
                }

                check = new SaleProduct();
                check = db.SaleProducts.FirstOrDefault(i => i.Type == Constants.SaleProductDevice && i.SourceId.Equals(product.Id));
                if (check != null)
                {
                    check.EName = string.Empty;
                    check.VName = product.Name;
                    check.Model = product.Code;
                    check.GroupCode = groupCode;
                    check.ManufactureId = Constants.ManufactureId;
                    check.CountryName = Constants.CountryName;
                    check.Specifications = product.Description;
                    check.CustomerSpecifications = product.Description;
                    check.MaterialPrice = product.Pricing;
                    check.SourceId = product.Id;
                    check.IsSync = true;
                    check.Status = true;
                    check.Type = Constants.SaleProductDevice;
                    check.UpdateBy = userId;
                    check.UpdateDate = DateTime.Now;

                    if (listImage.Count > 0)
                    {
                        check.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProduct,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    saleProductMedias = new List<SaleProductMedia>();
                    saleProductMedias = db.SaleProductMedias.Where(i => i.SaleProductId.Equals(check.Id) && i.Type == Constants.SaleProductMedia_Type_LibraryImage).ToList();

                    saleProductDocuments = new List<SaleProductDocument>();
                    saleProductDocuments = db.SaleProductDocuments.Where(i => i.SaleProductId.Equals(check.Id) && (i.Type == Constants.SaleProductDocument_Type_Catalog || i.Type == Constants.SaleProductDocument_Type_TechnicalTraining || i.Type == Constants.SaleProductDocument_Type_UserManual)).ToList();

                    db.SaleProductMedias.RemoveRange(saleProductMedias);
                    db.SaleProductDocuments.RemoveRange(saleProductDocuments);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };

                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    // Catalog
                    foreach (var item in productDocumentAttachModel.ListFileCatalog)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_Catalog
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in productDocumentAttachModel.ListGuidePractice)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Danh mục bài thực hành
                    foreach (var item in productDocumentAttachModel.ListDMBTH)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Tài liệu hướng dẫn bảo trì
                    foreach (var item in productDocumentAttachModel.ListGuideMaintenance)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in productDocumentAttachModel.ListGuidePractice)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_UserManual
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncProduct, check.Id, check.Model, conten);
                }
                else
                {
                    saleProduct = new SaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EName = string.Empty,
                        VName = product.Name,
                        Model = product.Code,
                        GroupCode = groupCode,
                        ManufactureId = Constants.ManufactureId,
                        CountryName = Constants.CountryName,
                        Specifications = product.Description,
                        CustomerSpecifications = product.Description,
                        MaterialPrice = product.Pricing,
                        SourceId = product.Id,
                        Type = Constants.SaleProductDevice,
                        IsSync = true,
                        Status = true,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        UpdateDate = DateTime.Now,
                        UpdateBy = userId,
                    };

                    if (listImage.Count > 0)
                    {
                        saleProduct.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    listSaleProduct.Add(saleProduct);

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProduct,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    // Catalog
                    foreach (var item in productDocumentAttachModel.ListFileCatalog)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_Catalog
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in productDocumentAttachModel.ListGuidePractice)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Danh mục bài thực hành
                    foreach (var item in productDocumentAttachModel.ListDMBTH)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Tài liệu hướng dẫn bảo trì
                    foreach (var item in productDocumentAttachModel.ListGuideMaintenance)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_TechnicalTraining
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in productDocumentAttachModel.ListGuidePractice)
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            CreateDate = item.CreateDate.Value,
                            Type = Constants.SaleProductDocument_Type_UserManual
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncProduct, saleProduct.Id, saleProduct.Model, conten);
                }
            }

            if (listCodeError.Count > 0)
            {
                return "Mã thiết bị <" + string.Join(", ", listCodeError) + "> đang thiếu thông tin đồng bộ. Bạn có muốn tiếp tục đồng bộ!";
            }

            using (var trans = db.Database.BeginTransaction())
            {
                db.SaleProducts.AddRange(listSaleProduct);
                db.SyncSaleProducts.AddRange(syncSaleProducts);
                db.SaleProductMedias.AddRange(listSaleProductMedia);
                db.SaleProductDocuments.AddRange(listSaleProductDocument);
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }

            return null;
        }

        public List<ProductSearchModel> GetProductNeedPublications(ProductSearchModel model)
        {
            var product = db.Products.AsNoTracking().Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
            var projectProduct = db.ProjectProducts.AsNoTracking().Where(r => r.ProductId.Equals(product.Id)).ToList();
            string projectId = "" ;
            if (model.Publications == 1)
            {
                projectProduct = projectProduct.Where(r => r.IsCatalogRequire == true).ToList();
                foreach (var item in projectProduct)
                {
                    projectId = string.Join(",", item.ProjectId);
                }
            }
            if (model.Publications == 2)
            {
                projectProduct = projectProduct.Where(r => r.IsUserGuideRequire == true).ToList();
                foreach (var item in projectProduct)
                {
                    projectId = string.Join(",", item.ProjectId);
                }
            }
            if (model.Publications == 3)
            {
                projectProduct = projectProduct.Where(r => r.IsMaintenaineGuideRequire == true).ToList();
                foreach (var item in projectProduct)
                {
                    projectId = string.Join(",", item.ProjectId);
                }
            }
            if (model.Publications == 4)
            {
                projectProduct = projectProduct.Where(r => r.IsPracticeGuideRequire == true).ToList();
                foreach (var item in projectProduct)
                {
                    projectId = string.Join(",", item.ProjectId);
                }
            }

            var project = (from a in db.Projects.AsNoTracking()
                           where projectId.ToUpper().Contains(a.Id.ToUpper())
                           orderby a.Code
                           select new ProductSearchModel
                           {
                               Id = a.Id,
                               Code = a.Code,
                               Name = a.Name,
                           }).AsQueryable();
            var modules = project.ToList();

            return modules;
        }
    }
}
