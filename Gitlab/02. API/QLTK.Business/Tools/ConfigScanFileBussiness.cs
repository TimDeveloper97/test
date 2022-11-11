using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.ConfigScanFile;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ConfigScanFiles
{
    public class ConfigScanFileBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ConfigScanFileModel> SearchConfigScanFile(ConfigScanFileSearchModel model)
        {
            SearchResultModel<ConfigScanFileModel> searchResult = new SearchResultModel<ConfigScanFileModel>();

            var dataQuery = (from a in db.ConfigScanFiles.AsNoTracking()
                             select new ConfigScanFileModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 Type = a.Type,
                                 PathFolderC = a.PathFolderC
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                dataQuery = dataQuery.Where(u => u.Description.ToUpper().Contains(model.Description.ToUpper()));
            }

            if (model.Type != 0)
            {
                dataQuery = dataQuery.Where(u => u.Type == model.Type);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddConfigScanFile(ConfigScanFileModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ConfigScanFile configScan = new ConfigScanFile
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        Type = model.Type,
                        PathFolderC = model.PathFolderC,
                        CreatedBy = model.CreateBy,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = model.CreateBy,
                        UpdatedDate = DateTime.Now
                    };

                    db.ConfigScanFiles.Add(configScan);
                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, configScan.Code, configScan.Id, Constants.LOG_FileScan);
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
        public void UpdateConfigScanFile(ConfigScanFileModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var configScanFile = db.ConfigScanFiles.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ScanFileHistoryModel>(configScanFile);

                    configScanFile.Name = model.Name.Trim();
                    configScanFile.Code = model.Code.Trim();
                    configScanFile.Description = model.Description.Trim();
                    configScanFile.Type = model.Type;
                    configScanFile.PathFolderC = model.PathFolderC;
                    configScanFile.UpdatedBy = model.UpdateBy;
                    configScanFile.UpdatedDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeTrainingHistoryModel>(configScanFile);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_FileScan, configScanFile.Id, configScanFile.Code, jsonBefor, jsonApter);

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

        public void DeleteConfigScanFile(ConfigScanFileModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var configScanFile = db.ConfigScanFiles.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (configScanFile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ConfigScanFile);
                    }

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ScanFileHistoryModel>(configScanFile);

                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_EmployeeTraining, configScanFile.Id, configScanFile.Code, jsonApter);

                    db.ConfigScanFiles.Remove(configScanFile);
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
        public object GetConfigScanFileInfo(ConfigScanFileModel model)
        {
            var resultInfo = db.ConfigScanFiles.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ConfigScanFileModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                Type = p.Type,
                PathFolderC = p.PathFolderC
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ConfigScanFile);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(ConfigScanFileModel model)
        {
            if (db.ConfigScanFiles.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ConfigScanFile);
            }

            if (db.ConfigScanFiles.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ConfigScanFile);
            }
        }

        public void CheckExistedForUpdate(ConfigScanFileModel model)
        {
            if (db.ConfigScanFiles.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ConfigScanFile);
            }

            if (db.ConfigScanFiles.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ConfigScanFile);
            }
        }
    }
}
