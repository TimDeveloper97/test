using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.PracticeGroup;
using NTS.Model.Repositories;
using NTS.Model.Specialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTK.Business.PracticeGroups
{
    public class PracticeGroupBusiness
    {
        QLTKEntities db = new QLTKEntities();
        // public SearchResultObject<TreeNode<PracticeGroupModel>> SearchPracticeGroup(PracticeGroupSearchModel modelSearch)
        public SearchResultModel<PracticeGroupModel> SearchPracticeGroup(PracticeGroupSearchModel modelSearch)
        {
            SearchResultModel<PracticeGroupModel> searchResult = new SearchResultModel<PracticeGroupModel>();
            try
            {
                var listPracticeGroup = (from o in db.PracticeGroups.AsNoTracking()
                                         orderby o.Code
                                         select new PracticeGroupModel
                                         {
                                             Id = o.Id,
                                             Code = o.Code,
                                             Description = o.Description,
                                             Name = o.Name,
                                             ParentId = o.ParentId,
                                             CreateBy = o.CreateBy,
                                             CreateDate = o.CreateDate,
                                             UpdateBy = o.UpdateBy,
                                             UpdateDate = o.UpdateDate,
                                         }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Id))
                {
                    listPracticeGroup = listPracticeGroup.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
                }

                List<PracticeGroupModel> listResult = new List<PracticeGroupModel>();
                var listParent = listPracticeGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<PracticeGroupModel> listChild = new List<PracticeGroupModel>();
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

                    listChild = GetPracticeGroupChild(parent.Id, listPracticeGroup.ToList(), modelSearch, index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }
                searchResult.TotalItem = listResult.Count();
                //listResult = listResult.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }

            return searchResult;
        }

        public SearchResultModel<PracticeGroupModel> GetListPracticeGroup(PracticeGroupSearchModel modelSearch)
        {
            SearchResultModel<PracticeGroupModel> searchResult = new SearchResultModel<PracticeGroupModel>();
            try
            {
                var listModuleGroup = (from o in db.ProductGroups.AsNoTracking()
                                       orderby o.Name
                                       select new PracticeGroupModel
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Description = o.Description,
                                           Name = o.Name,
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

                List<PracticeGroupModel> listResult = new List<PracticeGroupModel>();
                var listParent = listModuleGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<PracticeGroupModel> listChild = new List<PracticeGroupModel>();
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

                    listChild = GetPracticeGroupChild(parent.Id, listModuleGroup.ToList(), modelSearch, index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }
                searchResult.TotalItem = listResult.Count();
                //listResult = listResult.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return searchResult;
        }

        public List<PracticeGroupModel> GetPracticeGroupById(string Id)
        {
            List<PracticeGroupModel> search = new List<PracticeGroupModel>();
            List<PracticeGroupModel> searchResult = new List<PracticeGroupModel>();
            try
            {
                var listProductGroup = (from o in db.ProductGroups.AsNoTracking().OrderBy(r => r.Name)
                                        select new PracticeGroupModel
                                        {
                                            Id = o.Id,
                                            Name = o.Name,
                                            ParentId = o.ParentId,
                                            CreateBy = o.CreateBy,
                                            CreateDate = o.CreateDate,
                                            UpdateBy = o.UpdateBy,
                                            UpdateDate = o.UpdateDate,
                                        }).AsQueryable();
                List<PracticeGroupModel> listResult = new List<PracticeGroupModel>();
                var listParent = listProductGroup.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<PracticeGroupModel> listChild = new List<PracticeGroupModel>();
                foreach (var parent in listParent)
                {
                    listChild = GetPracticeGroupChild(parent.Id, listProductGroup.ToList());
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

        private List<PracticeGroupModel> GetPracticeGroupChild(string parentId,
          List<PracticeGroupModel> listProductGroup, PracticeGroupSearchModel modelSearch, string index)
        {
            List<PracticeGroupModel> listResult = new List<PracticeGroupModel>();
            var listChild = listProductGroup.Where(r => parentId.Equals(r.ParentId)).ToList();
            bool isSearch = false;
            int indexChild = 1;
            List<PracticeGroupModel> listChildChild = new List<PracticeGroupModel>();

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

                listChildChild = GetPracticeGroupChild(child.Id, listProductGroup, modelSearch, index + "." + indexChild);
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

        private List<PracticeGroupModel> GetPracticeGroupChild(string parentId, List<PracticeGroupModel> listDocumentTemplateType)
        {
            List<PracticeGroupModel> listResult = new List<PracticeGroupModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<PracticeGroupModel> listChildChild = new List<PracticeGroupModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetPracticeGroupChild(child.Id, listDocumentTemplateType);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public void AddPracticeGroup(PracticeGroupModel practiceGroupModel)
        {
            practiceGroupModel.Code = Util.RemoveSpecialCharacter(practiceGroupModel.Code);
            CheckInputPracticeGroup(practiceGroupModel);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    //var newpracticeGroup = new PracticeGroup();
                    var practiceGrroup = new NTS.Model.Repositories.PracticeGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = practiceGroupModel.Name,
                        Code = practiceGroupModel.Code,
                        ParentId = !string.IsNullOrEmpty(practiceGroupModel.ParentId) ? practiceGroupModel.ParentId : null,
                        Description = practiceGroupModel.Description,
                        CreateBy = practiceGroupModel.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = practiceGroupModel.CreateBy
                    };
                    db.PracticeGroups.Add(practiceGrroup);

                    SpecializeInPracticeGroup specializeInPracticeGroup;
                    foreach (var item in practiceGroupModel.ListSpecialize)
                    {
                        specializeInPracticeGroup = new SpecializeInPracticeGroup();
                        specializeInPracticeGroup.Id = Guid.NewGuid().ToString();
                        specializeInPracticeGroup.SpecializeId = item.Id;
                        specializeInPracticeGroup.PracticeGroupId = practiceGrroup.Id;
                        db.SpecializeInPracticeGroups.Add(specializeInPracticeGroup);
                    }

                    db.SaveChanges();
                    //luu Log lich su
                    string decription = String.Empty;
                    decription = "Thêm nhóm bài tập có tên là: " + practiceGroupModel.Name;
                    LogBusiness.SaveLogEvent(db, practiceGroupModel.CreateBy, decription);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
        }

        public void UpdatePracticeGroup(PracticeGroupModel model)
        {
            string NameOld = string.Empty;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckInputUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var practiceGroupEdit = db.PracticeGroups
                        .AsQueryable()
                        .Where(o => model.Id.Equals(o.Id))
                        .FirstOrDefault();
                    NameOld = practiceGroupEdit.Name;
                    practiceGroupEdit.Code = model.Code;
                    practiceGroupEdit.Name = model.Name;
                    practiceGroupEdit.Description = model.Description;
                    practiceGroupEdit.ParentId = !string.IsNullOrEmpty(model.ParentId) ? model.ParentId : null;
                    practiceGroupEdit.UpdateBy = model.CreateBy;
                    practiceGroupEdit.UpdateDate = DateTime.Now;
                    practiceGroupEdit.CreateBy = model.CreateBy;

                    var listOldSpecialize = db.SpecializeInPracticeGroups.Where(a => a.PracticeGroupId.Equals(model.Id)).ToList();
                    db.SpecializeInPracticeGroups.RemoveRange(listOldSpecialize);

                    SpecializeInPracticeGroup specializeInPracticeGroup;
                    foreach (var item in model.ListSpecialize)
                    {
                        specializeInPracticeGroup = new SpecializeInPracticeGroup();
                        specializeInPracticeGroup.Id = Guid.NewGuid().ToString();
                        specializeInPracticeGroup.SpecializeId = item.Id;
                        specializeInPracticeGroup.PracticeGroupId = model.Id;
                        db.SpecializeInPracticeGroups.Add(specializeInPracticeGroup);
                    }


                    db.SaveChanges();

                    string decription = string.Empty;
                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        decription = "Cập nhật nhóm bài thực hành tên là: " + NameOld;
                    }
                    else
                    {
                        decription = "Cập nhật nhóm bài thực hành có tên ban đầu là:  " +
                            NameOld +
                            " thành " +
                            model.Name;
                        ;
                    }
                    LogBusiness.SaveLogEvent(db, model.CreateBy, decription);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
        }

        public void DeletePracticeGroup(PracticeGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var practiceGroup = db.PracticeGroups.Where(o => model.Id.Equals(o.Id)).FirstOrDefault();

                    if (practiceGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.PracticeGroup);
                    }

                    var practice = db.Practices.Where(m => practiceGroup.Id.Equals(m.PracticeGroupId)).ToList().Count();
                    var practiceGroupChild = db.PracticeGroups.Where(o => practiceGroup.Id.Equals(o.ParentId)).ToList();
                    var count = practiceGroupChild.Count();
                    
                    if (count > 0 || practice > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.PracticeGroup);
                    }

                    var listSpecialize = db.SpecializeInPracticeGroups.Where(a => a.PracticeGroupId.Equals(model.Id)).ToList();
                    db.SpecializeInPracticeGroups.RemoveRange(listSpecialize);

                    db.PracticeGroups.RemoveRange(practiceGroupChild);
                    db.PracticeGroups.Remove(practiceGroup);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. ");
                }
            }

            //luu Log lich su
            string decription = "Xóa nhóm bài thực hành tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        private void CheckInputUpdate(PracticeGroupModel model)
        {
            if (db.PracticeGroups
                    .AsNoTracking()
                    .Where(o => (!model.Id.Equals(o.Id)) && model.Name.Equals(o.Name))
                    .Count() >
                0)
            {
                throw new Exception("Tên nhóm module này đã tồn tại, vui lòng nhập lại!");
            }

            if (db.PracticeGroups
                    .AsNoTracking()
                    .Where(o => (!model.Id.Equals(o.Id)) && model.Code.Equals(o.Code))
                    .Count() >
                0)
            {
                throw new Exception("Mã code này đã tồn tại, vui lòng nhập lại!");
            }

            var practiceGroupEdit = db.PracticeGroups.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
            if ((practiceGroupEdit.ParentId == null) && (model.ParentId != null))
            {
                throw new Exception("Không được quyền sửa nhóm cha thành nhóm con, vui lòng nhập lại!");
            }
        }

        public PracticeGroupModel GetPracticeGroupInfo(PracticeGroupModel model)
        {            
            var practiceGroup = db.PracticeGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (practiceGroup == null)
            {
                throw new Exception("Bài thực hành này không tồn tại");
            }

            try
            {
                model.Id = practiceGroup.Id;
                model.Name = practiceGroup.Name;
                model.Code = practiceGroup.Code;
                model.Description = practiceGroup.Description;
                model.ParentId = practiceGroup.ParentId;
                model.CreateBy = practiceGroup.CreateBy;
                model.CreateDate = practiceGroup.CreateDate;
                model.UpdateBy = practiceGroup.UpdateBy;
                model.UpdateDate = practiceGroup.UpdateDate;
                model.ListSpecialize = (from a in db.SpecializeInPracticeGroups.AsNoTracking()
                                        join b in db.Specializes.AsNoTracking() on a.SpecializeId equals b.Id
                                        where a.PracticeGroupId.Equals(model.Id)
                                        select new SpecializeResultModel()
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Code = b.Code,
                                            Description = b.Description,
                                        }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<PracticeGroupModel> GetPracticeGroupExcepted(PracticeGroupSearchModel modelSearch)
        {
            List<PracticeGroupModel> listExpected = new List<PracticeGroupModel>();
            List<PracticeGroupModel> listAll = new List<PracticeGroupModel>();
            PracticeGroupSearchModel modelAll = new PracticeGroupSearchModel();
            listAll = GetListPracticeGroup(modelAll).ListResult;

            List<PracticeGroupModel> listById = new List<PracticeGroupModel>();
            listById = GetPracticeGroupById(modelSearch.Id);

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

        private void CheckInputPracticeGroup(PracticeGroupModel practiceGroupModel)
        {
            int practiceGroupName = db.ModuleGroups
                .AsNoTracking()
                .Where(o => practiceGroupModel.Name.Equals(o.Name))
                .Count();

            int practiceGroupCode = db.ModuleGroups
                .AsNoTracking()
                .Where(o => practiceGroupModel.Code.Equals(o.Code))
                .Count();

            int practiceGroupId = db.ModuleGroups.AsNoTracking().Where(o => practiceGroupModel.Id.Equals(o.Id)).Count();

            if ((practiceGroupCode > 0) || (practiceGroupName > 0) || (practiceGroupId > 0))
            {
                throw new Exception("Trùng tên hoặc code nhóm bài thực hành");
            }
        }
    }
}
