using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.RawMaterial;
using NTS.Model.RawMaterialHistory;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.RawMaterials
{
    public class RawMaterialBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public object SearchRawMaterial(RawMaterialSearchModel modelSearch)
        {
            SearchResultModel<RawMaterialModel> searchResult = new SearchResultModel<RawMaterialModel>();
            var dataQuery = (from a in db.RawMaterials.AsNoTracking()
                             join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id into ab
                             from ba in ab.DefaultIfEmpty()
                             orderby a.Index
                             select new RawMaterialModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Index = a.Index.ToString(),
                                 Note = a.Note,
                                 MaterialCode = ba.Code,
                                 MaterialId = ba.Id,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.MaterialId))
            {
                dataQuery = dataQuery.Where(u => u.MaterialId.Equals(modelSearch.MaterialId));
            }
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public void DeleteRawMaterial(RawMaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var maxIndex = db.RawMaterials.AsNoTracking().Select(a => a.Index).Max();
                if (Int32.Parse(model.Index) <= maxIndex)
                {
                    int modelIndex = Int32.Parse(model.Index);
                    var listRawMaterial = db.RawMaterials.Where(b => b.Index >= modelIndex).ToList();
                    if (listRawMaterial.Count() > 0 && listRawMaterial != null)
                    {
                        foreach (var item in listRawMaterial)
                        {
                            if (!item.Id.Equals(model.Id))
                            {
                                var updateRawMaterial = db.RawMaterials.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                                updateRawMaterial.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var rawMaterial = db.RawMaterials.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (rawMaterial == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RawMaterial);
                    }
                    db.RawMaterials.Remove(rawMaterial);

                    var NameOrCode = rawMaterial.Code;


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<RawMaterialHistoryModel>(rawMaterial);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_RawMaterial, rawMaterial.Id, NameOrCode, jsonBefor);

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

        public object GetRawMaterialInfo(RawMaterialModel model)
        {
            var resultInfo = db.RawMaterials.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new RawMaterialModel
            {
                Id = p.Id,
                Note = p.Note,
                Name = p.Name,
                MaterialId = p.MaterialId,
                Code = p.Code,
                Index = p.Index.ToString(),
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RawMaterial);
            }

            return resultInfo;
        }

        public void AddRawMaterial(RawMaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.RawMaterials.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RawMaterial);
                }

                if (db.RawMaterials.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.RawMaterial);
                }

                var maxIndex = db.RawMaterials.AsNoTracking().Select(a => a.Index).Max();
                if (Int32.Parse(model.Index) <= maxIndex)
                {
                    int modelIndex = Int32.Parse(model.Index);
                    var listRawMaterial = db.RawMaterials.Where(b => b.Index >= modelIndex).ToList();
                    if (listRawMaterial.Count() > 0 && listRawMaterial != null)
                    {
                        foreach (var item in listRawMaterial)
                        {
                            var updateRawMaterial = db.RawMaterials.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateRawMaterial.Index++;
                        }
                    }
                }


                try
                {
                    RawMaterial newRawMaterial = new RawMaterial
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Code = model.Code,
                        MaterialId = model.MaterialId,
                        Note = model.Note,
                        Index = Int32.Parse(model.Index),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,

                    };
                    db.RawMaterials.Add(newRawMaterial);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, newRawMaterial.Id, Constants.LOG_RawMaterial);

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

        public void UpdateRawMaterial(RawMaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.RawMaterials.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.RawMaterial);
                }

                if (db.RawMaterials.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.RawMaterial);
                }

                try
                {
                    var checkRawMaterial = db.RawMaterials.Where(b => b.Index.ToString().Equals(model.Index)).FirstOrDefault();
                    if (checkRawMaterial != null)
                    {
                        var newRawMaterial = db.RawMaterials.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<RawMaterialHistoryModel>(newRawMaterial);


                        //int olđInext = newRawMaterial.Index;
                        if (checkRawMaterial.Index < newRawMaterial.Index)
                        {
                            var listRawMaterialChange = db.RawMaterials.Where(a => a.Index > checkRawMaterial.Index && a.Index < newRawMaterial.Index);
                            if (listRawMaterialChange.Count() > 0)
                            {
                                foreach (var item in listRawMaterialChange)
                                {
                                    item.Index++;
                                }

                            }
                            checkRawMaterial.Index++;
                        }

                        if (checkRawMaterial.Index > newRawMaterial.Index)
                        {
                            var listRawMaterialChange = db.RawMaterials.Where(a => a.Index > newRawMaterial.Index && a.Index < checkRawMaterial.Index);
                            if (listRawMaterialChange.ToList().Count() > 0)
                            {
                                foreach (var item in listRawMaterialChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkRawMaterial.Index = checkRawMaterial.Index - 1;
                        }

                        newRawMaterial.Index = Int32.Parse(model.Index);
                        newRawMaterial.Name = model.Name;
                        newRawMaterial.Code = model.Code;
                        newRawMaterial.MaterialId  = model.MaterialId;
                        newRawMaterial.Note = model.Note;
                        newRawMaterial.UpdateBy = model.UpdateBy;
                        newRawMaterial.UpdateDate = DateTime.Now;

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<RawMaterialHistoryModel>(newRawMaterial);

                        //UserLogUtil.LogHistotyUpdateInfo(db, newRawMaterial.UpdateBy, Constants.LOG_RawMaterial, newRawMaterial.Id, newRawMaterial.Code, jsonBefor, jsonApter);

                    }
                    else
                    {
                        var newRawMaterial = db.RawMaterials.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<RawMaterialHistoryModel>(newRawMaterial);

                        var listRawMaterial = (from a in db.RawMaterials.AsNoTracking()
                                               orderby a.Index ascending
                                               select new RawMaterialModel
                                               {
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Index = a.Index.ToString()
                                               }).AsQueryable();

                        if (newRawMaterial.Index == 1 && listRawMaterial.Count() == 1 && !model.Index.Equals("1"))
                        {
                            throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                        }

                        newRawMaterial.Name = model.Name;
                        newRawMaterial.Note = model.Note;
                        newRawMaterial.Index = Int32.Parse(model.Index);
                        newRawMaterial.UpdateBy = model.UpdateBy;
                        newRawMaterial.UpdateDate = DateTime.Now;

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<RawMaterialHistoryModel>(newRawMaterial);

                        //UserLogUtil.LogHistotyUpdateInfo(db, newRawMaterial.UpdateBy, Constants.LOG_RawMaterial, newRawMaterial.Id, newRawMaterial.Code, jsonBefor, jsonApter);
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

        public List<RawMaterialModel> GetListRawMaterial()
        {
            List<RawMaterialModel> searchResult = new List<RawMaterialModel>();
            searchResult = (from a in db.RawMaterials.AsNoTracking()
                             orderby a.Index
                             select new RawMaterialModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 MaterialId = a.MaterialId,
                                 Index = a.Index.ToString(),
                                 Note = a.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).ToList();

            return searchResult;
        }
    }
}
