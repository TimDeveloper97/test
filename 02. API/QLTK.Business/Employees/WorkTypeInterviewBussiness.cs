using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Question;
using NTS.Model.Repositories;
using NTS.Model.WorkTypeInterview;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.WorkTypeInterview
{
    public class WorkTypeInterviewBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<WorkTypeInterviewSearchResultModel> SearchWorkTypeInterview(WorkTypeInterviewSearchModel searchModel)
        {
            SearchResultModel<WorkTypeInterviewSearchResultModel> searchResult = new SearchResultModel<WorkTypeInterviewSearchResultModel>();
            var dataQuery = (from a in db.WorkTypeInterviews.AsNoTracking()
                             join d in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals d.Id
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id into ab
                             from abv in ab.DefaultIfEmpty()
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             select new WorkTypeInterviewSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 WorkTypeId = a.WorkTypeId,
                                 WorkTypeName = d.Name,
                                 DepartmentId = a.DepartmentId,
                                 SBUId = a.SBUId,
                                 SBUName = abv != null ? abv.Name : "",
                                 DepartmentName = acv != null ? acv.Name : "",
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.WorkTypeId))
            {
                dataQuery = dataQuery.Where(u => u.WorkTypeId.Equals(searchModel.WorkTypeId));
            }


            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="model"></param>
        public void DeleteWorkTypeInterview(WorkTypeInterviewModel model)
        {
            var workTypeInterviewExist = db.WorkTypeInterviews.FirstOrDefault(u => u.Id == model.Id);
            if (workTypeInterviewExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkTypeInterview);
            }

            try
            {
                var workTypeInterviewQuestion = db.WorkTypeInterviewQuestions.Where(a => a.WorkTypeInterviewId == model.Id).ToList();
                db.WorkTypeInterviewQuestions.RemoveRange(workTypeInterviewQuestion);

                db.WorkTypeInterviews.Remove(workTypeInterviewExist);

                var NameOrCode = workTypeInterviewExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkTypeInterviewHistoryModel>(workTypeInterviewExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_BankAccount, workTypeInterviewExist.Id.ToString(), NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="model"></param>
        public void CreateWorkTypeInterview(WorkTypeInterviewModel model)
        {
            model.Name = model.Name.NTSTrim();

            var workTypeInterviewNameExits = db.WorkTypeInterviews.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (workTypeInterviewNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkTypeInterview);
            }

            try
            {
                NTS.Model.Repositories.WorkTypeInterview workTypeInterview = new NTS.Model.Repositories.WorkTypeInterview
                {
                    Name = model.Name,
                    WorkTypeId = model.WorkTypeId,
                    DepartmentId = model.DepartmentId,
                    SBUId = model.SBUId,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.WorkTypeInterviews.Add(workTypeInterview);
                db.SaveChanges();

                foreach (var item in model.Questions)
                {
                    WorkTypeInterviewQuestion workTypeInterviewQuestion = new WorkTypeInterviewQuestion()
                    {
                        WorkTypeInterviewId = workTypeInterview.Id,
                        QuestionId = item.Id
                    };
                    db.WorkTypeInterviewQuestions.Add(workTypeInterviewQuestion);
                }

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, workTypeInterview.Name, workTypeInterview.Id.ToString(), Constants.LOG_WorkTypeInterview);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin phỏng vấn vị trí công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WorkTypeInterviewModel GetWorkTypeInterview(WorkTypeInterviewModel model)
        {
            var resultInfo = db.WorkTypeInterviews.AsNoTracking().Where(u => model.Id == u.Id).Select(p => new WorkTypeInterviewModel
            {
                Id = p.Id,
                Name = p.Name,
                WorkTypeId = p.WorkTypeId,
                DepartmentId = p.DepartmentId,
                SBUId = p.SBUId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkTypeInterview);
            }

            resultInfo.Questions = (from a in db.WorkTypeInterviewQuestions.AsNoTracking()
                                    join b in db.Questions.AsNoTracking() on a.QuestionId equals b.Id
                                    join c in db.QuestionGroups.AsNoTracking() on b.QuestionGroupId equals c.Id
                                    where a.WorkTypeInterviewId.Equals(model.Id)
                                    select new QuestionSearchResultModel
                                    {
                                        Id = b.Id,
                                        Code = b.Code,
                                        QuestionGroupId = b.QuestionGroupId,
                                        QuestionGroupName = c.Name,
                                        Type = b.Type,
                                        Answer = b.Answer,
                                        Content = b.Content,
                                    }).ToList();

            foreach (var item in resultInfo.Questions)
            {
                item.ListFile = (from a in db.QuestionFiles.AsNoTracking()
                                 where a.QuestionId.Equals(item.Id)
                                 select new QuestionFileModel
                                 {
                                     Id = a.Id,
                                     FileName = a.FileName,
                                     FileSize = a.FileSize,
                                     FilePath = a.FilePath
                                 }).ToList();
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật ngân hàng
        /// </summary>
        /// <param name="model"></param>
        public void UpdateWorkTypeInterview(WorkTypeInterviewModel model)
        {
            model.Name = model.Name.NTSTrim();

            var workTypeInterviewUpdate = db.WorkTypeInterviews.FirstOrDefault(a => a.Id == model.Id);
            if (workTypeInterviewUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkTypeInterview);
            }

            var workTypeInterviewNameExist = db.WorkTypeInterviews.AsNoTracking().FirstOrDefault(a => a.Id != model.Id && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (workTypeInterviewNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkTypeInterview);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkTypeInterviewHistoryModel>(workTypeInterviewUpdate);

            try
            {
                workTypeInterviewUpdate.Name = model.Name;
                workTypeInterviewUpdate.WorkTypeId = model.WorkTypeId;
                workTypeInterviewUpdate.DepartmentId = model.DepartmentId;
                workTypeInterviewUpdate.SBUId = model.SBUId;

                workTypeInterviewUpdate.UpdateBy = model.UpdateBy;
                workTypeInterviewUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<WorkTypeInterviewHistoryModel>(workTypeInterviewUpdate);

                var questions = db.WorkTypeInterviewQuestions.Where(a => a.WorkTypeInterviewId == model.Id).ToList();
                db.WorkTypeInterviewQuestions.RemoveRange(questions);
                foreach (var item in model.Questions)
                {
                    WorkTypeInterviewQuestion workTypeInterviewQuestion = new WorkTypeInterviewQuestion()
                    {
                        WorkTypeInterviewId = workTypeInterviewUpdate.Id,
                        QuestionId = item.Id
                    };
                    db.WorkTypeInterviewQuestions.Add(workTypeInterviewQuestion);
                }

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_BankAccount, workTypeInterviewUpdate.Id.ToString(), workTypeInterviewUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public SearchResultModel<QuestionSearchResultModel> SearchQuestion(QuestionSearchModel searchModel)
        {
            SearchResultModel<QuestionSearchResultModel> search = new SearchResultModel<QuestionSearchResultModel>();

            var dataQuery = (from a in db.Questions.AsNoTracking()
                             join b in db.QuestionGroups.AsNoTracking() on a.QuestionGroupId equals b.Id
                             where !searchModel.ListIdSelect.Contains(a.Id.ToString())
                             orderby a.Code
                             select new QuestionSearchResultModel
                             {
                                 Id = a.Id,
                                 QuestionGroupId = a.QuestionGroupId,
                                 QuestionGroupName = b.Name,
                                 Code = a.Code,
                                 Content = a.Content,
                                 Type = a.Type,
                                 Answer = a.Answer,
                                 CreateDate = a.CreateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.QuestionGroupId))
            {
                dataQuery = dataQuery.Where(u => u.QuestionGroupId.Equals(searchModel.QuestionGroupId));
            }

            if (searchModel.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == searchModel.Type);
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();

            foreach (var item in search.ListResult)
            {
                item.ListFile = (from a in db.QuestionFiles.AsNoTracking()
                                 where a.QuestionId.Equals(item.Id)
                                 select new QuestionFileModel
                                 {
                                     Id = a.Id,
                                     FileName = a.FileName,
                                     FileSize = a.FileSize,
                                     FilePath = a.FilePath
                                 }).ToList();
            }

            return search;
        }
    }
}
