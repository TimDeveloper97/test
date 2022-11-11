using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.ProjectPhase;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Solutions
{
    public class ProjectPhaseBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public object SearchProjectPhase(ProjectPhaseSearchModel modelSearch)
        {
            SearchResultModel<ProjectPhaseModel> searchResult = new SearchResultModel<ProjectPhaseModel>();
            try
            {
                List<ProjectPhaseModel> list = new List<ProjectPhaseModel>();
                var dataQuery = (from a in db.ProjectPhases.AsNoTracking()
                                 join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id
                                 orderby a.Code
                                 select new ProjectPhaseModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     SBUId = b.Name,
                                     ParentId = a.ParentId,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.Name) || !string.IsNullOrEmpty(modelSearch.Code) || !string.IsNullOrEmpty(modelSearch.SBUId))
                {
                    list = dataQuery.ToList();
                }
                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    list = list.Where(t => t.Code.Contains(modelSearch.Code)).ToList();
                }

                if (!string.IsNullOrEmpty(modelSearch.SBUId))
                {
                    list = list.Where(t => t.SBUId.Equals(modelSearch.SBUId)).ToList();
                }

                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    list = list.Where(t => t.Name.Contains(modelSearch.Name)).ToList();
                }
                List<ProjectPhaseModel> listRs = new List<ProjectPhaseModel>();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var checkExist = listRs.FirstOrDefault(t => item.Id.Equals(t.Id));
                        if (checkExist == null)
                        {
                            listRs.Add(item);
                        }
                        if (!string.IsNullOrEmpty(item.ParentId))
                        {
                            var entity = dataQuery.FirstOrDefault(t => item.ParentId.Equals(t.Id));
                            if (entity != null)
                            {
                                var check = listRs.FirstOrDefault(t => entity.Id.Equals(t.Id));
                                if (check == null)
                                {
                                    listRs.Add(entity);
                                }
                                if (!string.IsNullOrEmpty(entity.ParentId))
                                {
                                    listRs = GetMaterialGroupParent(entity.ParentId, listRs, dataQuery.ToList());
                                }
                            }
                        }
                    }
                }
                else if (string.IsNullOrEmpty(modelSearch.Name) && string.IsNullOrEmpty(modelSearch.Code))
                {
                    listRs = dataQuery.ToList();
                }
                else
                {
                    listRs = list.ToList();
                }
                var listResult = listRs.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listResult.Count;
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        private List<ProjectPhaseModel> GetMaterialGroupParent(string parentId, List<ProjectPhaseModel> listRs, List<ProjectPhaseModel> data)
        {
            var entity = data.FirstOrDefault(t => parentId.Equals(t.Id));
            if (entity != null)
            {
                var check = listRs.FirstOrDefault(t => entity.Id.Equals(t.Id));
                if (check == null)
                {
                    listRs.Add(entity);
                }
                if (!string.IsNullOrEmpty(entity.ParentId))
                {
                    GetMaterialGroupParent(entity.ParentId, listRs, data);
                }
            }
            return listRs;
        }

        public void DeleteProjectPhase(ProjectPhaseModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                
                if (db.ProjectPhases.AsNoTracking().Where(r => r.ParentId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectPhase);
                }
                try
                {
                    var projectPhase = db.ProjectPhases.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (projectPhase == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectPhase);
                    }
                    db.ProjectPhases.Remove(projectPhase);

                    var NameOrCode = projectPhase.Name;

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

        public object GetProjectPhase(ProjectPhaseModel model)
        {
            var resuldInfor = db.ProjectPhases.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProjectPhaseModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                SBUId = p.SBUId,
                ParentId = p.ParentId,
            }).FirstOrDefault();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectPhase);
            }
            return resuldInfor;
        }

        public void AddProjectPhase(ProjectPhaseModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên nhóm việc làm đã tồn tại chưa
                if (db.ProjectPhases.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProjectPhase);
                }
                // check mã nhóm việc làm đã tồn tại chưa
                if (db.ProjectPhases.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProjectPhase);
                }
                try
                {
                    ProjectPhase projectPhase = new ProjectPhase()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        SBUId = model.SBUId,
                        ParentId = model.ParentId,
                    };
                    db.ProjectPhases.Add(projectPhase);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, projectPhase.Code, projectPhase.Id, Constants.LOG_ProjectPhase);

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

        public void UpdateProjectPhase(ProjectPhaseModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.ProjectPhases.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProjectPhase);
                }

                if (db.ProjectPhases.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProjectPhase);
                }
                try
                {
                    var newProjectPhases = db.ProjectPhases.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    newProjectPhases.Name = model.Name.NTSTrim();
                    newProjectPhases.Code = model.Code.NTSTrim();
                    newProjectPhases.ParentId = model.ParentId;
                    newProjectPhases.SBUId = model.SBUId;

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
