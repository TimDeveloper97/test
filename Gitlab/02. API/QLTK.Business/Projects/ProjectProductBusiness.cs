using NTS.Business.Combobox;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.IncurredMaterial;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterialFinishDesign;
using NTS.Model.ModuleMaterials;
using NTS.Model.Plans;
using NTS.Model.ProductCatalog;
using NTS.Model.ProductDocument;
using NTS.Model.ProductStandards;
using NTS.Model.Project;
using NTS.Model.ProjectProducts;
using NTS.Model.Projects.ProjectProducts;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Utils;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Solutions;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ProjectProducts
{
    public class ProjectProductBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private readonly MaterialBusiness materialBusiness = new MaterialBusiness();

        public ProjectProductResultModel SearchProjectProduct(ProjectProductsSearchModel modelSearch)
        {
            ProjectProductResultModel searchResult = new ProjectProductResultModel();

            List<string> projectProductIds = new List<string>();
            if (!string.IsNullOrEmpty(modelSearch.MaterialName))
            {
                projectProductIds = db.ModuleMaterialFinishDesigns.AsNoTracking().Where(r => r.MaterialName.ToLower().Equals(modelSearch.MaterialName.ToLower()) || r.MaterialCode.ToLower().Equals(modelSearch.MaterialName.ToLower())).Select(s => s.ProjectProductId).ToList();
            }

            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ProjectId.Equals(modelSearch.ProjectId)
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
                                          ModuleStatus = a.ModuleStatus,
                                          DesignStatus = a.DesignStatus,
                                          DesignFinishDate = a.DesignFinishDate,
                                          MakeFinishDate = a.MakeFinishDate,
                                          DeliveryDate = a.DeliveryDate,
                                          TransferDate = a.TransferDate,
                                          Note = a.Note,
                                          Quantity = a.Quantity,
                                          RealQuantity = a.RealQuantity,
                                          Price = a.Price,
                                          Amount = a.Quantity * a.Price,
                                          ContractIndex = a.ContractIndex,
                                          IsGeneralDesign = a.IsGeneralDesign,
                                          DesignWorkStatus = a.DesignWorkStatus,
                                          DesignCloseDate = a.DesignCloseDate,
                                          GeneralDesignLastDate = a.GeneralDesignLastDate,
                                          MaterialExist = a.MaterialExist,
                                          IsMaterial = true,
                                          IsIncurred = a.IsIncurred,
                                          IsNeedQC = a.IsNeedQC,
                                          QCQuantity = a.QCQuantity
                                      }).AsQueryable();

            //var countParent = listProjectProduct.Where(t => string.IsNullOrEmpty(t.ParentId)).ToList().Count;
            if (!string.IsNullOrEmpty(modelSearch.Id))
            {
                listProjectProduct = listProjectProduct.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
            }

            var list = listProjectProduct.ToList();
            Module module;
            Product product;
            var materialBOMDrafts = db.MaterialImportBOMDrafts.AsNoTracking().ToList();
            var materials = db.Materials.AsNoTracking().ToList();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.ParentId) && item.DataType != Constants.ProjectProduct_DataType_Paradigm && item.DataType != Constants.ProjectProduct_DataType_Module && !item.MaterialExist)
                {
                    var parent = db.ProjectProducts.AsNoTracking().FirstOrDefault(a => a.Id.Equals(item.ParentId));
                    if (parent.DataType == Constants.ProjectProduct_DataType_Paradigm || parent.DataType == Constants.ProjectProduct_DataType_Module)
                    {
                        item.IsMaterial = false;
                    }
                }
                if (item.ModuleId != null)
                {
                    var materialImportBOMDrafts = materialBOMDrafts.Where(i => i.ProjectId.Equals(item.Id) && i.ModuleId.Equals(item.ModuleId) && (i.UpdateStatus == 0 || i.UpdateStatus == 1)).Select(a => new MaterialImportBOMDraftModel
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

                    if (materialImportBOMDrafts.Count > 0)
                    {
                        decimal total = GetPriceTHTK(materialImportBOMDrafts, materials);
                        item.PriceTHTK = System.Math.Truncate((total));
                        item.AmountTHTK = System.Math.Truncate((total));
                    }
                }
                if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        item.Code = product.Code;
                        item.Name = product.Name;
                    }
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
                    if (module != null)
                    {
                        item.Code = module.Code;
                        item.Name = module.Name;
                    }
                }
            }

            List<ProjectProductsModel> listResult = new List<ProjectProductsModel>();
            //var listParent = list.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            bool isSearch = false;
            int index = 1;

            List<ProjectProductsModel> listChild = new List<ProjectProductsModel>();
            IncurredMaterialResultModel incurredMaterial;
            IncurredMaterialSearchModel model;
            foreach (var item in list.Where(i => string.IsNullOrEmpty(i.ParentId)))
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.ContractName) && !item.ContractName.ToLower().Contains(modelSearch.ContractName.ToLower()))
                {
                    isSearch = false;
                }

                if (!string.IsNullOrEmpty(modelSearch.ContractCode) && !item.ContractCode.ToLower().Contains(modelSearch.ContractCode.ToLower()))
                {
                    isSearch = false;
                }

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    if (string.IsNullOrEmpty(item.Code) || !item.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                    {
                        isSearch = false;
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    if (string.IsNullOrEmpty(item.Name) || !item.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                    {
                        isSearch = false;
                    }
                }

                if (modelSearch.DataType != 0 && !item.DataType.Equals(modelSearch.DataType))
                {
                    isSearch = false;
                }

                if (modelSearch.ModuleStatus != 0 && !item.ModuleStatus.Equals(modelSearch.ModuleStatus))
                {
                    isSearch = false;
                }

                if (modelSearch.DesignStatus != 0 && !item.DesignStatus.Equals(modelSearch.DesignStatus))
                {
                    isSearch = false;
                }

                if (modelSearch.DesignStatus != 0 && !item.DesignStatus.Equals(modelSearch.DesignStatus))
                {
                    isSearch = false;
                }

                if (modelSearch.CostType == 2 && !item.IsIncurred)
                {
                    isSearch = false;
                }

                if (projectProductIds.Count > 0 && projectProductIds.IndexOf(item.Id) == -1)
                {
                    isSearch = false;
                }

                //if (isSearch)
                //{


                if (!string.IsNullOrEmpty(item.ParentId))
                {
                    GetProjectProductParent(item.ParentId, listResult, list);
                }

                listChild = GetProjectProductChild(item.Id, list.ToList(), modelSearch, index.ToString(), projectProductIds);

                if (isSearch || listChild.Count > 0)
                {
                    if (!listResult.Contains(item))
                    {
                        listResult.Add(item);
                    }

                    item.Index = index.ToString();


                    if (listChild.Count > 0)
                    {
                        item.PriceTHTK = item.PriceTHTK + listChild.Where(i => i.ParentId.Equals(item.Id)).Sum(i => i.PriceTHTK * (i.QuantityTHTK > 0 ? i.QuantityTHTK : i.Quantity));
                        item.AmountIncurred = listChild.Where(i => i.ParentId.Equals(item.Id)).Sum(i => i.AmountIncurred);
                        item.Price = listChild.Where(i => i.ParentId.Equals(item.Id)).Sum(i => i.Quantity * i.Price);
                        item.ColorGeneralDesign = listChild.FirstOrDefault(i => !i.IsProductGeneralDesign) != null ? false : true;
                        item.AmountTHTK = item.PriceTHTK * item.Quantity;
                    }
                    else
                    {
                        item.ColorGeneralDesign = item.IsProductGeneralDesign;
                        item.AmountTHTK = item.PriceTHTK * item.Quantity;
                    }

                    item.Amount = item.Price * item.Quantity;

                    foreach (var ite in listChild)
                    {
                        if (!listResult.Contains(ite))
                        {
                            listResult.Add(ite);
                        }
                    }

                    if (listChild.Count == 0 && (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm) && !string.IsNullOrEmpty(item.ModuleId) && item.DesignWorkStatus)
                    {
                        model = new IncurredMaterialSearchModel
                        {
                            Id = item.Id,
                            ModuleId = item.ModuleId
                        };
                        incurredMaterial = (IncurredMaterialResultModel)GetIncurredMaterial(model);
                        item.AmountIncurred = incurredMaterial.Total;
                    }

                    index++;
                }

                //}

            }

            searchResult.TotalItems = listResult.Count();

            if (listResult.Count() > 0)
            {
                int maxLen = listResult.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                listResult = listResult
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            foreach (var item in listResult)
            {
                if (item.DataType == Constants.ProjectProduct_DataType_Practice)
                {
                    item.DatatypeName = "Bài thực hành";
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    item.DatatypeName = "Sản phẩm/Lai sản xuất";
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    item.DatatypeName = "Mô hình/máy";
                }
                else
                    item.DatatypeName = "Module";
                item.ModuleStatusName = item.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung";
                item.DesignStatusName = item.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : item.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : item.DesignStatus == Constants.ProjectProduct_DesignStatus_Use ? "Tận dụng" : item.DesignStatus == Constants.ProjectProduct_DesignStatus_DesignStatus ? "Hàng bán thẳng" : "";
            }
            var listFather = listResult.Where(i => string.IsNullOrEmpty(i.ParentId));
            searchResult.ListResult = listResult;
            foreach (var f in listFather)
            {
                List<MaterialImportBOMDraftModel> listResultDone = new List<MaterialImportBOMDraftModel>();

                var projectGeneralDesign = db.ProjectGeneralDesigns.Where(a => a.ProjectProductId.Equals(f.Id)).ToList();
                decimal totalPriceParentFatherVTP = 0;
                foreach (var projectd in projectGeneralDesign)
                {
                    var listMeterialDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                              where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                              join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                              join c in db.Manufactures.AsNoTracking() on b.ManufactureId equals c.Id
                                              join d in db.Units.AsNoTracking() on b.UnitId equals d.Id
                                              where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                              orderby b.Code
                                              select new MaterialImportBOMDraftModel
                                              {
                                                  Id = a.MaterialId,
                                                  ModuleId = "vtp",
                                                  MaterialName = b.Name,
                                                  MaterialCode = b.Code,
                                                  ManufacturerCode = c.Code,
                                                  Quantity = (int)a.Quantity,
                                                  Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  UnitName = d.Name,
                                                  Amount = (int)a.Quantity * projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  ParentId = "vtp"
                                              }).ToList();

                    var listModuleDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                            where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                            join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                                            where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                            orderby b.Code
                                            select new MaterialImportBOMDraftModel
                                            {
                                                Id = a.MaterialId,
                                                ModuleId = "vtp",
                                                MaterialName = b.Name,
                                                MaterialCode = b.Code,
                                                Quantity = (int)a.Quantity,
                                                ManufacturerCode = Constants.Manufacture_TPA,
                                                UnitName = Constants.Unit_Bo,
                                                Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                ParentId = "vtp"
                                            }).ToList();

                    if (listModuleDesign.Count > 0 && projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_NotApproved)
                    {
                        ModulePriceInfoModel modulePriceInfoModel;
                        foreach (var item in listModuleDesign)
                        {
                            modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.Id);
                            if (modulePriceInfoModel != null)
                            {
                                item.Pricing = modulePriceInfoModel.Price;
                            }
                        }
                    }
                    foreach (var moduleF in listModuleDesign)
                    {
                        moduleF.Amount = moduleF.Quantity * moduleF.Pricing;
                    }

                    foreach (var ma in listMeterialDesign)
                    {
                        var materialF = listResultDone.FirstOrDefault(a => ma.MaterialCode.Equals(a.MaterialCode) && ma.ModuleId.Equals(a.ModuleId));
                        if (materialF == null)
                        {
                            listResultDone.Add(ma);
                        }
                    }
                    foreach (var mo in listModuleDesign)
                    {
                        var moduleF = listResultDone.FirstOrDefault(a => mo.MaterialCode.Equals(a.MaterialCode) && mo.ModuleId.Equals(a.ModuleId));
                        if (moduleF == null)
                        {
                            listResultDone.Add(mo);
                        }
                    }
                }
                decimal PriceParentVTP = 0;
                foreach (var done in listResultDone)
                {
                    PriceParentVTP = PriceParentVTP + done.Quantity * done.Pricing;
                }
                totalPriceParentFatherVTP = totalPriceParentFatherVTP + PriceParentVTP;
                f.PriceTHTK = f.PriceTHTK + totalPriceParentFatherVTP;
                f.AmountTHTK = f.RealQuantity * f.PriceTHTK;
            }

            searchResult.ToTalAmount = listFather.Sum(i => i.Amount);
            searchResult.ToTalAmountTHTK = listFather.Sum(i => i.AmountTHTK);
            searchResult.TotalAmountIncurred = listFather.Sum(i => i.AmountIncurred);
            searchResult.ColorTHTK = listFather.FirstOrDefault(i => !i.ColorGeneralDesign) != null ? true : false;
            searchResult.Total = listResult.Count();
            return searchResult;
        }


        public TotalTypePlan GetNumberWorkInProject(string id)
        {
            var projectProducts = db.ProjectProducts.AsNoTracking().Where(a => a.ProjectId.Equals(id) && a.Status != 4 && a.Status != 5).ToList();
            var plans = db.Plans.AsNoTracking().Where(a => a.ProjectId.Equals(id) && a.Status != 4 && a.Status != 5).ToList();
            var planAssigment = db.PlanAssignments.AsNoTracking().Select(r => r.PlanId).ToList();

            DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Now);

            TotalTypePlan totalTypePlan = new TotalTypePlan();

            // Tình trạng công việc
            totalTypePlan.WorkLate = plans.Where(r => r.IsPlan == true && r.PlanDueDate.HasValue && r.PlanDueDate.Value < today && r.Status != 3).Count();
            totalTypePlan.WorkEmptyStart = plans.Where(r => r.IsPlan == true && (!r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue)).Count();
            totalTypePlan.WorkIncurred = plans.Where(r => r.IsPlan == true && r.Type != 1).Count();

            // Tình trạng công đoạn
            totalTypePlan.StageLate = plans.Where(r => r.IsPlan == false && r.ContractDueDate.HasValue && r.PlanDueDate.HasValue && r.PlanDueDate > r.ContractDueDate).Count();
            totalTypePlan.StageEmptyStart = plans.Where(r => r.IsPlan == false && (!r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue)).Count();
            totalTypePlan.StageEmptyEnd = plans.Where(r => r.IsPlan == false && (!r.ContractStartDate.HasValue || !r.ContractDueDate.HasValue)).Count();

            // Tình trạng Module
            totalTypePlan.ParentProductEmptyStart = projectProducts.Where((r => r.DataType == Constants.ProjectProduct_DataType_ProjectProduct && (!r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue))).Count();
            totalTypePlan.ParentProductEmptyEnd = projectProducts.Where((r => r.DataType == Constants.ProjectProduct_DataType_ProjectProduct && (!r.ContractStartDate.HasValue || !r.ContractDueDate.HasValue))).Count();
            totalTypePlan.ParentProductIncurred = projectProducts.Where((r => r.DataType == Constants.ProjectProduct_DataType_ProjectProduct && (r.ModuleStatus == 2 || r.ModuleStatus == 3))).Count();

            // Tình trạng Sản phẩm
            totalTypePlan.ParentProductEmptyStart = projectProducts.Where((r => (r.DataType == Constants.ProjectProduct_DataType_Module || r.DataType == Constants.ProjectProduct_DataType_Paradigm) && (!r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue))).Count();
            totalTypePlan.ParentProductEmptyEnd = projectProducts.Where((r => (r.DataType == Constants.ProjectProduct_DataType_Module || r.DataType == Constants.ProjectProduct_DataType_Paradigm) && (!r.ContractStartDate.HasValue || !r.ContractDueDate.HasValue))).Count();
            totalTypePlan.ParentProductIncurred = projectProducts.Where((r => (r.DataType == Constants.ProjectProduct_DataType_Module || r.DataType == Constants.ProjectProduct_DataType_Paradigm) && (r.ModuleStatus == 2 || r.ModuleStatus == 3))).Count();

            // Tình trạng Sản phẩm
            totalTypePlan.ThieuNhaThau = plans.Where(r => r.IsPlan && string.IsNullOrEmpty(r.SupplierId)).Count();
            totalTypePlan.ThieuNguoiPhutrach = plans.Where(r => r.IsPlan && !planAssigment.Contains(r.Id)).Count();

            return totalTypePlan;
        }

        public GeneralPlan GetImplementationPlanVersusReality(string id)
        {
            GeneralPlan general = new GeneralPlan();
            var allStageOfProject = (from a in db.Plans.AsNoTracking()
                                     join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                     where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && a.ProjectId.Equals(id)
                                     orderby b.index
                                     select new PlanImplementReality
                                     {
                                         StageId = b.Id,
                                         StageName = b.Code,
                                         ContractDate = a.ContractDueDate,
                                         RealDate = a.PlanDueDate,
                                         DateDelay = 0,
                                         PlanId = a.Id,
                                         Status = a.Status
                                     }).ToList();
            List<PlanImplementReality> stages = new List<PlanImplementReality>();
            foreach (var item in allStageOfProject)
            {
                var stage = stages.FirstOrDefault(a => a.StageId.Equals(item.StageId));
                if (stage == null)
                {
                    stages.Add(item);
                }
            }
            ///
            decimal sumWeigth = 0;
            decimal sumDoneRatio = 0;
            //var Plans = GetListPlanByProjectId(id);
            var projectProducts = db.ProjectProducts.AsNoTracking().Where(a => a.ProjectId.Equals(id));
            var listProjectProductParent = projectProducts.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            foreach (var projectProductParent in listProjectProductParent)
            {
                if (projectProductParent.Status != 4 && projectProductParent.Status != 5)
                {
                    sumWeigth = sumWeigth + projectProductParent.Weight;
                    sumDoneRatio = sumDoneRatio + projectProductParent.DoneRatio * projectProductParent.Weight;
                }
            }
            if (sumWeigth == 0)
            {
                general.DoneRatio = "0%";
            }
            else
            {
                var resultRatio = (sumDoneRatio / sumWeigth);

                general.DoneRatio = resultRatio.ToString("F") + "%";
            }
            //tính số ngày delay
            foreach (var stage in stages.ToList())
            {
                var stageParents = allStageOfProject.Where(a => a.StageId.Equals(stage.StageId)).ToList();
                var countStageStopAndCancel = allStageOfProject.Where(a => (a.Status == 4 || a.Status == 5) && a.StageId.Equals(stage.StageId)).Count();
                if (countStageStopAndCancel != stageParents.Count())
                {
                    var countDone = allStageOfProject.Where(a => (a.Status == 3) && a.StageId.Equals(stage.StageId)).Count();
                    DateTime? stageContractMax = null;
                    DateTime? stagePlanMax = null;
                    if (countDone != stageParents.Count())
                    {
                        stageContractMax = stageParents.Where(a => a.Status != 3).Select(a => a.ContractDate).Max();
                        stagePlanMax = stageParents.Where(a => a.Status != 3).Select(a => a.RealDate).Max();
                    }
                    else
                    {
                        stageContractMax = stageParents.Select(a => a.ContractDate).Max();
                        stagePlanMax = stageParents.Select(a => a.RealDate).Max();
                    }

                    if ((stageContractMax == null || stagePlanMax == null))
                    {
                        stage.DateDelay = null;
                    }
                    else
                    {
                        stage.DateDelay = (((DateTime)stageContractMax) - ((DateTime)stagePlanMax)).Days;
                        //stage.PlanId = idStage;
                        if (stage.DateDelay >= 0)
                        {
                            stage.IsDelay = false;
                        }
                        else
                        {
                            stage.IsDelay = true;
                        }
                    }
                    stage.ContractDate = stageContractMax;
                    stage.PlanDate = stagePlanMax;


                }
                else
                {
                    stages.Remove(stage);
                }

            }
            decimal totalWidth = 0;
            if (stages.Count != 0)
            {
                decimal width = 90 / (decimal)(stages.Count());
                for (int i = 0; i < stages.Count; i++)
                {
                    if (i > 0)
                    {
                        var stage = stages[i - 1];
                        var widthCompare = width * (i + 1);
                        var SubWitdh = widthCompare - totalWidth;
                        if (stages[i].DateDelay == 0 || stages[i].DateDelay == null)
                        {
                            stages[i].Width = SubWitdh;
                        }
                        else if (stages[i].DateDelay < 0)
                        {
                            stages[i].Width = SubWitdh + 2;
                        }
                        else if (stages[i].DateDelay > 0)
                        {
                            stages[i].Width = SubWitdh - 2;
                        }

                    }
                    else if (i == 0)
                    {
                        if (stages[i].DateDelay == 0 || stages[i].DateDelay == null)
                        {
                            stages[i].Width = width;
                        }
                        else if (stages[i].DateDelay < 0)
                        {
                            stages[i].Width = width + 2;
                        }
                        else if (stages[i].DateDelay > 0)
                        {
                            stages[i].Width = width - 2;
                        }
                    }
                    totalWidth = totalWidth + stages[i].Width;
                }
            }

            general.TotalWitdh = totalWidth;
            general.PlanImplementRealities = stages;
            return general;
        }


        public DoneRatioProject GetRatioDoneOfProject(string id)
        {
            DoneRatioProject drp = new DoneRatioProject();
            var projectProducts = db.ProjectProducts.AsNoTracking().Where(a => a.ProjectId.Equals(id));
            var listProjectProductParent = projectProducts.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            var totalOpenPlan = 0;
            var totalOnGoingPlan = 0;
            var totalClosePlan = 0;
            foreach (var ParentProjectProduct in listProjectProductParent)
            {
                if (ParentProjectProduct.Status == 1)
                {
                    totalOpenPlan = totalOpenPlan + ParentProjectProduct.Weight;
                }
                else if (ParentProjectProduct.Status == 2)
                {
                    totalOnGoingPlan = totalOnGoingPlan + ParentProjectProduct.Weight;
                }
                else if (ParentProjectProduct.Status == 3)
                {
                    totalClosePlan = totalClosePlan + ParentProjectProduct.Weight;
                }

            }
            drp.Done = totalClosePlan;
            drp.NoImplementation = totalOpenPlan;
            drp.Implementation = totalOnGoingPlan;
            return drp;
        }

        public ProjectProblem GetNumberErrorOfProject(string id)
        {
            DateTime dateNow = DateTime.Now;

            var data = db.Errors.Where(a => a.ProjectId.Equals(id)).ToList();
            ProjectProblem result = new ProjectProblem();
            //result.RiskNoAction = data.Where(a => a.Status == 1 || a.Status == 2).Count();
            //result.Done = data.Where(a => a.Status == 7|| a.Status ==9 || a.Status ==10).Count();
            result.Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
            result.NoImplementation = data.Where(a => a.Status == 3).Count();
            var errorDelays = (from a in db.Errors.AsNoTracking()
                               join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                               where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && c.Status != Constants.ErrorFix_Status_Finish && a.ProjectId.Equals(id)
                               select new
                               {
                                   Id = a.Id,
                                   Status = a.Status,
                                   FixStatus = c.Status,
                                   Deadline = (c.DateTo.HasValue && (dateNow > c.DateTo)) && (c.Status != Constants.ErrorFix_Status_Finish) ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                               }).AsQueryable();
            result.ErrorDelay = errorDelays.Where(r => r.Status >= Constants.Problem_Status_Processed && r.Deadline > 0 && r.FixStatus != Constants.ErrorFix_Status_Finish).GroupBy(g => g.Id).Count();

            return result;
        }

        public ProjectStatusWork GetWorkOfProject(string id)
        {
            ProjectStatusWork psw = new ProjectStatusWork();
            var data = db.Plans.Where(a => a.IsPlan.Equals(true) && a.ProjectId.Equals(id));
            psw.WorkDone = data.Where(a => a.Status == 3).Count();
            psw.WorkImplement = data.Where(a => a.Status == 2).Count();
            psw.WorkNoImplement = data.Where(a => a.Status == 1).Count();

            return psw;
        }

        public List<ScheduleProjectResultModel> GetStageOfProject(string id)
        {
            var queryPlan = (from a in db.Plans.AsNoTracking()
                             join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                             where a.ProjectId.Equals(id) && a.IsPlan && a.StageId != null
                             select new ScheduleProjectResultModel()
                             {
                                 Id = a.Id,
                                 ProjectId = a.Id,
                                 ProjectProductId = a.ProjectProductId,
                                 ParentId = a.ParentId != null ? a.ParentId : a.ProjectProductId,
                                 StageName = a.Name,
                                 BackgroundColor = b.Color,
                                 StageId = a.Id,
                                 ContractStartDate = a.ContractStartDate,
                                 ContractDueDate = a.ContractDueDate,
                                 PlanStartDate = a.PlanStartDate,
                                 PlanDueDate = a.PlanDueDate,
                             }).ToList();

            return queryPlan;
        }

        public ProjectPlan GetProjectProductByProjectId(string id)
        {
            ProjectPlan projectPlan = new ProjectPlan();
            var listProjectProduct = GetListPlanByProjectId(id);
            var listProjectProductParent = listProjectProduct.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            var allStageOfProject = (from a in db.Plans.AsNoTracking()
                                     join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                     where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && a.ProjectId.Equals(id)
                                     orderby b.index
                                     select new StageModel
                                     {
                                         StageId = b.Id,
                                         StageName = b.Name,
                                         Date = a.PlanDueDate,
                                         PlanId = a.Id
                                     }).ToList();
            List<StageModel> stages = new List<StageModel>();
            foreach (var item in allStageOfProject)
            {
                var stage = stages.FirstOrDefault(a => a.StageId.Equals(item.StageId));
                if (stage == null)
                {
                    stages.Add(item);
                }
            }
            projectPlan.StageModels.AddRange(stages);
            List<ProductPlan> productPlans = new List<ProductPlan>();
            foreach (var item in listProjectProductParent)
            {
                List<ScheduleEntity> listResult = new List<ScheduleEntity>();
                List<ScheduleEntity> listChild = new List<ScheduleEntity>();

                listChild = GetScheduleProjectChild(item.Id, listProjectProduct);
                listResult.Add(item);
                listResult.AddRange(listChild);
                var listPlanOfProject = listResult.Where(a => a.ProjectId.Equals(id)).ToList();

                ProductPlan productPlan = new ProductPlan();
                productPlan.ProductName = item.NameView;
                productPlan.PlanId = item.Id;
                List<StageModel> listStages = new List<StageModel>();
                foreach (var item1 in allStageOfProject)
                {
                    var stage = listStages.FirstOrDefault(a => item1.StageId.Equals(a.StageId));
                    if (stage == null)
                    {
                        listStages.Add(new StageModel { StageName = item1.StageName, StageId = item1.StageId });
                    }
                }
                foreach (var stage in listStages)
                {
                    var listStageOfProject = listPlanOfProject.Where(a => stage.StageId.Equals(a.StageId) && !string.IsNullOrEmpty(a.StageName)).ToList();
                    List<StageModel> stageModels = new List<StageModel>();
                    foreach (var planStage in listStageOfProject)
                    {
                        var s = CaclulatorStage(listResult, planStage);
                        stageModels.Add(s);
                    }
                    int weigth = 0;
                    decimal done = 0;
                    foreach (var item1 in stageModels)
                    {
                        if (!Constants.ScheduleStatus.Cancel.Equals(item1.Status) && !Constants.ScheduleStatus.Stop.Equals(item1.Status))
                        {
                            weigth = weigth + item1.Weight;
                            done = done + item1.DoneRatio * item1.Weight;
                        }
                    }
                    if (weigth == 0)
                    {
                        stage.Status = 4;
                    }
                    else
                    {
                        var resultDone = done / weigth;
                        stage.DoneRatio = resultDone;
                        if (resultDone == 0)
                        {
                            stage.Status = 1;
                            stage.Date = stageModels.Where(a => a.Status == 1 || a.Status == 2).Select(a => a.Date).Max();
                        }
                        else if (resultDone == 100)
                        {
                            stage.Status = 3;
                        }
                        else
                        {
                            stage.Status = 2;
                            stage.Date = stageModels.Where(a => a.Status == 1 || a.Status == 2).Select(a => a.Date).Max();
                        }
                        var countStopCancel = stageModels.Where(a => a.Status == 4 || a.Status == 5).ToList().Count();
                        if (countStopCancel == stageModels.Count())
                        {
                            stage.Status = 4;
                        }
                    }



                }
                productPlan.StageModels = listStages;

                productPlans.Add(productPlan);
            }
            // xóa những sp đang stop hoặc cancel
            foreach (var item in productPlans.ToList())
            {
                var countStageStopCancel = item.StageModels.Where(a => a.Status == 4).Count();
                if (countStageStopCancel == item.StageModels.Count())
                {
                    productPlans.Remove(item);
                }
            }
            foreach (var item1 in projectPlan.StageModels.ToList())
            {
                List<StageModel> StageModels = new List<StageModel>();
                foreach (var item in productPlans.ToList())
                {
                    var s = item.StageModels.FirstOrDefault(a => a.StageName.Equals(item1.StageName));
                    StageModels.Add(s);
                }
                // đếm sô công đoạn trạng thái là stop và cancel
                var countStageCancelAndStop = StageModels.Where(a => a.Status == 4).Count();
                if (countStageCancelAndStop == productPlans.Count())
                {
                    //tất cả sản phẩm có công đoạn là stop hoặc cancel thì loại bỏ công đoạn đó ra
                    projectPlan.StageModels.Remove(item1);
                    foreach (var item in productPlans.ToList())
                    {
                        var s = item.StageModels.FirstOrDefault(a => a.StageName.Equals(item1.StageName));
                        item.StageModels.Remove(s);
                    }
                }

            }
            projectPlan.ProductPlans = (productPlans);

            return projectPlan;
        }

        private StageModel CaclulatorStage(List<ScheduleEntity> listPlanOfProject, ScheduleEntity stage)
        {
            if (!string.IsNullOrEmpty(stage.ParentId))
            {
                var parentStage = listPlanOfProject.FirstOrDefault(a => a.Id.Equals(stage.ParentId));
                if (string.IsNullOrEmpty(parentStage.ParentId))
                {
                    StageModel stageModel = new StageModel();
                    stageModel.PlanId = stage.Id;
                    stageModel.StageName = stage.StageName;
                    stageModel.Status = stage.Status;
                    stageModel.Weight = stage.Weight;
                    stageModel.Date = stage.PlanDueDate;
                    stageModel.DoneRatio = stage.DoneRatio;
                    return stageModel;
                }
                else
                {
                    StageModel stageModel = new StageModel();
                    stageModel.StageName = stage.StageName;
                    stageModel.Status = stage.Status;
                    stageModel.Weight = stage.Weight;
                    stageModel.Date = stage.PlanDueDate;
                    stageModel.DoneRatio = stage.DoneRatio;

                    var weigth = CaclulatorWeigth(listPlanOfProject, parentStage, stageModel);
                    stageModel.Weight = weigth;
                    return stageModel;
                }
            }
            return null;
        }

        private int CaclulatorWeigth(List<ScheduleEntity> listPlanOfProject, ScheduleEntity parentStage, StageModel stage)
        {
            var weigth = stage.Weight * parentStage.Weight;

            var Parent = listPlanOfProject.FirstOrDefault(a => a.Id.Equals(parentStage.ParentId));
            if (string.IsNullOrEmpty(Parent.ParentId))
            {
                return weigth;
            }
            else
            {
                stage.Weight = weigth;
                return CaclulatorWeigth(listPlanOfProject, Parent, stage);
            }
        }

        private void GetProjectProductParent(string parentId, List<ProjectProductsModel> listResult, List<ProjectProductsModel> listProjectProduct)
        {
            var parent = listProjectProduct.FirstOrDefault(r => parentId.Equals(r.Id));
            if (parent != null && !listResult.Contains(parent))
            {
                listResult.Add(parent);
                if (!string.IsNullOrEmpty(parent.ParentId))
                {
                    GetProjectProductParent(parent.ParentId, listResult, listProjectProduct);
                }

            }
        }

        private List<ProjectProductsModel> GetProjectProductChild(string parentId, List<ProjectProductsModel> listProjectProduct, ProjectProductsSearchModel modelSearch, string index, List<string> projectProductIds)
        {
            List<ProjectProductsModel> listResult = new List<ProjectProductsModel>();
            IncurredMaterialResultModel incurredMaterial;
            IncurredMaterialSearchModel model;
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

                if (modelSearch.DataType != 0 && !child.DataType.Equals(modelSearch.DataType))
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

                if (modelSearch.CostType == 2 && !child.IsIncurred)
                {
                    isSearch = false;
                }

                if (projectProductIds.Count > 0 && projectProductIds.IndexOf(child.Id) == -1)
                {
                    isSearch = false;
                }

                listChildChild = GetProjectProductChild(child.Id, listProjectProduct, modelSearch, index + "." + indexChild, projectProductIds);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    if (listChildChild.Count > 0)
                    {
                        child.Price = listChildChild.Where(i => i.ParentId.Equals(child.Id)).Sum(i => i.Price * i.Quantity);
                        child.PriceTHTK = listChildChild.Where(i => i.ParentId.Equals(child.Id)).Sum(i => i.PriceTHTK * (i.QuantityTHTK > 0 ? i.QuantityTHTK : i.Quantity));
                        child.AmountIncurred = listChildChild.Where(i => i.ParentId.Equals(child.Id)).Sum(i => i.AmountIncurred);

                        child.ColorGeneralDesign = listChildChild.FirstOrDefault(i => !i.IsProductGeneralDesign) != null ? false : true;
                    }
                    else if (listChildChild.Count == 0 && (child.DataType == Constants.ProjectProduct_DataType_Module || child.DataType == Constants.ProjectProduct_DataType_Paradigm) && !string.IsNullOrEmpty(child.ModuleId) && child.DesignWorkStatus)
                    {
                        child.ColorGeneralDesign = child.IsProductGeneralDesign;

                        model = new IncurredMaterialSearchModel
                        {
                            Id = child.Id,
                            ModuleId = child.ModuleId
                        };
                        incurredMaterial = (IncurredMaterialResultModel)GetIncurredMaterial(model);
                        child.AmountIncurred = incurredMaterial.Total;
                    }

                    child.AmountTHTK = child.PriceTHTK * (child.RealQuantityTHTK > 0 ? child.RealQuantityTHTK : child.RealQuantity);
                    child.Amount = child.Price * child.RealQuantity;

                    if (modelSearch.CostType == 1 && child.Amount - child.AmountTHTK - child.AmountIncurred > 0)
                    {
                        isSearch = false;
                    }

                    if (isSearch || listChildChild.Count > 0)
                    {
                        listResult.Add(child);
                        listResult.AddRange(listChildChild);

                        indexChild++;
                    }
                }

            }
            return listResult;
        }
        public void CreateProjectProduct(ProjectProductsModel model)
        {
            // Xoá ký tự đặc biệt
            model.ContractCode = Util.RemoveSpecialCharacter(model.ContractCode);
            CheckExistedForAdd(model);
            decimal realQuantityParent = 1;
            if (!string.IsNullOrEmpty(model.ParentId))
            {
                var realQuantity = db.ProjectProducts.AsNoTracking().FirstOrDefault(i => model.ParentId.Equals(i.Id));
                if (realQuantity != null)
                {
                    realQuantityParent = realQuantity.RealQuantity;
                }
            }
            model.RealQuantity = realQuantityParent * model.Quantity;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProjectProduct projectProduct = new ProjectProduct
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = model.ProjectId,
                        ParentId = model.ParentId,
                        ModuleId = model.ModuleId,
                        ProductId = model.ProductId,
                        ContractCode = model.ContractCode,
                        ContractName = model.ContractName,
                        Specifications = model.Specifications,
                        DataType = model.DataType,
                        ModuleStatus = model.ModuleStatus,
                        DesignStatus = model.DesignStatus,
                        DesignFinishDate = model.DesignFinishDate,
                        MakeFinishDate = model.MakeFinishDate,
                        DeliveryDate = model.DeliveryDate,
                        TransferDate = model.TransferDate,
                        ExpectedDesignFinishDate = model.ExpectedDesignFinishDate,
                        ExpectedMakeFinishDate = model.ExpectedMakeFinishDate,
                        ExpectedTransferDate = model.ExpectedTransferDate,
                        Note = model.Note,
                        Quantity = model.Quantity,
                        RealQuantity = model.RealQuantity,
                        Price = model.Price,
                        ContractIndex = model.ContractIndex,
                        IsGeneralDesign = model.IsGeneralDesign,
                        DesignWorkStatus = model.DesignWorkStatus,
                        DesignCloseDate = model.DesignWorkStatus ? DateTime.Now : (DateTime?)null,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.ProjectProducts.Add(projectProduct);
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

        public void UpdateProjectProduct(ProjectProductsModel model)
        {
            //xoá ký tự đặc biệt
            model.ContractCode = Util.RemoveSpecialCharacter(model.ContractCode);
            CheckExistedForUpdate(model);
            decimal realQuantityParent = 1;
            if (!string.IsNullOrEmpty(model.ParentId))
            {
                var realQuantity = db.ProjectProducts.AsNoTracking().FirstOrDefault(i => model.ParentId.Equals(i.Id));
                if (realQuantity != null)
                {
                    realQuantityParent = realQuantity.RealQuantity;
                }
            }
            model.RealQuantity = realQuantityParent * model.Quantity;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectProduct = db.ProjectProducts.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    projectProduct.ProjectId = model.ProjectId;
                    projectProduct.ParentId = model.ParentId;
                    projectProduct.ContractCode = model.ContractCode;
                    projectProduct.ContractName = model.ContractName;
                    projectProduct.Specifications = model.Specifications;
                    projectProduct.DataType = model.DataType;
                    if (model.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                    {
                        projectProduct.ModuleId = null;
                        projectProduct.ProductId = model.ProductId;
                    }
                    else if (model.DataType == Constants.ProjectProduct_DataType_Module || model.DataType == Constants.ProjectProduct_DataType_Paradigm)
                    {
                        projectProduct.ModuleId = model.ModuleId;
                        projectProduct.ProductId = null;
                    }
                    projectProduct.ModuleStatus = model.ModuleStatus;
                    projectProduct.DesignStatus = model.DesignStatus;
                    projectProduct.DesignFinishDate = model.DesignFinishDate;
                    projectProduct.MakeFinishDate = model.MakeFinishDate;
                    projectProduct.DeliveryDate = model.DeliveryDate;
                    projectProduct.TransferDate = model.TransferDate;
                    projectProduct.ExpectedDesignFinishDate = model.ExpectedDesignFinishDate;
                    projectProduct.ExpectedMakeFinishDate = model.ExpectedMakeFinishDate;
                    projectProduct.ExpectedTransferDate = model.ExpectedTransferDate;
                    projectProduct.Note = model.Note;
                    projectProduct.Quantity = model.Quantity;
                    projectProduct.RealQuantity = model.RealQuantity;
                    projectProduct.Price = model.Price;
                    projectProduct.ContractIndex = model.ContractIndex;
                    projectProduct.IsGeneralDesign = model.IsGeneralDesign;
                    projectProduct.DesignWorkStatus = model.DesignWorkStatus;
                    projectProduct.UpdateBy = model.UpdateBy;
                    projectProduct.UpdateDate = DateTime.Now;

                    projectProduct.IsCatalogRequire = model.ListPublications.MarketingCatalogs;
                    projectProduct.IsPracticeGuideRequire = model.ListPublications.MarketingPractice;
                    projectProduct.IsUserGuideRequire = model.ListPublications.MarketingDevice;
                    projectProduct.IsMaintenaineGuideRequire = model.ListPublications.MarketingMaintenance;
                    projectProduct.IsNeedQC = model.IsNeedQC;
                    projectProduct.QCQuantity = model.QCQuantity;
                    projectProduct.CatalogRequireNote = model.CatalogRequireNote;
                    projectProduct.UserGuideRequireNote = model.UserGuideRequireNote;
                    projectProduct.MaintenaineGuideRequireNote = model.MaintenaineGuideRequireNote;
                    projectProduct.PracticeGuideRequireNote = model.PracticeGuideRequireNote;
                    //tình trạng ấn phẩm




                    // Cập nhật thời gian hoàn thành thiết kê
                    if (projectProduct.DesignWorkStatus && !projectProduct.DesignCloseDate.HasValue && !string.IsNullOrEmpty(model.ModuleId) && (model.DataType == Constants.ProjectProduct_DataType_Module || model.DataType == Constants.ProjectProduct_DataType_Paradigm))
                    {
                        projectProduct.DesignCloseDate = DateTime.Now;
                        //ModuleMaterialSearchModel moduleMaterialSearchModel = new ModuleMaterialSearchModel
                        //{
                        //    ModuleId = model.ModuleId
                        //};

                        //var data = moduleMaterialBusiness.SearchModuleMaterial(moduleMaterialSearchModel);

                        //var moduleMaterials = (from a in data.ListResult
                        //                       group a by new { a.MaterialId } into g
                        //                       select new
                        //                       {
                        //                           g.Key.MaterialId,
                        //                           Quantity = g.Sum(i => i.ReadQuantity)
                        //                       }).ToList();

                        //List<ModuleMaterialFinishDesign> finishDesigns = new List<ModuleMaterialFinishDesign>();
                        //foreach (var item in moduleMaterials)
                        //{
                        //    finishDesigns.Add(new ModuleMaterialFinishDesign()
                        //    {
                        //        Id = Guid.NewGuid().ToString(),
                        //        ProjectProductId = projectProduct.Id,
                        //        MaterialId = item.MaterialId,
                        //        Quantity = item.Quantity
                        //    });
                        //}

                        //db.ModuleMaterialFinishDesigns.AddRange(finishDesigns);
                    }

                    //Cập nhật số lượng thực của Module tổng hợp thiết kế
                    var projectGeneralDesignModule = db.ProjectGeneralDesignModules.FirstOrDefault(i => model.Id.Equals(i.ProjectProductId));
                    if (projectGeneralDesignModule != null)
                    {
                        projectGeneralDesignModule.RealQuantity = (int)realQuantityParent * projectGeneralDesignModule.Quantity;
                        var projectGeneralDesign = db.ProjectGeneralDesigns.FirstOrDefault(i => i.Id.Equals(projectGeneralDesignModule.ProjectGeneralDesignId));
                        if (projectGeneralDesign != null)
                        {
                            if (projectProduct.DataType == Constants.ProjectProduct_DataType_Module || projectProduct.DataType == Constants.ProjectProduct_DataType_Paradigm)
                            {
                                if (projectProduct.ModuleId != projectGeneralDesignModule.ModuleId)
                                {
                                    if (projectGeneralDesign.ApproveStatus == 0)
                                    {
                                        projectGeneralDesignModule.ModuleId = projectProduct.ModuleId;
                                        projectGeneralDesignModule.ModulePrice = Math.Round(moduleMaterialBusiness.GetPriceModuleByModuleId(projectProduct.ModuleId, 0), 4);
                                        projectGeneralDesignModule.ContractPrice = projectProduct.Price;
                                    }
                                    else
                                    {
                                        throw NTSException.CreateInstance("Bạn không thể thay đổi thiết kế khi tổng hợp đã được duyệt!");
                                    }
                                }
                            }
                        }
                    }


                    var Parent = db.ProjectProducts.AsNoTracking().Where(i => model.Id.Equals(i.ParentId)).ToList();
                    if (Parent.Count > 0)
                    {
                        UpdateRealQuantity(model.RealQuantity, Parent);
                    }
                    if (projectProduct.IsNeedQC)
                    {
                        QCCheckList(projectProduct);
                    }



                    db.SaveChanges();
                    trans.Commit();
                }
                catch (NTSException ntsEx)
                {
                    trans.Rollback();
                    throw ntsEx;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }
        public void QCCheckList(ProjectProduct model)
        {
            List<ExportCompareModel> listExportCompares = new List<ExportCompareModel>();
            List<string> readerProjectProduct = new List<string>();
            List<string> readerModule = new List<string>();



            var listProductItem = db.ProductItems;
            var time = DateTime.Now.ToString("yy");
            int serialMax = 0;
            //if (listProductItem.ToList().Count > 0)
            //{
            //    var a = listProductItem.Max(i => i.SerialNumber);
            //    string newstring = a.Substring(2);
            //    serialMax = Int32.Parse(newstring);

            //}
            serialMax = Int32.Parse(db.SystemParams.FirstOrDefault(r => r.Id.Equals("2")).ValueParam);


            ProductItem productItem;

            var listQCCheck = listProductItem.Where(r => r.ProjectProductId.Equals(model.Id)).ToList();
            if (listQCCheck.Count > 0)
            {
            }
            else
            {


                for (int i = 0; i < model.RealQuantity; i++)
                {
                    serialMax++;
                    productItem = new ProductItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SerialNumber = String.Concat(time, serialMax.ToString("00000")),
                        ProjectId = model.ProjectId,
                        ProjectProductId = model.Id,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        Description = "",
                        QCStatus = 0,

                    };
                    db.ProductItems.Add(productItem);

                    var newSystemParam = db.SystemParams.FirstOrDefault(r => r.Id.Equals("2"));
                    newSystemParam.ValueParam = serialMax.ToString();
                    var year = db.SystemParams.FirstOrDefault(r => r.Id.Equals("1")).ValueParam;
                    if (year.Equals(DateTime.Now.ToString("yyyy"))) { }
                    else
                    {
                        var newYearSystemParam = db.SystemParams.FirstOrDefault(r => r.Id.Equals("1"));
                        newSystemParam.ValueParam = DateTime.Now.ToString("yyyy");
                    }
                }
            }

            db.SaveChanges();
        }
        public void UpdateRealQuantity(decimal realQuantityParent, List<ProjectProduct> listParent)
        {
            try
            {
                foreach (var item in listParent)
                {
                    var projectProduct = db.ProjectProducts.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                    projectProduct.RealQuantity = projectProduct.Quantity * realQuantityParent;
                    //Cập nhật số lượng thực của Module tổng hợp thiết kế
                    var projectGeneralDesignModule = db.ProjectGeneralDesignModules.FirstOrDefault(i => item.Id.Equals(i.ProjectProductId));
                    if (projectGeneralDesignModule != null)
                    {
                        projectGeneralDesignModule.RealQuantity = (int)realQuantityParent * projectGeneralDesignModule.Quantity;
                    }
                    var Parent = db.ProjectProducts.AsNoTracking().Where(i => item.Id.Equals(i.ParentId)).ToList();
                    if (Parent.Count > 0)
                    {
                        UpdateRealQuantity(projectProduct.RealQuantity, Parent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NTSLogException(listParent, ex);
            }
        }

        public void DeleteProjectProduct(ProjectProductsModel model)
        {
            List<ProjectProductsModel> listCheck = GetProjectProductById(model.Id);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in listCheck)
                    {
                        if (db.ProjectGeneralDesigns.FirstOrDefault(i => i.ProjectProductId.Equals(item.Id)) != null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectProduct);
                        }

                        if (db.ProjectGeneralDesignModules.FirstOrDefault(i => i.ProjectProductId.Equals(item.Id)) != null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectProduct);
                        }

                        if (db.BOMDesignTwoes.FirstOrDefault(i => i.ProjectProductId.Equals(item.Id)) != null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProjectProduct);
                        }

                        var plans = db.Plans.Where(a => a.ProjectProductId.Equals(item.Id)).ToList();
                        var listId = plans.Select(i => i.Id).ToList();
                        var planAssignments = db.PlanAssignments.Where(a => listId.Contains(a.PlanId)).ToList();
                        db.PlanAssignments.RemoveRange(planAssignments);
                        db.Plans.RemoveRange(plans);

                        var projectProduct = db.ProjectProducts.FirstOrDefault(i => i.Id.Equals(item.Id));
                        db.ProjectProducts.Remove(projectProduct);
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
        public List<ProjectProductsModel> GetProjectProductById(string Id)
        {
            List<ProjectProductsModel> search = new List<ProjectProductsModel>();
            List<ProjectProductsModel> searchResult = new List<ProjectProductsModel>();
            try
            {

                var listProjectProduct = (from o in db.ProjectProducts.AsNoTracking().OrderBy(r => r.ContractName)
                                          select new ProjectProductsModel
                                          {
                                              Id = o.Id,
                                              Note = o.Note,
                                              ContractName = o.ContractName,
                                              ParentId = o.ParentId,
                                              CreateBy = o.CreateBy,
                                              CreateDate = o.CreateDate,
                                              UpdateBy = o.UpdateBy,
                                              UpdateDate = o.UpdateDate,
                                          }).AsQueryable();
                List<ProjectProductsModel> listResult = new List<ProjectProductsModel>();
                var listParent = listProjectProduct.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<ProjectProductsModel> listChild = new List<ProjectProductsModel>();

                foreach (var parent in listParent)
                {

                    listChild = GetProjectProductChild(parent.Id, listProjectProduct.ToList());
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
                throw new NTSLogException(null, ex);
            }
            return search;
        }
        private List<ProjectProductsModel> GetProjectProductChild(string parentId, List<ProjectProductsModel> listDocumentTemplateType)
        {
            List<ProjectProductsModel> listResult = new List<ProjectProductsModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<ProjectProductsModel> listChildChild = new List<ProjectProductsModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetProjectProductChild(child.Id, listDocumentTemplateType);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }
        public object GetProjectProductInfo(ProjectProductsModel model)
        {
            var resultInfo = db.ProjectProducts.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProjectProductsModel
            {
                Id = p.Id,
                ProjectId = p.ProjectId,
                ParentId = p.ParentId,
                ModuleId = p.ModuleId,
                ProductId = p.ProductId,
                ContractCode = p.ContractCode,
                ContractName = p.ContractName,
                Specifications = p.Specifications,
                DataType = p.DataType,
                ModuleStatus = p.ModuleStatus,
                DesignStatus = p.DesignStatus,
                DesignFinishDate = p.DesignFinishDate,
                MakeFinishDate = p.MakeFinishDate,
                DeliveryDate = p.DeliveryDate,
                TransferDate = p.TransferDate,
                ExpectedDesignFinishDate = p.ExpectedDesignFinishDate,
                ExpectedMakeFinishDate = p.ExpectedMakeFinishDate,
                ExpectedTransferDate = p.ExpectedTransferDate,
                Note = p.Note,
                Quantity = p.Quantity,
                Price = p.Price,
                ContractIndex = p.ContractIndex,
                IsGeneralDesign = p.IsGeneralDesign,

                DesignWorkStatus = p.DesignWorkStatus,
                DesignCloseDate = p.DesignCloseDate,
                GeneralDesignLastDate = p.GeneralDesignLastDate,
                MarketingCatalogs = p.IsCatalogRequire,
                MarketingPractice = p.IsPracticeGuideRequire,
                MarketingDevice = p.IsUserGuideRequire,
                MarketingMaintenance = p.IsMaintenaineGuideRequire,
                IsNeedQC = p.IsNeedQC,
                QCQuantity = p.QCQuantity,
                CatalogRequireNote = p.CatalogRequireNote,
                UserGuideRequireNote = p.UserGuideRequireNote,
                MaintenaineGuideRequireNote = p.MaintenaineGuideRequireNote,
                PracticeGuideRequireNote = p.PracticeGuideRequireNote,
                RealQuantity = p.RealQuantity,
            }).FirstOrDefault();

            resultInfo.listId = (from r in db.QCCheckLists.AsNoTracking()
                                 join a in db.QCResults.AsNoTracking() on r.Id equals a.QCCheckListId
                                 where r.ProjectProductId.Equals(resultInfo.Id)
                                 select r
                          ).Any();


            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectProduct);
            }
            var listFileCatalog = (from a in db.ProductCatalogs.AsNoTracking()
                                   where a.ProductId.Equals(resultInfo.ProductId)
                                   select a).ToList().Count;
            if (listFileCatalog > 0)
            {
                resultInfo.IsCantalogs = true;
            }
            var listGuidePractice = (from a in db.ProductDocuments.AsNoTracking()
                                     where a.ProductId.Equals(resultInfo.ProductId) && a.FileType == Constants.ProductDocumentAttach_GuidePractive
                                     select a).ToList().Count;
            if (listGuidePractice > 0)
            {
                resultInfo.IsMarketingPractice = true;
            }
            var listDMBTH = (from a in db.ProductDocuments.AsNoTracking()
                             where a.ProductId.Equals(resultInfo.ProductId) && a.FileType == Constants.ProductDocumentAttach_DMBTH
                             select a).ToList().Count;
            if (listDMBTH > 0)
            {
                resultInfo.IsMarketingDevice = true;
            }

            var listGuideMaintenance = (from a in db.ProductDocuments.AsNoTracking()
                                        where a.ProductId.Equals(resultInfo.ProductId) && a.FileType == Constants.ProductDocumentAttach_GuideMaintenance
                                        select a).ToList().Count;
            if (listGuideMaintenance > 0)
            {
                resultInfo.IsMarketingMaintenance = true;
            }

            if (!string.IsNullOrEmpty(resultInfo.ProductId))
            {
                var product = db.Products.AsNoTracking().FirstOrDefault(a => a.Id.Equals(resultInfo.ProductId));
                if (product != null)
                {
                    resultInfo.ProductName = product.Name;
                }

            }

            if (!string.IsNullOrEmpty(resultInfo.ParentId))
            {
                var parent = db.ProjectProducts.AsNoTracking().FirstOrDefault(a => a.Id.Equals(resultInfo.ParentId));
                resultInfo.ParentDataType = parent.DataType;
                resultInfo.ParentModuleId = parent.ModuleId;
            }

            return resultInfo;
        }
        private void CheckExistedForAdd(ProjectProductsModel model)
        {
            if (db.ProjectProducts.AsNoTracking().Where(o => o.ContractName.Equals(model.ContractName) && o.ProjectId.Equals(model.ProjectId) && o.ParentId.Equals(model.ParentId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0023, TextResourceKey.ProjectProduct);
            }

            //if (db.ProjectProducts.AsNoTracking().Where(o => o.ContractCode.Equals(model.ContractCode)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0024, TextResourceKey.ProjectProduct);
            //}
        }
        public void CheckExistedForUpdate(ProjectProductsModel model)
        {
            if (db.ProjectProducts.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.ContractName.Equals(model.ContractName) && o.ProjectId.Equals(model.ProjectId) && o.ParentId.Equals(model.ParentId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0023, TextResourceKey.ProjectProduct);
            }

            //if (db.ProjectProducts.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.ContractCode.Equals(model.ContractCode)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0024, TextResourceKey.ProjectProduct);
            //}
        }
        public string CompareContract(ProjectProductsModel model)
        {

            List<ExportCompareModel> listExportCompares = new List<ExportCompareModel>();
            List<string> readerProjectProduct = new List<string>();
            List<string> readerModule = new List<string>();
            int indexx = 1;

            var listData = (from a in db.ProjectProducts.AsNoTracking()
                            join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into ab
                            from b in ab.DefaultIfEmpty()
                            join c in db.Products.AsNoTracking() on a.ProductId equals c.Id into ac
                            from ca in ac.DefaultIfEmpty()
                            where a.ProjectId.Equals(model.ProjectId)
                            orderby a.ContractIndex
                            select new ExportCompareModel
                            {

                                Id = a.Id,
                                ParentId = a.ParentId,
                                Name = a.ContractName,
                                Quantity = a.Quantity,
                                ModuleId = b.Id,
                                ModuleCode = b.Code,
                                Specifications = a.Specifications,
                                ProductId = ca.Id,
                                ProductCode = ca.Code,
                                ContractIndex = a.ContractIndex
                            }).ToList();

            var listParent = listData.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            bool isSearch = false;
            if (listParent.Count() > 0)
            {
                int maxLen = listParent.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                listParent = listParent
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }
            List<ExportCompareModel> listChild = new List<ExportCompareModel>();

            foreach (var parent in listParent)
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(model.Name) && !parent.Name.ToLower().Contains(model.Name.ToLower()))
                {
                    isSearch = false;
                }

                listChild = GetParentChild(parent.Id, listData, model, indexx.ToString());
                if (isSearch || listChild.Count > 0)
                {
                    //parent.Index = indexx.ToString();
                    listExportCompares.Add(parent);
                    indexx++;
                }

                //int maxLen = listChild.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                //Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                //listChild = listChild
                //           .Select(s =>
                //               new
                //               {
                //                   OrgStr = s,
                //                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                //               })
                //           .OrderBy(x => x.SortStr)
                //           .Select(x => x.OrgStr).ToList();

                listExportCompares.AddRange(listChild);
            }

            List<ExportCompareModel> listExportTemp = new List<ExportCompareModel>();

            foreach (var item in listExportCompares)
            {
                listExportTemp.Add(item);
                if (!string.IsNullOrEmpty(item.ProductCode))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(item.ModuleId))
                {
                    var moduleCode = "";
                    var module = db.Modules.FirstOrDefault(a => a.Id.Equals(item.ModuleId));
                    if (module != null)
                    {
                        moduleCode = module.Code;
                    }
                    if (!string.IsNullOrEmpty(item.Specifications))
                    {
                        readerProjectProduct = item.Specifications.Split('\n').ToList();
                        readerModule = module.Specification.Split('\n').ToList();
                        foreach (var ite in readerProjectProduct)
                        {
                            if (string.IsNullOrEmpty(ite))
                            {
                                continue;
                            }
                            var check = readerModule.Where(a => a.Trim().ToLower().Equals(ite.Trim().ToLower())).ToList();
                            if (check.Count == 0)
                            {
                                if (string.IsNullOrEmpty(module.Specification))
                                {
                                    listExportTemp.Add(new ExportCompareModel
                                    {
                                        Name = ite,
                                        Compare = "Thông số không có trên nguồn",
                                    });
                                }
                                else
                                {
                                    listExportTemp.Add(new ExportCompareModel
                                    {
                                        Name = ite,
                                        Compare = module.Specification,
                                    });
                                }

                            }
                            else
                            {
                                listExportTemp.Add(new ExportCompareModel
                                {
                                    Name = ite,
                                    Compare = "Thông số giống nhau",
                                });
                            }

                        }

                    }
                }
            }

            var listModel = listExportTemp.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TongHopSaiKhacHopDong_Template.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var listExport = listExportTemp.Select(t => new
                {

                    index = t.ContractIndex,
                    t.Name,
                    t.Quantity,
                    a = t.ModuleCode != null ? t.ModuleCode : t.ProductCode,
                    t.Compare,
                });
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 12].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp sai khác hợp đồng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp sai khác hợp đồng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        private List<ExportCompareModel> GetParentChild(string parentId, List<ExportCompareModel> listProjectProduct, ProjectProductsModel modelSearch, string index)
        {
            List<ExportCompareModel> listResult = new List<ExportCompareModel>();
            var listChild = listProjectProduct.Where(r => parentId.Equals(r.ParentId)).ToList();
            bool isSearch = false;
            int indexChild = 1;
            List<ExportCompareModel> listChildChild = new List<ExportCompareModel>();

            int maxLen = listChild.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

            Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

            listChild = listChild
                       .Select(s =>
                           new
                           {
                               OrgStr = s,
                               SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                           })
                       .OrderBy(x => x.SortStr)
                       .Select(x => x.OrgStr).ToList();

            foreach (var child in listChild)
            {

                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.Name) && !child.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                {
                    isSearch = false;
                }

                listChildChild = GetParentChild(child.Id, listProjectProduct, modelSearch, index + "." + indexChild);
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

        public ReturnModel Specification(IWorksheet sheet, string specifications, int y, int rowCount)
        {
            var check = sheet[y, 2].Value;
            var name = sheet[y, 3].Value;
            int a = y;

            for (int x = a; x <= rowCount; x++)
            {
                if (string.IsNullOrEmpty(check) && !string.IsNullOrEmpty(name))
                {
                    specifications += sheet[x, 3].Value + "\n";
                    check = sheet[x + 1, 2].Value;
                    name = sheet[x + 1, 3].Value;
                    if (x == rowCount)
                    {
                        y = x;
                    }
                }
                else { y = x - 1; break; }
            }
            string data = specifications;
            return new ReturnModel
            {
                Specification = data,
                Index = y
            };
        }

        public void ImportProduct(string userId, HttpPostedFile file, string projectId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            var checkProjectProduct = db.ProjectProducts.AsNoTracking().FirstOrDefault(e => projectId.Equals(e.ProjectId));
            if (checkProjectProduct != null)
            {
                throw NTSException.CreateInstance("Bạn chỉ có thể import dự án chưa có sản phẩm!");
            }
            string index, contractCode, contractName, specifications, quantity, note, dataType, moduleStatus, designStatus, price, codeDesign, deliveryDate, expectedDesignFinishDate, expectedMakeFinishDate, expectedTransferDate, isGeneralDesign;
            #region
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<ProjectProduct> list = new List<ProjectProduct>();
            List<Plan> listPlan = new List<Plan>();
            ProjectProduct itemC;
            List<int> rowName = new List<int>();
            List<int> rowDataType = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckDataType = new List<int>();
            List<int> rowCheckPrice = new List<int>();
            List<int> rowCheckModuleStatus = new List<int>();
            List<int> rowCheckDesignStatus = new List<int>();
            List<int> rowCheckDeliveryDate = new List<int>();
            List<int> rowCheckTransferDate = new List<int>();
            List<int> rowCheckExpectedDesignFinishDate = new List<int>();
            List<int> rowCheckExpectedMakeFinishDate = new List<int>();
            List<int> rowCheckExpectedTransferDate = new List<int>();
            List<int> rowCodeDesign = new List<int>();
            List<int> rowCheckCodeDesign = new List<int>();
            List<int> rowIsGeneralDesign = new List<int>();
            List<int> rowCheckIsGeneralDesign = new List<int>();
            string check;
            string name;
            string taskName = "";
            var listModule = db.Modules.AsNoTracking().ToList();
            var listProduct = db.Products.AsNoTracking().ToList();
            var listTaskModuleGroup = db.TaskModuleGroups.AsNoTracking().ToList();
            var listTask = db.Tasks.AsNoTracking().ToList();
            List<ImportModel> listImports = new List<ImportModel>();
            try
            {
                for (int i = 4; i <= rowCount; i++)
                {
                    itemC = new ProjectProduct();
                    itemC.Id = Guid.NewGuid().ToString();
                    index = sheet[i, 2].Value;
                    contractName = sheet[i, 3].Value;
                    contractCode = sheet[i, 10].Value;
                    specifications = "";
                    quantity = sheet[i, 11].Value;
                    note = sheet[i, 8].Value;
                    dataType = sheet[i, 12].Value;
                    price = sheet[i, 13].Value;
                    codeDesign = sheet[i, 14].Value;
                    moduleStatus = sheet[i, 15].Value;
                    designStatus = sheet[i, 16].Value;
                    expectedDesignFinishDate = sheet[i, 17].Value;
                    expectedMakeFinishDate = sheet[i, 18].Value;
                    expectedTransferDate = sheet[i, 20].Value;
                    deliveryDate = sheet[i, 19].Value;
                    //transferDate = sheet[i, 22].Value;
                    isGeneralDesign = sheet[i, 21].Value;

                    //Code
                    if (!string.IsNullOrEmpty(contractCode.Replace(".", "").Replace("-", "")))
                    {
                        itemC.ContractCode = contractCode;
                    }
                    else
                    {
                        itemC.ContractCode = "";
                    }

                    //Name
                    if (!string.IsNullOrEmpty(contractName))
                    {
                        itemC.ContractName = contractName.NTSTrim();
                    }
                    else if (string.IsNullOrEmpty(index) && string.IsNullOrEmpty(contractName))
                    {
                        break;
                    }
                    else
                    {
                        rowName.Add(i);
                    }

                    //ProjectId
                    itemC.ProjectId = projectId;

                    //ParentId
                    try
                    {
                        if (index.LastIndexOf(".") != -1)
                        {
                            var checkParenId = listImports.FirstOrDefault(e => index.Substring(0, index.LastIndexOf(".")).Equals(e.Index));
                            if (checkParenId != null)
                            {
                                itemC.ParentId = checkParenId.Id;
                            }
                        }
                        else
                        {
                            itemC.ParentId = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    itemC.Quantity = 1;
                    //Quantity
                    if (!string.IsNullOrEmpty(quantity))
                    {
                        itemC.Quantity = Convert.ToDecimal(quantity);
                    }

                    decimal realQuantityParent = 1;

                    //ReadQuantity
                    if (!string.IsNullOrEmpty(itemC.ParentId))
                    {
                        var realQuantity = listImports.FirstOrDefault(e => itemC.ParentId.Equals(e.Id));
                        if (realQuantity != null)
                        {
                            realQuantityParent = realQuantity.RealQuantity;
                        }
                    }
                    itemC.RealQuantity = realQuantityParent * itemC.Quantity;

                    listImports.Add(new ImportModel
                    {
                        Index = index,
                        Id = itemC.Id,
                        ParentId = itemC.ParentId,
                        RealQuantity = itemC.RealQuantity
                    });

                    //DataType
                    try
                    {
                        if (!string.IsNullOrEmpty(dataType))
                        {
                            if (dataType.Equals(Constants.ImportProjectProduct_DataType_Practice))
                            {
                                itemC.DataType = Constants.ProjectProduct_DataType_Practice;
                            }
                            else if (dataType.Equals(Constants.ImportProjectProduct_DataType_ProjectProduct))
                            {
                                itemC.DataType = Constants.ProjectProduct_DataType_ProjectProduct;
                            }
                            if (dataType.Equals(Constants.ImportProjectProduct_DataType_Paradigm))
                            {
                                itemC.DataType = Constants.ProjectProduct_DataType_Paradigm;
                            }
                            if (dataType.Equals(Constants.ImportProjectProduct_DataType_Module))
                            {
                                itemC.DataType = Constants.ProjectProduct_DataType_Module;
                            }
                        }
                        else
                        {
                            rowCheckDataType.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowDataType.Add(i);
                        continue;
                    }

                    //DesignStatus
                    try
                    {
                        if (!string.IsNullOrEmpty(designStatus))
                        {
                            if (designStatus.ToLower().Equals(Constants.ProjectProduct_DesignStatus_NewDesign_String))
                            {
                                itemC.DesignStatus = Constants.ProjectProduct_DesignStatus_NewDesign;
                            }
                            else if (designStatus.ToLower().Equals(Constants.ProjectProduct_DesignStatus_UpdateDesign_String))
                            {
                                itemC.DesignStatus = Constants.ProjectProduct_DesignStatus_UpdateDesign;
                            }
                            else if (designStatus.ToLower().Equals(Constants.ProjectProduct_DesignStatus_Use_String))
                            {
                                itemC.DesignStatus = Constants.ProjectProduct_DesignStatus_Use;
                            }
                        }
                        else
                        {
                            itemC.DesignStatus = 0;
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckDesignStatus.Add(i);
                        continue;
                    }

                    //ModuleId && ProductId
                    try
                    {
                        if (!string.IsNullOrEmpty(codeDesign))
                        {
                            if (itemC.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                            {
                                var product = listProduct.FirstOrDefault(o => codeDesign.Trim().ToUpper().Equals(o.Code.ToUpper()));
                                if (product != null)
                                {
                                    itemC.ProductId = product.Id;
                                }
                                else { rowCheckCodeDesign.Add(i); }
                            }
                            else if (itemC.DataType == Constants.ProjectProduct_DataType_Paradigm)
                            {
                                var module = listModule.FirstOrDefault(o => codeDesign.Trim().ToUpper().Equals(o.Code.ToUpper()));
                                if (module != null)
                                {
                                    itemC.ModuleId = module.Id;
                                }
                                else { rowCheckCodeDesign.Add(i); }
                            }
                            else if (itemC.DataType == Constants.ProjectProduct_DataType_Module)
                            {
                                var module = listModule.FirstOrDefault(o => codeDesign.Trim().ToUpper().Equals(o.Code.ToUpper()));
                                if (module != null)
                                {
                                    itemC.ModuleId = module.Id;
                                    var plans = new List<Plan>();
                                    if (itemC.DesignStatus.Equals(Constants.ProjectProduct_DesignStatus_NewDesign))
                                    {

                                    }
                                }
                                else { rowCheckCodeDesign.Add(i); }
                            }
                        }
                        //else
                        //{
                        //    rowCodeDesign.Add(i);
                        //}
                    }
                    catch (Exception)
                    {
                        rowCheckCodeDesign.Add(i);
                        continue;
                    }

                    //ModuleStatus
                    try
                    {
                        if (!string.IsNullOrEmpty(moduleStatus))
                        {
                            if (moduleStatus.ToLower().Equals(Constants.ProjectProduct_ModuleStatus_Project_String))
                            {
                                itemC.ModuleStatus = Constants.ProjectProduct_ModuleStatus_Project;
                            }
                            else if (moduleStatus.ToLower().Equals(Constants.ProjectProduct_ModuleStatus_Additional_String))
                            {
                                itemC.ModuleStatus = Constants.ProjectProduct_ModuleStatus_Additional;
                            }
                            else
                            {
                                rowCheckModuleStatus.Add(i);
                            }
                        }
                        else
                        {
                            itemC.ModuleStatus = Constants.ProjectProduct_ModuleStatus_Project;
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckModuleStatus.Add(i);
                        continue;
                    }


                    //ExpectedDesignFinishDate
                    try
                    {
                        if (!string.IsNullOrEmpty(expectedDesignFinishDate))
                        {
                            itemC.ExpectedDesignFinishDate = Convert.ToDateTime(expectedDesignFinishDate);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckExpectedDesignFinishDate.Add(i);
                        continue;
                    }

                    //ExpectedMakeFinishDate
                    try
                    {
                        if (!string.IsNullOrEmpty(expectedMakeFinishDate))
                        {
                            itemC.ExpectedMakeFinishDate = Convert.ToDateTime(expectedMakeFinishDate);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckExpectedMakeFinishDate.Add(i);
                        continue;
                    }

                    //ExpectedMakeFinishDate
                    try
                    {
                        if (!string.IsNullOrEmpty(expectedTransferDate))
                        {
                            itemC.ExpectedTransferDate = Convert.ToDateTime(expectedTransferDate);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckExpectedTransferDate.Add(i);
                        continue;
                    }

                    //DeliveryDate
                    try
                    {
                        if (!string.IsNullOrEmpty(deliveryDate))
                        {
                            itemC.DeliveryDate = Convert.ToDateTime(deliveryDate);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckDeliveryDate.Add(i);
                        continue;
                    }

                    //Price
                    try
                    {
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = Convert.ToDecimal(price);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckPrice.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(isGeneralDesign))
                    {
                        if (isGeneralDesign.Trim().ToUpper().Equals("Có".ToUpper()))
                        {
                            itemC.IsGeneralDesign = true;
                        }
                        else if (isGeneralDesign.Trim().ToUpper().Equals("Không".ToUpper()))
                        {
                            itemC.IsGeneralDesign = false;
                        }
                        else
                        {
                            rowCheckIsGeneralDesign.Add(i);
                        }
                    }
                    else
                    {
                        rowIsGeneralDesign.Add(i);
                    }

                    //Specifications
                    try
                    {
                        check = sheet[i + 1, 2].Value;
                        name = sheet[i + 1, 3].Value;
                        int y = i + 1;
                        if (string.IsNullOrEmpty(check) && !string.IsNullOrEmpty(name))
                        {
                            var data = Specification(sheet, specifications, y, rowCount);
                            itemC.Specifications = data.Specification;
                            i = data.Index;
                        }
                        else
                        {
                            itemC.Specifications = specifications;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    itemC.ContractIndex = index;
                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;
                    list.Add(itemC);
                }

                #endregion
            }
            catch (Exception ex)
            {
                //fs.Close();
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

            //fs.Close();
            workbook.Close();
            excelEngine.Dispose();

            if (rowCheckName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên hợp đồng dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên hợp đồng dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowDataType.Count > 0)
            {
                throw NTSException.CreateInstance("Kiểu dữ liệu dòng <" + string.Join(", ", rowDataType) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckDataType.Count > 0)
            {
                throw NTSException.CreateInstance("Kiểu dữ liệu dòng <" + string.Join(", ", rowCheckDataType) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCodeDesign.Count > 0)
            {
                throw NTSException.CreateInstance("Mã theo thiết kế dòng <" + string.Join(", ", rowCodeDesign) + "> không được để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckCodeDesign.Count > 0)
            {
                throw NTSException.CreateInstance("Mã theo thiết kế dòng <" + string.Join(", ", rowCheckCodeDesign) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckModuleStatus.Count > 0)
            {
                throw NTSException.CreateInstance("Tình trạng Module dòng <" + string.Join(", ", rowCheckModuleStatus) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckDesignStatus.Count > 0)
            {
                throw NTSException.CreateInstance("Tình trạng thiết kế dòng <" + string.Join(", ", rowCheckDesignStatus) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckExpectedDesignFinishDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày dự kiến hoàn thành thiết kế dòng <" + string.Join(", ", rowCheckExpectedDesignFinishDate) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckExpectedMakeFinishDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày dự kiến hoàn thành sản xuất dòng <" + string.Join(", ", rowCheckExpectedMakeFinishDate) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckExpectedTransferDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày dự kiến chuyển giao dòng <" + string.Join(", ", rowCheckExpectedTransferDate) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckDeliveryDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày giao hàng dòng <" + string.Join(", ", rowCheckDeliveryDate) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckTransferDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày chuyển giao dòng <" + string.Join(", ", rowCheckTransferDate) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckPrice.Count > 0)
            {
                throw NTSException.CreateInstance("Giá dòng <" + string.Join(", ", rowCheckPrice) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowIsGeneralDesign.Count > 0)
            {
                throw NTSException.CreateInstance("Tổng hợp thiết kế dòng <" + string.Join(", ", rowIsGeneralDesign) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckIsGeneralDesign.Count > 0)
            {
                throw NTSException.CreateInstance("Tổng hợp thiết kế dòng <" + string.Join(", ", rowCheckIsGeneralDesign) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.ProjectProducts.AddRange(list);
                    db.Plans.AddRange(listPlan);
                    db.SaveChanges();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }
        }

        /// <summary>
        /// Update tình trạng lập THTK
        /// </summary>
        /// <param name="product"></param>
        public void UpdateIsGeneralDesign(ProjectProductsModel product)
        {
            var projectProduct = db.ProjectProducts.FirstOrDefault(r => r.Id.Equals(product.Id));

            if (projectProduct == null)
            {

            }

            projectProduct.IsGeneralDesign = product.IsGeneralDesign;

            db.SaveChanges();
        }

        public SearchModuleMaterialResultModel<MaterialImportBOMDraftModel> SearchModuleMaterial(ModuleMaterialSearchModel modelSearch)
        {
            SearchModuleMaterialResultModel<MaterialImportBOMDraftModel> searchResult = new SearchModuleMaterialResultModel<MaterialImportBOMDraftModel>();

            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ParentId.Equals(modelSearch.ProjectProductId)
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
                                          ModuleStatus = a.ModuleStatus,
                                          DesignStatus = a.DesignStatus,
                                          DesignFinishDate = a.DesignFinishDate,
                                          MakeFinishDate = a.MakeFinishDate,
                                          DeliveryDate = a.DeliveryDate,
                                          TransferDate = a.TransferDate,
                                          Note = a.Note,
                                          Quantity = a.Quantity,
                                          RealQuantity = a.RealQuantity,
                                          Price = a.Price,
                                          Amount = a.Quantity * a.Price,
                                          ContractIndex = a.ContractIndex,
                                          IsGeneralDesign = a.IsGeneralDesign,
                                          DesignWorkStatus = a.DesignWorkStatus,
                                          DesignCloseDate = a.DesignCloseDate,
                                          GeneralDesignLastDate = a.GeneralDesignLastDate,
                                          MaterialExist = a.MaterialExist,
                                          IsMaterial = true,
                                          IsIncurred = a.IsIncurred,
                                          ModuleName = db.Modules.FirstOrDefault(v => v.Id.Equals(a.ModuleId)).Code,
                                      }).ToList();
            if (listProjectProduct.Count > 0)
            {
                List<MaterialImportBOMDraftModel> listResult = new List<MaterialImportBOMDraftModel>();
                List<MaterialImportBOMDraftModel> listResultDone = new List<MaterialImportBOMDraftModel>();
                foreach (var item1 in listProjectProduct)
                {
                    var dataQuery = (from a in db.MaterialImportBOMDrafts.AsNoTracking()
                                     where a.ProjectId.Equals(item1.Id) && a.ModuleId.Equals(item1.ModuleId) && (a.UpdateStatus == 1 || a.UpdateStatus == 0)
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
                                         Amount = a.Amount,
                                         ParentId = a.ModuleId
                                     }).AsQueryable();
                    if (modelSearch.IsParent)
                    {
                        var dataQueryParent = dataQuery.Where(a => a.MaterialCode.Equals(modelSearch.ContractCode) && !a.Index.Contains("."));
                        listResult.AddRange(dataQueryParent.ToList());
                        foreach (var item in dataQueryParent.ToList())
                        {
                            var listChild = dataQuery.Where(a => a.Index.StartsWith(item.Index + ".")).ToList();
                            listResult.AddRange(listChild);
                        }

                        if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                        {
                            listResult = listResult.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper())).ToList();
                        }
                        if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                        {
                            listResult = listResult.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper())).ToList();
                        }

                        if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                        {
                            listResult = listResult.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper())).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                        {
                            dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
                        }
                        if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                        {
                            dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
                        }

                        if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                        {
                            dataQuery = dataQuery.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper()));
                        }
                        searchResult.TotalItem = dataQuery.Count();
                        listResult = dataQuery.ToList();

                        var productChild = db.ProjectProducts.AsNoTracking().Where(a => a.ParentId.Equals(modelSearch.ProductProjectId)).ToList();
                        foreach (var item in listResult)
                        {
                            if (!item.Index.Contains("."))
                            {
                                var materialExist = productChild.FirstOrDefault(a => a.ContractCode.Equals(item.MaterialCode));
                                if (materialExist == null)
                                {
                                    item.IsNoProductChild = true;
                                }
                            }
                        }
                    }

                    if (listResult.Count() > 0)
                    {
                        int maxLen = listResult.Select(s => s.Index.Length).Max();

                        Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                        var listResult1 = listResult
                                   .Select(s =>
                                       new
                                       {
                                           OrgStr = s,
                                           SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                       })
                                   .OrderBy(x => x.SortStr)
                                   .Select(x => x.OrgStr).ToList();
                        foreach (var item2 in listResult1)
                        {
                            listResultDone.Add(item2);
                        }
                        MaterialImportBOMDraftModel mm = new MaterialImportBOMDraftModel();
                        mm.ModuleName = item1.ModuleName;
                        mm.Id = item1.ModuleId;
                        listResultDone.Add(mm);
                    }
                }
                var count = 0;
                var stt = 1;
                var projectGeneralDesign = db.ProjectGeneralDesigns.Where(a => a.ProjectProductId.Equals(modelSearch.ProjectProductId)).ToList();
                foreach (var projectd in projectGeneralDesign)
                {
                    var listMeterialDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                              where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                              join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                              join c in db.Manufactures.AsNoTracking() on b.ManufactureId equals c.Id
                                              join d in db.Units.AsNoTracking() on b.UnitId equals d.Id
                                              where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                              orderby b.Code
                                              select new MaterialImportBOMDraftModel
                                              {
                                                  Id = a.MaterialId,
                                                  ModuleId = "vtp",
                                                  MaterialName = b.Name,
                                                  MaterialCode = b.Code,
                                                  ManufacturerCode = c.Code,
                                                  Quantity = (int)a.Quantity,
                                                  Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  UnitName = d.Name,
                                                  Amount = (int)a.Quantity * projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  ParentId = "vtp"
                                              }).ToList();

                    var listModuleDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                            where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                            join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                                            where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                            orderby b.Code
                                            select new MaterialImportBOMDraftModel
                                            {
                                                Id = a.MaterialId,
                                                ModuleId = "vtp",
                                                MaterialName = b.Name,
                                                MaterialCode = b.Code,
                                                Quantity = (int)a.Quantity,
                                                ManufacturerCode = Constants.Manufacture_TPA,
                                                UnitName = Constants.Unit_Bo,
                                                Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                ParentId = "vtp"
                                            }).ToList();

                    if (listModuleDesign.Count > 0 && projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_NotApproved)
                    {
                        ModulePriceInfoModel modulePriceInfoModel;
                        foreach (var item in listModuleDesign)
                        {
                            modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(item.Id);
                            if (modulePriceInfoModel != null)
                            {
                                item.Pricing = modulePriceInfoModel.Price;
                            }
                        }
                    }
                    foreach (var module in listModuleDesign)
                    {
                        module.Amount = module.Quantity * module.Pricing;
                    }

                    foreach (var ma in listMeterialDesign)
                    {
                        var material = listResultDone.FirstOrDefault(a => ma.MaterialCode.Equals(a.MaterialCode) && ma.ModuleId.Equals(a.ModuleId));
                        if (material == null)
                        {
                            ma.Index = stt.ToString();
                            listResultDone.Add(ma);
                            stt++;
                        }
                        count++;
                    }
                    foreach (var mo in listModuleDesign)
                    {
                        var module = listResultDone.FirstOrDefault(a => mo.MaterialCode.Equals(a.MaterialCode) && mo.ModuleId.Equals(a.ModuleId));
                        if (module == null)
                        {
                            mo.Index = stt.ToString();
                            listResultDone.Add(mo);
                            stt++;
                        }
                        count++;
                    }
                }
                if (count > 0)
                {
                    MaterialImportBOMDraftModel mm = new MaterialImportBOMDraftModel();
                    mm.ModuleName = "VATTUPHU." + modelSearch.ContractCode;
                    mm.Id = "vtp";
                    listResultDone.Add(mm);
                }
                searchResult.ListResult = listResultDone;
                searchResult.Status9 = 20;
            }
            else
            {
                var dataQuery = (from a in db.MaterialImportBOMDrafts.AsNoTracking()
                                 where a.ProjectId.Equals(modelSearch.ProjectProductId) && a.ModuleId.Equals(modelSearch.ModuleId) && (a.UpdateStatus == 1 || a.UpdateStatus == 0)
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
                                 }).AsQueryable();

                List<MaterialImportBOMDraftModel> listResult = new List<MaterialImportBOMDraftModel>(); ;

                if (modelSearch.IsParent)
                {
                    var dataQueryParent = dataQuery.Where(a => a.MaterialCode.Equals(modelSearch.ContractCode) && !a.Index.Contains("."));
                    listResult.AddRange(dataQueryParent.ToList());
                    foreach (var item in dataQueryParent.ToList())
                    {
                        var listChild = dataQuery.Where(a => a.Index.StartsWith(item.Index + ".")).ToList();
                        listResult.AddRange(listChild);
                    }

                    if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                    {
                        listResult = listResult.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper())).ToList();
                    }
                    if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                    {
                        listResult = listResult.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper())).ToList();
                    }

                    if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                    {
                        listResult = listResult.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper())).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                    {
                        dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                    {
                        dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                    {
                        dataQuery = dataQuery.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper()));
                    }
                    searchResult.TotalItem = dataQuery.Count();
                    listResult = dataQuery.ToList();

                    var productChild = db.ProjectProducts.AsNoTracking().Where(a => a.ParentId.Equals(modelSearch.ProductProjectId)).ToList();
                    foreach (var item in listResult)
                    {
                        if (!item.Index.Contains("."))
                        {
                            var materialExist = productChild.FirstOrDefault(a => a.ContractCode.Equals(item.MaterialCode));
                            if (materialExist == null)
                            {
                                item.IsNoProductChild = true;
                            }
                        }
                    }
                }

                if (listResult.Count() > 0)
                {
                    int maxLen = listResult.Select(s => s.Index.Length).Max();

                    Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                    listResult = listResult
                               .Select(s =>
                                   new
                                   {
                                       OrgStr = s,
                                       SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                   })
                               .OrderBy(x => x.SortStr)
                               .Select(x => x.OrgStr).ToList();
                }

                searchResult.MaxDeliveryDay = listResult.Select(r => r.DeliveryDays).DefaultIfEmpty(0).Max();

                searchResult.ListResult = listResult;
                var materials = db.Materials.AsNoTracking().ToList();
                decimal totalAmount = GetPriceTHTK(searchResult.ListResult, materials);
                searchResult.TotalAmount = totalAmount;
            }
            return searchResult;
        }
        public object GetIncurredMaterial(IncurredMaterialSearchModel model)
        {
            List<IncurredMaterial> incurredMaterials = new List<IncurredMaterial>();
            List<ModuleMaterialResultModel> compareMaterials = new List<ModuleMaterialResultModel>();

            compareMaterials = (from a in db.IncurredMaterials.AsNoTracking()
                                join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                join c in db.Manufactures.AsNoTracking() on b.ManufactureId equals c.Id
                                where a.ProjectProductId.Equals(model.Id)
                                orderby b.Code
                                select new ModuleMaterialResultModel
                                {
                                    MaterialId = a.MaterialId,
                                    MaterialCode = b.Code,
                                    MaterialName = b.Name,
                                    ManufactureName = c.Name,
                                    Quantity = a.Quantity,
                                    LastBuyDate = b.LastBuyDate,
                                    InputPriceDate = b.InputPriceDate,
                                    PriceHistory = b.PriceHistory
                                }).ToList();

            if (!string.IsNullOrEmpty(model.Code))
            {
                compareMaterials.Where(i => i.MaterialCode.ToUpper().Contains(model.Code.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                compareMaterials.Where(i => i.MaterialName.ToUpper().Contains(model.Name.ToUpper())).ToList();
            }

            var day = materialBusiness.GetConfigMaterialLastByDate();
            TimeSpan timeSpan;
            foreach (var item in compareMaterials)
            {
                if (item.LastBuyDate.HasValue)
                {
                    timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }
            }

            decimal total = compareMaterials.Sum(i => i.Amount);

            return new IncurredMaterialResultModel
            {
                Total = total,
                LitsMaterial = compareMaterials
            };
        }

        public int SumWeigthOfPlan(List<Plan> data, string planId, int status, int sumWeigth)
        {
            var plan = data.FirstOrDefault(a => a.Id.Equals(planId));
            if (string.IsNullOrEmpty(plan.ParentId))
            {
                return sumWeigth * plan.Weight;
            }
            else
            {
                SumWeigthOfPlan(data, plan.ParentId, status, sumWeigth * plan.Weight);
            }
            return 0;
        }

        public int SumDoneRatiohOfPlan(Plan data, string parentId, int ratio)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                return ratio * data.Weight;
            }
            else
            {
                var plan = db.Plans.FirstOrDefault(a => a.Id.Equals(parentId));
                if (plan == null)
                {
                    return ratio * data.Weight;
                }
                else
                {
                    SumDoneRatiohOfPlan(plan, plan.ParentId, ratio * data.Weight);
                }
            }
            return 0;
        }
        public int SumWeigth(Plan data, string parentId, int weigth)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                return weigth;
            }
            else
            {
                var plan = db.Plans.FirstOrDefault(a => a.Id.Equals(parentId));
                if (plan == null)
                {
                    return weigth;
                }
                else
                {
                    SumWeigth(plan, plan.ParentId, plan.Weight * weigth);
                }

            }
            return 0;
        }
        private List<ScheduleEntity> GetScheduleProjectChild(string parentId,
          List<ScheduleEntity> listSchedulePrject)
        {
            List<ScheduleEntity> listResult = new List<ScheduleEntity>();
            var listChild = listSchedulePrject.Where(r => parentId.Equals(r.ParentId)).ToList();

            List<ScheduleEntity> listChildChild = new List<ScheduleEntity>();
            TimeSpan timeSpan;
            int dayOff = 0;
            DateTime dateTime = DateTime.Today;
            foreach (var child in listChild)
            {
                listChildChild = GetScheduleProjectChild(child.Id, listSchedulePrject);

                if (listChildChild.Count > 0)
                {
                    if (listChildChild.Where(a => a.PlanStartDate.HasValue).Count() > 0)
                    {
                        child.PlanStartDate = listChildChild.Where(a => a.PlanStartDate.HasValue)?.Min(a => a.PlanStartDate.Value);
                    }

                    if (listChildChild.Where(a => a.PlanDueDate.HasValue).Count() > 0)
                    {
                        child.PlanDueDate = listChildChild.Where(a => a.PlanDueDate.HasValue)?.Max(a => a.PlanDueDate.Value);
                    }

                    if (!child.IsPlan)
                    {
                        if (child.PlanStartDate.HasValue && child.PlanDueDate.HasValue)
                        {
                            child.PlanStartDate = child.PlanStartDate.Value.ToStartDate();
                            child.PlanDueDate = child.PlanDueDate.Value.ToStartDate();

                            timeSpan = new TimeSpan();
                            timeSpan = child.PlanDueDate.Value.Subtract(child.PlanStartDate.Value);

                            dayOff = 0;
                            dayOff = db.Holidays.Where(a => child.PlanStartDate.Value <= a.HolidayDate && a.HolidayDate <= child.PlanDueDate.Value).Count();

                            child.WorkTime = timeSpan.Days + 1 - dayOff;
                        }
                        child.EstimateTime = listChildChild.Where(a => a.ParentId.Equals(child.Id)).Sum(a => a.EstimateTime);
                    }

                    if (string.IsNullOrEmpty(child.StageId))
                    {
                        child.ContractStartDate = listChildChild.Where(a => a.ContractStartDate.HasValue && !a.IsPlan).Select(a => a.ContractStartDate)?.Min();
                        child.ContractDueDate = listChildChild.Where(a => a.ContractDueDate.HasValue && !a.IsPlan).Select(a => a.ContractDueDate)?.Min();
                    }
                }

                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        public SearchModuleMaterialResultModel<QCCheckListModel> SearchCheckList(string id)
        {
            SearchModuleMaterialResultModel<QCCheckListModel> searchModule = new SearchModuleMaterialResultModel<QCCheckListModel>();

            var lisQCCheckList = (from a in db.QCCheckLists.AsNoTracking()
                                  where a.ProjectProductId.Equals(id)
                                  join c in db.ProductStandardGroups on a.ProductStandardGroupId equals c.Id
                                  orderby c.Name, a.Code
                                  select new QCCheckListModel
                                  {
                                      Id = a.Id,
                                      Name = a.Name,
                                      Code = a.Code,
                                      ProductStandardGroupName = c.Name,
                                      Content = a.Content,
                                      Note = a.Note,
                                      ProjectProductId = a.ProjectProductId,
                                      ProductStandardGroupId = a.ProductStandardGroupId,
                                      Target = a.Target,
                                      DataType = a.DataType,
                                      OK_Images = a.OK_Images,
                                      NG_Images = a.NG_Images,

                                  }).AsQueryable();
            var qcResult = db.QCResults;
            var users = db.Users;
            var attach = db.Attachments;
            var list = lisQCCheckList.ToList();
            foreach (var item in list)
            {
                var lisQCResult = db.QCResults.FirstOrDefault(r => r.QCCheckListId.Equals(item.Id));
                if (lisQCResult != null)
                {
                    if (!string.IsNullOrEmpty(lisQCResult.QCBy))
                    {
                        item.CreateBy = users.FirstOrDefault(a => a.Id.Equals(lisQCResult.QCBy)).UserName;
                    }
                    item.QCDate = lisQCResult.QCDate;
                    if (lisQCResult.Status != null)
                    {
                        item.Status = lisQCResult.Status;
                    }
                    else
                    {
                        item.Status = 0;
                    }
                }
                else
                {
                    item.Status = 0;
                }
                //var lisQCcheck = db.QCCheckLists.FirstOrDefault(r => r.Id.Equals(item.Id));
                //if(lisQCcheck != null)
                //{
                //     var Listattach = attach.Where(r => r.ObjectId.Equals(lisQCcheck.Id)).ToList();
                //    if(Listattach.Count > 0)
                //    {
                //        item.ListFile.AddRange(Listattach);
                //    }
                //}


            }
            searchModule.ListResult = list;
            return searchModule;

        }

        public bool AddChecklisst(ProductStandardsModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    QCCheckList qCCheckList = new QCCheckList
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductStandardGroupId = model.ProductStandardGroupId,
                        ProjectProductId = model.ProjectProductId,
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Content = model.Content,
                        Target = model.Target,
                        Note = model.Note,
                        DataType = model.DataType,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    var listOkImage = (from s in model.ListImage
                                       select s.FilePath).ToList();
                    var lisNGImage = (from s in model.ListImageV
                                      select s.FilePath).ToList();
                    qCCheckList.OK_Images = string.Join(",", listOkImage);
                    qCCheckList.NG_Images = string.Join(",", lisNGImage);
                    db.QCCheckLists.Add(qCCheckList);
                    //Add file
                    if (model.ListAttach.Count > 0)
                    {
                        AttachmentBusiness attachment = new AttachmentBusiness();
                        attachment.CreatFile(model.ListAttach, model.CreateBy, qCCheckList.Id);

                    }

                    UserLogUtil.LogHistotyAdd(db, qCCheckList.CreateBy, qCCheckList.Code, qCCheckList.Id, Constants.LOG_ProductStandard);

                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return rs;
        }

        public void Delete(string id)
        {
            //var date = DateTime.Now;
            //DateTime dt = DateTime.Parse(date.ToShortDateString());
            var request = db.QCCheckLists.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {


                    var qCResult = db.QCResults.Where(a => a.QCCheckListId.Equals(id));

                    db.QCResults.RemoveRange(qCResult);
                    db.QCCheckLists.Remove(request);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }

        public void CopyQCCheckList(string id, CopyQCCheckListModel model)
        {

            QCCheckList qCCheckList;
            Attachment attachment;
            var list = model.ListCheck;
            foreach (var item in model.ListProjectProductId.ToList())
            {
                var listQC = db.QCCheckLists.Where(r => r.ProjectProductId.Equals(item)).Select(r => r.Code).ToList();
                if (listQC.Count > 0)
                {
                    foreach (var itm in model.ListCheck.ToList())
                    {
                        var idQCCheckList = string.Join(";", listQC);
                        if (idQCCheckList.Contains(itm.Code))
                        {

                        }
                        else
                        {
                            qCCheckList = new QCCheckList
                            {
                                Id = Guid.NewGuid().ToString(),
                                Code = itm.Code,
                                Content = itm.Content,
                                CreateBy = id,
                                CreateDate = DateTime.Now,
                                Name = itm.Name,
                                NG_Images = itm.NG_Images,
                                Note = itm.Note,
                                OK_Images = itm.OK_Images,
                                ProductStandardGroupId = itm.ProductStandardGroupId,
                                UpdateBy = id,
                                UpdateDate = DateTime.Now,
                                ProjectProductId = item,
                                Target = itm.Target,
                                DataType = itm.DataType,
                            };
                            db.QCCheckLists.Add(qCCheckList);
                            var listAttch = db.Attachments.Where(r => r.ObjectId.Equals(itm.Id)).ToList();
                            attachment = new Attachment();
                            foreach (var attch in listAttch)
                            {
                                attachment = new Attachment
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    CreateBy = id,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = id,
                                    UpdateDate = DateTime.Now,
                                    ObjectId = qCCheckList.Id,
                                    Name = attch.Name,
                                    FileName = attch.FileName,
                                    FilePath = attch.FilePath,
                                    Size = attch.Size,
                                    Extention = attch.Extention,
                                    Thumbnail = attch.Thumbnail,
                                    HashValue = attch.HashValue,
                                    Description = attch.Description,
                                };
                                db.Attachments.Add(attachment);
                            }
                        }
                    }

                }
                else
                {
                    foreach (var itm in model.ListCheck.ToList())
                    {

                        qCCheckList = new QCCheckList
                        {
                            Id = Guid.NewGuid().ToString(),
                            Code = itm.Code,
                            Content = itm.Content,
                            CreateBy = id,
                            CreateDate = DateTime.Now,
                            Name = itm.Name,
                            NG_Images = itm.NG_Images,
                            Note = itm.Note,
                            OK_Images = itm.OK_Images,
                            ProductStandardGroupId = itm.ProductStandardGroupId,
                            UpdateBy = id,
                            UpdateDate = DateTime.Now,
                            ProjectProductId = item,
                            Target = itm.Target,
                            DataType = itm.DataType,
                        };
                        db.QCCheckLists.Add(qCCheckList);
                        var listAttch = db.Attachments.Where(r => r.ObjectId.Equals(itm.Id)).ToList();
                        attachment = new Attachment();
                        foreach (var attch in listAttch)
                        {
                            attachment = new Attachment
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreateBy = id,
                                CreateDate = DateTime.Now,
                                UpdateBy = id,
                                UpdateDate = DateTime.Now,
                                ObjectId = qCCheckList.Id,
                                Name = attch.Name,
                                FileName = attch.FileName,
                                FilePath = attch.FilePath,
                                Size = attch.Size,
                                Extention = attch.Extention,
                                Thumbnail = attch.Thumbnail,
                                HashValue = attch.HashValue,
                                Description = attch.Description,
                            };
                            db.Attachments.Add(attachment);
                        }
                    }


                }
            }


            db.SaveChanges();

        }

        public void SelectStandard(string id, CopyQCCheckListModel model)
        {
            var projectProduct = db.ProjectProducts.FirstOrDefault(r => r.Id.Equals(model.ProjectProductId));
            var listProductStandard = db.ProductStandards;
            var listAllProductStandardAttach = db.ProductStandardAttaches.AsNoTracking();
            var lisQCCheckList = db.QCCheckLists;
            AttachmentBusiness attachment = new AttachmentBusiness();
            List<NTS.Model.Combobox.AttachmentModel> attachments = new List<NTS.Model.Combobox.AttachmentModel>();
            QCCheckList qCCheckList;
            if (projectProduct != null)
            {
                if (!string.IsNullOrEmpty(projectProduct.ModuleId))
                {
                    var module = db.Modules.FirstOrDefault(r => r.Id.Equals(projectProduct.ModuleId)).ModuleGroupId;
                    if (!string.IsNullOrEmpty(module))
                    {
                        var moduleGroupProductStandard = db.ModuleGroupProductStandards.Where(r => r.ModuleGroupId.Equals(module));
                        if (moduleGroupProductStandard.ToList().Count > 0)
                        {
                            foreach (var item in moduleGroupProductStandard)
                            {
                                var productStandard = listProductStandard.FirstOrDefault(r => r.Id.Equals(item.ProductStandardId));
                                if (productStandard != null)
                                {
                                    List<string> listid = new List<string>();
                                    var list = lisQCCheckList.Where(r => r.ProjectProductId.Equals(model.ProjectProductId)).Select(r => r.Code).ToList();
                                    var idQCCheckList = string.Join(";", list);
                                    if (!string.IsNullOrEmpty(idQCCheckList))
                                    {
                                        if (idQCCheckList.Contains(productStandard.Code))
                                        {

                                        }
                                        else
                                        {

                                            qCCheckList = new QCCheckList
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Code = productStandard.Code,
                                                Content = productStandard.Content,
                                                CreateBy = id,
                                                CreateDate = DateTime.Now,
                                                Name = productStandard.Name,
                                                NG_Images = productStandard.NG_Images,
                                                Note = productStandard.Note,
                                                OK_Images = productStandard.OK_Images,
                                                ProductStandardGroupId = productStandard.ProductStandardGroupId,
                                                UpdateBy = id,
                                                UpdateDate = DateTime.Now,
                                                ProjectProductId = model.ProjectProductId,
                                            };
                                            db.QCCheckLists.Add(qCCheckList);

                                            var listAttach = listAllProductStandardAttach.Where(r => r.ProductStandardId.Equals(item.ProductStandardId)).AsNoTracking().ToList();
                                            foreach (var itm in listAttach)
                                            {
                                                NTS.Model.Combobox.AttachmentModel attach = new NTS.Model.Combobox.AttachmentModel
                                                {
                                                    FileName = itm.FileName,
                                                    FilePath = itm.Path,
                                                    Size = itm.FileSize,

                                                };
                                                attachments.Add(attach);


                                            }
                                            attachment.CreatFile(attachments, id, qCCheckList.Id);
                                        }
                                    }
                                    else
                                    {
                                        qCCheckList = new QCCheckList
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Code = productStandard.Code,
                                            Content = productStandard.Content,
                                            CreateBy = id,
                                            CreateDate = DateTime.Now,
                                            Name = productStandard.Name,
                                            NG_Images = productStandard.NG_Images,
                                            Note = productStandard.Note,
                                            OK_Images = productStandard.OK_Images,
                                            ProductStandardGroupId = productStandard.ProductStandardGroupId,
                                            UpdateBy = id,
                                            UpdateDate = DateTime.Now,
                                            ProjectProductId = model.ProjectProductId,
                                        };
                                        db.QCCheckLists.Add(qCCheckList);

                                        if (listAllProductStandardAttach.ToList().Count > 0)
                                        {
                                            var listAttach = listAllProductStandardAttach.Where(r => r.ProductStandard.Equals(item.ProductStandardId)).AsNoTracking().ToList();
                                            foreach (var itm in listAttach)
                                            {
                                                NTS.Model.Combobox.AttachmentModel attach = new NTS.Model.Combobox.AttachmentModel
                                                {
                                                    FileName = itm.FileName,
                                                    FilePath = itm.Path,
                                                    Size = itm.FileSize,

                                                };
                                                attachments.Add(attach);


                                            }
                                            attachment.CreatFile(attachments, id, qCCheckList.Id);
                                        }

                                    }

                                }

                            }
                        }
                    }
                }
            }
            db.SaveChanges();


        }
        public decimal GetPriceTHTK(List<MaterialImportBOMDraftModel> listmaterial, List<Material> materials)
        {
            decimal priceTHTK = 0;
            var modules = listmaterial.Where(a => string.IsNullOrEmpty(a.Index)).ToList();
            if (modules.Count != 0)
            {
                foreach (var item in modules)
                {
                    var listMaterialOfModule = listmaterial.Where(a => item.Id.Equals(a.ParentId)).ToList();
                    var price = GetPriceModule(listMaterialOfModule, materials);
                    priceTHTK = priceTHTK + price;
                }
            }
            else
            {
                var price = GetPriceModule(listmaterial, materials);
                priceTHTK = priceTHTK + price;
            }
            return priceTHTK;
        }

        public decimal GetPriceModule(List<MaterialImportBOMDraftModel> listmaterial, List<Material> materials)
        {
            var listResult = (from a in listmaterial
                              join c in materials on a.MaterialCode equals c.Code
                              orderby a.MaterialCode
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
                              }).ToList();
            decimal priceTHTK = 0;
            if (listResult.Count == listmaterial.Count)
            {
                moduleMaterialBusiness.UpdateMaterialPrice(listResult);
                priceTHTK = listResult.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);
            }
            else
            {
                var data = (from a in listmaterial
                            orderby a.MaterialCode
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
                                Pricing = a.Pricing,
                                Index = a.Index,
                                Path = a.Path,
                                FileName = a.FileName,
                            }).ToList();
                moduleMaterialBusiness.UpdateMaterialPrice(data);
                priceTHTK = data.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);
            }
            return priceTHTK;
        }
        public List<ScheduleEntity> GetListPlanByProjectId(string id)
        {
            // Lấy dữ liệu kế hoạch trong danh mục sản phẩm của dự án
            var projectProducts = (from a in db.ProjectProducts.AsNoTracking().Where(r => r.ProjectId.Equals(id))
                                   join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into
                                   ab
                                   from ba in ab.DefaultIfEmpty()
                                   join c in db.Products.AsNoTracking() on a.ProductId equals c.Id into
                                   ac
                                   from ca in ac.DefaultIfEmpty()
                                   select new ScheduleEntity()
                                   {
                                       Id = a.Id,
                                       ParentId = a.ParentId,
                                       RealQuantity = a.RealQuantity,
                                       Weight = a.Weight,
                                       ContractCode = string.IsNullOrEmpty(ba.Code) ? ca.Code : ba.Code,
                                       ContractName = a.ContractName,
                                       NameView = a.ContractName,
                                       ContractIndex = a.ContractIndex,
                                       ContractStartDate = a.ContractStartDate,
                                       ContractDueDate = a.ContractDueDate,
                                       PlanStartDate = a.PlanStartDate,
                                       PlanDueDate = a.PlanDueDate,
                                       Duration = a.Duration,
                                       DoneRatio = a.DoneRatio,
                                       DataType = a.DataType,
                                       Status = a.Status,
                                       ProjectId = id,
                                       ModuleStatus = a.ModuleStatus

                                   }
                           ).ToList();

            // Lấy danh sách công việc chi tiết
            var plans = (from a in db.Plans.AsNoTracking().Where(r => r.ProjectId.Equals(id))
                         join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                         orderby b.index, a.Index
                         select new ScheduleEntity()
                         {
                             Id = a.Id,
                             ProjectProductId = a.ProjectProductId,
                             ParentId = a.ParentId != null ? a.ParentId : a.ProjectProductId,
                             StageName = a.ParentId == null ? a.Name.ToUpper() : string.Empty,
                             PlanName = a.ParentId != null ? a.Name.ToUpper() : string.Empty,
                             BackgroundColor = b.Color,
                             StageId = a.StageId,
                             ContractStartDate = a.ContractStartDate,
                             ContractDueDate = a.ContractDueDate,
                             PlanStartDate = a.PlanStartDate,
                             PlanDueDate = a.PlanDueDate,
                             Duration = a.Duration,
                             DoneRatio = a.DoneRatio,
                             Color = b.Color,
                             Weight = a.Weight,
                             IsPlan = a.IsPlan,
                             EstimateTime = a.EstimateTime,
                             Status = a.Status,
                             SupplierId = a.SupplierId,
                             Type = a.Type,
                             Index = b.index,
                             IndexPlan = a.Index,
                             CreateDate = a.CreateDate,
                             Description = a.Description,
                             ProjectId = id
                         }).ToList();

            var count = 0;

            if (projectProducts.Count() > 0)
            {
                int maxLen = projectProducts.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                projectProducts = projectProducts
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }
            // Update lại tên của Sản phẩm/ Module trên Plan
            foreach (var item in projectProducts)
            {
                string nameView = string.Empty;
                if (item.DataType == Constants.ProjectProduct_DataType_Practice)
                {
                    nameView = item.ContractIndex + " - BTH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    nameView = item.ContractIndex + " - SP - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    nameView = item.ContractIndex + " - M - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    nameView = item.ContractIndex + " - MH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
            }


            var planAssignment = db.PlanAssignments.AsNoTracking().ToList();
            var users = db.Users.AsNoTracking().ToList();

            DateTime dateTime = DateTime.Today;

            List<ScheduleEntity> schedules = projectProducts.Union(plans).ToList();

            foreach (var item in schedules)
            {
                if (!item.IsPlan)
                {
                    item.EstimateTime = null;
                }

                #region Tình trạng công việc
                // Nếu là Công việc
                if (item.IsPlan)
                {
                    if (!item.PlanStartDate.HasValue || !item.PlanDueDate.HasValue)
                    {
                        item.InternalStatus = "THIẾU NGÀY TK";
                    }
                    else if (dateTime > item.PlanDueDate && item.DoneRatio < 100)
                    {
                        item.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                    else
                    {
                        item.InternalStatus = "OK";
                    }
                }
                else
                {
                    if (item.PlanDueDate > item.ContractDueDate)
                    {
                        item.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                    }
                    else if (dateTime > item.PlanDueDate && item.DoneRatio < 100)
                    {
                        item.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                }
                #endregion

                item.ListIdUserId = planAssignment.Where(a => a.PlanId.Equals(item.Id)).OrderBy(a => a.IsMain).Select(a => a.UserId).ToList();
                item.ResponsiblePersionName = string.Join(", ", (from s in planAssignment
                                                                 where s.PlanId.Equals(item.Id)
                                                                 join m in users on s.UserId equals m.Id
                                                                 orderby s.IsMain descending
                                                                 select m.UserName).ToArray());

            }

            return schedules;
        }

    }
}
