using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.DataDistribution;
using NTS.Model.DataDistributionFile;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.DataDistributions
{
    public class DataDistributionBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<DataDistributionModel> SearchDataDistribution(DataDistributionModel modelSearch)
        {
            SearchResultModel<DataDistributionModel> searchResult = new SearchResultModel<DataDistributionModel>();

            var dataQuery = (from a in db.DataDistributions.AsNoTracking()
                             orderby a.Name
                             select new DataDistributionModel()
                             {
                                 Id = a.Id,
                                 DepartmentId = a.DepartmentId,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Description = a.Description,
                                 Type = a.Type,
                                 Path = a.Path,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.ToUpper().Contains(modelSearch.DepartmentId.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            //if (modelSearch.IsCreateUpdate)
            //{
            //    DataDistributionModel firstFolder = new DataDistributionModel();
            //    firstFolder.Name = "Cấp lớn nhất";
            //    firstFolder.Id = "-1";
            //    searchResult.ListResult.Add(firstFolder);
            //}
            return searchResult;
        }

        public void AddDataDistribution(DataDistributionModel model, string userLoginId)
        {
            var dataDistributionExist = db.DataDistributions.AsNoTracking().Where(a => a.ParentId.Equals(model.ParentId) && a.Name.Equals(model.Name) && a.DepartmentId.Equals(model.DepartmentId)).FirstOrDefault();
            if (dataDistributionExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Folder);
            }

            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.ParentId = null;
            }

            var parentFolder = db.DataDistributions.AsNoTracking().Where(a => a.Id.Equals(model.ParentId)).FirstOrDefault();
            if (parentFolder != null)
            {
                model.Path = parentFolder.Path + "/" + model.Name;
            }
            else
            {
                model.Path = model.Name;
            }

            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    DataDistribution newDataDistribution = new DataDistribution
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = model.ParentId,
                        Name = model.Name.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        Type = model.Type,
                        DepartmentId = model.DepartmentId,
                        Path = model.Path,
                        IsExportMaterial = model.IsExportMaterial
                    };

                    db.DataDistributions.Add(newDataDistribution);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, model.Name, null, Constants.LOG_DataDistribution);

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

        public DataDistributionModel GetDataDistributionInfo(DataDistributionModel model)
        {
            var resultInfo = db.DataDistributions.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DataDistributionModel
            {
                Id = p.Id,
                DepartmentId = p.DepartmentId,
                Name = p.Name,
                ParentId = p.ParentId,
                Description = p.Description,
                Type = p.Type,
                Path = p.Path,
                IsExportMaterial = p.IsExportMaterial
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }

            //if (resultInfo.ParentId == null)
            //{
            //    resultInfo.ParentId = "-1";
            //}
            return resultInfo;
        }

        public void UpdateDataDistribution(DataDistributionModel model, string userLoginId)
        {
            var dataQuery = (from a in db.DataDistributions.AsNoTracking()
                             orderby a.Name
                             where a.ParentId != null
                             select new DataDistributionModel()
                             {
                                 Id = a.Id,
                                 DepartmentId = a.DepartmentId,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Description = a.Description,
                                 Type = a.Type,
                                 Path = a.Path,
                                 IsExportMaterial = a.IsExportMaterial
                             }).AsQueryable();

            List<DataDistributionModel> allDataDistribution = new List<DataDistributionModel>();

            if (dataQuery != null)
            {
                allDataDistribution = dataQuery.ToList();
            }

            List<DataDistributionModel> listResult = new List<DataDistributionModel>();

            listResult = GetDataDistributionChild(model.Id, allDataDistribution);

            bool isParentIdOk = true;

            foreach (var item in listResult)
            {
                if (item.Id.Equals(model.ParentId))
                {
                    isParentIdOk = false;
                    break;
                }
            }

            if (model.Id.Equals(model.ParentId))
            {
                isParentIdOk = false;
            }

            if (!isParentIdOk)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0027, TextResourceKey.Folder);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                if (db.DataDistributions.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name)) && o.ParentId.Equals(model.ParentId) && o.DepartmentId.Equals(model.DepartmentId)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Folder);
                }

                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                var parentFolder = db.DataDistributions.Where(a => a.Id.Equals(model.ParentId)).FirstOrDefault();
                if (parentFolder != null)
                {
                    model.Path = parentFolder.Path + "/" + model.Name;
                }
                else
                {
                    model.Path = model.Name;
                }

                try
                {
                    var newDataDistribution = db.DataDistributions.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<DataDistributionHistoryModel>(newDataDistribution);

                    newDataDistribution.Name = model.Name.Trim();
                    newDataDistribution.DepartmentId = model.DepartmentId;
                    newDataDistribution.ParentId = model.ParentId;
                    newDataDistribution.Description = model.Description;
                    newDataDistribution.Type = model.Type;
                    newDataDistribution.Path = model.Path;
                    newDataDistribution.IsExportMaterial = model.IsExportMaterial;

                    if (listResult.Count() > 0)
                    {
                        foreach (var item in listResult)
                        {
                            var newDataDistributionChild = db.DataDistributions.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            newDataDistributionChild.Path = model.Path + "/" + item.Name;
                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DataDistributionHistoryModel>(newDataDistribution);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_DataDistribution, newDataDistribution.Id, newDataDistribution.Name, jsonBefor, jsonApter);

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

        public List<DataDistributionModel> GetListFolderChild(string parentId, List<DataDistributionModel> allDataDistribution)
        {
            List<DataDistributionModel> listResult = new List<DataDistributionModel>();
            var listChild = allDataDistribution.Where(a => a.ParentId.Equals(parentId)).ToList();
            List<DataDistributionModel> listChildChild = new List<DataDistributionModel>();
            foreach (var item in listChild)
            {
                listChildChild = GetListFolderChild(item.Id, allDataDistribution);
                listResult.AddRange(listChild);
                listResult.AddRange(listChildChild);
            }
            return listResult;
        }

        public void DeleteDataDistribution(DataDistributionModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var fileInFolder = db.DataDistributionFileLinks.Where(a => a.DataDistributionId.Equals(model.Id)).ToList();
                    if (fileInFolder.Count() > 0)
                    {
                        db.DataDistributionFileLinks.RemoveRange(fileInFolder);
                    }

                    var dataDistribution = db.DataDistributions.Where(m => m.Id.Equals(model.Id)).FirstOrDefault();
                    if (dataDistribution != null)
                    {
                        db.DataDistributions.Remove(dataDistribution);
                    }


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DataDistributionHistoryModel>(dataDistribution);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_DataDistribution, dataDistribution.Id, dataDistribution.Name, jsonBefor);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }
        }

        private List<DataDistributionModel> GetDataDistributionChild(string parentId, List<DataDistributionModel> listDocumentTemplateType)
        {
            List<DataDistributionModel> listResult = new List<DataDistributionModel>();
            var listChild = listDocumentTemplateType.Where(r => r.ParentId.Equals(parentId)).ToList();

            List<DataDistributionModel> listChildChild = new List<DataDistributionModel>();
            foreach (var child in listChild)
            {
                listChildChild = GetDataDistributionChild(child.Id, listDocumentTemplateType);

                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public SearchResultModel<DataDistributionFileModel> GetDataDistributionFile(DataDistributionModel modelSearch)
        {
            SearchResultModel<DataDistributionFileModel> searchResult = new SearchResultModel<DataDistributionFileModel>();

            var dataQuery = (from a in db.DataDistributionFiles.AsNoTracking()
                             join b in db.DataDistributionFileLinks.AsNoTracking() on a.Id equals b.DataDistributionFileId
                             where b.DataDistributionId.Equals(modelSearch.Id)
                             orderby a.Name
                             select new DataDistributionFileModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 FolderContain = a.FolderContain,
                                 FolderPath = a.FolderPath,
                                 FileName = a.FileName,
                                 GetType = a.GetType,
                                 FilterThongSo = a.FilterThongSo,
                                 FilterMaVatLieu = a.FilterMaVatLieu,
                                 FilterDonVi = a.FilterDonVi,
                                 Type = a.Type,
                                 MAT = a.MAT,
                                 TEM = a.TEM,
                                 Description = a.Description,
                                 Extension = a.Extension,
                                 DataDistributionId = b.DataDistributionId,
                                 DataDistributionFileLinkId = b.Id
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void DeleteDataDistributionFileByFolder(DataDistributionFileModel model, string userLoginId)
        {
            var file = db.DataDistributionFileLinks.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (file == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.DataDistributionFileLinks.Remove(file);

                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_DataDistribution, file.Id, "Xóa folder");

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }
        }

        public object SearchDataDistributionFile(DataDistributionFileSearchModel modelSearch)
        {
            SearchResultModel<DataDistributionFileModel> searchResult = new SearchResultModel<DataDistributionFileModel>();

            var dataQuery = (from a in db.DataDistributionFiles.AsNoTracking()
                             where !modelSearch.ListSelectId.Contains(a.Id)
                             orderby a.Name
                             select new DataDistributionFileModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 FolderContain = a.FolderContain,
                                 FolderPath = a.FolderPath,
                                 FileName = a.FileName,
                                 GetType = a.GetType,
                                 FilterThongSo = a.FilterThongSo,
                                 FilterMaVatLieu = a.FilterMaVatLieu,
                                 FilterDonVi = a.FilterDonVi,
                                 Type = a.Type,
                                 MAT = a.MAT,
                                 TEM = a.TEM,
                                 Description = a.Description,
                                 Extension = a.Extension
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddDataDistributionFileLink(DataDistributionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ListFile.Count() > 0)
                    {
                        DataDistributionFileLink dataDistributionFileLink;
                        foreach (var item in model.ListFile)
                        {
                            dataDistributionFileLink = new DataDistributionFileLink();
                            dataDistributionFileLink.Id = Guid.NewGuid().ToString();
                            dataDistributionFileLink.DataDistributionId = model.Id;
                            dataDistributionFileLink.DataDistributionFileId = item.Id;
                            db.DataDistributionFileLinks.Add(dataDistributionFileLink);
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

        public void AddDataDistributionFile(DataDistributionFileModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    DataDistributionFile newDataDistribution = new DataDistributionFile
                    {
                        Id = Guid.NewGuid().ToString(),
                        GetType = model.GetType,
                        Name = model.Name,
                        Type = model.Type,
                        FolderContain = model.FolderContain,
                        Description = model.Description,
                        Extension = model.Extension,
                        FilterThongSo = model.FilterThongSo,
                        FilterDonVi = model.FilterDonVi,
                        TEM = model.TEM,
                        MAT = model.MAT,
                        FilterMaVatLieu = model.FilterMaVatLieu,
                        FilterManufacturer = model.FilterManufacturer,
                        FilterMaterialCodeStart = model.FilterMaterialCodeStart,
                        FilterRawMaterial = model.FilterRawMaterial,
                        FilterRawMaterialCode = model.FilterRawMaterialCode
                    };

                    db.DataDistributionFiles.Add(newDataDistribution);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, newDataDistribution.Name, newDataDistribution.Id, Constants.LOG_DataDistribution);
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

        public void UpdateDataDistributionFile(DataDistributionFileModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newDataDistributionFile = db.DataDistributionFiles.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();


                    //var jsonApter = AutoMapperConfig.Mapper.Map<DataDistributionFileHistoryModel>(newDataDistributionFile);

                    newDataDistributionFile.GetType = model.GetType;
                    newDataDistributionFile.Name = model.Name;
                    newDataDistributionFile.Type = model.Type;
                    newDataDistributionFile.FolderContain = model.FolderContain;
                    newDataDistributionFile.Description = model.Description;
                    newDataDistributionFile.Extension = model.Extension;
                    newDataDistributionFile.FilterThongSo = model.FilterThongSo;
                    newDataDistributionFile.FilterDonVi = model.FilterDonVi;
                    newDataDistributionFile.TEM = model.TEM;
                    newDataDistributionFile.MAT = model.MAT;
                    newDataDistributionFile.FilterMaVatLieu = model.FilterMaVatLieu;
                    newDataDistributionFile.FilterManufacturer = model.FilterManufacturer;
                    newDataDistributionFile.FilterMaterialCodeStart = model.FilterMaterialCodeStart;
                    newDataDistributionFile.FilterRawMaterial = model.FilterRawMaterial;
                    newDataDistributionFile.FilterRawMaterialCode = model.FilterRawMaterialCode;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<DataDistributionFileHistoryModel>(newDataDistributionFile);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_EmployeeTraining, newDataDistributionFile.Id, newDataDistributionFile.Name, jsonBefor, jsonApter);

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

        public void DeleteDataDistributionFile(DataDistributionFileModel model, string userLoginId)
        {
            var dataDistributionFileLink = db.DataDistributionFileLinks.Where(a => a.DataDistributionFileId.Equals(model.Id)).FirstOrDefault();
            if (dataDistributionFileLink != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.File);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dataDistributionFile = db.DataDistributionFiles.Where(m => m.Id.Equals(model.Id)).FirstOrDefault();
                    if (dataDistributionFile != null)
                    {
                        db.DataDistributionFiles.Remove(dataDistributionFile);
                    }


                    //var jsonApter = AutoMapperConfig.Mapper.Map<DataDistributionFileHistoryModel>(dataDistributionFile);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_DataDistribution, dataDistributionFile.Id, dataDistributionFile.Name, jsonApter);

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

        public DataDistributionFileModel GetDataDistributionFileInfo(DataDistributionFileModel model)
        {
            var resultInfo = db.DataDistributionFiles.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DataDistributionFileModel
            {
                Id = p.Id,
                GetType = p.GetType,
                Name = p.Name,
                Type = p.Type,
                FolderContain = p.FolderContain,
                Description = p.Description,
                Extension = p.Extension,
                FilterThongSo = p.FilterThongSo,
                FilterDonVi = p.FilterDonVi,
                TEM = p.TEM,
                MAT = p.MAT,
                FilterMaVatLieu = p.FilterMaVatLieu,
                FilterRawMaterialCode = p.FilterRawMaterialCode,
                FilterRawMaterial = p.FilterRawMaterial,
                FilterMaterialCodeStart = p.FilterMaterialCodeStart,
                FilterManufacturer = p.FilterManufacturer
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.File);
            }

            //if (resultInfo.ParentId == null)
            //{
            //    resultInfo.ParentId = "-1";
            //}
            return resultInfo;
        }
    }
}
