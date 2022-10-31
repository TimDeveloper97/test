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
    public class SalaryGroupBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm mức lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<SalaryGroupModel> SearchSalaryGroup(SalaryGroupSearchModel searchModel)
        {
            SearchResultModel<SalaryGroupModel> searchResult = new SearchResultModel<SalaryGroupModel>();
            var dataQuery = (from a in db.SalaryGroups.AsNoTracking()
                             select new SalaryGroupModel
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
        public void DeleteSalaryGroup(SalaryGroupModel model, string userId)
        {
            var salaryGroupExist = db.SalaryGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (salaryGroupExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryGroup);
            }

            var workTypes = db.WorkTypes.AsNoTracking().FirstOrDefault(a => a.SalaryGroupId.Equals(model.Id));
            var salaryLevels = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => a.SalaryGroupId.Equals(model.Id));
            if (workTypes != null || salaryLevels != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SalaryGroup);
            }

            try
            {
                db.SalaryGroups.Remove(salaryGroupExist);

                var NameOrCode = salaryGroupExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryGroupHistoryModel>(salaryGroupExist);
                //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SalaryGroup, salaryGroupExist.Id, NameOrCode, jsonBefor);

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
        public void CreateSalaryGroup(SalaryGroupModel model, string userId)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryLevelNameExits = db.SalaryGroups.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (salaryLevelNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryGroup);
            }

            try
            {
                NTS.Model.Repositories.SalaryGroup salaryGroup = new NTS.Model.Repositories.SalaryGroup
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

                db.SalaryGroups.Add(salaryGroup);

                UserLogUtil.LogHistotyAdd(db, userId, salaryGroup.Name, salaryGroup.Id, Constants.LOG_SalaryGroup);

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
        public SalaryGroupModel GetSalaryGroupById(SalaryGroupModel model)
        {
            var resultInfo = db.SalaryGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SalaryGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryGroup);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật mức lương
        /// </summary>
        /// <param name="model"></param>
        public void UpdateGroup(SalaryGroupModel model, string userId)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryGroupUpdate = db.SalaryGroups.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (salaryGroupUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryGroup);
            }

            var salaryLevelNameExist = db.SalaryGroups.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (salaryLevelNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryGroup);
            }
          
            //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryGroupHistoryModel>(salaryGroupUpdate);

            try
            {
                salaryGroupUpdate.Name = model.Name;
                salaryGroupUpdate.Code = model.Code;
                salaryGroupUpdate.Note = model.Note;
                salaryGroupUpdate.UpdateBy = userId;
                salaryGroupUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<SalaryGroupHistoryModel>(salaryGroupUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_SalaryGroup, salaryGroupUpdate.Id, salaryGroupUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
