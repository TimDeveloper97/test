using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Bussiness;
using NTS.Model.Bussiness.Application;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Business
{
   public class ApplicationService
    {
        private readonly QLTKEntities db = new QLTKEntities();

        /// <summary>
        ///  Tìm kiếm ứng dụng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ApplicationModel> SearchApplication (ApplicationSearchModel modelSearch)
        {
            SearchResultModel<ApplicationModel> searchResult = new SearchResultModel<ApplicationModel>();

            var dataQuery = (from a in db.Applications.AsNoTracking()
                             orderby a.Index
                             select new ApplicationModel
                             {
                                Index=a.Index,
                                Name=a.Name,
                                Code = a.Code,
                                Note=a.Note,
                                Id=a.Id,
                             }).AsQueryable();

            // Tìm kiếm theo tên khóa học
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Thêm mới ứng dụng
        /// </summary>
        /// <param name="model"></param>
        public void CreateApplication (ApplicationModel model, string userId)
        {
            if (db.Applications.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Application);
            }
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.Applications.AsNoTracking().ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listInfi = db.Applications.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listInfi.Count() > 0 && listInfi != null)
                    {
                        foreach (var item in listInfi)
                        {
                            var updateInfi = db.Applications.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateInfi.Index++;
                        }
                    }
                }
                try
                {
                    NTS.Model.Repositories.Application newInfi = new NTS.Model.Repositories.Application
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Note = model.Note.Trim(),
                        Index = model.Index,

                    };
                    db.Applications.Add(newInfi);
                    UserLogUtil.LogHistotyAdd(db, userId, newInfi.Code, newInfi.Id, Constants.LOG_Application);
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
        /// Lấy thông tin dữ liệu theo id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetInfoApplication (string id)
        {

            var resultInfo = db.Applications.AsNoTracking().Where(u => u.Id.Equals(id)).Select(p => new ApplicationModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Index = p.Index,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Application);
            }
            return resultInfo;
        }

        /// <summary>
        /// Cập nhật khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateApplication (ApplicationModel model, string userId)
        {
            if (db.Applications.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Application);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    model.Code = Util.RemoveSpecialCharacter(model.Code);
                    var checkApp = db.Applications.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkApp != null)
                    {
                        var newApp = db.Applications.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                        //var jsonBefore = AutoMapperConfig.Mapper.Map<ApplicationModel>(newApp);


                        int olđInext = newApp.Index;
                        if (checkApp.Index < newApp.Index)
                        {
                            var listInfiChange1 = db.Applications.Where(a => a.Index > checkApp.Index && a.Index < newApp.Index);
                            if (listInfiChange1.Count() > 0)
                            {
                                foreach (var item in listInfiChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkApp.Index++;
                        }

                        if (checkApp.Index > newApp.Index)
                        {

                            checkApp.Index = checkApp.Index - 1;
                        }
                        newApp.Index = model.Index;
                        newApp.Name = model.Name.Trim();
                        newApp.Code = model.Code.Trim();
                        newApp.Note = model.Note.Trim();
                        //var jsonAfter = AutoMapperConfig.Mapper.Map<ApplicationModel>(newApp);
                        //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Application, newApp.Code, newApp.Id, jsonBefore, jsonAfter);
                    }
                    else
                    {
                        var newInfi = db.Applications.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                        //var jsonBefore = AutoMapperConfig.Mapper.Map<ApplicationModel>(newInfi);
                        var listInfi = (from a in db.Applications.AsNoTracking()
                                        orderby a.Index ascending
                                        select new ApplicationModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Code = a.Code,
                                            Index = a.Index,
                                            Note = a.Note
                                        }).AsQueryable();
                        if (newInfi.Index == 1 && listInfi.Count() == 1 && !model.Index.Equals("1"))
                        {
                            throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                        }
                        newInfi.Name = model.Name;
                        newInfi.Code = model.Code;
                        newInfi.Note = model.Note;
                        newInfi.Index = model.Index;
                        //var jsonAfter = AutoMapperConfig.Mapper.Map<ApplicationModel>(newInfi);
                        //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Application, newInfi.Code, newInfi.Id, jsonBefore, jsonAfter);
                    }

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
        /// Delete khóa học
        /// </summary>
        /// <param name="model"></param>
        public void DeleteApplication (string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
               
                var model = db.Applications.FirstOrDefault(a => a.Id.Equals(id));
                var maxIndex = db.Applications.AsNoTracking().Select(a => a.Index).Max();
                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listApplicaton = db.Applications.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listApplicaton.Count() > 0 && listApplicaton != null)
                    {
                        foreach (var item in listApplicaton)
                        {
                            if (!item.Id.Equals(model.Id))
                            {
                                var updateApplication = db.Applications.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                                updateApplication.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var application = db.Applications.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (application == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    }
                    db.Applications.Remove(application);
                    //var jsonBefore = AutoMapperConfig.Mapper.Map<ApplicationModel>(application);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_Application, application.Id, application.Code, jsonBefore);
                    //db.SaveChanges();
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
