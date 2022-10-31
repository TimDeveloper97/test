using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model;
using NTS.Model.Combobox;
using System.Web;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.Common;
using NTS.Model.ProductStandards;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ModuleGroupProductStandard;
using NTS.Model.Stage;
using NTS.Model.GroupModule;

namespace QLTK.Business.QLTKGROUPMODUL
{
    public class GroupModuleBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultModel<TreeNode<GroupModuleModel>> SearchGroupModul(GroupModuleSearchModel modelSearch)
        {
            SearchResultModel<TreeNode<GroupModuleModel>> searchResult = new SearchResultModel<TreeNode<GroupModuleModel>>();
            try
            {
                var listGroup = (from a in db.ModuleGroups.AsNoTracking()
                                 select new GroupModuleModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ParentId = a.ParentId,
                                 }).ToList();
                var listParentGroup = from r in listGroup
                                      where string.IsNullOrEmpty(r.ParentId)
                                      select r;

                TreeNode<GroupModuleModel> parentNode;
                TreeNode<GroupModuleModel> childNode;
                List<TreeNode<GroupModuleModel>> listChild;
                foreach (var groupModule in listParentGroup)
                {
                    parentNode = new TreeNode<GroupModuleModel>();
                    parentNode.data = groupModule;

                    var listChilden = from r in listGroup
                                      where r.ParentId.Equals(groupModule.Id)
                                      select r;
                    listChild = new List<TreeNode<GroupModuleModel>>();
                    foreach (var item in listChilden)
                    {
                        childNode = new TreeNode<GroupModuleModel>();

                        childNode.data = item;
                        listChild.Add(childNode);
                    }

                    parentNode.children = listChild;
                    searchResult.ListResult.Add(parentNode);

                }
                searchResult.TotalItem = listGroup.Count();
            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.List");
            }

