using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.BankAccount;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.BankAccount
{
    public class BankAccountBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm ngân hàng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<BankAccountSearchResultModel> SearchBankAccount(BankAccountSearchModel searchModel)
        {
            SearchResultModel<BankAccountSearchResultModel> searchResult = new SearchResultModel<BankAccountSearchResultModel>();
            var dataQuery = (from a in db.BankAccounts.AsNoTracking()
                             select new BankAccountSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Code = a.Code
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
        /// Xóa ngân hàng
        /// </summary>
        /// <param name="model"></param>
        public void DeleteBankAccount(BankAccountModel model)
        {
            var bankAccountExist = db.BankAccounts.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (bankAccountExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.BankAccount);
            }

            var bankAccountUsed = db.EmployeeBankAccounts.AsNoTracking().FirstOrDefault(a => a.BankAccountId.Equals(model.Id));
            if (bankAccountUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.BankAccount);
            }

            try
            {
                db.BankAccounts.Remove(bankAccountExist);

                var NameOrCode = bankAccountExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<BankAccountHistoryModel>(bankAccountExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_BankAccount, bankAccountExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới ngân hàng
        /// </summary>
        /// <param name="model"></param>
        public void CreateBankAccount(BankAccountModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var bankAccountNameExits = db.BankAccounts.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (bankAccountNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.BankAccount);
            }

            var bankAccountCodeExits = db.BankAccounts.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (bankAccountCodeExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.BankAccount);
            }

            try
            {
                NTS.Model.Repositories.BankAccount bankAccount = new NTS.Model.Repositories.BankAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Code = model.Code,
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.BankAccounts.Add(bankAccount);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, bankAccount.Name, bankAccount.Id, Constants.LOG_BankAccount);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin ngân hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BankAccountModel GetBankAccount(BankAccountModel model)
        {
            var resultInfo = db.BankAccounts.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new BankAccountModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.BankAccount);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật ngân hàng
        /// </summary>
        /// <param name="model"></param>
        public void UpdateBankAccount(BankAccountModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var bankAccountUpdate = db.BankAccounts.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (bankAccountUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.BankAccount);
            }

            var bankAccountNameExist = db.BankAccounts.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (bankAccountNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.BankAccount);
            }

            var BankAccountCodeExist = db.BankAccounts.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (BankAccountCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.BankAccount);
            }

            try
            {
                bankAccountUpdate.Name = model.Name;
                bankAccountUpdate.Code = model.Code;
                bankAccountUpdate.Note = model.Note;
                bankAccountUpdate.UpdateBy = model.UpdateBy;
                bankAccountUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<BankAccountHistoryModel>(bankAccountUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_BankAccount, bankAccountUpdate.Id, bankAccountUpdate.Name, bankAccountUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
