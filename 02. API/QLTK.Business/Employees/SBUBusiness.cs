using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.NTSDepartment;
using NTS.Model.Repositories;
using NTS.Model.SBU;
using NTS.Model.UserHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SBUs
{
    public class SBUBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SBUModel> SearchSBU(SBUSearchModel modelSearch)
        {
            SearchResultModel<SBUModel> searchResult = new SearchResultModel<SBUModel>();

            var dataQuery = (from a in db.SBUs.AsNoTracking()
                             select new SBUModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Status = a.Status,
                                 Location = a.Location,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Status.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.Status.Equals(modelSearch.Status.ToString()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<DepartmentResultModel> SearchDepartment(DepartmentSearchModel modelSearch)
        {
            SearchResultModel<DepartmentResultModel> searchResult = new SearchResultModel<DepartmentResultModel>();

            var dataQuery = (from a in db.Departments.AsNoTracking()
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.ManagerId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             select new DepartmentResultModel
                             {
                                 Id = a.Id,
                                 SBUId = a.SBUId,
                                 SBUName = b.Name,
                                 ManagerId = a.ManagerId,
                                 ManagerName = c.Name,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Status = a.Status,
                                 PhoneNumber = a.PhoneNumber,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddSBU(SBUModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SBU newSBU = new SBU
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        Status = model.Status,
                        Location = model.Location.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.SBUs.Add(newSBU);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newSBU.Code, newSBU.Id, Constants.LOG_SBU);

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
        public void UpdateSBU(SBUModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSBU = db.SBUs.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SBUHistoryModel>(newSBU);

                    newSBU.Name = model.Name.NTSTrim();
                    newSBU.Code = model.Code.NTSTrim();
                    newSBU.Note = model.Note.NTSTrim();
                    newSBU.Status = model.Status;
                    newSBU.Location = model.Location.NTSTrim();
                    newSBU.UpdateBy = model.UpdateBy;
                    newSBU.UpdateDate = DateTime.Now;


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SBUHistoryModel>(newSBU);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_SBU, newSBU.Id, newSBU.Code, jsonBefor, jsonApter);

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

        public void DeleteSBU(SBUModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var sbu = db.Departments.AsNoTracking().Where(m => m.SBUId.Equals(model.Id)).FirstOrDefault();
                if (sbu != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SBU);
                }

                var customer = db.Customers.AsNoTracking().Where(m => m.SBUId.Equals(model.Id)).FirstOrDefault();
                if (customer != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SBU);
                }

                var saleProductTypes = db.SaleProductTypes.AsNoTracking().Where(m => m.SBUId.Equals(model.Id)).FirstOrDefault();
                if (saleProductTypes != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SBU);
                }

                try
                {
                    var sBU = db.SBUs.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (sBU == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SBU);
                    }

                    db.SBUs.Remove(sBU);

                    var NameOrCode = sBU.Code;


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SBUHistoryModel>(sBU);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_SBU, sBU.Id, NameOrCode, jsonBefor);

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
        public object GetSBUInfo(SBUModel model)
        {
            var resultInfo = db.SBUs.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SBUModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
                Status = p.Status,
                Location = p.Location
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SBU);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(SBUModel model)
        {
            if (db.SBUs.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SBU);
            }

            if (db.SBUs.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SBU);
            }
        }

        public void CheckExistedForUpdate(SBUModel model)
        {
            if (db.SBUs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SBU);
            }

            if (db.SBUs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SBU);
            }
        }
    }
}
