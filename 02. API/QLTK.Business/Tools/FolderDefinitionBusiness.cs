using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.FolderDefinition;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.FolderDefinitions
{
    public class FolderDefinitionBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public List<FolderDefinitionModel> GetListFolderDefinition(FolderDefinitionModel model)
        {
            List<FolderDefinitionModel> list = new List<FolderDefinitionModel>();
            var data = db.FolderDefinitions.AsNoTracking().Where(t => t.TypeDefinitionId.Equals(model.TypeDefinitionId) && t.ObjectType == model.ObjectType && t.DepartmentId.Equals(model.DepartmentId)).Select(m => new FolderDefinitionModel
            {
                FolderDefinitionId = m.FolderDefinitionId,
                TypeDefinitionId = m.TypeDefinitionId,
                FolderDefinitionManageId = m.FolderDefinitionManageId,
                FolderDefinitionFirst = m.FolderDefinitionFirst,
                FolderDefinitionBetween = m.FolderDefinitionBetween,
                FolderDefinitionLast = m.FolderDefinitionLast,
                FolderDefinitionBetweenIndex = m.FolderDefinitionBetweenIndex,
                StatusCheckFile = m.StatusCheckFile,
                FolderType = m.FolderType,
                StatusCheckFolder = m.StatusCheckFolder,
                ExtensionFile = m.ExtensionFile,
                ObjectType = m.ObjectType,
                DepartmentId = m.DepartmentId,
            }).ToList();

            string objectTypeName = GetObjectTypeName(model.ObjectType);

            foreach (var item in data)
            {
                item.Name = item.FolderDefinitionFirst;
                if (item.FolderDefinitionBetween == Constants.FolderDefinitionBetween_Group)
                {
                    item.Name += "MaNhom" + objectTypeName;
                }
                else if (item.FolderDefinitionBetween == Constants.FolderDefinitionBetween_Object)
                {
                    item.Name += "Ma" + objectTypeName;
                }
                else if (item.FolderDefinitionBetween == Constants.FolderDefinitionBetween_GroupParent)
                {
                    item.Name += "MaNhomCha" + objectTypeName;
                }
                else if (item.FolderDefinitionBetween == Constants.FolderDefinitionBetween_Module)
                {
                    item.Name += "MaModuleNguon";
                }

                if (item.FolderDefinitionBetweenIndex == 1)
                {
                    item.Name += "(01-99)";
                }
                else if (item.FolderDefinitionBetweenIndex == 2)
                {
                    item.Name += "(a-z)";
                }

                item.Name += item.FolderDefinitionLast;
            }
            return data.OrderBy(o => o.Name).ToList();
        }

        /// <summary>
        /// Lấy tên đối tượng theo loại
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private string GetObjectTypeName(int objectType)
        {
            switch (objectType)
            {
                case Constants.Definition_ObjectType_ClassRoom:
                    {
                        return "PhongHoc";
                    }
                case Constants.Definition_ObjectType_Module:
                    {
                        return "Module";
                    }
                case Constants.Definition_ObjectType_Product:
                    {
                        return "ThietBi";
                    }
                case Constants.Definition_ObjectType_Solution:
                    {
                        return "GiaiPhap";
                    }
                default:
                    {
                        return "Module";
                    }
            }
        }

        public void AddFolderDefinition(FolderDefinitionModel model, string departmentId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    FolderDefinition folderDefinition = new FolderDefinition()
                    {
                        FolderDefinitionId = Guid.NewGuid().ToString(),
                        TypeDefinitionId = model.TypeDefinitionId,
                        FolderDefinitionManageId = model.FolderDefinitionManageId,
                        FolderDefinitionFirst = model.FolderDefinitionFirst.NTSTrim(),
                        FolderDefinitionBetween = model.FolderDefinitionBetween,
                        FolderDefinitionLast = model.FolderDefinitionLast.NTSTrim(),
                        FolderDefinitionBetweenIndex = model.FolderDefinitionBetweenIndex,
                        StatusCheckFile = model.StatusCheckFile,
                        FolderType = model.FolderType,
                        StatusCheckFolder = model.StatusCheckFolder,
                        ExtensionFile = model.ExtensionFile,
                        ObjectType = model.ObjectType,
                        DepartmentId = departmentId
                    };

                    if (model.CheckExtensionFile == false)
                    {
                        folderDefinition.ExtensionFile = null;
                    }
                    else
                    {
                        folderDefinition.ExtensionFile = "." + model.ExtensionFile;
                    }
                    db.FolderDefinitions.Add(folderDefinition);
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

        public void UpdateFolderDefinition(FolderDefinitionModel model, string departmentId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var folderDefinition = db.FolderDefinitions.Where(r => r.FolderDefinitionId.Equals(model.FolderDefinitionId)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<FolderDefinitionHistoryModel>(folderDefinition);

                    if (!departmentId.Equals(folderDefinition.DepartmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.FolderDefinition);
                    }

                    folderDefinition.TypeDefinitionId = model.TypeDefinitionId;
                    folderDefinition.FolderDefinitionManageId = model.FolderDefinitionManageId;
                    folderDefinition.FolderDefinitionFirst = model.FolderDefinitionFirst.NTSTrim();
                    folderDefinition.FolderDefinitionBetween = model.FolderDefinitionBetween;
                    folderDefinition.FolderDefinitionLast = model.FolderDefinitionLast.NTSTrim();
                    folderDefinition.FolderDefinitionBetweenIndex = model.FolderDefinitionBetweenIndex;
                    folderDefinition.StatusCheckFile = model.StatusCheckFile;
                    folderDefinition.FolderType = model.FolderType;
                    folderDefinition.StatusCheckFolder = model.StatusCheckFolder;
                    folderDefinition.ExtensionFile = model.ExtensionFile;
                    folderDefinition.ObjectType = model.ObjectType;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<FolderDefinitionHistoryModel>(folderDefinition);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_FolderDefinition, folderDefinition.FolderDefinitionId, folderDefinition.FolderDefinitionFirst, jsonBefor, jsonApter);

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

        public void DeleteFolderDefinition(FolderDefinitionModel model, string departmentId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var folderDefinitions = db.FolderDefinitions.AsNoTracking().Where(m => m.FolderDefinitionManageId.Equals(model.FolderDefinitionId)).FirstOrDefault();
                if (folderDefinitions != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FolderDefinition);
                }

                var fileDefinition = db.FileDefinitions.AsNoTracking().Where(m => m.FolderDefinitionId.Equals(model.FolderDefinitionId)).FirstOrDefault();
                if (fileDefinition != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FolderDefinition);
                }

                try
                {
                    var folder = db.FolderDefinitions.FirstOrDefault(u => u.FolderDefinitionId.Equals(model.FolderDefinitionId));
                    if (folder == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FolderDefinition);
                    }

                    if (!departmentId.Equals(folder.DepartmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.FolderDefinition);
                    }

                    db.FolderDefinitions.Remove(folder);
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

        public FolderDefinitionModel GetFolderDefinitionInfo(FolderDefinitionModel model)
        {
            var resultInfo = db.FolderDefinitions.AsNoTracking().Where(u => model.FolderDefinitionId.Equals(u.FolderDefinitionId)).Select(m => new FolderDefinitionModel
            {
                FolderDefinitionId = m.FolderDefinitionId,
                TypeDefinitionId = m.TypeDefinitionId,
                FolderDefinitionManageId = m.FolderDefinitionManageId,
                FolderDefinitionFirst = m.FolderDefinitionFirst,
                FolderDefinitionBetween = m.FolderDefinitionBetween,
                FolderDefinitionLast = m.FolderDefinitionLast,
                FolderDefinitionBetweenIndex = m.FolderDefinitionBetweenIndex,
                StatusCheckFile = m.StatusCheckFile,
                FolderType = m.FolderType,
                StatusCheckFolder = m.StatusCheckFolder,
                ExtensionFile = m.ExtensionFile,
                ObjectType = m.ObjectType,
                DepartmentId = m.DepartmentId,
            }).FirstOrDefault();

            return resultInfo;
        }

        public List<FolderDefinitionModel> GetFolderDefinitions(int designType, int objectType, string departmentId)
        {
            var rs = db.FolderDefinitions.AsNoTracking().Where(w => w.TypeDefinitionId == designType && w.ObjectType == objectType && departmentId.Equals(w.DepartmentId)).Select(m => new FolderDefinitionModel
            {
                FolderDefinitionId = m.FolderDefinitionId,
                TypeDefinitionId = m.TypeDefinitionId,
                FolderDefinitionManageId = m.FolderDefinitionManageId,
                FolderDefinitionFirst = m.FolderDefinitionFirst,
                FolderDefinitionBetween = m.FolderDefinitionBetween,
                FolderDefinitionLast = m.FolderDefinitionLast,
                FolderDefinitionBetweenIndex = m.FolderDefinitionBetweenIndex,
                StatusCheckFile = m.StatusCheckFile,
                FolderType = m.FolderType,
                StatusCheckFolder = m.StatusCheckFolder,
                ExtensionFile = m.ExtensionFile,
                ObjectType = m.ObjectType,
                DepartmentId = m.DepartmentId
            }).ToList();

            return rs;
        }
    }
}
