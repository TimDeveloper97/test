using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.EmployeeGroups;
using NTS.Model.Repositories;
using NTS.Model.UserHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Linq;

namespace QLTK.Business.EmployeeGroups
{
    public class EmployeeGroupBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm nhóm nhân viên 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<EmployeeGroupModel> GetListEmployeeGroup(EmployeeGroupSearchModel modelSearch)
        {
            SearchResultModel<EmployeeGroupModel> searchResult = new SearchResultModel<EmployeeGroupModel>();
            var listEmployeeGroup = (from o in db.EmployeeGroups.AsNoTracking()
                                     orderby o.Name
                                     select new EmployeeGroupModel
                                     {
                                         EmployeeGroupId = o.EmployeeGroupId,
                                         Code = o.Code,
                                         Name = o.Name,
                                         Note = o.Note,
                                     }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                listEmployeeGroup = listEmployeeGroup.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                listEmployeeGroup = listEmployeeGroup.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            searchResult.TotalItem = listEmployeeGroup.Count();
            var listResult = NTS.Model.SQLHelpper.OrderBy(listEmployeeGroup, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Xóa nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void DeleteEmployeeGroup(EmployeeGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var employeeGroup = db.Employees.AsNoTracking().Where(m => m.EmployeeGroupId.Equals(model.EmployeeGroupId)).FirstOrDefault();
                if (employeeGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.EmployeeGroup);
                }
                try
                {
                    var employeeGroupsss = db.EmployeeGroups.FirstOrDefault(u => u.EmployeeGroupId.Equals(model.EmployeeGroupId));
                    if (employeeGroupsss == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeGroup);
                    }
                    db.EmployeeGroups.Remove(employeeGroupsss);

                    var NameOrCode = employeeGroupsss.Name;
                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeGroupHistoryModel>(employeeGroupsss);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_GroupEmployee, employeeGroupsss.EmployeeGroupId, NameOrCode, jsonBefor);

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

        /// <summary>
        ///  Get nhóm nhân viên 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetEmployeeGroup(EmployeeGroupModel model)
        {
            var resuldInfor = db.EmployeeGroups.AsNoTracking().Where(u => model.EmployeeGroupId.Equals(u.EmployeeGroupId)).Select(p => new EmployeeGroupModel
            {
                EmployeeGroupId = p.EmployeeGroupId,
                Code = p.Code,
                Name = p.Name,
                Note = p.Note,
            }).FirstOrDefault();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeGroup);
            }
            return resuldInfor;
        }

        /// <summary>
        ///  Thêm mới nhóm nhân viên 
        /// </summary>
        /// <param name="model"></param>
        public void AddEmployeeGroup(EmployeeGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // check mã nhóm nhân viên  đã tồn tại chưa
                if (db.EmployeeGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EmployeeGroup);
                }
                try
                {
                    EmployeeGroup newtestEmployeeGroup = new EmployeeGroup()
                    {
                        EmployeeGroupId = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.EmployeeGroups.Add(newtestEmployeeGroup);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newtestEmployeeGroup.Code, newtestEmployeeGroup.EmployeeGroupId, Constants.LOG_GroupEmployee);

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

        /// <summary>
        /// Update nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void UpdateEmployeeGroup(EmployeeGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.EmployeeGroups.AsNoTracking().Where(o => !o.EmployeeGroupId.Equals(model.EmployeeGroupId) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EmployeeGroup);
                }
                try
                {
                    var newtestCri = db.EmployeeGroups.Where(r => r.EmployeeGroupId.Equals(model.EmployeeGroupId)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<EmployeeGroupHistoryModel>(newtestCri);

                    newtestCri.Name = model.Name.NTSTrim();
                    newtestCri.Note = model.Note.NTSTrim();
                    newtestCri.Code = model.Code.NTSTrim();
                    newtestCri.UpdateBy = model.UpdateBy;
                    newtestCri.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeGroupHistoryModel>(newtestCri);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_GroupEmployee, newtestCri.EmployeeGroupId, newtestCri.Code, jsonBefor, jsonApter);
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
