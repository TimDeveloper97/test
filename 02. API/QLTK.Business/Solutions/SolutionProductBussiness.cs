using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SolutionProducts;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SolutionProduct
{
    public class SolutionProductBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterials.ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private readonly ProductBusiness productBusiness = new ProductBusiness();

        public object SearchSolutionProduct(SolutionProductSearchModel modelSearch)
        {
            SearchResultModel<SolutionProductSearchResultsModel> searchResult = new SearchResultModel<SolutionProductSearchResultsModel>();

            var dataQuery = (from a in db.SolutionProducts.AsNoTracking()
                             join b in db.Solutions.AsNoTracking() on a.SolutionId equals b.Id
                             join c in db.Modules.AsNoTracking() on a.ObjectId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join p in db.Products.AsNoTracking() on a.ObjectId equals p.Id into ap
                             from apn in ap.DefaultIfEmpty()
                             where a.SolutionId.Equals(modelSearch.SolutionId)
                             orderby a.Code
                             select new SolutionProductSearchResultsModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Price = a.Price,
                                 Quantity = a.Quantity,
                                 ObjectName = a.ObjectType == Constants.Solution_ObjectType_Module ? ca.Name : apn.Name,
                                 ObjectCode = a.ObjectType == Constants.Solution_ObjectType_Module ? ca.Code : apn.Code,
                                 Specification = a.Specification,
                                 SolutionId = a.SolutionId,
                             }).AsQueryable();

            decimal sumTotalPrice;
            if (dataQuery.ToList().Count > 0)
            {
                sumTotalPrice = dataQuery.Sum(a => a.Quantity * a.Price);
            }
            else
            {
                sumTotalPrice = 0;
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(a => a.Code.ToLower().Contains(modelSearch.Code.ToLower()) || a.Name.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return new { searchResult, sumTotalPrice };
        }

        public SolutionProductModel GetByIdSolutionProduct(string solutionProductId)
        {
            var resultInfo = db.SolutionProducts.AsNoTracking().Where(u => u.Id.Equals(solutionProductId)).Select(p => new SolutionProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Quantity = p.Quantity,
                Price = p.Price,
                ObjectId = p.ObjectId,
                ObjectType = p.ObjectType,
                Specification = p.Specification,
                SolutionId = p.SolutionId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionProduct);
            }

            return resultInfo;
        }

        public void CreateSolutionProduct(SolutionProductModel model, string userId)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            CheckExistedForAdd(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.SolutionProduct newSoulutinProduct = new NTS.Model.Repositories.SolutionProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        ObjectType = model.ObjectType,
                        ObjectId = model.ObjectId,
                        SolutionId = model.SolutionId,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        CreateBy = userId,
                        Specification = model.Specification,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    db.SolutionProducts.Add(newSoulutinProduct);
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

        public void UpdateSolutionProduct(SolutionProductModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            CheckExistedForUpdate(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSolutionProduct = db.SolutionProducts.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    newSolutionProduct.Name = model.Name.Trim();
                    newSolutionProduct.Code = model.Code.Trim();
                    newSolutionProduct.Price = model.Price;
                    newSolutionProduct.Quantity = model.Quantity;
                    newSolutionProduct.Specification = model.Specification;
                    newSolutionProduct.SolutionId = model.SolutionId;
                    newSolutionProduct.ObjectId = model.ObjectId;
                    newSolutionProduct.ObjectType = model.ObjectType;
                    newSolutionProduct.UpdateBy = model.UpdateBy;
                    newSolutionProduct.UpdateDate = DateTime.Now;
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

        public void DeleteSolutionProduct(string solutionProductId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var solutinProduct = db.SolutionProducts.FirstOrDefault(a => a.Id.Equals(solutionProductId));
                    if (solutinProduct == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionProduct);
                    }

                    db.SolutionProducts.Remove(solutinProduct);
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

        private void CheckExistedForAdd(SolutionProductModel model)
        {
            if (db.SolutionProducts.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SolutionProduct);
            }

            if (db.SolutionProducts.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SolutionProduct);
            }
        }

        public void CheckExistedForUpdate(SolutionProductModel model)
        {
            if (db.Specializes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SolutionProduct);
            }

            if (db.Specializes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SolutionProduct);
            }
        }

        public object GetObjectInfo(SolutionProductModel objectModel)
        {
            SolutionProductModel objectInfo = new SolutionProductModel();
            if (objectModel.ObjectType == Constants.Solution_ObjectType_Module)
            {
                var module = db.Modules.AsNoTracking().FirstOrDefault(a => a.Id.Equals(objectModel.ObjectId));
                if (module != null)
                {
                    objectInfo.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(module.Id, 0);
                    objectInfo.Name = module.Name;
                    objectInfo.Code = module.Code;
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                }
            }
            else if (objectModel.ObjectType == Constants.Solution_ObjectType_Product)
            {
                var product = db.Products.AsNoTracking().FirstOrDefault(a => a.Id.Equals(objectModel.ObjectId));
                if (product != null)
                {
                    objectInfo.Price = productBusiness.GetProductPrice(product.Id);
                    objectInfo.Name = product.Name;
                    objectInfo.Code = product.Code;
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                }
            }

            return objectInfo;
        }

    }
}