            return searchResult;
        }

        public SearchResultModel<GroupModuleModel> GetListModuleGroup(GroupModuleSearchModel modelSearch)
        {
            SearchResultModel<GroupModuleModel> searchResult = new SearchResultModel<GroupModuleModel>();
            var listModuleGroup = (from o in db.ModuleGroups.AsNoTracking()
                                   orderby o.Name
                                   select new GroupModuleModel
                                   {
                                       Id = o.Id,
                                       Code = o.Code,
                                       Description = o.Description,
                                       Name = o.Name,
                                       Note = o.Note,
                                       ParentId = o.ParentId,
                                       CreateBy = o.CreateBy,
                                       CreateDate = o.CreateDate,
                                       UpdateBy = o.UpdateBy,
                                       UpdateDate = o.UpdateDate,
                                   }).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.Id))
            {
                listModuleGroup = listModuleGroup.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
            }
            //List<GroupModuleModel> listResult = new List<GroupModuleModel>();
            //var listParent = listModuleGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            //bool isSearch = false;
            //int index = 1;

            //List<GroupModuleModel> listChild = new List<GroupModuleModel>();

            //foreach (var parent in listParent)
            //{
            //    isSearch = true;
            //    if (!string.IsNullOrEmpty(modelSearch.Name) && !parent.Name.ToLower().Contains(modelSearch.Name.ToLower()))
            //    {
            //        isSearch = false;
            //    }

            //    if (!string.IsNullOrEmpty(modelSearch.Code) && !parent.Code.ToLower().Contains(modelSearch.Code.ToLower()))
            //    {
            //        isSearch = false;
            //    }

            //    listChild = GetModuleGroupChild(parent.Id, listModuleGroup.ToList(), modelSearch, index.ToString());
            //    if (isSearch || listChild.Count > 0)
            //    {
            //        parent.Index = index.ToString();
            //        listResult.Add(parent);
            //        index++;
            //    }

            //    listResult.AddRange(listChild);
            //}

            searchResult.TotalItem = listModuleGroup.Count();
            //listResult = listResult.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            searchResult.ListResult = listModuleGroup.OrderBy(t => t.Code).ToList();

            return searchResult;
        }

        private List<GroupModuleModel> GetModuleGroupChild(string parentId,
          List<GroupModuleModel> listModuleGroup, GroupModuleSearchModel modelSearch, string index)
        {
            List<GroupModuleModel> listResult = new List<GroupModuleModel>();
            var listChild = listModuleGroup.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            int indexChild = 1;
            List<GroupModuleModel> listChildChild = new List<GroupModuleModel>();
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

                listChildChild = GetModuleGroupChild(child.Id, listModuleGroup, modelSearch, index + "." + indexChild);
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

        public void DeleteGroupModule(GroupModuleModel model)
        {
            var groupModule = db.ModuleGroups.AsNoTracking().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
            if (groupModule == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
            }

            List<GroupModuleModel> listCheck = GetModuleGroupById(model.Id);
            if (listCheck.Count > 0)
            {
                foreach (var item in listCheck)
                {
                    var moduleGroupCheck = db.Modules.AsNoTracking().Where(m => m.ModuleGroupId.Equals(item.Id)).FirstOrDefault();
                    if (moduleGroupCheck != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ModuleGroup);
                    }

                    var taskModuleGroup = db.TaskModuleGroups.AsNoTracking().Where(m => m.ModuleGroupId.Equals(item.Id)).FirstOrDefault();
                    if (taskModuleGroup != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ModuleGroup);
                    }
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in listCheck)
                    {
                        var deleteModuleGroup = db.ModuleGroups.Where(m => m.Id.Equals(item.Id)).FirstOrDefault();
                        db.ModuleGroups.Remove(deleteModuleGroup);

                        var moduleGroupProductStandard = db.ModuleGroupProductStandards.Where(a => a.ModuleGroupId.Equals(item.Id)).ToList();
                        if (moduleGroupProductStandard.Count > 0)
                        {
                            db.ModuleGroupProductStandards.RemoveRange(moduleGroupProductStandard);
                        }
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
            //luu Log lich su
            //string decription = "Xóa nhóm modul tên là: " + model.Name;
            //LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        public void AddGroupModule(GroupModuleModel groupModuleModel)
        {
            //xoá ký tự đặc biệt
            groupModuleModel.Code = Util.RemoveSpecialCharacter(groupModuleModel.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.ModuleGroups.AsNoTracking().Where(o => o.Name.Equals(groupModuleModel.Name) && string.Compare(o.ParentId, groupModuleModel.ParentId) == 0).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ModuleGroup);
                }

                if (db.ModuleGroups.AsNoTracking().Where(o => o.Code.Equals(groupModuleModel.Code) && string.Compare(o.ParentId, groupModuleModel.ParentId) == 0).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ModuleGroup);
                }

                if (string.IsNullOrEmpty(groupModuleModel.ParentId))
                {
                    groupModuleModel.ParentId = null;
                }
                try
                {

                    var dateNow = DateTime.Now;
                    ModuleGroup newGroupModule = new ModuleGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = groupModuleModel.Name,
                        Code = groupModuleModel.Code,
                        ParentId = groupModuleModel.ParentId,
                        Description = groupModuleModel.Description,
                        Note = groupModuleModel.Note,
                        CreateBy = groupModuleModel.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = groupModuleModel.CreateBy
                    };
                    db.ModuleGroups.Add(newGroupModule);

                    if (groupModuleModel.ListProductStandard != null && groupModuleModel.ListProductStandard.Count > 0)
                    {
                        foreach (var item in groupModuleModel.ListProductStandard)
                        {
                            ModuleGroupProductStandard moduleGroupProductStandard = new ModuleGroupProductStandard()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductStandardId = item.Id,
                                ModuleGroupId = newGroupModule.Id,
                            };
                            db.ModuleGroupProductStandards.Add(moduleGroupProductStandard);
                        }
                    }

                    if (groupModuleModel.ListStage != null && groupModuleModel.ListStage.Count > 0)
                    {
                        foreach (var item in groupModuleModel.ListStage)
                        {
                            ModuleGroupStage moduleGroupStage = new ModuleGroupStage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleGroupId = newGroupModule.Id,
                                StageId = item.Id,
                                Time = item.Time,
                            };
                            db.ModuleGroupStages.Add(moduleGroupStage);
                        }
                    }

                    if (groupModuleModel.ListTestCriteri != null && groupModuleModel.ListTestCriteri.Count > 0)
                    {
                        foreach (var item in groupModuleModel.ListTestCriteri)
                        {
                            ModuleGroupTestCriteria moduleGroupTestCriteria = new ModuleGroupTestCriteria()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleGroupId = newGroupModule.Id,
                                TestCriteriasId = item.Id,
                            };

                            db.ModuleGroupTestCriterias.Add(moduleGroupTestCriteria);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(groupModuleModel, ex);
                }
            }
            try
            {
                //luu Log lich su
                string decription = String.Empty;
                decription = "Thêm nhóm module có tên là: " + groupModuleModel.Name;
                LogBusiness.SaveLogEvent(db, groupModuleModel.CreateBy, decription);
            }
            catch (Exception) { }

        }

        public GroupModuleModel GetGroupModuleInfo(GroupModuleModel model)
        {
            var moduleGroupInfo = db.ModuleGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (moduleGroupInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
            }
            model.Id = moduleGroupInfo.Id;
            model.Name = moduleGroupInfo.Name;
            model.Code = moduleGroupInfo.Code;
            model.Description = moduleGroupInfo.Description;
            model.ParentId = moduleGroupInfo.ParentId;
            model.Note = moduleGroupInfo.Note;
            model.CreateBy = moduleGroupInfo.CreateBy;
            model.CreateDate = moduleGroupInfo.CreateDate;
            model.UpdateBy = moduleGroupInfo.UpdateBy;
            model.UpdateDate = moduleGroupInfo.UpdateDate;

            var productStandard = (from a in db.ModuleGroupProductStandards.AsNoTracking()
                                   where a.ModuleGroupId.Equals(moduleGroupInfo.Id)
                                   join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                   join c in db.ProductStandardGroups.AsNoTracking() on b.ProductStandardGroupId equals c.Id
                                   join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                                   join e in db.Users.AsNoTracking() on b.CreateBy equals e.Id
                                   join f in db.Employees.AsNoTracking() on e.EmployeeId equals f.Id
                                   orderby b.Code
                                   select new ProductStandardsModel
                                   {
                                       Id = b.Id,
                                       Code = b.Code,
                                       Name = b.Name,
                                       ProductStandardGroupId = b.ProductStandardGroupId,
                                       Note = b.Note,
                                       Content = b.Content,
                                       Target = b.Target,
                                       Version = b.Version,
                                       EditContent = b.EditContent,
                                       ProductStandardGroupName = c.Name,
                                       DepartmentId = b.DepartmentId,
                                       DepartmentName = d.Name,
                                       CreateByName = f.Name,
                                       CreateDate = b.CreateDate,
                                       CreateBy = b.CreateBy,
                                       UpdateBy = b.UpdateBy,
                                       UpdateDate = b.UpdateDate,
                                   }).ToList();
            model.ListProductStandard = productStandard;

            // Danh sách công đoạn

            var listStage = (from c in db.ModuleGroups.AsNoTracking()
                             join a in db.ModuleGroupStages.AsNoTracking() on c.Id equals a.ModuleGroupId
                             join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                             join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                             join e in db.SBUs.AsNoTracking() on d.SBUId equals e.Id
                             orderby b.Name
                             where a.ModuleGroupId.Equals(moduleGroupInfo.Id) && b.DepartmentId.Equals(model.DepartementId)
                             select new NTS.Model.GroupModule.GroupModuleStageModel
                             {
                                 Id = b.Id,
                                 ModuleGroupName = c.Name,
                                 Name = b.Name,
                                 Code = b.Code,
                                 DepartmentName = d.Name,
                                 SBUName = e.Name,
                                 Time = a.Time
                             }).ToList();

            model.ListStage = listStage;

            var listTestCriteri = (from a in db.ModuleGroupTestCriterias.AsNoTracking()
                                   where a.ModuleGroupId.Equals(moduleGroupInfo.Id)
                                   join b in db.TestCriterias.AsNoTracking() on a.TestCriteriasId equals b.Id
                                   join c in db.TestCriteriaGroups.AsNoTracking() on b.TestCriteriaGroupId equals c.Id
                                   orderby b.Name
                                   select new ModuleGroupTestCriteiaModel()
                                   {
                                       Id = a.TestCriteriasId,
                                       Name = b.Name,
                                       Code = b.Code,
                                       TechnicalRequirements = b.TechnicalRequirements,
                                       Note = b.Note,
                                       TestCriteriaGroupName = c.Name,
                                   }).ToList();

            model.ListTestCriteri = listTestCriteri;

            return model;
        }

        public void UpdateGroupModule(GroupModuleModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            using (var trans = db.Database.BeginTransaction())
            {
                var moduleGroupInfo = db.ModuleGroups.FirstOrDefault(u => u.Id.Equals(model.Id));

                if (db.ModuleGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ModuleGroup);
                }

                if (db.ModuleGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ModuleGroup);
                }
                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                try
                {
                    //cập nhật nhóm module
                    moduleGroupInfo.Code = model.Code;
                    moduleGroupInfo.Name = model.Name;
                    moduleGroupInfo.Description = model.Description;
                    moduleGroupInfo.Note = model.Note;
                    moduleGroupInfo.ParentId = model.ParentId;
                    moduleGroupInfo.UpdateBy = model.CreateBy;
                    moduleGroupInfo.UpdateDate = DateTime.Now;
                    moduleGroupInfo.CreateBy = model.CreateBy;

                    //cập nhật danh sách tiêu chuẩn ProductStandard
                    var listModuleProductStandard = db.ModuleGroupProductStandards.Where(a => a.ModuleGroupId.Equals(model.Id)).ToList();
                    if (listModuleProductStandard.Count > 0)
                    {
                        db.ModuleGroupProductStandards.RemoveRange(listModuleProductStandard);
                    }

                    if (model.ListProductStandard != null && model.ListProductStandard.Count > 0)
                    {
                        foreach (var item in model.ListProductStandard)
                        {
                            ModuleGroupProductStandard moduleGroupProductStandard = new ModuleGroupProductStandard()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleGroupId = model.Id,
                                ProductStandardId = item.Id,
                            };
                            db.ModuleGroupProductStandards.Add(moduleGroupProductStandard);
                        }
                    }

                    var listStage = (from x in db.ModuleGroupStages
                                     join b in db.Stages on x.StageId equals b.Id
                                     where b.DepartmentId.Equals(model.DepartementId) && x.StageId.Equals(b.Id)
                                     select x).ToList();

                    if (listStage.Count > 0)
                    {
                        db.ModuleGroupStages.RemoveRange(listStage);
                    }

                    // danh sách công đoạn
                    if (model.ListStage != null && model.ListStage.Count > 0)
                    {
                        foreach (var item in model.ListStage)
                        {


                            ModuleGroupStage moduleGroupStage = new ModuleGroupStage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleGroupId = model.Id,
                                StageId = item.Id,
                                Time = item.Time,
                            };
                            db.ModuleGroupStages.Add(moduleGroupStage);
                        }
                    }

                    // Danh sách tiêu chí kiếm tra

                    var listTestCri = db.ModuleGroupTestCriterias.Where(a => a.ModuleGroupId.Equals(model.Id)).ToList();
                    if (listTestCri.Count > 0)
                    {
                        db.ModuleGroupTestCriterias.RemoveRange(listTestCri);
                    }

                    if (model.ListTestCriteri != null && model.ListTestCriteri.Count > 0)
                    {
                        foreach (var item in model.ListTestCriteri)
                        {
                            ModuleGroupTestCriteria moduleGroupTestCriteria = new ModuleGroupTestCriteria()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleGroupId = model.Id,
                                TestCriteriasId = item.Id,
                            };
                            db.ModuleGroupTestCriterias.Add(moduleGroupTestCriteria);
                        }
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

        //Tìm kiếm nhóm module theo Id
        public List<GroupModuleModel> GetModuleGroupById(string Id)
        {
            List<GroupModuleModel> search = new List<GroupModuleModel>();
            List<GroupModuleModel> searchResult = new List<GroupModuleModel>();
            try
            {

                var listModuleGroup = (from o in db.ModuleGroups.AsNoTracking().OrderBy(r => r.Name)
                                       select new GroupModuleModel
                                       {
                                           Id = o.Id,
                                           Note = o.Note,
                                           Name = o.Name,
                                           ParentId = o.ParentId,
                                           CreateBy = o.CreateBy,
                                           CreateDate = o.CreateDate,
                                           UpdateBy = o.UpdateBy,
                                           UpdateDate = o.UpdateDate,
                                       }).AsQueryable();
                List<GroupModuleModel> listResult = new List<GroupModuleModel>();
                var listParent = listModuleGroup.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<GroupModuleModel> listChild = new List<GroupModuleModel>();

                foreach (var parent in listParent)
                {

                    listChild = GetModuleGroupChild(parent.Id, listModuleGroup.ToList());
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
                throw new NTSLogException(null, ex);
            }
            return search.ToList();
        }
        private List<GroupModuleModel> GetModuleGroupChild(string parentId, List<GroupModuleModel> listDocumentTemplateType)
        {
            List<GroupModuleModel> listResult = new List<GroupModuleModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<GroupModuleModel> listChildChild = new List<GroupModuleModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetModuleGroupChild(child.Id, listDocumentTemplateType);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        //Danh sách ProductStandards
        public object ProductStandards(ProductStandardsSearchModel modelSearch)
        {
            SearchResultModel<ProductStandardsModel> searchResult = new SearchResultModel<ProductStandardsModel>();
            try
            {
                var dataQuery = (from a in db.ProductStandards.AsNoTracking()
                                 join b in db.ProductStandardGroups.AsNoTracking() on a.ProductStandardGroupId equals b.Id
                                 join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id
                                 join d in db.Users.AsNoTracking() on a.CreateBy equals d.Id
                                 where !modelSearch.ListIdSelect.Contains(a.Id)
                                 orderby a.Code
                                 select new ProductStandardsModel
                                 {
                                     Id = a.Id,
                                     ProductStandardGroupId = a.ProductStandardGroupId,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Note = a.Note,
                                     Content = a.Content,
                                     Target = a.Target,
                                     Version = a.Version,
                                     EditContent = a.EditContent,
                                     ProductStandardGroupName = b.Name,
                                     DepartmentId = a.DepartmentId,
                                     DepartmentName = c.Name,

                                     CreateByName = d.UserName,
                                     DataType = a.DataType,
                                     CreateDate = a.CreateDate,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,

                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.ProductStandardGroupId))
                {
                    dataQuery = dataQuery.Where(u => u.ProductStandardGroupId.Equals(modelSearch.ProductStandardGroupId));
                }

                //if (!string.IsNullOrEmpty(modelSearch.Name))
                //{
                //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                //}

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }

                if (modelSearch.DataType.HasValue)
                {
                    dataQuery = dataQuery.Where(r => r.DataType == modelSearch.DataType.Value);
                }
                searchResult.TotalItem = dataQuery.Count();
                //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = dataQuery.ToList();
                int i = 1;
                foreach (var item in searchResult.ListResult)
                {
                    item.Index = 1;
                    i++;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Danh sách công đoạn
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public object Stage(StageSearchModel modelSearch)
        {
            SearchResultModel<StageModel> searchResult = new SearchResultModel<StageModel>();
            try
            {

                var dataQuery = (from a in db.Stages.AsNoTracking()
                                 join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                 join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                 where !modelSearch.ListIdSelect.Contains(a.Id) && b.Id.Equals(modelSearch.DepartmentId)
                                 orderby a.Name
                                 select new StageModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Note = a.Note,
                                     DepartmentId = b.Id,
                                     DepartmentName = b.Name,
                                     SBUId = c.Id,
                                     SBUName = c.Name,
                                     Time = a.Time,
                                     Code = a.Code,
                                     CreateDate = a.CreateDate,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
                                     StageId=a.Id
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }

                if (modelSearch.Time.HasValue)
                {
                    dataQuery = dataQuery.Where(u => u.Time <= modelSearch.Time);
                }

                searchResult.TotalItem = dataQuery.Count();
                //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = dataQuery.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }
        public void AddCustomerType(ProductStandardsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ModuleGroupProductStandard moduleGroupProductStandard = new ModuleGroupProductStandard()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductStandardId = model.Id,
                        ModuleGroupId = model.ModuleGroupId,
                    };

                    db.ModuleGroupProductStandards.Add(moduleGroupProductStandard);
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

        public void DeleteProductStandards(ModuleGroupProductStandardModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var moduleProductStandard = db.ModuleProductStandards.AsNoTracking().Where(m => m.ProductStandardId.Equals(model.Id) && m.ModuleGroupId.Equals(model.ModuleGroupId)).FirstOrDefault();
                if (moduleProductStandard != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandard);
                }

                try
                {
                    var moduleGroupProductStandard = db.ModuleGroupProductStandards.Where(u => u.ProductStandardId.Equals(model.Id) && u.ModuleGroupId.Equals(model.ModuleGroupId)).FirstOrDefault();
                    if (moduleGroupProductStandard == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandard);
                    }

                    db.ModuleGroupProductStandards.Remove(moduleGroupProductStandard);
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

        //Lấy module group còn lại khi update
        public List<GroupModuleModel> GetModuleGroupExcepted(GroupModuleSearchModel modelSearch)
        {
            List<GroupModuleModel> listExpected = new List<GroupModuleModel>();

            List<GroupModuleModel> listAll = new List<GroupModuleModel>();
            GroupModuleSearchModel modelAll = new GroupModuleSearchModel();
            listAll = GetListModuleGroup(modelAll).ListResult;

            List<GroupModuleModel> listById = new List<GroupModuleModel>();
            listById = GetModuleGroupById(modelSearch.Id);

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
    }
}
