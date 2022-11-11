using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProductStandardTPAs;
using NTS.Model.Repositories;
using NTS.Model.Unit;
using NTS.Model.UnitHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ProductStandardTPAs
{
    public class ProductStandardTPATypeBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public object SearchType(ProductStandardTPATypeSearchModel modelSearch)
        {
            SearchResultModel<ProductStandardTPATypeModel> searchResult = new SearchResultModel<ProductStandardTPATypeModel>();

            var dataQuery = (from a in db.ProductStandardTPATypes.AsNoTracking()
                             orderby a.Name
                             select new ProductStandardTPATypeModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Index = a.Index,
                                 ParentId = a.ParentId,
                                 Note = a.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            //var dataProductStandardType = dataQuery.Where(a => a.ParentId == null).OrderBy(t => t.Index);
            var listResult = dataQuery.ToList();

            //foreach (var item in listResult)
            //{
            //    item.ListProductStandardTPATypeParent = dataQuery.Where(a => a.ParentId.Equals(item.Id)).OrderBy(a => a.Index).ToList();
            //}

            searchResult.ListResult = listResult;

            return searchResult;
        }

        public void DeleteType(ProductStandardTPATypeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                if (db.ProductStandardTPAs.AsNoTracking().Where(r => r.ProductStandardTPATypeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandardTPAType);
                }

                //if (!string.IsNullOrEmpty(model.ParentId))
                //{
                //    var Indexs = db.ProductStandardTPATypes.Where(b=> b.ParentId.Equals(model.ParentId)).ToList();
                //    var maxIndex = Indexs.Select(x => x.Index).Max();
                //    if (model.Index <= maxIndex)
                //    {
                //        int modelIndex = model.Index;
                //        var listUnit = Indexs.Where(b => b.Index >= modelIndex);
                //        if (listUnit.Count() > 0 && listUnit != null)
                //        {
                //            foreach (var item in listUnit)
                //            {
                //                if (!item.Id.Equals(model.Id))
                //                {
                //                    var updateUnit = db.ProductStandardTPATypes.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                //                    updateUnit.Index--;
                //                }

                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    var Indexs = db.ProductStandardTPATypes.AsNoTracking().ToList();
                //    var maxIndex = Indexs.Select(x => x.Index).Max();
                //    if (model.Index <= maxIndex)
                //    {
                //        int modelIndex = model.Index;
                //        var listUnit = Indexs.Where(b => b.Index >= modelIndex);
                //        if (listUnit.Count() > 0 && listUnit != null)
                //        {
                //            foreach (var item in listUnit)
                //            {
                //                if (!item.Id.Equals(model.Id))
                //                {
                //                    var updateUnit = db.ProductStandardTPATypes.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                //                    updateUnit.Index--;
                //                }

                //            }
                //        }
                //    }
                //}

                try
                {
                    var unit = db.ProductStandardTPATypes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (unit == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    }

                    var types = db.ProductStandardTPATypes.ToList();

                    removeChild(types, unit.Id);

                    db.ProductStandardTPATypes.Remove(unit);

                    var NameOrCode = unit.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ProductStandardTPATypeHistoryModel>(unit);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_ProductStandardTPATypes, unit.Id, NameOrCode, jsonBefor);

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

        private void removeChild(List<ProductStandardTPAType> types, string parrentId)
        {
            var childs = types.Where(r => parrentId.Equals(r.ParentId)).ToList();

            foreach (var item in childs)
            {
                if (db.ProductStandardTPAs.AsNoTracking().Where(r => r.ProductStandardTPATypeId.Equals(item.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandardTPAType);
                }

                removeChild(types, item.Id);

                db.ProductStandardTPATypes.Remove(item);
            }
        }

        public void AddType(ProductStandardTPATypeModel model)
        {
            if (db.ProductStandardTPATypes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandardTPAType);
            }


            using (var trans = db.Database.BeginTransaction())
            {
                //if (!string.IsNullOrEmpty(model.ParentId))
                //{
                //    var indexs = db.ProductStandardTPATypes.Where(a => a.ParentId.Equals(model.ParentId)).ToList();
                //    var maxIndex = 1;
                //    if (indexs.Count > 0)
                //    {
                //        maxIndex = indexs.Select(a => a.Index).Max();
                //    }

                //    if (model.Index <= maxIndex)
                //    {
                //        int modelIndex = model.Index;
                //        var listUnit = indexs.Where(b => b.Index >= modelIndex).ToList();
                //        if (listUnit.Count() > 0 && listUnit != null)
                //        {
                //            foreach (var item in listUnit)
                //            {
                //                var updateUnit = db.ProductStandardTPATypes.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                //                updateUnit.Index++;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    var indexs = db.ProductStandardTPATypes.ToList();

                //    var maxIndex = 1;
                //    if (indexs.Count > 0)
                //    {
                //        maxIndex = indexs.Select(a => a.Index).Max();
                //    }

                //    if (model.Index <= maxIndex)
                //    {
                //        int modelIndex = model.Index;
                //        var listUnit = indexs.Where(b => b.Index >= modelIndex).ToList();
                //        if (listUnit.Count() > 0 && listUnit != null)
                //        {
                //            foreach (var item in listUnit)
                //            {
                //                var updateUnit = db.ProductStandardTPATypes.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                //                updateUnit.Index++;
                //            }
                //        }
                //    }
                //}

                try
                {
                    ProductStandardTPAType newUnit = new ProductStandardTPAType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Note = model.Note.Trim(),
                        Index = model.Index,
                        ParentId = model.ParentId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    if (string.IsNullOrEmpty(newUnit.ParentId))
                    {
                        newUnit.ParentId = null;
                    }

                    db.ProductStandardTPATypes.Add(newUnit);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newUnit.Name, newUnit.Id, Constants.LOG_ProductStandardTPATypes);

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

        public object GetTypeInfo(ProductStandardTPATypeModel model)
        {
            var resultInfo = db.ProductStandardTPATypes.Where(u => model.Id.Equals(u.Id)).Select(p => new ProductStandardTPATypeModel
            {
                Id = p.Id,
                Note = p.Note,
                Name = p.Name,
                Index = p.Index,
                ParentId = p.ParentId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandardTPAType);
            }

            return resultInfo;
        }

        public void UpdateType(ProductStandardTPATypeModel model)
        {
            if (db.ProductStandardTPATypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newUnit = db.ProductStandardTPATypes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ProductStandardTPATypeHistoryModel>(newUnit);

                    newUnit.ParentId = model.ParentId;
                    newUnit.Index = model.Index;
                    newUnit.Name = model.Name.Trim();
                    newUnit.Note = model.Note.Trim();
                    newUnit.UpdateBy = model.UpdateBy;
                    newUnit.UpdateDate = DateTime.Now;

                    if (string.IsNullOrEmpty(newUnit.ParentId))
                    {
                        newUnit.ParentId = null;
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ProductStandardTPATypeHistoryModel>(newUnit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Unit, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);

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

        public List<ProductStandardTPATypeModel> GetListType()
        {
            var rs = (from a in db.ProductStandardTPATypes.AsNoTracking()
                      orderby a.Index
                      select new ProductStandardTPATypeModel
                      {
                          Id = a.Id,
                          Name = a.Name,
                          Index = a.Index,
                          ParentId = a.ParentId,
                          Note = a.Note,
                          CreateDate = a.CreateDate,
                          CreateBy = a.CreateBy,
                          UpdateBy = a.UpdateBy,
                          UpdateDate = a.UpdateDate,
                      }).ToList();
            return rs;
        }
    }
}
