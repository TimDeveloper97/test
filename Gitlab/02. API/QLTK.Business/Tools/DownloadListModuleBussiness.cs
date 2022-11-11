using AutoMapper.Mappers;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DataDistribution;
using NTS.Model.DataDistributionFile;
using NTS.Model.DownloadListModules;
using NTS.Model.DownloadMaterialDesign;
using NTS.Model.DownloadModule;
using NTS.Model.ModuleDesignDocument;
using NTS.Model.ModuleMaterials;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.DownloadListModule
{
    public class DownloadListModuleBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<DownloadModuleSearchResultModel> SearchModule(ModuleSearchModel modelSearch)
        {
            SearchResultModel<DownloadModuleSearchResultModel> searchResult = new SearchResultModel<DownloadModuleSearchResultModel>();

            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ModuleId
                             join c in db.Projects.AsNoTracking() on b.ProjectId equals c.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id) && modelSearch.ProjectId.Equals(b.ProjectId)
                             orderby a.Name
                             select new DownloadModuleSearchResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ProjectCode = c.Code,
                                 ProjectName = c.Name,
                                 UpdateDate = a.UpdateDate,
                                 FileElectric = a.FileElectric,
                                 FileElectronic = a.FileElectronic,
                                 FileMechanics = a.FileMechanics,
                                 ElectricExist = a.ElectricExist,
                                 ElectronicExist = a.ElectronicExist,
                                 MechanicsExist = a.MechanicsExist,
                                 IsEnought = (!a.FileElectric || (a.FileElectric && a.ElectricExist)) && (!a.FileElectronic || (a.FileElectronic && a.ElectronicExist)) && (!a.FileMechanics || (a.FileMechanics && a.MechanicsExist)) && (!a.IsHMI || (a.IsHMI && a.HMIExist)) && (!a.IsPLC || (a.IsPLC && a.PLCExist)) && (!a.IsSoftware || (a.IsSoftware && a.SoftwareExist)) && (!a.IsFilm || (a.IsFilm && a.FilmExist)),

                             }).AsQueryable();

            // tìm kiếm theo mã module
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || r.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public List<DownloadModuleDesignDataModel> GetData(DownloadModuleDesignModel model, string departmentId)
        {
            List<DownloadModuleDesignDataModel> data = new List<DownloadModuleDesignDataModel>();
            var dataDistributions = GetDataDistributions(departmentId);

            Module module;  
            string rootPath = string.Empty;
            string filePath = string.Empty;
            List<string> files;
            DownloadModuleDesignDataModel dataModel;
            List<DownloadModuleDesignInfoModel> modules;
            DownloadModuleDesignInfoModel moduleInfo;
            List<ModuleMaterialResultModel> listMaterial;
            DownloadModuleDesignSubInfoModel moduleSubInfo;
            foreach (var moduleId in model.ModuleIds)
            {
                modules = new List<DownloadModuleDesignInfoModel>();

                module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleId));

                if (module == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                }

                var moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                if (moduleGroup == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                }

                moduleInfo = new DownloadModuleDesignInfoModel
                {
                    ModuleCode = module.Code,
                    ModuleGroupCode = moduleGroup.Code,
                    IsFullDesign = true
                };

                var moduleGroupParent = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                if (moduleGroupParent != null)
                {
                    moduleInfo.ModuleGroupParentCode = moduleGroupParent.Code;
                }

                moduleInfo.Materials = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(moduleId)).
                     Select(s => new ModuleMaterialResultModel
                     {
                         IndexExport = s.Index,
                         ModuleCode = module.Code,
                         MaterialCode = s.MaterialCode,
                         MaterialName = s.MaterialName,
                         Specification = s.Specification,
                         Index = module.Code + "." + s.Index,
                         RawMaterialCode = s.RawMaterialCode,
                         RawMaterial = s.RawMaterial,
                         ManufacturerCode = s.ManufacturerCode,
                         UnitName = s.UnitName,
                         Quantity = s.Quantity
                     }).ToList();

                foreach (var material in moduleInfo.Materials)
                {
                    material.Quantity = GetParentQuantity(moduleInfo.Materials, material.IndexExport);
                }

                moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                    where d.ModuleId.Equals(moduleId)
                                                    select new ModuleDesignDocumentDownloadModel
                                                    {
                                                        Name = d.Name,
                                                        Path = d.Path,
                                                        ServerPath = d.ServerPath,
                                                        FileType = d.FileType
                                                    }).ToList();


                //GetDataMateriaSub(module.Code, moduleInfo.Materials, modules);

                GetDataModuleSub(module.Code, moduleInfo.Materials, moduleInfo);

                modules.Add(moduleInfo);

                rootPath = string.Empty;
                filePath = string.Empty;
                files = new List<string>();
                string folderPath;
                foreach (var distribution in dataDistributions)
                {
                    foreach (var moduleD in modules)
                    {
                        dataModel = new DownloadModuleDesignDataModel();
                        listMaterial = new List<ModuleMaterialResultModel>();
                        folderPath = distribution.Path.Replace("codecha", module.Code).Replace("code", moduleD.ModuleCode).Replace("manhomcha", moduleD.ModuleGroupParentCode).Replace("manhom", moduleD.ModuleGroupCode);

                        if (!folderPath.Contains("mavattu"))
                        {
                            dataModel.Path = folderPath;
                        }

                        foreach (var file in distribution.ListFile)
                        {
                            filePath = file.FolderContain.Replace("codecha", module.Code).Replace("code", moduleD.ModuleCode).Replace("manhomcha", moduleD.ModuleGroupParentCode).Replace("manhom", moduleD.ModuleGroupCode);

                            moduleSubInfo = GetDataDetails(file, filePath, folderPath, moduleD.ModuleDesignDocuments, moduleD.Materials, false);

                            dataModel.Files.AddRange(moduleSubInfo.Files);
                            listMaterial.AddRange(moduleSubInfo.Materials);

                            foreach (var moduleSub in moduleD.ModuleSubs)
                            {
                                filePath = file.FolderContain.Replace("codecha", module.Code).Replace("code", moduleSub.ModuleCode).Replace("manhomcha", moduleSub.ModuleGroupParentCode).Replace("manhom", moduleSub.ModuleGroupCode);
                                if (moduleSub.IsFullDesign)
                                {
                                    moduleSubInfo = GetDataDetails(file, filePath, folderPath, moduleSub.ModuleDesignDocuments, moduleSub.Materials, true);
                                }
                                else
                                {
                                    moduleSubInfo = GetDataDetails(file, filePath, folderPath, moduleD.ModuleDesignDocuments, moduleD.Materials, true);
                                }
                                dataModel.Files.AddRange(moduleSubInfo.Files);
                                listMaterial.AddRange(moduleSubInfo.Materials);
                            }

                            #region Tách hàm
                            //if (file.GetType == Constants.DataDistributionFile_GetType_OneFile)
                            //{
                            //    dataModel.Files.AddRange(moduleD.ModuleDesignDocuments.Where(r => r.Path.ToLower().Equals(filePath.ToLower())).Select(s => new DownloadModuleDesignFileModel
                            //    {
                            //        GoogleDriveId = s.Id,
                            //        Name = s.Name,
                            //        Path = folderPath,
                            //        ServerPath = s.ServerPath
                            //    }).ToList());
                            //}
                            //else
                            //{
                            //    isSearchmaterial = false;
                            //    materials = moduleD.Materials.ToList();

                            //    if (!string.IsNullOrEmpty(file.FilterMaterialCodeStart) && !string.IsNullOrEmpty(file.FilterMaterialCodeStart.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(r => r.MaterialCode.ToUpper().StartsWith(file.FilterMaterialCodeStart.ToUpper())).ToList();
                            //    }

                            //    if (!string.IsNullOrEmpty(file.FilterRawMaterialCode) && !string.IsNullOrEmpty(file.FilterRawMaterialCode.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(r => !string.IsNullOrEmpty(r.RawMaterialCode) && r.RawMaterialCode.ToUpper().Equals(file.FilterRawMaterialCode.ToUpper())).ToList();
                            //    }

                            //    if (!string.IsNullOrEmpty(file.FilterDonVi) && !string.IsNullOrEmpty(file.FilterDonVi.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(r => r.UnitName.ToUpper().Equals(file.FilterDonVi.ToUpper())).ToList();
                            //    }

                            //    if (!string.IsNullOrEmpty(file.FilterRawMaterial) && !string.IsNullOrEmpty(file.FilterRawMaterial.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(r => !string.IsNullOrEmpty(r.RawMaterial) && r.RawMaterial.ToUpper().Equals(file.FilterRawMaterial.ToUpper())).ToList();
                            //    }

                            //    if (!string.IsNullOrEmpty(file.FilterManufacturer) && !string.IsNullOrEmpty(file.FilterManufacturer.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(r => r.ManufacturerCode.ToUpper().Equals(file.FilterManufacturer.ToUpper())).ToList();
                            //    }

                            //    // Mã vật liệu
                            //    if (file.FilterMaVatLieu)
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(m => !string.IsNullOrEmpty(m.RawMaterialCode)).ToList();
                            //    }

                            //    // Có thông số
                            //    if (!string.IsNullOrEmpty(file.FilterThongSo) && !string.IsNullOrEmpty(file.FilterThongSo.Trim()))
                            //    {
                            //        isSearchmaterial = true;
                            //        materials = materials.Where(m => !string.IsNullOrEmpty(m.Specification) && m.Specification.ToLower().Equals(file.FilterThongSo.ToLower())).ToList();
                            //    }

                            //    if (isSearchmaterial)
                            //    {
                            //        foreach (var material in materials)
                            //        {
                            //            dataModel.Files.AddRange(moduleD.ModuleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + material.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                            //            {
                            //                GoogleDriveId = s.Id,
                            //                Name = s.Name,
                            //                Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                            //                ServerPath = s.ServerPath
                            //            }).ToList());
                            //            listMaterial.Add(material);
                            //            if (!string.IsNullOrEmpty(material.Specification) && material.Specification.ToUpper().Equals("HÀN"))
                            //            {
                            //                materialChilds = moduleD.Materials.Where(m => m.Index.StartsWith(material.Index + ".")).ToList();
                            //                foreach (var materialChid in materialChilds)
                            //                {
                            //                    dataModel.Files.AddRange(moduleD.ModuleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + materialChid.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                            //                    {
                            //                        GoogleDriveId = s.Id,
                            //                        Name = s.Name,
                            //                        Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                            //                        ServerPath = s.ServerPath
                            //                    }).ToList());
                            //                }
                            //                listMaterial.AddRange(materialChilds);
                            //            }
                            //        }
                            //    }

                            //    // Mặt
                            //    if (file.MAT || file.TEM || (!isSearchmaterial && !string.IsNullOrEmpty(file.Extension.Trim())))
                            //    {
                            //        dataModel.Files.AddRange(moduleD.ModuleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower() + "\\") && r.Name.ToLower().EndsWith(file.Extension.ToLower())
                            //                        && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).Select(s => new DownloadModuleDesignFileModel
                            //                        {
                            //                            GoogleDriveId = s.Id,
                            //                            Name = s.Name,
                            //                            Path = folderPath,
                            //                            ServerPath = s.ServerPath
                            //                        }).ToList());
                            //    }
                            //}

                            #endregion
                        }

                        if (distribution.IsExportMaterial)
                        {
                            var dataExist = data.FirstOrDefault(r => r.Path.Equals(dataModel.Path));

                            var listgr = (from a in listMaterial
                                          group a by new { a.IndexExport, a.ModuleCode, a.MaterialCode, a.MaterialName, a.RawMaterial, a.Quantity } into g
                                          select new ModuleMaterialResultModel
                                          {
                                              IndexExport = g.Key.IndexExport,
                                              ModuleCode = g.Key.ModuleCode,
                                              MaterialCode = g.Key.MaterialCode,
                                              MaterialName = g.Key.MaterialName,
                                              RawMaterial = g.Key.RawMaterial,
                                              Quantity = g.Key.Quantity
                                          }).ToList();

                            string projectCode = string.Empty;
                            var project = db.Projects.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ProjectId));
                            if (project != null)
                            {
                                projectCode = project.Code;
                            }
                            if (dataExist != null)
                            {
                                dataExist.IsExportMaterial = distribution.IsExportMaterial;
                                dataExist.ListMaterial.AddRange(listgr);
                                dataExist.ProjectCode = projectCode;
                            }
                            else
                            {
                                dataModel.IsExportMaterial = distribution.IsExportMaterial;
                                dataModel.ProjectCode = projectCode;
                                dataModel.ListMaterial.AddRange(listgr);
                            }
                        }

                        dataModel.Files = dataModel.Files.GroupBy(r => new { r.Name, r.Path, r.ServerPath }).Select(s => new DownloadModuleDesignFileModel
                        {
                            ServerPath = s.Key.ServerPath,
                            Path = s.Key.Path,
                            Name = s.Key.Name
                        }).ToList();

                        data.Add(dataModel);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="folderPath"></param>
        /// <param name="moduleDesignDocuments"></param>
        /// <param name="materials"></param>
        /// <param name="isSub"></param>
        /// <returns></returns>
        private DownloadModuleDesignSubInfoModel GetDataDetails(DataDistributionFileModel file, string filePath, string folderPath,
            List<ModuleDesignDocumentDownloadModel> moduleDesignDocuments, List<ModuleMaterialResultModel> materials, bool isSub)
        {
            DownloadModuleDesignSubInfoModel result = new DownloadModuleDesignSubInfoModel();

            List<DownloadModuleDesignFileModel> files;
            List<ModuleMaterialResultModel> materialChilds;
            bool isSearchmaterial = false;
            List<ModuleMaterialResultModel> materialSearch;
            if (file.GetType == Constants.DataDistributionFile_GetType_OneFile)
            {
                result.Files.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().Equals(filePath.ToLower())).Select(s => new DownloadModuleDesignFileModel
                {
                    Name = s.Name,
                    Path = folderPath,
                    ServerPath = s.ServerPath
                }).ToList());
            }
            else
            {
                isSearchmaterial = false;
                materialSearch = materials.ToList();

                if (!string.IsNullOrEmpty(file.FilterMaterialCodeStart) && !string.IsNullOrEmpty(file.FilterMaterialCodeStart.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(r => r.MaterialCode.ToUpper().StartsWith(file.FilterMaterialCodeStart.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterRawMaterialCode) && !string.IsNullOrEmpty(file.FilterRawMaterialCode.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(r => !string.IsNullOrEmpty(r.RawMaterialCode) && r.RawMaterialCode.ToUpper().Equals(file.FilterRawMaterialCode.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterDonVi) && !string.IsNullOrEmpty(file.FilterDonVi.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(r => r.UnitName.ToUpper().Equals(file.FilterDonVi.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterRawMaterial) && !string.IsNullOrEmpty(file.FilterRawMaterial.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(r => !string.IsNullOrEmpty(r.RawMaterial) && r.RawMaterial.ToUpper().Equals(file.FilterRawMaterial.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterManufacturer) && !string.IsNullOrEmpty(file.FilterManufacturer.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(r => r.ManufacturerCode.ToUpper().Equals(file.FilterManufacturer.ToUpper())).ToList();
                }

                // Mã vật liệu
                if (file.FilterMaVatLieu)
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(m => !string.IsNullOrEmpty(m.RawMaterialCode)).ToList();
                }

                // Có thông số
                if (!string.IsNullOrEmpty(file.FilterThongSo) && !string.IsNullOrEmpty(file.FilterThongSo.Trim()))
                {
                    isSearchmaterial = true;
                    materialSearch = materialSearch.Where(m => !string.IsNullOrEmpty(m.Specification) && m.Specification.ToLower().Equals(file.FilterThongSo.ToLower())).ToList();
                }

                if (isSearchmaterial)
                {
                    foreach (var material in materialSearch)
                    {
                        // 20201011
                        //files = moduleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + material.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                        //{
                        //    GoogleDriveId = s.Id,
                        //    Name = s.Name,
                        //    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                        //    ServerPath = s.ServerPath
                        //}).ToList();

                        files = moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((material.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                        {
                            Name = s.Name,
                            Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                            ServerPath = s.ServerPath
                        }).ToList();

                        result.Files.AddRange(files);

                        if (!isSub || files.Count > 0)
                        {
                            result.Materials.Add(material);
                        }

                        if (!string.IsNullOrEmpty(material.Specification) && material.Specification.ToUpper().Equals("HÀN"))
                        {
                            materialChilds = materials.Where(m => m.Index.StartsWith(material.Index + ".")).ToList();
                            foreach (var materialChid in materialChilds)
                            {
                                // 20201011
                                //files = moduleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + materialChid.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                                //{
                                //    GoogleDriveId = s.Id,
                                //    Name = s.Name,
                                //    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                //    ServerPath = s.ServerPath
                                //}).ToList();

                                files = moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((materialChid.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                                {
                                    Name = s.Name,
                                    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                    ServerPath = s.ServerPath
                                }).ToList();

                                result.Files.AddRange(files);

                                if (!isSub || files.Count > 0)
                                {
                                    result.Materials.Add(materialChid);
                                }
                            }
                        }
                    }
                }

                // Mặt
                if (file.MAT || file.TEM || (!isSearchmaterial && !string.IsNullOrEmpty(file.Extension.Trim())))
                {
                    result.Files.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower() + "\\") && r.Name.ToLower().EndsWith(file.Extension.ToLower())
                                    && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).Select(s => new DownloadModuleDesignFileModel
                                    {
                                        Name = s.Name,
                                        Path = folderPath,
                                        ServerPath = s.ServerPath
                                    }).ToList());
                }
            }

            return result;
        }

        private decimal GetParentQuantity(List<ModuleMaterialResultModel> materials, string indexChild)
        {
            string indexParent = Util.GetIndexParent(indexChild);

            if (string.IsNullOrEmpty(indexParent))
            {
                return 1;
            }

            var parent = materials.FirstOrDefault(r => r.Index.Equals(indexParent));

            if (parent != null)
            {
                return parent.Quantity * GetParentQuantity(materials, parent.Index);
            }

            return 1;
        }

        #region Download theo vật tư

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public List<DownloadModuleDesignDataModel> GetDataMaterial(DownloadMarterialDesignModel model, string departmentId)
        {
            List<DownloadModuleDesignDataModel> data = new List<DownloadModuleDesignDataModel>();
            var dataDistributions = GetDataDistributions(departmentId);

            Module module;
            List<DownloadMarterialDetailsModel> materialCodes;
            List<ModuleMaterialResultModel> materialChilds;
            string rootPath = string.Empty;
            string filePath = string.Empty;
            DownloadModuleDesignDataModel dataModel;
            List<DownloadModuleDesignInfoModel> modules;
            DownloadModuleDesignInfoModel moduleInfo;
            string folderPath, filePathSub;
            bool isSearchmaterial = false;
            ModuleMaterialResultModel materialHan;
            foreach (var moduleId in model.ListMaterialModule)
            {
                List<ModuleMaterialResultModel> listMaterial = new List<ModuleMaterialResultModel>();

                modules = new List<DownloadModuleDesignInfoModel>();
                module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.Equals(moduleId.ModuleCode));
                if (module == null)
                {
                    continue;
                    //throw NTSException.CreateInstance(MessageResourceKey.MSG0073, TextResourceKey.Module, moduleId.ModuleCode);
                }

                var moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                if (moduleGroup == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                }

                moduleInfo = new DownloadModuleDesignInfoModel
                {
                    ModuleCode = module.Code,
                    ModuleGroupCode = moduleGroup.Code,
                    IsFullDesign = true
                };

                var moduleGroupParent = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                if (moduleGroupParent != null)
                {
                    moduleInfo.ModuleGroupParentCode = moduleGroupParent.Code;
                }

                moduleInfo.Materials = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(module.Id)).
                     Select(s => new ModuleMaterialResultModel
                     {
                         MaterialCode = s.MaterialCode,
                         MaterialName = s.MaterialName,
                         Specification = s.Specification,
                         Index = module.Code + "." + s.Index,
                         RawMaterialCode = s.RawMaterialCode,
                         RawMaterial = s.RawMaterial,
                         ManufacturerCode = s.ManufacturerCode,
                         UnitName = s.UnitName
                     }).ToList();

                moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                    where d.ModuleId.Equals(module.Id)
                                                    select new ModuleDesignDocumentDownloadModel
                                                    {
                                                        Name = d.Name,
                                                        Path = d.Path,
                                                        ServerPath = d.ServerPath,
                                                        FileType = d.FileType
                                                    }).ToList();

                GetDataModuleSub(module.Code, moduleInfo.Materials, moduleInfo);

                rootPath = string.Empty;
                filePath = string.Empty;
                filePathSub = string.Empty;
                foreach (var distribution in dataDistributions)
                {
                    dataModel = new DownloadModuleDesignDataModel();
                    folderPath = distribution.Path.Replace("codecha", module.Code).Replace("code", moduleInfo.ModuleCode).Replace("manhomcha", moduleInfo.ModuleGroupParentCode).Replace("manhom", moduleInfo.ModuleGroupCode);

                    if (!folderPath.Contains("mavattu"))
                    {
                        dataModel.Path = folderPath;
                    }

                    foreach (var file in distribution.ListFile)
                    {
                        filePath = file.FolderContain.Replace("codecha", module.Code).Replace("code", moduleInfo.ModuleCode).Replace("manhomcha", moduleInfo.ModuleGroupParentCode).Replace("manhom", moduleInfo.ModuleGroupCode);

                        if (file.GetType == Constants.DataDistributionFile_GetType_OneFile)
                        {
                            dataModel.Files.AddRange(moduleInfo.ModuleDesignDocuments.Where(r => r.Path.ToLower().Equals(filePath.ToLower())).Select(s => new DownloadModuleDesignFileModel
                            {
                                Name = s.Name,
                                Path = folderPath,
                                ServerPath = s.ServerPath
                            }).ToList());
                        }
                        else
                        {

                            isSearchmaterial = false;
                            materialCodes = (from m in moduleId.ListMaterials
                                             select m).ToList();

                            if (!string.IsNullOrEmpty(file.FilterMaterialCodeStart) && !string.IsNullOrEmpty(file.FilterMaterialCodeStart.Trim()))
                            {
                                isSearchmaterial = true;
                                materialCodes = materialCodes.Where(r => r.MaterialCode.ToUpper().StartsWith(file.FilterMaterialCodeStart.ToUpper())).ToList();
                            }

                            // Có thông số
                            if (!string.IsNullOrEmpty(file.FilterThongSo) && !string.IsNullOrEmpty(file.FilterThongSo.Trim()))
                            {
                                isSearchmaterial = true;
                                materialCodes = materialCodes.Where(m => !string.IsNullOrEmpty(m.Specification) && m.Specification.ToLower().Equals(file.FilterThongSo.ToLower())).ToList();
                            }

                            if (isSearchmaterial)
                            {
                                foreach (var material in materialCodes)
                                {
                                    dataModel.Files.AddRange(moduleInfo.ModuleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((material.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                                    {
                                        Name = s.Name,
                                        Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                        ServerPath = s.ServerPath
                                    }).ToList());

                                    if (!string.IsNullOrEmpty(material.Specification) && material.Specification.ToUpper().Equals("HÀN"))
                                    {
                                        materialHan = moduleInfo.Materials.FirstOrDefault(r => r.MaterialCode.ToUpper().Equals(material.MaterialCode));

                                        if (materialHan != null)
                                        {
                                            materialChilds = moduleInfo.Materials.Where(m => m.Index.StartsWith(materialHan.Index + ".")).ToList();
                                            foreach (var materialChid in materialChilds)
                                            {
                                                dataModel.Files.AddRange(moduleInfo.ModuleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((materialChid.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                                                {
                                                    Name = s.Name,
                                                    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                                    ServerPath = s.ServerPath
                                                }).ToList());

                                                foreach (var moduleSub in moduleInfo.ModuleSubs)
                                                {
                                                    filePathSub = file.FolderContain.Replace("codecha", moduleSub.ModuleCode).Replace("code", moduleSub.ModuleCode).Replace("manhomcha", moduleSub.ModuleGroupParentCode).Replace("manhom", moduleSub.ModuleGroupCode);

                                                    dataModel.Files.AddRange(moduleSub.ModuleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePathSub.ToLower()) && r.Path.ToLower().EndsWith((materialChid.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePathSub.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                                                    {
                                                        Name = s.Name,
                                                        Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                                        ServerPath = s.ServerPath
                                                    }).ToList());
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Mặt
                            if (file.MAT || file.TEM || (!isSearchmaterial && !string.IsNullOrEmpty(file.Extension.Trim())))
                            {
                                foreach (var item in moduleId.ListMaterials)
                                {
                                    dataModel.Files.AddRange(moduleInfo.ModuleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower() + "\\") && r.Name.ToLower().StartsWith(item.MaterialCode.ToLower()) && r.Name.ToLower().EndsWith(file.Extension.ToLower())
                                                    && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).Select(s => new DownloadModuleDesignFileModel
                                                    {
                                                        Name = s.Name,
                                                        Path = folderPath,
                                                        ServerPath = s.ServerPath
                                                    }).ToList());
                                }
                            }
                        }
                    }

                    data.Add(dataModel);

                }
            }
            return data;
        }

        private List<DownloadModuleDesignFileModel> GetDataMaterialDetails(DataDistributionFileModel file, string filePath, string folderPath,
           List<ModuleDesignDocument> moduleDesignDocuments, List<ModuleMaterialResultModel> materials, List<string> materialImports, bool isSub)
        {
            bool isSearchmaterial = false;
            List<ModuleMaterialResultModel> materialChilds;
            List<ModuleMaterialResultModel> materialResults;
            List<DownloadModuleDesignFileModel> result = new List<DownloadModuleDesignFileModel>();

            if (file.GetType == Constants.DataDistributionFile_GetType_OneFile)
            {
                result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().Equals(filePath.ToLower())).Select(s => new DownloadModuleDesignFileModel
                {
                    GoogleDriveId = s.Id,
                    Name = s.Name,
                    Path = folderPath,
                    ServerPath = s.ServerPath
                }).ToList());
            }
            else
            {
                isSearchmaterial = false;
                materialResults = (from m in materials
                                   join d in materialImports on m.MaterialCode equals d
                                   select m).ToList();

                if (!string.IsNullOrEmpty(file.FilterMaterialCodeStart) && !string.IsNullOrEmpty(file.FilterMaterialCodeStart.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(r => r.MaterialCode.ToUpper().StartsWith(file.FilterMaterialCodeStart.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterRawMaterialCode) && !string.IsNullOrEmpty(file.FilterRawMaterialCode.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(r => !string.IsNullOrEmpty(r.RawMaterialCode) && r.RawMaterialCode.ToUpper().Equals(file.FilterRawMaterialCode.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterDonVi) && !string.IsNullOrEmpty(file.FilterDonVi.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(r => r.UnitName.ToUpper().Equals(file.FilterDonVi.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterRawMaterial) && !string.IsNullOrEmpty(file.FilterRawMaterial.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(r => !string.IsNullOrEmpty(r.RawMaterial) && r.RawMaterial.ToUpper().Equals(file.FilterRawMaterial.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(file.FilterManufacturer) && !string.IsNullOrEmpty(file.FilterManufacturer.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(r => r.ManufacturerCode.ToUpper().Equals(file.FilterManufacturer.ToUpper())).ToList();
                }

                // Mã vật liệu
                if (file.FilterMaVatLieu)
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(m => !string.IsNullOrEmpty(m.RawMaterialCode)).ToList();
                }

                // Có thông số
                if (!string.IsNullOrEmpty(file.FilterThongSo) && !string.IsNullOrEmpty(file.FilterThongSo.Trim()))
                {
                    isSearchmaterial = true;
                    materialResults = materialResults.Where(m => !string.IsNullOrEmpty(m.Specification) && m.Specification.ToLower().Equals(file.FilterThongSo.ToLower())).ToList();
                }

                if (isSearchmaterial)
                {
                    foreach (var material in materialResults)
                    {
                        //20201110
                        //result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + material.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                        //{
                        //    GoogleDriveId = s.Id,
                        //    Name = s.Name,
                        //    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                        //    ServerPath = s.ServerPath
                        //}).ToList());

                        result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((material.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                        {
                            GoogleDriveId = s.Id,
                            Name = s.Name,
                            Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                            ServerPath = s.ServerPath
                        }).ToList());

                        if (!string.IsNullOrEmpty(material.Specification) && material.Specification.ToUpper().Equals("HÀN"))
                        {
                            materialChilds = materials.Where(m => m.Index.StartsWith(material.Index + ".")).ToList();
                            foreach (var materialChid in materialChilds)
                            {
                                //20201110
                                //result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().Equals((filePath + "\\" + materialChid.MaterialCode + file.Extension).ToLower())).Select(s => new DownloadModuleDesignFileModel
                                //{
                                //    GoogleDriveId = s.Id,
                                //    Name = s.Name,
                                //    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                //    ServerPath = s.ServerPath
                                //}).ToList());

                                result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower()) && r.Path.ToLower().EndsWith((materialChid.MaterialCode + file.Extension).ToLower()) && !r.Path.ToLower().StartsWith(filePath.ToLower() + "\\com")).Select(s => new DownloadModuleDesignFileModel
                                {
                                    GoogleDriveId = s.Id,
                                    Name = s.Name,
                                    Path = folderPath.Replace("mavattu", material.MaterialCode.Replace('/', ')')),
                                    ServerPath = s.ServerPath
                                }).ToList());
                            }
                        }
                    }
                }

                // Mặt
                if (file.MAT || file.TEM || (!isSearchmaterial && !string.IsNullOrEmpty(file.Extension.Trim())))
                {
                    foreach (var item in materialImports)
                    {
                        result.AddRange(moduleDesignDocuments.Where(r => r.Path.ToLower().StartsWith(filePath.ToLower() + "\\") && r.Name.ToLower().StartsWith(item.ToLower()) && r.Name.ToLower().EndsWith(file.Extension.ToLower())
                                        && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).Select(s => new DownloadModuleDesignFileModel
                                        {
                                            GoogleDriveId = s.Id,
                                            Name = s.Name,
                                            Path = folderPath,
                                            ServerPath = s.ServerPath
                                        }).ToList());
                    }
                }

            }

            return result;
        }

        #endregion

        private void GetDataMateriaSub(string moduleCode, List<ModuleMaterialResultModel> moduleMaterials, List<DownloadModuleDesignInfoModel> modules)
        {
            var materialPCB = moduleMaterials.Where(r => !r.MaterialCode.ToUpper().StartsWith(moduleCode.ToUpper()) && (r.MaterialCode.ToUpper().StartsWith("PCB") || r.MaterialCode.ToUpper().StartsWith("TPA"))).ToList();

            Module module;
            ModuleGroup moduleGroup;
            DownloadModuleDesignInfoModel moduleInfo;
            string moduleId;
            foreach (var material in materialPCB)
            {
                moduleInfo = new DownloadModuleDesignInfoModel();

                module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(material.MaterialCode.ToUpper()));

                if (module != null)
                {
                    if (modules.Count(r => r.ModuleCode.Equals(module.Code) && r.IsFullDesign) > 0)
                    {
                        continue;
                    }

                    moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                        where d.ModuleId.Equals(module.Id)
                                                        select new ModuleDesignDocumentDownloadModel
                                                        {
                                                            Name = d.Name,
                                                            Path = d.Path,
                                                            ServerPath = d.ServerPath,
                                                            FileType = d.FileType
                                                        }).ToList();

                    moduleInfo.Materials = (from s in db.ModuleMaterials.AsNoTracking()
                                            where s.ModuleId.Equals(module.Id)
                                            select new ModuleMaterialResultModel
                                            {
                                                MaterialCode = s.MaterialCode,
                                                MaterialName = s.MaterialName,
                                                Specification = s.Specification,
                                                Index = module.Code + "." + s.Index,
                                                RawMaterialCode = s.RawMaterialCode,
                                                RawMaterial = s.RawMaterial,
                                                ManufacturerCode = s.ManufacturerCode,
                                                UnitName = s.UnitName
                                            }).ToList();

                    moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                    if (moduleGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                    }

                    moduleInfo.ModuleCode = module.Code;
                    moduleInfo.ModuleGroupCode = moduleGroup.Code;
                    moduleInfo.IsFullDesign = true;

                    moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                    if (moduleGroup != null)
                    {
                        moduleInfo.ModuleGroupParentCode = moduleGroup.Code;
                    }

                    modules.Add(moduleInfo);

                    GetDataMateriaSub(module.Code, moduleInfo.Materials, modules);
                }
                else
                {
                    moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                        join m in db.Modules.AsNoTracking() on d.ModuleId equals m.Id
                                                        where material.MaterialCode.ToUpper().StartsWith(m.Code.ToUpper()) && d.Name.ToUpper().StartsWith(material.MaterialCode.ToUpper())
                                                        select new ModuleDesignDocumentDownloadModel
                                                        {
                                                            Name = d.Name,
                                                            Path = d.Path,
                                                            ServerPath = d.ServerPath,
                                                            FileType = d.FileType,
                                                            ModuleId = d.ModuleId
                                                        }).ToList();

                    if (moduleInfo.ModuleDesignDocuments.Count > 0)
                    {

                        moduleId = moduleInfo.ModuleDesignDocuments.FirstOrDefault().ModuleId;
                        module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.ToUpper().Equals(moduleId));

                        moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                        if (moduleGroup == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                        }

                        moduleInfo.ModuleCode = module.Code;
                        moduleInfo.ModuleGroupCode = moduleGroup.Code;
                        moduleInfo.IsFullDesign = false;

                        moduleInfo.Materials = (from s in db.ModuleMaterials.AsNoTracking()
                                                where s.ModuleId.Equals(module.Id)
                                                select new ModuleMaterialResultModel
                                                {
                                                    MaterialCode = s.MaterialCode,
                                                    MaterialName = s.MaterialName,
                                                    Specification = s.Specification,
                                                    Index = module.Code + "." + s.Index,
                                                    RawMaterialCode = s.RawMaterialCode,
                                                    RawMaterial = s.RawMaterial,
                                                    ManufacturerCode = s.ManufacturerCode,
                                                    UnitName = s.UnitName
                                                }).ToList();

                        moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                        if (moduleGroup != null)
                        {
                            moduleInfo.ModuleGroupParentCode = moduleGroup.Code;
                        }

                        modules.Add(moduleInfo);
                    }
                }
            }
        }

        private void GetDataModuleSub(string moduleCode, List<ModuleMaterialResultModel> moduleMaterials, DownloadModuleDesignInfoModel moduleParent)
        {
            var materialPCB = moduleMaterials.Where(r => !r.MaterialCode.ToUpper().StartsWith(moduleCode.ToUpper()) && (r.MaterialCode.ToUpper().StartsWith("PCB") || r.MaterialCode.ToUpper().StartsWith("TPA")))
                .GroupBy(r => r.MaterialCode).Select(s => s.Key).ToList();

            Module module;
            ModuleGroup moduleGroup;
            DownloadModuleDesignInfoModel moduleInfo;
            DownloadModuleDesignInfoModel moduleInfoExist;
            foreach (var materialCode in materialPCB)
            {
                moduleInfo = new DownloadModuleDesignInfoModel();

                module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(materialCode.ToUpper()));

                if (module != null)
                {
                    if (moduleParent.ModuleSubs.Count(r => r.ModuleCode.Equals(module.Code) && r.IsFullDesign) > 0)
                    {
                        continue;
                    }

                    moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                        where d.ModuleId.Equals(module.Id)
                                                        select new ModuleDesignDocumentDownloadModel
                                                        {
                                                            Name = d.Name,
                                                            Path = d.Path,
                                                            ServerPath = d.ServerPath,
                                                            FileType = d.FileType
                                                        }).ToList();

                    moduleInfo.Materials = (from s in db.ModuleMaterials.AsNoTracking()
                                            where s.ModuleId.Equals(module.Id)
                                            select new ModuleMaterialResultModel
                                            {
                                                MaterialCode = s.MaterialCode,
                                                MaterialName = s.MaterialName,
                                                Specification = s.Specification,
                                                Index = module.Code + "." + s.Index,
                                                RawMaterialCode = s.RawMaterialCode,
                                                RawMaterial = s.RawMaterial,
                                                ManufacturerCode = s.ManufacturerCode,
                                                UnitName = s.UnitName
                                            }).ToList();

                    moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                    if (moduleGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                    }

                    moduleInfo.ModuleCode = module.Code;
                    moduleInfo.ModuleGroupCode = moduleGroup.Code;
                    moduleInfo.IsFullDesign = true;

                    moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                    if (moduleGroup != null)
                    {
                        moduleInfo.ModuleGroupParentCode = moduleGroup.Code;
                    }

                    moduleParent.ModuleSubs.Add(moduleInfo);

                    GetDataModuleSub(module.Code, moduleInfo.Materials, moduleParent);
                }
                else
                {
                     module = db.Modules.AsNoTracking().FirstOrDefault(r => materialCode.ToUpper().StartsWith(r.Code.ToUpper()));

                    if (module == null)
                    {
                        continue;
                    }

                    if (moduleParent.ModuleSubs.Count(r => r.ModuleCode.Equals(module.Code) && r.IsFullDesign) > 0)
                    {
                        continue;
                    }

                    moduleInfo.ModuleDesignDocuments = (from d in db.ModuleDesignDocuments.AsNoTracking()
                                                        where d.ModuleId.Equals(module.Id) && d.Name.ToUpper().StartsWith(materialCode.ToUpper())
                                                        select new ModuleDesignDocumentDownloadModel
                                                        {
                                                            Name = d.Name,
                                                            Path = d.Path,
                                                            ServerPath = d.ServerPath,
                                                            FileType = d.FileType,
                                                            ModuleId = d.ModuleId
                                                        }).ToList();

                    if (moduleInfo.ModuleDesignDocuments.Count > 0)
                    {
                        moduleParent.ModuleDesignDocuments.AddRange(moduleInfo.ModuleDesignDocuments);
                        //moduleId = moduleInfo.ModuleDesignDocuments.FirstOrDefault().ModuleId;
                        //module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.ToUpper().Equals(moduleId));

                        //moduleInfoExist = moduleParent.ModuleSubs.FirstOrDefault(r => r.ModuleCode.Equals(module.Code) && !r.IsFullDesign);

                        moduleInfoExist = moduleParent.ModuleSubs.FirstOrDefault(r => r.ModuleCode.Equals(module.Code));
                        if (moduleInfoExist == null)
                        {
                            moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(module.ModuleGroupId));

                            if (moduleGroup == null)
                            {
                                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ModuleGroup);
                            }

                            moduleInfo.ModuleCode = module.Code;
                            moduleInfo.ModuleGroupCode = moduleGroup.Code;
                            moduleInfo.IsFullDesign = false;

                            moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleGroup.ParentId));

                            if (moduleGroup != null)
                            {
                                moduleInfo.ModuleGroupParentCode = moduleGroup.Code;
                            }

                            moduleParent.ModuleSubs.Add(moduleInfo);
                        }
                        else if (!moduleInfoExist.IsFullDesign)
                        {
                            moduleInfoExist.ModuleDesignDocuments.AddRange(moduleInfo.ModuleDesignDocuments);
                        }
                    }
                }
            }
        }

        public ImportListModuleModel ImportExcelListModel(DownloadModuleSearchModel model, HttpPostedFile file)
        {
            ImportListModuleModel rs = new ImportListModuleModel();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            var moduleCode = string.Empty;
            var modules = db.Modules.AsNoTracking();
            try
            {
                for (int i = 2; i <= rowCount; i++)
                {
                    moduleCode = sheet[i, 2].Value;
                    var module = modules.FirstOrDefault(t => moduleCode.Equals(t.Code));
                    if (module != null)
                    {
                        if (!model.ListIdSelect.Contains(module.Id))
                        {
                            model.ListIdSelect.Add(module.Id);
                        }

                    }
                }
                var dataQuery = (from a in db.Modules.AsNoTracking()
                                 join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ModuleId
                                 join c in db.Projects.AsNoTracking() on b.ProjectId equals c.Id
                                 where model.ProjectId.Equals(b.ProjectId)
                                 orderby a.Name
                                 select new DownloadModuleSearchResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ProjectCode = c.Code,
                                     ProjectName = c.Name,
                                     UpdateDate = a.UpdateDate,
                                     IsEnought = (!a.FileElectric || (a.FileElectric && a.ElectricExist)) && (!a.FileElectronic || (a.FileElectronic && a.ElectronicExist)) && (!a.FileMechanics || (a.FileMechanics && a.MechanicsExist)) && (!a.IsHMI || (a.IsHMI && a.HMIExist)) && (!a.IsPLC || (a.IsPLC && a.PLCExist)) && (!a.IsSoftware || (a.IsSoftware && a.SoftwareExist)) && (!a.IsFilm || (a.IsFilm && a.FilmExist)),
                                 }).AsQueryable();

                // tìm kiếm theo mã module
                if (!string.IsNullOrEmpty(model.Code))
                {
                    dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()) || r.Name.ToUpper().Contains(model.Code.ToUpper()));
                }

                var listNoSelect = dataQuery.Where(t => !model.ListIdSelect.Contains(t.Id));
                var listSelect = dataQuery.Where(t => model.ListIdSelect.Contains(t.Id));

                rs.TotalSelect = listSelect.Count();
                rs.TotalNoSelect = listNoSelect.Count();
                var listResult = SQLHelpper.OrderBy(listNoSelect, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();

                rs.ListSelect = listSelect.ToList();
                rs.ListNoSelect = listResult;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
            return rs;
        }

        private List<DataDistributionModel> GetDataDistributions(string departmentId)
        {
            var data = (from a in db.DataDistributions.AsNoTracking()
                        where a.DepartmentId.Equals(departmentId)
                        select new DataDistributionModel
                        {
                            Id = a.Id,
                            DepartmentId = a.DepartmentId,
                            Name = a.Name,
                            ParentId = a.ParentId,
                            Description = a.Description,
                            Type = a.Type,
                            Path = a.Path,
                            IsExportMaterial = a.IsExportMaterial,
                        }).ToList();

            foreach (var item in data)
            {
                item.ListFile = (from a in db.DataDistributionFileLinks.AsNoTracking()
                                 join b in db.DataDistributionFiles.AsNoTracking() on a.DataDistributionFileId equals b.Id
                                 where a.DataDistributionId.Equals(item.Id)
                                 select new DataDistributionFileModel
                                 {
                                     Id = b.Id,
                                     Name = b.Name,
                                     FolderContain = b.FolderContain,
                                     GetType = b.GetType,
                                     FilterThongSo = b.FilterThongSo,
                                     FilterMaVatLieu = b.FilterMaVatLieu,
                                     FilterDonVi = b.FilterDonVi,
                                     Extension = b.Extension,
                                     FilterManufacturer = b.FilterManufacturer,
                                     FilterMaterialCodeStart = b.FilterMaterialCodeStart,
                                     FilterRawMaterial = b.FilterRawMaterial,
                                     FilterRawMaterialCode = b.FilterRawMaterialCode
                                 }).ToList();
            }

            return data;
        }
    }
}
