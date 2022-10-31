using AutoMapper.Mappers;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DMVTImportSAP;
using NTS.Model.Employees;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterialFinishDesign;
using NTS.Model.ModuleMaterials;
using NTS.Model.NTSDepartment;
using NTS.Model.Plans;
using NTS.Model.ProductAccessories;
using NTS.Model.ProjectGeneralDesign;
using NTS.Model.ProjectGeneralDesignMaterials;
using NTS.Model.ProjectGeneralDesignModule;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using QLTK.Business.DMVTimportSAP;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.ProjectProducts;
using QLTK.Business.Projects;
using QLTK.Business.QLTKMODULE;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace QLTK.Business.ProjectGeneralDesigns
{
    public class ProjectGeneralDesignBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ProjectProductBusiness business = new ProjectProductBusiness();
        private readonly DMVTImportSAPBussiness _business = new DMVTImportSAPBussiness();
        private readonly ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();

        public SearchResultModel<ProjectProductsModel> SearchProjectProductExport(ProjectProductsSearchModel modelSearch)
        {
            SearchResultModel<ProjectProductsModel> searchResult = new SearchResultModel<ProjectProductsModel>();

            ProjectProductsSearchModel model = new ProjectProductsSearchModel();
            model.ProjectId = modelSearch.ProjectId;
            var data = business.SearchProjectProduct(model);

            //var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
            //                          where a.ProjectId.Equals(modelSearch.ProjectId)
            //                          orderby a.ContractIndex
            //                          select new ProjectProductsModel
            //                          {
            //                              Id = a.Id,
            //                              ProjectId = a.ProjectId,
            //                              ParentId = a.ParentId,
            //                              ModuleId = a.ModuleId,
            //                              ProductId = a.ProductId,
            //                              ContractCode = a.ContractCode,
            //                              ContractName = a.ContractName,
            //                              DataType = a.DataType,
            //                              ContractIndex = a.ContractIndex
            //                          }).ToList();
            var listProjectProduct = data.ListResult;
            var listProjectGeneralDesign = db.ProjectGeneralDesignModules.AsNoTracking().ToList();
            foreach (var item in listProjectProduct)
            {
                var projectGeneralDesigns = db.ProjectGeneralDesigns.AsNoTracking().Where(i => item.Id.Equals(i.ProjectProductId)).ToList();
                var projectGeneralDesignModule = (from a in db.ProjectGeneralDesignModules.AsNoTracking()
                                                  join b in db.ProjectGeneralDesigns.AsNoTracking() on a.ProjectGeneralDesignId equals b.Id
                                                  where a.ProjectProductId.Equals(item.Id)
                                                  select new ProjectGeneralDesignModuleModel
                                                  {
                                                      CreateIndex = b.CreateIndex
                                                  }).FirstOrDefault();
                if (projectGeneralDesigns.Count > 0)
                {
                    item.THTK = item.ContractCode + ".THTK";
                    item.CreateIndex = projectGeneralDesigns.Max(i => i.CreateIndex);
                    int index = 1;
                    modelSearch.DataType = Constants.ProjectProduct_DataType_Module;
                    var listchild = GetProjectProductChild(item.Id, listProjectProduct, modelSearch, index.ToString());
                    bool check = true;
                    if (listchild.Count > 0)
                    {
                        foreach (var items in listchild)
                        {
                            if (listProjectGeneralDesign.FirstOrDefault(i => i.ProjectProductId.Equals(items.Id)) == null)
                            {
                                check = false;
                                break;
                            }
                        }
                    }
                    item.CheckTHTK = check;
                }
                else if (projectGeneralDesignModule != null)
                {
                    item.CheckTHTK = true;
                    item.CreateIndex = projectGeneralDesignModule.CreateIndex;
                }
            }
            searchResult.ListResult = listProjectProduct;
            return searchResult;
        }

        public SearchResultModel<ProjectGeneralDesignResultModel> SearchProjectGeneralDesign(ProjectGeneralDesignSearchModel modelSearch)
        {
            SearchResultModel<ProjectGeneralDesignResultModel> searchResult = new SearchResultModel<ProjectGeneralDesignResultModel>();

            var dataQuery = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
                             join c in db.Departments.AsNoTracking() on a.DepartmentRequestId equals c.Id into ac
                             from acx in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentCreateId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             where a.ProjectProductId.Equals(modelSearch.ProjectProductId)
                             orderby a.CreateIndex descending
                             select new ProjectGeneralDesignResultModel
                             {
                                 Id = a.Id,
                                 CreateIndex = a.CreateIndex,
                                 ProjectProductCode = b.ContractCode,
                                 ProjectProductName = b.ContractName,
                                 DepartmentRequestId = a.DepartmentRequestId,
                                 DepartmentRequestName = acx != null ? acx.Name : string.Empty,
                                 DepartmentCreateId = a.DepartmentCreateId,
                                 DepartmentCreateName = da != null ? da.Name : string.Empty,
                                 RequestDate = a.RequestDate,
                                 CreateDate = a.CreateDate,
                                 DataType = b.DataType,
                                 ProductId = b.ProductId,
                                 ModuleId = b.ModuleId,
                                 ApproveStatus = a.ApproveStatus
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = /*SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize)*/dataQuery.ToList();
            searchResult.ListResult = listResult;
            foreach (var item in listResult)
            {
                if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    var product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        item.DesignCode = product.Code;
                        item.DesignName = product.Name;
                    }
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    var module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
                    if (module != null)
                    {
                        item.DesignCode = module.Code;
                        item.DesignName = module.Name;
                    }
                }
            }
            return searchResult;
        }

        public void UpdateMaterialImportBOM(ProjectGeneralDesignModel model)
        {
            foreach (var mpp in model.ListModule)
            {
                var materials = db.MaterialImportBOMDrafts.Where(a => a.ProjectId.Equals(mpp.ProjectProductId) && a.ModuleId.Equals(mpp.ModuleId));
                if (materials.Count() > 0)
                {
                    db.MaterialImportBOMDrafts.RemoveRange(materials);
                }
                var materialOfModules = (from a in db.ModuleMaterials.AsNoTracking()
                                         join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                         where a.ModuleId.Equals(mpp.ModuleId)
                                         orderby a.Index
                                         select new ModuleMaterialResultModel
                                         {
                                             Id = a.Id,
                                             ModuleId = a.ModuleId,
                                             MaterialId = a.MaterialId,
                                             MaterialCode = a.MaterialCode,
                                             MaterialName = a.MaterialName,
                                             Specification = a.Specification,
                                             RawMaterialCode = a.RawMaterialCode,
                                             RawMaterial = a.RawMaterial,
                                             Price = a.Price,
                                             Quantity = a.Quantity,
                                             ReadQuantity = a.Quantity,
                                             Amount = a.Amount,
                                             Weight = a.Weight,
                                             ManufacturerId = a.ManufacturerId,
                                             ManufacturerCode = a.ManufacturerCode,
                                             Note = a.Note,
                                             UnitName = a.UnitName,
                                             Pricing = c.Pricing,
                                             LastBuyDate = c.LastBuyDate,
                                             Index = a.Index,
                                             InputPriceDate = c.InputPriceDate,
                                             PriceHistory = c.PriceHistory,
                                             DeliveryDays = c.DeliveryDays,
                                             Path = a.Path,
                                             FileName = a.FileName,
                                             IsSetup = c.IsSetup
                                         }).ToList().OrderBy(a => a.Index);
                List<MaterialImportBOMDraft> list = new List<MaterialImportBOMDraft>();
                foreach (var item in materialOfModules)
                {
                    MaterialImportBOMDraft material = new MaterialImportBOMDraft();
                    material.ProjectId = mpp.ProjectProductId;
                    material.ModuleId = mpp.ModuleId;
                    material.Id = Guid.NewGuid().ToString();
                    material.Index = item.Index;
                    material.Name = item.MaterialName;
                    material.Code = item.MaterialCode;
                    material.Specification = item.Specification;
                    material.RawMaterial = item.RawMaterial;
                    material.RawMaterialCode = item.RawMaterialCode;
                    material.UnitName = item.UnitName;
                    material.Weight = item.Weight;
                    material.Quantity = item.Quantity;
                    material.Pricing = item.Pricing;
                    material.Note = item.Note;
                    material.Amount = item.Amount;
                    material.ManufactureCode = item.ManufacturerCode;
                    material.Status = false;
                    material.UpdateStatus = 1;
                    list.Add(material);
                }
                db.MaterialImportBOMDrafts.AddRange(list);
            }
            db.SaveChanges();
        }

        public MaterialChangeDataModel GetListMaterialOfModule(ProjectGeneralDesignModel model)
        {
            MaterialChangeDataModel result = new MaterialChangeDataModel();
            result.IsExit = false;
            List<ProjectGeneralDesignModuleModel> moduleProjectProducts = new List<ProjectGeneralDesignModuleModel>();
            List<MaterialChangeModel> oldMaterialChangeModels = new List<MaterialChangeModel>();
            List<MaterialChangeModel> newMaterialChangeModels = new List<MaterialChangeModel>();
            List<MaterialChangeModel> allMaterialOfModules = new List<MaterialChangeModel>();
            foreach (var module in model.ListModule)
            {
                ProjectGeneralDesignModuleModel moduleProjectProduct = new ProjectGeneralDesignModuleModel();
                moduleProjectProduct.ModuleId = module.ModuleId;
                moduleProjectProduct.ProjectProductId = module.ProjectProductId;
                moduleProjectProduct.ModuleCode = module.ModuleCode;

                moduleProjectProducts.Add(moduleProjectProduct);
                result.IsExit = true;
                var materialOfModules = (from a in db.ModuleMaterials.AsNoTracking()
                                         join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                         where a.ModuleId.Equals(module.ModuleId)
                                         orderby a.Index
                                         select new ModuleMaterialResultModel
                                         {
                                             Id = a.Id,
                                             ModuleId = a.ModuleId,
                                             MaterialId = a.MaterialId,
                                             MaterialCode = a.MaterialCode,
                                             MaterialName = a.MaterialName,
                                             Specification = a.Specification,
                                             RawMaterialCode = a.RawMaterialCode,
                                             RawMaterial = a.RawMaterial,
                                             Price = a.Price,
                                             Quantity = a.Quantity,
                                             ReadQuantity = a.Quantity,
                                             Amount = a.Amount,
                                             Weight = a.Weight,
                                             ManufacturerId = a.ManufacturerId,
                                             ManufacturerCode = a.ManufacturerCode,
                                             Note = a.Note,
                                             UnitName = a.UnitName,
                                             Pricing = c.Pricing,
                                             LastBuyDate = c.LastBuyDate,
                                             Index = a.Index,
                                             InputPriceDate = c.InputPriceDate,
                                             PriceHistory = c.PriceHistory,
                                             DeliveryDays = c.DeliveryDays,
                                             Path = a.Path,
                                             FileName = a.FileName,
                                             IsSetup = c.IsSetup
                                         }).ToList().OrderBy(a => a.Index);

                var materialOfBOM1AndBOM2 = db.MaterialImportBOMDrafts.Where(a => a.ProjectId.Equals(module.ProjectProductId) && a.ModuleId.Equals(module.ModuleId) && (a.UpdateStatus == 0 || a.UpdateStatus == 1)).ToList().OrderBy(a => a.Index);
                foreach (var item in materialOfBOM1AndBOM2)
                {
                    //old data
                    MaterialChangeModel m = new MaterialChangeModel();
                    m.Id = item.Id;
                    m.Index = item.Index;
                    m.Name = item.Name;
                    m.Code = item.Code;
                    m.Specification = item.Specification;
                    m.RawMaterial = "-".Equals(item.RawMaterial) ? "" : item.RawMaterial;
                    m.RawMaterialCode = item.RawMaterialCode;
                    m.UnitName = item.UnitName;
                    m.Weight = item.Weight;
                    m.Quantity = item.Quantity;
                    m.Pricing = item.Pricing;
                    m.Note = item.Note;
                    m.Amount = item.Amount;
                    m.ManufactureCode = item.ManufactureCode;
                    m.TotalPrice = m.Quantity * m.Pricing;
                    m.ModuleId = module.ModuleId;
                    m.ModuleCode = module.ModuleCode;

                    oldMaterialChangeModels.Add(m);
                }
                foreach (var item in materialOfBOM1AndBOM2)
                {
                    if (materialOfModules.FirstOrDefault(a => a.MaterialCode.Equals(item.Code)) == null)
                    {
                        MaterialChangeModel m = new MaterialChangeModel();
                        m.NewIndex = item.Index;
                        m.Code = item.Code;
                        m.NewName = item.Name;
                        m.NewSpecification = item.Specification;
                        m.NewRawMaterial = (item.RawMaterial == null || item.RawMaterial.Equals("-")) ? "" : item.RawMaterial;
                        m.NewRawMaterialCode = item.RawMaterialCode;
                        m.NewUnitName = item.UnitName;
                        m.NewWeight = item.Weight;
                        m.NewPricing = item.Pricing;
                        m.NewNote = item.Note;
                        m.NewManufactureCode = item.ManufactureCode;
                        m.NewQuantity = item.Quantity;
                        m.NewAmount = item.Amount;
                        m.NewTotalPrice = m.NewQuantity * m.NewPricing;
                        m.ModuleId = module.ModuleId;
                        m.ModuleCode = module.ModuleCode;
                        newMaterialChangeModels.Add(m);
                    }

                }

                foreach (var item in materialOfModules)
                {
                    if (materialOfBOM1AndBOM2.FirstOrDefault(a => a.Code.Equals(item.MaterialCode)) == null)
                    {
                        MaterialChangeModel m = new MaterialChangeModel();
                        m.Id = item.Id;
                        m.THTKIndex = item.Index;
                        m.THTKName = item.MaterialName;
                        m.Code = item.MaterialCode;
                        m.THTKSpecification = item.Specification;
                        m.THTKRawMaterial = item.RawMaterial;
                        m.THTKRawMaterialCode = item.RawMaterialCode;
                        m.THTKUnitName = item.UnitName;
                        m.THTKWeight = item.Weight;
                        m.THTKQuantity = item.Quantity;
                        m.THTKPricing = item.Pricing;
                        m.THTKNote = item.Note;
                        m.THTKAmount = item.Amount;
                        m.THTKManufactureCode = item.ManufacturerCode;
                        m.THTKTotalPrice = item.Quantity * item.Pricing;
                        m.ModuleId = module.ModuleId;
                        m.ModuleCode = module.ModuleCode;

                        allMaterialOfModules.Add(m);
                    }
                }
                foreach (var item in oldMaterialChangeModels)
                {
                    foreach (var item1 in newMaterialChangeModels)
                    {
                        if (item1.Code.Equals(item.Code))
                        {
                            if (item1.NewIndex.Equals(item.Index) && item1.NewName.Equals(item.Name) && item1.Code.Equals(item.Code)
                            && item1.NewSpecification != null ? item1.NewSpecification.Equals(item.Specification == null ? "" : item.Specification) : "".Equals(item.Specification == null ? "" : item.Specification)
                            && item1.NewRawMaterial != null ? item1.NewRawMaterial.Equals(item.RawMaterial == null ? "" : item.RawMaterial) : "".Equals(item.RawMaterial == null ? "" : item.RawMaterial)
                            && item1.NewRawMaterialCode != null ? item1.NewRawMaterialCode.Equals(item.RawMaterialCode == null ? "" : item.RawMaterialCode) : "".Equals(item.RawMaterialCode == null ? "" : item.RawMaterialCode)
                            && item1.NewUnitName.Equals(item.UnitName)
                            && item1.NewWeight.Equals(item.Weight)
                            && item1.NewPricing.Equals(item.Pricing)
                            && item1.NewNote != null ? item1.NewNote.Equals(item.Note == null ? "" : item.Note) : "".Equals(item.Note == null ? "" : item.Note)
                            && item1.NewManufactureCode.Equals(item.ManufactureCode)
                            && item1.NewQuantity.Equals(item.Quantity)
                            && item1.NewAmount.Equals(item.Amount)
                            //&& item1.NewTotalPrice.Equals(item.TotalPrice)
                            )
                            {
                                item1.DupplicateItem = false;
                            }
                            else
                            {
                                item1.DupplicateItem = true;
                            }
                        }

                    }

                    foreach (var item1 in allMaterialOfModules)
                    {
                        if (item1.Code.Equals(item.Code))
                        {
                            if (item1.THTKIndex.Equals(item.Index) && item1.THTKName.Equals(item.Name) && item1.Code.Equals(item.Code)
                            && item1.THTKSpecification != null ? item1.THTKSpecification.Equals(item.Specification == null ? "" : item.Specification) : "".Equals(item.Specification == null ? "" : item.Specification)
                            && item1.THTKRawMaterial != null ? item1.THTKRawMaterial.Equals(item.RawMaterial == null ? "" : item.RawMaterial) : "".Equals(item.RawMaterial == null ? "" : item.RawMaterial)
                            && item1.THTKRawMaterialCode != null ? item1.THTKRawMaterialCode.Equals(item.RawMaterialCode == null ? "" : item.RawMaterialCode) : "".Equals(item.RawMaterialCode == null ? "" : item.RawMaterialCode)
                            && item1.THTKUnitName.Equals(item.UnitName)
                            && item1.THTKWeight.Equals(item.Weight)
                            && item1.THTKPricing.Equals(item.Pricing)
                            && item1.THTKNote != null ? item1.THTKNote.Equals(item.Note == null ? "" : item.Note) : "".Equals(item.Note == null ? "" : item.Note)
                            && item1.THTKManufactureCode.Equals(item.ManufactureCode)
                            && item1.THTKQuantity.Equals(item.Quantity)
                            && item1.THTKAmount.Equals(item.Amount)
                            //&& item1.TotalPrice.Equals(item.TotalPrice)
                            )
                            {
                                item1.DupplicateItem = false;
                            }
                            else
                            {
                                item1.DupplicateItem = true;
                            }
                        }
                    }

                }
            }

            result.ProjectProductId = model.ProjectProductId;
            result.ModuleId = model.ModuleId;
            result.ModuleIdProjectProducts = moduleProjectProducts;
            result.oldMaterialChangeModels = oldMaterialChangeModels.OrderByNatural("Index").ToList();
            result.newMaterialChangeModels = newMaterialChangeModels.OrderByNatural("Index").ToList();
            result.allMaterialOfModules = allMaterialOfModules.OrderByNatural("Index").ToList();
            return result;
        }

        public string AddProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            string projectGeneralDesignId = Guid.NewGuid().ToString();
            try
            {
                var check = CheckApproveStatus(model.ProjectProductId);
                if (!check)
                {
                    throw NTSException.CreateInstance("Bạn chưa phê duyệt lần tổng hợp gần nhất");
                }

                bool checkCreate = false;
                if (model.ListModule.FirstOrDefault(i => i.ModuleStatus == 1) != null)
                {
                    checkCreate = true;
                }
                if (model.ListMaterial.FirstOrDefault(i => i.CreateIndex == model.CreateIndex) != null)
                {
                    checkCreate = true;
                }
                if (!checkCreate)
                {
                    throw NTSException.CreateInstance("Bạn chưa chọn sản phẩm hoặc thêm vật tư!");
                }
                var listMaterial = db.Materials.AsNoTracking().ToList();
                foreach (var item in model.ListModule)
                {
                    var listMaterialId = (from a in db.ModuleMaterials.AsNoTracking()
                                          join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                          where a.ModuleId.Equals(item.Id) && c.Status.Equals("2")
                                          orderby a.MaterialCode
                                          select new ModuleMaterialResultModel
                                          {
                                              MaterialId = a.MaterialId
                                          }).ToList();
                    if (listMaterialId.Count > 0)
                    {
                        var names = string.Join(", ", listMaterialId.Select(a => a.MaterialName).ToArray());
                        throw NTSException.CreateInstance("Module chứa vật tư đã ngưng sản xuất : " + names + "");
                    }
                }

                ProjectProduct projectProduct;

                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        ProjectGeneralDesign projectGeneralDesign = new ProjectGeneralDesign
                        {
                            Id = projectGeneralDesignId,
                            Index = model.Index.NTSTrim(),
                            DepartmentRequestId = model.DepartmentRequestId,
                            DepartmentCreateId = model.DepartmentCreateId,
                            RequestDate = model.RequestDate,
                            ProjectProductId = model.ProjectProductId,
                            ProjectId = model.ProjectId,
                            CreateIndex = model.CreateIndex,
                            DesignBy = model.DesignBy,
                            ApproveStatus = model.ApproveStatus,
                            CreateBy = model.CreateBy,
                            CreateDate = DateTime.Now,
                            UpdateBy = model.CreateBy,
                            UpdateDate = DateTime.Now
                        };

                        projectProduct = new ProjectProduct();
                        projectProduct = db.ProjectProducts.FirstOrDefault(i => i.Id.Equals(model.ProjectProductId));
                        if (projectProduct != null)
                        {
                            projectProduct.GeneralDesignLastDate = DateTime.Now;
                        }

                        if (model.ListModule.Count > 0)
                        {
                            int startIndex = 1;
                            var projectGeneralDesignModules = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                               join b in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                               where a.ProjectProductId.Equals(model.ProjectProductId)
                                                               select new { b.Index }).ToList();
                            if (projectGeneralDesignModules.Count > 0)
                            {
                                startIndex = projectGeneralDesignModules.Max(i => i.Index);
                                startIndex++;
                            }

                            ProjectGeneralDesignModule checkModule;
                            //List<ModuleMaterialFinishDesignModel> moduleMaterialFinishDesigns;
                            //List<ModuleMaterialFinishDesignModel> moduleMaterials;
                            //SearchModuleMaterialResultModel<ModuleMaterialResultModel> result;
                            List<ModuleMaterialFinishDesignModel> compareMaterials = new List<ModuleMaterialFinishDesignModel>();
                            List<IncurredMaterial> incurredMaterials = new List<IncurredMaterial>();
                            List<IncurredMaterial> removeIncurredMaterials = new List<IncurredMaterial>();
                            SearchModuleMaterialResultModel<ModuleMaterialResultModel> resultModel;
                            ModuleMaterialSearchModel moduleMaterialSearchModel;
                            List<ModuleMaterialFinishDesign> list = new List<ModuleMaterialFinishDesign>();
                            foreach (var item in model.ListModule)
                            {
                                projectProduct = db.ProjectProducts.FirstOrDefault(i => i.Id.Equals(item.ProjectProductId));

                                checkModule = new ProjectGeneralDesignModule();
                                checkModule = db.ProjectGeneralDesignModules.FirstOrDefault(i => i.ProjectProductId.Equals(item.ProjectProductId));
                                if (checkModule == null)
                                {
                                    ProjectGeneralDesignModule projectGeneralDesignModule = new ProjectGeneralDesignModule()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProjectGeneralDesignId = projectGeneralDesign.Id,
                                        ProjectProductId = item.ProjectProductId,
                                        ModuleId = item.ModuleId,
                                        Quantity = item.Quantity,
                                        ErrorQuantity = item.TotalError,
                                        ContractPrice = item.ContractPrice,
                                        ModulePrice = item.Pricing,
                                        Note = item.Note,
                                        ModuleStatus = item.ModuleStatus,
                                        Index = startIndex
                                    };

                                    if (projectProduct != null)
                                    {
                                        projectGeneralDesignModule.RealQuantity = (int)projectProduct.RealQuantity;
                                        if (projectGeneralDesignModule.Quantity != projectProduct.Quantity)
                                        {
                                            projectGeneralDesignModule.RealQuantity = (int)(projectProduct.RealQuantity / projectProduct.Quantity) * projectGeneralDesignModule.Quantity;
                                        }

                                        // Cập nhật ngày lập tổng hợp
                                        projectProduct.GeneralDesignLastDate = DateTime.Now;
                                    }

                                    db.ProjectGeneralDesignModules.Add(projectGeneralDesignModule);
                                    startIndex++;

                                    // Lưu DMVT và giá khi lập tổng hợp
                                    //    list = db.ModuleMaterialFinishDesigns.Where(i => i.ProjectProductId.Equals(item.ProjectProductId)).ToList();
                                    //    db.ModuleMaterialFinishDesigns.RemoveRange(list);

                                    //    moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                    //    {
                                    //        ModuleId = item.ModuleId
                                    //    };

                                    //    resultModel = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                    //    List<ModuleMaterialFinishDesign> finishDesigns = new List<ModuleMaterialFinishDesign>();
                                    //    foreach (var material in resultModel.ListResult)
                                    //    {
                                    //        finishDesigns.Add(new ModuleMaterialFinishDesign()
                                    //        {
                                    //            Id = Guid.NewGuid().ToString(),
                                    //            ProjectProductId = projectProduct.Id,
                                    //            ModuleId = material.ModuleId,
                                    //            MaterialId = material.MaterialId,
                                    //            MaterialCode = material.MaterialCode,
                                    //            MaterialName = material.MaterialName,
                                    //            Specification = material.Specification,
                                    //            RawMaterialCode = material.RawMaterialCode,
                                    //            RawMaterial = material.RawMaterial,
                                    //            Price = Math.Round(material.Pricing, 4),
                                    //            Quantity = material.Quantity,
                                    //            ReadQuantity = Math.Round(material.ReadQuantity, 4),
                                    //            Amount = material.Amount,
                                    //            Weight = material.Weight,
                                    //            ManufacturerId = material.ManufacturerId,
                                    //            Note = material.Note,
                                    //            UnitName = material.UnitName,
                                    //            Index = material.Index,
                                    //            ManufacturerCode = material.ManufacturerCode,
                                    //        });
                                    //    }

                                    //    db.ModuleMaterialFinishDesigns.AddRange(finishDesigns);
                                    //}
                                    //        else
                                    //        {
                                    //            if (db.ModuleMaterialFinishDesigns.FirstOrDefault(i => i.ProjectProductId.Equals(item.ProjectProductId)) == null)
                                    //            {
                                    //                moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                    //                {
                                    //                    ModuleId = item.ModuleId
                                    //                };

                                    //                resultModel = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                    //                List<ModuleMaterialFinishDesign> finishDesigns = new List<ModuleMaterialFinishDesign>();
                                    //                foreach (var material in resultModel.ListResult)
                                    //                {
                                    //                    finishDesigns.Add(new ModuleMaterialFinishDesign()
                                    //                    {
                                    //                        Id = Guid.NewGuid().ToString(),
                                    //                        ProjectProductId = projectProduct.Id,
                                    //                        ModuleId = material.ModuleId,
                                    //                        MaterialId = material.MaterialId,
                                    //                        MaterialCode = material.MaterialCode,
                                    //                        MaterialName = material.MaterialName,
                                    //                        Specification = material.Specification,
                                    //                        RawMaterialCode = material.RawMaterialCode,
                                    //                        RawMaterial = material.RawMaterial,
                                    //                        Price = Math.Round(material.Pricing, 4),
                                    //                        Quantity = material.Quantity,
                                    //                        ReadQuantity = Math.Round(material.ReadQuantity, 4),
                                    //                        Amount = material.Amount,
                                    //                        Weight = material.Weight,
                                    //                        ManufacturerId = material.ManufacturerId,
                                    //                        Note = material.Note,
                                    //                        UnitName = material.UnitName,
                                    //                        Index = material.Index,
                                    //                        ManufacturerCode = material.ManufacturerCode,
                                    //                    });
                                    //                }

                                    //                db.ModuleMaterialFinishDesigns.AddRange(finishDesigns);
                                    //            }
                                    //            else
                                    //            {
                                    //                // Tính danh mục vật tư phát sinh
                                    //                if (projectProduct.DataType == Constants.ProjectProduct_DataType_Module || projectProduct.DataType == Constants.ProjectProduct_DataType_Paradigm)
                                    //                {
                                    //                    removeIncurredMaterials = db.IncurredMaterials.Where(i => i.ProjectProductId.Equals(item.ProjectProductId)).ToList();
                                    //                    db.IncurredMaterials.RemoveRange(removeIncurredMaterials);

                                    //                    moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                    //                    {
                                    //                        ModuleId = item.ModuleId
                                    //                    };

                                    //                    result = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                    //                    moduleMaterials = new List<ModuleMaterialFinishDesignModel>();
                                    //                    moduleMaterials = (from a in result.ListResult
                                    //                                       group a by new { a.MaterialId } into g
                                    //                                       select new ModuleMaterialFinishDesignModel()
                                    //                                       {
                                    //                                           MaterialId = g.Key.MaterialId,
                                    //                                           Quantity = g.Sum(i => i.ReadQuantity)
                                    //                                       }).ToList();

                                    //                    moduleMaterialFinishDesigns = new List<ModuleMaterialFinishDesignModel>();
                                    //                    moduleMaterialFinishDesigns = (from a in db.ModuleMaterialFinishDesigns.AsNoTracking()
                                    //                                                   where a.ProjectProductId.Equals(a.ProjectProductId)
                                    //                                                   group a by new { a.MaterialId, a.ProjectProductId, a.Price } into g
                                    //                                                   select new ModuleMaterialFinishDesignModel()
                                    //                                                   {
                                    //                                                       MaterialId = g.Key.MaterialId,
                                    //                                                       ProjectProductId = g.Key.ProjectProductId,
                                    //                                                       Quantity = g.Sum(i => i.Quantity),
                                    //                                                       Price = g.Key.Price
                                    //                                                   }).ToList();

                                    //                    compareMaterials = (from a in moduleMaterials
                                    //                                        join b in moduleMaterialFinishDesigns on a.MaterialId equals b.MaterialId into ab
                                    //                                        from ba in ab.DefaultIfEmpty()
                                    //                                        select new ModuleMaterialFinishDesignModel
                                    //                                        {
                                    //                                            MaterialId = a.MaterialId,
                                    //                                            ProjectProductId = ba != null ? ba.ProjectProductId : string.Empty,
                                    //                                            Quantity = ba != null ? a.Quantity - ba.Quantity : 0,
                                    //                                            Price = ba != null ? ba.Price : 0
                                    //                                        }).ToList();

                                    //                    foreach (var material in compareMaterials)
                                    //                    {
                                    //                        if (material.Quantity > 0)
                                    //                        {
                                    //                            incurredMaterials.Add(new IncurredMaterial()
                                    //                            {
                                    //                                Id = Guid.NewGuid().ToString(),
                                    //                                ProjectProductId = item.ProjectProductId,
                                    //                                MaterialId = material.MaterialId,
                                    //                                Quantity = material.Quantity,
                                    //                                Price = material.Price
                                    //                            });
                                    //                        }
                                    //                    }

                                    //                    db.IncurredMaterials.AddRange(incurredMaterials);

                                    //                    projectProduct.IsIncurred = incurredMaterials.Count > 0;
                                    //                }
                                    //            }
                                }
                            }
                        }

                        if (model.ListMaterial.Count > 0)
                        {
                            foreach (var item in model.ListMaterial)
                            {
                                ProjectGeneralDesignMaterial projectGeneralDesignMaterial = new ProjectGeneralDesignMaterial()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectGeneralDesignId = projectGeneralDesign.Id,
                                    MaterialId = item.Id,
                                    Quantity = item.Quantity,
                                    Inventoty = item.Inventoty,
                                    ContractPrice = item.ContractPrice,
                                    Price = item.Price,
                                    Note = item.Note,
                                    Type = item.Type
                                };
                                db.ProjectGeneralDesignMaterials.Add(projectGeneralDesignMaterial);
                            }
                        }

                        db.ProjectGeneralDesigns.Add(projectGeneralDesign);
                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (NTSException ntsex)
                    {
                        throw ntsex;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new NTSLogException(model, ex);
                    }
                }
            }
            catch (NTSException ntsex)
            {
                throw ntsex;
            }
            catch (Exception ex)
            {
                NtsLog.logger.Error(ex);
                throw new NTSLogException(model, ex);
            }

            return projectGeneralDesignId;
        }
        public void UpdateProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            bool checkUpdate = false;
            ProjectProduct projectProduct;
            if (model.ListModule.FirstOrDefault(i => i.ModuleStatus == 1 || i.ModuleStatus == 2 && i.CreateIndex == model.CreateIndex) != null)
            {
                checkUpdate = true;
            }
            if (model.ListMaterial.FirstOrDefault(i => i.CreateIndex == model.CreateIndex) != null)
            {
                checkUpdate = true;
            }
            if (!checkUpdate)
            {
                throw NTSException.CreateInstance("Bạn chưa chọn sản phẩm hoặc thêm vật tư!");
            }
            var listMaterial = db.Materials.AsNoTracking().ToList();
            foreach(var item in model.ListModule)
            {
                var listMaterialId = (from a in db.ModuleMaterials.AsNoTracking()
                                 join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                 where a.ModuleId.Equals(item.Id) && c.Status.Equals("2")
                                 orderby a.MaterialCode
                                 select new ModuleMaterialResultModel
                                 {
                                     MaterialId = a.MaterialId,
                                     MaterialName = a.MaterialName,
                                 }).ToList();
                if(listMaterialId.Count > 0)
                {
                    var names = string.Join(", ", listMaterialId.Select(a => a.MaterialName).ToArray());
                    throw NTSException.CreateInstance("Module chứa vật tư đã ngưng sản xuất : "+ names + "");
                }
            }
            

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectGeneralDesign = db.ProjectGeneralDesigns.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    projectGeneralDesign.Index = model.Index;
                    projectGeneralDesign.DepartmentRequestId = model.DepartmentRequestId;
                    projectGeneralDesign.DepartmentCreateId = model.DepartmentCreateId;
                    projectGeneralDesign.RequestDate = model.RequestDate;
                    projectGeneralDesign.ProjectProductId = model.ProjectProductId;
                    projectGeneralDesign.ProjectId = model.ProjectId;
                    projectGeneralDesign.CreateIndex = model.CreateIndex;
                    projectGeneralDesign.DesignBy = model.DesignBy;
                    projectGeneralDesign.ApproveStatus = model.ApproveStatus;
                    projectGeneralDesign.UpdateBy = model.UpdateBy;
                    projectGeneralDesign.UpdateDate = DateTime.Now;

                    projectProduct = new ProjectProduct();
                    projectProduct = db.ProjectProducts.FirstOrDefault(i => i.Id.Equals(model.ProjectProductId));
                    if (projectProduct != null)
                    {
                        projectProduct.GeneralDesignLastDate = DateTime.Now;
                    }

                    var generalDesignModules = db.ProjectGeneralDesignModules.Where(i => model.Id.Equals(i.ProjectGeneralDesignId)).ToList();
                    if (generalDesignModules.Count > 0)
                    {
                        foreach (var item in generalDesignModules)
                        {
                            if (model.ListModule.FirstOrDefault(i => i.ProjectProductId.Equals(item.ProjectProductId)) == null)
                            {
                                db.ProjectGeneralDesignModules.Remove(item);
                            }
                        }
                    }
                    if (model.ListModule.Count > 0)
                    {
                        int startIndex = 1;
                        var projectGeneralDesignModules = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                           join b in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                           where a.ProjectProductId.Equals(model.ProjectProductId)
                                                           select new { b.Index }).ToList();
                        if (projectGeneralDesignModules.Count > 0)
                        {
                            startIndex = projectGeneralDesignModules.Max(i => i.Index);
                            startIndex++;
                        }

                        SearchModuleMaterialResultModel<ModuleMaterialResultModel> resultModel;
                        ModuleMaterialSearchModel moduleMaterialSearchModel;
                        List<ModuleMaterialFinishDesign> list = new List<ModuleMaterialFinishDesign>();
                        ProjectGeneralDesignModule checkModule;
                        foreach (var item in model.ListModule)
                        {
                            projectProduct = db.ProjectProducts.FirstOrDefault(i => i.Id.Equals(item.ProjectProductId));
                            checkModule = db.ProjectGeneralDesignModules.FirstOrDefault(i => i.ProjectProductId.Equals(item.ProjectProductId));
                            if (checkModule == null)
                            {
                                ProjectGeneralDesignModule projectGeneralDesignModule = new ProjectGeneralDesignModule()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectGeneralDesignId = projectGeneralDesign.Id,
                                    ProjectProductId = item.ProjectProductId,
                                    ModuleId = item.ModuleId,
                                    Quantity = item.Quantity,
                                    ErrorQuantity = item.TotalError,
                                    ContractPrice = item.ContractPrice,
                                    ModulePrice = item.Pricing,
                                    Note = item.Note,
                                    ModuleStatus = item.ModuleStatus,
                                    Index = startIndex,
                                };

                                if (projectProduct != null)
                                {
                                    projectGeneralDesignModule.RealQuantity = (int)projectProduct.RealQuantity;
                                    if (projectGeneralDesignModule.Quantity != projectProduct.Quantity)
                                    {
                                        projectGeneralDesignModule.RealQuantity = (int)(projectProduct.RealQuantity / projectProduct.Quantity) * projectGeneralDesignModule.Quantity;
                                    }

                                    projectProduct.GeneralDesignLastDate = DateTime.Now;
                                }
                                db.ProjectGeneralDesignModules.Add(projectGeneralDesignModule);
                                startIndex++;

                                // Lưu DMVT và giá khi lập tổng hợp
                                //list = db.ModuleMaterialFinishDesigns.Where(i => i.ProjectProductId.Equals(item.ProjectProductId)).ToList();
                                //db.ModuleMaterialFinishDesigns.RemoveRange(list);

                                //moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                //{
                                //    ModuleId = item.ModuleId
                                //};

                                //resultModel = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                //List<ModuleMaterialFinishDesign> finishDesigns = new List<ModuleMaterialFinishDesign>();
                                //foreach (var material in resultModel.ListResult)
                                //{
                                //    finishDesigns.Add(new ModuleMaterialFinishDesign()
                                //    {
                                //        Id = Guid.NewGuid().ToString(),
                                //        ProjectProductId = projectProduct.Id,
                                //        ModuleId = material.ModuleId,
                                //        MaterialId = material.MaterialId,
                                //        MaterialCode = material.MaterialCode,
                                //        MaterialName = material.MaterialName,
                                //        Specification = material.Specification,
                                //        RawMaterialCode = material.RawMaterialCode,
                                //        RawMaterial = material.RawMaterial,
                                //        Price = Math.Round(material.Pricing, 4),
                                //        Quantity = material.Quantity,
                                //        ReadQuantity = Math.Round(material.ReadQuantity, 4),
                                //        Amount = material.Amount,
                                //        Weight = material.Weight,
                                //        ManufacturerId = material.ManufacturerId,
                                //        Note = material.Note,
                                //        UnitName = material.UnitName,
                                //        Index = material.Index,
                                //        ManufacturerCode = material.ManufacturerCode,
                                //    });
                                //}

                                //db.ModuleMaterialFinishDesigns.AddRange(finishDesigns);
                            }
                            else
                            {
                                //if (checkModule.ProjectGeneralDesignId.Equals(model.Id))
                                //{
                                checkModule.ErrorQuantity = item.TotalError;
                                checkModule.ContractPrice = item.ContractPrice;
                                checkModule.ModulePrice = item.Pricing;
                                checkModule.Note = item.Note;
                                checkModule.ModuleStatus = item.ModuleStatus;
                                //checkModule.RealQuantity = item.RealQuantity;
                                if (checkModule.Quantity != item.Quantity)
                                {
                                    checkModule.RealQuantity = (checkModule.RealQuantity / checkModule.Quantity) * item.Quantity;
                                }
                                checkModule.Quantity = item.Quantity;
                                //}

                                //if (db.ModuleMaterialFinishDesigns.FirstOrDefault(i => i.ProjectProductId.Equals(item.ProjectProductId)) == null)
                                //{
                                //    moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                //    {
                                //        ModuleId = item.ModuleId
                                //    };

                                //    resultModel = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                //    List<ModuleMaterialFinishDesign> finishDesigns = new List<ModuleMaterialFinishDesign>();
                                //    foreach (var material in resultModel.ListResult)
                                //    {
                                //        finishDesigns.Add(new ModuleMaterialFinishDesign()
                                //        {
                                //            Id = Guid.NewGuid().ToString(),
                                //            ProjectProductId = projectProduct.Id,
                                //            ModuleId = material.ModuleId,
                                //            MaterialId = material.MaterialId,
                                //            MaterialCode = material.MaterialCode,
                                //            MaterialName = material.MaterialName,
                                //            Specification = material.Specification,
                                //            RawMaterialCode = material.RawMaterialCode,
                                //            RawMaterial = material.RawMaterial,
                                //            Price = Math.Round(material.Pricing, 4),
                                //            Quantity = material.Quantity,
                                //            ReadQuantity = Math.Round(material.ReadQuantity, 4),
                                //            Amount = material.Amount,
                                //            Weight = material.Weight,
                                //            ManufacturerId = material.ManufacturerId,
                                //            Note = material.Note,
                                //            UnitName = material.UnitName,
                                //            Index = material.Index,
                                //            ManufacturerCode = material.ManufacturerCode,
                                //        });
                                //    }

                                //    db.ModuleMaterialFinishDesigns.AddRange(finishDesigns);
                                //}
                                //else
                                //{
                                //    #region Lưu danh mục vật tư
                                //    // Tính danh mục vật tư phát sinh
                                //    List<ModuleMaterialFinishDesignModel> moduleMaterialFinishDesigns;
                                //    List<ModuleMaterialFinishDesignModel> moduleMaterials;
                                //    SearchModuleMaterialResultModel<ModuleMaterialResultModel> result;
                                //    List<ModuleMaterialFinishDesignModel> compareMaterials = new List<ModuleMaterialFinishDesignModel>();
                                //    List<IncurredMaterial> incurredMaterials = new List<IncurredMaterial>();
                                //    List<IncurredMaterial> removeIncurredMaterials = new List<IncurredMaterial>();

                                //    removeIncurredMaterials = db.IncurredMaterials.Where(i => i.ProjectProductId.Equals(item.ProjectProductId)).ToList();
                                //    db.IncurredMaterials.RemoveRange(removeIncurredMaterials);

                                //    moduleMaterialSearchModel = new ModuleMaterialSearchModel
                                //    {
                                //        ModuleId = item.ModuleId
                                //    };

                                //    result = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                                //    moduleMaterials = new List<ModuleMaterialFinishDesignModel>();
                                //    moduleMaterials = (from a in result.ListResult
                                //                       group a by new { a.MaterialId } into g
                                //                       select new ModuleMaterialFinishDesignModel()
                                //                       {
                                //                           MaterialId = g.Key.MaterialId,
                                //                           Quantity = g.Sum(i => i.ReadQuantity)
                                //                       }).ToList();

                                //    moduleMaterialFinishDesigns = new List<ModuleMaterialFinishDesignModel>();
                                //    moduleMaterialFinishDesigns = (from a in db.ModuleMaterialFinishDesigns.AsNoTracking()
                                //                                   where a.ProjectProductId.Equals(item.ProjectProductId)
                                //                                   group a by new { a.MaterialId, a.ProjectProductId, a.Price } into g
                                //                                   select new ModuleMaterialFinishDesignModel()
                                //                                   {
                                //                                       MaterialId = g.Key.MaterialId,
                                //                                       ProjectProductId = g.Key.ProjectProductId,
                                //                                       Quantity = g.Sum(i => i.Quantity),
                                //                                       Price = g.Key.Price
                                //                                   }).ToList();

                                //    compareMaterials = (from a in moduleMaterials
                                //                        join b in moduleMaterialFinishDesigns on a.MaterialId equals b.MaterialId into ab
                                //                        from ba in ab.DefaultIfEmpty()
                                //                        select new ModuleMaterialFinishDesignModel
                                //                        {
                                //                            MaterialId = a.MaterialId,
                                //                            ProjectProductId = a.ProjectProductId,
                                //                            Quantity = ba != null ? a.Quantity - ba.Quantity : 0,
                                //                            Price = ba != null ? ba.Price : 0
                                //                        }).ToList();

                                //    foreach (var material in compareMaterials)
                                //    {
                                //        if (material.Quantity > 0)
                                //        {
                                //            incurredMaterials.Add(new IncurredMaterial()
                                //            {
                                //                Id = Guid.NewGuid().ToString(),
                                //                ProjectProductId = item.ProjectProductId,
                                //                MaterialId = material.MaterialId,
                                //                Quantity = material.Quantity,
                                //                Price = material.Price
                                //            });
                                //        }
                                //    }

                                //    db.IncurredMaterials.AddRange(incurredMaterials);
                                //    #endregion
                                //}
                            }
                        }
                    }

                    var generalDesignMaterials = db.ProjectGeneralDesignMaterials.Where(i => model.Id.Equals(i.ProjectGeneralDesignId)).ToList();
                    if (generalDesignMaterials.Count > 0)
                    {
                        db.ProjectGeneralDesignMaterials.RemoveRange(generalDesignMaterials);
                    }
                    if (model.ListMaterial.Count > 0)
                    {
                        foreach (var item in model.ListMaterial)
                        {
                            ProjectGeneralDesignMaterial projectGeneralDesignMaterial = new ProjectGeneralDesignMaterial()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectGeneralDesignId = projectGeneralDesign.Id,
                                MaterialId = item.Id,
                                Quantity = item.Quantity,
                                Inventoty = item.Inventoty,
                                ContractPrice = item.ContractPrice,
                                Price = item.Price,
                                Note = item.Note,
                                Type = item.Type
                            };
                            db.ProjectGeneralDesignMaterials.Add(projectGeneralDesignMaterial);
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

        public void DeleteProjectGeneralDesign(ProjectGeneralDesignModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectGeneralDesign = db.ProjectGeneralDesigns.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (projectGeneralDesign == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectGeneralDesign);
                    }
                    var projectGeneralDesignModules = db.ProjectGeneralDesignModules.Where(i => model.Id.Equals(i.ProjectGeneralDesignId)).ToList();
                    var projectGeneralDesignMaterials = db.ProjectGeneralDesignMaterials.Where(i => model.Id.Equals(i.ProjectGeneralDesignId)).ToList();
                    if (projectGeneralDesignModules.Count > 0)
                    {
                        db.ProjectGeneralDesignModules.RemoveRange(projectGeneralDesignModules);
                    }
                    if (projectGeneralDesignMaterials.Count > 0)
                    {
                        db.ProjectGeneralDesignMaterials.RemoveRange(projectGeneralDesignMaterials);
                    }
                    db.ProjectGeneralDesigns.Remove(projectGeneralDesign);
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
        public ProjectGeneralDesignModel GetProjectGeneralDesignInfo(ProjectGeneralDesignModel model)
        {
            var resultInfo = db.ProjectGeneralDesigns.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProjectGeneralDesignModel
            {
                Id = p.Id,
                Index = p.Index,
                DepartmentRequestId = p.DepartmentRequestId,
                DepartmentCreateId = p.DepartmentCreateId,
                RequestDate = p.RequestDate,
                ProjectProductId = p.ProjectProductId,
                ProjectId = p.ProjectId,
                CreateIndex = p.CreateIndex,
                ApproveStatus = p.ApproveStatus,
                DesignBy = p.DesignBy,
                CreateDate = p.CreateDate,
            }).FirstOrDefault();

            var listmaterial = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                where !b.ProjectGeneralDesignId.Equals(model.Id) && a.ProjectProductId.Equals(resultInfo.ProjectProductId)
                                select new { b.MaterialId, a.CreateIndex, b.Quantity }).ToList();

            var listmaterialIndex = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                     join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                     where a.ProjectProductId.Equals(resultInfo.ProjectProductId)
                                     select new { b.MaterialId, a.CreateIndex, b.Quantity }).ToList();

            // Kiểm tra xem có được cập nhật dang sách vật tư không
            if (listmaterial.Count > 0)
            {
                var maxCreateIndex = listmaterial.Max(i => i.CreateIndex);
                if (maxCreateIndex > resultInfo.CreateIndex)
                {
                    resultInfo.IsUpdate = true;
                }
            }

            var listMeterialDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                      where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                      join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                      join c in db.Manufactures.AsNoTracking() on b.ManufactureId equals c.Id
                                      join d in db.Units.AsNoTracking() on b.UnitId equals d.Id
                                      where a.ProjectGeneralDesignId.Equals(model.Id)
                                      orderby b.Code
                                      select new ProjectGeneralDesignMaterialsModel
                                      {
                                          Id = a.MaterialId,
                                          Name = b.Name,
                                          Code = b.Code,
                                          Manafacture = c.Code,
                                          Quantity = (int)a.Quantity,
                                          OldQuantity = (int)a.Quantity,
                                          Inventoty = a.Inventoty,
                                          ContractPrice = a.ContractPrice,
                                          Price = resultInfo.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                          UnitName = d.Name,
                                          Type = a.Type,
                                          Status = b.Status,
                                          CreateIndex = resultInfo.CreateIndex
                                      }).ToList();

            var listModuleDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                    where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                    join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                                    where a.ProjectGeneralDesignId.Equals(model.Id)
                                    orderby b.Code
                                    select new ProjectGeneralDesignMaterialsModel
                                    {
                                        Id = a.MaterialId,
                                        Name = b.Name,
                                        Code = b.Code,
                                        Manafacture = Constants.Manufacture_TPA,
                                        Quantity = (int)a.Quantity,
                                        OldQuantity = (int)a.Quantity,
                                        Inventoty = a.Inventoty,
                                        ContractPrice = a.ContractPrice,
                                        Price = a.Price,
                                        UnitName = Constants.Unit_Bo,
                                        Type = a.Type,
                                        ModuleStatusUse = b.Status,
                                        CreateIndex = resultInfo.CreateIndex
                                    }).ToList();

            if (listModuleDesign.Count > 0 && resultInfo.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_NotApproved)
            {
                ModulePriceInfoModel modulePriceInfoModel;
                foreach (var item in listModuleDesign)
                {
                    modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.Id);
                    if (modulePriceInfoModel != null)
                    {
                        item.Price = modulePriceInfoModel.Price;
                    }
                }
            }

            resultInfo.ListMaterial.AddRange(listMeterialDesign);
            resultInfo.ListMaterial.AddRange(listModuleDesign);
            int minCreateIndex;
            decimal minQuantity = 0;
            if (resultInfo.CreateIndex > 1)
            {
                foreach (var item in resultInfo.ListMaterial)
                {
                    if (listmaterial.FirstOrDefault(i => i.MaterialId.Equals(item.Id)) != null)
                    {
                        item.IsDelete = true;
                    }

                    if (listmaterialIndex.Count() > 0)
                    {
                        var data = listmaterialIndex.Where(i => i.MaterialId.Equals(item.Id)).ToList();
                        if (data.Count > 0)
                        {
                            minCreateIndex = data.Min(i => i.CreateIndex);
                            minQuantity = data.FirstOrDefault(i => i.CreateIndex == minCreateIndex).Quantity;
                            if (minCreateIndex > 0 && data.FirstOrDefault(i => i.CreateIndex == minCreateIndex).Quantity == item.Quantity)
                            {
                                item.CreateIndex = minCreateIndex;
                            }
                            else
                            {
                                item.CreateIndex = resultInfo.CreateIndex;
                                item.Qty = item.Quantity - minQuantity;
                            }
                        }
                        else
                        {
                            item.CreateIndex = resultInfo.CreateIndex;
                        }
                    }
                    else
                    {
                        item.CreateIndex = resultInfo.CreateIndex;
                    }
                }
            }
            resultInfo.ListMaterial.OrderBy(i => i.Code);
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectGeneralDesign);
            }

            return resultInfo;
        }

        public GetDataModel GeneralDesign(ProjectGeneralDesignModel model)
        {
            var department = db.Departments.AsNoTracking().FirstOrDefault(i => model.DepartmentId.Equals(i.Id));
            var sbuId = string.Empty;
            if (department != null)
            {
                sbuId = department.SBUId;
            }

            decimal totalAmounts = 0;
            decimal totalPriceContract = 0;
            var check = db.ProjectProducts.AsNoTracking().FirstOrDefault(i => model.ProjectProductId.Equals(i.Id));
            if (check != null)
            {
                if (check.DataType != Constants.ProjectProduct_DataType_ProjectProduct && check.DataType != Constants.ProjectProduct_DataType_Module && check.DataType != Constants.ProjectProduct_DataType_Paradigm)
                {
                    throw NTSException.CreateInstance("Bạn chỉ được chọn kiểu thiết kế là sản phẩm, module, mô hình!");
                }
            }

            #region lấy thông tin lần tổng hợp trước
            bool checkVersion = false;
            var getProjectGeneralDesign = 0;
            ProjectGeneralDesignModel projectGeneralDesignModuleModel = new ProjectGeneralDesignModel();
            var listCheck = db.ProjectGeneralDesigns.AsNoTracking().Where(i => i.ProjectProductId.Equals(model.ProjectProductId)).ToList();
            if (listCheck.Count() > 0)
            {
                getProjectGeneralDesign = listCheck.Max(i => i.CreateIndex);
                var dataOld = db.ProjectGeneralDesigns.AsNoTracking().FirstOrDefault(i => model.ProjectProductId.Equals(i.ProjectProductId) && i.CreateIndex == getProjectGeneralDesign);
                if (dataOld != null)
                {
                    checkVersion = true;
                    projectGeneralDesignModuleModel.DepartmentCreateId = dataOld.DepartmentCreateId;
                    var departmentCreate = db.Departments.AsNoTracking().FirstOrDefault(i => i.Id.Equals(dataOld.DepartmentCreateId));
                    if (departmentCreate != null)
                    {
                        projectGeneralDesignModuleModel.SBUCreateId = departmentCreate.SBUId;
                    }
                    projectGeneralDesignModuleModel.DepartmentRequestId = dataOld.DepartmentRequestId;
                    var departmentRequest = db.Departments.AsNoTracking().FirstOrDefault(i => i.Id.Equals(dataOld.DepartmentRequestId));
                    if (departmentRequest != null)
                    {
                        projectGeneralDesignModuleModel.SBURequestId = departmentRequest.SBUId;
                    }
                    projectGeneralDesignModuleModel.RequestDate = dataOld.RequestDate;
                    projectGeneralDesignModuleModel.Index = dataOld.Index;
                    projectGeneralDesignModuleModel.DesignBy = dataOld.DesignBy;
                }
            }
            #endregion

            var data = (from a in db.ProjectProducts.AsNoTracking()
                        where a.Id.Equals(model.ProjectProductId)
                        select new ProjectProductsResuldModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            ProductId = a.ProductId,
                            ProjectId = a.ProjectId,
                            ProjectProductId = a.Id,
                            ParentId = a.ParentId,
                            ContractName = a.ContractName,
                            ContractCode = a.ContractCode,
                            ModuleStatus = a.ModuleStatus,
                            Manufacture = "TPA",
                            Quantity = a.Quantity,
                            ContractQuantity = a.Quantity,
                            CheckQuantity = a.Quantity,
                            DataType = a.DataType,
                            DesignStatus = a.DesignStatus,
                            Index = a.ContractIndex,
                            ContractPrice = a.Price,
                            Amount = a.Quantity * a.Price,
                            RealQuantity = a.RealQuantity,
                            Pricing = a.Price,
                            IsGeneralDesign = a.IsGeneralDesign
                        }).ToList();

            ModulePriceInfoModel modulePriceInfoModel;
            List<ProductAccessoriesModel> listMaterial = new List<ProductAccessoriesModel>();
            List<ProjectProductsResuldModel> listModule = new List<ProjectProductsResuldModel>();
            List<ProjectProductsResuldModel> listRs = new List<ProjectProductsResuldModel>();
            List<ProjectProductsResuldModel> listIsGeneralDesignTrue = new List<ProjectProductsResuldModel>();
            List<ProjectProductsResuldModel> listIsGeneralDesignFalse = new List<ProjectProductsResuldModel>();
            List<ProjectProductsModel> listOrderBy = new List<ProjectProductsModel>();
            listModule = data;
            var projectproduct = data.FirstOrDefault();
            if (projectproduct != null)
            {
                if (projectproduct.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    var product = db.Products.AsNoTracking().FirstOrDefault(i => projectproduct.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        projectproduct.ModuleCode = product.Code;
                        projectproduct.ModuleName = product.Name;
                    }

                    #region Lay danh sach Module
                    var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                              join b in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals b.ProjectProductId into ab
                                              from ba in ab.DefaultIfEmpty()
                                              join c in db.ProjectGeneralDesigns.AsNoTracking() on ba.ProjectGeneralDesignId equals c.Id into ac
                                              from ca in ac.DefaultIfEmpty()
                                              where a.ProjectId.Equals(model.ProjectId) && a.DesignStatus != Constants.ProjectProduct_DesignStatus_DesignStatus
                                              orderby a.ContractIndex
                                              select new ProjectProductsModel
                                              {
                                                  Id = a.Id,
                                                  ProjectId = a.ProjectId,
                                                  ParentId = a.ParentId,
                                                  ModuleId = a.ModuleId,
                                                  ProductId = a.ProductId,
                                                  ContractCode = a.ContractCode,
                                                  ContractName = a.ContractName,
                                                  Specifications = a.Specifications,
                                                  DataType = a.DataType,
                                                  ModuleStatus = 1,
                                                  DesignStatus = a.DesignStatus,
                                                  DesignFinishDate = a.DesignFinishDate,
                                                  MakeFinishDate = a.MakeFinishDate,
                                                  DeliveryDate = a.DeliveryDate,
                                                  TransferDate = a.TransferDate,
                                                  Note = ba.Note,
                                                  Quantity = ba != null ? ba.Quantity : a.Quantity,
                                                  ContractQuantity = a.Quantity,
                                                  RealQuantity = ba != null ? ba.RealQuantity : a.RealQuantity,
                                                  Price = a.Price,
                                                  ContractIndex = a.ContractIndex,
                                                  ContractPrice = ba == null ? a.Price : ba.ContractPrice,
                                                  CreateIndex = ca != null ? ca.CreateIndex : 0,
                                                  SaveIndex = ba != null ? ba.Index : 1000,
                                                  ModulePrice = ba != null ? ba.ModulePrice : 0,
                                                  IsGeneralDesign = a.IsGeneralDesign,
                                                  ApproveStatus = ca != null ? ca.ApproveStatus : 0
                                              }).ToList();

                    int index = 1;
                    ProjectProductsSearchModel modelSearch = new ProjectProductsSearchModel();
                    //modelSearch.DataType = Constants.ProjectProduct_DataType_Module;
                    var listModuleProduct = GetProjectProductChild(projectproduct.Id, listProjectProduct, modelSearch, index.ToString());

                    #region OrderBy theo lần lập và hạng mục
                    var listOrderByIndex = listModuleProduct.Where(i => i.SaveIndex != 1000).OrderBy(i => i.SaveIndex).ToList();
                    listOrderBy.AddRange(listOrderByIndex);
                    var listOrderByContractIndex = listModuleProduct.Where(i => i.SaveIndex == 1000).OrderBy(i => i.SaveIndex).ToList();
                    if (listOrderByContractIndex.Count() > 0)
                    {
                        int maxLen = listOrderByContractIndex.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                        Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                        listOrderByContractIndex = listOrderByContractIndex
                                   .Select(s =>
                                       new
                                       {
                                           OrgStr = s,
                                           SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                       })
                                   .OrderBy(x => x.SortStr)
                                   .Select(x => x.OrgStr).ToList();
                    }
                    listOrderBy.AddRange(listOrderByContractIndex);
                    #endregion
                    var materials = db.Materials.AsNoTracking().ToList();
                    foreach (var item in listOrderBy)
                    {
                        //if (!string.IsNullOrEmpty(item.ModuleId))
                        //{
                        var projectGeneralDesignModule = db.ProjectGeneralDesignModules.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectProductId));
                        if (projectGeneralDesignModule != null)
                        {
                            item.ModuleStatus = 2;
                            item.Checked = true;
                        }

                        var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item.ModuleId));
                        if (module != null)
                        {
                            var errors = (from a in db.Errors.AsNoTracking()
                                          join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                          where
                                          a.ObjectId.Equals(item.ModuleId) &&
                                          sbuId.Equals(b.SBUId) && b.IsDesign == true
                                          //&& a.Status > Constants.Problem_Status_Awaiting_Confirm
                                          //&& a.Status < Constants.Problem_Status_Ok_QC
                                          select new { a.Id, a.Status }).ToList();

                            modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.ModuleId);
                            //var amount = item.ModuleStatus == 2 && item.ApproveStatus == 1 ? item.Quantity * item.ModulePrice : item.Quantity * modulePriceInfoModel.Price;
                            var dataQuery = (from a in db.MaterialImportBOMDrafts.AsNoTracking()
                                             where a.ProjectId.Equals(item.Id) && a.ModuleId.Equals(item.ModuleId) && (a.UpdateStatus == 1 || a.UpdateStatus == 0)
                                             orderby a.Code
                                             select new MaterialImportBOMDraftModel
                                             {
                                                 Id = a.Id,
                                                 ModuleId = a.ModuleId,
                                                 MaterialId = a.Id,
                                                 MaterialCode = a.Code,
                                                 MaterialName = a.Name,
                                                 Specification = a.Specification,
                                                 RawMaterialCode = a.RawMaterialCode,
                                                 RawMaterial = a.RawMaterial,
                                                 Pricing = a.Pricing,
                                                 Quantity = a.Quantity,
                                                 ReadQuantity = a.Quantity,
                                                 Weight = a.Weight,
                                                 //ManufacturerId = a.ManufacturerId,
                                                 ManufacturerCode = a.ManufactureCode,
                                                 Note = a.Note,
                                                 UnitName = a.UnitName,
                                                 Index = a.Index,
                                                 Amount = a.Amount
                                             }).ToList();
                            var amount = business.GetPriceModule(dataQuery, materials);

                            listRs.Add(new ProjectProductsResuldModel
                            {
                                Id = module.Id,
                                ModuleId = module.Id,
                                ProjectProductId = item.Id,
                                Manufacture = "TPA",
                                ModuleStatus = item.ModuleStatus,
                                Checked = item.Checked,
                                ContractCode = item.ContractCode,
                                ContractName = item.ContractName,
                                ModuleCode = module.Code,
                                ModuleName = module.Name,
                                Note = item.Note,
                                Quantity = item.Quantity,
                                ContractQuantity = item.ContractQuantity,
                                CheckQuantity = item.Quantity,
                                ContractPrice = item.ContractPrice,
                                //Pricing = item.ModuleStatus == 2 && item.ApproveStatus == 1 ? item.ModulePrice : modulePriceInfoModel.Price,
                                Pricing = amount,
                                TotalError = errors.Where(a => a.Status > Constants.Problem_Status_Awaiting_Confirm && a.Status < Constants.Problem_Status_Ok_QC).Count(),
                                TotalNoDone = errors.Where(a => a.Status < Constants.Problem_Status_Close ).Count(),
                                Amount = item.Quantity * amount,
                                ContractAmount = item.ContractQuantity * item.ContractPrice,
                                CreateIndex = item.CreateIndex,
                                RealQuantity = item.RealQuantity,
                                IsGeneralDesign = item.IsGeneralDesign,
                                IsNoPrice = modulePriceInfoModel.IsNoPrice,
                                StatusUse = module.Status
                            });
                        }
                        else
                        {
                            var amount = item.Quantity * 0;
                            //var errors = db.Errors.Where(i => i.ObjectId.Equals(item.ModuleId)).Count();
                            listRs.Add(new ProjectProductsResuldModel
                            {
                                ProjectProductId = item.Id,
                                Manufacture = "TPA",
                                ModuleStatus = item.ModuleStatus,
                                Checked = item.Checked,
                                ContractCode = item.ContractCode,
                                ContractName = item.ContractName,
                                ContractPrice = item.ContractPrice,
                                Note = item.Note,
                                Quantity = item.Quantity,
                                CheckQuantity = item.Quantity,
                                ContractQuantity = item.ContractQuantity,
                                Pricing = 0,
                                Amount = amount,
                                ContractAmount = item.ContractQuantity * item.ContractPrice,
                                //TotalError = errors,
                                CreateIndex = item.CreateIndex,
                                RealQuantity = item.RealQuantity,
                                IsGeneralDesign = item.IsGeneralDesign,
                                IsNoPrice = true,
                                StatusUse = Constants.Module_Status_Use_One
                            });
                        }
                        //}
                    }

                    listIsGeneralDesignTrue = listRs.Where(i => i.IsGeneralDesign).ToList();
                    totalAmounts = listIsGeneralDesignTrue.Sum(i => i.Amount);
                    totalPriceContract = listIsGeneralDesignTrue.Sum(i => i.ContractAmount);
                    listIsGeneralDesignFalse = listRs.Where(i => !i.IsGeneralDesign).ToList();

                    #endregion
                    #region Lay danh sach Material
                    if (getProjectGeneralDesign > 0)
                    {
                        var listMaterialDesign = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                  join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                  where b.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                                  join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id
                                                  join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                                                  join e in db.Units.AsNoTracking() on c.UnitId equals e.Id
                                                  where a.ProjectProductId.Equals(model.ProjectProductId) && a.CreateIndex == getProjectGeneralDesign
                                                  orderby c.Code
                                                  select new ProductAccessoriesModel
                                                  {
                                                      Id = b.MaterialId,
                                                      Name = c.Name,
                                                      Code = c.Code,
                                                      Manafacture = d.Code,
                                                      Quantity = (int)b.Quantity,
                                                      OldQuantity = (int)b.Quantity,
                                                      Inventoty = b.Inventoty,
                                                      ContractPrice = b.ContractPrice,
                                                      Price = c.Pricing,
                                                      IsDelete = true,
                                                      UnitName = e.Name,
                                                      Type = b.Type,
                                                      StatusUse = c.Status
                                                  }).ToList();

                        var listModuleDesign = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                where b.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                                join c in db.Modules.AsNoTracking() on b.MaterialId equals c.Id
                                                where a.ProjectProductId.Equals(model.ProjectProductId) && a.CreateIndex == getProjectGeneralDesign
                                                orderby c.Code
                                                select new ProductAccessoriesModel
                                                {
                                                    Id = b.MaterialId,
                                                    Name = c.Name,
                                                    Code = c.Code,
                                                    Manafacture = Constants.Manufacture_TPA,
                                                    Quantity = (int)b.Quantity,
                                                    OldQuantity = (int)b.Quantity,
                                                    Inventoty = b.Inventoty,
                                                    ContractPrice = b.ContractPrice,
                                                    Price = c.Pricing,
                                                    IsDelete = true,
                                                    UnitName = Constants.Unit_Bo,
                                                    Type = b.Type,
                                                    ModuleStatusUse = c.Status
                                                }).ToList();

                        listMaterial.AddRange(listMaterialDesign);
                        listMaterial.AddRange(listModuleDesign);
                    }
                    else
                    {
                        listMaterial = (from a in db.ProductAccessories.AsNoTracking()
                                        join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                                        join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                        join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                                        join e in db.Units.AsNoTracking() on c.UnitId equals e.Id
                                        where a.ProductId.Equals(projectproduct.ProductId)
                                        orderby c.Code
                                        select new ProductAccessoriesModel
                                        {
                                            Id = a.MaterialId,
                                            Code = c.Code,
                                            Name = c.Name,
                                            ProductId = a.ProductId,
                                            MaterialId = a.MaterialId,
                                            Manafacture = d.Code,
                                            Quantity = a.Quantity,
                                            Price = a.Price,
                                            Note = a.Note,
                                            TotalError = 0,
                                            UnitName = e.Name,
                                            Type = Constants.ProjectGaneralDesignMaterial_Type_Material,
                                            StatusUse = c.Status
                                        }).ToList();
                    }
                    #endregion
                }
                else if (projectproduct.DataType == Constants.ProjectProduct_DataType_Module || projectproduct.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    if (projectproduct.DesignStatus != Constants.ProjectProduct_DesignStatus_DesignStatus)
                    {
                        var module = db.Modules.AsNoTracking().FirstOrDefault(i => projectproduct.ModuleId.Equals(i.Id));
                        if (module != null)
                        {
                            var errors = (from a in db.Errors.AsNoTracking()
                                          join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                          where
                                          a.ObjectId.Equals(module.Id) &&
                                          sbuId.Equals(b.SBUId) && b.IsDesign == true &&
                                          a.Status != Constants.Problem_Status_Creating &&
                                          a.Status != Constants.Problem_Status_Awaiting_Confirm &&
                                          a.Status != Constants.Problem_Status_Ok_QC
                                          select new { a.Id, a.Status }).ToList();
                            projectproduct.TotalError = errors.Where(a => a.Status > Constants.Problem_Status_Awaiting_Confirm && a.Status < Constants.Problem_Status_Ok_QC).Count();
                            projectproduct.TotalNoDone = errors.Where(a => a.Status == Constants.Problem_Status_Ok_QC).Count();

                            var projectGeneralDesignModule = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                              join b in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                              where b.ProjectProductId.Equals(model.ProjectProductId) && b.ModuleId.Equals(module.Id)
                                                              select new ProjectGeneralDesignModuleModel
                                                              {
                                                                  ContractPrice = b.ContractPrice,
                                                                  Quantity = b.Quantity,
                                                                  ProjectGeneralDesignId = b.ProjectGeneralDesignId,
                                                                  ModulePrice = b.ModulePrice,
                                                                  ApproveStatus = a.ApproveStatus
                                                              }).FirstOrDefault();
                            //db.ProjectGeneralDesignModules.FirstOrDefault(i => i.ModuleId.Equals(module.Id) && i.ProjectProductId.Equals(model.ProjectProductId));
                            if (projectGeneralDesignModule != null)
                            {
                                projectproduct.ModuleStatus = 2;
                                projectproduct.ApproveStatus = projectGeneralDesignModule.ApproveStatus;
                                projectproduct.Checked = true;
                                projectproduct.ContractPrice = projectGeneralDesignModule.ContractPrice;
                                projectproduct.Quantity = projectGeneralDesignModule.Quantity;
                                var projectGeneralDesign = db.ProjectGeneralDesigns.AsNoTracking().FirstOrDefault(i => i.Id.Equals(projectGeneralDesignModule.ProjectGeneralDesignId));
                                if (projectGeneralDesign != null)
                                {
                                    projectproduct.CreateIndex = projectGeneralDesign.CreateIndex;
                                }
                            }
                            else
                            {
                                projectproduct.ContractPrice = projectproduct.Pricing;
                                projectproduct.ModuleStatus = 1;
                                projectproduct.Checked = false;
                            }

                            decimal totalAmount = 0;
                            if (projectproduct.ModuleStatus == 2 && projectproduct.ApproveStatus == 1)
                            {
                                projectproduct.Pricing = projectGeneralDesignModule.ModulePrice;
                                totalAmount = projectproduct.Pricing;
                            }
                            else
                            {
                                modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(projectproduct.ModuleId);
                                projectproduct.Pricing = modulePriceInfoModel.Price;
                                projectproduct.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                                totalAmount = modulePriceInfoModel.Price;
                            }

                            projectproduct.ModuleCode = module.Code;
                            projectproduct.ModuleName = module.Name;
                            totalAmounts = projectproduct.Quantity * projectproduct.Pricing;
                            totalPriceContract = projectproduct.ContractPrice;
                            projectproduct.TotalAmount = totalAmounts;
                            projectproduct.TotalPriceContract = totalPriceContract;
                            projectproduct.ContractQuantity = projectproduct.Quantity;
                            projectproduct.ContractAmount = projectproduct.ContractQuantity * projectproduct.ContractPrice;
                            projectproduct.Amount = projectproduct.Quantity * projectproduct.Pricing;
                            projectproduct.Id = module.Id;
                        }
                        else
                        {
                            var checkTH = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                           join b in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                           where a.Id.Equals(model.Id) && b.ProjectProductId.Equals(model.ProjectProductId)
                                           select new { a.CreateIndex, b.ContractPrice, b.Quantity }).FirstOrDefault();
                            if (checkTH != null)
                            {
                                projectproduct.ModuleStatus = 2;
                                projectproduct.Checked = true;
                                projectproduct.ContractPrice = projectproduct.Pricing;
                                projectproduct.ContractAmount = projectproduct.ContractPrice * projectproduct.Quantity;
                                projectproduct.Quantity = checkTH.Quantity;
                                projectproduct.CreateIndex = checkTH.CreateIndex;
                            }
                            else
                            {
                                projectproduct.ContractAmount = projectproduct.Pricing * projectproduct.Quantity;
                            }
                        }
                    }

                    // Lấy vật tư lần tổng hợp trước
                    if (getProjectGeneralDesign > 0)
                    {
                        var listMaterialDesign = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                  join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                  where b.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                                  join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id
                                                  join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                                                  join e in db.Units.AsNoTracking() on c.UnitId equals e.Id
                                                  where a.ProjectProductId.Equals(model.ProjectProductId) && a.CreateIndex == getProjectGeneralDesign
                                                  orderby c.Code
                                                  select new ProductAccessoriesModel
                                                  {
                                                      Id = b.MaterialId,
                                                      Name = c.Name,
                                                      Code = c.Code,
                                                      Manafacture = d.Code,
                                                      Quantity = (int)b.Quantity,
                                                      OldQuantity = (int)b.Quantity,
                                                      Inventoty = b.Inventoty,
                                                      ContractPrice = b.ContractPrice,
                                                      Price = c.Pricing,
                                                      IsDelete = true,
                                                      UnitName = e.Name,
                                                      Type = b.Type,
                                                      StatusUse = c.Status
                                                  }).ToList();

                        var listModuleDesign = (from a in db.ProjectGeneralDesigns.AsNoTracking()
                                                join b in db.ProjectGeneralDesignMaterials.AsNoTracking() on a.Id equals b.ProjectGeneralDesignId
                                                where b.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                                join c in db.Modules.AsNoTracking() on b.MaterialId equals c.Id
                                                where a.ProjectProductId.Equals(model.ProjectProductId) && a.CreateIndex == getProjectGeneralDesign
                                                orderby c.Code
                                                select new ProductAccessoriesModel
                                                {
                                                    Id = b.MaterialId,
                                                    Name = c.Name,
                                                    Code = c.Code,
                                                    Manafacture = Constants.Manufacture_TPA,
                                                    Quantity = (int)b.Quantity,
                                                    OldQuantity = (int)b.Quantity,
                                                    Inventoty = b.Inventoty,
                                                    ContractPrice = b.ContractPrice,
                                                    Price = c.Pricing,
                                                    IsDelete = true,
                                                    UnitName = Constants.Unit_Bo,
                                                    Type = b.Type,
                                                    ModuleStatusUse = c.Status
                                                }).ToList();

                        listMaterial.AddRange(listMaterialDesign);
                        listMaterial.AddRange(listModuleDesign);
                    }
                }
            }
            var createIndex = 1;
            var indexList = db.ProjectGeneralDesigns.Where(i => i.ProjectProductId.Equals(projectproduct.ProjectProductId)).ToList().Select(i => i.CreateIndex);
            if (indexList.Count() > 0)
            {
                createIndex = indexList.Max() + 1;
            }
            return new GetDataModel
            {
                Data = projectproduct,
                CreateIndex = createIndex,
                ListModule = listModule,
                ListModuleProduct = listIsGeneralDesignTrue,
                ListModuleProductFalse = listIsGeneralDesignFalse,
                ListMaterial = listMaterial.OrderBy(i => i.Code).ToList(),
                TotalAmount = totalAmounts,
                TotalPriceContract = totalPriceContract,
                CheckVersion = checkVersion,
                Models = projectGeneralDesignModuleModel
            };
        }

        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             join c in db.Units.AsNoTracking() on a.UnitId equals c.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a.ManufactureId,
                                 ManufactureName = b.Code,
                                 UnitName = c.Name,
                                 Price = a.Pricing,
                                 Note = a.Note
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
            {
                dataQuery = dataQuery.Where(a => a.ManufactureId.Equals(modelSearch.ManufactureId));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            int i = 1;

            foreach (var item in listResult)
            {
                item.Index = i;
                i++;
            }
            return searchResult;
        }

        public object GetListDepartment(string SBU)
        {
            var dataQuery = (from a in db.Departments.AsNoTracking()
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id
                             orderby a.Name
                             select new DepartmentModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 SBUId = a.SBUId,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(SBU))
            {
                dataQuery = dataQuery.Where(i => i.SBUId.Equals(SBU));
            }
            var data = dataQuery.ToList();
            return data;
        }

        public object GetData(string employeeId)
        {
            var data = (from a in db.Employees.AsNoTracking()
                        join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                        join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                        where a.Id.Equals(employeeId)
                        select new EmployeeModel
                        {
                            Id = a.Id,
                            DepartmantId = b.Id,
                            SBUID = c.Id
                        }).FirstOrDefault();
            return data;
        }

        public string ExpoetGeneralDesign(ProjectGeneralDesignModel model)
        {
            var checkError = model.ListModule.FirstOrDefault(i => i.ModuleStatus == 2 && i.TotalError > 0);
            if (checkError != null)
            {
                throw NTSException.CreateInstance("Sản phẩm vẫn còn lỗi. Không thể xuất biểu mẫu!");
            }

            var moduleNotUse = model.ListModule.Where(i => i.Status == Constants.Module_Status_Not_Use).Select(s => s.ModuleCode).ToList();
            if (moduleNotUse.Count > 0)
            {
                throw NTSException.CreateInstance($"Sản phẩm < {string.Join(";", moduleNotUse)} > đã ngừng sử dụng. Không thể xuất biểu mẫu!");
            }

            var materialNotUse = model.ListMaterial.Where(i => i.ModuleStatusUse == Constants.Module_Status_Not_Use || (i.ModuleStatusUse == 0 && !Constants.Material_Status_Use.Equals(i.Status))).Select(s => s.Code).ToList();
            if (materialNotUse.Count > 0)
            {
                throw NTSException.CreateInstance($"Vật tư < {string.Join(";", materialNotUse)} > đã ngừng sử dụng. Không thể xuất biểu mẫu!");
            }


            var customer = (from a in db.Projects.AsNoTracking()
                            join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                            where a.Id.Equals(model.ProjectId)
                            select new { b.Name, a.Code, b.Contact }).FirstOrDefault();
            var createIndex = db.ProjectGeneralDesigns.AsNoTracking().Where(i => model.ProjectProductId.Equals(i.ProjectProductId) && model.CreateIndex >= i.CreateIndex).ToList();
            var deparmentRequestId = "";
            var deparmentRequest = db.Departments.AsNoTracking().FirstOrDefault(i => model.DepartmentRequestId.Equals(i.Id));
            if (deparmentRequest == null)
            {
                deparmentRequestId = "";
            }
            else
            {
                deparmentRequestId = deparmentRequest.Name;
            }

            var deparmentPerformId = "";
            var deparmentPerform = db.Departments.AsNoTracking().FirstOrDefault(i => model.DepartmentCreateId.Equals(i.Id));
            if (deparmentPerform == null)
            {
                deparmentPerformId = "";
            }
            else
            {
                deparmentPerformId = deparmentPerform.Name;
            }

            var employee = "";
            var employees = db.Employees.AsNoTracking().FirstOrDefault(i => model.DesignBy.Equals(i.Id));
            if (employees == null)
            {
                employee = "";
            }
            else
            {
                employee = employees.Name;
            }

            DateTime? finishdate;
            var finishdates = db.ProjectProducts.AsNoTracking().FirstOrDefault(i => model.ProjectProductId.Equals(i.Id));
            if (finishdates == null)
            {
                finishdate = null;
            }
            else
            {
                finishdate = finishdates.DesignFinishDate != null ? finishdates.DesignFinishDate : null;
            }

            var data = (from a in db.ProjectProducts.AsNoTracking()
                        where a.Id.Equals(model.ProjectProductId)
                        select new ProjectProductsResuldModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            ProductId = a.ProductId,
                            ProjectId = a.ProjectId,
                            ContractName = a.ContractName,
                            ContractCode = a.ContractCode,
                            DataType = a.DataType
                        }).FirstOrDefault();

            if (data != null)
            {
                if (data.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    var product = db.Products.AsNoTracking().FirstOrDefault(i => data.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        data.ModuleCode = product.Code;
                        data.ModuleName = product.Name;
                    }
                }
                else if (data.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    var module = db.Modules.AsNoTracking().FirstOrDefault(i => data.ModuleId.Equals(i.Id));
                    if (module != null)
                    {
                        data.ModuleCode = module.Code;
                        data.ModuleName = module.Name;
                    }
                }
            }

            var moduleNopPrice = model.ListModule.Where(r => r.ModuleStatus == 2 && r.IsNoPrice && r.CreateIndex == model.CreateIndex).ToList();
            if (moduleNopPrice.Count > 0)
            {
                return ExportMaterialNoPrice(moduleNopPrice, data.ContractCode);
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TongHopThietKe_Template.xlsm"));

            IWorksheet sheet = workbook.Worksheets[0];

            if (customer != null)
            {
                IRange iRangeDataKHCuoi = sheet.FindFirst("<khachhangcuoi>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataKHCuoi.Text = iRangeDataKHCuoi.Text.Replace("<khachhangcuoi>", customer.Name);
                //sheet.Replace("Tên KH cuối:", "Tên KH cuối: " + customer.Name);
                IRange iRangeMaDA = sheet.FindFirst("<mada>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeMaDA.Text = iRangeMaDA.Text.Replace("<mada>", customer.Code);
                //sheet.Replace("Mã DA: ", "Mã DA: " + customer.Code);
                IRange iRangePTHĐ = sheet.FindFirst("<phutrachHĐ>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangePTHĐ.Text = iRangePTHĐ.Text.Replace("<phutrachHĐ>", customer.Contact);
                //sheet.Replace("Phụ trách HĐ:", "Phụ trách HĐ: " + customer.Contact);
            }
            IRange iRangeSTHTK = sheet.FindFirst("<sothtk>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            IRange iRangeSHĐ = sheet.FindFirst("<sohd>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeSTHTK.Text = iRangeSTHTK.Text.Replace("<sothtk>", "[" + model.Index + "]-[" + data.ContractCode + "]-[PTK]/[" + model.CreateIndex + "]");
            iRangeSHĐ.Text = iRangeSHĐ.Text.Replace("<sohd>", "[" + model.Index + "]-[" + data.ContractCode + "]-[PTK]/[" + model.CreateIndex + "]");
            //sheet.Replace("Số HĐ:", "Số HĐ: [" + model.Index + "]-[" + data.ContractCode + "]-[PTK]/[" + model.CreateIndex + "]");
            IRange iRangeBPYC = sheet.FindFirst("<bophanyeucau>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeBPYC.Text = iRangeBPYC.Text.Replace("<bophanyeucau>", deparmentRequestId);
            //sheet.Replace("Bộ Phận YC:", "Bộ Phận YC: " + deparmentRequestId);
            if (model.RequestDate != null)
            {
                IRange iRangeNYC = sheet.FindFirst("<ngayyc>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeNYC.Text = iRangeNYC.Text.Replace("<ngayyc>", DateTimeHelper.ToStringDDMMYY(model.RequestDate));
                //sheet.Replace("Ngày YC:", "Ngày YC: " + DateTimeHelper.ToStringDDMMYY(model.RequestDate));
            }
            IRange iRangeBPTH = sheet.FindFirst("<bophanthuchien>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeBPTH.Text = iRangeBPTH.Text.Replace("<bophanthuchien>", deparmentPerformId);
            //sheet.Replace("Bộ Phận TH: ", "Bộ Phận TH: " + deparmentPerformId);
            IRange iRangeNgayHT = sheet.FindFirst("<ngayht>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeNgayHT.Text = iRangeNgayHT.Text.Replace("<ngayht>", finishdate != null ? DateTimeHelper.ToStringDDMMYY(finishdate.Value) : "");
            //sheet.Replace("Ngày HT:", "Ngày HT: " + DateTimeHelper.ToStringDDMMYY(finishdate.Value));
            IRange iRangePTTK = sheet.FindFirst("<phutrachTK>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangePTTK.Text = iRangePTTK.Text.Replace("<phutrachTK>", employee);
            //sheet.Replace("Phụ trách TK:", "Phụ trách TK: " + employee);
            IRange iRangeTenSPHD = sheet.FindFirst("<tensanphamHĐ>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeTenSPHD.Text = iRangeTenSPHD.Text.Replace("<tensanphamHĐ>", data.ContractName);
            //sheet.Replace("Tên sản phẩm theo hợp đồng: ", "Tên sản phẩm theo hợp đồng: " + data.ContractName);
            IRange iRangeMaSPHD = sheet.FindFirst("<masanphamHĐ>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeMaSPHD.Text = iRangeMaSPHD.Text.Replace(" <masanphamHĐ>", data.ContractCode);
            //sheet.Replace("Mã SP theo hợp đồng:", "Mã SP theo hợp đồng: " + data.ContractCode);
            IRange iRangeTenSPTK = sheet.FindFirst("<tensanphamTK>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeTenSPTK.Text = iRangeTenSPTK.Text.Replace("<tensanphamTK>", data.ModuleName);
            //sheet.Replace("Tên sản phẩm theo thiết kế:", "Tên sản phẩm theo thiết kế: " + data.ModuleName);
            IRange iRangeMaSPTK = sheet.FindFirst("<masanphamTK>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeMaSPTK.Text = iRangeMaSPTK.Text.Replace("<masanphamTK>", data.ModuleCode);
            //sheet.Replace("Mã SP theo thiết kế:", "Mã SP theo thiết kế: " + data.ModuleCode);
            IRange iRangeHM = sheet.FindFirst("<index>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeHM.Text = iRangeHM.Text.Replace("<index>", model.Index);
            //sheet.Replace("Hạng mục:", "Hạng mục: " + model.Index);

            if (createIndex.Count() > 0)
            {
                DateTime? create1 = null;
                DateTime? create2 = null;
                DateTime? create3 = null;
                DateTime? create4 = null;
                DateTime? create5 = null;
                if (createIndex.FirstOrDefault(i => i.CreateIndex == 1) != null)
                {
                    create1 = createIndex.FirstOrDefault(i => i.CreateIndex == 1).CreateDate;
                }
                if (createIndex.FirstOrDefault(i => i.CreateIndex == 2) != null)
                {
                    create2 = createIndex.FirstOrDefault(i => i.CreateIndex == 2).CreateDate;
                }
                if (createIndex.FirstOrDefault(i => i.CreateIndex == 3) != null)
                {
                    create3 = createIndex.FirstOrDefault(i => i.CreateIndex == 3).CreateDate;
                }
                if (createIndex.FirstOrDefault(i => i.CreateIndex == 4) != null)
                {
                    create4 = createIndex.FirstOrDefault(i => i.CreateIndex == 4).CreateDate;
                }
                if (createIndex.FirstOrDefault(i => i.CreateIndex == 5) != null)
                {
                    create5 = createIndex.FirstOrDefault(i => i.CreateIndex == 5).CreateDate;
                }
                IRange iRangecreate1 = sheet.FindFirst("<ngaylan1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangecreate1.Text = iRangecreate1.Text.Replace("<ngaylan1>", create1 != null ? DateTimeHelper.ToStringDDMMYY(create1.Value) : "");
                //sheet.Replace("Lần 1 ngày:", "Lần 1 ngày: " + create1 != null ? DateTimeHelper.ToStringDDMMYY(create1) : "");
                IRange iRangecreate2 = sheet.FindFirst("<ngaylan2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangecreate2.Text = iRangecreate2.Text.Replace("<ngaylan2>", create2 != null ? DateTimeHelper.ToStringDDMMYY(create2.Value) : "");
                //sheet.Replace("Lần 2 ngày:", "Lần 2 ngày: " + create2 != null ? DateTimeHelper.ToStringDDMMYY(create2) : "");
                IRange iRangecreate3 = sheet.FindFirst("<ngaylan3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangecreate3.Text = iRangecreate3.Text.Replace("<ngaylan3>", create3 != null ? DateTimeHelper.ToStringDDMMYY(create3.Value) : "");
                //sheet.Replace("Lần 3 ngày:", "Lần 3 ngày: " + create3 != null ? DateTimeHelper.ToStringDDMMYY(create3) : "");
                IRange iRangecreate4 = sheet.FindFirst("<ngaylan4>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangecreate4.Text = iRangecreate4.Text.Replace("<ngaylan4>", create4 != null ? DateTimeHelper.ToStringDDMMYY(create4.Value) : "");
                //sheet.Replace("Lần 4 ngày:", "Lần 4 ngày: " + create4 != null ? DateTimeHelper.ToStringDDMMYY(create4) : "");
                IRange iRangecreate5 = sheet.FindFirst("<ngaylan5>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangecreate5.Text = iRangecreate5.Text.Replace("<ngaylan5>", create5 != null ? DateTimeHelper.ToStringDDMMYY(create5.Value) : "");
                //sheet.Replace("Lần 5 ngày:", "Lần 5 ngày: " + create5 != null ? DateTimeHelper.ToStringDDMMYY(create5) : "");
            }

            var Time = DateTime.Now;
            IRange iRangeData = sheet.FindFirst("<Data1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data1>", string.Empty);
            //IRange iRangeData2 = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //iRangeData2.Text = iRangeData2.Text.Replace("<day>", Time.Day.ToString());
            //IRange iRangeData3 = sheet.FindFirst("<month>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //iRangeData3.Text = iRangeData3.Text.Replace("<month>", Time.Month.ToString());
            //IRange iRangeData4 = sheet.FindFirst("<year>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //iRangeData4.Text = iRangeData4.Text.Replace("<year>", Time.Year.ToString());
            var total = model.ListModule.Count();
            var total1 = model.ListMaterial.Count();
            //iRangeData1.Text = iRangeData1.Text.Replace("<data>", string.Empty);
            if (model.ListModule.Count > 0)
            {
                var listExport = model.ListModule.Select((a, i) => new
                {
                    index = i + 1,
                    a.ContractName,
                    a.ModuleName,
                    a.ContractCode,
                    a.ModuleCode,
                    a.Manufacture,
                    view10 = a.CreateIndex <= model.CreateIndex ? a.Quantity : 1,
                    a.TotalError,
                    a.Pricing,
                    a.Amount,
                    a.ContractQuantity,
                    a.ContractPrice,
                    view = a.ContractQuantity * a.ContractPrice,
                    view11 = a.CreateIndex <= model.CreateIndex ? a.Note : "",
                    view12 = a.CreateIndex == 1 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view13 = a.CreateIndex == 2 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view14 = a.CreateIndex == 3 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view15 = a.CreateIndex == 4 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view16 = a.CreateIndex == 5 && a.CreateIndex <= model.CreateIndex ? "X" : ""
                });

                if (listExport.Count() > 2)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                //sheet.InsertRow(17 + total, total1 - 2, ExcelInsertOptions.FormatAsBefore);
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 16].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 16].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 16].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 16].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 16].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 16].CellStyle.WrapText = true;
            }

            IRange iRangeData1 = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData1.Text = iRangeData1.Text.Replace("<data>", string.Empty);
            if (model.ListMaterial.Count > 0)
            {
                var listMaterial = model.ListMaterial.Select((a, i) => new
                {
                    index = i + 1,
                    view2 = "",
                    a.Name,
                    view3 = "",
                    a.Code,
                    a.Manafacture,
                    a.Quantity,
                    a.UnitName,
                    a.Price,
                    view9 = "",
                    view10 = "",
                    a.ContractPrice,
                    view11 = "",
                    a.Note,
                    view12 = a.CreateIndex == 1 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view13 = a.CreateIndex == 2 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view14 = a.CreateIndex == 3 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view15 = a.CreateIndex == 4 && a.CreateIndex <= model.CreateIndex ? "X" : "",
                    view16 = a.CreateIndex == 5 && a.CreateIndex <= model.CreateIndex ? "X" : ""
                });

                if (listMaterial.Count() > 2)
                {
                    sheet.InsertRow(iRangeData1.Row + 1, listMaterial.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                //sheet.InsertRow(17 + total, total1 - 2, ExcelInsertOptions.FormatAsBefore);
                sheet.ImportData(listMaterial, iRangeData1.Row, iRangeData1.Column, false);
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData1.Row - 1, 1, iRangeData1.Row + total1 - 1, 14].CellStyle.WrapText = true;
            }

            string folderName = DateTime.Now.ToStringYYMMDDHHMMSSFFF();
            string folderPath = "Template/" + Constants.FolderExport + folderName;
            string localPath = HttpContext.Current.Server.MapPath("~/" + folderPath);
            Directory.CreateDirectory(localPath);

            string pathreturn = folderPath + "/THTK-" + data.ContractCode + "- Lan " + model.CreateIndex + ".xlsm";
            string pathFileSave = HttpContext.Current.Server.MapPath("~/" + pathreturn);

            workbook.SaveAs(pathFileSave);
            workbook.Close();

            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            foreach (var module in model.ListModule)
            {
                if (module.CreateIndex == model.CreateIndex && !string.IsNullOrEmpty(module.ModuleId))
                {
                    moduleMaterialBusiness.ExportExcelMaterialBOMDraft(new ModuleMaterialSearchModel { ModuleId = module.ModuleId,ProjectProductId = model.ProjectProductId }, folderName);
                }
            }

            string fileZipName = ZipHelper.ZipFileInFolder(localPath, data.ContractCode + "_" + model.CreateIndex, data.ContractCode + "_" + model.CreateIndex);

            return folderPath + "/" + fileZipName;
        }

        /// <summary>
        /// Xuất danh sách vật tư thiếu giá
        /// </summary>
        /// <param name="ListModule"></param>
        /// <returns></returns>
        private string ExportMaterialNoPrice(List<ProjectGeneralDesignModuleModel> modules, string contractCode)
        {
            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            List<ModuleMaterialResultModel> materials = new List<ModuleMaterialResultModel>();
            foreach (var module in modules)
            {
                materials.AddRange(moduleMaterialBusiness.GetMaterialNoPriceByModuleId(module.ModuleId));
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/DanhSachVatTuThieuGia.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = materials.Select((a, i) => new
            {
                SupplierCode = db.Suppliers.FirstOrDefault(s => s.Id.Equals(db.SupplierManufactures.FirstOrDefault(sm => sm.ManufactureId.Equals(a.ManufacturerId)).SupplierId))?.Code,
                a.MaterialName,
                a.MaterialCode,
                BuyDate = string.Empty,
                Quantity = 1,
                Price = 0,
                Unit = string.Empty,
                Amount = 0,
                a.ModuleCode
            });

            if (materials.Count > 0)
            {
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + materials.Count - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + materials.Count - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + materials.Count - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + materials.Count - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + materials.Count - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + materials.Count - 1, 9].CellStyle.WrapText = true;
            }
            string resultPathClient = "Template/" + Constants.FolderExport + contractCode + ".xlsx";

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;
        }

        private List<ProjectProductsModel> GetProjectProductChild(string parentId, List<ProjectProductsModel> listProjectProduct, ProjectProductsSearchModel modelSearch, string index)
        {
            List<int> listDataType = new List<int> { Constants.ProjectProduct_DataType_Module, Constants.ProjectProduct_DataType_Paradigm };
            List<ProjectProductsModel> listResult = new List<ProjectProductsModel>();
            var listChild = listProjectProduct.Where(r => parentId.Equals(r.ParentId)).ToList();
            bool isSearch = false;
            int indexChild = 1;
            List<ProjectProductsModel> listChildChild = new List<ProjectProductsModel>();

            foreach (var child in listChild)
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.ContractName) && !child.ContractName.ToLower().Contains(modelSearch.ContractName.ToLower()))
                {
                    isSearch = false;
                }
                if (!string.IsNullOrEmpty(modelSearch.ContractCode) && !child.ContractCode.ToLower().Contains(modelSearch.ContractCode.ToLower()))
                {
                    isSearch = false;
                }
                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    if (string.IsNullOrEmpty(child.Code) || !child.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                    {
                        isSearch = false;
                    }
                }
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    if (string.IsNullOrEmpty(child.Name) || !child.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                    {
                        isSearch = false;
                    }
                }
                if (modelSearch.DataType != 0 && !listDataType.Contains(child.DataType))
                {
                    isSearch = false;
                }
                if (modelSearch.ModuleStatus != 0 && !child.ModuleStatus.Equals(modelSearch.ModuleStatus))
                {
                    isSearch = false;
                }
                if (modelSearch.DesignStatus != 0 && !child.DesignStatus.Equals(modelSearch.DesignStatus))
                {
                    isSearch = false;
                }

                listChildChild = GetProjectProductChild(child.Id, listProjectProduct, modelSearch, index + "." + indexChild);
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

        public string ExportExcelManage(ProjectGeneralDesignModel model)
        {
            string pathExcel = "";
            var data = GetProjectGeneralDesignInfo(model);
            var module = GeneralDesign(model);
            if (module.Data.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
            {
                foreach (var item in module.ListModuleProduct)
                {
                    data.ListModule.Add(new ProjectGeneralDesignModuleModel
                    {
                        ModuleId = item.ModuleId,
                        CreateIndex = item.CreateIndex,
                        ContractName = item.ContractName,
                        ModuleName = item.ModuleName,
                        ContractCode = item.ContractCode,
                        ModuleCode = item.ModuleCode,
                        Manufacture = item.Manufacture,
                        Quantity = Convert.ToInt32(item.Quantity),
                        ContractQuantity = Convert.ToInt32(item.ContractQuantity),
                        TotalError = item.TotalError,
                        Pricing = item.Pricing,
                        Amount = item.Amount,
                        ContractPrice = item.ContractPrice,
                        Note = item.Note,
                        IsNoPrice = item.IsNoPrice,
                        ModuleStatus = item.ModuleStatus,
                        Status = item.StatusUse
                    });
                }

                // List Module không được tích chọn thiết kế
                foreach (var item in module.ListModuleProductFalse)
                {
                    data.ListMaterial.Add(new ProjectGeneralDesignMaterialsModel
                    {

                        Id = item.Id,
                        Name = item.ContractName,
                        Code = item.ContractCode,
                        Manafacture = Constants.Manufacture_TPA,
                        Quantity = (int)item.Quantity,
                        OldQuantity = (int)item.Quantity,
                        //Inventoty = b.Inventoty,
                        ContractPrice = item.ContractPrice,
                        //Price = c.Pricing,
                        //IsDelete = true,
                        UnitName = Constants.Unit_Bo,
                        ModuleStatusUse = item.StatusUse,


                        //Type = b.Type
                    });
                }
            }
            else if (module.Data.DataType == Constants.ProjectProduct_DataType_Module || module.Data.DataType == Constants.ProjectProduct_DataType_Paradigm)
            {
                foreach (var item in module.ListModule)
                {
                    data.ListModule.Add(new ProjectGeneralDesignModuleModel
                    {
                        ModuleId = item.ModuleId,
                        CreateIndex = item.CreateIndex,
                        ContractName = item.ContractName,
                        ModuleName = item.ModuleName,
                        ContractCode = item.ContractCode,
                        ModuleCode = item.ModuleCode,
                        Manufacture = item.Manufacture,
                        Quantity = Convert.ToInt32(item.Quantity),
                        ContractQuantity = Convert.ToInt32(item.ContractQuantity),
                        TotalError = item.TotalError,
                        Pricing = item.Pricing,
                        Amount = item.Amount,
                        ContractPrice = item.ContractPrice,
                        Note = item.Note,
                        IsNoPrice = item.IsNoPrice,
                        ModuleStatus = item.ModuleStatus,
                        Status = item.StatusUse
                    });
                    //}
                }
            }
            //var moduleErrors = data.ListModule.Where(t => t.TotalError > 0).ToList();
            //if (moduleErrors.Count > 0)
            //{
            //    pathExcel = "ModuleError";
            //}
            //else
            //{
            pathExcel = ExpoetGeneralDesign(data);
            //}
            return pathExcel;
        }

        public string ExportExcelBOM(ProjectGeneralDesignModel model)
        {
            string pathExcel = "";
            var data = GetProjectGeneralDesignInfo(model);
            var module = GeneralDesign(model);
            DesignModuleInfoModel designModuleInfoModel = new DesignModuleInfoModel();
            designModuleInfoModel.ProjectId = model.ProjectId;
            designModuleInfoModel.ProjectProductId = model.ProjectProductId;
            designModuleInfoModel.ListMaterial = data.ListMaterial;
            designModuleInfoModel.CreateIndex = data.CreateIndex;
            foreach (var item in module.ListModuleProductFalse)
            {
                if (item.Checked == true && item.CreateIndex == data.CreateIndex)
                {
                    designModuleInfoModel.ListMaterial.Add(new ProjectGeneralDesignMaterialsModel
                    {
                        Id = item.Id,
                        Name = item.ContractName,
                        Code = item.ContractCode,
                        Quantity = (int)item.Quantity,
                        Price = item.ContractPrice,
                        Manafacture = item.Manufacture,
                        Parameter = Constants.Parameter_TPA,
                        Type = Constants.PracticeSupMaterial_Type_Module
                    });
                }
            }

            if (module.Data.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
            {
                foreach (var item in module.ListModuleProduct)
                {
                    if (item.Checked == true && item.CreateIndex == data.CreateIndex)
                    {
                        designModuleInfoModel.Modules.Add(new DesignModuleModel
                        {
                            Id = item.ModuleId,
                            //CreateIndex = item.CreateIndex,
                            //ContractName = item.ContractName,
                            ModuleName = item.ModuleName,
                            //ContractCode = item.ContractCode,
                            ModuleCode = item.ModuleCode,
                            //Manufacture = item.Manufacture,
                            Quantity = item.Quantity,
                            RealQuantity = item.RealQuantity,
                            Parameter = Constants.Parameter_TPA,
                            //TotalError = item.TotalError,
                            //Pricing = item.Pricing,
                            //Amount = item.Amount, 
                            //ContractPrice = item.ContractPrice,
                            //Note = item.Note
                        });
                    }
                }
            }
            else if (module.Data.DataType == Constants.ProjectProduct_DataType_Module || module.Data.DataType == Constants.ProjectProduct_DataType_Paradigm)
            {
                foreach (var item in module.ListModule)
                {
                    if (item.Checked == true && item.CreateIndex == data.CreateIndex)
                    {
                        designModuleInfoModel.Modules.Add(new DesignModuleModel
                        {
                            Id = item.ModuleId,
                            //CreateIndex = item.CreateIndex,
                            //ContractName = item.ContractName,
                            ModuleName = item.ModuleName,
                            // ContractCode = item.ContractCode,
                            ModuleCode = item.ModuleCode,
                            //Manufacture = item.Manufacture,
                            Quantity = item.Quantity,
                            RealQuantity = item.RealQuantity,
                            //TotalError = item.TotalError,
                            //Pricing = item.Pricing,
                            //Amount = item.Amount,
                            //ContractPrice = item.ContractPrice,
                            //Note = item.Note
                        });
                    }
                }
            }
            pathExcel = ExportBOM(designModuleInfoModel);
            return pathExcel;
        }

        public string ExportBOM(DesignModuleInfoModel model)
        {
            model.ListMaterial = model.ListMaterial.Where(i => i.CreateIndex == model.CreateIndex).ToList();
            foreach (var item in model.ListMaterial)
            {
                if (item.Qty > 0)
                {
                    item.Quantity = Convert.ToInt32(item.Qty);
                }
            }
            var project = db.Projects.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ProjectId));

            if (project == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
            }
            var projectProduct = db.ProjectProducts.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ProjectProductId));

            List<string> ListfileZip = new List<string>();

            List<DesignMaterialExportModel> materialExport = new List<DesignMaterialExportModel>();

            string date = DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss");

            string exportPath = $"Template/Export/{model.ProjectId}_{date}";

            string newPath = HttpContext.Current.Server.MapPath("~/" + exportPath);
            Directory.CreateDirectory(newPath);

            int index = 1;
            foreach (var module in model.Modules)
            {
                var materials = _business.GetModuleMaterials(module.Id, string.Empty);

                foreach (var item in materials)
                {
                    if (!string.IsNullOrEmpty(item.Parameter))
                    {
                        if (item.Parameter.ToUpper().Equals(Constants.Manufacture_TPA.ToUpper()) && item.Unit.ToUpper().Equals(Constants.Unit_Bo.ToUpper()) && item.Manufacturer.ToUpper().Equals(Constants.Manufacture_TPA.ToUpper()))
                        {
                            item.GroupCode = "114";
                        }
                    }
                }

                module.ProjectCode = project.Code;
                module.WarehouseCode = project.WarehouseCode;
                module.Materials = _business.GenerateMaterials(materials, index.ToString(), module.RealQuantity);
                module.Parameter = Constants.Manufacture_TPA;
                materialExport.AddRange(_business.ExportModuleMaterials(module, index.ToString(), project.Code, newPath, false));
                index++;
            }

            //foreach (var item in model.ListMaterial)
            //{
            //    var module = db.Modules.Where(i => item.Code.ToUpper().Equals(i.Code.ToUpper())).Select(p => new DesignModuleModel
            //    {
            //        Id = p.Id,
            //        ModuleName = p.Name,
            //        ModuleCode = p.Code,
            //    }).FirstOrDefault();
            //    if (module != null)
            //    {
            //        var materials = _business.GetModuleMaterials(module.Id, string.Empty);
            //        module.ProjectCode = project.Code;
            //        module.WarehouseCode = project.WarehouseCode;
            //        module.Materials = _business.GenerateMaterials(materials, index.ToString(), module.RealQuantity);

            //        materialExport.AddRange(_business.ExportModuleMaterials(module, index.ToString(), project.Code, newPath));
            //        index++;
            //    }
            //}

            if (model.ListMaterial.Count > 0)
            {
                List<DesignMaterialModel> listMaterial = new List<DesignMaterialModel>();
                DesignModuleModel module = new DesignModuleModel();
                var config = db.Configs.AsNoTracking().FirstOrDefault(i => i.Code.Equals(Constants.BOM_VATTUPHU_INDEX));
                if (config != null)
                {
                    module.ProjectCode = project.Code;
                    module.ModuleCode = "VATTUPHU." + projectProduct.ContractCode;
                    module.ModuleName = "Vật tư phụ";
                    module.Index = config.Value;
                    module.Parameter = "TPA";
                    module.WarehouseCode = project.WarehouseCode;
                    module.Quantity = 1;
                    module.RealQuantity = 1;
                    int indexMaterial = 1;
                    var listMaterials = db.Materials.AsNoTracking().ToList();
                    foreach (var item in model.ListMaterial)
                    {
                        var moduleMaterial = db.Modules.AsNoTracking().Where(i => item.Code.ToUpper().Equals(i.Code.ToUpper())).Select(p => new DesignModuleModel
                        {
                            Id = p.Id,
                            ModuleName = p.Name,
                            ModuleCode = p.Code,
                        }).FirstOrDefault();

                        if (item.Type == Constants.ProjectGaneralDesignMaterial_Type_Material)
                        {
                            var materials = (from a in listMaterials
                                             join b in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals b.Id
                                             join c in db.MaterialGroupTPAs.AsNoTracking() on b.MaterialGroupTPAId equals c.Id
                                             join d in db.Units.AsNoTracking() on a.UnitId equals d.Id
                                             where a.Id.Equals(item.Id)
                                             select new MaterialModel
                                             {
                                                 MaterialGroupName = c.Code,
                                                 UnitName = d.Name,
                                                 Parameter = a.Specification,
                                                 Note = a.Note
                                             }).FirstOrDefault();

                            listMaterial.Add(new DesignMaterialModel
                            {
                                Index = indexMaterial.ToString(),
                                ModuleIndex = indexMaterial.ToString(),
                                Id = item.Id,
                                MaterialName = item.Name,
                                MaterialCode = item.Code,
                                Quantity = item.Quantity,
                                ModuleQuantity = item.Quantity,
                                Price = item.Price,
                                Manufacturer = item.Manafacture,
                                Unit = materials.UnitName,
                                GroupCode = materials.MaterialGroupName,
                                Parameter = materials.Parameter,
                                RawMaterial = materials.RawMaterial,
                                Note = materials.Note,
                                //ModuleQuantity = item.Quantity
                            });

                            if (moduleMaterial != null)
                            {
                                var material = _business.GetModuleMaterials(moduleMaterial.Id, string.Empty);
                                var materialModels = _business.GenerateMaterials(material, indexMaterial.ToString(), item.Quantity);
                                listMaterial.AddRange(materialModels);
                            }
                        }
                        else if (item.Type == Constants.PracticeSupMaterial_Type_Module)
                        {
                            var modules = (from a in db.Modules.AsNoTracking()
                                           join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                                           where a.Id.Equals(item.Id)
                                           select new MaterialModel
                                           {
                                               MaterialGroupName = b.Code,
                                           }).FirstOrDefault();

                            listMaterial.Add(new DesignMaterialModel
                            {
                                Index = indexMaterial.ToString(),
                                ModuleIndex = indexMaterial.ToString(),
                                Id = item.Id,
                                MaterialName = item.Name,
                                MaterialCode = item.Code,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                Manufacturer = item.Manafacture,
                                Unit = Constants.Unit_Bo,
                                GroupCode = "101",
                                Parameter = Constants.Manufacture_TPA,
                                RawMaterial = moduleMaterial != null ? modules.RawMaterial : "",
                                Note = moduleMaterial != null ? modules.Note : "",
                                ModuleQuantity = item.Quantity
                            });

                            if (moduleMaterial != null)
                            {
                                var material = _business.GetModuleMaterials(moduleMaterial.Id, string.Empty);
                                var materialModels = _business.GenerateMaterials(material, indexMaterial.ToString(), item.Quantity);
                                listMaterial.AddRange(materialModels);
                            }
                        }

                        indexMaterial++;
                    }

                    foreach (var material in listMaterial)
                    {
                        material.ModuleQuantity = material.ModuleQuantity * projectProduct.RealQuantity;
                        material.ModuleIndex = config.Value.ToString() + "." + material.ModuleIndex;
                    }
                    //module.Materials = _business.GenerateMaterialProjectGarenalDesign(listMaterial, config.Value.ToString(), projectProduct.RealQuantity);

                    module.Materials = listMaterial;
                    materialExport.AddRange(_business.ExportModuleMaterials(module, config.Value.ToString(), project.Code, newPath, true));
                }
            }

            _business.ExportBOM(materialExport, newPath, projectProduct);
            return Path.Combine(exportPath, _business.ZipDMVTSAP(newPath, date, project.Code));
        }

        public void UpdateApproveStatus(ProjectGaneralDesignApproveStatusModel model)
        {
            var projectGeneralDesign = db.ProjectGeneralDesigns.FirstOrDefault(i => i.Id.Equals(model.Id));
            if (projectGeneralDesign != null)
            {
                projectGeneralDesign.ApproveStatus = model.ApproveStatus;
                if (model.ListPlan.Count() > 0)
                {
                    UpdatePlanInList(model.ListPlan);
                }
            }
            else
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectGeneralDesign);
            }
            db.SaveChanges();
        }

        public List<PlanResultModel> GetListPlanByProjectProduct(ProjectGeneralDesignModel model)
        {
            List<string> listId = new List<string>();
            listId = model.ListModule.Select(i => i.ProjectProductId).ToList();
            List<PlanResultModel> listPlan = new List<PlanResultModel>();
            //var listPlan = (from a in db.Plans.AsNoTracking()
            //                join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
            //                //where b.DepartmentId.Equals(modelSearch.DepartmentId)
            //                join e in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals e.Id
            //                join c in db.Employees.AsNoTracking() on a.ResponsiblePersion equals c.Id into ac
            //                from ca in ac.DefaultIfEmpty()
            //                join d in db.Tasks.AsNoTracking() on a.TaskId equals d.Id
            //                where listId.Contains(a.ProjectProductId) && d.Type == Constants.Task_Design && a.Status != Constants.Plan_Status_Done && string.IsNullOrEmpty(a.ReferenceId)
            //                orderby a.Index
            //                select new PlanResultModel()
            //                {
            //                    Id = a.Id,
            //                    ProjectId = b.Id,
            //                    ProjectName = b.Name,
            //                    ProjectCode = b.Code,
            //                    DepartmentId = b.DepartmentId,
            //                    TaskId = a.Id,
            //                    TaskName = d.Name,
            //                    ProjectProductId = a.ProjectProductId,
            //                    ContractCode = e.ContractCode,
            //                    ContractName = e.ContractName,
            //                    ResponsiblePersion = a.ResponsiblePersion,
            //                    ResponsiblePersionName = ca.Name,
            //                    //ExecutionTime = a.ExecutionTime,
            //                    StartDate = a.StartDate,
            //                    EndDate = a.EndDate,
            //                    RealStartDate = a.RealStartDate,
            //                    RealEndDate = a.RealEndDate,
            //                    EmployeeCode = ca.Code,
            //                    Done = a.Done,
            //                    DataType = e.DataType,
            //                    ProductId = e.ProductId,
            //                    ModuleId = e.ModuleId
            //                }).ToList();
            //foreach (var item in listPlan)
            //{
            //    if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
            //    {
            //        var product = db.Products.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item.ProductId));
            //        if (product != null)
            //        {
            //            item.DesignCode = product.Code;
            //            item.DesignName = product.Name;
            //        }
            //    }
            //    else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm || item.DataType == Constants.ProjectProduct_DataType_Module)
            //    {
            //        var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item.ModuleId));
            //        if (module != null)
            //        {
            //            item.DesignCode = module.Code;
            //            item.DesignName = module.Name;
            //        }
            //    }
            //}
            //listPlan.OrderBy(i => i.DesignCode);
            return listPlan;
        }

        public void UpdatePlanInList(List<PlanResultModel> listPlan)
        {
            if (listPlan.Count > 0)
            {
                foreach (var item in listPlan)
                {
                    var plan = db.Plans.FirstOrDefault(i => i.Id.Equals(item.Id));
                    if (plan != null)
                    {
                        plan.Status = Constants.Plan_Status_Done;
                        //plan.Done = 100;
                        //plan.RealEndDate = DateTime.Now;
                    }
                }
            }
        }

        public bool CheckApproveStatus(string projectProductId)
        {
            bool check = false;
            var projectGeneralDesigns = db.ProjectGeneralDesigns.AsNoTracking().Where(i => i.ProjectProductId.Equals(projectProductId)).ToList();
            if (projectGeneralDesigns.Count > 0)
            {
                var createIndex = projectGeneralDesigns.Max(i => i.CreateIndex);
                var data = projectGeneralDesigns.FirstOrDefault(i => i.CreateIndex == createIndex);
                if (data != null)
                {
                    if (data.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved)
                    {
                        check = true;
                    }
                }
            }
            else
            {
                check = true;
            }
            return check;
        }
    }
}
