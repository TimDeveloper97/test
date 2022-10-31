using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.ProjectAttch;
using NTS.Model.Repositories;
using NTS.Model.Specialize;
using NTS.Model.SurveyContent;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;

namespace QLTK.Business.SurveyContent
{
    public class SurveyContentBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SurveyContentCreateModel> Search(SurveyContentSearchModel modelSearch)
        {
            SearchResultModel<SurveyContentCreateModel> searchResult = new SearchResultModel<SurveyContentCreateModel>();

            var dataQuery = (from a in db.SurveyContents.AsNoTracking()
                             where a.SurveyId.Equals(modelSearch.SurveyId)
                             select new SurveyContentCreateModel()
                             {
                                 Id = a.Id,
                                 Content = a.Content,
                                 Result = a.Result,
                                 SurveyId = a.SurveyId,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Result))
            {
                dataQuery = dataQuery.Where(u => u.Result.ToUpper().Contains(modelSearch.Result.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Content))
            {
                dataQuery = dataQuery.Where(u => u.Content.ToUpper().Contains(modelSearch.Content.ToUpper()));
            }
            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderBy(t => t.Content).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList(); searchResult.ListResult = listResult;
            //foreach (var item in searchResult.ListResult)
            //{
            //    item.ListUser = db.SurveyContentUsers.AsNoTracking().Where(t => t.SurveyContentId.Equals(item.Id)).Select(m => new CustomerRequirementUserInfoModel
            //    {
            //        Id = m.Id,
            //        SurveyContentId = m.SurveyContentId,
            //        UserId = m.UserId,
            //    }).ToList();
            //}
            return searchResult;
        }

        public SurveyContentCreateModel Get(string id)
        {
            var resultInfo = db.SurveyContents.AsNoTracking().Where(u => u.Id.Equals(id)).Select(p => new SurveyContentCreateModel()
            {
                Id = p.Id,
                Content = p.Content,
                Result = p.Result,
                SurveyId = p.SurveyId,

            }).FirstOrDefault();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyContent);
            }
            resultInfo.ListSurveyContentAttach = (from m in db.SurveyContentAttaches.AsNoTracking()
                                                  where m.SurveyContentId.Equals(id)
                                                  select new SurveyContentAttachModel
                                                  {
                                                      Id = m.Id,
                                                      //Type = m.Type,
                                                      Name = m.Name,
                                                      Note = m.Note,
                                                      FilePath = m.FilePath,
                                                      FileName = m.FileName,
                                                      FileSize = m.FileSize,
                                                  }).ToList();
            resultInfo.EmployeeId = (from m in db.SurveyContentUsers.AsNoTracking()
                                     where m.SurveyContentId.Equals(id)
                                     select m.UserId.ToString()).FirstOrDefault();


