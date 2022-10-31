using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.ReasonChangeIncome;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReasonChangeIncome
{
    public class ReasonChangeIncomeBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ReasonChangeIncomeSearchResultModel> SearchReason(ReasonChangeIncomeSearchModel searchModel)
        {
            SearchResultModel<ReasonChangeIncomeSearchResultModel> searchResult = new SearchResultModel<ReasonChangeIncomeSearchResultModel>();
            var dataQuery = (from a in db.ReasonChangeIncomes.AsNoTracking()
                             select new ReasonChangeIncomeSearchResultModel
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
        /// Xóa lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        public void DeleteReason(ReasonChangeIncomeModel model)
        {
            var reasonExist = db.ReasonChangeIncomes.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (reasonExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeIncome);
            }

            var reasonUsed = db.EmployeeChangeIncomes.AsNoTracking().FirstOrDefault(a => a.ReasonChangeIncomeId.Equals(model.Id));
            if (reasonUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ReasonChangeIncome);
            }

            try
            {
                db.ReasonChangeIncomes.Remove(reasonExist);

                var NameOrCode = reasonExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<ReasonChangeIncomeHistoryModel>(reasonExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_ReasonChangeIncome, reasonExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        public void CreateReason(ReasonChangeIncomeModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonExits = db.ReasonChangeIncomes.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonChangeIncome);
            }

            try
            {
                NTS.Model.Repositories.ReasonChangeIncome reason = new NTS.Model.Repositories.ReasonChangeIncome
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.ReasonChangeIncomes.Add(reason);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, reason.Name, reason.Id, Constants.LOG_Reason);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReasonChangeIncomeModel GetReason(ReasonChangeIncomeModel model)
        {
            var resultInfo = db.ReasonChangeIncomes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ReasonChangeIncomeModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeIncome);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật lý do điều chỉnh thu nhập
        /// </summary>
        /// <param name="model"></param>
        public void UpdateReason(ReasonChangeIncomeModel model)
        {
            model.Name = model.Name.NTSTrim();
            var reasonUpdate = db.ReasonChangeIncomes.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (reasonUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ReasonChangeIncome);
            }

            var reasonNameExist = db.ReasonChangeIncomes.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (reasonNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ReasonChangeIncome);
            }

            try
            {
                reasonUpdate.Name = model.Name;
                reasonUpdate.Note = model.Note;
                reasonUpdate.UpdateBy = model.UpdateBy;
                reasonUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<ReasonHistoryModel>(reasonUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_ReasonChangeIncome, reasonUpdate.Id, reasonUpdate.Name, reasonUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
