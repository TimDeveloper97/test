using Microsoft.VisualBasic.ApplicationServices;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.CustomerContact;
using NTS.Model.IncurredMaterial;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterialFinishDesign;
using NTS.Model.ModuleMaterials;
using NTS.Model.Plans;
using NTS.Model.Practice;
using NTS.Model.Project;
using NTS.Model.ProjectProducts;
using NTS.Model.Quotation;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.Survey;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.QLTKMG;
using QLTK.Business.Solutions;
using RabbitMQ.Util;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.Collections.Grouping;
using Syncfusion.XPS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLTK.Business.ProjectProducts
{
    public class ProjectProductQcBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private MaterialBusiness materialBusiness = new MaterialBusiness();

        public ProjectProductResultModel SearchProjectProduct(ProjectProductsSearchModel modelSearch)
        {
            ProjectProductResultModel searchResult = new ProjectProductResultModel();

            List<string> projectProductIds = new List<string>();
            if (!string.IsNullOrEmpty(modelSearch.MaterialName))
            {
                projectProductIds = db.ModuleMaterialFinishDesigns.AsNoTracking().Where(r => r.MaterialName.ToLower().Equals(modelSearch.MaterialName.ToLower()) || r.MaterialCode.ToLower().Equals(modelSearch.MaterialName.ToLower())).Select(s => s.ProjectProductId).ToList();
            }

            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ProjectId.Equals(modelSearch.ProjectId) && a.IsNeedQC
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
                                          QCQuantity = a.QCQuantity,
                                          ProjectProductId = a.Id
                                      }).AsQueryable();

            //var countParent = listProjectProduct.Where(t => string.IsNullOrEmpty(t.ParentId)).ToList().Count;
            if (!string.IsNullOrEmpty(modelSearch.Id))
            {
                listProjectProduct = listProjectProduct.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
            }

            var list = listProjectProduct.ToList();
            Module module;
            Product product;
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
            foreach (var item in list.ToList())
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
            List<ProjectProductsModel> listProjectProductQc = new List<ProjectProductsModel>();
            List<ProjectProductsModel> listProjectProductDoQc = new List<ProjectProductsModel>();
            List<ProjectProductsModel> listProjectProductNotQc = new List<ProjectProductsModel>();
            var listProductItem = db.ProductItems.ToList();
            ProjectProductsModel productQc;
            foreach (var item in listResult)
            {
                var listProductQc = listProductItem.Where(r => r.ProjectProductId.Equals(item.Id)).OrderBy(r => r.SerialNumber).ToList();
                var listQc = listProductItem.Where(r => r.ProjectProductId.Equals(item.Id) && r.QCStatus != 0).OrderBy(r => r.SerialNumber).ToList();
                if (listProductQc.Count > 0)
                {

                    if (item.QCQuantity == listProductQc.Count)
                    {
                        foreach (var itm in listProductQc)
                        {
                            if (itm.QCStatus != 0)
                            {
                                productQc = new ProjectProductsModel()
                                {
                                    ContractCode = item.ContractCode,
                                    ContractName = item.ContractName,
                                    Code = item.Code,
                                    Name = item.Name,
                                    DatatypeName = item.DatatypeName,
                                    ModuleStatusName = item.ModuleStatusName,
                                    DesignStatusName = item.DesignStatusName,
                                    ProjectProductId = item.Id,
                                    SerialNumber = itm.SerialNumber,
                                    QCStatus = itm.QCStatus,
                                };

                                listProjectProductQc.Add(productQc);

                            }
                            else
                            {
                                productQc = new ProjectProductsModel()
                                {
                                    ContractCode = item.ContractCode,
                                    ContractName = item.ContractName,
                                    Code = item.Code,
                                    Name = item.Name,
                                    DatatypeName = item.DatatypeName,
                                    ModuleStatusName = item.ModuleStatusName,
                                    DesignStatusName = item.DesignStatusName,
                                    ProjectProductId = item.Id,
                                    QCStatus = itm.QCStatus,
                                };

                                listProjectProductQc.Add(productQc);
                            }

                        }

                    }
                    else
                    {
                        int m = 0;
                        if (listQc.Count >= item.QCQuantity)
                        {
                            foreach (var itm in listProductQc)
                            {
                                if (itm.QCStatus != 0)
                                {
                                    productQc = new ProjectProductsModel()
                                    {
                                        ContractCode = item.ContractCode,
                                        ContractName = item.ContractName,
                                        Code = item.Code,
                                        Name = item.Name,
                                        DatatypeName = item.DatatypeName,
                                        ModuleStatusName = item.ModuleStatusName,
                                        DesignStatusName = item.DesignStatusName,
                                        ProjectProductId = item.Id,
                                        SerialNumber = itm.SerialNumber,
                                        QCStatus = itm.QCStatus,
                                    };
                                    m++;
                                    if (m <= item.Quantity)
                                    {
                                        listProjectProductQc.Add(productQc);
                                    }

                                }

                            }


                        }
                        else
                        {
                            foreach (var itm in listProductQc)
                            {
                                if (itm.QCStatus != 0)
                                {
                                    productQc = new ProjectProductsModel()
                                    {
                                        ContractCode = item.ContractCode,
                                        ContractName = item.ContractName,
                                        Code = item.Code,
                                        Name = item.Name,
                                        DatatypeName = item.DatatypeName,
                                        ModuleStatusName = item.ModuleStatusName,
                                        DesignStatusName = item.DesignStatusName,
                                        ProjectProductId = item.Id,
                                        SerialNumber = itm.SerialNumber,
                                        QCStatus = itm.QCStatus,
                                    };
                                    listProjectProductQc.Add(productQc);

                                }

                            }
                            for(int i=0; i<= item.QCQuantity - listQc.Count; i++)
                            {
                                listProjectProductQc.Add(item);
                            }

                        }

                    }



                }


            }
            if (!string.IsNullOrEmpty(modelSearch.QCStatus))
            {
                listProjectProductQc = listProjectProductQc.Where(r => r.QCStatus == Int32.Parse(modelSearch.QCStatus)).ToList();
            }
            searchResult.ListResult = listProjectProductQc;
            searchResult.ToTalAmount = listFather.Sum(i => i.Amount);
            searchResult.ToTalAmountTHTK = listFather.Sum(i => i.AmountTHTK);
            searchResult.TotalAmountIncurred = listFather.Sum(i => i.AmountIncurred);
            searchResult.ColorTHTK = listFather.FirstOrDefault(i => !i.ColorGeneralDesign) != null ? true : false;
            searchResult.Total = listProjectProductQc.Count();
            return searchResult;
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

        public string ExportExcelQC(ProjectProductsModel model)
        {


            var listProjectProductItem = (from a in db.ProductItems.AsNoTracking()
                                          where a.ProjectId.Equals(model.ProjectId)
                                          join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
                                          orderby a.ProjectProductId
                                          select new ProjectProductsModel
                                          {
                                              Id = a.Id,
                                              ParentId = b.ParentId,
                                              ContractCode = b.ContractCode,
                                              ContractName = b.ContractName,
                                              DataType = b.DataType,
                                              ProductId = b.ProductId,
                                              SerialNumber = a.SerialNumber,
                                              QCStatusName = a.QCStatus == 0 ? "Chưa QC": a.QCStatus == 1 ? "Đang QC": a.QCStatus == 2 ? "Không đạt" : "Đạt",
                                          }).AsQueryable().ToList();
            string parentId = null;
            string newParentId = null;
            var listProjectProducts = db.ProjectProducts.AsNoTracking();
            var listProduct = db.Products.AsNoTracking();
            foreach (var item in listProjectProductItem)
            {
                if (!string.IsNullOrEmpty(item.ParentId))
                {
                    parentId = item.ParentId;
                    do
                    {
                        newParentId = parentId;
                        parentId = listProjectProducts.Where(r => r.Id.Equals(parentId)).FirstOrDefault().ParentId;
                    } while (!string.IsNullOrEmpty(parentId));
                    item.ParentId = newParentId;
                    item.ContractCode = listProjectProducts.Where(r => r.Id.Equals(item.ParentId)).FirstOrDefault().ContractCode;
                    item.ContractName = listProjectProducts.Where(r => r.Id.Equals(item.ParentId)).FirstOrDefault().ContractName;
                }
                if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    var products = listProduct.FirstOrDefault(i => i.Id.Equals(item.ProductId));
                    if (products != null)
                    {
                        item.Code = products.Code;
                        item.Name = products.Name;
                    }
                }
            }
            var listData = listProjectProductItem;

            var project = db.Projects.AsNoTracking().Where(r => r.Id.Equals(model.ProjectId)).FirstOrDefault();


            var listModel = listData.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/SanPhamCanQc.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange rangeProjectCode = sheet.FindFirst("<projectcode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeProjectCode.Text = rangeProjectCode.Text.Replace("<projectcode>", (project.Code));
                IRange rangeProjectName = sheet.FindFirst("<projectname>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeProjectName.Text = rangeProjectName.Text.Replace("<projectname>", (project.Name));

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listData.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ContractName,
                    a.ContractCode,
                    a.Name,
                    a.Code,
                    a.SerialNumber,
                    a.QCStatusName


                });
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders.Color = ExcelKnownColors.Black;
                sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 12].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Sản Phẩm Cần Qc" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Sản Phẩm Cần Qc" + ".xls";

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

        public ProjectProductResultModel searchShowProjectProductQc(ProjectProductsSearchModel modelSearch)
        {
            ProjectProductResultModel searchResult = new ProjectProductResultModel();
            var lisProductItem = (from a in db.ProductItems.AsNoTracking()
                                  where a.ProjectId.Equals(modelSearch.ProjectId)
                                  join b in db.ProjectProducts on a.ProjectProductId equals b.Id
                                  select new ProjectProductsModel
                                  {
                                      Id = a.Id,
                                      ProjectProductId = a.ProjectProductId,
                                      ContractCode = b.ContractCode,
                                      ContractName = b.ContractName,
                                      SerialNumber = a.SerialNumber,
                                      QCStatus = a.QCStatus,
                                      DesignStatusName = b.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : b.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : b.DesignStatus == Constants.ProjectProduct_DesignStatus_Use ? "Tận dụng" : b.DesignStatus == Constants.ProjectProduct_DesignStatus_DesignStatus ? "Hàng bán thẳng" : "",
                                      ModuleStatusName = b.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung",
                                      DatatypeName = b.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : b.DataType == Constants.ProjectProduct_DataType_ProjectProduct ? "Sản phẩm/Lai sản xuất" : b.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình/máy" : "Module",
                                      //Name = !string.IsNullOrEmpty(b.ProductId) ? db.Products.FirstOrDefault(i => b.ProductId.Equals(i.Id)).Name : "",
                                      //Code = !string.IsNullOrEmpty(b.ProductId) ? db.Products.FirstOrDefault(i => b.ProductId.Equals(i.Id)).Code : "",
                                      ParentId = a.ProjectProductId,
                                  }).ToList();


            var groupProductItem = lisProductItem.GroupBy(g => g.ProjectProductId).Select(s => s.Key).ToList();
            var listProjectProduct = db.ProjectProducts.ToList();
            var lisProductQc = db.ProductItems.ToList();
            string parentId = null;
            List<ProjectProductsModel> newLisProductItem = new List<ProjectProductsModel>();
            newLisProductItem.AddRange(lisProductItem);
            ProjectProductsModel newProjectProduct;
            foreach (var item in groupProductItem)
            {
                int qc = lisProductQc.Where(r => r.ProjectProductId.Equals(item)).Count();
                int qcreached = lisProductQc.Where(r => r.QCStatus == 2).Count();
                int qcfailed = lisProductQc.Where(r => r.QCStatus == 3).Count();
                var projectproduct = listProjectProduct.Where(r => r.Id.Equals(item)).FirstOrDefault();

                newProjectProduct = new ProjectProductsModel()
                {
                    Id = projectproduct.Id,
                    ProjectProductId = projectproduct.Id,
                    ContractCode = projectproduct.ContractCode,
                    ContractName = projectproduct.ContractName,
                    SerialNumber = $"{qcreached}/{qcfailed}/{qc}",
                    QCStatus = 5,
                    DesignStatusName = projectproduct.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : projectproduct.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : projectproduct.DesignStatus == Constants.ProjectProduct_DesignStatus_Use ? "Tận dụng" : projectproduct.DesignStatus == Constants.ProjectProduct_DesignStatus_DesignStatus ? "Hàng bán thẳng" : "",
                    ModuleStatusName = projectproduct.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung",
                    DatatypeName = projectproduct.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : projectproduct.DataType == Constants.ProjectProduct_DataType_ProjectProduct ? "Sản phẩm/Lai sản xuất" : projectproduct.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình/máy" : "Module",
                    //Name = !string.IsNullOrEmpty(projectproduct.ProductId) ? db.Products.FirstOrDefault(i => projectproduct.ProductId.Equals(i.Id)).Name : "",
                    //Code = !string.IsNullOrEmpty(projectproduct.ProductId) ? db.Products.FirstOrDefault(i => projectproduct.ProductId.Equals(i.Id)).Code : "",
                    ParentId = projectproduct.ParentId,
                };
                if (!string.IsNullOrEmpty(projectproduct.ProductId))
                {
                    newProjectProduct.Name = db.Products.FirstOrDefault(i => projectproduct.ProductId.Equals(i.Id)).Name;
                    newProjectProduct.Code = db.Products.FirstOrDefault(i => projectproduct.ProductId.Equals(i.Id)).Code;
                }
                if (!string.IsNullOrEmpty(projectproduct.ModuleId))
                {
                    newProjectProduct.Name = db.Modules.FirstOrDefault(i => projectproduct.ModuleId.Equals(i.Id)).Name;
                    newProjectProduct.Code = db.Modules.FirstOrDefault(i => projectproduct.ModuleId.Equals(i.Id)).Code;
                }
                newLisProductItem.Add(newProjectProduct);

                if (!string.IsNullOrEmpty(projectproduct.ParentId))
                {
                    parentId = projectproduct.ParentId;
                    do
                    {

                        var parent = listProjectProduct.Where(r => r.Id.Equals(parentId)).FirstOrDefault();
                        parentId = parent.ParentId;
                        newProjectProduct = new ProjectProductsModel()
                        {
                            Id = parent.Id,
                            ProjectProductId = parent.Id,
                            ContractCode = parent.ContractCode,
                            ContractName = parent.ContractName,
                            SerialNumber = $"{qcreached}/{qcfailed}/{qc}",
                            QCStatus = 5,
                            DesignStatusName = parent.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : parent.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : parent.DesignStatus == Constants.ProjectProduct_DesignStatus_Use ? "Tận dụng" : parent.DesignStatus == Constants.ProjectProduct_DesignStatus_DesignStatus ? "Hàng bán thẳng" : "",
                            ModuleStatusName = parent.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung",
                            DatatypeName = parent.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : parent.DataType == Constants.ProjectProduct_DataType_ProjectProduct ? "Sản phẩm/Lai sản xuất" : parent.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình/máy" : "Module",
                            Name = !string.IsNullOrEmpty(parent.ProductId) ? db.Products.FirstOrDefault(i => parent.ProductId.Equals(i.Id)).Name : !string.IsNullOrEmpty(parent.ModuleId) ? db.Modules.FirstOrDefault(i => parent.ModuleId.Equals(i.Id)).Name : "",
                            Code = !string.IsNullOrEmpty(parent.ProductId) ? db.Products.FirstOrDefault(i => parent.ProductId.Equals(i.Id)).Code : !string.IsNullOrEmpty(parent.ModuleId) ? db.Modules.FirstOrDefault(i => parent.ModuleId.Equals(i.Id)).Code : "",
                            ParentId = parent.ParentId,
                        };
                        newLisProductItem.Add(newProjectProduct);


                    } while (!string.IsNullOrEmpty(parentId));
                }
            }
            searchResult.ListResult = newLisProductItem.ToList();
            return searchResult;
        }

        public ProductsItemModel GetProductItemInfoBySerialNumber(string serialNumber)
        {
            var productsItem = (from a in db.ProductItems.AsNoTracking()
                                where a.SerialNumber.Equals(serialNumber)
                                join b in db.Projects on a.ProjectId equals b.Id
                                join c in db.ProjectProducts on a.ProjectProductId equals c.Id
                                select new ProductsItemModel
                                {
                                    Id = a.Id,
                                    ProjectName = b.Name,
                                    //ProductName = !string.IsNullOrEmpty(c.ProductId) ? db.Products.FirstOrDefault(r => r.Id.Equals(c.ProductId)).Name : !string.IsNullOrEmpty(c.ModuleId) ? db.Modules.FirstOrDefault(r => r.Id.Equals(c.ModuleId)).Name : "",
                                    ProductName = c.ContractName,
                                    QCStatus = a.QCStatus,
                                    ProjectProductId = a.ProjectProductId,
                                    ProductId = c.ProductId,
                                    ModuleId = c.ModuleId,
                                }).FirstOrDefault();
            List<ProductStandardGroupModel> listProductStandardGroupModel = new List<ProductStandardGroupModel>();
            var productStandards = db.ProductStandards.ToList();
            var productStandardsGroup = db.ProductStandardGroups.ToList();
            var qCCheckList = db.QCCheckLists.AsNoTracking().Where(r => r.ProjectProductId.Equals(productsItem.ProjectProductId)).OrderBy(r => r.ProductStandardGroupId).ToList();
            if (!string.IsNullOrEmpty(productsItem.ProductId))
            {

                var listProductImage = (from r in db.ProductImages.AsNoTracking()
                                        where r.ProductId.Equals(productsItem.ProductId)
                                        select r.FilePath).ToList();
                productsItem.ImageLink = listProductImage;



            }
            else if (!string.IsNullOrEmpty(productsItem.ModuleId))
            {
                var listModuleImage = (from r in db.ModuleImages.AsNoTracking()
                                       where r.ModuleId.Equals(productsItem.ModuleId)
                                       select r.FilePath).ToList();
                productsItem.ImageLink = listModuleImage;

            }

            foreach (var item in qCCheckList)
            {
                var moduleProductStandardsGroup = (from a in productStandardsGroup
                                                   where a.Id.Equals(item.ProductStandardGroupId)
                                                   select new ProductStandardGroupModel
                                                   {
                                                       Id = a.Id,
                                                       Name = a.Name,
                                                       Code = a.Code,
                                                   }).FirstOrDefault();
                listProductStandardGroupModel.Add(moduleProductStandardsGroup);
            }
            var QCResult = db.QCResults.ToList();
            var listqCCheckList = db.QCCheckLists.AsNoTracking().Where(r => r.ProjectProductId.Equals(productsItem.ProjectProductId)).ToList();
            List<QCResultModel> checkLit;
            int u = 0;
            List<QCCheckList> listQcCheckByProductStandardsGroupId;
            foreach (var item in listProductStandardGroupModel)
            {
                listQcCheckByProductStandardsGroupId = new List<QCCheckList>();
                checkLit = new List<QCResultModel>();
                int a = 0;
                int b = 0;
                listQcCheckByProductStandardsGroupId = listqCCheckList.Where(r => r.ProductStandardGroupId.Equals(item.Id)).ToList();
                foreach (var itn in listQcCheckByProductStandardsGroupId)
                {
                    var checkQCResults = (from r in QCResult
                                          where r.QCCheckListId.Equals(itn.Id)
                                          select new QCResultModel()
                                          {
                                              QCCheckListId = itn.Id,
                                              Status = r.Status
                                          }).FirstOrDefault();
                    if (checkQCResults != null)
                    {
                        checkLit.Add(checkQCResults);
                        if (checkQCResults.Status == 2)
                        {
                            a++;
                        }
                        else if (checkQCResults.Status == 1)
                        {
                            b++;
                        }
                    }

                }
                if (listQcCheckByProductStandardsGroupId.Count == checkLit.Count)
                {
                    if (a == 0)
                    {
                        item.Status = 2;
                    }
                    else
                    {
                        item.Status = 3;
                        u++;
                    }

                }
                else if (checkLit.Count == 0)
                {
                    item.Status = 0;
                }
                else
                {
                    item.Status = 1;
                }
                item.Total = listQcCheckByProductStandardsGroupId.Count;
                item.TotalNG = a;
            }
            productsItem.Total = listProductStandardGroupModel.Count;
            productsItem.TotalNG = u;
            productsItem.ListStandardsGroup = listProductStandardGroupModel.GroupBy(t => new { t.ProductStandardsGroupId, t.Id, t.Name, t.Code, t.Total, t.TotalNG, t.Status }).Select(b => new ProductStandardGroupModel
            {
                Id = b.Key.Id,
                ProductStandardsGroupId = b.Key.ProductStandardsGroupId,
                Name = b.Key.Name,
                Code = b.Key.Code,
                Total = b.Key.Total,
                TotalNG = b.Key.TotalNG,
                Status = b.Key.Status,
            }).ToList();
            if (productsItem == null)
            {
                throw NTSException.CreateInstance("Sản phẩm này không tồn tại!");
            }

            return productsItem;
        }

        public SearchResultModel<QCCheckListModel> GetListStandardById(QCCheckListModel model)
        {
            SearchResultModel<QCCheckListModel> searchResult = new SearchResultModel<QCCheckListModel>();
            var listQCCheckList = (from a in db.QCCheckLists.AsNoTracking()
                                   where a.ProductStandardGroupId.Equals(model.ProductStandardGroupId) && a.ProjectProductId.Equals(model.ProjectProductId)
                                   orderby a.Code
                                   select new QCCheckListModel
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Code = a.Code,
                                       ProductStandardGroupId = a.ProductStandardGroupId,
                                       NG_Images = a.NG_Images,
                                       OK_Images = a.OK_Images,
                                   }).ToList();
            var qCResult = db.QCResults.ToList();
            var productStandardGroup = db.ProductStandardGroups.ToList();
            List<string> ngImg;
            int v = 0;
            foreach (var item in listQCCheckList.ToList())
            {
                var status = qCResult.FirstOrDefault(r => r.QCCheckListId.Equals(item.Id));
                if (status != null)
                {
                    item.Status = status.Status;
                    if (item.Status == 2)
                    {
                        v++;
                    }
                    item.QCDate = status.QCDate;
                }
                else
                {
                    item.Status = 0;
                    item.QCDate = null;
                }
                item.ProductStandardGroupName = productStandardGroup.FirstOrDefault(r => r.Id.Equals(item.ProductStandardGroupId)).Name;
                if (!string.IsNullOrEmpty(item.NG_Images))
                {
                    item.NGImg = item.NG_Images.Split(';').ToList();
                }
                if (!string.IsNullOrEmpty(item.OK_Images))
                {
                    item.OKImg = item.OK_Images.Split(';').ToList();
                }

            }
            searchResult.ListResult = listQCCheckList.ToList();
            searchResult.TotalItem = listQCCheckList.ToList().Count();
            searchResult.Status1 = v;
            return searchResult;
        }

        public QCCheckListModel GetQCCheckListById(string id)
        {
            var qCCheckList = (from a in db.QCCheckLists.AsNoTracking()
                               where a.Id.Equals(id)
                               join b in db.ProductStandardGroups on a.ProductStandardGroupId equals b.Id
                               select new QCCheckListModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   Code = a.Code,
                                   OK_Images = a.OK_Images,
                                   NG_Images = a.NG_Images,
                                   Content = a.Content,
                                   ProductStandardGroupName = b.Name,
                                   ProductStandardGroupId = a.ProductStandardGroupId,
                               }).FirstOrDefault();
            if (!string.IsNullOrEmpty(qCCheckList.NG_Images))
            {
                qCCheckList.NGImg = qCCheckList.NG_Images.Split(';').ToList();
            }
            if (!string.IsNullOrEmpty(qCCheckList.OK_Images))
            {
                qCCheckList.OKImg = qCCheckList.OK_Images.Split(';').ToList();
            }
            if (qCCheckList != null)
            {
                qCCheckList.QCResult = (from a in db.QCResults.AsNoTracking()
                                        where a.QCCheckListId.Equals(qCCheckList.Id)
                                        select new QCResultModel
                                        {
                                            Id = a.Id,
                                            Status = a.Status,
                                            QCDate = a.QCDate,
                                            Reason = a.Reason,
                                            QCBy = a.QCBy,

                                        }).FirstOrDefault();
                if (qCCheckList.QCResult != null)
                {
                    if (!string.IsNullOrEmpty(qCCheckList.QCResult.QCBy))
                    {
                        qCCheckList.QCResult.QCByName = db.Users.FirstOrDefault(r => r.Id.Equals(qCCheckList.QCResult.QCBy)).UserName;
                        //QCByImagePath = !string.IsNullOrEmpty(b.EmployeeId) ? db.Employees.FirstOrDefault(r => r.Id.Equals(b.EmployeeId)).ImagePath : "",
                        var empoy = db.Users.FirstOrDefault(r => r.Id.Equals(qCCheckList.QCResult.QCBy));
                        qCCheckList.QCResult.QCByImagePath = db.Employees.FirstOrDefault(r => r.Id.Equals(empoy.EmployeeId)).ImagePath;
                    }
                    if (qCCheckList.QCResult != null)
                    {
                        qCCheckList.Attachment = (from a in db.Attachments.AsNoTracking()
                                                  where a.ObjectId.Equals(qCCheckList.QCResult.Id)
                                                  select new QuoteDocumentModel
                                                  {
                                                      Id = a.Id,
                                                      FilePath = a.FilePath,
                                                      FileName = a.FileName,
                                                      Size = a.Size,
                                                  }).ToList();
                    }
                }
            }


            return qCCheckList;
        }

        public void CreatQCResult(QCResultCreatModel model)
        {


            SummaryQuotesBussiness summaryQuotesBussiness = new SummaryQuotesBussiness();
            var qcResult = db.QCResults.Where(r => r.QCCheckListId.Equals(model.QCCheckListId)).FirstOrDefault();
            var attach = db.Attachments;
            if (qcResult != null)
            {
                qcResult.QCCheckListId = model.QCCheckListId;
                qcResult.ProductItemId = model.ProductItemId;
                qcResult.Status = model.Status;
                qcResult.QCDate = DateTime.Now;
                qcResult.QCBy = model.UserId;
                qcResult.Reason = model.Reason;

                var lisAttach = attach.Where(r => r.ObjectId.Equals(qcResult.Id)).ToList();
                if (lisAttach.Count > 0)
                {
                    db.Attachments.RemoveRange(lisAttach);
                }
                if (model.Attachment != null)
                {
                    summaryQuotesBussiness.CreatFile(model.Attachment, model.UserId, qcResult.Id);
                }
            }
            else
            {
                var listQCResult = db.QCResults.Where(r => r.ProductItemId.Equals(model.ProductItemId));
                if (listQCResult.ToList().Count == 0)
                {
                    var productItem = db.ProductItems.FirstOrDefault(r => r.Id.Equals(model.ProductItemId));
                    productItem.QCStatus = 1;
                }

                QCResult qCResult = new QCResult
                {
                    Id = Guid.NewGuid().ToString(),
                    QCCheckListId = model.QCCheckListId,
                    ProductItemId = model.ProductItemId,
                    Status = model.Status,
                    QCDate = DateTime.Now,
                    QCBy = model.UserId,
                    Reason = model.Reason,
                };
                db.QCResults.Add(qCResult);
                summaryQuotesBussiness.CreatFile(model.Attachment, model.UserId, qCResult.Id);


            }

            db.SaveChanges();
        }


        public void UpdateProductStatus(QCResultCreatModel model, string UserId)
        {
            var productItem = db.ProductItems.FirstOrDefault(r => r.Id.Equals(model.Id));
            if (productItem != null)
            {
                productItem.QCStatus = model.Status;
                productItem.UpdateBy = UserId;
                productItem.UpdateDate = DateTime.Now;
            }
            db.SaveChanges();
        }


    }
}
