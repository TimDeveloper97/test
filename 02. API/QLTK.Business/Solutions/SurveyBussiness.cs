using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Survey;
using NTS.Model.Customers;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.SurveyContent;
using Newtonsoft.Json;
using RabbitMQ.Client.Framing.Impl;

namespace QLTK.Business.Solutions
{
    public class SurveyBussiness
    {
        private QLTKEntities db = new QLTKEntities();


        public SearchResultModel<SurveySearchResultModel> SearchSurvey(SurveySearchResultModel modelSearch)
        {
            SearchResultModel<SurveySearchResultModel> searchResult = new SearchResultModel<SurveySearchResultModel>();

            var dataQuery = (from a in db.Surveys.AsNoTracking()
                             join b in db.CustomerRequirements.AsNoTracking() on a.CustomerRequirementId equals b.Id
                             //join c in db.ProjectPhases.AsNoTracking() on a.ProjectPhaseId equals c.Id
                             where !modelSearch.ListSurveyId.Contains(a.Id)
                             select new SurveySearchResultModel
                             {
                                 Id = a.Id,
                                 CustomerRequirementId = a.CustomerRequirementId,
                                 SurveyDate = a.SurveyDate,
                                 Times = a.Times,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.CustomerRequirementId))
            {
                dataQuery = dataQuery.Where(t => t.CustomerRequirementId.Equals(modelSearch.CustomerRequirementId));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.OrderBy(t => t.CustomerRequirementId).ToList();
            if (searchResult.ListResult.Count > 0)
            {
                foreach (var item in searchResult.ListResult)
                {
                    item.Time = JsonConvert.SerializeObject(item.Times);
                }
            }
            return searchResult;
        }

        public void CreateSurvey(SurveyCreateModel model)
        {
            List<SurveyTool> ListMaterial = new List<SurveyTool>();
            List<SurveyUser> listEmployee = new List<SurveyUser>();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Survey survey = new Survey
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerRequirementId = model.CustomerRequirementId,
                        SurveyDate = model.SurveyDate,
                        Times = JsonConvert.SerializeObject(model.Time),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        CustomerContactId = model.CustomerContactId,
                        Description = model.Description,
                    };

                    db.Surveys.Add(survey);

                    if (model.ListRequest.Count > 0)
                    {
                        foreach (var item in model.ListRequest)
                        {

                            NTS.Model.Repositories.SurveyContent surveyContent = new NTS.Model.Repositories.SurveyContent()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Content = item.Content,
                                Result = item.Result,
                                SurveyId = survey.Id,
                                Level = item.Level,
                            };

                            db.SurveyContents.Add(surveyContent);

                            if (!string.IsNullOrEmpty(item.EmployeeId))
                            {

                                NTS.Model.Repositories.SurveyContentUser surveyContentUser = new NTS.Model.Repositories.SurveyContentUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyContentId = surveyContent.Id,
                                    UserId = item.EmployeeId,

                                };
                                db.SurveyContentUsers.Add(surveyContentUser);
                            }

                            if (item.ListSurveyContentAttach.Count > 0)
                            {
                                List<SurveyContentAttach> surveyContentAttachModels = new List<SurveyContentAttach>();
                                foreach (var contentAttach in item.ListSurveyContentAttach)
                                {
                                    if (contentAttach.FilePath != null && contentAttach.FilePath != "")
                                    {
                                        NTS.Model.Repositories.SurveyContentAttach surveyContentAttach = new NTS.Model.Repositories.SurveyContentAttach()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SurveyContentId = surveyContent.Id,
                                            Name = contentAttach.Name,
                                            Note = contentAttach.Note,
                                            Type = contentAttach.Type,
                                            FileSize = contentAttach.FileSize,
                                            CreateDate = DateTime.Now,
                                            CreateBy = model.CreateBy,
                                            UpdateBy = model.CreateBy,
                                            FileName = contentAttach.FileName,
                                            FilePath = contentAttach.FilePath,
                                            UpdateDate = DateTime.Now
                                        };
                                        surveyContentAttachModels.Add(surveyContentAttach);
                                    }
                                }
                                db.SurveyContentAttaches.AddRange(surveyContentAttachModels);
                            }
                        }
                    }


                    if (model.ListMaterial.Count > 0)
                    {
                        foreach (var item in model.ListMaterial)
                        {
                            SurveyTool surveyTool = new SurveyTool()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SurveyId = survey.Id,
                                MaterialId = item.Id,
                                Note = item.Note,
                                Quantity = item.Quantity,
                            };
                            ListMaterial.Add(surveyTool);
                        }
                        db.SurveyTools.AddRange(ListMaterial);
                    }

                    if (model.ListUser.Count > 0)
                    {
                        foreach (var item in model.ListUser)
                        {
                            SurveyUser surveyUser = new SurveyUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SurveyId = survey.Id,
                                UserId = item.Id
                            };
                            listEmployee.Add(surveyUser);
                        }
                        db.SurveyUsers.AddRange(listEmployee);
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

        public void DeleteSurvey(string id, string userId)
        {
            var request = db.Surveys.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Survey);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.SurveyUsers.Where(a => a.SurveyId.Equals(id));
                    var tool = db.SurveyTools.Where(b => b.SurveyId.Equals(id));
                    var content = db.SurveyContents.Where(c => c.SurveyId.Equals(id));

                    if (content.Count() > 0)
                    {
                        foreach (var item in content)
                        {
                            if (item.SurveyContentAttaches.Count() > 0)
                            {
                                db.SurveyContentAttaches.RemoveRange(item.SurveyContentAttaches);
                            }
                            if (item.SurveyContentUsers.Count() > 0)
                            {
                                db.SurveyContentUsers.RemoveRange(item.SurveyContentUsers);
                            }

                        }
                    }



                    db.SurveyUsers.RemoveRange(user);
                    db.SurveyTools.RemoveRange(tool);
                    db.SurveyContents.RemoveRange(content);
                    db.Surveys.Remove(request);

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

        public void UpdatetSurvey(SurveyCreateModel model, string userId)
        {
            List<SurveyUser> listEmployee = new List<SurveyUser>();
            List<SurveyTool> listMaterial = new List<SurveyTool>();
            List<SurveyContentUser> listContentUer = new List<SurveyContentUser>();
            var SurveyUpdate = db.Surveys.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (SurveyUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Survey);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    //SurveyUpdate.ProjectPhaseId = model.ProjectPhaseId;
                    SurveyUpdate.Times = JsonConvert.SerializeObject(model.Time); ;
                    SurveyUpdate.SurveyDate = model.SurveyDate;
                    SurveyUpdate.UpdateBy = userId;
                    SurveyUpdate.UpdateDate = DateTime.Now;

                    var request = db.SurveyContents.Where(t => t.SurveyId.Equals(model.Id)).ToList();
                    if (request.Count > 0)
                    {
                        db.SurveyContents.RemoveRange(request);
                    }
                    if (model.ListRequest.Count > 0)
                    {
                        foreach (var item in model.ListRequest)
                        {
                            NTS.Model.Repositories.SurveyContent surveyContent = new NTS.Model.Repositories.SurveyContent()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SurveyId = model.Id,
                                Content = item.Content,
                                Result = item.Result,
                                Level = item.Level,
                            };
                            db.SurveyContents.Add(surveyContent);

                            if (item.ListSurveyContentAttach.Count > 0)
                            {
                                SurveyContentAttach surveyContentAttach;
                                foreach (var contentAttach in item.ListSurveyContentAttach)
                                {
                                    if (string.IsNullOrEmpty(item.Id))
                                    {
                                        surveyContentAttach = new SurveyContentAttach()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SurveyContentId = model.Id,
                                            Name = contentAttach.Name,
                                            Type = contentAttach.Type,
                                            Note = contentAttach.Note,
                                            FilePath = contentAttach.FilePath,
                                            FileName = contentAttach.FileName,
                                            FileSize = contentAttach.FileSize,
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
                                            if (contentAttach.IsDelete)
                                            {
                                                db.SurveyContentAttaches.Remove(surveyContentAttach);
                                            }
                                            else
                                            {
                                                surveyContentAttach.Name = contentAttach.Name;
                                                surveyContentAttach.Note = contentAttach.Note;
                                                surveyContentAttach.Type = contentAttach.Type;
                                                surveyContentAttach.FileName = contentAttach.FileName;
                                                surveyContentAttach.FilePath = contentAttach.FilePath;
                                                surveyContentAttach.FileSize = contentAttach.FileSize;
                                                surveyContentAttach.UpdateBy = userId;
                                                surveyContentAttach.UpdateDate = DateTime.Now;
                                            }
                                        }
                                    }
                                }
                            }

                            var user = db.SurveyContentUsers.FirstOrDefault(u => u.SurveyContentId.Equals(item.Id));
                            {
                                if (user != null)
                                {
                                    db.SurveyContentUsers.Remove(user);
                                }
                            }
                            if (!string.IsNullOrEmpty(item.EmployeeId))
                            {

                                NTS.Model.Repositories.SurveyContentUser surveyContentUser = new NTS.Model.Repositories.SurveyContentUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyContentId = surveyContent.Id,
                                    UserId = item.EmployeeId,
                                };
                                db.SurveyContentUsers.Add(surveyContentUser);
                            }
                        }
                    }


                    var users = db.SurveyUsers.Where(t => t.SurveyId.Equals(model.Id)).ToList();
                    if (users.Count > 0)
                    {
                        db.SurveyUsers.RemoveRange(users);
                    }
                    if (model.ListUser.Count > 0)
                    {
                        foreach (var item in model.ListUser)
                        {
                            if (item.IsNew)
                            {
                                SurveyUser surveyUser = new SurveyUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = SurveyUpdate.Id,
                                    UserId = item.Id
                                };
                                listEmployee.Add(surveyUser);
                            }
                            else
                            {
                                SurveyUser surveyUser = new SurveyUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = SurveyUpdate.Id,
                                    UserId = item.UserId
                                };
                                listEmployee.Add(surveyUser);
                            }

                        }
                        db.SurveyUsers.AddRange(listEmployee);
                    }

                    var tool = db.SurveyTools.Where(t => t.SurveyId.Equals(model.Id)).ToList();
                    if (tool.Count > 0)
                    {
                        db.SurveyTools.RemoveRange(tool);
                    }
                    if (model.ListMaterial.Count > 0)
                    {
                        foreach (var item in model.ListMaterial)
                        {
                            if (item.IsNew)
                            {
                                SurveyTool surveyTool = new SurveyTool()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = SurveyUpdate.Id,
                                    MaterialId = item.Id,
                                    Note = item.Note,
                                    Quantity = item.Quantity,
                                    Code = item.Code,
                                    Name = item.Name,
                                };
                                listMaterial.Add(surveyTool);
                            }
                            else
                            {
                                SurveyTool surveyTool = new SurveyTool()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = SurveyUpdate.Id,
                                    MaterialId = item.MaterialId,
                                    Note = item.Note,
                                    Quantity = item.Quantity,
                                    Code = item.Code,
                                    Name = item.Name
                                };
                                listMaterial.Add(surveyTool);
                            }

                        }
                        db.SurveyTools.AddRange(listMaterial);
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

        public SurveyInfoModel GetSurveyById(string id)
        {
            var request = (from a in db.Surveys.AsNoTracking()
                           where a.Id.Equals(id)
                           select new SurveyInfoModel
                           {
                               Id = a.Id,
                               CustomerRequirementId = a.CustomerRequirementId,
                               //ProjectPhaseId = a.ProjectPhaseId,
                               SurveyDate = a.SurveyDate,
                               Times = a.Times,
                               CustomerContactId = a.CustomerContactId,
                               Description = a.Description

                           }).FirstOrDefault();
            if (request != null)
            {
                request.Time = JsonConvert.DeserializeObject<object>(request.Times);
            }
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Survey);
            }
            List<CustomerRequirementUserInfoModel> listUsers = new List<CustomerRequirementUserInfoModel>();

            request.ListRequest = (from a in db.SurveyContents.AsNoTracking()
                                   where a.SurveyId.Equals(request.Id)
                                   select new SurveyContentCreateModel
                                   {
                                       Id = a.Id,
                                       Content = a.Content,
                                       Result = a.Result,
                                   }).ToList();

            if (request.ListRequest != null)
            {

                foreach (var item in request.ListRequest)
                {
                    var contentUsers = (from a in db.SurveyContentUsers.AsNoTracking()
                                        join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                        join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                        where a.SurveyContentId.Equals(item.Id)
                                        select c.Name
                                        ).ToList();
                    item.Name = String.Join(", ", contentUsers.ToArray());
                    var listAttach = (from m in db.SurveyContentAttaches.AsNoTracking()
                                      where m.SurveyContentId.Equals(item.Id)
                                      select new SurveyContentAttachModel
                                      {
                                          Id = m.Id,
                                          Type = m.Type,
                                          Name = m.Name,
                                          Note = m.Note,
                                          FilePath = m.FilePath,
                                          FileName = m.FileName,
                                          FileSize = m.FileSize,
                                      }).ToList();
                    var listcontentUsers = (from a in db.SurveyContentUsers.AsNoTracking()
                                            join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                            join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                            join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                                            where a.SurveyContentId.Equals(item.Id)
                                            select new CustomerRequirementUserInfoModel
                                            {
                                                SurveyContentId = a.SurveyContentId,
                                                UserId = a.UserId,
                                                Code = c.Code,
                                                Name = c.Name,
                                                PhoneNumber = c.PhoneNumber,
                                                Email = c.Email,
                                                Status = c.Status,
                                                DepartmentName = d.Name,
                                                IsNew = false
                                            }).FirstOrDefault();
                    if (listcontentUsers != null)
                    {
                        listUsers.Add(listcontentUsers);
                    }
                }
            }


            var ListSurveyUser = (from a in db.SurveyUsers.AsNoTracking()
                                  join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                  join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                  join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                                  join e in db.Surveys.AsNoTracking() on a.SurveyId equals e.Id
                                  where a.SurveyId.Equals(request.Id)
                                  select new CustomerRequirementUserInfoModel
                                  {
                                      SurveyId = a.SurveyId,
                                      UserId = a.UserId,
                                      Code = c.Code,
                                      Name = c.Name,
                                      PhoneNumber = c.PhoneNumber,
                                      Email = c.Email,
                                      Status = c.Status,
                                      DepartmentName = d.Name,
                                      IsNew = false
                                  }).FirstOrDefault();
            if (ListSurveyUser != null)
            {
                listUsers.Add(ListSurveyUser);
            }

            request.ListUser = listUsers;


            request.ListMaterial = (from a in db.SurveyTools.AsNoTracking()
                                    join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                    join c in db.MaterialGroups.AsNoTracking() on b.MaterialGroupId equals c.Id
                                    join d in db.Manufactures.AsNoTracking() on b.ManufactureId equals d.Id
                                    join e in db.Surveys.AsNoTracking() on a.SurveyId equals e.Id
                                    where a.SurveyId.Equals(request.Id)
                                    select new CustomerRequirementMaterialInfoModel
                                    {
                                        Id = b.Id,
                                        SurveyId = a.SurveyId,
                                        MaterialId = a.MaterialId,
                                        Name = b.Name,
                                        Code = b.Code,
                                        MaterialGroupName = c.Name,
                                        ManufactureCode = d.Name,
                                        Pricing = b.Pricing,
                                        Note = a.Note,
                                        Quantity = a.Quantity,
                                    }).ToList();
            return request;
        }

    }
}
