using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.QuestionGroup;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.QuestionGroup
{
    public class QuestionGroupBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm nhóm câu hỏi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<QuestionGroupSearchResultModel> SearchQuestionGroup(QuestionGroupSearchModel searchModel)
        {
            SearchResultModel<QuestionGroupSearchResultModel> searchResult = new SearchResultModel<QuestionGroupSearchResultModel>();
            var dataQuery = (from a in db.QuestionGroups.AsNoTracking()
                             select new QuestionGroupSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        /// <summary>
        /// Xóa nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void DeleteQuestionGroup(QuestionGroupModel model)
        {
            var questionGroupExist = db.QuestionGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (questionGroupExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuestionGroup);
            }

            var questionGoupChildExist = db.QuestionGroups.FirstOrDefault(a => a.ParentId.Equals(model.Id));
            if (questionGoupChildExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.QuestionGroup);
            }

            var questionUsed = db.Questions.AsNoTracking().FirstOrDefault(a => a.QuestionGroupId.Equals(model.Id));
            if (questionUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.QuestionGroup);
            }

            try
            {
                db.QuestionGroups.Remove(questionGroupExist);

                var NameOrCode = questionGroupExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<QuestionGroupHistoryModel>(questionGroupExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_QuestionGroup, questionGroupExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void CreateQuestionGroup(QuestionGroupModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;
            var questionGroupNameExist = db.QuestionGroups.AsNoTracking().FirstOrDefault(a=>a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (questionGroupNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.QuestionGroup);
            }

            var questionGroupCodeExist = db.QuestionGroups.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (questionGroupCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuestionGroup);
            }

            try
            {
                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                NTS.Model.Repositories.QuestionGroup questionGroup = new NTS.Model.Repositories.QuestionGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    ParentId = model.ParentId,
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.QuestionGroups.Add(questionGroup);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, questionGroup.Name, questionGroup.Id, Constants.LOG_QuestionGroup);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QuestionGroupModel GetQuestionGroup(QuestionGroupModel model)
        {
            var resultInfo = db.QuestionGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new QuestionGroupModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuestionGroup);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật nhóm câu hỏi
        /// </summary>
        /// <param name="model"></param>
        public void UpdateQuestionGroup(QuestionGroupModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;

            var questionGroupUpdate = db.QuestionGroups.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (questionGroupUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuestionGroup);
            }

            var questionGroupNameExist = db.QuestionGroups.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (questionGroupNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.QuestionGroup);
            }

            var questionGroupCodeExist = db.QuestionGroups.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (questionGroupCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuestionGroup);
            }

            try
            {
                //var jsonBefor = AutoMapperConfig.Mapper.Map<QuestionGroupHistoryModel>(questionGroupUpdate);

                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                questionGroupUpdate.Name = model.Name;
                questionGroupUpdate.Code = model.Code;
                questionGroupUpdate.ParentId = model.ParentId;
                questionGroupUpdate.Note = model.Note;
                questionGroupUpdate.UpdateBy = model.UpdateBy;
                questionGroupUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<QuestionGroupHistoryModel>(questionGroupUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_QuestionGroup, questionGroupUpdate.Id, questionGroupUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
