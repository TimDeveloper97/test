using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Materials;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using System.Web;
using System.Net.Http;
using NTS.Model.Common;
using Newtonsoft.Json;
using NTS.Model;
using NTS.Model.ModulePart;
using System.IO;
using Syncfusion.XlsIO;
using NTS.Model.MaterialParameter;
using NTS.Model.MaterialParameterValue;
using NTS.Model.MaterialBuyHistory;
using System.Data.Entity.Core.Objects;
using System.Web.Hosting;
using NTS.Common;
using NTS.Model.ModuleMaterials;
using static NTS.Model.Materials.MaterialModel;
using System.Windows.Forms;
using Syncfusion.DocIO.DLS;
using System.Drawing;
using System.Text.RegularExpressions;
using NTS.Common.Resource;
using QLTK.Business.Suppliers;
using NTS.Caching;
using System.Configuration;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using QLTK.Business.QLTKMODULE;
using QLTK.Business.ModuleMaterials;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using NTS.Model.Supplier;
using System.Web.Script.Serialization;

namespace QLTK.Business.Materials
{
    public class MaterialBusiness : IDisposable
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly SupplierBusiness supplierBusiness = new SupplierBusiness();

        public object SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultMaterial<MaterialResultModel> searchResult = new SearchResultMaterial<MaterialResultModel>();
            List<string> listParentId = new List<string>();

            try
            {
                var dataQuery = (from a in db.Materials.AsNoTracking()
                                 join b in db.Units.AsNoTracking() on a.UnitId equals b.Id into ab
                                 from b in ab.DefaultIfEmpty()
                                 join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id into ac
                                 from c in ac.DefaultIfEmpty()
                                 join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                                 from d in ad.DefaultIfEmpty()
                                 join e in db.RawMaterials.AsNoTracking() on a.RawMaterialId equals e.Id into ae
                                 from e in ae.DefaultIfEmpty()
                                 orderby a.Code
                                 select new MaterialResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ManufactureId = a == null ? "" : a.ManufactureId,
                                     UnitId = a.UnitId,
                                     MaterialGroupId = a.MaterialGroupId,
                                     ManufactureCode = d.Code,
                                     UnitName = b == null ? "" : b.Name,
                                     MaterialGroupName = c == null ? "" : c.Code,
                                     Pricing = a.Pricing,
                                     PriceHistory = a.PriceHistory,
                                     PriceNearest = a.Pricing,
                                     Note = a.Note,
                                     DeliveryDays = a.DeliveryDays,
                                     ThumbnailPath = a.ThumbnailPath,
                                     LastBuyDate = a.LastBuyDate,
                                     IsUsuallyUse = a.IsUsuallyUse,
                                     MechanicalType = a.MechanicalType,
                                     Status = a.Status,
                                     RawMaterialName = e == null ? "" : e.Name,
                                     Is3D = a.Is3D,
                                     IsDataSheet = a.IsDataSheet,
                                     MaterialType = a.MaterialType,
                                     Is3DExist = a.Is3DExist,
                                     IsDataSheetExist = a.IsDatasheetExist,
                                     InputPriceDate = a.InputPriceDate,
                                     IsSendSale = a.IsSendSale,
                                     SyncDate = a.SyncDate,
                                     IsRedundant = a.IsRedundant ? a.IsRedundant : false,
                                 }).AsQueryable();

