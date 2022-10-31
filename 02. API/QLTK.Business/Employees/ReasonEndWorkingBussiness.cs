using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.ReasonEndWorking;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReasonEndWorking
{
    public class ReasonEndWorkingBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm lý do nghỉ việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ReasonSearchResultModel> SearchReason(ReasonSearchModel searchModel)
        {
            SearchResultModel<ReasonSearchResultModel> searchResult = new SearchResultModel<ReasonSearchResultModel>();
            var dataQuery = (from a in db.ReasonEndWorkings.AsNoTracking()
                             select new ReasonSearchResultModel
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
        /// Xóa lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        public void DeleteReason(ReasonModel model)
        {
            var reasonExist = db.ReasonEndWorkings.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (reasonExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonEndWorking);
            }

            var reasonUsed = db.Employees.AsNoTracking().FirstOrDefault(a => a.ReasonEndWorkingId.Equals(model.Id));
            if (reasonUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ReasonEndWorking);
            }

            try
            {
                db.ReasonEndWorkings.Remove(reasonExist);

                var NameOrCode = reasonExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<ReasonHistoryModel>(reasonExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Reason, reasonExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        public void CreateReason(ReasonModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonExits = db.ReasonEndWorkings.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonEndWorking);
            }

            try
            {
                NTS.Model.Repositories.ReasonEndWorking reason = new NTS.Model.Repositories.ReasonEndWorking
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.ReasonEndWorkings.Add(reason);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, reason.Name, reason.Id, Constants.LOG_Reason);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReasonModel GetReason(ReasonModel model)
        {
            var resultInfo = db.ReasonEndWorkings.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ReasonModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonEndWorking);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật lý do nghỉ việc
        /// </summary>
        /// <param name="model"></param>
        public void UpdateReason(ReasonModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonUpdate = db.ReasonEndWorkings.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (reasonUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonEndWorking);
            }

            var reasonNameExist = db.ReasonEndWorkings.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonEndWorking);
            }

            try
            {
                reasonUpdate.Name = model.Name;
                reasonUpdate.Note = model.Note;
                reasonUpdate.UpdateBy = model.UpdateBy;
                reasonUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<ReasonHistoryModel>(reasonUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Reason, reasonUpdate.Id, reasonUpdate.Name, reasonUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