            return resultInfo;
        }

        public SurveyContentCreateModel Create(SurveyContentCreateModel model)
        {
            List<SurveyContentUser> listEmployee = new List<SurveyContentUser>();
            using (var trans = db.Database.BeginTransaction())
            {
                string id;
                try
                {
                    NTS.Model.Repositories.SurveyContent newSurveyContent = new NTS.Model.Repositories.SurveyContent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = model.Content.Trim(),
                        Result = model.Result.Trim(),
                        SurveyId = model.SurveyId,
                    };

                    db.SurveyContents.Add(newSurveyContent);

                    if (model.ListSurveyContentAttach.Count > 0)
                    {
                        List<SurveyContentAttach> listFileEntity = new List<SurveyContentAttach>();
                        foreach (var item in model.ListSurveyContentAttach)
                        {
                            if (item.FilePath != null && item.FilePath != "")
                            {
                                SurveyContentAttach fileEntity = new SurveyContentAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyContentId = newSurveyContent.Id,
                                    Name = item.Name,
                                    Note = item.Note,
                                    Type = 0,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now,
                                    CreateBy = model.CreateBy,
                                    UpdateBy = model.CreateBy,
                                    FileName = item.FileName,
                                    FilePath = item.FilePath,
                                    UpdateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.SurveyContentAttaches.AddRange(listFileEntity);
                    }

                    if (!string.IsNullOrEmpty(model.EmployeeId))
                    {
                        
                            SurveyContentUser surveyContentUser = new SurveyContentUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SurveyContentId = newSurveyContent.Id,
                                UserId = model.EmployeeId
                            };
                            listEmployee.Add(surveyContentUser);
                        db.SurveyContentUsers.AddRange(listEmployee);
                    }


                    id = newSurveyContent.Id;
                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newSurveyContent.Result, newSurveyContent.Content, Constants.LOG_SurveyMaterial);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
                var resultInfo = db.SurveyContents.Where(u => u.Id.Equals(id)).Select(p => new SurveyContentCreateModel()
                {
                    Id = p.Id,
                    Content = p.Content,
                    Result = p.Result,
                }).FirstOrDefault();
                return resultInfo;
            }

        }

        public void Update(SurveyContentCreateModel model, string userId)
        {
            List<SurveyContentUser> listEmployee = new List<SurveyContentUser>();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSurveyContent = db.SurveyContents.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                   //var jsonApter = AutoMapperConfig.Mapper.Map<SurveyContentCreateModel>(newSurveyContent);

                    newSurveyContent.Content = model.Content.Trim();
                    newSurveyContent.Result = model.Result.Trim();

                    SurveyContentAttach surveyContentAttach;
                    foreach (var item in model.ListSurveyContentAttach)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            surveyContentAttach = new SurveyContentAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SurveyContentId = model.Id,
                                Name = item.Name,
                                Type = 0,
                                Note = item.Note,
                                FilePath = item.FilePath,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now,
                            };

                            db.SurveyContentAttaches.Add(surveyContentAttach);
                        }
                        else
                        {
                            surveyContentAttach = db.SurveyContentAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));

                            if (surveyContentAttach != null)
                            {
                                if (item.IsDelete)
                                {
                                    db.SurveyContentAttaches.Remove(surveyContentAttach);
                                }
                                else
                                {
                                    surveyContentAttach.Name = item.Name;
                                    surveyContentAttach.Note = item.Note;
                                    surveyContentAttach.Type = 0;
                                    surveyContentAttach.FileName = item.FileName;
                                    surveyContentAttach.FilePath = item.FilePath;
                                    surveyContentAttach.FileSize = item.FileSize;
                                    surveyContentAttach.UpdateBy = userId;
                                    surveyContentAttach.UpdateDate = DateTime.Now;
                                }
                            }
                        }
                    }

                    var users = db.SurveyContentUsers.Where(t => t.SurveyContentId.Equals(model.Id)).ToList();
                    if (users.Count > 0)
                    {
                        db.SurveyContentUsers.RemoveRange(users);
                    }
                    if (!string.IsNullOrEmpty(model.EmployeeId))
                    {
                        
                                SurveyContentUser surveyContentUser = new SurveyContentUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyContentId = model.Id,
                                    UserId = model.EmployeeId
                                };
                                listEmployee.Add(surveyContentUser);

                        db.SurveyContentUsers.AddRange(listEmployee);
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

        public void Delete(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var _SurveyContent = db.SurveyContents.FirstOrDefault(a => a.Id.Equals(id));
                    if (_SurveyContent == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyContent);
                    }

                    var attachs = db.SurveyContentAttaches.Where(a => a.SurveyContentId.Equals(id));
                    var users = db.SurveyContentUsers.Where(a => a.SurveyContentId.Equals(id));

                    db.SurveyContentAttaches.RemoveRange(attachs);
                    db.SurveyContentUsers.RemoveRange(users);
                    db.SurveyContents.Remove(_SurveyContent);

                    var NameOrCode = _SurveyContent.Content;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }

        public SearchResultModel<SurveyContentAttachModel> SearchDocumentFile(SurveyContentAttachModel searchModel)
        {
            SearchResultModel<SurveyContentAttachModel> searchResult = new SearchResultModel<SurveyContentAttachModel>();

            var dataQuery = (from a in db.SurveyContentAttaches.AsNoTracking()
                             where a.Id.Equals(searchModel.Id)
                             select new SurveyContentAttachModel
                             {
                                 Id = a.Id,
                                 SurveyContentId = a.SurveyContentId,
                                 FileName = a.FileName,
                                 FilePath = a.FilePath,
                                 Name = a.Name
                             }).AsQueryable();


            var listDocumenFile = dataQuery.ToList();

            searchResult.ListResult.AddRange(listDocumenFile);


            searchResult.TotalItem = searchResult.ListResult.Count();
            return searchResult;
        }


    }



}
