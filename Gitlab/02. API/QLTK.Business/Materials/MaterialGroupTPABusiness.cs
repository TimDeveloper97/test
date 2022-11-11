using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.MaterialGroupTPA;
using NTS.Model.Repositories;
using NTS.Common.Resource;
using System;
using System.Linq;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;

namespace QLTK.Business.MaterialGroupTPAs
{
    public class MaterialGroupTPABusiness
    {
        private QLTKEntities db = new QLTKEntities();
        public object SearchMaterialGroupTPA(MaterialGroupTPASearchModel modelSearch)
        {
            SearchResultModel<MaterialGroupTPAModel> searchResult = new SearchResultModel<MaterialGroupTPAModel>();

            var dataQuery = (from a in db.MaterialGroupTPAs.AsNoTracking()
                             orderby a.Code
                             select new MaterialGroupTPAModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
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
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public void DeleteMaterialGroupTPA(MaterialGroupTPAModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var materialGroup = db.MaterialGroups.AsNoTracking().Where(m => m.MaterialGroupTPAId.Equals(model.Id)).FirstOrDefault();

                var material = db.Materials.AsNoTracking().Where(m => m.MaterialGroupTPAId.Equals(model.Id)).FirstOrDefault();

                if (materialGroup != null || material != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MaterialGroupTPA);
                }

                try
                {
                    var materialGroupTPA = db.MaterialGroupTPAs.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (materialGroupTPA == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialGroupTPA);
                    }

                    db.MaterialGroupTPAs.Remove(materialGroupTPA);

                    var CodeOrName = materialGroupTPA.Code;
                    //var jsonBefor = AutoMapperConfig.Mapper.Map<MarterialGroupTPAHistoryModel>(materialGroupTPA);

                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_MaterialGroupTPa, materialGroupTPA.Id, CodeOrName, jsonBefor);

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

        public object GetMaterialGroupTPAInfo(MaterialGroupTPAModel model)
        {
            var resultInfo = db.MaterialGroupTPAs.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new MaterialGroupTPAModel
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                Code = p.Code
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialGroupTPA);
            }

            return resultInfo;
        }

        public void AddMaterialGroupTPA(MaterialGroupTPAModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            CheckExistedForAdd(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    MaterialGroupTPA newMaterialGroupTPA = new MaterialGroupTPA
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.MaterialGroupTPAs.Add(newMaterialGroupTPA);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, newMaterialGroupTPA.Id, Constants.LOG_MaterialGroupTPa);

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

        private void CheckExistedForAdd(MaterialGroupTPAModel model)
        {
            if (db.MaterialGroupTPAs.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MaterialGroupTPA);
            }

            if (db.MaterialGroupTPAs.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MaterialGroupTPA);
            }
        }

        public void UpdateMaterialGroupTPA(MaterialGroupTPAModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            CheckExistedForUpdate(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newMaterialGroupTPA = db.MaterialGroupTPAs.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<MarterialGroupTPAHistoryModel>(newMaterialGroupTPA);

                    newMaterialGroupTPA.Name = model.Name;
                    newMaterialGroupTPA.Description = model.Description;
                    newMaterialGroupTPA.Code = model.Code;
                    newMaterialGroupTPA.UpdateBy = model.UpdateBy;
                    newMaterialGroupTPA.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<MarterialGroupTPAHistoryModel>(newMaterialGroupTPA);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_MaterialGroupTPa, newMaterialGroupTPA.Id, newMaterialGroupTPA.Code, jsonBefor, jsonApter);

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

        private void CheckExistedForUpdate(MaterialGroupTPAModel model)
        {
            if (db.MaterialGroupTPAs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MaterialGroupTPA);
            }

            if (db.MaterialGroupTPAs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MaterialGroupTPA);
            }
        }
    }
}
