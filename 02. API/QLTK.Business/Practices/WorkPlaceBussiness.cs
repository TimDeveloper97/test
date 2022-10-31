using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Specialize;
using NTS.Model.WorkPlace;
using NTS.Model.WorkPlaceHistory;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;

namespace QLTK.Business.WorkPlace
{
    public class WorkPlace
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<WorkPlaceResultModel> SearchWorkPlace(WorkPlaceSearchModel modelSearch)
        {
            SearchResultModel<WorkPlaceResultModel> searchResult = new SearchResultModel<WorkPlaceResultModel>();

            var dataQuery = (from a in db.Workplaces.AsNoTracking()
                             orderby a.Name
                select new WorkPlaceResultModel()
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

        public WorkPlaceModel GetByIdWorkPlace(WorkPlaceModel model)
        {
            var resultInfo = db.Workplaces.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new WorkPlaceModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkPlace);
            }
            return resultInfo;
        }

        public WorkPlaceModel AddWorkPlace(WorkPlaceModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);

            using (var trans = db.Database.BeginTransaction())
            {
                string id;
                try
                {
                    NTS.Model.Repositories.Workplace newWorkPlace = new NTS.Model.Repositories.Workplace()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Description = model.Description.Trim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.Workplaces.Add(newWorkPlace);
                    id = newWorkPlace.Id;

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newWorkPlace.Code, newWorkPlace.Id, Constants.LOG_WorkPlace);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
                var resultInfo = db.Workplaces.Where(u => u.Id.Equals(id)).Select(p => new WorkPlaceModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                }).FirstOrDefault();
                return resultInfo; 
            }
            
        }

        public void UpdateWorkPlace(WorkPlaceModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newWorkPlace = db.Workplaces.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkPlaceHistoryModel>(newWorkPlace);

                    newWorkPlace.Name = model.Name.Trim();
                    newWorkPlace.Code = model.Code.Trim();
                    newWorkPlace.Description = model.Description.Trim();
                    newWorkPlace.UpdateBy = model.UpdateBy;
                    newWorkPlace.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkPlaceHistoryModel>(newWorkPlace);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_WorkPlace, newWorkPlace.Id, newWorkPlace.Code, jsonBefor, jsonApter);
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

        public void DeleteWorkPlace(WorkPlaceModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var workPlace = db.ExpertWorkplaces.AsNoTracking().Where(u => u.WorkplaceId.Equals(model.Id)).FirstOrDefault();
                if (workPlace != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.WorkPlace);
                }

                try
                {
                    var _WorkPlace = db.Workplaces.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_WorkPlace == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkPlace);
                    }

                    db.Workplaces.Remove(_WorkPlace);

                    var NameOrCode = _WorkPlace.Name;


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkPlaceHistoryModel>(_WorkPlace);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_WorkPlace, _WorkPlace.Id, NameOrCode, jsonBefor);

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

        private void CheckExistedForAdd(WorkPlaceModel model)
        {
            if (db.Workplaces.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkPlace);
            }

            if (db.Workplaces.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.WorkPlace);
            }
        }
        public void CheckExistedForUpdate(WorkPlaceModel model)
        {
            if (db.Workplaces.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkPlace);
            }

            if (db.Workplaces.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkPlace);
            }

        }
    }



}
