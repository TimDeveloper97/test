using System;
using NTS.Model.ProductStandardGroup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Repositories;
using NTS.Model.Combobox;
using NTS.Model;
using NTS.Common;
using NTS.Common.Resource;

namespace QLTK.Business.ProductStandardGroups
{
    public class ProductStandardGroupBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ProductStandardGroupModel> SearchProductStandardGroup(ProductStandardGroupSearchModel modelSearch)
        {
            SearchResultModel<ProductStandardGroupModel> searchResult = new SearchResultModel<ProductStandardGroupModel>();

            var dataQuery = (from a in db.ProductStandardGroups.AsNoTracking()
                             orderby a.Code
                             select new ProductStandardGroupModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList(); /*SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();*/
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddProductStandardGroup(ProductStandardGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProductStandardGroup newProductStandardGroup = new ProductStandardGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Note = model.Note,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.ProductStandardGroups.Add(newProductStandardGroup);
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
        public void UpdateProductStandardGroup(ProductStandardGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newProductStandardGroup = db.ProductStandardGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newProductStandardGroup.Name = model.Name.Trim();
                    newProductStandardGroup.Code = model.Code.Trim();
                    newProductStandardGroup.Note = model.Note.Trim();
                    newProductStandardGroup.UpdateBy = model.UpdateBy;
                    newProductStandardGroup.UpdateDate = DateTime.Now;

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

        public void DeleteProductStandardGroup(ProductStandardGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var productStandardGroup = db.ProductStandards.AsNoTracking().Where(m => m.ProductStandardGroupId.Equals(model.Id)).FirstOrDefault();
                if (productStandardGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandardGroup);
                }

                try
                {
                    var productstandardGroup = db.ProductStandardGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (productstandardGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandardGroup);
                    }

                    db.ProductStandardGroups.Remove(productstandardGroup);
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
        public object GetProductStandardGroupInfo(ProductStandardGroupModel model)
        {
            var resultInfo = db.ProductStandardGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProductStandardGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandardGroup);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(ProductStandardGroupModel model)
        {
            if (db.ProductStandardGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandardGroup);
            }

            if (db.ProductStandardGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandardGroup);
            }
        }

        public void CheckExistedForUpdate(ProductStandardGroupModel model)
        {
            if (db.ProductStandardGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandardGroup);
            }

            if (db.ProductStandardGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandardGroup);
            }
        }
    }
}
