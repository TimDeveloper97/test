using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Degree;
using NTS.Model.DegreeHistory;
using NTS.Model.Repositories;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using SQLHelpper = NTS.Utils.SQLHelpper;

namespace QLTK.Business.Degree
{
    public class Degree
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<DegreeResultModel> SearchDegree(DegreeSearchModel modelSearch)
        {
            SearchResultModel<DegreeResultModel> searchResult = new SearchResultModel<DegreeResultModel>();

            var dataQuery = (from a in db.Degrees.AsNoTracking()
                orderby a.Name
                select new DegreeResultModel()
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
        public object GetDegreeInfo(DegreeModel model)
        {
            var resultInfo = db.Degrees.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new DegreeModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Degree);
            }
            return resultInfo;
        }
        public string AddDegree(DegreeModel model, string userId)
        {
            string id;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    NTS.Model.Repositories.Degree newDegree = new NTS.Model.Repositories.Degree
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Description =  model.Description.NTSTrim(),
                    };

                    db.Degrees.Add(newDegree);
                    id = newDegree.Id;

                    UserLogUtil.LogHistotyAdd(db, userId, newDegree.Code, newDegree.Id, Constants.LOG_Degree);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return id;
        }
        public void UpdateDegree(DegreeModel model, string userId)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newDegree = db.Degrees.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    
                    //var jsonApter = AutoMapperConfig.Mapper.Map<DegreeHistoryModel>(newDegree);

                    newDegree.Name = model.Name.NTSTrim();
                    newDegree.Code = model.Code.NTSTrim();
                    newDegree.Description = model.Description.NTSTrim();

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DegreeHistoryModel>(newDegree);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Degree, newDegree.Id, newDegree.Code, jsonBefor, jsonApter);

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
        public void DeleteDegree(DegreeModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var degree = db.Subjects.AsNoTracking().Where(u => u.DegreeId.Equals(model.Id)).FirstOrDefault();
                if (degree != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Degree);
                }

                var __degree = db.Experts.AsNoTracking().Where(u => u.DegreeId.Equals(model.Id)).FirstOrDefault();
                if (__degree != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Degree);
                }

                try
                {
                    var _degree = db.Degrees.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_degree == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Degree);
                    }

                    db.Degrees.Remove(_degree);

                    var NameOrCode = _degree.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DegreeHistoryModel>(_degree);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_Degree, _degree.Id, NameOrCode, jsonBefor);

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
        private void CheckExistedForAdd(DegreeModel model)
        {
            if (db.Degrees.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Degree);
            }

            if (db.Degrees.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Degree);
            }
        }
        public void CheckExistedForUpdate(DegreeModel model)
        {
            if (db.Degrees.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Degree);
            }

            if (db.Degrees.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Degree);
            }
        }
    }
}
