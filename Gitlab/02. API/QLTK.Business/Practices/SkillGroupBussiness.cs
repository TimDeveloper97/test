using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SkillGroup;
using NTS.Utils;
using Syncfusion.XlsIO.Implementation.XmlSerialization;

namespace QLTK.Business.SkillGroup
{
    public class SkillGroup
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SkillGroupResultModel> GetListSkillGroup(SkillGroupSearchModel modelSearch)
        {
            SearchResultModel<SkillGroupResultModel> searchResult = new SearchResultModel<SkillGroupResultModel>();
            try
            {
                var listSkillGroup = (from o in db.SkillGroups.AsNoTracking()
                                      orderby o.Name
                                       select new SkillGroupResultModel
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Note = o.Note,
                                           Name = o.Name,
                                           ParentId = o.ParentId,
                                       }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Id))
                {
                    listSkillGroup = listSkillGroup.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
                }

                List<SkillGroupResultModel> listResult = new List<SkillGroupResultModel>();
                var listParent = listSkillGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<SkillGroupResultModel> listChild = new List<SkillGroupResultModel>();
                foreach (var parent in listParent)
                {
                    isSearch = true;
                    if (!string.IsNullOrEmpty(modelSearch.Name) && !parent.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                    {
                        isSearch = false;
                    }
                    if (!string.IsNullOrEmpty(modelSearch.Code) && !parent.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                    {
                        isSearch = false;
                    }

                    listChild = GetSkillGroupChild(parent.Id, listSkillGroup.ToList(), modelSearch, index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }
                searchResult.TotalItem = listResult.Count();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return searchResult;
        }

        private List<SkillGroupResultModel> GetSkillGroupChild(string parentId,
          List<SkillGroupResultModel> listSkillGroup, SkillGroupSearchModel modelSearch, string index)
        {
            List<SkillGroupResultModel> listResult = new List<SkillGroupResultModel>();
            var listChild = listSkillGroup.Where(r => parentId.Equals(r.ParentId)).ToList();
            bool isSearch = false;
            int indexChild = 1;
            List<SkillGroupResultModel> listChildChild = new List<SkillGroupResultModel>();

            foreach (var child in listChild)
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.Name) && !child.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                {
                    isSearch = false;
                }
                if (!string.IsNullOrEmpty(modelSearch.Code) && !child.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                {
                    isSearch = false;
                }

                listChildChild = GetSkillGroupChild(child.Id, listSkillGroup, modelSearch, index + "." + indexChild);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    listResult.Add(child);
                    indexChild++;
                }
                listResult.AddRange(listChildChild);
            }
            return listResult;
        }

