using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ImportProfileProblemExist;
using NTS.Model.ReportProblemExist;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ReportProblemExists
{
    public class ReportProblemExistBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public object GetListImportProfileProblemExist(ImportProfileProblemExistSearchModel model)
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            if (string.IsNullOrEmpty(model.TimeType))
            {
                model.TimeType = Constants.TimeType_Between;
            }

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year.Value, model.Month.Value, model.Quarter.Value, ref dateFrom, ref dateTo);
            }

            var result = (from a in db.ImportProfileProblemExists.AsNoTracking()
                          join b in db.ImportProfiles.AsNoTracking() on a.ImportProfileId equals b.Id
                          join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                          join d in db.Suppliers.AsNoTracking() on b.SupplierId equals d.Id into ad
                          from da in ad.DefaultIfEmpty()
                          join e in db.Employees.AsNoTracking() on c.EmployeeId equals e.Id
                          orderby a.Step
                          select new ImportProfileProblemExistModel
                          {
                              Id = a.Id,
                              ImportProfileId = a.ImportProfileId,
                              Note = a.Note,
                              Plan = a.Plan,
                              Step = a.Step,
                              Status = a.Status,
                              PRCode = b.PRCode,
                              ProjectCodeList = b.ProjectCode,
                              EmployeeId = c.EmployeeId,
                              EmployeeName = e.Name,
                              EmployeeCode = e.Code,
                              SupplierId = da != null ? da.Id : string.Empty,
                              SupplierName = da != null ? da.Name : string.Empty,
                              SupplierCode = da != null ? da.Code : string.Empty,
                              CreateDate = a.CreateDate.Value,
                              Code = b.Code,
                              CurrencyUnit = b.CurrencyUnit
                          }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProjectCode))
            {
                result = result.Where(i => i.ProjectCodeList.ToUpper().Contains(model.ProjectCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.PRCode))
            {
                result = result.Where(i => i.PRCode.ToUpper().Contains(model.PRCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.EmployeeId))
            {
                result = result.Where(i => i.EmployeeId.Equals(model.EmployeeId));
            }

            if (!string.IsNullOrEmpty(model.SupplierId))
            {
                result = result.Where(i => i.SupplierId.Equals(model.SupplierId));
            }

            if (model.Status.HasValue)
            {
                result = result.Where(i => i.Status == model.Status);
            }

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                result = result.Where(i => i.CreateDate >= dateFrom && i.CreateDate <= dateTo);
            }
            else
            {
                if (model.DateFrom.HasValue)
                {
                    result = result.Where(i => i.CreateDate >= model.DateFrom.Value);
                }

                if (model.DateTo.HasValue)
                {
                    model.DateTo = model.DateTo.Value.ToEndDate();
                    result = result.Where(i => i.CreateDate <= model.DateTo.Value);
                }
            }

            var listResult = result.OrderBy(i => i.Code).ThenBy(i => i.Step).ToList();

            ImportProfile importProfile;
            Dictionary<string, ImportProfile> profiles = new Dictionary<string, ImportProfile>();
            foreach (var item in listResult)
            {
                if (profiles.ContainsKey(item.ImportProfileId))
                {
                    importProfile = profiles[item.ImportProfileId];
                }
                else
                {
                    importProfile = new ImportProfile();
                    importProfile = db.ImportProfiles.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item.ImportProfileId));

                    profiles.Add(item.ImportProfileId, importProfile);
                }

                if (importProfile != null)
                {
                    if (item.Step < importProfile.Step)
                    {
                        item.LateDay = LateDateByStep(importProfile, item.Step);
                    }
                    else if (item.Step == importProfile.Step)
                    {
                        item.LateDay = LateDateByStepImport(importProfile, item.Step);
                    }

                    item.AmountVND = item.CurrencyUnit == Constants.CurrencyUnit_VNĐ ? importProfile.Amount : importProfile.Amount * importProfile.SupplierExchangeRate;
                }
            }

            var list = (from a in listResult
                        group a by new { a.ImportProfileId, a.EmployeeId, a.SupplierId, a.ProjectCodeList, a.AmountVND } into g
                        select new
                        {
                            g.Key.ImportProfileId,
                            g.Key.EmployeeId,
                            g.Key.SupplierId,
                            g.Key.ProjectCodeList,
                            g.Key.AmountVND
                        }).ToList();

            #region Theo nhân viên
            var listEmployee = (from a in listResult
                                orderby a.EmployeeCode
                                group a by new { a.EmployeeId, a.EmployeeName, a.EmployeeCode } into g
                                select new ReportProblemExistModel
                                {
                                    Id = g.Key.EmployeeId,
                                    Name = g.Key.EmployeeName,
                                    Code = g.Key.EmployeeCode,
                                    SupplierQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                    ContractQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Contract),
                                    PayQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Payment),
                                    ProductionQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Production),
                                    TranportQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Transport),
                                    CustomsQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Customs),
                                    WarehouseQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Import),
                                    AmountVND = list.Where(i => i.EmployeeId.Equals(g.Key.EmployeeId)).Sum(i => i.AmountVND),
                                }).ToList();

            foreach (var item in listEmployee)
            {
                item.Total = list.Count(i => i.EmployeeId.Equals(item.Id));
            }

            #endregion

            #region Theo nhà cung cấp
            var listSupplier = (from a in listResult
                                where !string.IsNullOrEmpty(a.SupplierId)
                                orderby a.SupplierCode
                                group a by new { a.SupplierId, a.SupplierName, a.SupplierCode } into g
                                select new ReportProblemExistModel
                                {
                                    Id = g.Key.SupplierId,
                                    Name = g.Key.SupplierName,
                                    Code = g.Key.SupplierCode,
                                    SupplierQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                    ContractQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Contract),
                                    PayQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Payment),
                                    ProductionQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Production),
                                    TranportQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Transport),
                                    CustomsQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Customs),
                                    WarehouseQuantity = g.Count(i => i.Step == Constants.ImportProfile_Step_Import),
                                    AmountVND = list.Where(i => i.SupplierId.Equals(g.Key.SupplierId)).Sum(i => i.AmountVND),
                                }).ToList();

            foreach (var item in listSupplier)
            {
                item.Total = list.Count(i => i.SupplierId.Equals(item.Id));
            }
            #endregion

            #region Theo mã dự án
            string[] listProjectCode;
            List<string> listProject = new List<string>();
            foreach (var item in listResult)
            {
                if (!string.IsNullOrEmpty(item.ProjectCodeList))
                {
                    listProjectCode = item.ProjectCodeList.Split(',');
                    listProject.AddRange(listProjectCode);
                }
            }

            var listGroup = listProject.GroupBy(i => i).Select(i => i.Key).OrderBy(i => i).ToList();
            var listProduct = db.PurchaseRequestProducts.ToList();
            List<ReportProblemExistModel> listReportProjectCode = new List<ReportProblemExistModel>();
            ReportProblemExistModel reportProjectCode;
            foreach (var item in listGroup)
            {
                reportProjectCode = new ReportProblemExistModel
                {
                    Code = item,
                    Name = listProduct.FirstOrDefault(i => i.ProjectCode.Equals(item))?.ProjectName,
                    SupplierQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_ConfirmSupplier && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    ContractQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Contract && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    PayQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Payment && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    ProductionQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Production && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    TranportQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Transport && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    CustomsQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Customs && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    WarehouseQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Import && !string.IsNullOrEmpty(i.ProjectCodeList) && i.ProjectCodeList.Contains(item)),
                    AmountVND = list.Where(i => i.ProjectCodeList.Contains(item)).Sum(i => i.AmountVND),
                };
                listReportProjectCode.Add(reportProjectCode);
            }

            foreach (var item in listReportProjectCode)
            {
                item.Total = list.Count(i => i.ProjectCodeList.Contains(item.Code));
            }
            #endregion

            #region Tổng hợp
            var supplierQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_ConfirmSupplier);
            var contractQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Contract);
            var payQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Payment);
            var productionQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Production);
            var tranportQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Transport);
            var customsQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Customs);
            var warehouseQuantity = listResult.Count(i => i.Step == Constants.ImportProfile_Step_Import);
            #endregion

            var totalItem = listResult.Count();
            var totalProcessed = listResult.Count(i => i.Status == Constants.ReportProblem_Status_Processed);
            var totalNoProcessed = listResult.Count(i => i.Status == Constants.ReportProblem_Status_NoProcessed);
            return new
            {
                totalItem,
                totalProcessed,
                totalNoProcessed,
                supplierQuantity,
                contractQuantity,
                payQuantity,
                productionQuantity,
                tranportQuantity,
                customsQuantity,
                warehouseQuantity,
                listEmployee,
                listSupplier,
                listReportProjectCode,
                listResult
            };
        }

        public string ExcelReportProblemExist(ReportProblemExistExcelModel model)
        {
            ImportProfileProblemExist importProfile;
            foreach (var item in model.ListResult)
            {
                importProfile = new ImportProfileProblemExist();
                importProfile = db.ImportProfileProblemExists.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item.Id));
                if (importProfile != null)
                {
                    importProfile.Plan = item.Plan;
                }
            }
            db.SaveChanges();

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportProblemExist.xlsx"));
            IWorksheet sheet = workbook.Worksheets[0];

            // Tổng hợp
            IRange iRange1 = sheet.FindFirst("<supplierQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange1.Number = model.SupplierQuantity;
            IRange iRange2 = sheet.FindFirst("<contractQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange2.Number = model.ContractQuantity;
            IRange iRange3 = sheet.FindFirst("<payQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange3.Number = model.PayQuantity;
            IRange iRange4 = sheet.FindFirst("<productionQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange4.Number = model.ProductionQuantity;
            IRange iRange5 = sheet.FindFirst("<tranportQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange5.Number = model.TranportQuantity;
            IRange iRange6 = sheet.FindFirst("<customs>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange6.Number = model.CustomsQuantity;
            IRange iRange7 = sheet.FindFirst("<import>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange7.Number = model.WarehouseQuantity;

            // ImportData nhân viên
            IRange iRangeDataEmployee = sheet.FindFirst("<Em>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataEmployee.Text = iRangeDataEmployee.Text.Replace("<Em>", string.Empty);
            var listEmployee = model.ListEmployee.OrderBy(i => i.Name).Select((a, i) => new
            {
                Index = i + 1,
                a.Name,
                a.Total,
                a.SupplierQuantity,
                a.ContractQuantity,
                a.PayQuantity,
                a.ProductionQuantity,
                a.TranportQuantity,
                a.CustomsQuantity,
                a.WarehouseQuantity
            });

            if (listEmployee.Count() > 1)
            {
                sheet.InsertRow(iRangeDataEmployee.Row + 1, listEmployee.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listEmployee, iRangeDataEmployee.Row, iRangeDataEmployee.Column, false);

            // ImportData nhân viên
            IRange iRangeDataSupplier = sheet.FindFirst("<Su>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataSupplier.Text = iRangeDataSupplier.Text.Replace("<Su>", string.Empty);
            var listSupplier = model.ListSupplier.OrderBy(i => i.Name).Select((a, i) => new
            {
                Index = i + 1,
                a.Name,
                a.Total,
                a.SupplierQuantity,
                a.ContractQuantity,
                a.PayQuantity,
                a.ProductionQuantity,
                a.TranportQuantity,
                a.CustomsQuantity,
                a.WarehouseQuantity
            });

            if (listSupplier.Count() > 1)
            {
                sheet.InsertRow(iRangeDataSupplier.Row + 1, listSupplier.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listSupplier, iRangeDataSupplier.Row, iRangeDataSupplier.Column, false);

            // ImportData nhân viên
            IRange iRangeDataProject = sheet.FindFirst("<Pr>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataProject.Text = iRangeDataProject.Text.Replace("<Pr>", string.Empty);
            var listProject = model.ListReportProjectCode.OrderBy(i => i.Name).Select((a, i) => new
            {
                Index = i + 1,
                a.Name,
                a.Total,
                a.SupplierQuantity,
                a.ContractQuantity,
                a.PayQuantity,
                a.ProductionQuantity,
                a.TranportQuantity,
                a.CustomsQuantity,
                a.WarehouseQuantity
            });

            if (listProject.Count() > 1)
            {
                sheet.InsertRow(iRangeDataProject.Row + 1, listProject.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listProject, iRangeDataProject.Row, iRangeDataProject.Column, false);

            // ImportData vấn đề tồn đọng
            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var total = model.ListResult.Count;
            var listExport = model.ListResult.OrderBy(i => i.Step).Select((a, i) => new
            {
                Index = i + 1,
                a.Note,
                a.Plan,
                a.Code,
                a.SupplierName,
                Step = GetTyName(a.Step),
                a.LateDay,
                Status = a.Status == 1 ? "Đã xử lý" : "Chưa xử lý",
                a.EmployeeName
            });

            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo vấn đề tồn đọng" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPath = "Template/" + Constants.FolderExport + "Báo cáo vấn đề tồn đọng" + ".xlsx";

            return resultPath;
        }

        private string GetTyName(int type)
        {
            switch (type)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    return "Xác định nhà cung cấp";
                case Constants.ImportProfile_Step_Contract:
                    return "Làm hợp đồng";
                case Constants.ImportProfile_Step_Customs:
                    return "Thủ tục thông quan";
                case Constants.ImportProfile_Step_Import:
                    return "Nhập kho";
                case Constants.ImportProfile_Step_Payment:
                    return "Thanh toán";
                case Constants.ImportProfile_Step_Production:
                    return "Theo dõi tiến độ sản xuất";
                case Constants.ImportProfile_Step_Transport:
                    return "Lựa chọn nhà vận chuyển";
                default:
                    return string.Empty;
            }
        }

        private int LateDateByStep(ImportProfile importProfile, int step)
        {
            int day = 0;
            switch (step)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    day = importProfile.SupplierExpectedDate.Value.Subtract(importProfile.SupplierFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Contract:
                    day = importProfile.ContractExpectedDate.Value.Subtract(importProfile.ContractFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Customs:
                    day = importProfile.CustomExpectedDate.Value.Subtract(importProfile.CustomsFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Import:
                    day = importProfile.WarehouseExpectedDate.Value.Subtract(importProfile.WarehouseFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Payment:
                    day = importProfile.PayExpectedDate.Value.Subtract(importProfile.PayFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Production:
                    day = importProfile.ProductionExpectedDate.Value.Subtract(importProfile.ProductionFinishDate.Value).Days;
                    break;
                case Constants.ImportProfile_Step_Transport:
                    day = importProfile.TransportExpectedDate.Value.Subtract(importProfile.TransportFinishDate.Value).Days;
                    break;
                default:
                    day = 0;
                    break;
            }

            if (day < 0)
            {
                day = 0;
            }

            return day;
        }

        private int LateDateByStepImport(ImportProfile importProfile, int step)
        {
            int day = 0;
            switch (step)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    if (importProfile.SupplierFinishDate.HasValue)
                    {
                        day = importProfile.SupplierExpectedDate.Value.Subtract(importProfile.SupplierFinishDate.Value).Days;
                    }
                    else if (importProfile.SupplierExpectedDate.HasValue)
                    {
                        day = importProfile.SupplierExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Contract:
                    if (importProfile.ContractFinishDate.HasValue)
                    {
                        day = importProfile.ContractExpectedDate.Value.Subtract(importProfile.ContractFinishDate.Value).Days;
                    }
                    else if (importProfile.ContractExpectedDate.HasValue)
                    {
                        day = importProfile.ContractExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Customs:
                    if (importProfile.CustomsFinishDate.HasValue)
                    {
                        day = importProfile.CustomExpectedDate.Value.Subtract(importProfile.CustomsFinishDate.Value).Days;
                    }
                    else if (importProfile.CustomExpectedDate.HasValue)
                    {
                        day = importProfile.CustomExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Import:
                    if (importProfile.WarehouseFinishDate.HasValue)
                    {
                        day = importProfile.WarehouseExpectedDate.Value.Subtract(importProfile.WarehouseFinishDate.Value).Days;
                    }
                    else if (importProfile.WarehouseExpectedDate.HasValue)
                    {
                        day = importProfile.WarehouseExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Payment:
                    if (importProfile.PayFinishDate.HasValue)
                    {
                        day = importProfile.PayExpectedDate.Value.Subtract(importProfile.PayFinishDate.Value).Days;
                    }
                    else if (importProfile.PayExpectedDate.HasValue)
                    {
                        day = importProfile.PayExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Production:
                    if (importProfile.ProductionFinishDate.HasValue)
                    {
                        day = importProfile.ProductionExpectedDate.Value.Subtract(importProfile.ProductionFinishDate.Value).Days;
                    }
                    else if (importProfile.ProductionExpectedDate.HasValue)
                    {
                        day = importProfile.ProductionExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                case Constants.ImportProfile_Step_Transport:
                    if (importProfile.TransportFinishDate.HasValue)
                    {
                        day = importProfile.TransportExpectedDate.Value.Subtract(importProfile.TransportFinishDate.Value).Days;
                    }
                    else if (importProfile.TransportExpectedDate.HasValue)
                    {
                        day = importProfile.TransportExpectedDate.Value.Subtract(DateTime.Now).Days;
                    }
                    break;
                default:
                    break;
            }

            if (day < 0)
            {
                day = 0;
            }

            return day;
        }
    }
}
