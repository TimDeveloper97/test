using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
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

namespace QLTK.Business.Units
{
    public class UnitBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public object SearchUnit(UnitSearchModel modelSearch)
        {
            SearchResultModel<UnitModel> searchResult = new SearchResultModel<UnitModel>();

            var dataQuery = (from a in db.Units.AsNoTracking()
                             orderby a.Index
                             select new UnitModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Index = a.Index,
                                 Description = a.Description,
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
            dataQuery = dataQuery.OrderBy(t => t.Index);
            var listResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public void DeleteUnit(UnitModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Materials.AsNoTracking().Where(r => r.UnitId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Unit);
                }

                var maxIndex = db.Units.AsNoTracking().Select(a => a.Index).Max();
                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listUnit = db.Units.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listUnit.Count() > 0 && listUnit != null)
                    {
                        foreach (var item in listUnit)
                        {
                            if (!item.Id.Equals(model.Id))
                            {
                                var updateUnit = db.Units.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                                updateUnit.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var unit = db.Units.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (unit == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    }
                    db.Units.Remove(unit);

                    var NameOrCode = unit.Name;
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

        public void AddUnit(UnitModel model)
        {
            if (db.Units.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }


            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.Units.ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listUnit = db.Units.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listUnit.Count() > 0 && listUnit != null)
                    {
                        foreach (var item in listUnit)
                        {
                            var updateUnit = db.Units.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateUnit.Index++;
                        }
                    }
                }
                try
                {
                    Unit newUnit = new Unit
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        Index = model.Index,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,

                    };
                    db.Units.Add(newUnit);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newUnit.Name, newUnit.Id, Constants.LOG_Unit);

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

        public object GetUnitInfo(UnitModel model)
        {
            var resultInfo = db.Units.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new UnitModel
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                Index = p.Index
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
            }

            return resultInfo;
        }

        public void UpdateUnit(UnitModel model)
        {
            if (db.Units.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Unit);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkUnit = db.Units.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkUnit != null)
                    {
                        var newUnit = db.Units.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<UnitHistoryModel>(newUnit);


                        int olđInext = newUnit.Index;
                        if (checkUnit.Index < newUnit.Index)
                        {
                            var listUnitChange1 = db.Units.Where(a => a.Index > checkUnit.Index && a.Index < newUnit.Index);
                            if (listUnitChange1.Count() > 0)
                            {
                                foreach (var item in listUnitChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkUnit.Index++;
                        }

                        if (checkUnit.Index > newUnit.Index)
                        {
                            var listUnitChange = db.Units.Where(a => a.Index > newUnit.Index && a.Index < checkUnit.Index);
                            if (listUnitChange.ToList().Count() > 0)
                            {
                                foreach (var item in listUnitChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkUnit.Index = checkUnit.Index - 1;
                        }
                        newUnit.Index = model.Index;
                        newUnit.Name = model.Name.Trim();
                        newUnit.Description = model.Description.Trim();
                        newUnit.UpdateBy = model.UpdateBy;
                        newUnit.UpdateDate = DateTime.Now;

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<UnitHistoryModel>(newUnit);

                        //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Unit, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);
                    }
                    else
                    {
                        var newUnit = db.Units.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<UnitHistoryModel>(newUnit);

                        var listUnit = (from a in db.Units.AsNoTracking()
                                        orderby a.Index ascending
                                        select new UnitModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Index = a.Index
                                        }).AsQueryable();
                        if (newUnit.Index == 1 && listUnit.Count() == 1 && !model.Index.Equals("1"))
                        {
                            throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                        }
                        newUnit.Name = model.Name.Trim();
                        newUnit.Description = model.Description.Trim();
                        newUnit.Index = model.Index;
                        newUnit.UpdateBy = model.UpdateBy;
                        newUnit.UpdateDate = DateTime.Now;

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<UnitHistoryModel>(newUnit);

                        //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Unit, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);

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

        public List<UnitModel> GetListUnit()
        {
            var rs = (from a in db.Units.AsNoTracking()
                      orderby a.Index
                      select new UnitModel
                      {
                          Id = a.Id,
                          Name = a.Name,
                          Index = a.Index,
                          Description = a.Description,
                          CreateDate = a.CreateDate,
                          CreateBy = a.CreateBy,
                          UpdateBy = a.UpdateBy,
                          UpdateDate = a.UpdateDate,
                      }).ToList();
            return rs;
        }
    }
}
