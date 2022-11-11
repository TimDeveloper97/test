using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.DesignStructure;
using NTS.Model.Employees;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.DesignStructures
{
    public class DesignStructureBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<DesignStructureResultModel> SearchDesignStructure(DesignStructureSearchModel modelSearch)
        {
            SearchResultModel<DesignStructureResultModel> searchResult = new SearchResultModel<DesignStructureResultModel>();

            var dataQuery = (from a in db.DesignStructures.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             where a.Type == modelSearch.Type && a.ObjectType == modelSearch.ObjectType
                             select new DesignStructureResultModel
                             {
                                 Name = a.Name,
                                 Id = a.Id,
                                 ParentId = a.ParentId,
                                 Type = a.Type,
                                 IsOpen = a.IsOpen,
                                 DepartmentId = a.DepartmentId,
                                 SBUId = b.SBUId,                               
                             }).OrderBy(o => o.Name).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.SBUId.Equals(u.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.DepartmentId.Equals(u.DepartmentId));
            }


            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            foreach (var item in searchResult.ListResult)
            {
                item.ListFile = db.DesignStructureFiles.AsNoTracking().Where(t => t.DesignStructureId.Equals(item.Id)).Select(m => new DesignStructureFileModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    DesignStructureId = m.DesignStructureId,
                    Description = m.Description,
                    Exist = m.Exist,
                    IsTemplate = m.IsTemplate,
                    IsInsertData = m.IsInsertData,
                    Path = m.Path,
                }).ToList();
            }

            return searchResult;
        }

        public void CreateDesignStructure(DesignStructureModel model)
        {
            if (db.DesignStructures.AsNoTracking().Where(o => o.Type == model.Type && o.ObjectType == model.ObjectType && o.DepartmentId.Equals(model.DepartmentId) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DesignStructure);
            }

            var parent = db.DesignStructures.Where(a => a.Id.Equals(model.ParentId)).FirstOrDefault();
            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.ParentId = null;
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    DesignStructure designStructure = new DesignStructure
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Type = model.Type,
                        ObjectType = model.ObjectType,
                        ParentId = model.ParentId,
                        Description = model.Description,
                        Contain = model.Contain,
                        Extension = model.Extension,
                        ParentPath = parent != null ? parent.Path : "",
                        Path = parent != null ? (parent.Path + "/" + model.Name) : model.Name,
                        IsOpen = model.IsOpen,
                        DepartmentId = model.DepartmentId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.DesignStructures.Add(designStructure);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, designStructure.Name, designStructure.Id, Constants.LOG_DesignStructure);
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

        public object GetInfoDesignStructure(DesignStructureModel model)
        {
            var resultInfo = db.DesignStructures.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new DesignStructureModel
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                ParentId = p.ParentId,
                Type = p.Type,
                Contain = p.Contain,
                Extension = p.Extension,
                IsOpen = p.IsOpen,
                DepartmentId = p.DepartmentId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DesignStructure);
            }

            return resultInfo;
        }

        public void UpdateDesignStructure(DesignStructureModel model)
        {
            if (db.DesignStructures.AsNoTracking().Where(o => o.Type == model.Type && o.ObjectType == model.ObjectType && !o.Id.Equals(model.Id) && o.DepartmentId.Equals(model.DepartmentId) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DesignStructure);
            }

            var parent = db.DesignStructures.Where(a => a.Id.Equals(model.ParentId)).FirstOrDefault();
            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.ParentId = null;
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newDesignStructure = db.DesignStructures.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<DesignStructureHistoryModel>(newDesignStructure);

                    if (!string.IsNullOrEmpty(newDesignStructure.DepartmentId) && !newDesignStructure.DepartmentId.Equals(model.DepartmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.DesignStructure);
                    }
                    newDesignStructure.Name = model.Name;
                    newDesignStructure.Description = model.Description;
                    newDesignStructure.Contain = model.Contain;
                    newDesignStructure.Extension = model.Extension;
                    newDesignStructure.ParentId = model.ParentId;
                    newDesignStructure.ParentPath = parent != null ? parent.Path : "";
                    newDesignStructure.Path = parent != null ? (parent.Path + "/" + model.Name) : model.Name;
                    newDesignStructure.UpdateBy = model.UpdateBy;
                    newDesignStructure.UpdateDate = DateTime.Now;
                    newDesignStructure.IsOpen = model.IsOpen;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DesignStructureHistoryModel>(newDesignStructure);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_EmployeeTraining, newDesignStructure.Id, newDesignStructure.Name, jsonBefor, jsonApter);

                    UpdateDesignStructureChild(newDesignStructure.Id, newDesignStructure.Path);

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

        private void UpdateDesignStructureChild(string parentId, string parentPath)
        {
            var childs = db.DesignStructures.Where(r => parentId.Equals(r.ParentId));

            foreach (var item in childs)
            {
                item.ParentPath = string.IsNullOrEmpty(parentPath) ? string.Empty : parentPath;
                item.Path = string.IsNullOrEmpty(parentPath) ? item.Name : parentPath + "/" + item.Name;

                UpdateDesignStructureChild(item.Id, item.Path);
            }
        }

        public void DeleteStructure(DesignStructureModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var subStructure = db.DesignStructures.AsNoTracking().Where(m => m.ParentId.Equals(model.Id)).FirstOrDefault();

                if (subStructure != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.DesignStructure);
                }

                try
                {
                    var designStructure = db.DesignStructures.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (designStructure == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DesignStructure);
                    }
                    if (!string.IsNullOrEmpty(designStructure.DepartmentId) && !designStructure.DepartmentId.Equals(model.DepartmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.DesignStructure);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DesignStructureHistoryModel>(designStructure);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_DesignStructure, designStructure.Id, designStructure.Name, jsonBefor);

                    db.DesignStructures.Remove(designStructure);
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
        /// DesignStructureFile
        /// </summary>
        /// <param name="model"></param>
        public void CreateDesignStructureFile(DesignStructureFileModel model)
        {

            if (db.DesignStructureFiles.AsNoTracking().Where(o => o.Name.Equals(model.Name) && o.DesignStructureId.Equals(model.DesignStructureId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DesignStructureFile);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    DesignStructureFile designStructureFile = new DesignStructureFile
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        DesignStructureId = model.DesignStructureId,
                        Exist = model.Exist,
                        Description = model.Description,
                        Path = model.Path,
                        IsInsertData = model.IsInsertData,
                        IsTemplate = model.IsTemplate,
                    };

                    db.DesignStructureFiles.Add(designStructureFile);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, designStructureFile.Name, designStructureFile.Id, Constants.LOG_DesignStructure);
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

        public void DeleteStructureFile(DesignStructureFileModel model, string departmentId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var designStructureFile = db.DesignStructureFiles.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (designStructureFile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DesignStructureFile);
                    }

                    var designStructure = db.DesignStructures.FirstOrDefault(i => i.Id.Equals(designStructureFile.DesignStructureId));
                    if (designStructure != null && !designStructure.DepartmentId.Equals(departmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.DesignStructure);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DesignStructureFileHistoryModel>(designStructureFile);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_DesignStructure, designStructureFile.Id, designStructureFile.Name, jsonBefor);

                    db.DesignStructureFiles.Remove(designStructureFile);
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

        public void UpdateDesignStructureFile(DesignStructureFileModel model, string departmentId)
        {

            if (db.DesignStructureFiles.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name) && o.DesignStructureId.Equals(model.DesignStructureId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.DesignStructureFile);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newDesignStructureFile = db.DesignStructureFiles.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<DesignStructureFileHistoryModel>(newDesignStructureFile);

                    var designStructure = db.DesignStructures.FirstOrDefault(i => i.Id.Equals(newDesignStructureFile.DesignStructureId));
                    if (designStructure != null && !designStructure.DepartmentId.Equals(departmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.DesignStructure);
                    }
                    newDesignStructureFile.Name = model.Name;
                    newDesignStructureFile.Description = model.Description;
                    newDesignStructureFile.Exist = model.Exist;
                    newDesignStructureFile.Path = model.Path;
                    newDesignStructureFile.IsTemplate = model.IsTemplate;
                    newDesignStructureFile.IsInsertData = model.IsInsertData;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DesignStructureFileHistoryModel>(newDesignStructureFile);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_EmployeeTraining, newDesignStructureFile.Id, newDesignStructureFile.Name, jsonBefor, jsonApter);



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

        public object GetInfoDesignStructureFile(DesignStructureFileModel model)
        {
            var resultInfo = db.DesignStructureFiles.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DesignStructureFileModel
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                DesignStructureId = p.DesignStructureId,
                Path = p.Path,
                IsInsertData = p.IsInsertData,
                IsTemplate = p.IsTemplate,
                Exist = p.Exist
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DesignStructureFile);
            }

            return resultInfo;
        }

        /// <summary>
        /// Lấy thông tin để tạo cấu trúc thư mục
        /// </summary>
        /// <param name="designStructureCreateModel"></param>
        /// <param name="userId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public object GetInfoDesignStructureCreate(DesignStructureCreateModel designStructureCreateModel, string userId, string departmentId)
        {
            DesignStructureCreateModel returnInfo = new DesignStructureCreateModel();
            returnInfo.ListDesignStructure = (from a in db.DesignStructures.AsNoTracking()
                                              where a.Type == designStructureCreateModel.Type && a.DepartmentId.Equals(departmentId) && a.ObjectType == designStructureCreateModel.ObjectType
                                              select new DesignStructureModel
                                              {
                                                  Name = a.Name,
                                                  Id = a.Id,
                                                  ParentId = a.ParentId,
                                                  Type = a.Type,
                                                  ObjectType = a.ObjectType,
                                                  Path = a.Path,
                                                  ParentPath = a.ParentPath,
                                                  IsOpen = a.IsOpen,
                                                
                                              }).ToList();

            foreach (var item in returnInfo.ListDesignStructure)
            {
                item.ListFile = db.DesignStructureFiles.AsNoTracking().Where(t => t.DesignStructureId.Equals(item.Id)).Select(m => new DesignStructureFileModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    DesignStructureId = m.DesignStructureId,
                    Description = m.Description,
                    Exist = m.Exist,
                    IsTemplate = m.IsTemplate,
                    IsInsertData = m.IsInsertData,
                    Path = m.Path,
                }).ToList();
            }


            switch (designStructureCreateModel.ObjectType)
            {
                case Constants.Definition_ObjectType_Module:
                    {
                        var module = db.Modules.AsNoTracking().FirstOrDefault(a => a.Code.Equals(designStructureCreateModel.ObjectCode));
                        if (module == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                        }

                        returnInfo.ObjectName = module.Name;

                        var group = db.ModuleGroups.AsNoTracking().Where(a => a.Id.Equals(module.ModuleGroupId)).FirstOrDefault();
                        if (group != null)
                        {
                            returnInfo.ObjectGroupCode = group.Code;
                            group = db.ModuleGroups.AsNoTracking().Where(a => a.Id.Equals(group.ParentId)).FirstOrDefault();
                            if (group != null)
                            {
                                returnInfo.ParentGroupCode = group.Code;
                            }
                        }

                        break;
                    }
                case Constants.Definition_ObjectType_Product:
                    {
                        var product = db.Products.AsNoTracking().FirstOrDefault(a => a.Code.Equals(designStructureCreateModel.ObjectCode));
                        if (product == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                        }

                        returnInfo.ObjectName = product.Name;

                        var group = db.ProductGroups.AsNoTracking().Where(a => a.Id.Equals(product.ProductGroupId)).FirstOrDefault();
                        if (group != null)
                        {
                            returnInfo.ObjectGroupCode = group.Code;
                            group = db.ProductGroups.AsNoTracking().Where(a => a.Id.Equals(group.ParentId)).FirstOrDefault();
                            if (group != null)
                            {
                                returnInfo.ParentGroupCode = group.Code;
                            }
                        }
                        break;
                    }
                case Constants.Definition_ObjectType_ClassRoom:
                    {
                        var clasRoom = db.ClassRooms.AsNoTracking().FirstOrDefault(a => a.Code.Equals(designStructureCreateModel.ObjectCode));
                        if (clasRoom == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ClassRoom);
                        }

                        returnInfo.ObjectName = clasRoom.Name;

                        var group = db.RoomTypes.AsNoTracking().Where(a => a.Id.Equals(clasRoom.RoomTypeId)).FirstOrDefault();
                        if (group != null)
                        {
                            returnInfo.ObjectGroupCode = group.Code;
                            group = db.RoomTypes.AsNoTracking().Where(a => a.Id.Equals(group.Id)).FirstOrDefault();
                            if (group != null)
                            {
                                returnInfo.ParentGroupCode = group.Code;
                            }
                        }

                        break;
                    }
                case Constants.Definition_ObjectType_Solution:
                    {
                        var solution = db.Solutions.AsNoTracking().FirstOrDefault(a => a.Code.Equals(designStructureCreateModel.ObjectCode));
                        if (solution == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                        }

                        returnInfo.ObjectName = solution.Name;

                        var group = db.SolutionGroups.AsNoTracking().Where(a => a.Id.Equals(solution.SolutionGroupId)).FirstOrDefault();
                        if (group != null)
                        {
                            returnInfo.ObjectGroupCode = group.Code;
                            group = db.SolutionGroups.AsNoTracking().Where(a => a.Id.Equals(group.ParentId)).FirstOrDefault();
                            if (group != null)
                            {
                                returnInfo.ParentGroupCode = group.Code;
                            }
                        }

                        break;
                    }
            }

            returnInfo.ObjectCode = designStructureCreateModel.ObjectCode;
            returnInfo.ObjectType = designStructureCreateModel.ObjectType;
            returnInfo.Type = designStructureCreateModel.Type;

            var employeeInfo = (from a in db.Employees.AsNoTracking()
                                join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId
                                where b.Id.Equals(userId)
                                select new EmployeeModel
                                {
                                    Name = a.Name,
                                    Code = a.Code,
                                    UserName = b.UserName
                                }).FirstOrDefault();

            if (employeeInfo != null)
            {
                returnInfo.CreateBy = employeeInfo.UserName + " - " + employeeInfo.Code;
            }

            return returnInfo;
        }

        public EmployeeModel GetEmployeeInfoByUserId(String userId)
        {
            var employeeInfo = (from a in db.Employees.AsNoTracking()
                                join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId
                                where b.Id.Equals(userId)
                                select new EmployeeModel
                                {
                                    Name = a.Name,
                                    Code = a.Code
                                }).FirstOrDefault();

            if (employeeInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
            }

            return employeeInfo;
        }

        /// <summary>
        /// Lấy danh sách thư mục cha
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public List<ComboboxMultilevelResult> GetFolderParent(DesignStructureSearchParentModel searchModel)
        {
            var designStructures = (from a in db.DesignStructures.AsNoTracking()
                                    where a.Type == searchModel.Type && a.ObjectType == searchModel.ObjectType
                                    && !a.Id.Equals(searchModel.DesignStructureId) && a.DepartmentId.Equals(searchModel.DepartmentId)
                                    select new ComboboxMultilevelResult
                                    {
                                        Name = a.Name,
                                        Id = a.Id,
                                        ParentId = a.ParentId
                                    }).ToList();

            var childs = GetFolderParentChild(designStructures, searchModel.DesignStructureId);

            designStructures = (from d in designStructures
                                join c in childs on d.Id equals c.Id into cd
                                from cdn in cd.DefaultIfEmpty()
                                where cdn == null
                                select d).ToList();

            return designStructures;
        }

        /// <summary>
        /// Lấy danh sách thư mục con
        /// </summary>
        /// <param name="designStructures"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<ComboboxMultilevelResult> GetFolderParentChild(List<ComboboxMultilevelResult> designStructures, string parentId)
        {
            List<ComboboxMultilevelResult> designStructureChilds = new List<ComboboxMultilevelResult>();
            var childs = designStructures.Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(parentId)).ToList();

            designStructureChilds.AddRange(childs);

            foreach (var item in childs)
            {
                designStructureChilds.AddRange(GetFolderParentChild(designStructures, item.Id));
            }

            return designStructureChilds;
        }
    }
}
