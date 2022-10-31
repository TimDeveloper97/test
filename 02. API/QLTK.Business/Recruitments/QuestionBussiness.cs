using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Question;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Question
{
    public class QuestionBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm câu hỏi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<QuestionSearchResultModel> SearchQuestion(QuestionSearchModel searchModel)
        {
            SearchResultModel<QuestionSearchResultModel> searchResult = new SearchResultModel<QuestionSearchResultModel>();
            var dataQuery = (from a in db.Questions.AsNoTracking()
                             join b in db.QuestionGroups.AsNoTracking() on a.QuestionGroupId equals b.Id
                             orderby a.CreateBy descending
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

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();

            foreach (var item in searchResult.ListResult)
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

            return searchResult;
        }

        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void DeleteQuestion(QuestionModel model)
        {
            var questionExist = db.Questions.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (questionExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }

            var questionUsed = db.WorkTypeInterviewQuestions.AsNoTracking().FirstOrDefault(a => a.QuestionId.Equals(model.Id));
            if (questionUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Question);
            }

            try
            {
                var questionFiles = db.QuestionFiles.Where(a => a.QuestionId.Equals(model.Id)).ToList();
                db.QuestionFiles.RemoveRange(questionFiles);

                db.Questions.Remove(questionExist);

                var NameOrCode = questionExist.Code;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<QuestionHistoryModel>(questionExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Question, questionExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void CreateQuestion(QuestionModel model)
        {
            model.Code = model.Code.NTSTrim();

            var questionExits = db.Questions.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (questionExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Question);
            }

            try
            {
                NTS.Model.Repositories.Question question = new NTS.Model.Repositories.Question
                {
                    Id = Guid.NewGuid().ToString(),
                    QuestionGroupId = model.QuestionGroupId,
                    Code = model.Code,
                    Type = model.Type,
                    Score = model.Score,
                    Content = model.Content,
                    Answer = model.Answer,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.Questions.Add(question);

                if (model.QuestionFiles.Count > 0)
                {
                    foreach (var item in model.QuestionFiles)
                    {
                        NTS.Model.Repositories.QuestionFile questionFile = new NTS.Model.Repositories.QuestionFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = question.Id,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            FilePath = item.FilePath,
                            CreateBy = model.CreateBy,
                            CreateDate = model.CreateDate,
                            UpdateBy = model.UpdateBy,
                            UpdateDate = model.UpdateDate
                        };
                        db.QuestionFiles.Add(questionFile);
                    }
                }

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, question.Code, question.Id, Constants.LOG_Question);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QuestionModel GetQuestion(QuestionModel model)
        {
            var resultInfo = db.Questions.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new QuestionModel
            {
                Id = p.Id,
                QuestionGroupId = p.QuestionGroupId,
                Code = p.Code,
                Type = p.Type,
                Score = p.Score,
                Content = p.Content,
                Answer = p.Answer,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.CreateBy,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }

            resultInfo.QuestionFiles = (from a in db.QuestionFiles.AsNoTracking()
                                        where a.QuestionId.Equals(resultInfo.Id)
                                        select new QuestionFileModel
                                        {
                                            Id = a.Id,
                                            QuestionId = a.QuestionId,
                                            FileName = a.FileName,
                                            FilePath = a.FilePath,
                                            FileSize = a.FileSize,
                                            CreateDate = a.CreateDate
                                        }).ToList();

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void UpdateQuestion(QuestionModel model)
        {
            model.Code = model.Code.NTSTrim();
            var questionUpdate = db.Questions.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (questionUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }


            var questionCodeExist = db.Questions.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (questionCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Question);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<QuestionHistoryModel>(questionUpdate);


            try
            {
                questionUpdate.Code = model.Code;
                questionUpdate.QuestionGroupId = model.QuestionGroupId;
                questionUpdate.Type = model.Type;
                questionUpdate.Score = model.Score;
                questionUpdate.Content = model.Content;
                questionUpdate.Answer = model.Answer;
                questionUpdate.UpdateBy = model.UpdateBy;
                questionUpdate.UpdateDate = DateTime.Now;

                var questionFilesExist = db.QuestionFiles.Where(a => a.QuestionId.Equals(model.Id)).ToList();
                db.QuestionFiles.RemoveRange(questionFilesExist);

                if (model.QuestionFiles.Count > 0)
                {
                    foreach (var item in model.QuestionFiles)
                    {
                        NTS.Model.Repositories.QuestionFile questionFile = new NTS.Model.Repositories.QuestionFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = questionUpdate.Id,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            FileSize = item.FileSize,
                            CreateBy = model.CreateBy,
                            CreateDate = model.CreateDate,
                            UpdateBy = model.UpdateBy,
                            UpdateDate = model.UpdateDate
                        };
                        db.QuestionFiles.Add(questionFile);
                    }
                }

                //var jsonApter = AutoMapperConfig.Mapper.Map<QuestionHistoryModel>(questionUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Question, questionUpdate.Id, questionUpdate.Code, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

    }
}
