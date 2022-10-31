using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.ReasonChangeInsurance;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReasonChangeInsurance
{
    public class ReasonChangeInsuranceBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ReasonChangeInsuranceSearchResultModel> SearchReason(ReasonChangeInsuranceSearchModel searchModel)
        {
            SearchResultModel<ReasonChangeInsuranceSearchResultModel> searchResult = new SearchResultModel<ReasonChangeInsuranceSearchResultModel>();
            var dataQuery = (from a in db.ReasonChangeInsurances.AsNoTracking()
                             select new ReasonChangeInsuranceSearchResultModel
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
        /// Xóa lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        public void DeleteReason(ReasonChangeInsuranceModel model)
        {
            var reasonExist = db.ReasonChangeInsurances.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (reasonExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeInsurance);
            }

            var reasonUsed = db.EmployeeChangeInsurances.AsNoTracking().FirstOrDefault(a => a.ReasonChangeInsuranceId.Equals(model.Id));
            if (reasonUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ReasonChangeInsurance);
            }

            try
            {
                db.ReasonChangeInsurances.Remove(reasonExist);

                var NameOrCode = reasonExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<ReasonChangeInsuranceHistoryModel>(reasonExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_ReasonChangeInsurance, reasonExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        public void CreateReason(ReasonChangeInsuranceModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonExits = db.ReasonChangeInsurances.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonChangeInsurance);
            }

            try
            {
                NTS.Model.Repositories.ReasonChangeInsurance reason = new NTS.Model.Repositories.ReasonChangeInsurance
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.ReasonChangeInsurances.Add(reason);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, reason.Name, reason.Id, Constants.LOG_Reason);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReasonChangeInsuranceModel GetReason(ReasonChangeInsuranceModel model)
        {
            var resultInfo = db.ReasonChangeInsurances.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ReasonChangeInsuranceModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeInsurance);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật lý do điều chỉnh bảo hiểm xã hội
        /// </summary>
        /// <param name="model"></param>
        public void UpdateReason(ReasonChangeInsuranceModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonUpdate = db.ReasonChangeInsurances.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (reasonUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeInsurance);
            }

            var reasonNameExist = db.ReasonChangeInsurances.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonChangeInsurance);
            }

            try
            {
                reasonUpdate.Name = model.Name;
                reasonUpdate.Note = model.Note;
                reasonUpdate.UpdateBy = model.UpdateBy;
                reasonUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<ReasonHistoryModel>(reasonUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_ReasonChangeInsurance, reasonUpdate.Id, reasonUpdate.Name, reasonUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
