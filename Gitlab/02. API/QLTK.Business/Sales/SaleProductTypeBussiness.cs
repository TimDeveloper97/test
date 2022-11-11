using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProductStandardTPAs;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using NTS.Model.Unit;
using NTS.Model.UnitHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SaleProducts
{
    public class SaleProductTypeBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public void DeleteType(SaleProductTypeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.SaleProducts.AsNoTracking().Where(r => !string.IsNullOrEmpty(r.SaleProductTypeId) && r.SaleProductTypeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandardTPAType);
                }

                try
                {
                    var unit = db.SaleProductTypes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (unit == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    }

                    var types = db.SaleProductTypes.ToList();

                    RemoveChild(types, unit.Id);

                    db.SaleProductTypes.Remove(unit);

                    var NameOrCode = unit.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SaleProductTypeHistoryModel>(unit);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_SaleProductTypes, unit.Id, NameOrCode, jsonBefor);

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

        private void RemoveChild(List<SaleProductType> types, string parrentId)
        {
            var childs = types.Where(r => parrentId.Equals(r.ParentId)).ToList();

            foreach (var item in childs)
            {
                if (db.SaleProducts.AsNoTracking().Where(r => r.SaleProductTypeId.Equals(item.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandardTPAType);
                }

                RemoveChild(types, item.Id);

                db.SaleProductTypes.Remove(item);
            }
        }

        public void AddType(SaleProductTypeModel model)
        {
            if (db.SaleProductTypes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }


            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    SaleProductType newUnit = new SaleProductType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Note = model.Note.Trim(),
                        EXWRate = model.EXWRate,
                        PublicRate = model.PublicRate,
                        Index = model.Index,
                        ParentId = model.ParentId,
                        SBUId = model.SBUId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        Code = !string.IsNullOrEmpty(model.Code) ? model.Code.Trim() : string.Empty
                    };

                    if (string.IsNullOrEmpty(newUnit.ParentId))
                    {
                        newUnit.ParentId = null;
                    }

                    db.SaleProductTypes.Add(newUnit);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newUnit.Name, newUnit.Id, Constants.LOG_SaleProductTypes);

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

        public object GetTypeInfo(SaleProductTypeModel model)
        {
            var resultInfo = db.SaleProductTypes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SaleProductTypeModel
            {
                Id = p.Id,
                Note = p.Note,
                Name = p.Name,
                EXWRate = p.EXWRate,
                PublicRate = p.PublicRate,
                Index = p.Index,
                ParentId = p.ParentId,
                Code = p.Code,
                SBUId  = p.SBUId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandardTPAType);
            }

            return resultInfo;
        }

        public void UpdateType(SaleProductTypeModel model)
        {
            if (db.SaleProductTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newUnit = db.SaleProductTypes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SaleProductTypeHistoryModel>(newUnit);

                    newUnit.ParentId = model.ParentId;
                    newUnit.Name = model.Name.Trim();
                    newUnit.Code = !string.IsNullOrEmpty(model.Code) ? model.Code.Trim() : string.Empty;
                    newUnit.EXWRate = model.EXWRate;
                    newUnit.PublicRate = model.PublicRate;
                    newUnit.Note = model.Note;
                    newUnit.SBUId = model.SBUId;
                    newUnit.Index = model.Index;
                    newUnit.UpdateBy = model.UpdateBy;
                    newUnit.UpdateDate = DateTime.Now;

                    if (string.IsNullOrEmpty(newUnit.ParentId))
                    {
                        newUnit.ParentId = null;
                    }

                    var listProduct = db.SaleProducts.Where(t => t.SaleProductTypeId.Equals(model.Id)).ToList();
                    if (listProduct.Count > 0)
                    {
                        foreach (var item in listProduct)
                        {
                            item.EXWTPAPrice = item.MaterialPrice * newUnit.EXWRate;
                            item.PublicPrice = item.MaterialPrice * newUnit.PublicRate;
                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SaleProductTypeHistoryModel>(newUnit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Unit, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);

                    var saleProducts = db.SaleProducts.Where(r => r.SaleProductTypeId.Equals(newUnit.Id));

                    foreach (var item in saleProducts)
                    {
                        item.EXWTPAPrice = newUnit.EXWRate * item.MaterialPrice;
                        item.PublicPrice = newUnit.PublicRate * item.MaterialPrice;
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

        public List<SaleProductTypeModel> GetListType()
        {
            var rs = (from a in db.SaleProductTypes.AsNoTracking()
                      orderby a.Index
                      select new SaleProductTypeModel
                      {
                          Id = a.Id,
                          Name = a.Name,
                          Code = a.Code,
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
