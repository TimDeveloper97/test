using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Bank;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Bank
{
    public class BankBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<BankResultModel> SearchBank(BankSearchModel modelSearch)
        {

            SearchResultModel<BankResultModel> searchResult = new SearchResultModel<BankResultModel>();

            var data = (from a in db.Banks.AsNoTracking()
                        join b in db.Experts.AsNoTracking() on a.ExpertId equals b.Id
                        select new BankResultModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Account = a.Account,
                            AccountName = a.AccountName,
                        }).AsQueryable();

            searchResult.ListResult = data.ToList();
            return searchResult;
        }
        public BankModel GetBankInfo(BankModel model)
        {
            var resultInfo = db.Banks.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new BankModel()
            {
                Id = p.Id,
                Name = p.Name,
                Account = p.Account,
                AccountName = p.AccountName,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Bank);
            }
            return resultInfo;
        }
        public void DeleteBank(BankModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _bank = db.Banks.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_bank == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Bank);
                    }

                    db.Banks.Remove(_bank);
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
    }
}
