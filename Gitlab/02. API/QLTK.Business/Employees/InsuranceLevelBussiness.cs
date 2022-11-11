using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.InsuranceLevel;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.InsuranceLevel
{
    public class InsuranceLevelBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm mức đóng bảo hiểm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<InsuranceLevelSearchResultModel> SearchInsuranceLevel(InsuranceLevelSearchModel searchModel)
        {
            SearchResultModel<InsuranceLevelSearchResultModel> searchResult = new SearchResultModel<InsuranceLevelSearchResultModel>();
            var dataQuery = (from a in db.InsuranceLevels.AsNoTracking()
                             select new InsuranceLevelSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Money = a.Money
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
        /// Xóa mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        public void DeleteInsuranceLevel(InsuranceLevelModel model)
        {
            var insuranceLevelExist = db.InsuranceLevels.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (insuranceLevelExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.InsuranceLevel);
            }

            var insuranceLevelUsed = db.Employees.AsNoTracking().FirstOrDefault(a => a.InsuranceLevelId.Equals(model.Id));
            if (insuranceLevelUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.InsuranceLevel);
            }

            try
            {
                db.InsuranceLevels.Remove(insuranceLevelExist);

                var NameOrCode = insuranceLevelExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<InsuranceLevelHistoryModel>(insuranceLevelExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_InsuranceLevel, insuranceLevelExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        public void CreateInsuranceLevel(InsuranceLevelModel model)
        {
            var insuranceLevelExits = db.InsuranceLevels.AsNoTracking().FirstOrDefault(a => a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (insuranceLevelExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.InsuranceLevel);
            }

            try
            {
                NTS.Model.Repositories.InsuranceLevel insuranceLevel = new NTS.Model.Repositories.InsuranceLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    Money = model.Money,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.InsuranceLevels.Add(insuranceLevel);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, insuranceLevel.Name, insuranceLevel.Id, Constants.LOG_InsuranceLevel);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InsuranceLevelModel GetInsuranceLevel(InsuranceLevelModel model)
        {
            var resultInfo = db.InsuranceLevels.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new InsuranceLevelModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note,
                Money = p.Money
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.InsuranceLevel);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật lý do mức đóng bảo hiểm
        /// </summary>
        /// <param name="model"></param>
        public void UpdateInsuranceLevel(InsuranceLevelModel model)
        {
            var insuranceLevelUpdate = db.InsuranceLevels.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (insuranceLevelUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.InsuranceLevel);
            }

            var insuranceLevelNameExist = db.InsuranceLevels.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (insuranceLevelNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.InsuranceLevel);
            }

            try
            {
                insuranceLevelUpdate.Name = model.Name;
                insuranceLevelUpdate.Note = model.Note;
                insuranceLevelUpdate.Money = model.Money;
                insuranceLevelUpdate.UpdateBy = model.UpdateBy;
                insuranceLevelUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<InsuranceLevelHistoryModel>(insuranceLevelUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_InsuranceLevel, insuranceLevelUpdate.Id, insuranceLevelUpdate.Name, insuranceLevelUpdate, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}

