using NTS.Model.Combobox;
using NTS.Model.NTSDepartment;
using NTS.Model.Repositories;
using NTS.Utils;
using System;
using System.Linq;
using NTS.Model.Employees;
using NTS.Common;
using NTS.Common.Resource;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.UserHistory;

namespace QLTK.Business.Departments
{
    public class DepartmentBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<DepartmentResultModel> SearchDepartment(DepartmentSearchModel modelSearch)
        {
            SearchResultModel<DepartmentResultModel> searchResult = new SearchResultModel<DepartmentResultModel>();

            var dataQuery = (from a in db.Departments.AsNoTracking()
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id into ab
                             from abx in ab.DefaultIfEmpty()
                             join c in db.Employees.AsNoTracking() on a.ManagerId equals c.Id into ac
                             from acx in ac.DefaultIfEmpty()
                             orderby a.Code
                             select new DepartmentResultModel
                             {
                                 Id = a.Id,
                                 SBUId = a.SBUId,
                                 SBUName = abx.Name,
                                 ManagerId = acx.Id,
                                 ManagerName = acx != null ? acx.Name : "",
                                 JobPositionId = acx.JobPositionId,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Status = a.Status,
                                 PhoneNumber = a.PhoneNumber,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 IsDesign = a.IsDesign,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.ManagerId))
            {
                dataQuery = dataQuery.Where(u => u.ManagerId.Equals(modelSearch.ManagerId));
            }
            if (!string.IsNullOrEmpty(modelSearch.Status.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.Status.Equals(modelSearch.Status.ToString()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            foreach (var item in listResult)
            {
                if (!string.IsNullOrEmpty(item.JobPositionId) && !item.JobPositionId.Equals("3"))
                {
                    item.ManagerName = "";
                }
            }
            return searchResult;
        }
        public void AddDepartment(DepartmentModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Department newDepartment = new Department
                    {
                        Id = Guid.NewGuid().ToString(),
                        SBUId = model.SBUId,
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Status = model.Status,
                        PhoneNumber = model.PhoneNumber.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        IsDesign = model.IsDesign,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.Departments.Add(newDepartment);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newDepartment.Code, newDepartment.Id, Constants.LOG_Department);

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
        public void UpdateDepartment(DepartmentModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newDepartment = db.Departments.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<DepartmentHistoryModel>(newDepartment);

                    newDepartment.SBUId = model.SBUId;
                    newDepartment.Name = model.Name.NTSTrim();
                    newDepartment.Code = model.Code.NTSTrim();
                    newDepartment.Status = model.Status;
                    newDepartment.PhoneNumber = model.PhoneNumber;
                    newDepartment.Description = model.Description.NTSTrim();
                    newDepartment.UpdateBy = model.UpdateBy;
                    newDepartment.UpdateDate = DateTime.Now;
                    newDepartment.IsDesign = model.IsDesign;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DepartmentHistoryModel>(newDepartment);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Department, newDepartment.Id, newDepartment.Code, jsonBefor, jsonApter);

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

        public void DeleteDepartment(DepartmentModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var department = db.ProductStandards.AsNoTracking().Where(m => m.DepartmentId.Equals(model.Id)).FirstOrDefault();
                if (department != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Department);
                }

                try
                {
                    var departMent = db.Departments.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (departMent == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Department);
                    }

                    db.Departments.Remove(departMent);

                    var NameOrCode = departMent.Code;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DepartmentHistoryModel>(departMent);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Department, departMent.Id, NameOrCode, jsonBefor);

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
        public DepartmentModel GetDepartmentInfo(DepartmentModel model)
        {
            var resultInfo = db.Departments.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DepartmentModel
            {
                Id = p.Id,
                SBUId = p.SBUId,
                Name = p.Name,
                Code = p.Code,
                IsDesign = p.IsDesign,
                Status = p.Status.ToString(),
                PhoneNumber = p.PhoneNumber,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Department);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(DepartmentModel model)
        {
            if (db.Departments.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Department);
            }

            if (db.Departments.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Department);
            }
        }

        public void CheckExistedForUpdate(DepartmentModel model)
        {
            if (db.Departments.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Department);
            }

            if (db.Departments.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Department);
            }
        }
    }
}