                //if (!string.IsNullOrEmpty(modelSearch.Name))
                //{
                //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                //}

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.MaterialGroupName))
                {
                    dataQuery = dataQuery.Where(u => u.MaterialGroupName.ToUpper().Contains(modelSearch.MaterialGroupName.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
                {
                    dataQuery = dataQuery.Where(u => u.ManufactureId.Equals(modelSearch.ManufactureId));
                }

                if (!string.IsNullOrEmpty(modelSearch.MaterialType))
                {
                    dataQuery = dataQuery.Where(u => u.MaterialType.Equals(modelSearch.MaterialType));
                }

                if (modelSearch.IsSendSale.HasValue)
                {
                    dataQuery = dataQuery.Where(u => u.IsSendSale == modelSearch.IsSendSale);
                }


                //if (modelSearch.IsSendSale)
                //{
                //    dataQuery = dataQuery.Where(u => u.IsSendSale.Equals(modelSearch.IsSendSale));
                //}

                if (!string.IsNullOrEmpty(modelSearch.MaterialGroupId))
                {
                    var materialGroups = db.MaterialGroups.AsNoTracking().ToList();

                    var materialGroup = materialGroups.FirstOrDefault(i => i.Id.Equals(modelSearch.MaterialGroupId));

                    if (materialGroup != null)
                    {
                        listParentId.Add(materialGroup.Id);
                    }

                    listParentId.AddRange(supplierBusiness.GetListParent(modelSearch.MaterialGroupId, materialGroups));
                    var listMaterialGroup = listParentId.AsQueryable();
                    dataQuery = (from a in dataQuery
                                 join b in listMaterialGroup on a.MaterialGroupId equals b
                                 select new MaterialResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ManufactureId = a == null ? "" : a.ManufactureId,
                                     UnitId = a.UnitId,
                                     MaterialGroupId = a.MaterialGroupId,
                                     ManufactureCode = a.ManufactureCode,
                                     UnitName = a == null ? "" : a.Name,
                                     MaterialGroupName = a == null ? "" : a.MaterialGroupName,
                                     Pricing = a.Pricing,
                                     PriceHistory = a.PriceHistory,
                                     PriceNearest = a.Pricing,
                                     Note = a.Note,
                                     DeliveryDays = a.DeliveryDays,
                                     ThumbnailPath = a.ThumbnailPath,
                                     LastBuyDate = a.LastBuyDate,
                                     IsUsuallyUse = a.IsUsuallyUse,
                                     MechanicalType = a.MechanicalType,
                                     Status = a.Status,
                                     RawMaterialName = a == null ? "" : a.Name,
                                     Is3D = a.Is3D,
                                     IsDataSheet = a.IsDataSheet,
                                     MaterialType = a.MaterialType,
                                     Is3DExist = a.Is3DExist,
                                     IsDataSheetExist = a.IsDataSheetExist,
                                     InputPriceDate = a.InputPriceDate,
                                     IsSendSale = a.IsSendSale,
                                     SyncDate = a.SyncDate,
                                     IsRedundant = a.IsRedundant,
                                 }).AsQueryable();
                }

                if (!string.IsNullOrEmpty(modelSearch.Status3D))
                {
                    if (modelSearch.Status3D.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(a => (!a.Is3D.HasValue || a.Is3D.Value) && a.Is3DExist);
                    }
                    else
                    {
                        dataQuery = dataQuery.Where(a => (!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist);
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.RedundantStatus))
                {
                    if (modelSearch.RedundantStatus.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(a => a.IsRedundant);
                    }
                    else
                    {
                        dataQuery = dataQuery.Where(a => !a.IsRedundant);
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.StatusDatasheed))
                {
                    if (modelSearch.StatusDatasheed.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(a => (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && a.IsDataSheetExist);
                    }
                    else
                    {
                        dataQuery = dataQuery.Where(a => (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist);
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.ImagePath))
                {
                    if (modelSearch.ImagePath.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(a => !string.IsNullOrEmpty(a.ThumbnailPath));
                    }
                    else
                    {
                        dataQuery = dataQuery.Where(a => string.IsNullOrEmpty(a.ThumbnailPath));
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.MechanicalType))
                {
                    dataQuery = dataQuery.Where(u => u.MechanicalType.ToUpper().Contains(modelSearch.MechanicalType.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.RawMaterialName))
                {
                    dataQuery = dataQuery.Where(u => u.RawMaterialName.ToUpper().Contains(modelSearch.RawMaterialName.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.IsAllFile))
                {
                    if (modelSearch.IsAllFile.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(a => ((!a.Is3D.HasValue || a.Is3D.Value) && a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && a.IsDataSheetExist)
                        || (a.Is3D.HasValue && a.Is3DExist && a.Is3D.Value && (!a.IsDataSheet.HasValue || !a.IsDataSheet.Value))
                        || (a.IsDataSheet.HasValue && a.IsDataSheetExist && a.IsDataSheet.Value && (!a.Is3D.HasValue || !a.Is3D.Value))
                        );
                    }
                    else if (modelSearch.IsAllFile.Equals("0"))
                    {
                        dataQuery = dataQuery.Where(a =>
                        ((!a.Is3D.HasValue || a.Is3D.Value) && a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                        || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && a.IsDataSheetExist)
                        || ((!a.Is3D.HasValue || !a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                        || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                        || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || !a.IsDataSheet.Value)));
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.Status))
                {
                    dataQuery = dataQuery.Where(u => u.Status.Equals(modelSearch.Status));
                }

                if (modelSearch.Pricing != null)
                {
                    if (modelSearch.MaterialPriceType == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.Pricing == modelSearch.Pricing);
                    }
                    else if (modelSearch.MaterialPriceType == 2)
                    {
                        dataQuery = dataQuery.Where(u => u.Pricing > modelSearch.Pricing);
                    }
                    else if (modelSearch.MaterialPriceType == 3)
                    {
                        dataQuery = dataQuery.Where(u => u.Pricing >= modelSearch.Pricing);
                    }
                    else if (modelSearch.MaterialPriceType == 4)
                    {
                        dataQuery = dataQuery.Where(u => u.Pricing < modelSearch.Pricing);
                    }
                    else if (modelSearch.MaterialPriceType == 5)
                    {
                        dataQuery = dataQuery.Where(u => u.Pricing <= modelSearch.Pricing);
                    }

                }

                if (modelSearch.HistoryPrice.HasValue)
                {
                    if (modelSearch.MaterialHistoryPriceType == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.PriceNearest == modelSearch.HistoryPrice.Value);
                    }
                    else if (modelSearch.MaterialHistoryPriceType == 2)
                    {
                        dataQuery = dataQuery.Where(u => u.PriceNearest > modelSearch.HistoryPrice.Value);
                    }
                    else if (modelSearch.MaterialHistoryPriceType == 3)
                    {
                        dataQuery = dataQuery.Where(u => u.PriceNearest >= modelSearch.HistoryPrice.Value);
                    }
                    else if (modelSearch.MaterialHistoryPriceType == 4)
                    {
                        dataQuery = dataQuery.Where(u => u.PriceNearest < modelSearch.HistoryPrice.Value);
                    }
                    else if (modelSearch.MaterialHistoryPriceType == 5)
                    {
                        dataQuery = dataQuery.Where(u => u.PriceNearest <= modelSearch.HistoryPrice.Value);
                    }

                }

                if (modelSearch.DeliveryDays != null)
                {
                    if (modelSearch.DeliveryDaysType == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.DeliveryDays == modelSearch.DeliveryDays);
                    }
                    else if (modelSearch.DeliveryDaysType == 2)
                    {
                        dataQuery = dataQuery.Where(u => u.DeliveryDays > modelSearch.DeliveryDays);
                    }
                    else if (modelSearch.DeliveryDaysType == 3)
                    {
                        dataQuery = dataQuery.Where(u => u.DeliveryDays >= modelSearch.DeliveryDays);
                    }
                    else if (modelSearch.DeliveryDaysType == 4)
                    {
                        dataQuery = dataQuery.Where(u => u.DeliveryDays < modelSearch.DeliveryDays);
                    }
                    else if (modelSearch.DeliveryDaysType == 5)
                    {
                        dataQuery = dataQuery.Where(u => u.DeliveryDays <= modelSearch.DeliveryDays);
                    }

                }

                if (modelSearch.LastDelivery.HasValue)
                {
                    var dateNow = DateTime.Now;
                    var date = dateNow.AddDays(-modelSearch.LastDelivery.Value).Date;
                    if (modelSearch.LastDeliveryType == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.LastBuyDate == date);
                    }
                    else if (modelSearch.LastDeliveryType == 2)
                    {
                        dataQuery = dataQuery.Where(u => u.LastBuyDate < date);
                    }
                    else if (modelSearch.LastDeliveryType == 3)
                    {
                        dataQuery = dataQuery.Where(u => u.LastBuyDate <= date);
                    }
                    else if (modelSearch.LastDeliveryType == 4)
                    {
                        dataQuery = dataQuery.Where(u => u.LastBuyDate > date && u.LastBuyDate <= dateNow.Date);
                    }
                    else if (modelSearch.LastDeliveryType == 5)
                    {
                        dataQuery = dataQuery.Where(u => u.LastBuyDate >= date && u.LastBuyDate <= dateNow.Date);
                    }

                }

                var materialNoPricing = dataQuery.Where(a => a.Pricing == 0);
                searchResult.TotalItemExten = materialNoPricing.Count();
                searchResult.TotalItem = dataQuery.Count();
                searchResult.Date = dataQuery.Max(i => i.SyncDate);
                searchResult.TotalNoFile = dataQuery.Where(a =>
                ((!a.Is3D.HasValue || a.Is3D.Value) && a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && a.IsDataSheetExist)
                || ((!a.Is3D.HasValue || !a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || a.IsDataSheet.Value) && !a.IsDataSheetExist)
                || ((!a.Is3D.HasValue || a.Is3D.Value) && !a.Is3DExist && (!a.IsDataSheet.HasValue || !a.IsDataSheet.Value))
                ).Count();
                var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;

                var day = GetConfigMaterialLastByDate();
                foreach (var item in listResult)
                {
                    if (item.Is3DExist)
                    {
                        item.IsDocument3D = 1;
                    }

                    if (item.IsDataSheetExist)
                    {
                        item.IsDocumentDataSheet = 1;
                    }

                    if (item.LastBuyDate.HasValue)
                    {
                        TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

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
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        public string ExportExcelListError(List<FileErrorModel> model)
        {
            //try
            //{
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ErrorListMaterial.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = model.Count;



            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
            if (model.Count() == 0)
            {
                throw NTSException.CreateInstance("Không có tài liệu để export!");
            }
            var listExport = model.Select((a, i) => new
            {
                Index = i + 1,
                Name = a.Materialname,
                Manufacture = a.ManuafactureName,
                Error = a.ErrorMessage
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "ErrorListMaterial" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "ErrorListMaterial" + ".xlsx";

            return resultPathClient;
        }

            public MaterialChangeDataModel MaterialImportFileBOM(string userId, HttpPostedFile file, string projectProductId, string moduleId, bool isExit, bool confirm)
        {
            List<ModuleMaterialResultModel> moduleMaterialResultModels = new List<ModuleMaterialResultModel>();
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<ModuleMaterialFinishDesign> moduleMaterialFinishDesigns = new List<ModuleMaterialFinishDesign>();
            List<Material> materials = new List<Material>();
            List<ModuleMaterial> moduleMaterials = new List<ModuleMaterial>();
            List<ModuleMaterialModel> listModuleMaterial = new List<ModuleMaterialModel>();
            List<MaterialImportBOMDraft> materialImportBOMDrafts = new List<MaterialImportBOMDraft>();
            var js = new JavaScriptSerializer();
            var units = db.Units.AsNoTracking().ToList();
            var rawMaterials = db.RawMaterials.AsNoTracking().ToList();
            var manufactures = db.Manufactures.AsNoTracking().ToList();
            try
            {
                for (int i = 7; i <= rowCount; i++)
                {
                    var row = i;
                    if (string.IsNullOrEmpty(sheet[i, 1].Value) && string.IsNullOrEmpty(sheet[i, 2].Value) && string.IsNullOrEmpty(sheet[i, 3].Value) && string.IsNullOrEmpty(sheet[i, 4].Value) &&
                        string.IsNullOrEmpty(sheet[i, 5].Value) && string.IsNullOrEmpty(sheet[i, 6].Value) && string.IsNullOrEmpty(sheet[i, 7].Value) && string.IsNullOrEmpty(sheet[i, 8].Value) &&
                        string.IsNullOrEmpty(sheet[i, 9].Value) && string.IsNullOrEmpty(sheet[i, 10].Value) && string.IsNullOrEmpty(sheet[i, 11].Value) && string.IsNullOrEmpty(sheet[i, 12].Value) &&
                        string.IsNullOrEmpty(sheet[i, 13].Value))
                    {
                        break;
                    }
                    MaterialImportBOMDraft materialImportBOMDraft = new MaterialImportBOMDraft();
                    materialImportBOMDraft.Id = Guid.NewGuid().ToString();
                    materialImportBOMDraft.ProjectId = projectProductId;
                    materialImportBOMDraft.ModuleId = moduleId;
                    if (!string.IsNullOrEmpty(sheet[i, 2].Value))
                    {
                        materialImportBOMDraft.Name = sheet[i, 2].Value;
                    }
                    else
                    {
                        throw NTSException.CreateInstance("Tên Vật tư dòng <" + string.Join(", ", row) + "> Không được để trống!");
                    }
                    materialImportBOMDraft.Specification = sheet[i, 3].Value;
                    if (!string.IsNullOrEmpty(sheet[i, 4].Value))
                    {
                        materialImportBOMDraft.Code = sheet[i, 4].Value;
                    }
                    else
                    {
                        throw NTSException.CreateInstance("Mã Vật tư dòng <" + string.Join(", ", row) + "> Không được để trống!");
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 5].Value))
                    {
                        var rawMaterialCode = sheet[i, 5].Value;
                        var rawMaterial = rawMaterials.FirstOrDefault(a => a.Code.Equals(rawMaterialCode));
                        if (rawMaterial != null)
                        {
                            materialImportBOMDraft.RawMaterialCode = rawMaterial.Code;
                        }
                        else
                        {
                            throw NTSException.CreateInstance("Mã Vật liệu <" + string.Join(", ", row) + "> Không tồn tại!");
                        }
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 6].Value))
                    {
                        var unitName = sheet[i, 6].Value;
                        var u = units.Where(a => a.Name.ToUpper().Equals(unitName)).FirstOrDefault();
                        if (u == null)
                        {
                            throw NTSException.CreateInstance("Đơn vị dòng <" + string.Join(", ", row) + "> sai định dạng!");
                        }
                        materialImportBOMDraft.UnitName = u.Name;
                    }
                    else
                    {
                        throw NTSException.CreateInstance("Đơn vị dòng <" + string.Join(", ", row) + "> không được để trống!");
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 7].Value))
                    {
                        try
                        {
                            var quantity = decimal.Parse(sheet[i, 7].Value);
                            materialImportBOMDraft.Quantity = quantity;
                        }
                        catch (Exception)
                        {
                            throw NTSException.CreateInstance("Số lượng vật tư dòng <" + string.Join(", ", row) + "> không đúng định dạng!");
                        }
                    }
                    else
                    {
                        throw NTSException.CreateInstance("Số lượng vật tư dòng <" + string.Join(", ", row) + "> không được để trống!");
                    }
                    materialImportBOMDraft.RawMaterial = sheet[i, 8].Value;
                    if (!string.IsNullOrEmpty(sheet[i, 9].Value))
                    {
                        try
                        {
                            if(!sheet[i, 9].Value.Equals("-"))
                            {
                                var weight = decimal.Parse(sheet[i, 9].Value);
                                materialImportBOMDraft.Weight = weight;
                            }
                            else
                            {
                                materialImportBOMDraft.Weight = 0;
                            }
                        }
                        catch (Exception)
                        {
                            throw NTSException.CreateInstance("Khối lượng vật tư dòng <" + string.Join(", ", row) + "> không đúng định dạng!");
                        }
                    }
                    else
                    {
                        materialImportBOMDraft.Weight = 0;
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 10].Value))
                    {
                        var manufactureCode = sheet[i, 10].Value;
                        var ma = manufactures.Where(a => a.Code.ToUpper().Equals(manufactureCode.ToUpper())).FirstOrDefault();
                        if (ma == null)
                        {
                            throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", row) + "> không tồn tại!");
                        }
                        materialImportBOMDraft.ManufactureCode = ma.Code;
                    }
                    else
                    {
                        throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", row) + "> không được để trống!");
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 12].Value))
                    {
                        try
                        {
                            var pricing = decimal.Parse(sheet[i, 12].Value);
                            if (!pricing.Equals("-"))
                            {
                                materialImportBOMDraft.Pricing = pricing;
                            }
                        }
                        catch (Exception)
                        {
                            throw NTSException.CreateInstance("Đơn giá vật tư dòng <" + string.Join(", ", row) + "> không đúng định dạng!");
                        }
                    }
                    else
                    {
                        materialImportBOMDraft.Pricing = 0;
                    }
                    if (!string.IsNullOrEmpty(sheet[i, 13].DisplayText) )
                    {
                        try
                        {
                            if(!sheet[i, 13].DisplayText.NTSTrim().Equals("-"))
                            {
                                materialImportBOMDraft.Amount = decimal.Parse(sheet[i, 13].DisplayText.NTSTrim());
                            }
                        }
                        catch (Exception)
                        {
                            throw NTSException.CreateInstance("Giá vật tư dòng <" + string.Join(", ", row) + "> không đúng định dạng!");
                        }
                    }
                    else
                    {
                        materialImportBOMDraft.Amount = 0;
                    }
                    materialImportBOMDraft.Note = sheet[i, 11].Value;
                    if (!string.IsNullOrEmpty(sheet[i, 1].Value))
                    {
                        materialImportBOMDraft.Index = sheet[i, 1].Value;
                    }
                    else
                    {
                        throw NTSException.CreateInstance("STT vật tư dòng <" + string.Join(", ", row) + "> không được bỏ trống!");
                    }
                    materialImportBOMDraft.Status = true;
                    //if (materialImportBOMDrafts.FirstOrDefault(a => a.Code.Equals(materialImportBOMDraft.Code)) != null)
                    //{
                    //    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", row) + "> đang bị trùng!");
                    //}
                    if (materialImportBOMDrafts.FirstOrDefault(a => a.Index.Equals(materialImportBOMDraft.Index)) != null)
                    {
                        throw NTSException.CreateInstance("STT vật tư dòng <" + string.Join(", ", row) + "> đang bị trùng!");
                    }
                    materialImportBOMDrafts.Add(materialImportBOMDraft);

                    #endregion
                    if (string.IsNullOrEmpty(sheet[i, 4].Value))
                    {
                        throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", row) + "> không được để trống!");
                        //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                    }
                }
                //check ton tai trong db
                MaterialChangeDataModel result = new MaterialChangeDataModel();
                var materialOlds = db.MaterialImportBOMDrafts.Where(a => a.ProjectId.Equals(projectProductId) && a.ModuleId.Equals(moduleId) && (a.UpdateStatus == 0 || a.UpdateStatus == 1)).ToList();
                if (materialOlds.Count() > 0)
                {
                    isExit = true;
                }
                else
                {
                    result.IsExit = false;
                };
                if (isExit && !confirm)
                {
                    List<MaterialChangeModel> oldMaterialChangeModels = new List<MaterialChangeModel>();
                    List<MaterialChangeModel> newMaterialChangeModels = new List<MaterialChangeModel>();

                    if (materialOlds.Count() >= materials.Count())
                    {
                        foreach (var item in materialOlds)
                        {
                            //old data
                            MaterialChangeModel m = new MaterialChangeModel();
                            m.Id = item.Id;
                            m.Index = item.Index;
                            m.Name = item.Name;
                            m.Code = item.Code;
                            m.Specification = item.Specification;
                            m.RawMaterial = item.RawMaterial;
                            m.RawMaterialCode = item.RawMaterialCode;
                            m.UnitName = item.UnitName;
                            m.Weight = item.Weight;
                            m.Quantity = item.Quantity;
                            m.Pricing = item.Pricing;
                            m.Note = item.Note;
                            m.Amount = item.Amount;
                            m.ManufactureCode = item.ManufactureCode;
                            m.TotalPrice = m.Quantity * m.Pricing;

                            oldMaterialChangeModels.Add(m);
                        }

                        foreach (var newMaterial in materialImportBOMDrafts)
                        {
                            MaterialChangeModel m = new MaterialChangeModel();
                            m.NewIndex = newMaterial.Index;
                            m.Code = newMaterial.Code;
                            m.NewName = newMaterial.Name;
                            m.NewSpecification = newMaterial.Specification;
                            m.NewRawMaterial = newMaterial.RawMaterial;
                            m.NewRawMaterialCode = newMaterial.RawMaterialCode;
                            m.NewUnitName = newMaterial.UnitName;
                            m.NewWeight = newMaterial.Weight;
                            m.NewPricing = newMaterial.Pricing;
                            m.NewNote = newMaterial.Note;
                            m.NewManufactureCode = newMaterial.ManufactureCode;
                            m.NewQuantity = newMaterial.Quantity;
                            m.NewAmount = newMaterial.Amount;
                            m.NewTotalPrice = m.NewQuantity * m.NewPricing;

                            var materialDuppllicateCode = oldMaterialChangeModels.FirstOrDefault(a => a.Code.Equals(newMaterial.Code) && !a.Index.Equals(newMaterial.Index));
                            var materialDuppllicateIndex = oldMaterialChangeModels.FirstOrDefault(a => a.Index.Equals(newMaterial.Index));
                            // new data

                            if (materialDuppllicateCode != null && materialDuppllicateIndex != null)
                            {
                                m.DupplicateCode = true;
                                m.DupplicateIndex = true;
                                newMaterialChangeModels.Add(m);
                            }
                            else if (materialDuppllicateCode == null && materialDuppllicateIndex != null)
                            {
                                m.DupplicateCode = false;
                                m.DupplicateIndex = true;
                                newMaterialChangeModels.Add(m);
                            }
                            else if (materialDuppllicateCode != null && materialDuppllicateIndex == null)
                            {
                                m.DupplicateCode = true;
                                m.DupplicateIndex = false;
                                newMaterialChangeModels.Add(m);
                            }
                            else
                            {
                                m.DupplicateCode = false;
                                m.DupplicateIndex = false;
                                newMaterialChangeModels.Add(m);
                            }
                        }
                    }
                    foreach (var item in newMaterialChangeModels)
                    {
                        if (checkMaterial(newMaterialChangeModels, item))
                        {
                            item.NewPricing = getParentAmountMaterial(newMaterialChangeModels, item);
                            item.NewAmount = item.NewQuantity * item.NewPricing;
                            item.NewTotalPrice = item.NewQuantity * item.NewPricing;
                        }
                    }
                    result.oldMaterialChangeModels = oldMaterialChangeModels;
                    result.newMaterialChangeModels = newMaterialChangeModels;
                    result.IsExit = true;
                    result.Confirm = false;
                    return result;
                }
                // confirm data de cap nhat lan import moi
                if (confirm)
                {
                    var listMaterial = db.MaterialImportBOMDrafts.Where(a => a.ProjectId.Equals(projectProductId) && a.ModuleId.Equals(moduleId) && (a.UpdateStatus == 0 || a.UpdateStatus == 1));
                    foreach (var item in db.MaterialImportBOMDrafts)
                    {
                        item.Status = false;
                        //item.UpdateStatus = 0;
                    }
                    foreach (var item in materialImportBOMDrafts)
                    {
                        var material = listMaterial.FirstOrDefault(a => a.Index.Equals(item.Index));
                        if (material != null)
                        {
                            material.Index = item.Index;
                            material.Name = item.Name;
                            material.Code = item.Code;
                            material.Specification = item.Specification;
                            material.RawMaterial = item.RawMaterial;
                            material.RawMaterialCode = item.RawMaterialCode;
                            material.UnitName = item.UnitName;
                            material.Weight = item.Weight;
                            material.Quantity = item.Quantity;
                            material.Pricing = item.Pricing;
                            material.Note = item.Note;
                            material.Amount = item.Amount;
                            material.ManufactureCode = item.ManufactureCode;
                            material.Status = false;
                            material.UpdateStatus = 1;

                            item.UpdateStatus = 2;
                            db.MaterialImportBOMDrafts.Add(item);

                        }
                        else if (listMaterial.FirstOrDefault(a => !a.Index.Equals(item.Index) && a.Code.Equals(item.Code)) != null)
                        {
                            var m = listMaterial.FirstOrDefault(a => !a.Index.Equals(item.Index) && a.Code.Equals(item.Code));
                            m.Quantity = (m.Quantity + item.Quantity);
                            m.Status = false;
                            m.UpdateStatus = 1;

                            item.UpdateStatus = 3;
                            db.MaterialImportBOMDrafts.Add(item);
                        }
                        else
                        {
                            item.UpdateStatus = 0;
                            db.MaterialImportBOMDrafts.Add(item);
                        }
                    }
                    var materialImportBOMDraftsJson = js.Serialize(materialImportBOMDrafts);
                    result.Content = materialImportBOMDraftsJson;
                }
                db.SaveChanges();
                return result;

            }
            catch (NTSException ntsEx)
            {
                workbook.Close();
                excelEngine.Dispose();

                throw ntsEx;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
        }

        public bool checkMaterial(List<MaterialChangeModel> newMaterialChangeModels, MaterialChangeModel newMaterial)
        {
            var index = newMaterial.NewIndex.IndexOf('.');
            if (index == -1)
            {
                foreach (var item in newMaterialChangeModels)
                {
                    if (item.NewIndex.Contains(newMaterial.NewIndex) && !item.NewIndex.Equals(newMaterial.NewIndex))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        public decimal getParentAmountMaterial(List<MaterialChangeModel> newMaterialChangeModels, MaterialChangeModel newMaterial)
        {
            decimal pricing = 0;
            foreach (var item in newMaterialChangeModels)
            {
                if (item.NewIndex.Contains(newMaterial.NewIndex) && !item.NewIndex.Equals(newMaterial.NewIndex))
                {
                    pricing = pricing + item.NewAmount;
                }
            }
            return pricing;
        }
        public List<MaterialFolderModel> GetDataDownloadMaterialDocument3Ds(List<MaterialDocumentDownloadModel> model)
        {
            List<MaterialFolderModel> results = new List<MaterialFolderModel>();
            foreach (var item in model)
            {
                var m = db.Materials.Where(a => a.Id.Equals(item.MaterialId)).FirstOrDefault();
                MaterialFolderModel materialFolderModel = new MaterialFolderModel();

                if(m == null)
                {
                    materialFolderModel.Manufature = item.MaterialManufatureError;
                    materialFolderModel.Name = item.MaterialCodeNotFound;
                    materialFolderModel.IsExist = false;
                }
                else
                {
                    materialFolderModel.Manufature = db.Manufactures.FirstOrDefault(a => a.Id.Equals(m.ManufactureId)).Name;
                    materialFolderModel.Name = m.Code;
                    materialFolderModel.IsExist = true;
                    materialFolderModel.ListMaterialDocument3DFolderModel = (from a in db.MaterialDesign3D.AsNoTracking()
                                                                             join b in db.Design3D.AsNoTracking() on a.Design3DId equals b.Id
                                                                             join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                                                                             join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                                                             where a.MaterialId.Equals(item.MaterialId)
                                                                             select new FileDocumentModel
                                                                             {
                                                                                 Name = b.FileName,
                                                                                 Path = b.FilePath,
                                                                             }).ToList();
                }
                
                results.Add(materialFolderModel);
            }
            return results;
        }

        public List<MaterialDocumentDownloadModel> MaterialCodeImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<Material> materials = new List<Material>();
            try
            {
                var J3 = sheet[3, 10].Value;
                var ModuleCode = J3.Substring(3).NTSTrim();

                for (int i = 7; i <= rowCount; i++)
                {
                    Material material = new Material();
                    var row = i;
                    if (string.IsNullOrEmpty(sheet[i, 1].Value) && string.IsNullOrEmpty(sheet[i, 2].Value) && string.IsNullOrEmpty(sheet[i, 3].Value) && string.IsNullOrEmpty(sheet[i, 4].Value) &&
                        string.IsNullOrEmpty(sheet[i, 5].Value) && string.IsNullOrEmpty(sheet[i, 6].Value) && string.IsNullOrEmpty(sheet[i, 7].Value) && string.IsNullOrEmpty(sheet[i, 8].Value) &&
                        string.IsNullOrEmpty(sheet[i, 9].Value) && string.IsNullOrEmpty(sheet[i, 10].Value) && string.IsNullOrEmpty(sheet[i, 11].Value) && string.IsNullOrEmpty(sheet[i, 12].Value) &&
                        string.IsNullOrEmpty(sheet[i, 13].Value))
                    {
                        break;
                    }

                    if (!string.IsNullOrEmpty(sheet[i, 4].Value))
                    {
                        var code = sheet[i, 4].Value.Trim();
                        material = db.Materials.Where(m => m.Code.Equals(code)).FirstOrDefault();
                    }
                    if(material != null)
                    {
                        materials.Add(material);
                    }
                    if(material == null && !string.IsNullOrEmpty(sheet[i, 4].Value))
                    {
                        Material materialEror = new Material();
                        materialEror.Code = sheet[i, 4].Value;
                        if(!string.IsNullOrEmpty(sheet[i, 10].Value))
                        {
                            materialEror.Name = sheet[i, 10].Value;
                        }
                        materials.Add(materialEror);
                    }

                }
                List<Design3DModel> Design3DModels = new List<Design3DModel>();
                List<MaterialDocumentDownloadModel> listResult = new List<MaterialDocumentDownloadModel>();
                foreach (var material in materials)
                {
                    MaterialDocumentDownloadModel re = new MaterialDocumentDownloadModel();
                    if (string.IsNullOrEmpty(material.Id))
                    {
                        re.MaterialCodeNotFound = material.Code;
                        re.MaterialManufatureError = material.Name;
                    }
                    re.MaterialId = material.Id;
                    re.ModuleCode = ModuleCode;
                    if (!string.IsNullOrEmpty(material.Id))
                    {
                        var listMaterialDesign3D = (from a in db.MaterialDesign3D.AsNoTracking()
                                                    join b in db.Design3D.AsNoTracking() on a.Design3DId equals b.Id
                                                    join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                                                    join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                                    where a.MaterialId.Equals(material.Id)
                                                    select new Design3DModel
                                                    {
                                                        Id = b.Id,
                                                        FileName = b.FileName,
                                                        FilePath = b.FilePath,
                                                        Size = b.Size,
                                                        CreateBy = b.CreateBy,
                                                        CreateByName = d.Name,
                                                        CreateDate = b.CreateDate,
                                                        UpdateBy = b.UpdateBy,
                                                        UpdateDate = b.UpdateDate
                                                    }).ToList();
                        List<string> md3DIds = new List<string>();
                        foreach (var l in listMaterialDesign3D)
                        {
                            if (Design3DModels.Where(dm => dm.Id.Equals(l.Id)).Count() == 0)
                            {
                                md3DIds.Add(l.Id);
                            }
                        }
                        re.Document3DId = md3DIds;
                    }
                    
                    listResult.Add(re);
                }

                return listResult;

            }
            catch (NTSException ntsEx)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ntsEx;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

        }

        public string GetGroupMaterialCodeInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/MaVatTu_Template.xlsx"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            var listCustomer = db.Materials.AsNoTracking().Select(i => i.Code).ToList();
            sheet0.Range["D7:D1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
            IRange iRangeData = sheet1.FindFirst("<materialCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<materialCode>", string.Empty);
            var listExport = listCustomer.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });
            sheet1.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "MaVatTu_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "MaVatTu_Template" + ".xls";

            return resultPathClient;
        }

        public void AddMaterial(MaterialModel materialModel, HttpFileCollection hfc, string userLoginId)
        {
            //xoá ký tự đặc biệt
            materialModel.Code = Util.RemoveSpecialCharacter(materialModel.Code);

            //if (db.Materials.AsNoTracking().Where(o => o.Name.Equals(materialModel.Name)).Count() > 0)
            //{
            //    throw new Exception("Tên vật tư đã tồn tại! Vui lòng xem lại!");
            //}
            if (db.Materials.AsNoTracking().Where(o => o.Code.Equals(materialModel.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Material);
            }

            using (var trans = db.Database.BeginTransaction())
            {

                try
                {

                    var dateNow = DateTime.Now;
                    Material newMaterial = new Material()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = materialModel.Name.NTSTrim(),
                        Code = materialModel.Code.NTSTrim(),
                        ManufactureId = materialModel.ManufactureId,
                        UnitId = materialModel.UnitId,
                        MaterialGroupId = materialModel.MaterialGroupId,
                        Note = materialModel.Note,
                        Pricing = materialModel.Pricing,
                        DeliveryDays = materialModel.DeliveryDays,
                        LastBuyDate = materialModel.LastBuyDate,
                        IsUsuallyUse = materialModel.IsUsuallyUse,
                        Is3D = materialModel.Is3D,
                        IsDataSheet = materialModel.IsDataSheet,
                        IsSetup = materialModel.IsSetup,
                        MaterialGroupTPAId = materialModel.MaterialGroupTPAId,
                        RawMaterial = materialModel.RawMaterial.NTSTrim(),
                        RawMaterialId = materialModel.RawMaterialId,
                        Weight = materialModel.Weight.NTSTrim(),
                        Status = materialModel.Status,
                        MechanicalType = materialModel.MechanicalType,
                        MaterialType = materialModel.MaterialType,
                        IsRedundant = materialModel.IsRedundant,
                        RedundantAmount = materialModel.RedundantAmount,
                        RedundantDescription = materialModel.RedundantDescription,
                        RedundantDeliveryNote = materialModel.RedundantDeliveryNote,


                        CreateBy = materialModel.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = materialModel.CreateBy
                    };

                    //Xử lý thêm materialParameter và materialParameterValue vào bảng Specification
                    if (materialModel.ListMaterialParameter.Count > 0)
                    {
                        foreach (var item in materialModel.ListMaterialParameter)
                        {
                            foreach (var i in item.ListValue)
                            {
                                if (i.IsChecked)
                                {
                                    Specification specification = new Specification()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        MaterialId = newMaterial.Id,
                                        MaterialParameterId = item.Id,
                                        MaterialParameterValueId = i.Id

                                    };
                                    db.Specifications.Add(specification);
                                }
                            }
                        }
                    }


                    //thêm bảng Design3d(file đính kèm)
                    List<string> listDesign3DId = new List<string>();
                    if (materialModel.ListFileDesign3D != null)
                    {
                        foreach (var item in materialModel.ListFileDesign3D)
                        {
                            Design3D design3D = new Design3D()
                            {
                                Id = Guid.NewGuid().ToString(),
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                Size = item.Size,
                                CreateBy = materialModel.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                UpdateBy = materialModel.CreateBy
                            };
                            db.Design3D.Add(design3D);
                            listDesign3DId.Add(design3D.Id);
                        }
                    }

                    //thêm bảng  MaterialDesign3D
                    if (listDesign3DId.Count > 0)
                    {
                        foreach (var item in listDesign3DId)
                        {
                            MaterialDesign3D materialDesign3D = new MaterialDesign3D()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = newMaterial.Id,
                                Design3DId = item,
                            };
                            db.MaterialDesign3D.Add(materialDesign3D);
                        }
                        newMaterial.Is3DExist = true;
                    }

                    //thêm bảng datasheet
                    List<string> listDataSheetId = new List<string>();
                    if (materialModel.ListFileDataSheet != null)
                    {
                        foreach (var item in materialModel.ListFileDataSheet)
                        {
                            if (item.FilePath != null && item.FilePath != "")
                            {
                                DataSheet dataSheet = new DataSheet()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ManufactureId = materialModel.ManufactureId,
                                    FileName = item.FileName,
                                    FilePath = item.FilePath,
                                    Size = item.Size,
                                    CreateBy = materialModel.CreateBy,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = materialModel.CreateBy
                                };
                                db.DataSheets.Add(dataSheet);
                                listDataSheetId.Add(dataSheet.Id);
                            }
                        }

                    }

                    //thêm bảng MaterialDataSheet
                    if (listDataSheetId.Count > 0)
                    {
                        foreach (var item in listDataSheetId)
                        {
                            MaterialDataSheet materialDataSheet = new MaterialDataSheet()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = newMaterial.Id,
                                DataSheetId = item,
                            };
                            db.MaterialDataSheets.Add(materialDataSheet);
                        }
                        newMaterial.IsDatasheetExist = true;
                    }

                    //Thêm MaterialImage
                    if (materialModel.ListImage != null && materialModel.ListImage.Count > 0)
                    {
                        foreach (var item in materialModel.ListImage)
                        {
                            MaterialImage materialImage = new MaterialImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = newMaterial.Id,
                                Path = item.Path,
                                ThumbnailPath = item.ThumbnailPath
                            };
                            db.MaterialImages.Add(materialImage);
                        }
                        newMaterial.ImagePath = materialModel.ListImage[0].Path;
                        newMaterial.ThumbnailPath = materialModel.ListImage[0].ThumbnailPath;
                    }

                    //Thêm bảng MaterialModuleGroup

                    if (materialModel.ListModuleGroupId != null && materialModel.ListModuleGroupId.Count > 0)
                    {
                        foreach (var item in materialModel.ListModuleGroupId)
                        {
                            MaterialModuleGroup materialModuleGroup = new MaterialModuleGroup()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = newMaterial.Id,
                                ModuleGroupId = item,
                            };
                            db.MaterialModuleGroups.Add(materialModuleGroup);
                        }
                    }

                    db.Materials.Add(newMaterial);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, newMaterial.Code, newMaterial.Id, Constants.LOG_Material);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(materialModel, ex);
                }
            }
        }

        public void UpdateMaterial(MaterialModel materialModel, HttpFileCollection hfc, string userLoginId)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //xoá ký tự đặc biệt
                    materialModel.Code = Util.RemoveUnicodeCharacter(materialModel.Code);
                    //if (db.Materials.AsNoTracking().Where(o => !o.Id.Equals(materialModel.Id) && (o.Name.Equals(materialModel.Name))).Count() > 0)
                    //{
                    //    throw new Exception("Tên của vật tư này đã tồn tại! Vui lòng xem lại!");
                    //}
                    if (db.Materials.AsNoTracking().Where(o => !o.Id.Equals(materialModel.Id) && (o.Code.Equals(materialModel.Code))).Count() > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Material);
                    }

                    var material = db.Materials.Where(o => o.Id.Equals(materialModel.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<MaterialHistoryModel>(material);

                    if (material == null)
                    {
                        throw new Exception("Vật tư này đã bị xoá bởi người dùng khác");
                    }

                    //edit

                    material.Code = materialModel.Code.Trim();
                    material.Name = materialModel.Name.Trim();
                    material.Note = materialModel.Note;
                    material.MaterialGroupId = materialModel.MaterialGroupId;
                    material.MaterialGroupTPAId = materialModel.MaterialGroupTPAId;
                    material.UnitId = materialModel.UnitId;
                    material.ManufactureId = materialModel.ManufactureId;
                    material.DeliveryDays = materialModel.DeliveryDays;
                    material.IsUsuallyUse = materialModel.IsUsuallyUse;
                    material.MaterialType = materialModel.MaterialType;
                    material.MechanicalType = materialModel.MechanicalType;
                    material.RawMaterial = materialModel.RawMaterial;
                    material.Status = materialModel.Status;
                    material.Is3D = materialModel.Is3D;
                    material.IsDataSheet = materialModel.IsDataSheet;
                    material.IsSetup = materialModel.IsSetup;
                    material.Weight = materialModel.Weight;
                    material.RawMaterialId = materialModel.RawMaterialId;
                    material.LastBuyDate = materialModel.LastBuyDate;
                    material.UpdateBy = materialModel.UpdateBy;
                    material.UpdateDate = DateTime.Now;
                    material.IsRedundant = materialModel.IsRedundant;
                    material.RedundantAmount = materialModel.RedundantAmount;
                    material.RedundantDescription = materialModel.RedundantDescription;
                    material.RedundantDeliveryNote = materialModel.RedundantDeliveryNote;


                    if (materialModel.Pricing != material.Pricing)
                    {
                        material.Pricing = materialModel.Pricing;
                        material.InputPriceDate = DateTime.Now;
                    }


                    //Update bảng Specification
                    var listSpecification = db.Specifications.Where(a => a.MaterialId.Equals(material.Id)).ToList();
                    if (listSpecification.Count > 0)
                    {
                        db.Specifications.RemoveRange(listSpecification);
                    }

                    if (materialModel.ListMaterialParameter.Count > 0)
                    {
                        foreach (var item in materialModel.ListMaterialParameter)
                        {
                            foreach (var i in item.ListValue)
                            {
                                if (i.IsChecked)
                                {
                                    Specification specification = new Specification()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        MaterialId = material.Id,
                                        MaterialParameterId = item.Id,
                                        MaterialParameterValueId = i.Id

                                    };
                                    db.Specifications.Add(specification);
                                }
                            }
                        }
                    }

                    //Update bảng Design3D

                    if (materialModel.ListFileDesign3D.Count > 0)
                    {
                        List<string> listDesign3DId = new List<string>();
                        var listMaterialDesign3D = db.MaterialDesign3D.Where(a => a.MaterialId.Equals(materialModel.Id));
                        var list = listMaterialDesign3D.ToList();
                        foreach (var item in materialModel.ListFileDesign3D)
                        {
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                var name = db.Design3D.Where(i => i.FileName.Equals(item.FileName)).FirstOrDefault();
                                if (name != null)
                                {
                                    var materialDesign3Ds = db.MaterialDesign3D.Where(i => i.Design3DId.Equals(name.Id)).ToList();
                                    if (materialDesign3Ds.Count > 0)
                                    {
                                        db.MaterialDesign3D.RemoveRange(materialDesign3Ds);
                                        db.Design3D.Remove(name);
                                    }
                                }
                                if (!string.IsNullOrEmpty(item.FilePath))
                                {
                                    Design3D design3D = new Design3D()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        FileName = item.FileName,
                                        FilePath = item.FilePath,
                                        Size = item.Size,
                                        CreateBy = materialModel.CreateBy,
                                        CreateDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        UpdateBy = materialModel.CreateBy
                                    };
                                    db.Design3D.Add(design3D);
                                    listDesign3DId.Add(design3D.Id);
                                }
                            }
                            else
                            {
                                foreach (var ite in listMaterialDesign3D)
                                {
                                    if (ite.Design3DId.Equals(item.Id))
                                    {
                                        list.Remove(ite);
                                    }
                                }
                            }
                        }
                        if (listDesign3DId.Count > 0)
                        {
                            foreach (var item in listDesign3DId)
                            {
                                MaterialDesign3D materialDesign3D = new MaterialDesign3D()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MaterialId = materialModel.Id,
                                    Design3DId = item,
                                };
                                db.MaterialDesign3D.Add(materialDesign3D);
                            }
                            material.Is3DExist = true;
                        }
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var listDesign3D = db.Design3D.Where(d => d.Id.Equals(item.Design3DId)).FirstOrDefault();
                                var materialDesign3D = db.MaterialDesign3D.Where(i => i.Design3DId.Equals(item.Design3DId)).FirstOrDefault();
                                if (listDesign3D != null)
                                {
                                    db.Design3D.Remove(listDesign3D);
                                }
                            }
                            db.MaterialDesign3D.RemoveRange(list);
                        }
                    }
                    else
                    {
                        var listMaterialDesign3D = db.MaterialDesign3D.Where(a => a.MaterialId.Equals(materialModel.Id)).ToList();

                        if (listMaterialDesign3D.Count > 0)
                        {
                            foreach (var item in listMaterialDesign3D)
                            {
                                var listDesign3D = db.Design3D.Where(d => d.Id.Equals(item.Design3DId)).FirstOrDefault();
                                if (listDesign3D != null)
                                {
                                    db.Design3D.Remove(listDesign3D);
                                }
                            }
                            db.MaterialDesign3D.RemoveRange(listMaterialDesign3D);
                        }
                        material.Is3DExist = false;
                    }

                    //Update bảng datasheet

                    if (materialModel.ListFileDataSheet.Count > 0)
                    {
                        List<string> listDataSheetId = new List<string>();
                        var listMaterialDataSheet = db.MaterialDataSheets.Where(a => a.MaterialId.Equals(materialModel.Id));
                        var list = listMaterialDataSheet.ToList();
                        foreach (var item in materialModel.ListFileDataSheet)
                        {
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                var name = db.DataSheets.Where(i => i.FileName.Equals(item.FileName)).FirstOrDefault();
                                if (name != null)
                                {
                                    var materialDataSheets = db.MaterialDataSheets.Where(i => i.DataSheetId.Equals(name.Id)).ToList();
                                    if (materialDataSheets.Count > 0)
                                    {
                                        db.MaterialDataSheets.RemoveRange(materialDataSheets);
                                        db.DataSheets.Remove(name);
                                    }
                                }
                                if (!string.IsNullOrEmpty(item.FilePath))
                                {
                                    DataSheet dataSheet = new DataSheet()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ManufactureId = materialModel.ManufactureId,
                                        FileName = item.FileName,
                                        FilePath = item.FilePath,
                                        Size = item.Size,
                                        CreateBy = materialModel.CreateBy,
                                        CreateDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        UpdateBy = materialModel.CreateBy
                                    };
                                    db.DataSheets.Add(dataSheet);
                                    listDataSheetId.Add(dataSheet.Id);
                                }
                            }
                            else
                            {
                                foreach (var ite in listMaterialDataSheet)
                                {
                                    if (ite.DataSheetId.Equals(item.Id))
                                    {
                                        list.Remove(ite);
                                    }
                                }
                            }
                        }
                        if (listDataSheetId.Count > 0)
                        {
                            foreach (var item in listDataSheetId)
                            {
                                MaterialDataSheet materialDataSheet = new MaterialDataSheet()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    MaterialId = materialModel.Id,
                                    DataSheetId = item,
                                };
                                db.MaterialDataSheets.Add(materialDataSheet);
                            }

                            material.IsDatasheetExist = true;
                        }
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var dataSheet = db.DataSheets.Where(d => d.Id.Equals(item.DataSheetId)).FirstOrDefault();
                                var materialDataSheet = db.MaterialDataSheets.Where(i => i.DataSheetId.Equals(item.DataSheetId)).FirstOrDefault();
                                if (dataSheet != null)
                                {
                                    db.DataSheets.Remove(dataSheet);
                                }
                            }
                            db.MaterialDataSheets.RemoveRange(list);
                        }
                    }
                    else
                    {
                        var listMaterialDataSheet = db.MaterialDataSheets.Where(a => a.MaterialId.Equals(materialModel.Id)).ToList();

                        if (listMaterialDataSheet.Count > 0)
                        {
                            foreach (var item in listMaterialDataSheet)
                            {
                                var listDataSheet = db.DataSheets.Where(d => d.Id.Equals(item.DataSheetId)).FirstOrDefault();
                                if (listDataSheet != null)
                                {
                                    db.DataSheets.Remove(listDataSheet);
                                }
                            }
                            db.MaterialDataSheets.RemoveRange(listMaterialDataSheet);
                        }
                        material.IsDatasheetExist = false;
                    }


                    //update materialImage
                    var listImage = db.MaterialImages.Where(a => a.MaterialId.Equals(materialModel.Id)).ToList();

                    if (listImage.Count > 0)
                    {
                        db.MaterialImages.RemoveRange(listImage);
                    }

                    if (materialModel.ListImage != null && materialModel.ListImage.Count > 0)
                    {
                        foreach (var item in materialModel.ListImage)
                        {
                            MaterialImage materialImage = new MaterialImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = materialModel.Id,
                                Path = item.Path,
                                ThumbnailPath = item.ThumbnailPath
                            };
                            db.MaterialImages.Add(materialImage);

                        }
                        material.ImagePath = materialModel.ListImage[0].Path;
                        material.ThumbnailPath = materialModel.ListImage[0].ThumbnailPath;
                    }
                    else
                    {
                        material.ImagePath = string.Empty;
                        material.ThumbnailPath = string.Empty;
                    }

                    //update bảng MaterialModuleGroup
                    var oldMaterialModuleGroup = db.MaterialModuleGroups.Where(m => m.MaterialId.Equals(materialModel.Id)).ToList();
                    if (oldMaterialModuleGroup.Count > 0)
                    {
                        db.MaterialModuleGroups.RemoveRange(oldMaterialModuleGroup);
                    }

                    if (materialModel.ListModuleGroupId != null && materialModel.ListModuleGroupId.Count > 0)
                    {
                        foreach (var item in materialModel.ListModuleGroupId)
                        {
                            MaterialModuleGroup materialModuleGroup = new MaterialModuleGroup()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = materialModel.Id,
                                ModuleGroupId = item,
                            };
                            db.MaterialModuleGroups.Add(materialModuleGroup);
                        }
                    }


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<MaterialHistoryModel>(material);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_Material, material.Id, material.Code, jsonBefor, jsonApter);

                    //save
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(materialModel, ex);
                }
            }
        }

        public void DeleteMaterial(MaterialModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var material = db.Materials.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (material == null)
                {
                    throw NTSException.CreateInstance("Vật tư không tồn tại");
                }
                var materialPart = db.MaterialParts.FirstOrDefault(m => m.MaterialItemId.Equals(model.Id));
                if (materialPart != null)
                {
                    throw NTSException.CreateInstance("Vật tư này đang được sử dụng. Không thể xoá");
                }
                var materialBuyHistory = db.MaterialBuyHistories.FirstOrDefault(m => m.MaterialId.Equals(model.Id));
                if (materialBuyHistory != null)
                {
                    throw NTSException.CreateInstance("Vật tư đang có lịch sử mua. Không thể xoá");
                }

                var moduleMaterial = db.ModuleMaterials.Where(u => u.MaterialId.Equals(model.Id)).ToList();
                if (moduleMaterial.Count > 0)
                {
                    throw NTSException.CreateInstance("Có module đang sử dụng vật tư này. Không thể xoá");
                }

                try
                {

                    var materialDesign3D = db.MaterialDesign3D.Where(u => u.MaterialId.Equals(model.Id)).ToList();
                    if (materialDesign3D != null && materialDesign3D.Count() > 0)
                    {
                        foreach (var item in materialDesign3D)
                        {
                            var design3D = db.Design3D.Where(d => d.Id.Equals(item.Design3DId)).FirstOrDefault();
                            if (design3D != null)
                            {
                                db.Design3D.Remove(design3D);
                            }
                        }
                        db.MaterialDesign3D.RemoveRange(materialDesign3D);
                    }
                    var materialDataSheet = db.MaterialDataSheets.Where(u => u.MaterialId.Equals(model.Id)).ToList();
                    if (materialDataSheet != null && materialDataSheet.Count > 0)
                    {
                        foreach (var item in materialDataSheet)
                        {
                            var dataSheet = db.DataSheets.Where(d => d.Id.Equals(item.DataSheetId)).FirstOrDefault();
                            if (dataSheet != null)
                            {
                                db.DataSheets.Remove(dataSheet);
                            }
                        }
                        db.MaterialDataSheets.RemoveRange(materialDataSheet);
                    }
                    var specification = db.Specifications.Where(u => u.MaterialId.Equals(model.Id)).ToList();
                    if (specification != null && specification.Count() > 0)
                    {
                        db.Specifications.RemoveRange(specification);
                    }
                    var materialImage = db.MaterialImages.Where(a => a.MaterialId.Equals(model.Id)).ToList();
                    if (materialImage != null && materialImage.Count() > 0)
                    {
                        db.MaterialImages.RemoveRange(materialImage);
                    }

                    var listMaterialModuleGroup = db.MaterialModuleGroups.Where(m => m.MaterialId.Equals(model.Id));
                    if (listMaterialModuleGroup != null && listMaterialModuleGroup.Count() > 0)
                    {
                        db.MaterialModuleGroups.RemoveRange(listMaterialModuleGroup);
                    }

                    var converUnits = db.ConverUnits.Where(m => m.MaterialId.Equals(model.Id)).ToList();
                    if (converUnits != null && converUnits.Count() > 0)
                    {
                        db.ConverUnits.RemoveRange(converUnits);
                    }


                    db.Materials.Remove(material);

                    //var jsonApter = AutoMapperConfig.Mapper.Map<MaterialHistoryModel>(material);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Material, material.Id, material.Code, jsonApter);

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

        public object GetMaterialInfo(MaterialModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                throw new Exception("Có lỗi trong quá trình xử lý!");
            }
            try
            {
                var material = db.Materials.AsNoTracking().FirstOrDefault(o => o.Id.Equals(model.Id));
                model.Id = material.Id;
                model.Code = material.Code;
                model.Name = material.Name;
                model.Note = material.Note;
                model.MaterialGroupId = material.MaterialGroupId;
                model.MaterialGroupTPAId = material.MaterialGroupTPAId;
                model.UnitId = material.UnitId;
                model.ManufactureId = material.ManufactureId;
                model.Pricing = material.Pricing;
                model.DeliveryDays = material.DeliveryDays;
                model.ImagePath = material.ImagePath;
                model.ThumbnailPath = material.ThumbnailPath;
                model.IsUsuallyUse = material.IsUsuallyUse;
                model.MaterialType = material.MaterialType;
                model.MechanicalType = material.MechanicalType;
                model.RawMaterial = material.RawMaterial;
                model.Status = material.Status;
                model.Is3D = material.Is3D;
                model.IsDataSheet = material.IsDataSheet;
                model.IsSetup = material.IsSetup;
                model.Weight = material.Weight;
                model.RawMaterialId = material.RawMaterialId;
                model.LastBuyDate = material.LastBuyDate;
                model.CreateBy = material.CreateBy;
                model.CreateDate = material.CreateDate;
                model.UpdateBy = material.UpdateBy;
                model.UpdateDate = material.UpdateDate;
                model.IsSendSale = material.IsSendSale;

                model.IsRedundant = material.IsRedundant;
                model.RedundantAmount = material.RedundantAmount;
                model.RedundantDescription = material.RedundantDescription;
                model.RedundantDeliveryNote = material.RedundantDeliveryNote;

                MaterialParameterBusiness materialParameterBusiness = new MaterialParameterBusiness();
                model.ListMaterialParameter = materialParameterBusiness.GetParameterByGroupId(model.MaterialGroupId);

                var listSpecification = db.Specifications.AsNoTracking().Where(a => a.MaterialId.Equals(model.Id)).ToList();

                foreach (var item in model.ListMaterialParameter)
                {
                    foreach (var item2 in item.ListValue)
                    {
                        foreach (var item3 in listSpecification)
                        {
                            if (item2.Id.Equals(item3.MaterialParameterValueId))
                            {
                                item2.IsChecked = true;
                            }
                        }
                    }
                }

                //var listMaterialPart = (from a in db.MaterialParts.AsNoTracking()
                //                        where a.MaterialItemId.Equals(model.Id)
                //                        join b in db.Modules on a.ModuleId equals b.Id
                //                        join c in db.ModuleGroups on b.ModuleGroupId equals c.Id
                //                        orderby a.Qty descending
                //                        select new MaterialPartModel
                //                        {
                //                            Id = a.Id,
                //                            ModuleId = a.ModuleId,
                //                            Qty = a.Qty,
                //                            MaterialItemId = a.MaterialItemId,
                //                            MaterialCode = a.MaterialCode,
                //                            MaterialName = a.MaterialName,
                //                            ModuleGroupName = c.Name
                //                        }).ToList();
                //model.ListMaterialPart = listMaterialPart;

                var listModuleMaterial = (from a in db.ModuleMaterials.AsNoTracking()
                                          where a.MaterialId.Equals(model.Id)
                                          join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                          join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                          group a by new
                                          {
                                              ModuleName = b.Name,
                                              ModuleCode = b.Code,
                                              ModuleGroupName = c.Name,
                                          } into g
                                          select new ModuleMaterialResultModel
                                          {
                                              ModuleName = g.Key.ModuleName,
                                              ModuleCode = g.Key.ModuleCode,
                                              ModuleGroupName = g.Key.ModuleGroupName,
                                              TotalQuantity = g.Sum(s => s.Quantity),
                                          }).ToList();

                model.ListMaterialPart = listModuleMaterial;
                var listMaterialDesign3D = (from a in db.MaterialDesign3D.AsNoTracking()
                                            join b in db.Design3D.AsNoTracking() on a.Design3DId equals b.Id
                                            join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                                            join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                            where a.MaterialId.Equals(model.Id)
                                            select new Design3DModel
                                            {
                                                Id = b.Id,
                                                FileName = b.FileName,
                                                FilePath = b.FilePath,
                                                Size = b.Size,
                                                CreateBy = b.CreateBy,
                                                CreateByName = d.Name,
                                                CreateDate = b.CreateDate,
                                                UpdateBy = b.UpdateBy,
                                                UpdateDate = b.UpdateDate
                                            }).ToList();
                model.ListFileDesign3D = listMaterialDesign3D;

                var dataSheet = (from a in db.MaterialDataSheets.AsNoTracking()
                                 join b in db.DataSheets.AsNoTracking() on a.DataSheetId equals b.Id
                                 join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                                 join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                 where a.MaterialId.Equals(model.Id)
                                 select new DataSheetModel
                                 {
                                     Id = b.Id,
                                     FileName = b.FileName,
                                     FilePath = b.FilePath,
                                     Size = b.Size,
                                     CreateBy = b.CreateBy,
                                     CreateByName = d.Name,
                                     CreateDate = b.CreateDate,
                                     UpdateBy = b.UpdateBy,
                                     UpdateDate = b.UpdateDate,
                                     ManufactureId = b.ManufactureId
                                 }).ToList();
                model.ListFileDataSheet = dataSheet;

                var listImage = (from a in db.MaterialImages.AsNoTracking()
                                 where a.MaterialId.Equals(model.Id)
                                 select new MaterialImageModel
                                 {
                                     Id = a.Id,
                                     MaterialId = a.MaterialId,
                                     Path = a.Path,
                                     ThumbnailPath = a.ThumbnailPath
                                 });
                model.ListImage = listImage.ToList();

                var listMaterialModuleGroup = db.MaterialModuleGroups.AsNoTracking().Where(m => m.MaterialId.Equals(model.Id)).ToList();
                List<string> listModuleId = new List<string>();
                foreach (var item in listMaterialModuleGroup)
                {
                    listModuleId.Add(item.ModuleGroupId);
                }
                model.ListModuleGroupId = listModuleId;
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý ");
            }
        }

        public List<MaterialPartModel> GetListModulePart(MaterialSearchModel model)
        {
            List<MaterialPartModel> listMaterialPart = new List<MaterialPartModel>();
            try
            {
                if (!string.IsNullOrEmpty(model.Id))
                {
                    listMaterialPart = (from a in db.MaterialParts.AsNoTracking()
                                        where a.MaterialItemId.Equals(model.Id)
                                        orderby a.Qty descending
                                        select new MaterialPartModel
                                        {
                                            Id = a.Id,
                                            ModuleId = a.ModuleId,
                                            MaterialCode = a.MaterialCode,
                                            MaterialItemId = a.MaterialItemId,
                                            MaterialName = a.MaterialName,
                                            Qty = a.Qty,
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.ErrosProcess");
            }
            return listMaterialPart;
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string materialName, supplierCode, materialCode, buyDate, quantity, price, priceUnit, totalPrice;
            var materials = db.Materials.AsNoTracking().Select(s => new MaterialModel { Id = s.Id, Code = s.Code, LastBuyDate = s.LastBuyDate, ManufactureId = s.ManufactureId }).ToList();
            var suppliers = db.Suppliers.AsNoTracking().Select(s => new SupplierModel { Id = s.Id, Code = s.Code }).ToList();
            var supplierManufactures = db.SupplierManufactures.AsNoTracking().ToList();
            var histories = db.MaterialBuyHistories.AsNoTracking().Select(s => new MaterialBuyHistoryModel { Id = s.Id, MaterialId = s.MaterialId, BuyDate = s.BuyDate, SupplierId = s.SupplierId, Price = s.Price }).ToList();

            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<MaterialBuyHistory> list = new List<MaterialBuyHistory>();
            List<MaterialBuyHistory> listUpdate = new List<MaterialBuyHistory>();
            List<SupplierManufacture> listSupplierManufacture = new List<SupplierManufacture>();
            MaterialBuyHistory itemC;
            List<int> rowSupplierErrors = new List<int>();
            List<int> rowMaterialErrors = new List<int>();
            List<int> rowMaterialNameErrors = new List<int>();
            List<int> rowMaterialEmpty = new List<int>();
            List<int> rowMaterialNameEmpty = new List<int>();
            List<int> rowSupplierEmpty = new List<int>();
            List<int> rowBuyDateEmpty = new List<int>();
            List<int> rowQuantityEmpty = new List<int>();
            List<int> rowPriceEmpty = new List<int>();
            List<int> rowCheckCodeName = new List<int>();
            List<MaterialModel> materialUpdates = new List<MaterialModel>();

            try
            {
                bool isUpdatePrice;
                MaterialModel material; MaterialModel materialU;
                for (int i = 2; i <= rowCount; i++)
                {
                    material = null;
                    itemC = new MaterialBuyHistory();
                    itemC.Id = Guid.NewGuid().ToString();
                    supplierCode = sheet[i, 1].Value;
                    materialName = sheet[i, 2].Value;
                    materialCode = sheet[i, 3].Value;
                    buyDate = sheet[i, 4].Value;
                    quantity = sheet[i, 5].Value;
                    price = sheet[i, 6].Value;
                    priceUnit = sheet[i, 7].Value;
                    totalPrice = sheet[i, 8].Value;

                    var manufactureId = string.Empty;
                    var supplierId = string.Empty;

                    try
                    {
                        if (!string.IsNullOrEmpty(materialCode))
                        {
                            material = materials.FirstOrDefault(t => t.Code.ToUpper().Trim().Equals(materialCode.ToUpper().Trim()));
                            if (material != null)
                            {
                                itemC.MaterialId = material.Id;
                                manufactureId = material.ManufactureId;
                            }
                            else
                            {
                                rowMaterialErrors.Add(i);
                            }
                        }
                        else
                        {
                            rowMaterialEmpty.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowMaterialErrors.Add(i);
                        continue;
                    }

                    //try
                    //{
                    //    if (!string.IsNullOrEmpty(materialName))
                    //    {
                    //        var test = materials.FirstOrDefault(t => t.Name.Equals(materialName)).Id;
                    //        if (itemC.MaterialId != test)
                    //        {
                    //            rowCheckCodeName.Add(i);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        rowMaterialNameEmpty.Add(i);
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //    rowMaterialNameErrors.Add(i);
                    //    continue;
                    //}

                    try
                    {
                        if (!string.IsNullOrEmpty(supplierCode))
                        {
                            var supplier = suppliers.FirstOrDefault(u => u.Code.ToUpper().Trim().Equals(supplierCode.ToUpper().Trim()));
                            if (supplier != null)
                            {
                                itemC.SupplierId = supplier.Id;
                                supplierId = supplier.Id;
                            }
                            else
                            {
                                rowSupplierErrors.Add(i);
                            }

                        }
                        else
                        {
                            rowSupplierEmpty.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowSupplierErrors.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            itemC.Quantity = decimal.Parse(quantity);
                        }
                        else
                        {
                            rowQuantityEmpty.Add(i);
                        }
                    }
                    catch
                    {
                        rowQuantityEmpty.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = decimal.Parse(price);
                        }
                        else
                        {
                            rowPriceEmpty.Add(i);
                        }
                    }
                    catch
                    {
                        rowPriceEmpty.Add(i);
                        continue;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    try
                    {
                        if (!string.IsNullOrEmpty(buyDate))
                        {
                            itemC.BuyDate = DateTime.Parse(buyDate);
                        }
                        else
                        {
                            rowBuyDateEmpty.Add(i);
                        }
                    }
                    catch
                    {
                        rowBuyDateEmpty.Add(i);
                        continue;
                    }

                    var check = histories.FirstOrDefault(t => t.MaterialId.Equals(itemC.MaterialId) && t.SupplierId.Equals(itemC.SupplierId) && t.BuyDate == itemC.BuyDate && t.Price.Equals(itemC.Price));
                    if (check != null)
                    {
                        itemC.Id = check.Id;
                        listUpdate.Add(itemC);
                    }
                    else
                    {
                        list.Add(itemC);
                    }

                    if (material != null)
                    {
                        isUpdatePrice = false;
                        if (material.LastBuyDate.HasValue)
                        {
                            if (material.LastBuyDate.Value < itemC.BuyDate)
                            {
                                material.Pricing = itemC.Price;
                                material.LastBuyDate = itemC.BuyDate;
                                material.PriceHistory = itemC.Price;
                                isUpdatePrice = true;
                            }
                        }
                        else
                        {
                            isUpdatePrice = true;
                            material.Pricing = itemC.Price;
                            material.LastBuyDate = itemC.BuyDate;
                            material.PriceHistory = itemC.Price;
                        }

                        if (isUpdatePrice)
                        {
                            materialU = materialUpdates.FirstOrDefault(r => r.Id.Equals(material.Id));
                            if (materialU == null)
                            {
                                materialUpdates.Add(new MaterialModel
                                {
                                    Id = material.Id,
                                    Pricing = material.Pricing,
                                    LastBuyDate = material.LastBuyDate,
                                    PriceHistory = material.PriceHistory
                                });
                            }
                            else
                            {
                                materialU.Pricing = material.Pricing;
                                materialU.LastBuyDate = material.LastBuyDate;
                                materialU.PriceHistory = material.PriceHistory;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(manufactureId) && !string.IsNullOrEmpty(supplierId))
                    {
                        var supplierManufacture = supplierManufactures.FirstOrDefault(t => manufactureId.Equals(t.ManufactureId) && supplierId.Equals(t.SupplierId));
                        if (supplierManufacture == null)
                        {
                            SupplierManufacture entity = new SupplierManufacture()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = manufactureId,
                                SupplierId = supplierId
                            };
                            supplierManufactures.Add(entity);
                            listSupplierManufacture.Add(entity);
                        }
                    }
                }

                #endregion    
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            if (rowCheckCodeName.Count > 0)
            {
                throw NTSException.CreateInstance("Vật tư dòng <" + string.Join(", ", rowCheckCodeName) + "> không tồn tại!");
            }

            if (rowMaterialEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialEmpty) + "> không được phép để trống!");
            }

            if (rowMaterialNameEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialEmpty) + "> không được phép để trống!");
            }

            if (rowSupplierEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Mã NCC dòng <" + string.Join(", ", rowSupplierEmpty) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowBuyDateEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày mua dòng <" + string.Join(", ", rowBuyDateEmpty) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowQuantityEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantityEmpty) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowPriceEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowPriceEmpty) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowMaterialErrors.Count > 0)
            {
                throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialErrors) + "> không tồn tại trên hệ thống. Bạn hãy kiểm tra lại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowMaterialNameErrors.Count > 0)
            {
                throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialNameErrors) + "> không tồn tại trên hệ thống. Bạn hãy kiểm tra lại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowSupplierErrors.Count > 0)
            {
                throw NTSException.CreateInstance("Mã NCC dòng <" + string.Join(", ", rowSupplierErrors) + "> không tồn tại trên hệ thống. Bạn hãy kiểm tra lại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //Update giá mới cho vật tư
                    foreach (var item in materialUpdates)
                    {
                        var material = db.Materials.FirstOrDefault(t => t.Id.Equals(item.Id));
                        if (material != null)
                        {
                            material.Pricing = item.Pricing;
                            material.PriceHistory = item.PriceHistory;
                            material.LastBuyDate = item.LastBuyDate;
                        }
                    }

                    foreach (var item in listUpdate)
                    {
                        var materialBuyHistorie = db.MaterialBuyHistories.FirstOrDefault(t => t.Id.Equals(item.Id));
                        if (materialBuyHistorie != null)
                        {
                            materialBuyHistorie.MaterialId = item.MaterialId;
                            materialBuyHistorie.SupplierId = item.SupplierId;
                            materialBuyHistorie.BuyDate = item.BuyDate;
                            materialBuyHistorie.Quantity = item.Quantity;
                            materialBuyHistorie.Price = item.Price;
                            materialBuyHistorie.UpdateBy = userId;
                            materialBuyHistorie.UpdateDate = DateTime.Now;
                        }
                    }

                    db.SupplierManufactures.AddRange(listSupplierManufacture);
                    db.MaterialBuyHistories.AddRange(list);

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

        public bool CreatNewBuyHistory(string userId, ImportMaterialBuyHistoryResult model)
        {
            bool rs = false;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<MaterialBuyHistory> list = new List<MaterialBuyHistory>();
                    foreach (var item in model.ListExist)
                    {
                        MaterialBuyHistory entity = new MaterialBuyHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MaterialId = item.MaterialId,
                            SupplierId = item.SupplierId,
                            BuyDate = item.BuyDate,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            CreateBy = userId,
                            UpdateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        };
                        list.Add(entity);
                    }
                    db.MaterialBuyHistories.AddRange(list);

                    //Update giá mới cho vật tư
                    foreach (var item in model.ListExist)
                    {
                        var material = db.Materials.FirstOrDefault(t => t.Id.Equals(item.MaterialId));
                        if (material.LastBuyDate.HasValue)
                        {
                            if (material.LastBuyDate.Value < item.BuyDate)
                            {
                                material.Pricing = item.Price;
                                material.LastBuyDate = item.BuyDate;
                            }
                        }
                        else
                        {
                            material.Pricing = item.Price;
                            material.LastBuyDate = item.BuyDate;
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
            return rs;
        }

        public bool OverwriteBuyHistory(string userId, ImportMaterialBuyHistoryResult model)
        {
            bool rs = false;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.ListExist)
                    {
                        var entity = db.MaterialBuyHistories.Where(t => t.MaterialId.Equals(item.MaterialId) && t.SupplierId.Equals(item.SupplierId) && t.BuyDate == item.BuyDate).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                        entity.MaterialId = item.MaterialId;
                        entity.SupplierId = item.SupplierId;
                        entity.BuyDate = item.BuyDate;
                        entity.Quantity = item.Quantity;
                        entity.Price = item.Price;
                        entity.UpdateBy = userId;
                        entity.UpdateDate = DateTime.Now;
                    }
                    //Update giá mới cho vật tư
                    foreach (var item in model.ListExist)
                    {
                        var material = db.Materials.FirstOrDefault(t => t.Id.Equals(item.MaterialId));
                        if (material.LastBuyDate.HasValue)
                        {
                            if (material.LastBuyDate.Value < item.BuyDate)
                            {
                                material.Pricing = item.Price;
                                material.LastBuyDate = item.BuyDate;
                            }
                        }
                        else
                        {
                            material.Pricing = item.Price;
                            material.LastBuyDate = item.BuyDate;
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
            return rs;
        }

        public MaterialBuyHistoryResultModel GetHistoryByMaterialId(MaterialBuyHistorySearchModel model)
        {
            MaterialBuyHistoryResultModel rs = new MaterialBuyHistoryResultModel();
            List<MaterialBuyHistoryModel> ret = new List<MaterialBuyHistoryModel>();
            try
            {
                var data = (from a in db.MaterialBuyHistories.AsNoTracking()
                            join b in db.Suppliers.AsNoTracking() on a.SupplierId equals b.Id
                            where a.MaterialId.Equals(model.Id)
                            orderby a.BuyDate descending
                            select new MaterialBuyHistoryModel
                            {
                                Id = a.Id,
                                SupplierName = b.Name,
                                SupplierCode = b.Code,
                                BuyDate = a.BuyDate,
                                SupplierId = a.SupplierId,
                                MaterialId = a.MaterialId,
                                Quantity = a.Quantity,
                                Price = a.Price,
                                Total = a.Quantity * a.Price,
                                DateCount = EntityFunctions.DiffDays(a.BuyDate, DateTime.Now).Value
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(model.Filter))
                {
                    data = data.Where(t => t.SupplierCode.Contains(model.Filter) || t.SupplierName.Contains(model.Filter));
                }

                if (data.ToList().Count > 0)
                {
                    ret = data.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
                }

                rs.ListMaterialBuyHistory = ret;
                rs.TotalItem = data.ToList().Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return rs;
        }

        public string ExportExcel(MaterialSearchModel modelSearch)
        {
            modelSearch.IsExport = true;
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Units.AsNoTracking() on a.UnitId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.RawMaterials.AsNoTracking() on a.RawMaterialId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             orderby a.Name
                             select new MaterialResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a == null ? "" : a.ManufactureId,
                                 UnitId = a.UnitId,
                                 MaterialGroupId = a.MaterialGroupId,
                                 ManufactureCode = d.Code,
                                 UnitName = b == null ? "" : b.Name,
                                 MaterialGroupName = c.Code,
                                 Pricing = a.Pricing,
                                 Note = a.Note,
                                 DeliveryDays = a.DeliveryDays,
                                 ThumbnailPath = a.ThumbnailPath,
                                 LastBuyDate = a.LastBuyDate,
                                 IsUsuallyUse = a.IsUsuallyUse,
                                 MechanicalType = a.MechanicalType,
                                 Status = a.Status,
                                 RawMaterialName = e == null ? "" : e.Name,
                                 Is3D = a.Is3D,
                                 IsDataSheet = a.IsDataSheet,
                                 MaterialType = a.MaterialType,
                                 Is3DExist = a.Is3DExist,
                                 IsDataSheetExist = a.IsDatasheetExist,
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
                dataQuery = dataQuery.Where(u => u.ManufactureId.ToUpper().Equals(modelSearch.ManufactureId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.MaterialType))
            {
                dataQuery = dataQuery.Where(u => u.MaterialType.ToUpper().Equals(modelSearch.MaterialType.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.MaterialGroupId))
            {
                dataQuery = dataQuery.Where(u => u.MaterialGroupId.ToUpper().Equals(modelSearch.MaterialGroupId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.IsAllFile))
            {
                if (modelSearch.IsAllFile.Equals("1"))
                {
                    dataQuery = dataQuery.Where(u => u.Is3DExist && u.IsDataSheetExist);
                }
                else
                {
                    dataQuery = dataQuery.Where(u => !u.Is3DExist || !u.IsDataSheetExist);
                }
            }

            if (!string.IsNullOrEmpty(modelSearch.Status))
            {
                dataQuery = dataQuery.Where(u => u.Status.Equals(modelSearch.Status));
            }

            List<MaterialResultModel> listMaterial = dataQuery.ToList();
            //foreach (var item in listMaterial)
            //{
            //    if (item.MaterialType.Equals("1"))
            //    {
            //        item.MaterialType = "Vật tư tiêu chuẩn";
            //    }
            //    else if (item.MaterialType.Equals("2"))
            //    {
            //        item.MaterialType = "Vật tư phi tiêu chuẩn";
            //    }

            //    //if (item.Status.Equals("0"))
            //    //{
            //    //    item.Status = "Đang sử dụng";
            //    //}
            //    //else if (item.Status.Equals("1"))
            //    //{
            //    //    item.Status = "Tạm dừng";
            //    //}
            //    //else if (item.Status.Equals("2"))
            //    //{
            //    //    item.Status = "Ngưng sản xuất";
            //    //}
            //}
            //var listMaterialDesign = db.MaterialDesign3D.AsNoTracking();

            if (listMaterial.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Material.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = listMaterial.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = listMaterial.Select((o, i) => new
                {
                    Index = i + 1,
                    o.MaterialGroupName,
                    o.Code,
                    o.Name,
                    o.ManufactureCode,
                    viewMaterialType = o.MaterialType.Equals("1") ? "Vật tư tiêu chuẩn" : "Vật tư phi tiêu chuẩn",
                    o.UnitName,
                    viewThumbnailPath = !string.IsNullOrEmpty(o.ThumbnailPath) ? "Có" : "Không",
                    o.Pricing,
                    o.DeliveryDays,
                    o.LastDelivery,
                    viewStatus = o.Status.Equals("0") ? "Đang sử dụng" : o.Status.Equals("1") ? "Tạm dừng" : "Ngưng sản xuất",
                    o.RawMaterialName,
                    o.MechanicalType,
                    viewIsDocument3D = o.Is3DExist == true ? "Có tài liệu" : "Không có tài liệu",
                    viewIsDocumentDataSheet = o.IsDataSheetExist == true ? "Có tài liệu" : "Không có tài liệu"
                }).ToList();

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                //var range = sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders;
                ////range.LineStyle = ExcelLineStyle.Thin;

                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 15].CellStyle.WrapText = true;

                var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vật tư_" + time + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vật tư_" + time + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new NTSLogException(modelSearch, ex);
            }
        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/VatTu_Import_Template.xlsx"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets[2];
            IWorksheet sheet3 = workbook.Worksheets[3];
            IWorksheet sheet4 = workbook.Worksheets[4];

            var listGroup = db.MaterialGroups.AsNoTracking().Select(i => i.Code).ToList();
            var listGroupTPA = db.MaterialGroupTPAs.AsNoTracking().Select(i => i.Code).ToList();
            var listManufacture = db.Manufactures.AsNoTracking().Select(i => i.Code).ToList();
            var listUnit = db.Units.AsNoTracking().Select(i => i.Name).ToList();
            sheet0.Range["B3:B1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
            sheet0.Range["G3:G1000"].DataValidation.DataRange = sheet2.Range["A1:A1000"];
            sheet0.Range["I3:I1000"].DataValidation.DataRange = sheet3.Range["A1:A1000"];
            sheet0.Range["C3:C1000"].DataValidation.DataRange = sheet4.Range["A1:A1000"];
            IRange iRangeData = sheet1.FindFirst("<materialGroupCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<materialGroupCode>", string.Empty);
            IRange iRangeDatas = sheet2.FindFirst("<manufacture>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDatas.Text = iRangeDatas.Text.Replace("<manufacture>", string.Empty);
            IRange iRangeDatass = sheet3.FindFirst("<unit>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDatass.Text = iRangeDatass.Text.Replace("<unit>", string.Empty);
            IRange iRangeData4 = sheet4.FindFirst("<materialGroupTPA>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData4.Text = iRangeData4.Text.Replace("<materialGroupTPA>", string.Empty);
            var listExport = listGroup.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            var listExports = listManufacture.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            var listExportss = listUnit.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            var listExport4 = listGroupTPA.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            sheet1.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet2.ImportData(listExports, iRangeDatas.Row, iRangeDatas.Column, false);
            sheet3.ImportData(listExportss, iRangeDatass.Row, iRangeDatass.Column, false);
            sheet4.ImportData(listExport4, iRangeData4.Row, iRangeData4.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "VatTu_Import_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "VatTu_Import_Template" + ".xls";

            return resultPathClient;
        }

        public void ImportFileMaterial(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string materialGroup, materialGroupTPA, name, code, materialType, manufacture, price, unitname, deliveryDay, status, rawMaterial;
            var materials = db.Materials.AsNoTracking().Select(s => new MaterialModel
            {
                Id = s.Id,
                Code = s.Code
            }).ToList();

            var listManufacture = db.Manufactures.AsNoTracking().ToList();

            var materialGroups = db.MaterialGroups.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<Material> list = new List<Material>();
            Material itemC;
            List<int> rowMaterialGroup = new List<int>();
            List<int> rowMaterialGroupTPA = new List<int>();
            List<int> rowName = new List<int>();
            List<int> rowCode = new List<int>();
            List<int> rowMaterialType = new List<int>();
            List<int> rowManufacture = new List<int>();
            List<int> rowUnit = new List<int>();
            List<int> rowSatus = new List<int>();
            List<int> rowDate = new List<int>();
            List<int> rowCheckCode = new List<int>();
            //List<int> rowCheckName = new List<int>();
            List<int> rowCheckMaterialGroup = new List<int>();
            List<int> rowCheckMaterialGroupTPA = new List<int>();
            List<int> rowCheckMaterialType = new List<int>();
            List<int> rowCheckManufacture = new List<int>();
            List<int> rowCheckUnit = new List<int>();
            List<int> rowCheckStatus = new List<int>();
            List<int> rowCheckUnicode = new List<int>();
            List<int> rowPrice = new List<int>();
            MaterialGroup mGroup;
            MaterialGroupTPA mGroupTPA;
            Manufacture manu;
            Unit unit;
            try
            {
                MaterialModel MaterialExist;
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new Material();
                    itemC.Id = Guid.NewGuid().ToString();
                    materialGroup = sheet[i, 2].Value;
                    materialGroupTPA = sheet[i, 3].Value;
                    name = sheet[i, 4].Value;
                    code = sheet[i, 5].Value;
                    materialType = sheet[i, 6].Value;
                    manufacture = sheet[i, 7].Value;
                    price = sheet[i, 8].Value;
                    unitname = sheet[i, 9].Value;
                    deliveryDay = sheet[i, 10].Value;
                    status = sheet[i, 11].Value;
                    rawMaterial = sheet[i, 12].Value;

                    //Nhóm vật tư

                    if (!string.IsNullOrEmpty(materialGroup))
                    {
                        mGroup = materialGroups.FirstOrDefault(u => u.Code.Equals(materialGroup.Trim()));

                        if (mGroup == null)
                        {
                            rowCheckMaterialGroup.Add(i);
                            continue;
                        }

                        itemC.MaterialGroupId = mGroup.Id;
                    }
                    else
                    {
                        rowMaterialGroup.Add(i);
                    }

                    //Nhóm vật tư TPA
                    if (!string.IsNullOrEmpty(materialGroupTPA))
                    {
                        mGroupTPA = db.MaterialGroupTPAs.AsNoTracking().FirstOrDefault(u => u.Code.Equals(materialGroupTPA.Trim()));

                        if (mGroupTPA == null)
                        {
                            rowCheckMaterialGroupTPA.Add(i);
                            continue;
                        }

                        itemC.MaterialGroupTPAId = mGroupTPA.Id;
                    }
                    else
                    {
                        rowMaterialGroupTPA.Add(i);
                    }

                    //Name
                    if (!string.IsNullOrEmpty(name))
                    {
                        itemC.Name = name.Trim();
                    }
                    else
                    {
                        rowName.Add(i);
                        continue;
                    }

                    //Code
                    if (!string.IsNullOrEmpty(code))
                    {
                        bool checkss = hasSpecialChar(code.Trim());
                        if (checkss == true)
                        {
                            rowCheckUnicode.Add(i);
                        }
                        else
                        {
                            MaterialExist = materials.FirstOrDefault(s => s.Code.ToLower().Equals(code.ToLower()));
                            if (MaterialExist != null)
                            {
                                rowCheckCode.Add(i);
                                continue;
                            }
                            else
                            {
                                itemC.Code = code.Trim();
                            }
                        }
                    }
                    else
                    {
                        rowCode.Add(i);
                        continue;
                    }

                    //MaterialType

                    if (!string.IsNullOrEmpty(materialType))
                    {
                        if (materialType.Trim().Equals("Vật tư tiêu chuẩn"))
                        {
                            itemC.MaterialType = "1";
                        }
                        else if (materialType.Trim().Equals("Vật tư phi tiêu chuẩn"))
                        {
                            itemC.MaterialType = "2";
                        }

                        if (itemC.MaterialType == null)
                        {
                            rowCheckMaterialType.Add(i);
                            continue;
                        }
                    }
                    else
                    {
                        rowMaterialType.Add(i);
                        continue;
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        manu = db.Manufactures.AsNoTracking().FirstOrDefault(u => u.Code.Equals(manufacture.Trim()));

                        if (manu == null)
                        {
                            rowCheckManufacture.Add(i);
                            continue;
                        }

                        itemC.ManufactureId = manu.Id;
                    }
                    else
                    {
                        rowManufacture.Add(i);
                        continue;
                    }

                    try
                    {
                        //Price
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Pricing = Convert.ToDecimal(price.Trim());
                        }
                        else
                        {
                            itemC.Pricing = 0;
                        }
                    }
                    catch (Exception)
                    {
                        rowPrice.Add(i);
                        continue;
                    }

                    //Unit

                    if (!string.IsNullOrEmpty(unitname))
                    {
                        unit = db.Units.AsNoTracking().FirstOrDefault(u => u.Name.Equals(unitname.Trim()));

                        if (unit == null)
                        {
                            rowCheckUnit.Add(i);
                            continue;
                        }

                        itemC.UnitId = unit.Id;
                    }
                    else
                    {
                        rowUnit.Add(i);
                        continue;
                    }

                    //DeliveryDay
                    try
                    {
                        if (!string.IsNullOrEmpty(deliveryDay))
                        {
                            itemC.DeliveryDays = Convert.ToInt32(deliveryDay.Trim());
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(manufacture)) {
                                var manufactures = listManufacture.FirstOrDefault(r => r.Code.Equals(manufacture.Trim()));
                                if (manufactures == null)
                                {
                                    itemC.DeliveryDays = 0;
                                }
                                else
                                {
                                    itemC.DeliveryDays = manufactures.LeadTime;
                                } 
                            }
                        }
                    }
                    catch (Exception)
                    {
                        rowDate.Add(i);
                        continue;
                    }

                    //Status
                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status.Trim().Equals("Đang sử dụng"))
                        {
                            itemC.Status = "0";
                        }
                        else if (status.Trim().Equals("Tạm dừng"))
                        {
                            itemC.Status = "1";
                        }
                        else if (status.Trim().Equals("Ngừng sản xuất"))
                        {
                            itemC.Status = "2";
                        }

                        if (itemC.Status == null)
                        {
                            rowCheckStatus.Add(i);
                            continue;
                        }
                    }
                    else
                    {
                        rowUnit.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(rawMaterial))
                    {
                        itemC.RawMaterial = rawMaterial;
                    }
                    itemC.Is3D = true;
                    itemC.IsDataSheet = true;
                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(itemC.Code))
                    {
                        if (list.Count(r => r.Code.ToUpper() == itemC.Code.ToUpper()) == 0)
                        {
                            list.Add(itemC);
                        }
                    }

                }

                #endregion

                if (rowCheckUnicode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCheckUnicode) + "> chứa ký tự đặc biệt hoặc chữ có dấu!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCheckCode) + "> đã tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckMaterialGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Nhóm vật tư dòng <" + string.Join(", ", rowCheckMaterialGroup) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckMaterialGroupTPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Nhóm vật tư TPA dòng <" + string.Join(", ", rowCheckMaterialGroupTPA) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckMaterialType.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại vật tư dòng <" + string.Join(", ", rowCheckMaterialType) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckManufacture.Count > 0)
                {
                    throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", rowCheckManufacture) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckUnit.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị tính dòng <" + string.Join(", ", rowCheckUnit) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckStatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Tình trạng vật tư dòng <" + string.Join(", ", rowCheckStatus) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowDate.Count > 0)
                {
                    throw NTSException.CreateInstance("Thời gian đặt hàng dòng <" + string.Join(", ", rowDate) + "> không đúng. Giá trị phải là số!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
                }

                if (rowName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowMaterialGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Nhóm vật tư dòng <" + string.Join(", ", rowMaterialGroup) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowMaterialGroupTPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Nhóm vật tư TPA dòng <" + string.Join(", ", rowMaterialGroupTPA) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowMaterialType.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại vật tư dòng <" + string.Join(", ", rowMaterialType) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowManufacture.Count > 0)
                {
                    throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", rowManufacture) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowUnit.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị tính dòng <" + string.Join(", ", rowUnit) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowSatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Tình trạng vật tư dòng <" + string.Join(", ", rowSatus) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá vật tư dòng <" + string.Join(", ", rowSatus) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                db.Materials.AddRange(list);
                db.SaveChanges();
            }
            catch (NTSException ntsEx)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ntsEx;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
        }

        public List<MaterialFromDBModel> GetListMaterial()
        {
            var rs = (from a in db.Materials.AsNoTracking()
                      join m in db.Manufactures.AsNoTracking() on a.ManufactureId equals m.Id
                      select new MaterialFromDBModel
                      {
                          Id = a.Id,
                          Code = a.Code,
                          Name = a.Name,
                          Status = a.Status,
                          ManufactureId = a.ManufactureId,
                          ManufactureCode = m.Code,
                          Pricing = a.Pricing,
                          CreateDate = a.CreateDate,
                          MaterialGroupId = a.MaterialGroupId,
                          Note = a.Note,
                          UnitName = a.UnitId
                      }).ToList();
            return rs;
        }

        private static readonly string[] VietNamChar = new string[]
        {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
        };

        public static string LocDau(string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            return str;
        }

        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ*{}!^<>?| ";
            foreach (var item in specialChar)
            {
                if (input.Contains(item))
                    return true;
            }

            return false;
        }

        public string ExportDMVTNotDB(List<MaterialExportModel> listDMVTNotDB)
        {
            if (listDMVTNotDB.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0039, TextResourceKey.MaterialNotDB);
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/VatTu_Import_Not_DB_Template.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = listDMVTNotDB.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                IWorksheet sheet1 = workbook.Worksheets[1];
                IWorksheet sheet2 = workbook.Worksheets[2];
                IWorksheet sheet3 = workbook.Worksheets[3];
                IWorksheet sheet4 = workbook.Worksheets[4];

                var listGroup = db.MaterialGroups.AsNoTracking().Select(i => i.Code).ToList();
                var listGroupTPA = db.MaterialGroupTPAs.AsNoTracking().Select(i => i.Code).ToList();
                var listManufacture = db.Manufactures.AsNoTracking().Select(i => i.Code).ToList();
                var listUnit = db.Units.AsNoTracking().Select(i => i.Name).ToList();
                sheet.Range["B3:B1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
                sheet.Range["G3:G1000"].DataValidation.DataRange = sheet2.Range["A1:A1000"];
                sheet.Range["I3:I1000"].DataValidation.DataRange = sheet3.Range["A1:A1000"];
                sheet.Range["C3:C1000"].DataValidation.DataRange = sheet4.Range["A1:A1000"];
                IRange iRangeData_1 = sheet1.FindFirst("<materialGroupCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData_1.Text = iRangeData_1.Text.Replace("<materialGroupCode>", string.Empty);
                IRange iRangeData_2 = sheet2.FindFirst("<manufacture>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData_2.Text = iRangeData_2.Text.Replace("<manufacture>", string.Empty);
                IRange iRangeData_3 = sheet3.FindFirst("<unit>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData_3.Text = iRangeData_3.Text.Replace("<unit>", string.Empty);
                IRange iRangeData_4 = sheet4.FindFirst("<materialGroupTPA>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData_4.Text = iRangeData_4.Text.Replace("<materialGroupTPA>", string.Empty);
                var listExport_1 = listGroup.OrderBy(a => a).Select((o, i) => new
                {
                    o,
                });

                var listExport_2 = listManufacture.OrderBy(a => a).Select((o, i) => new
                {
                    o,
                });

                var listExport_3 = listUnit.OrderBy(a => a).Select((o, i) => new
                {
                    o,
                });

                var listExport_4 = listGroupTPA.OrderBy(a => a).Select((o, i) => new
                {
                    o,
                });

                var listExport = listDMVTNotDB.Select((o, i) => new
                {
                    Index = i + 1,
                    g = string.Empty,
                    a = string.Empty,
                    o.Name,
                    o.Code,
                    materialType = string.Empty,
                    o.ManufactureCode,
                    price = string.Empty,
                    o.DV,
                    deliveryDays = string.Empty,
                    p = string.Empty,
                    o.VL,
                });
                sheet1.ImportData(listExport_1, iRangeData_1.Row, iRangeData_1.Column, false);
                sheet2.ImportData(listExport_2, iRangeData_2.Row, iRangeData_2.Column, false);
                sheet3.ImportData(listExport_3, iRangeData_3.Row, iRangeData_3.Column, false);
                sheet4.ImportData(listExport_4, iRangeData_4.Row, iRangeData_4.Column, false);
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 15].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vật tư không có trên hệ thống" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vật tư không có trên hệ thống" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new NTSLogException(null, ex);
            }
        }

        /// <summary>
        /// Kiem tra gia cua vat tu
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public SearchResultMaterialPriceModel<ModuleMaterialResultModel> CheckPriceMaterial(string userId, HttpPostedFile file)
        {
            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            ModuleBussiness moduleBussiness = new ModuleBussiness();
            SearchResultMaterialPriceModel<ModuleMaterialResultModel> result = new SearchResultMaterialPriceModel<ModuleMaterialResultModel>();

            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038, TextResourceKey.File);
            }

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();

            ModuleMaterialResultModel materialModel;
            ModuleMaterialResultModel materialSubModel;
            Material material;
            Manufacture manufacture;
            Unit unit;
            RawMaterial rawMaterial;
            List<ModuleMaterialResultModel> materials = new List<ModuleMaterialResultModel>();
            for (int i = 7; i < rowCount; i++)
            {
                if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value))
                {
                    materialModel = new ModuleMaterialResultModel()
                    {
                        Index = sheet[i, 1].Value,
                        MaterialName = sheet[i, 2].Value,
                        Specification = sheet[i, 3].Value,
                        MaterialCode = sheet[i, 4].Value,
                        RawMaterialCode = sheet[i, 5].Value,
                        UnitName = sheet[i, 6].Value,
                        Quantity = sheet[i, 7].Value.ConvertToDecimal(),
                        RawMaterial = sheet[i, 8].Value,
                        Weight = sheet[i, 9].Value.ConvertToDecimal(),
                        ManufacturerCode = sheet[i, 10].Value,
                        Note = sheet[i, 11].Text,
                        ReadQuantity = sheet[i, 7].Value.ConvertToDecimal()
                    };

                    material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(materialModel.MaterialCode.ToUpper()));

                    if (material == null)
                    {
                        materialModel.Pricing = 0;
                        materialModel.LastBuyDate = null;
                        materialModel.PriceHistory = 0;
                        materialModel.InputPriceDate = null;
                    }
                    else
                    {
                        materialModel.Pricing = material.Pricing;
                        materialModel.LastBuyDate = material.LastBuyDate;
                        materialModel.PriceHistory = material.PriceHistory;
                        materialModel.InputPriceDate = material.InputPriceDate;
                    }

                    materials.Add(materialModel);

                    if (!string.IsNullOrEmpty(materialModel.RawMaterialCode) && materialModel.Weight > 0 && Constants.Manufacture_TPA.Equals(materialModel.Specification.ToUpper()))
                    {
                        material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(materialModel.RawMaterialCode.ToUpper()));

                        if (material == null)
                        {
                            continue;
                        }

                        materialSubModel = new ModuleMaterialResultModel()
                        {
                            Index = materialModel.Index + ".1",
                            MaterialName = material.Name,
                            MaterialCode = material.Code,
                            Weight = 0,
                            ManufacturerCode = materialModel.ManufacturerCode,
                            Price = material.Pricing
                        };

                        materialSubModel.Pricing = material.Pricing;
                        materialSubModel.LastBuyDate = material.LastBuyDate;
                        materialSubModel.PriceHistory = material.PriceHistory;
                        materialSubModel.InputPriceDate = material.InputPriceDate;

                        manufacture = db.Manufactures.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.ManufactureId));
                        materialSubModel.ManufacturerCode = manufacture.Code;

                        unit = db.Units.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.UnitId));
                        materialSubModel.UnitName = unit.Name;

                        var unitKg = db.Units.AsNoTracking().FirstOrDefault(r => r.Name.ToUpper().Equals(Constants.Unit_Kg));

                        if (unitKg != null)
                        {
                            materialSubModel.Quantity = moduleBussiness.GetTotalByConvertUnit(material.Id, unitKg.Id, unit.Id, materialModel.Weight, materialModel.Quantity);
                        }
                        else
                        {
                            materialSubModel.Quantity = materialModel.Quantity;
                        }

                        materialSubModel.ReadQuantity = materialSubModel.Quantity;

                        materials.Add(materialSubModel);
                    }
                    else if (string.IsNullOrEmpty(materialModel.RawMaterialCode) && !string.IsNullOrEmpty(materialModel.RawMaterial) && materialModel.Weight > 0
                   && Constants.Manufacture_TPA.Equals(materialModel.Specification.ToUpper()) && Constants.Manufacture_TPA.Equals(materialModel.ManufacturerCode.ToUpper()))
                    {
                        rawMaterial = db.RawMaterials.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(materialModel.RawMaterial.ToUpper()));

                        if (rawMaterial == null || string.IsNullOrEmpty(rawMaterial.MaterialId))
                        {
                            continue;
                        }

                        material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Id.Equals(rawMaterial.MaterialId));

                        if (material == null)
                        {
                            continue;
                        }

                        materialSubModel = new ModuleMaterialResultModel()
                        {
                            Index = materialModel.Index + ".1",
                            MaterialName = material.Name,
                            MaterialCode = material.Code,
                            Weight = 0,
                            ManufacturerCode = materialModel.ManufacturerCode,
                            Quantity = materialModel.Weight,
                            Price = material.Pricing
                        };


                        materialSubModel.ReadQuantity = materialSubModel.Quantity;

                        materialSubModel.Pricing = material.Pricing;
                        materialSubModel.LastBuyDate = material.LastBuyDate;
                        materialSubModel.PriceHistory = material.PriceHistory;
                        materialSubModel.InputPriceDate = material.InputPriceDate;

                        manufacture = db.Manufactures.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.ManufactureId));
                        materialSubModel.ManufacturerCode = manufacture.Code;

                        unit = db.Units.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.UnitId));
                        materialSubModel.UnitName = unit.Name;

                        materials.Add(materialSubModel);
                    }
                }
                else
                {
                    break;
                }
            }

            moduleMaterialBusiness.UpdateMaterialPrice(materials);

            result.ListResult = materials;
            result.TotalItem = materials.Count;
            result.TotalAmount = materials.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);
            return result;
        }

        public string ExportCheckPrice(List<ModuleMaterialResultModel> materials)
        {
            if (materials == null || materials.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_Material_Price.xlsm"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = materials.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = materials.Select((o, i) => new
                {
                    o.Index,
                    o.MaterialName,
                    o.Specification,
                    o.MaterialCode,
                    o.RawMaterialCode,
                    o.UnitName,
                    o.Quantity,
                    o.RawMaterial,
                    o.Weight,
                    o.ManufacturerCode,
                    o.Note,
                    o.Pricing
                });

                if (materials.Count > 10)
                {
                    sheet.InsertRow(iRangeData.Row + 1, materials.Count - 9, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 12].CellStyle.WrapText = true;

                string resultPathClient = "Template/" + Constants.FolderExport + DateTime.Now.ToString("yyyyMMddHHmmss") + "_Danh sách vật tư kiểm tra giá" + ".xlsm";

                string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new NTSLogException(materials, ex);
            }
        }

        public int GetConfigMaterialLastByDate()
        {
            int day = 0;
            RedisService<ConfigModel> redisService = RedisService<ConfigModel>.GetInstance();

            // Key config vật tư
            string keyConfig = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheConfig"] + Constants.Config_Material_LastBuy;
            if (redisService.Exists(keyConfig))
            {
                day = redisService.Get<int>(keyConfig);
            }
            else
            {
                var config = db.Configs.AsNoTracking().Where(r => r.Code.Equals(Constants.Config_Material_LastBuy)).FirstOrDefault();
                day = Convert.ToInt32(config.Value);

                // Lưu cache vật tư
                redisService.Add<int>(keyConfig, day);
            }
            return day;
        }

        public string ImportFileSync(HttpPostedFile file, bool isConfirm, string userId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            string code;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<int> rowCode = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<string> listId = new List<string>();
            string confirm = string.Empty;

            try
            {
                var list = db.Materials.AsNoTracking().ToList();
                for (int i = 3; i <= rowCount; i++)
                {
                    code = sheet[i, 2].Value;

                    if (!string.IsNullOrEmpty(code))
                    {
                        var data = list.FirstOrDefault(a => a.Code.Trim().Equals(code.Trim()));
                        if (data != null)
                        {
                            listId.Add(data.Id);
                        }
                        else
                        {
                            rowCheckCode.Add(i);
                        }
                    }
                    else
                    {
                        rowCode.Add(i);
                    }
                }

                #endregion

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCode) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCheckCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                confirm = SyncSaleMaterial(false, isConfirm, listId, userId);
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            return confirm;
        }

        public string SyncSaleMaterial(bool isAll, bool isConfirm, List<string> listMaterialId, string userId)
        {
            SaleProduct saleProduct;
            SaleProduct check;
            SaleProductMedia saleProductMedia;
            List<SaleProductMedia> saleProductMedias;
            List<SaleProduct> listSaleProduct = new List<SaleProduct>();
            List<SaleProductMedia> listSaleProductMedia = new List<SaleProductMedia>();
            SyncSaleProduct syncSaleProduct;
            List<SyncSaleProduct> syncSaleProducts = new List<SyncSaleProduct>();
            Material material;
            List<MaterialImage> listImage;
            string groupCode = string.Empty;
            string conten = $"Đồng bộ sản phẩm kinh doanh";
            var listMaterialGroup = db.MaterialGroups.ToList();
            List<string> listCodeError = new List<string>();

            if (isAll)
            {
                listMaterialId = new List<string>();
                listMaterialId = db.Materials.AsNoTracking().Select(i => i.Id).ToList();
            }

            foreach (var id in listMaterialId)
            {
                material = new Material();
                material = db.Materials.FirstOrDefault(i => i.Id.Equals(id));
                if (material == null)
                {
                    continue;
                }
                material.IsSendSale = true;
                material.SyncDate = DateTime.Now;
                var day = GetConfigMaterialLastByDate();
                if (material.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(material.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        material.Pricing = material.PriceHistory;
                    }
                    else if (!material.InputPriceDate.HasValue || material.InputPriceDate.Value.Date < material.LastBuyDate.Value.Date)
                    {
                        material.Pricing = 0;
                    }
                }

                listImage = new List<MaterialImage>();
                listImage = db.MaterialImages.AsNoTracking().Where(i => i.MaterialId.Equals(material.Id)).ToList();

                groupCode = listMaterialGroup.FirstOrDefault(i => i.Id.Equals(material.MaterialGroupId))?.Code;

                // Kiểm tra thông tin vật tư, nếu thiếu thì cảnh báo
                if (!isConfirm && (string.IsNullOrEmpty(groupCode) ||
                    string.IsNullOrEmpty(material.ManufactureId) ||
                    string.IsNullOrEmpty(material.Note) ||
                    material.Pricing <= 0 || material.DeliveryDays <= 0))
                {
                    listCodeError.Add(material.Code);
                    continue;
                }

                check = new SaleProduct();
                check = db.SaleProducts.FirstOrDefault(i => i.Type == Constants.SaleProductMaterial && i.SourceId.Equals(material.Id));
                if (check != null)
                {
                    check.EName = string.Empty;
                    check.VName = material.Name;
                    check.Model = material.Code;
                    check.GroupCode = groupCode;
                    check.ManufactureId = material.ManufactureId;
                    //check.CountryName = Constants.CountryName;
                    check.Specifications = material.Note;
                    check.SpecificationDate = material.UpdateDate;
                    check.MaterialPrice = material.Pricing;
                    check.DeliveryTime = material.DeliveryDays.ToString();
                    check.SourceId = material.Id;
                    check.IsSync = true;
                    check.Status = true;
                    check.Type = Constants.SaleProductMaterial;
                    check.UpdateBy = userId;
                    check.UpdateDate = DateTime.Now;

                    if (listImage.Count > 0)
                    {
                        check.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProductMaterial,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    saleProductMedias = new List<SaleProductMedia>();
                    saleProductMedias = db.SaleProductMedias.Where(i => i.SaleProductId.Equals(check.Id) && i.Type == Constants.SaleProductMedia_Type_LibraryImage).ToList();

                    db.SaleProductMedias.RemoveRange(saleProductMedias);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = material.UpdateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }
                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncMaterial, check.Id, check.Model, conten);
                }
                else
                {
                    saleProduct = new SaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EName = string.Empty,
                        VName = material.Name,
                        Model = material.Code,
                        GroupCode = groupCode,
                        ManufactureId = material.ManufactureId,
                        //check.CountryName = Constants.CountryName,
                        Specifications = material.Note,
                        SpecificationDate = material.UpdateDate,
                        MaterialPrice = material.Pricing,
                        DeliveryTime = material.DeliveryDays.ToString(),
                        SourceId = material.Id,
                        IsSync = true,
                        Status = true,
                        Type = Constants.SaleProductMaterial,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        UpdateDate = DateTime.Now,
                        UpdateBy = userId,
                    };

                    if (listImage.Count > 0)
                    {
                        saleProduct.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    listSaleProduct.Add(saleProduct);

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProductMaterial,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.Path,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = material.UpdateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncMaterial, saleProduct.Id, saleProduct.Model, conten);
                }
            }

            if (listCodeError.Count > 0)
            {
                return "Mã vật tư <" + string.Join(", ", listCodeError) + "> đang thiếu thông tin đồng bộ. Bạn có muốn tiếp tục đồng bộ!";
            }

            using (var trans = db.Database.BeginTransaction())
            {
                db.SaleProducts.AddRange(listSaleProduct);
                db.SyncSaleProducts.AddRange(syncSaleProducts);
                db.SaleProductMedias.AddRange(listSaleProductMedia);
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }

            return null;
        }

        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        // Track whether Dispose has been called.
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        ~MaterialBusiness()
        {
            this.Dispose(false);
        }
    }
}