        public SearchResultModel<SkillGroupResultModel> SearchSkillGroup(SkillGroupSearchModel modelSearch)
        {
            SearchResultModel<SkillGroupResultModel> searchResult = new SearchResultModel<SkillGroupResultModel>();

            var dataQuery = (from a in db.SkillGroups.AsNoTracking()
                orderby a.Name
                select new SkillGroupResultModel()
                {
                    Id = a.Id,
                    ParentId = a.ParentId,
                    Name = a.Name,
                    Code = a.Code,
                    Note = a.Note,
                    CreateBy = a.CreateBy,
                    CreateDate = a.CreateDate,
                    UpdateBy = a.UpdateBy,
                    UpdateDate = a.UpdateDate,
                }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where((u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper())));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
                .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        private List<SkillGroupResultModel> GetSkillGroupChild(string parentId, List<SkillGroupResultModel> listDocumentTemplateType)
        {
            List<SkillGroupResultModel> listResult = new List<SkillGroupResultModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<SkillGroupResultModel> listChildChild = new List<SkillGroupResultModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetSkillGroupChild(child.Id, listDocumentTemplateType);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public List<SkillGroupResultModel> GetSkillGroupById(string Id)
        {
            List<SkillGroupResultModel> search = new List<SkillGroupResultModel>();
            List<SkillGroupResultModel> searchResult = new List<SkillGroupResultModel>();
            try
            {
                var listSkillGroup = (from o in db.SkillGroups.AsNoTracking().OrderBy(r => r.Name)
                                        select new SkillGroupResultModel
                                        {
                                            Id = o.Id,
                                            Name = o.Name,
                                            ParentId = o.ParentId,
                                            Note = o.Note,
                                            CreateBy = o.CreateBy,
                                            CreateDate = o.CreateDate,
                                            UpdateBy = o.UpdateBy,
                                            UpdateDate = o.UpdateDate,
                                        }).AsQueryable();
                List<SkillGroupResultModel> listResult = new List<SkillGroupResultModel>();
                var listParent = listSkillGroup.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<SkillGroupResultModel> listChild = new List<SkillGroupResultModel>();
                foreach (var parent in listParent)
                {
                    listChild = GetSkillGroupChild(parent.Id, listSkillGroup.ToList());
                    if (listChild.Count >= 0)
                    {
                        listResult.Add(parent);

                    }
                    listResult.AddRange(listChild);
                }
                search = listResult;
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return search.ToList();
        }
        public SkillGroupModel GetSkillGroupInfo(SkillGroupModel model)
        {
            var skillGroupInfo = db.SkillGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (skillGroupInfo == null)
            {
                throw new Exception("Nhóm kỹ này đã bị xóa bởi người dùng khác");
            }
            try
            {
                model.Id = skillGroupInfo.Id;
                model.Name = skillGroupInfo.Name;
                model.Code = skillGroupInfo.Code;
                model.Note = skillGroupInfo.Note;
                model.ParentId = skillGroupInfo.ParentId;
                model.CreateBy = skillGroupInfo.CreateBy;
                model.CreateDate = skillGroupInfo.CreateDate;
                model.UpdateBy = skillGroupInfo.UpdateBy;
                model.UpdateDate = skillGroupInfo.UpdateDate;

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public SkillGroupModel GetByIdSkillGroup(SkillGroupModel model)
        {
            var resultInfo = db.SkillGroups.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new SkillGroupModel()
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SkillGroup);
            }

            return resultInfo;
        }

        public void AddSkilGroup(SkillGroupModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if(string.IsNullOrEmpty(model.ParentId))
                    {
                        model.ParentId = null;
                    }
                    NTS.Model.Repositories.SkillGroup newSkillGroup = new NTS.Model.Repositories.SkillGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = model.ParentId,
                        Code = model.Code,
                        Name = model.Name,
                        Note = model.Note,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.SkillGroups.Add(newSkillGroup);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void UpdateSkillGroup(SkillGroupModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSkillGroup = db.SkillGroups.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    newSkillGroup.Id = model.Id;
                    newSkillGroup.ParentId = model.ParentId;
                    newSkillGroup.Code = model.Code;
                    newSkillGroup.Name = model.Name;
                    newSkillGroup.Note = model.Note;
                    newSkillGroup.UpdateDate = DateTime.Now;
                    newSkillGroup.UpdateBy = model.UpdateBy;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void DeleteSkillGroup(SkillGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var skillGroup = db.Skills.AsNoTracking().Where(m => m.SkillGroupId.Equals(model.Id)).FirstOrDefault();
                if (skillGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SkillGroup);
                }

                try
                {
                    var _newSkillGroup = db.SkillGroups.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_newSkillGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SkillGroup);
                    }

                    db.SkillGroups.Remove(_newSkillGroup);
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

        public List<SkillGroupResultModel> GetSkillGroupExcepted(SkillGroupSearchModel modelSearch)
        {
            List<SkillGroupResultModel> listExpected = new List<SkillGroupResultModel>();
            List<SkillGroupResultModel> listAll = new List<SkillGroupResultModel>();
            SkillGroupSearchModel modelAll = new SkillGroupSearchModel();
            listAll = GetListSkillGroup(modelAll).ListResult;

            List<SkillGroupResultModel> listById = new List<SkillGroupResultModel>();
            listById = GetSkillGroupById(modelSearch.Id);

            foreach (var itemAll in listAll)
            {
                foreach (var itemById in listById)
                {
                    if (itemAll.Id.Equals(itemById.Id))
                    {
                        listExpected.Add(itemAll);
                    }
                }
            }
            return listAll.Except(listExpected).ToList();
        }

        public void CheckExistedForAdd(SkillGroupModel model)
        {
            if (db.SkillGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SkillGroup);
            }

            if (db.SkillGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SkillGroup);
            }
        }

        public void CheckExistedForUpdate(SkillGroupModel model)
        {
            if (db.SkillGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SkillGroup);
            }

            if (db.SkillGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SkillGroup);
            }

        }
    }
}
