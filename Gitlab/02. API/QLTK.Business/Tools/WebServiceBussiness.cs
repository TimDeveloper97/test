using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model.ClassRoom;
using NTS.Model.Combobox;
using NTS.Model.ConverUnit;
using NTS.Model.DataCheckModuleUpload;
using NTS.Model.DesignDocuments;
using NTS.Model.DesignStructure;
using NTS.Model.Error;
using NTS.Model.GeneralTemplate;
using NTS.Model.Materials;
using NTS.Model.ModuleDesignDocument;
using NTS.Model.Product;
using NTS.Model.QLTKMODULE;
using NTS.Model.RawMaterial;
using NTS.Model.Repositories;
using NTS.Model.Solution;
using NTS.Model.WebService;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business
{
    public class WebServiceBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public object SearchDesign3D()
        {
            var list3D = (from a in db.Design3D.AsNoTracking()
                          orderby a.FileName
                          select new
                          {
                              a.Id,
                              a.FileName,
                              a.FilePath,
                              a.Size,
                          }).ToList();
            return list3D;
        }

        /// <summary>
        /// Lấy danh sách module
        /// </summary>
        /// <returns></returns>
        public List<DataCheckModuleModel> GetModules()
        {
            var moduels = (from a in db.Modules.AsNoTracking()
                           select new DataCheckModuleModel
                           {
                               Name = a.Name,
                               Code = a.Code,
                           }).ToList();
            return moduels;
        }

        public object SearchListMaterial()
        {
            var listMaterial = (from a in db.Materials.AsNoTracking()
                                orderby a.Name
                                join c in db.Manufactures on a.ManufactureId equals c.Id
                                join d in db.MaterialGroups on a.MaterialGroupId equals d.Id
                                select new
                                {
                                    a.Id,
                                    a.Name,
                                    a.Code,
                                    ManufactureName = c.Name,
                                    ManufactureCode = c.Code,
                                    a.Note,
                                    a.CreateBy,
                                    IsUsuallyUse = a.IsUsuallyUse != null ? a.IsUsuallyUse : false,
                                    a.Status,
                                    MaterialGroupName = d.Name,
                                    MaterialGroupCode = d.Code,
                                }).ToList();
            return listMaterial;
        }

        public object SearchListModuleDesignDocument()
        {
            var list = (from a in db.ModuleDesignDocuments.AsNoTracking()
                        select new
                        {
                            a.Id,
                            a.ModuleId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.FileSize,
                            a.FileType,
                            a.DesignType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            return list;
        }

        public object SearchListRawMaterial()
        {
            var list = (from a in db.RawMaterials.AsNoTracking()
                        select new
                        {
                            a.Id,
                            a.Code,
                            a.Name,
                            a.Price,
                            a.Note
                        }).ToList();
            return list;
        }

        public object SearchErrorModuleNotDone()
        {
            var list = (from a in db.Errors.AsNoTracking()
                        where a.Status < 5
                        join b in db.Modules on a.ModuleErrorVisualId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        select new
                        {
                            ModuleErrorVisualCode = b.Code
                        }).ToList();
            return list;
        }

        public object SearchConvertUnit()
        {
            var list = (from a in db.ConverUnits.AsNoTracking()
                        orderby a.Quantity
                        join b in db.Materials on a.MaterialId equals b.Id
                        select new
                        {
                            Id = a.Id,
                            MaterialCode = b.Code,
                        }).AsQueryable();
            return list;
        }

        public object SearchDesignStructure()
        {
            var list = (from a in db.DesignStructures.AsNoTracking()
                        orderby a.Name
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.Type,
                            a.Extension,
                            a.Path,
                            a.ParentPath
                        }).ToList();
            return list;
        }

        public object SearchDesignStructureFile()
        {
            var list = (from a in db.DesignStructureFiles.AsNoTracking()
                        orderby a.Name
                        select new
                        {
                            a.DesignStructureId,
                            a.Name,
                            a.Exist,
                            a.IsTemplate,
                            a.IsInsertData,
                            a.Path

                        }).ToList();
            return list;
        }

        public object SearchModule(string moduleCode)
        {
            var module = (from a in db.Modules.AsNoTracking()
                          where a.Code.Equals(moduleCode)
                          select new
                          {
                              a.Specification
                          }).FirstOrDefault();
            return module;
        }

        public ResultApiModel SearchListModule()
        {
            var list = (from a in db.Modules.AsNoTracking()
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.Code,
                            a.Specification
                        }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel CheckFileTPAUse(string path, string moduleCode, long size)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var moduleId = db.Modules.Where(a => a.Code.Equals(moduleCode)).Select(b => b.Id).FirstOrDefault();
            if (moduleId == null)
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.Module);
                return resultApiModel;
            }

            var moduleDesignDocument = (from a in db.ModuleDesignDocuments.AsNoTracking()
                                        where a.ModuleId.Equals(moduleId) && a.Path.Equals(path)
                                        select new ModuleDesignDocumentModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Path = a.Path,
                                            FileSize = a.FileSize
                                        }).FirstOrDefault();

            if (moduleDesignDocument != null)
            {
                resultApiModel.SuccessStatus = true;
                resultApiModel.Data = moduleDesignDocument;
                if (size != moduleDesignDocument.FileSize)
                {
                    resultApiModel.Message = $"File {path} khác size trên nguồn";
                }
            }
            else
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = $"FILE {path} DÙNG CHUNG KHÔNG CÓ TRÊN NGUỒN";
            }

            return resultApiModel;
        }

        public ResultApiModel CheckFileSize(string path, string moduleCode, long size)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var moduleId = db.Modules.AsNoTracking().Where(a => a.Code.Equals(moduleCode)).Select(b => b.Id).FirstOrDefault();
            if (moduleId == null)
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.Module);
                return resultApiModel;
            }

            var moduleDesignDocument = (from a in db.ModuleDesignDocuments.AsNoTracking()
                                        where a.ModuleId.Equals(moduleId) && a.Path.Equals(path)
                                        select new ModuleDesignDocumentModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Path = a.Path,
                                            FileSize = a.FileSize
                                        }).FirstOrDefault();

            if (moduleDesignDocument != null)
            {
                resultApiModel.Data = moduleDesignDocument;
                if (size != moduleDesignDocument.FileSize)
                {
                    resultApiModel.Message = $"File {path} khác size trên nguồn";
                }
            }
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel LoadCAD(List<CADModel> listCAD)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            if (listCAD != null)
            {
                foreach (var item in listCAD)
                {
                    var documentDesign = db.ModuleDesignDocuments.AsNoTracking().Where(a => a.Path.Replace("/", "").Replace(@"\", "").Equals(item.FTPFilePath.Replace("/", "").Replace(@"\", ""))).FirstOrDefault();
                    if (documentDesign != null)
                    {
                        item.IdGoogleApi = documentDesign.Id;
                    }
                }
            }

            resultApiModel.Data = listCAD;
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetListManufacture()
        {
            var listManufacture = (from a in db.Manufactures.AsNoTracking()
                                   orderby a.Name
                                   select new
                                   {
                                       a.Id,
                                       a.Name,
                                       a.Code,
                                       a.Website,
                                       a.Email,
                                       a.Address,
                                       a.Phone,
                                       a.Description,
                                       a.MaterialType,
                                       a.Status
                                   }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = listManufacture.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel SearchListUnit()
        {
            var listUnit = (from a in db.Units.AsNoTracking()
                            orderby a.Name
                            select new
                            {
                                a.Id,
                                a.Name,
                                a.Index,
                                a.Description
                            }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = listUnit.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel CheckFileCADShare(string path, string moduleCode, long size)
        {
            ResultApiModel resultApiModel = new ResultApiModel();

            //var moduleDesignDocument = db.ModuleDesignDocuments.Where(a => a.Name.Equals(moduleCode + ".dwg")).FirstOrDefault();

            var moduleDesignDocument = (from a in db.ModuleDesignDocuments.AsNoTracking()
                                        where a.Name.Equals(moduleCode + ".dwg")
                                        select new ModuleDesignDocumentModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            ModuleId = a.ModuleId,
                                            FileSize = a.FileSize,
                                            DesignType = a.DesignType,
                                            ParentId = a.ParentId,
                                            Path = a.Path,
                                            FileType = a.FileType

                                        }).FirstOrDefault();
            if (moduleDesignDocument != null)
            {
                resultApiModel.SuccessStatus = true;
                resultApiModel.Data = moduleDesignDocument;
            }
            else
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = "FILE TPA DÙNG CHUNG KHÔNG CÓ TRÊN NGUỒN";
            }

            return resultApiModel;
        }

        public ResultApiModel GetDesignStructure(string moduleCode, int type)
        {

            var dataQuery = (from a in db.DesignStructures.AsNoTracking()
                             where a.Type == type
                             select new DesignStructureResultModel
                             {
                                 Name = a.Name,
                                 Id = a.Id,
                                 ParentId = a.ParentId,
                                 Type = a.Type,
                                 Path = a.Path,
                                 ListFile = db.DesignStructureFiles.Where(t => t.DesignStructureId.Equals(a.Id)).Select(m => new NTS.Model.DesignStructure.DesignStructureFileModel
                                 {
                                     Id = m.Id,
                                     Name = m.Name,
                                     DesignStructureId = m.DesignStructureId,
                                     Description = m.Description,
                                     Exist = m.Exist,
                                     IsTemplate = m.IsTemplate,
                                     IsInsertData = m.IsInsertData,
                                     Path = m.Path,
                                 }).ToList(),
                             }).AsQueryable();
            List<DesignStructureResultModel> listResult = new List<DesignStructureResultModel>();
            listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                item.Path = item.Path.Replace("code", moduleCode);
            }
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = listResult;
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetDesign3D()
        {
            var list3D = (from a in db.Design3D.AsNoTracking()
                          orderby a.FileName
                          select new
                          {
                              a.Id,
                              a.FileName,
                              a.FilePath,
                              a.Size,
                          }).ToList();

            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list3D;
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public List<CheckMatrialModel> GetListMaterial()
        {
            var listMaterial = (from a in db.Materials.AsNoTracking()
                                orderby a.Name
                                join c in db.Manufactures.AsNoTracking() on a.ManufactureId equals c.Id
                                join d in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals d.Id
                                join e in db.Units.AsNoTracking() on a.UnitId equals e.Id
                                select new CheckMatrialModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Code = a.Code,
                                    ManufactureCode = c.Code,
                                    Status = a.Status,
                                    UnitName = e.Name,
                                    Is3DExist = a.Is3DExist
                                }).ToList();

            return listMaterial;
        }

        public ResultApiModel GetModuleDesignDocument()
        {
            var list = (from a in db.ModuleDesignDocuments.AsNoTracking()
                        select new
                        {
                            a.Id,
                            a.ModuleId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.FileSize,
                            a.FileType,
                            a.DesignType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetModuleDesignDocument(string designType)
        {
            var list = (from a in db.ModuleDesignDocuments.AsNoTracking()
                        where a.FileType.Equals(designType)
                        select new
                        {
                            a.Id,
                            a.ModuleId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.FileSize,
                            a.FileType,
                            a.DesignType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetRawMaterial()
        {
            var list = (from a in db.RawMaterials.AsNoTracking()
                        select new
                        {
                            a.Id,
                            a.Code,
                            a.Name,
                            a.Price,
                            a.Note
                        }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetErrorModuleNotDone()
        {
            var list = (from a in db.Errors.AsNoTracking()
                        where a.Status < 5
                        join b in db.Modules.AsNoTracking() on a.ModuleErrorVisualId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        select new
                        {
                            ModuleErrorVisualCode = b.Code
                        }).AsQueryable();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetConvertUnit()
        {
            var list = (from a in db.ConverUnits.AsNoTracking()
                        orderby a.Quantity
                        join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                        select new
                        {
                            Id = a.Id,
                            MaterialCode = b.Code,
                        }).AsQueryable();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetDesignStructure()
        {
            var list = (from a in db.DesignStructures.AsNoTracking()
                        orderby a.Name
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.Type,
                            a.Extension,
                            a.Path,
                            a.ParentPath
                        }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetDesignStructureFile()
        {
            var list = (from a in db.DesignStructureFiles.AsNoTracking()
                        orderby a.Name
                        select new
                        {
                            a.DesignStructureId,
                            a.Name,
                            a.Exist,
                            a.IsTemplate,
                            a.IsInsertData,
                            a.Path,
                        }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public ResultApiModel GetDesignStructureFile(string DesignStrctureId)
        {
            var list = (from a in db.DesignStructureFiles.AsNoTracking()
                        where a.DesignStructureId.Equals(DesignStrctureId)
                        orderby a.Name
                        select new
                        {
                            a.DesignStructureId,
                            a.Name,
                            a.Exist,
                            a.IsTemplate,
                            a.IsInsertData,
                            a.Path,
                        }).ToList();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = list.ToList();
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        public object GetListDesignStrcture()
        {
            var listDesignStrcture = (from a in db.DesignStructures.AsNoTracking()
                                      where a.Type == 1
                                      orderby a.Name
                                      select new
                                      {
                                          a.Id,
                                          a.Name,
                                          a.ParentId,
                                          a.Path,
                                          a.ParentPath,
                                      }).ToList();
            return listDesignStrcture;
        }

        public ResultApiModel GetFileInFolderMAT(MATModel model)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            try
            {
                var moduleDocument = (from a in db.ModuleDesignDocuments.AsNoTracking()
                                      join m in db.Modules.AsNoTracking() on a.ModuleId equals m.Id
                                      where m.Code.ToUpper().Equals(model.ModuleCode.ToUpper())
                                      select a).ToList();

                var fileMAT = (from a in moduleDocument
                               join f in model.FileLocals on a.Name equals f.FileName
                               where a.Path.Contains(model.NamePath)
                               select new MATFileResultModel
                               {
                                   FileName = f.FileName,
                                   FilePath = f.FilePath,
                                   FileServerSize = a.FileSize,
                                   FileSize = f.FileSize,
                                   Status = a.FileSize != f.FileSize ? Constants.ModuleDesignDocument_FileMAT_Status_Size : Constants.ModuleDesignDocument_FileMAT_Status_None
                               }).ToList();

                var fileMatNameError = (from f in model.FileLocals
                                        join m in fileMAT on f.FilePath equals m.FilePath into fm
                                        from fmn in fm.DefaultIfEmpty()
                                        where fmn == null
                                        select new MATFileResultModel
                                        {
                                            FileName = f.FileName,
                                            FilePath = f.FilePath,
                                            FileServerSize = 0,
                                            FileSize = f.FileSize,
                                            Status = Constants.ModuleDesignDocument_FileMAT_Status_Name
                                        }).ToList();

                fileMAT.AddRange(fileMatNameError);
                resultApiModel.Data = fileMAT.OrderBy(o => o.FilePath);
                resultApiModel.SuccessStatus = true;
            }
            catch
            {
                resultApiModel.SuccessStatus = false;
            }

            return resultApiModel;
        }

        public ResultApiModel GetFileInFolderIGS(IGSModel model)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            try
            {
                var moduleDocument = (from a in db.ModuleDesignDocuments.AsNoTracking()
                                      join m in db.Modules.AsNoTracking() on a.ModuleId equals m.Id
                                      where m.Code.ToUpper().Equals(model.ModuleCode.ToUpper())
                                      select a).ToList();

                var fileIGS = (from a in moduleDocument
                               join f in model.FileLocals on a.Name equals f.FileName
                               where a.Path.Contains(model.NamePath)
                               select new IGSFileResultModel
                               {
                                   FileName = f.FileName,
                                   FilePath = f.FilePath,
                                   FileServerSize = a.FileSize,
                                   FileSize = f.FileSize,
                                   Status = a.FileSize != f.FileSize ? Constants.ModuleDesignDocument_FileIGS_Status_Size : Constants.ModuleDesignDocument_FileIGS_Status_None
                               }).ToList();

                var fileMatNameError = (from f in model.FileLocals
                                        join m in fileIGS on f.FilePath equals m.FilePath into fm
                                        from fmn in fm.DefaultIfEmpty()
                                        where fmn == null
                                        select new IGSFileResultModel
                                        {
                                            FileName = f.FileName,
                                            FilePath = f.FilePath,
                                            FileServerSize = 0,
                                            FileSize = f.FileSize,
                                            Status = Constants.ModuleDesignDocument_FileIGS_Status_Name
                                        }).ToList();

                fileIGS.AddRange(fileMatNameError);
                resultApiModel.Data = fileIGS.OrderBy(o => o.FilePath);
                resultApiModel.SuccessStatus = true;
            }
            catch
            {
                resultApiModel.SuccessStatus = false;
            }

            return resultApiModel;
        }

        public ResultApiModel GetModuleInfo(string moduleCode)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var moduleInfo = (from a in db.Modules.AsNoTracking()
                              join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                              where a.Code.Equals(moduleCode)
                              select new NTS.Model.WebService.ModuleModel
                              {
                                  Specification = a.Specification,
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ModuleGroupId = a.ModuleGroupId,
                                  ParentGroupId = b.ParentId,
                                  ModuleGroupCode = b.Code,
                                  ParentGroupCode = string.Empty
                              }).FirstOrDefault();

            if (moduleInfo != null)
            {
                var parentGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleInfo.ParentGroupId));

                if (parentGroup != null)
                {
                    moduleInfo.ParentGroupCode = parentGroup.Code;
                }

                resultApiModel.Data = moduleInfo;
                resultApiModel.SuccessStatus = true;
            }
            else
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = "Module không tồn tại!";
            }

            return resultApiModel;
        }

        public ResultApiModel GetMosuleStand(string moduleCode)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            // lấy id module group
            var moduleGroupId = db.Modules.AsNoTracking().Where(a => a.Code.Equals(moduleCode)).Select(a => a.ModuleGroupId).FirstOrDefault();

            // lấy danh sách product stand trong module group
            var listModuleStand = (from a in db.ModuleGroupProductStandards.AsNoTracking()
                                   where a.ModuleGroupId.Equals(moduleGroupId)
                                   join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                   where b.DataType == Constants.Employee_WorkType_CK
                                   orderby b.Name
                                   select new ProductStandModel()
                                   {
                                       Name = b.Name,
                                       Code = b.Code
                                   }).ToList();

            resultApiModel.Data = listModuleStand;
            resultApiModel.SuccessStatus = true;

            return resultApiModel;
        }

        public ResultApiModel GetModuleByModuleId(string moduleId)
        {
            db = new QLTKEntities();
            ResultApiModel resultApiModel = new ResultApiModel();
            var moduleInfo = (from a in db.Modules.AsNoTracking()
                              join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                              where a.Id.Equals(moduleId)
                              select new NTS.Model.WebService.ModuleModel
                              {
                                  Specification = a.Specification,
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ModuleGroupId = a.ModuleGroupId,
                                  ParentGroupId = b.ParentId,
                                  ModuleGroupCode = b.Code,
                                  ParentGroupCode = string.Empty,
                                  CurrentVersion = a.CurrentVersion
                              }).FirstOrDefault();

            if (moduleInfo != null)
            {
                var parentGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleInfo.ParentGroupId));

                if (parentGroup != null)
                {
                    moduleInfo.ParentGroupCode = parentGroup.Code;
                }

                resultApiModel.Data = moduleInfo;
                resultApiModel.SuccessStatus = true;
            }
            else
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = "Module không tồn tại!";
            }

            return resultApiModel;
        }

        public ResultApiModel CheckFile3D(Design3DFileModel model)
        {
            ResultApiModel resultApiModel = new ResultApiModel();

            var filePart = model.Files.Where(r => !r.FileName.StartsWith("TPA")).ToList();

            List<string> fileParts = new List<string>();
            Design3D materialDesign3D;

            var design3Ds = db.Design3D.AsNoTracking().ToList();

            foreach (var item in filePart)
            {
                materialDesign3D = design3Ds.FirstOrDefault(r => r.FileName.ToLower().Equals(item.FileName.ToLower()));

                if (materialDesign3D != null && materialDesign3D.Size != item.Size)
                {
                    fileParts.Add(item.FilePath);
                }
            }

            var fileModule = model.Files.Where(r => r.FileName.StartsWith(model.ModuleCode)).ToList();

            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(model.ModuleCode.ToLower()));
            string moduleId = string.Empty;
            if (module != null)
            {
                moduleId = module.Id;
            }

            var moduleDesignDocumentDB = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                          join m in db.Modules.AsNoTracking() on d.ModuleId equals m.Id
                                          select new
                                          {
                                              m.Code,
                                              d.ModuleId,
                                              d.Name,
                                              d.FileSize,
                                              d.ServerPath,
                                          }).ToList();

            var moduleDesignDocuments = moduleDesignDocumentDB.Select(s => new
            {
                s.Code,
                s.ModuleId,
                s.Name,
                s.FileSize,
                s.ServerPath,
                FileName = Path.GetFileNameWithoutExtension(s.Name)
            }).ToList();

            List<string> fileModules = new List<string>();

            foreach (var item in fileModule)
            {
                var moduleDesignDocument = moduleDesignDocuments.FirstOrDefault(r => r.ModuleId.Equals(moduleId) && r.Name.ToLower().Equals(item.FileName.ToLower()));

                if (moduleDesignDocument != null && moduleDesignDocument.FileSize != item.Size)
                {
                    fileModules.Add(item.FilePath);
                }
            }

            var fileShare = model.Files.Where(f => f.FileName.StartsWith("TPA") && !f.FileName.StartsWith(model.ModuleCode)).ToList();

            var documentModuleByCode = moduleDesignDocuments.Where(d => d.Name.ToLower().StartsWith(d.Code.ToLower()) && !Path.GetFileNameWithoutExtension(d.Name).ToLower().Equals(d.Code.ToLower())).ToList();

            var documents = (from d in documentModuleByCode
                             join f in fileShare on d.Name equals f.FileName
                             select new
                             {
                                 d.ModuleId,
                                 d.Name,
                                 d.FileSize,
                                 ModuleCode = d.Code,
                                 d.ServerPath
                             }).ToList();

            var documentModuleByCodeIpt = moduleDesignDocuments.Where(d => d.FileName.ToLower().Equals(d.Code.ToLower()) && Path.GetExtension(d.Name).ToLower().Equals(".ipt")).ToList();
            var documentModuleByCodeIptAndIam = moduleDesignDocuments.Where(d => d.FileName.ToLower().Equals(d.Code.ToLower()) && (Path.GetExtension(d.Name).ToLower().Equals(".ipt") || Path.GetExtension(d.Name).ToLower().Equals(".iam"))).ToList();

            var documentSource = (from d in documentModuleByCodeIpt
                                  join f in fileShare on d.FileName equals Path.GetFileNameWithoutExtension(f.FileName)
                                  select d).ToList();

            var documentErrorSize = (from f in fileShare
                                     join d in documents on f.FileName equals d.Name
                                     where d.FileSize != f.Size
                                     select f.FilePath).ToList();

            var listDocumentFileSize = ((from f in fileShare
                                         join d in documents on f.FileName equals d.Name
                                         where d.FileSize != f.Size
                                         select new
                                         {
                                             d.ModuleId,
                                             d.Name,
                                             f.FilePath,
                                             d.ServerPath
                                         }).ToList());

            documentErrorSize.AddRange((from f in fileShare
                                        join d in documentSource on Path.GetFileNameWithoutExtension(f.FileName) equals d.FileName
                                        where d.FileSize != f.Size
                                        select f.FilePath).ToList());

            var documentSourceFileSiza = (from d in documentModuleByCodeIptAndIam
                                          join f in fileShare on d.Name equals f.FileName
                                          select d).ToList();
            listDocumentFileSize.AddRange((from f in fileShare
                                           join d in documentSourceFileSiza on f.FileName equals d.Name
                                           where d.FileSize != f.Size
                                           select new
                                           {
                                               d.ModuleId,
                                               d.Name,
                                               f.FilePath,
                                               d.ServerPath
                                           }).ToList());

            var documentErrorexist = (from f in fileShare
                                      join d in documents on f.FileName equals d.Name into fd
                                      from fdn in fd.DefaultIfEmpty()
                                      join ds in documentSource on Path.GetFileNameWithoutExtension(f.FileName) equals ds.FileName into fds
                                      from fdsn in fds.DefaultIfEmpty()
                                      where fdn == null && fdsn == null
                                      select f.FilePath).ToList();

            resultApiModel.Data = new
            {
                FileShareDifferentSize = documentErrorSize,
                FileShareNotExist = documentErrorexist,
                FilePartDifferentSize = fileParts,
                FileModuleDifferentSize = fileModules,
                ListDocumentFileSize = listDocumentFileSize
            };

            resultApiModel.SuccessStatus = true;

            return resultApiModel;
        }

        /// <summary>
        /// Lấy thông tin FTP
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public FTPServer GetFTPServer()
        {
            var fTPServer = db.FTPServers.AsNoTracking().FirstOrDefault();

            if (fTPServer == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FTP);
            }

            return fTPServer;
        }

        #region Vật tư

        public DataCheckDMVTModel GetDataCheckDMVT()
        {
            DataCheckDMVTModel data = new DataCheckDMVTModel();

            data.Materials = (from a in db.Materials.AsNoTracking()
                              join c in db.Manufactures.AsNoTracking() on a.ManufactureId equals c.Id
                              join e in db.Units.AsNoTracking() on a.UnitId equals e.Id
                              select new CheckMatrialModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Code = a.Code,
                                  ManufactureCode = c.Code,
                                  Status = a.Status,
                                  UnitName = e.Name,
                                  Is3DExist = a.Is3DExist
                              }).ToList();

            data.ConverUnits = (from a in db.ConverUnits.AsNoTracking()
                                join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                select new CheckConverUnitModel
                                {
                                    Id = a.Id,
                                    MaterialCode = c.Code
                                }).ToList();

            data.Manufactures = (from a in db.Manufactures.AsNoTracking()
                                 select new CheckManufactureModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     Status = a.Status
                                 }).ToList();

            data.Units = (from a in db.Units.AsNoTracking()
                          select new CheckUnitModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                          }).ToList();

            data.RawMaterials = (from a in db.RawMaterials.AsNoTracking()
                                 select new CheckRawMaterialModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }).ToList();

            return data;
        }
        #endregion

        #region Tài liệu thiết kế module
        /// <summary>
        /// Lấy tất cả tk module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public object GetAllModuleDesignDocument(string moduleId, int designType)
        {
            var list = (from a in db.ModuleDesignDocuments.AsNoTracking()
                        where a.ModuleId.Equals(moduleId) && a.DesignType == designType
                        select new
                        {
                            a.Id,
                            a.ModuleId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.ServerPath,
                            a.FileSize,
                            a.HashValue,
                            a.FileType,
                            a.DesignType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            return list.ToList();
        }

        /// <summary>
        /// Lấy thông tin thư mục download 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<FolderUploadModel> GetFolderDownloadModuleDesignDocument(string moduleId, string folderId)
        {
            var documents = db.ModuleDesignDocuments.AsNoTracking().Where(r => r.ModuleId.Equals(moduleId)).ToList();
            var document = documents.FirstOrDefault(r => r.Id.Equals(folderId));

            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel = new FolderUploadModel()
            {
                LocalPath = document.Path,
                ServerPath = document.ServerPath,
                Name = document.Name,
                Id = document.Id,
                Files = GetFileDownloadModuleDesignDocument(document.Id, documents)
            };

            folders.Add(folderUploadModel);
            folders.AddRange(GetFolderDownloadChild(folderUploadModel.Id, documents));

            return folders;
        }

        /// <summary>
        /// Lấy thông tin thư mục con
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FolderUploadModel> GetFolderDownloadChild(string parentId, List<ModuleDesignDocument> moduleDesignDocuments)
        {
            var documents = moduleDesignDocuments.Where(r => r.ParentId.Equals(parentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder)).ToList();

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel;
            foreach (var item in documents)
            {
                folderUploadModel = new FolderUploadModel()
                {
                    LocalPath = item.Path,
                    ServerPath = item.ServerPath,
                    Name = item.Name,
                    Id = item.Id,
                    Files = GetFileDownloadModuleDesignDocument(item.Id, moduleDesignDocuments)
                };

                folders.Add(folderUploadModel);
                folders.AddRange(GetFolderDownloadChild(folderUploadModel.Id, moduleDesignDocuments));
            }

            return folders;
        }

        /// <summary>
        /// Lấy danh sách file của thư mục
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FileUploadModel> GetFileDownloadModuleDesignDocument(string parentId, List<ModuleDesignDocument> moduleDesignDocuments)
        {
            var documents = (from m in moduleDesignDocuments
                             where m.ParentId.Equals(parentId) && m.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)
                             select new FileUploadModel()
                             {
                                 LocalPath = m.Path,
                                 Name = m.Name,
                                 ServerPath = m.ServerPath
                             }).ToList();

            return documents;
        }
        #endregion

        #region Thiết bị
        public UploadProductModel GetProductById(string productId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var product = (from a in db.Products.AsNoTracking()
                           join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id
                           where a.Id.Equals(productId)
                           select new UploadProductModel
                           {
                               Id = a.Id,
                               Code = a.Code,
                               GroupCode = b.Code,
                               ParentGroupId = b.ParentId,
                               ParentGroupCode = string.Empty
                           }).FirstOrDefault();

            if (product != null)
            {
                var parentGroup = db.ProductGroups.FirstOrDefault(r => r.Id.Equals(product.ParentGroupId));

                if (parentGroup != null)
                {
                    product.ParentGroupCode = parentGroup.Code;
                }
            }

            return product;
        }

        /// <summary>
        /// Lấy tất cả tk thiết bị
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public object GetAllProductDesignDocument(string productId)
        {
            var list = (from a in db.ProductDesignDocuments.AsNoTracking()
                        where a.ProductId.Equals(productId)
                        select new
                        {
                            a.Id,
                            a.ProductId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.ServerPath,
                            a.FileSize,
                            a.FileType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            return list.ToList();
        }

        /// <summary>
        /// Lấy thông tin thư mục download thiết bị
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<FolderUploadModel> GetFolderDownloadProductDesignDocument(string productId, string folderId)
        {
            var document = db.ProductDesignDocuments.AsNoTracking().FirstOrDefault(r => r.Id.Equals(folderId) && r.ProductId.Equals(productId));

            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel = new FolderUploadModel()
            {
                LocalPath = document.Path,
                ServerPath = document.ServerPath,
                Name = document.Name,
                Id = document.Id,
                Files = GetFileDownloadProductDesignDocument(document.Id)
            };

            folders.Add(folderUploadModel);
            folders.AddRange(GetFolderDownloadProudctChild(folderUploadModel.Id));

            return folders;
        }

        /// <summary>
        /// Lấy thông tin thư mục con thiết bị
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FolderUploadModel> GetFolderDownloadProudctChild(string parentId)
        {
            var documents = db.ProductDesignDocuments.AsNoTracking().Where(r => r.ParentId.Equals(parentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder)).ToList();

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel;
            foreach (var item in documents)
            {
                folderUploadModel = new FolderUploadModel()
                {
                    LocalPath = item.Path,
                    ServerPath = item.ServerPath,
                    Name = item.Name,
                    Id = item.Id,
                    Files = GetFileDownloadProductDesignDocument(item.Id)
                };

                folders.Add(folderUploadModel);
                folders.AddRange(GetFolderDownloadProudctChild(folderUploadModel.Id));
            }

            return folders;
        }

        /// <summary>
        /// Lấy danh sách file của thư mục
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FileUploadModel> GetFileDownloadProductDesignDocument(string parentId)
        {
            var documents = (from m in db.ProductDesignDocuments.AsNoTracking()
                             where m.ParentId.Equals(parentId) && m.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)
                             select new FileUploadModel()
                             {
                                 LocalPath = m.Path,
                                 Name = m.Name,
                                 ServerPath = m.ServerPath
                             }).ToList();

            return documents;
        }
        #endregion

        public ResultApiModel getListProductModule(string ModuleId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();

            var data = (from a in db.ProductModules.AsNoTracking()
                        join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                        where a.ModuleId.Equals(ModuleId)
                        orderby b.Code
                        select new ProductModuleModel
                        {
                            Id = b.Id,
                            Code = b.Code,
                            Name = b.Name,
                        }).ToList();

            resultApiModel.Data = data;
            resultApiModel.SuccessStatus = true;
            return resultApiModel;
        }

        //public ResultApiModel getPathServer(string Path)
        //{
        //    ResultApiModel resultApiModel = new ResultApiModel();

        //    var data = (from a in db.pro.AsNoTracking()
        //                join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
        //                where a.ModuleId.Equals(ModuleId)
        //                orderby b.Code
        //                select new ProductModuleModel
        //                {
        //                    Id = b.Id,
        //                    Code = b.Code,
        //                    Name = b.Name,
        //                }).ToList();

        //    resultApiModel.Data = data;
        //    resultApiModel.SuccessStatus = true;
        //    return resultApiModel;
        //}

        #region Phòng học
        public UploadClassRoomModel GetClassRoomById(string classroomId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var classRoom = (from a in db.ClassRooms.AsNoTracking()
                             join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id
                             where a.Id.Equals(classroomId)
                             select new UploadClassRoomModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 GroupCode = b.Code,
                                 //ParentGroupId = b.ParentId,
                                 ParentGroupCode = string.Empty
                             }).FirstOrDefault();

            if (classRoom != null)
            {
                var parentGroup = db.RoomTypes.AsNoTracking().FirstOrDefault(r => r.Id.Equals(classRoom.ParentGroupId));

                if (parentGroup != null)
                {
                    classRoom.ParentGroupCode = parentGroup.Code;
                }
            }

            return classRoom;
        }

        /// <summary>
        /// Lấy tất cả tk thiết bị
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public object GetAllClassRoomDesignDocument(string classRoomId)
        {
            var list = (from a in db.ClassRoomDesignDocuments.AsNoTracking()
                        where a.ClassRoomId.Equals(classRoomId)
                        select new
                        {
                            a.Id,
                            a.ClassRoomId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.ServerPath,
                            a.FileSize,
                            a.FileType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            return list.ToList();
        }

        /// <summary>
        /// Lấy thông tin thư mục download phòng học
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public List<FolderUploadModel> GetFolderDownloadClassRoomDesignDocument(string classRoomId, string folderId)
        {
            var document = db.ClassRoomDesignDocuments.AsNoTracking().FirstOrDefault(r => r.Id.Equals(folderId) && r.ClassRoomId.Equals(classRoomId));

            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel = new FolderUploadModel()
            {
                LocalPath = document.Path,
                ServerPath = document.ServerPath,
                Name = document.Name,
                Id = document.Id,
                Files = GetFileDownloadClassRoomDesignDocument(document.Id)
            };

            folders.Add(folderUploadModel);
            folders.AddRange(GetFolderDownloadProudctChild(folderUploadModel.Id));

            return folders;
        }

        /// <summary>
        /// Lấy thông tin thư mục con thiết bị
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FolderUploadModel> GetFolderDownloadClassRoomChild(string parentId)
        {
            var documents = db.ClassRoomDesignDocuments.AsNoTracking().Where(r => r.ParentId.Equals(parentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder)).ToList();

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel;
            foreach (var item in documents)
            {
                folderUploadModel = new FolderUploadModel()
                {
                    LocalPath = item.Path,
                    ServerPath = item.ServerPath,
                    Name = item.Name,
                    Id = item.Id,
                    Files = GetFileDownloadClassRoomDesignDocument(item.Id)
                };

                folders.Add(folderUploadModel);
                folders.AddRange(GetFolderDownloadClassRoomChild(folderUploadModel.Id));
            }

            return folders;
        }

        /// <summary>
        /// Lấy danh sách file của thư mục
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FileUploadModel> GetFileDownloadClassRoomDesignDocument(string parentId)
        {
            var documents = (from m in db.ClassRoomDesignDocuments.AsNoTracking()
                             where m.ParentId.Equals(parentId) && m.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)
                             select new FileUploadModel()
                             {
                                 LocalPath = m.Path,
                                 Name = m.Name,
                                 ServerPath = m.ServerPath
                             }).ToList();

            return documents;
        }
        #endregion

        #region Giải pháp
        public UploadSolutionModel GetSolutionById(string productId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            var solution = (from a in db.Solutions.AsNoTracking()
                            join b in db.SolutionGroups.AsNoTracking() on a.SolutionGroupId equals b.Id
                            where a.Id.Equals(productId)
                            select new UploadSolutionModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                GroupCode = b.Code,
                                ParentGroupId = b.ParentId,
                                ParentGroupCode = string.Empty
                            }).FirstOrDefault();

            if (solution != null)
            {
                var parentGroup = db.SolutionGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(solution.ParentGroupId));

                if (parentGroup != null)
                {
                    solution.ParentGroupCode = parentGroup.Code;
                }
            }

            return solution;
        }

        /// <summary>
        /// Lấy tất cả tk giải pháp
        /// </summary>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        public object GetAllSolutionDesignDocument(string solutionId)
        {
            var list = (from a in db.SolutionDesignDocuments.AsNoTracking()
                        where a.SolutionId.Equals(solutionId)
                        select new
                        {
                            a.Id,
                            a.SolutionId,
                            a.ParentId,
                            a.Name,
                            a.Path,
                            a.ServerPath,
                            a.FileSize,
                            a.FileType,
                            a.CreateBy,
                            a.CreateDate,
                            a.UpdateBy,
                            a.UpdateDate
                        }).ToList();

            return list.ToList();
        }

        /// <summary>
        /// Lấy thông tin thư mục download giải pháp
        /// </summary>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        public List<FolderUploadModel> GetFolderDownloadSolutionDesignDocument(string solutionId, string folderId)
        {
            var document = db.SolutionDesignDocuments.AsNoTracking().FirstOrDefault(r => r.Id.Equals(folderId) && r.SolutionId.Equals(solutionId));

            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Folder);
            }

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel = new FolderUploadModel()
            {
                LocalPath = document.Path,
                ServerPath = document.ServerPath,
                Name = document.Name,
                Id = document.Id,
                Files = GetFileDownloadSolutionDesignDocument(document.Id)
            };

            folders.Add(folderUploadModel);
            folders.AddRange(GetFolderDownloadSolutionChild(folderUploadModel.Id));

            return folders;
        }

        /// <summary>
        /// Lấy thông tin thư mục con giải pháp
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FolderUploadModel> GetFolderDownloadSolutionChild(string parentId)
        {
            var documents = db.SolutionDesignDocuments.AsNoTracking().Where(r => r.ParentId.Equals(parentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder)).ToList();

            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            FolderUploadModel folderUploadModel;
            foreach (var item in documents)
            {
                folderUploadModel = new FolderUploadModel()
                {
                    LocalPath = item.Path,
                    ServerPath = item.ServerPath,
                    Name = item.Name,
                    Id = item.Id,
                    Files = GetFileDownloadSolutionDesignDocument(item.Id)
                };

                folders.Add(folderUploadModel);
                folders.AddRange(GetFolderDownloadSolutionChild(folderUploadModel.Id));
            }

            return folders;
        }

        /// <summary>
        /// Lấy danh sách file của thư mục
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<FileUploadModel> GetFileDownloadSolutionDesignDocument(string parentId)
        {
            var documents = (from m in db.SolutionDesignDocuments.AsNoTracking()
                             where m.ParentId.Equals(parentId) && m.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)
                             select new FileUploadModel()
                             {
                                 LocalPath = m.Path,
                                 Name = m.Name,
                                 ServerPath = m.ServerPath
                             }).ToList();

            return documents;
        }
        #endregion

        #region Update verions
        /// <summary>
        /// Lấy thông tin version mới
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public NtsUpdateVersionModel GetNewVersion(string version)
        {
            NtsUpdateVersionModel updateVerionInfo = new NtsUpdateVersionModel();
            Version versionCurrent = new Version(version);

            var updateVerion = (from n in db.NTSUpdateVersions.AsNoTracking()
                                orderby n.Version descending
                                select n).FirstOrDefault();

            if (updateVerion != null && versionCurrent.CompareTo(new Version(updateVerion.Version)) < 0)
            {
                updateVerionInfo.IsUpdate = true;
                updateVerionInfo.Path = updateVerion.FilePath;
                updateVerionInfo.VersionNew = updateVerion.Version;
            }

            return updateVerionInfo;
        }
        #endregion
    }
}
