using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.FileDefinition;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.FileDefinitions
{
    public class FileDefinitionBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public List<FileDefinitionModel> GetListFileDefinition(FileDefinitionModel model)
        {
            List<FileDefinitionModel> list = new List<FileDefinitionModel>();
            var data = db.FileDefinitions.AsNoTracking().Where(t => t.FolderDefinitionId.Equals(model.FolderDefinitionId)).Select(m => new FileDefinitionModel
            {
                FileDefinitionId = m.FileDefinitionId,
                FolderDefinitionId = m.FolderDefinitionId,
                FileDefinitionNameFirst = m.FileDefinitionNameFirst,
                FileDefinitionNameBetween = m.FileDefinitionNameBetween,
                FileDefinitionNameBetweenIndex = m.FileDefinitionNameBetweenIndex,
                FileDefinitionNameLast = m.FileDefinitionNameLast,
                FileType = m.FileType,
                TypeDefinitionId = m.TypeDefinitionId,
                ObjectType = m.ObjectType
            }).ToList();

            string objectTypeName = GetObjectTypeName(model.ObjectType);

            list = data;
            foreach (var item in list)
            {
                item.Name = item.FileDefinitionNameFirst;
                if (item.FileDefinitionNameBetween == 1)
                {
                    item.Name += "MaNhom" + objectTypeName;
                }
                else if (item.FileDefinitionNameBetween == 2)
                {
                    item.Name += "Ma" + objectTypeName;
                }
                else if (item.FileDefinitionNameBetween == 3)
                {
                    item.Name += "MaNhomCha" + objectTypeName;
                }
                else if (item.FileDefinitionNameBetween == 4)
                {
                    item.Name += "( )";
                }
                else if (item.FileDefinitionNameBetween == Constants.FolderDefinitionBetween_Module)
                {
                    item.Name += "MaModuleNguon";
                }

                if (item.FileDefinitionNameBetweenIndex == 1)
                {
                    item.Name += "(01-99)";
                }
                else if (item.FileDefinitionNameBetweenIndex == 2)
                {
                    item.Name += "(a-z)";
                }
                item.Name += item.FileDefinitionNameLast;
            }
            return list;
        }

        /// <summary>
        /// Lấy tên loại đối tượng theo loại
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
        public void AddFileDefinition(FileDefinitionModel model, string departmentId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var folderDefinition = db.FolderDefinitions.FirstOrDefault(r => r.FolderDefinitionId.Equals(model.FolderDefinitionId));

                    if (folderDefinition == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FolderDefinition);
                    }

                    FileDefinition fileDefinition = new FileDefinition()
                    {
                        FileDefinitionId = Guid.NewGuid().ToString(),
                        FolderDefinitionId = model.FolderDefinitionId,
                        FileDefinitionNameFirst = model.FileDefinitionNameFirst.NTSTrim(),
                        FileDefinitionNameBetween = model.FileDefinitionNameBetween,
                        FileDefinitionNameBetweenIndex = model.FileDefinitionNameBetweenIndex,
                        FileDefinitionNameLast = model.FileDefinitionNameLast.NTSTrim(),
                        FileType = model.FileType,
                        TypeDefinitionId = folderDefinition.TypeDefinitionId,
                        ObjectType = folderDefinition.ObjectType,
                        DepartmentId = departmentId
                    };

                    db.FileDefinitions.Add(fileDefinition);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, fileDefinition.FileDefinitionNameLast, fileDefinition.FileDefinitionId, Constants.LOG_FolderDefinition);
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

        public void UpdateFileDefinition(FileDefinitionModel model, string departmentId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var resultInfo = db.FileDefinitions.Where(u => u.FileDefinitionId.Equals(model.FileDefinitionId)).FirstOrDefault();
                if (resultInfo == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FileDefinition);
                }

                if (!departmentId.Equals(resultInfo.DepartmentId))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.FolderDefinition);
                }

                try
                {
                    var fileDefinition = db.FileDefinitions.Where(r => r.FileDefinitionId.Equals(model.FileDefinitionId)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<FileDefinitionHistoryModel>(fileDefinition);

                    fileDefinition.FolderDefinitionId = model.FolderDefinitionId;
                    fileDefinition.FileDefinitionNameFirst = model.FileDefinitionNameFirst.NTSTrim();
                    fileDefinition.FileDefinitionNameBetween = model.FileDefinitionNameBetween;
                    fileDefinition.FileDefinitionNameBetweenIndex = model.FileDefinitionNameBetweenIndex;
                    fileDefinition.FileDefinitionNameLast = model.FileDefinitionNameLast.NTSTrim();
                    fileDefinition.FileType = model.FileType;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<FileDefinitionHistoryModel>(fileDefinition);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_FolderDefinition, fileDefinition.FolderDefinitionId, fileDefinition.FileDefinitionNameLast, jsonBefor, jsonApter);

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

        public void DeleteFileDefinition(FileDefinitionModel model, string departmentId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var folderDefinition = db.FolderDefinitions.AsNoTracking().Where(m => m.FolderDefinitionId.Equals(model.FolderDefinitionId)).FirstOrDefault();
                if (folderDefinition != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FileDefinition);
                }

                try
                {
                    var fileDefinition = db.FileDefinitions.FirstOrDefault(u => u.FileDefinitionId.Equals(model.FileDefinitionId));
                    if (fileDefinition == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FileDefinition);
                    }

                    if (!departmentId.Equals(fileDefinition.DepartmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.FolderDefinition);
                    }

                    db.FileDefinitions.Remove(fileDefinition);
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

        public FileDefinitionModel GetFileDefinitionInfo(FileDefinitionModel model)
        {
            var resultInfo = db.FileDefinitions.AsNoTracking().Where(u => model.FileDefinitionId.Equals(u.FileDefinitionId)).Select(m => new FileDefinitionModel
            {
                FileDefinitionId = m.FileDefinitionId,
                FolderDefinitionId = m.FolderDefinitionId,
                FileDefinitionNameFirst = m.FileDefinitionNameFirst,
                FileDefinitionNameBetween = m.FileDefinitionNameBetween,
                FileDefinitionNameBetweenIndex = m.FileDefinitionNameBetweenIndex,
                FileDefinitionNameLast = m.FileDefinitionNameLast,
                FileType = m.FileType,
                TypeDefinitionId = m.TypeDefinitionId,
                ObjectType = m.ObjectType
            }).FirstOrDefault();

            return resultInfo;
        }

        /// <summary>
        /// Lấy danh sách file
        /// </summary>
        /// <param name="designType">Loại thiết kế</param>
        /// <param name="objectType">Loại đối tượng</param>
        /// <returns></returns>
        public List<FileDefinitionModel> GetFileDefinitions(int designType, int objectType, string departmentId)
        {
            var rs = db.FileDefinitions.AsNoTracking().Where(w => w.TypeDefinitionId == designType && w.ObjectType == objectType && departmentId.Equals(w.DepartmentId)).Select(m => new FileDefinitionModel
            {
                FileDefinitionId = m.FileDefinitionId,
                FolderDefinitionId = m.FolderDefinitionId,
                FileDefinitionNameFirst = m.FileDefinitionNameFirst,
                FileDefinitionNameBetween = m.FileDefinitionNameBetween,
                FileDefinitionNameBetweenIndex = m.FileDefinitionNameBetweenIndex,
                FileDefinitionNameLast = m.FileDefinitionNameLast,
                FileType = m.FileType,
                TypeDefinitionId = m.TypeDefinitionId
            }).ToList();

            return rs;
        }
    }
}
