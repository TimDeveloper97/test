using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Industry;
using NTS.Model.IndustryHistory;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Industrys
{
    public class IndustryBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public object SearchIndustry(IndustrySearchModel modelSearch)
        {
            SearchResultModel<IndustryModel> searchResult = new SearchResultModel<IndustryModel>();
            try
            {

                var dataQuery = (from a in db.Industries.AsNoTracking()
                                 orderby a.Code
                                 select new IndustryModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
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
                var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        public void DeleteIndustry(IndustryModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var industry = db.Industries.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (industry == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Industry);
                    }
                    db.Industries.Remove(industry);

                    var NameOrCode = industry.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<IndustryHistoryModel>(industry);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Industry, industry.Id, NameOrCode, jsonBefor);

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

        public object GetIndustry(IndustryModel model)
        {
            var resuldInfor = db.Industries.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new IndustryModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
            }).FirstOrDefault();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Industry);
            }
            return resuldInfor;
        }

        public void AddIndustry(IndustryModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên nhóm việc làm đã tồn tại chưa
                if (db.Industries.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Industry);
                }
                // check mã nhóm việc làm đã tồn tại chưa
                if (db.Industries.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Industry);
                }
                try
                {
                    Industry industry = new Industry()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                    };
                    db.Industries.Add(industry);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, industry.Code, industry.Id, Constants.LOG_Industry);

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

        public void UpdateIndustry(IndustryModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.Industries.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Industry);
                }

                if (db.Industries.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Industry);
                }
                try
                {
                    var newIndustries = db.Industries.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<IndustryHistoryModel>(newIndustries);

                    newIndustries.Name = model.Name.NTSTrim();
                    newIndustries.Description = model.Description.NTSTrim();
                    newIndustries.Code = model.Code.NTSTrim();

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<IndustryHistoryModel>(newIndustries);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Industry, newIndustries.Id, newIndustries.Code, jsonBefor, jsonApter);

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
