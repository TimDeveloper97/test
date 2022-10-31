using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.HistoryVersion
{
    public class HistoryVersionBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public HistoryVersionModel GetDataHistoryVersion(HistoryVersionModel model)
        {
            HistoryVersionModel result = new HistoryVersionModel();
            if (model.Type == Constants.HistoryVersion_Type_Module)
            {
                var data = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    result = new HistoryVersionModel()
                    {
                        Id = data.Id,
                        Type = Constants.HistoryVersion_Type_Module,
                        Version = data.CurrentVersion,
                        EditContent = data.EditContent,
                        UpdateDate = data.UpdateDate
                    };
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                }
            }
            else if (model.Type == Constants.HistoryVersion_Type_Product)
            {
                var data = db.Products.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    result = new HistoryVersionModel()
                    {
                        Id = data.Id,
                        Type = Constants.HistoryVersion_Type_Product,
                        Version = data.CurentVersion,
                        EditContent = data.Content,
                        UpdateDate = data.UpdateDate
                    };
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                }
            }
            else if (model.Type == Constants.HistoryVersion_Type_Practice)
            {
                var data = db.Practices.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    result = new HistoryVersionModel()
                    {
                        Id = data.Id,
                        Type = Constants.HistoryVersion_Type_Practice,
                        Version = data.CurentVersion,
                        EditContent = data.Content,
                        UpdateDate = data.UpdateDate
                    };
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
                }
            }

            return result;
        }

        public void UpdateVersion(HistoryVersionModel model, string userId)
        {
            if (model.Type == Constants.HistoryVersion_Type_Module)
            {
                var data = db.Modules.FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    ModuleOldVersion moduleHistory = new ModuleOldVersion()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ModuleId = model.Id,
                        Content = model.EditContent,
                        Version = model.Version,
                        CreateBy = userId,
                        CreateDate = DateTime.Now
                    };

                    data.CurrentVersion = model.Version + 1;
                    data.EditContent = model.Content;
                    data.UpdateBy = userId;
                    data.UpdateDate = DateTime.Now;
                    db.ModuleOldVersions.Add(moduleHistory);

                    var moduleDesignDocument = (from r in db.ModuleDesignDocuments
                                                where r.ModuleId == model.Id
                                                select r);

                    ModuleDesignDocumentOld moduleDesignDocumentOld = new ModuleDesignDocumentOld();
                    List<ModuleDesignDocumentOld> listModuleDocumentOld = new List<ModuleDesignDocumentOld>();
                    foreach (var item in moduleDesignDocument)
                    {
                        moduleDesignDocumentOld = new ModuleDesignDocumentOld()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ModuleOldVersionId = moduleHistory.Id,
                            Name = item.Name,
                            Path = item.Path,
                            ServerPath = item.ServerPath,
                            FileSize = item.FileSize,
                            FileType = item.FileType,
                            ParentId = item.ParentId,
                            DesignType = item.DesignType,
                            CreateBy = item.CreateBy,
                            CreateDate = item.CreateDate,
                            UpdateBy = item.UpdateBy,
                            UpdateDate = item.UpdateDate,
                            HashValue = item.HashValue
                        };
                        listModuleDocumentOld.Add(moduleDesignDocumentOld);
                    }
                    db.ModuleDesignDocumentOlds.AddRange(listModuleDocumentOld);
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                }
            }
            else if (model.Type == Constants.HistoryVersion_Type_Product)
            {
                var data = db.Products.FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    ProductVersion productVersion = new ProductVersion()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = model.Id,
                        Content = model.EditContent,
                        Version = model.Version,
                        CreateBy = userId,
                        CreateDate = DateTime.Now
                    };

                    data.CurentVersion = model.Version + 1;
                    data.Content = model.Content;
                    data.UpdateBy = userId;
                    data.UpdateDate = DateTime.Now;
                    db.ProductVersions.Add(productVersion);
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Product);
                }
            }
            else if (model.Type == Constants.HistoryVersion_Type_Practice)
            {
                var data = db.Practices.FirstOrDefault(i => i.Id.Equals(model.Id));
                if (data != null)
                {
                    PracticeOldVersion practiceOldVersion = new PracticeOldVersion()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PracticeId = model.Id,
                        Content = model.EditContent,
                        Version = model.Version,
                        CreateBy = userId,
                        CreateDate = DateTime.Now
                    };

                    data.CurentVersion = model.Version + 1;
                    data.EditContent = model.Content;
                    data.UpdateBy = userId;
                    data.UpdateDate = DateTime.Now;
                    db.PracticeOldVersions.Add(practiceOldVersion);
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
                }
            }
            db.SaveChanges();
        }
    }
}
