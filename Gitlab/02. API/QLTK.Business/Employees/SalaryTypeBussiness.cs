using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using NTS.Model.SalaryLevel;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SalaryLevel
{
    public class SalaryTypeBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm mức lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<SalaryTypeModel> SearchSalaryLevel(SalaryTypeSearchModel searchModel)
        {
            SearchResultModel<SalaryTypeModel> searchResult = new SearchResultModel<SalaryTypeModel>();
            var dataQuery = (from a in db.SalaryTypes.AsNoTracking()
                             select new SalaryTypeModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Code = a.Code,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa mức lương
        /// </summary>
        /// <param name="model"></param>
        public void DeleteSalaryType(SalaryTypeModel model, string userId)
        {
            var salaryTypeExist = db.SalaryTypes.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (salaryTypeExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryType);
            }

            var workTypes = db.WorkTypes.AsNoTracking().FirstOrDefault(a => a.SalaryGroupId.Equals(model.Id));
            var salaryLevels = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => a.SalaryGroupId.Equals(model.Id));
            if (workTypes != null || salaryLevels != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SalaryType);
            }

            try
            {
                db.SalaryTypes.Remove(salaryTypeExist);

                var NameOrCode = salaryTypeExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryTypeHistoryModel>(salaryTypeExist);
                //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SalaryType, salaryTypeExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới mức lương
        /// </summary>
        /// <param name="model"></param>
        public void CreateSalaryType(SalaryTypeModel model, string userId)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryTypeNameExits = db.SalaryTypes.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (salaryTypeNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryType);
            }
            

            try
            {
                NTS.Model.Repositories.SalaryType salaryType = new NTS.Model.Repositories.SalaryType
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Code = model.Code,
                    Note = model.Note,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now,
                };

                db.SalaryTypes.Add(salaryType);

                UserLogUtil.LogHistotyAdd(db, userId, salaryType.Name, salaryType.Id, Constants.LOG_SalaryType);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin mức lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SalaryTypeModel GetSalaryTypeById(SalaryTypeModel model)
        {
            var resultInfo = db.SalaryTypes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SalaryTypeModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryType);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật mức lương
        /// </summary>
        /// <param name="model"></param>
        public void UpdateSalaryType(SalaryTypeModel model, string userId)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryTypeUpdate = db.SalaryTypes.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (salaryTypeUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryType);
            }

            var salaryTypeUpdateExist = db.SalaryTypes.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (salaryTypeUpdateExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryType);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryLevelHistoryModel>(salaryTypeUpdate);

            try
            {
                salaryTypeUpdate.Name = model.Name;
                salaryTypeUpdate.Code = model.Code;
                salaryTypeUpdate.Note = model.Note;
                salaryTypeUpdate.UpdateBy = userId;
                salaryTypeUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<SalaryTypeHistoryModel>(salaryTypeUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_SalaryType, salaryTypeUpdate.Id, salaryTypeUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
