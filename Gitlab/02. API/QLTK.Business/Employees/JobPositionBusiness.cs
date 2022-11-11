using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.JobPosition;
using NTS.Model.Repositories;
using NTS.Model.UserHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Linq;
namespace QLTK.Business.JobPositionss
{
    public class JobPositionBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        /// <summary>
        /// Tìm kiếm chức vụ
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public object SearchJobPostiton(JobPositionSearchModel searchModel)
        {
            SearchResultModel<JobPositionModel> searchResult = new SearchResultModel<JobPositionModel>();

            var dataQuery = (from a in db.JobPositions.AsNoTracking()
                             orderby a.Index
                             select new JobPositionModel
                             {
                                 Id = a.Id,
                                 Index = a.Index,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            //if (!string.IsNullOrEmpty(searchModel.Code))
            //{
            //    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            //}
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa chức vụ
        /// </summary>
        /// <param name="model"></param>
        public void DeleteJobPosition(JobPositionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var maxIndex = db.JobPositions.AsNoTracking().Select(a => a.Index).Max();
                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listUnit = db.JobPositions.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listUnit.Count() > 0 && listUnit != null)
                    {
                        foreach (var item in listUnit)
                        {
                            if (!item.Id.Equals(model.Id))
                            {
                                var updateUnit = db.JobPositions.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                                updateUnit.Index--;
                            }

                        }
                    }
                }
                try
                {
                    var jobPositions = db.JobPositions.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (jobPositions == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.JobPosition);
                    }

                    db.JobPositions.Remove(jobPositions);

                    var NameOrCode = jobPositions.Name;
                    //var jsonBefor = AutoMapperConfig.Mapper.Map<JobPositionHistoryModel>(jobPositions);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_JobPosition, jobPositions.Id, NameOrCode, jsonBefor);

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

        /// <summary>
        /// Thêm mới chức vụ
        /// </summary>
        /// <param name="model"></param>
        public void AddJobPosition(JobPositionModel model)
        {
            var indexs = db.JobPositions.ToList();
            var maxIndex = 2;
            if (indexs.Count > 0)
            {
                maxIndex = indexs.Select(a => a.Index).Max();
            }

            if (model.Index <= maxIndex)
            {
                int modelIndex = model.Index;
                var listUnit = db.JobPositions.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                if (listUnit.Count() > 0 && listUnit != null)
                {
                    foreach (var item in listUnit)
                    {
                        var updateUnit = db.JobPositions.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                        updateUnit.Index++;
                    }
                }
            }
            using (var trans = db.Database.BeginTransaction())
            {
                // Check tên chức vụ chí đã tồn tại chưa
                if (db.JobPositions.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.JobPosition);
                }
                try
                {
                    JobPosition jobPosition = new JobPosition
                    {
                        Id = Guid.NewGuid().ToString(),
                        Index = model.Index,
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        IsOnlyOne = model.IsOnlyOne,
                    };

                    db.JobPositions.Add(jobPosition);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, jobPosition.Name, jobPosition.Id, Constants.LOG_JobPosition);

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

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        public void UpdateJobPosition(JobPositionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                // Check tên tiêu chí đã tồn tại chưa
                if (db.JobPositions.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.JobPosition);
                }
                try
                {
                    var checkUnit = db.JobPositions.Where(b => b.Index == model.Index).FirstOrDefault();


                    if (checkUnit != null)
                    {
                        var newUnit = db.JobPositions.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<JobPositionHistoryModel>(newUnit);

                        int olđInext = newUnit.Index;
                        if (checkUnit.Index < newUnit.Index)
                        {
                            var listUnitChange1 = db.JobPositions.Where(a => a.Index > checkUnit.Index && a.Index < newUnit.Index);
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
                            var listUnitChange = db.JobPositions.Where(a => a.Index > newUnit.Index && a.Index < checkUnit.Index);
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
                        newUnit.Name = model.Name.NTSTrim();
                        newUnit.Code = model.Code.NTSTrim();
                        newUnit.Description = model.Description.NTSTrim();
                        newUnit.UpdateBy = model.UpdateBy;
                        newUnit.UpdateDate = DateTime.Now;
                        newUnit.IsOnlyOne = model.IsOnlyOne;


                        //var jsonBefor = AutoMapperConfig.Mapper.Map<JobPositionHistoryModel>(newUnit);

                        //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_JobPosition, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);
                    }
                    else
                    {
                        var newUnit = db.JobPositions.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        //var jsonApter = AutoMapperConfig.Mapper.Map<JobPositionHistoryModel>(newUnit);

                        var listUnit = (from a in db.JobPositions.AsNoTracking()
                                        orderby a.Index ascending
                                        select new JobPositionModel
                                        {
                                            Id = a.Id,
                                            Index = a.Index,
                                            Name = a.Name,
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
                        newUnit.IsOnlyOne = model.IsOnlyOne;

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<JobPositionHistoryModel>(newUnit);

                        //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_JobPosition, newUnit.Id, newUnit.Name, jsonBefor, jsonApter);
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

        /// <summary>
        /// Get chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetJobPosition(JobPositionModel model)
        {
            var resultInfo = db.JobPositions.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new JobPositionModel
            {
                Id = p.Id,
                Index = p.Index,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                IsOnlyOne = p.IsOnlyOne,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialGroupTPA);
            }

            return resultInfo;
        }
    }
}
