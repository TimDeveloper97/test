using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Specialize;
using NTS.Model.SpecializeHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;

namespace QLTK.Business.Specialize
{
   public class Specialize
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SpecializeResultModel> SearchSpecialize(SpecializeSearchModel modelSearch)
        {
            SearchResultModel<SpecializeResultModel> searchResult = new SearchResultModel<SpecializeResultModel>();

            var dataQuery = (from a in db.Specializes.AsNoTracking()
                             orderby a.Name
                select new SpecializeResultModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Code = a.Code,
                    Description = a.Description,
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
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
                .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SpecializeModel GetByIdSpecialize(SpecializeModel model)
        {
            var resultInfo = db.Specializes.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new SpecializeModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Specialize);
            }

            return resultInfo;
        }

        public SpecializeModel AddSpecialize(SpecializeModel model)
        {
            string id;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.Specialize newSpecialize = new NTS.Model.Repositories.Specialize()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.Specializes.Add(newSpecialize);
                    id = newSpecialize.Id;

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newSpecialize.Code, newSpecialize.Id, Constants.LOG_Specialize);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
                var resultInfo = db.Specializes.Where(u => u.Id.Equals(id)).Select(p => new SpecializeModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                }).FirstOrDefault();
                return resultInfo;

            }
        }

        public void UpdateSpecialize(SpecializeModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSpecialize = db.Specializes.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SpecializeHistoryModel>(newSpecialize);

                    newSpecialize.Name = model.Name.Trim();
                    newSpecialize.Code = model.Code.Trim();
                    newSpecialize.Description = model.Description.Trim();
                    newSpecialize.UpdateBy = model.UpdateBy;
                    newSpecialize.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SpecializeHistoryModel>(newSpecialize);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Specialize, newSpecialize.Id, newSpecialize.Code, jsonBefor, jsonApter);

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

        public void DeleteSpecialize(SpecializeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                //check mã đc sử dụng
                var specialize = db.SpecializeExperts.AsNoTracking().Where(u => u.SpecializeId.Equals(model.Id)).FirstOrDefault();
                if (specialize != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Specialize);
                }
                try
                {
                    var _specialize = db.Specializes.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_specialize == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Specialize);
                    }

                    db.Specializes.Remove(_specialize);

                    var NameOrCode = _specialize.Name;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SpecializeHistoryModel>(_specialize);

                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Specialize, _specialize.Id, NameOrCode, jsonApter);
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

        private void CheckExistedForAdd(SpecializeModel model)
        {
            if (db.Specializes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Specialize);
            }

            if (db.Specializes.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Specialize);
            }
        }

        public void CheckExistedForUpdate(SpecializeModel model)
        {
            if (db.Specializes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Specialize);
            }

            if (db.Specializes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Specialize);
            }

        }
    }
}
