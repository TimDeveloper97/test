using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using NTS.Model.WorkLocation;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.WorkLocation
{
    public class WorkLocationBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        /// <summary>
        /// Tìm kiếm địa điểm làm việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<WorkLocationSearchResultModel> SearchWorkLocation(WorkLocationSearchModel searchModel)
        {
            SearchResultModel<WorkLocationSearchResultModel> searchResult = new SearchResultModel<WorkLocationSearchResultModel>();
            var dataQuery = (from a in db.WorkLocations.AsNoTracking()
                             select new WorkLocationSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa địa điểm làm việc
        /// </summary>
        /// <param name="model"></param>
        public void DeleteWorkLocation(WorkLocationModel model)
        {
            var workLocationExist = db.WorkLocations.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (workLocationExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkLocation);
            }

            var workLocationUsed = db.Employees.AsNoTracking().FirstOrDefault(a => a.WorkLocationId.Equals(model.Id));
            if (workLocationUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.WorkLocation);
            }

            try
            {
                db.WorkLocations.Remove(workLocationExist);

                var NameOrCode = workLocationExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkLocationHistoryModel>(workLocationExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_WorkLocation, workLocationExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới địa điểm làm việc
        /// </summary>
        /// <param name="model"></param>
        public void CreateWorkLocation(WorkLocationModel model)
        {
            model.Name = model.Name.NTSTrim();
            var workLocationExits = db.WorkLocations.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (workLocationExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkLocation);
            }

            try
            {
                NTS.Model.Repositories.WorkLocation workLocation = new NTS.Model.Repositories.WorkLocation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.WorkLocations.Add(workLocation);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, workLocation.Name, workLocation.Id, Constants.LOG_WorkLocation);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin địa điểm làm việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WorkLocationModel GetWorkLocation(WorkLocationModel model)
        {
            var workLocationInfo = db.WorkLocations.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new WorkLocationModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note
            }).FirstOrDefault();

            if (workLocationInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkLocation);
            }

            return workLocationInfo;
        }

        /// <summary>
        /// Cập nhật địa điểm làm việc
        /// </summary>
        /// <param name="model"></param>
        public void UpdateWorkLocation(WorkLocationModel model)
        {
            model.Name = model.Name.NTSTrim();
            var workLocationUpdate = db.WorkLocations.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (workLocationUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkLocation);
            }

            var workLocationNameExist = db.WorkLocations.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (workLocationNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkLocation);
            }

            try
            {
                workLocationUpdate.Name = model.Name;
                workLocationUpdate.Note = model.Note;
                workLocationUpdate.UpdateBy = model.UpdateBy;
                workLocationUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<WorkLocationHistoryModel>(workLocationUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_WorkLocation, workLocationUpdate.Id, workLocationUpdate.Name, workLocationUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

    }
}
