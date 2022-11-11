using NTS.Common;
using NTS.Common.Excel;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Holiday;
using NTS.Model.ImportProfileDocumentConfigs;
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

namespace QLTK.Business.ImportProfiles
{
    public class ImportProfileReportBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public ImportProfileReportOngoingModel ReporOngoing(ImportProfileSearchModel searchModel)
        {
            searchModel.Step = null;
            var importProfiles = SearchImportProfile(searchModel, false, false);

            ImportProfileReportOngoingModel result = new ImportProfileReportOngoingModel();
            result.ImportProfile.Total = importProfiles.TotalItem;
            result.ImportProfile.ContractQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Contract);
            result.ImportProfile.CustomsQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Customs);
            result.ImportProfile.OngoingQuantity = importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing);
            result.ImportProfile.PayQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Payment);
            result.ImportProfile.ProductionQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Production);
            result.ImportProfile.SlowQuantity = importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow);
            result.ImportProfile.SupplierQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier);
            result.ImportProfile.TranportQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Transport);
            result.ImportProfile.WarehouseQuantity = importProfiles.ListResult.Count(r => r.Step == Constants.ImportProfile_Step_Import);

            result.BarChartData.Add(result.ImportProfile.SupplierQuantity);
            result.BarChartData.Add(result.ImportProfile.ContractQuantity);
            result.BarChartData.Add(result.ImportProfile.PayQuantity);
            result.BarChartData.Add(result.ImportProfile.ProductionQuantity);
            result.BarChartData.Add(result.ImportProfile.TranportQuantity);
            result.BarChartData.Add(result.ImportProfile.CustomsQuantity);
            result.BarChartData.Add(result.ImportProfile.WarehouseQuantity);

            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_ConfirmSupplier));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Contract));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Payment));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Production));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Transport));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Customs));
            result.BarChartDataLabels.Add(GetTyName(Constants.ImportProfile_Step_Import));

            result.PieChartData.Add(result.ImportProfile.SupplierQuantity);
            result.PieChartData.Add(result.ImportProfile.ContractQuantity);
            result.PieChartData.Add(result.ImportProfile.PayQuantity);
            result.PieChartData.Add(result.ImportProfile.ProductionQuantity);
            result.PieChartData.Add(result.ImportProfile.TranportQuantity);
            result.PieChartData.Add(result.ImportProfile.CustomsQuantity);
            result.PieChartData.Add(result.ImportProfile.WarehouseQuantity);
            result.ImportProfileEmployee = importProfiles.ListResult.GroupBy(g => new { g.EmployeeId, g.EmployeeName })
                                                        .Select(s => new ImportProfileReportOngoingQuntityModel
                                                        {
                                                            Name = s.Key.EmployeeName,
                                                            Id = s.Key.EmployeeId,
                                                            Total = s.Count(),
                                                            ContractQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Contract),
                                                            CustomsQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Customs),
                                                            OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                            PayQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Payment),
                                                            ProductionQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Production),
                                                            SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                            SupplierQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                                            TranportQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Transport),
                                                            WarehouseQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Import),
                                                            PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                            AmountVND = s.Sum(r => r.AmountVND),
                                                        }).ToList();

            result.ImportProfileSupplier = importProfiles.ListResult.GroupBy(g => new { g.SupplierId, g.SupplierCode })
                                                       .Select(s => new ImportProfileReportOngoingQuntityModel
                                                       {
                                                           Name = string.IsNullOrEmpty(s.Key.SupplierCode) ? "Chưa xác định" : s.Key.SupplierCode,
                                                           Id = s.Key.SupplierId,
                                                           Total = s.Count(),
                                                           ContractQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Contract),
                                                           CustomsQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Customs),
                                                           OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                           PayQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Payment),
                                                           ProductionQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Production),
                                                           SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                           SupplierQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                                           TranportQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Transport),
                                                           WarehouseQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Import),
                                                           PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                           AmountVND = s.Sum(r => r.AmountVND),
                                                       }).ToList();

            result.ImportProfileTransport = importProfiles.ListResult.Where(w => w.Step >= Constants.ImportProfile_Step_Transport).GroupBy(g => new { g.TransportSupplierName, g.TransportSupplierId })
                                                      .Select(s => new ImportProfileReportOngoingQuntityModel
                                                      {
                                                          Name = string.IsNullOrEmpty(s.Key.TransportSupplierName) ? "Chưa xác định" : s.Key.TransportSupplierName,
                                                          Total = s.Count(),
                                                          ContractQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Contract),
                                                          CustomsQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Customs),
                                                          OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                          PayQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Payment),
                                                          ProductionQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Production),
                                                          SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                          SupplierQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                                          TranportQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Transport),
                                                          WarehouseQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Import),
                                                          PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                          AmountVND = s.Sum(r => r.AmountVND),
                                                          CustomsInlandCosts = s.Sum(r => r.CustomsInlandCosts),
                                                          TransportationInternationalCosts = s.Sum(r => r.TransportationInternationalCosts)
                                                      }).ToList();

            result.ImportProfileCustoms = importProfiles.ListResult.Where(w => w.Step >= Constants.ImportProfile_Step_Customs).GroupBy(g => new { g.CustomsSupplierName, g.CustomsSupplierId })
                                                    .Select(s => new ImportProfileReportOngoingQuntityModel
                                                    {
                                                        Name = string.IsNullOrEmpty(s.Key.CustomsSupplierName) ? "Chưa xác định" : s.Key.CustomsSupplierName,
                                                        Total = s.Count(),
                                                        ContractQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Contract),
                                                        CustomsQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Customs),
                                                        OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                        PayQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Payment),
                                                        ProductionQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Production),
                                                        SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                        SupplierQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier),
                                                        TranportQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Transport),
                                                        WarehouseQuantity = s.Count(r => r.Step == Constants.ImportProfile_Step_Import),
                                                        PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                        AmountVND = s.Sum(r => r.AmountVND),
                                                        CustomsInlandCosts = s.Sum(r => r.CustomsInlandCosts),
                                                        TransportationInternationalCosts = s.Sum(r => r.TransportationInternationalCosts)
                                                    }).ToList();

            return result;
        }

        public string OngoingExportExcel(ImportProfileSearchModel searchModel)
        {
            var report = ReporOngoing(searchModel);

            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ImportProfile_Report_Ongoing.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];

                IRange iRangeData = sheet.FindFirst("<Total>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.Total;
                }

                iRangeData = sheet.FindFirst("<OngoingQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.OngoingQuantity;
                }

                iRangeData = sheet.FindFirst("<SlowQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.SlowQuantity;
                }

                iRangeData = sheet.FindFirst("<SupplierQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.SupplierQuantity;
                }

                iRangeData = sheet.FindFirst("<ContractQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.ContractQuantity;
                }

                iRangeData = sheet.FindFirst("<PayQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.PayQuantity;
                }

                iRangeData = sheet.FindFirst("<ProductionQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.ProductionQuantity;
                }

                iRangeData = sheet.FindFirst("<TranportQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.TranportQuantity;
                }

                iRangeData = sheet.FindFirst("<CustomsQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.CustomsQuantity;
                }

                iRangeData = sheet.FindFirst("<WarehouseQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.WarehouseQuantity;
                }

                iRangeData = sheet.FindFirst("<EmployeeData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<EmployeeData>", string.Empty);

                    var employeeExport = report.ImportProfileEmployee.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                        o.SupplierQuantity,
                        o.ContractQuantity,
                        o.PayQuantity,
                        o.ProductionQuantity,
                        o.TranportQuantity,
                        o.CustomsQuantity,
                        o.WarehouseQuantity
                    }).ToList();

                    if (employeeExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, employeeExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(employeeExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierData>", string.Empty);

                    var supplierExport = report.ImportProfileSupplier.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                        o.SupplierQuantity,
                        o.ContractQuantity,
                        o.PayQuantity,
                        o.ProductionQuantity,
                        o.TranportQuantity,
                        o.CustomsQuantity,
                        o.WarehouseQuantity
                    }).ToList();

                    if (supplierExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, supplierExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(supplierExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierTranportData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierTranportData>", string.Empty);

                    var transportExport = report.ImportProfileTransport.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                        o.TranportQuantity,
                        o.CustomsQuantity,
                        o.WarehouseQuantity
                    }).ToList();

                    if (transportExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, transportExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(transportExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierCustomsData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierCustomsData>", string.Empty);

                    var customsExport = report.ImportProfileCustoms.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                        o.CustomsQuantity,
                        o.WarehouseQuantity
                    }).ToList();

                    if (customsExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, customsExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(customsExport, iRangeData.Row, iRangeData.Column, false);
                }

                IRange iRangeDataTop = sheet.FindFirst("<ChartBar-Top>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataTop.Text = iRangeDataTop.Text.Replace("<ChartBar-Top>", string.Empty);
                IRange iRangeDataRight = sheet.FindFirst("<ChartBar-Right>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataRight.Text = iRangeDataRight.Text.Replace("<ChartBar-Right>", string.Empty);
                IRange iRangeDataBottom = sheet.FindFirst("<ChartBar-Bottom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataBottom.Text = iRangeDataBottom.Text.Replace("<ChartBar-Bottom>", string.Empty);

                ExportUtils.ExportToChartBar(sheet, iRangeDataTop.Row, iRangeDataTop.Column, iRangeDataRight.Column, iRangeDataBottom.Row, "Hồ sơ đang thực hiện", report.BarChartDataLabels.ToArray(), report.BarChartData.Cast<object>().ToArray());

                iRangeDataTop = sheet.FindFirst("<ChartPie-Top>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataTop.Text = iRangeDataTop.Text.Replace("<ChartPie-Top>", string.Empty);
                iRangeDataRight = sheet.FindFirst("<ChartPie-Right>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataRight.Text = iRangeDataRight.Text.Replace("<ChartPie-Right>", string.Empty);
                iRangeDataBottom = sheet.FindFirst("<ChartPie-Bottom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataBottom.Text = iRangeDataBottom.Text.Replace("<ChartPie-Bottom>", string.Empty);

                ExportUtils.ExportToChartPie(sheet, iRangeDataTop.Row, iRangeDataTop.Column, iRangeDataRight.Column, iRangeDataBottom.Row, report.BarChartDataLabels.ToArray(), report.PieChartData.Cast<object>().ToArray());

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo hồ sơ nhập khẩu đang thực hiện_" + time + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();
            }

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo hồ sơ nhập khẩu đang thực hiện_" + time + ".xlsx";

            return resultPathClient;
        }


        public ImportProfileReportSummaryModel ReporSummary(ImportProfileSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.TimeType))
            {
                searchModel.TimeType = "13";
            }


            if (!searchModel.TimeType.Equals(Constants.TimeType_Between))
            {
                DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
                SearchHelper.GetDateFromDateToByTimeType(searchModel.TimeType, searchModel.Year, searchModel.Month, searchModel.Quarter, ref dateFrom, ref dateTo);

                searchModel.CreateDateFrom = dateFrom;
                searchModel.CreateDateTo = dateTo;
            }
            else
            {
                if (searchModel.DateFrom.HasValue)
                {
                    searchModel.CreateDateFrom = searchModel.DateFrom.Value.ToStartDate();
                }

                if (searchModel.DateTo.HasValue)
                {
                    searchModel.CreateDateTo = searchModel.DateTo.Value.ToEndDate();
                }
            }

            //searchModel.Step = Constants.ImportProfile_Step_Finish;
            var importProfiles = SearchImportProfile(searchModel, false, true);

            ImportProfileReportSummaryModel result = new ImportProfileReportSummaryModel();
            result.ImportProfile.Total = importProfiles.TotalItem;
            result.ImportProfile.OngoingQuantity = importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing);
            result.ImportProfile.SlowQuantity = importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow);

            result.PieChartData.Add(result.ImportProfile.OngoingQuantity);
            result.PieChartData.Add(result.ImportProfile.SlowQuantity);

            result.ImportProfileEmployee = importProfiles.ListResult.GroupBy(g => new { g.EmployeeId, g.EmployeeName })
                                                        .Select(s => new ImportProfileReportSummaryQuntityModel
                                                        {
                                                            Name = s.Key.EmployeeName,
                                                            Id = s.Key.EmployeeId,
                                                            Total = s.Count(),
                                                            OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                            SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                            PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                            AmountVND = (s.Sum(r => r.AmountVND)),
                                                            ContractQuantity = s.Count(r => r.ContractFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            CustomsQuantity = s.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            PayQuantity = s.Count(r => r.PayFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            ProductionQuantity = s.Count(r => r.ProductionFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            SupplierQuantity = s.Count(r => r.SupplierFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            TranportQuantity = s.Count(r => r.TransportFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                            WarehouseQuantity = s.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                        }).ToList();

            result.ImportProfileSupplier = importProfiles.ListResult.GroupBy(g => new { g.SupplierId, g.SupplierCode })
                                                       .Select(s => new ImportProfileReportSummaryQuntityModel
                                                       {
                                                           Name = string.IsNullOrEmpty(s.Key.SupplierCode) ? "Chưa xác định" : s.Key.SupplierCode,
                                                           Id = s.Key.SupplierId,
                                                           Total = s.Count(),
                                                           OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                           SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                           PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                           AmountVND = (s.Sum(r => r.AmountVND)),
                                                           ContractQuantity = s.Count(r => r.ContractFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           CustomsQuantity = s.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           PayQuantity = s.Count(r => r.PayFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           ProductionQuantity = s.Count(r => r.ProductionFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           SupplierQuantity = s.Count(r => r.SupplierFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           TranportQuantity = s.Count(r => r.TransportFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                           WarehouseQuantity = s.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                       }).ToList();

            result.ImportProfileTransport = importProfiles.ListResult.Where(w => w.Step >= Constants.ImportProfile_Step_Transport).GroupBy(g => new { g.TransportSupplierId, g.TransportSupplierName })
                                                      .Select(s => new ImportProfileReportSummaryQuntityModel
                                                      {
                                                          Name = string.IsNullOrEmpty(s.Key.TransportSupplierName) ? "Chưa xác định" : s.Key.TransportSupplierName,
                                                          Total = s.Count(),
                                                          OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                          SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                          PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                          AmountVND = (s.Sum(r => r.AmountVND)),
                                                          CustomsInlandCosts = s.Sum(r => r.CustomsInlandCosts),
                                                          TransportationInternationalCosts = s.Sum(r => r.TransportationInternationalCosts),
                                                          TranportQuantity = s.Count(r => r.TransportFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                          CustomsQuantity = s.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                          WarehouseQuantity = s.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                      }).ToList();

            result.ImportProfileCustoms = importProfiles.ListResult.Where(w => w.Step >= Constants.ImportProfile_Step_Customs).GroupBy(g => new { g.CustomsSupplierId, g.CustomsSupplierName })
                                                    .Select(s => new ImportProfileReportSummaryQuntityModel
                                                    {
                                                        Name = string.IsNullOrEmpty(s.Key.CustomsSupplierName) ? "Chưa xác định" : s.Key.CustomsSupplierName,
                                                        Total = s.Count(),
                                                        OngoingQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing),
                                                        SlowQuantity = s.Count(r => r.Status == Constants.ImportProfile_Status_Slow),
                                                        PercentSlow = ((decimal)s.Count(r => r.Status == Constants.ImportProfile_Status_Slow) / (decimal)s.Count()) * 100,
                                                        AmountVND = (s.Sum(r => r.AmountVND)),
                                                        CustomsInlandCosts = s.Sum(r => r.CustomsInlandCosts),
                                                        TransportationInternationalCosts = s.Sum(r => r.TransportationInternationalCosts),
                                                        CustomsQuantity = s.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                        WarehouseQuantity = s.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow),
                                                    }).ToList();


            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_ConfirmSupplier));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Contract));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Payment));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Production));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Transport));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Customs));
            result.BarChartStepLabels.Add(GetTyName(Constants.ImportProfile_Step_Import));

            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.SupplierFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.ContractFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.PayFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.ProductionFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.TransportFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));
            result.BarChartStepData.Add(importProfiles.ListResult.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Ongoing));

            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.SupplierFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.ContractFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.PayFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.ProductionFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.TransportFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.CustomsFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));
            result.BarChartStepDataSlow.Add(importProfiles.ListResult.Count(r => r.WarehouseFinishStatus == Constants.ImportProfilePayment_StepStatus_Slow));

            switch (searchModel.TimeType)
            {
                case Constants.TimeType_Today:
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            result.BarChartLabels.Add(i.ToString());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Hour == i));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Hour == i));
                        }
                        break;
                    }
                case Constants.TimeType_Yesterday:
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            result.BarChartLabels.Add(i.ToString());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Hour == i));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Hour == i));
                        }
                        break;
                    }
                case Constants.TimeType_ThisWeek:
                    {
                        DateTime dateInWeek;
                        for (int i = 0; i < 7; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));

                        }
                        break;
                    }
                case Constants.TimeType_LastWeek:
                    {
                        DateTime dateInWeek;
                        for (int i = 0; i < 7; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));

                        }
                        break;
                    }
                case Constants.TimeType_SevenDay:
                    {
                        DateTime dateInWeek;
                        for (int i = 0; i < 7; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));

                        }
                        break;
                    }
                case Constants.TimeType_ThisMonth:
                    {
                        DateTime dateInWeek;
                        int day = DateTime.DaysInMonth(searchModel.CreateDateFrom.Value.Year, searchModel.CreateDateFrom.Value.Month);
                        for (int i = 0; i < day; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                        }
                        break;
                    }
                case Constants.TimeType_LastMonth:
                    {
                        DateTime dateInWeek;
                        int day = DateTime.DaysInMonth(searchModel.CreateDateFrom.Value.Year, searchModel.CreateDateFrom.Value.Month);
                        for (int i = 0; i < day; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                        }
                        break;
                    }
                case Constants.TimeType_Month:
                    {
                        DateTime dateInWeek;
                        int day = DateTime.DaysInMonth(searchModel.CreateDateFrom.Value.Year, searchModel.CreateDateFrom.Value.Month);
                        for (int i = 0; i < day; i++)
                        {
                            dateInWeek = searchModel.CreateDateFrom.Value.AddDays(i);
                            result.BarChartLabels.Add(dateInWeek.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate >= dateInWeek.ToStartDate() && r.CreateDate <= dateInWeek.ToEndDate()));
                        }
                        break;
                    }
                case Constants.TimeType_Quarter:
                    {
                        DateTime dateStartMonth;
                        for (int i = 0; i < 3; i++)
                        {
                            dateStartMonth = searchModel.CreateDateFrom.Value.AddMonths(i);
                            result.BarChartLabels.Add(dateStartMonth.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Month == dateStartMonth.Month));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Month == dateStartMonth.Month));
                        }
                        break;
                    }
                case Constants.TimeType_ThisYear:
                    {
                        DateTime dateStartMonth;
                        for (int i = 0; i < 12; i++)
                        {
                            dateStartMonth = searchModel.CreateDateFrom.Value.AddMonths(i);
                            result.BarChartLabels.Add(dateStartMonth.ToStringDDMMYY());
                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Month == dateStartMonth.Month));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Month == dateStartMonth.Month));
                        }
                        break;
                    }
                case Constants.TimeType_LastYear:
                    {
                        DateTime dateStartMonth;
                        for (int i = 0; i < 12; i++)
                        {
                            dateStartMonth = searchModel.CreateDateFrom.Value.AddMonths(i);
                            result.BarChartLabels.Add(dateStartMonth.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Month == dateStartMonth.Month));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Month == dateStartMonth.Month));
                        }
                        break;
                    }
                case Constants.TimeType_Year:
                    {
                        DateTime dateStartMonth;
                        for (int i = 0; i < 12; i++)
                        {
                            dateStartMonth = searchModel.CreateDateFrom.Value.AddMonths(i);
                            result.BarChartLabels.Add(dateStartMonth.ToStringDDMMYY());

                            result.BarChartData.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Ongoing && r.CreateDate.Month == dateStartMonth.Month));
                            result.BarChartDataSlow.Add(importProfiles.ListResult.Count(r => r.Status == Constants.ImportProfile_Status_Slow && r.CreateDate.Month == dateStartMonth.Month));
                        }
                        break;
                    }
                default:
                    break;
            }

            return result;
        }

        public string SummaryExportExcel(ImportProfileSearchModel searchModel)
        {
            var report = ReporSummary(searchModel);

            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ImportProfile_Report_Summary.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];

                IRange iRangeData = sheet.FindFirst("<Total>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.Total;
                }

                iRangeData = sheet.FindFirst("<OngoingQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.OngoingQuantity;
                }

                iRangeData = sheet.FindFirst("<SlowQuantity>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = report.ImportProfile.SlowQuantity;
                }

                iRangeData = sheet.FindFirst("<EmployeeData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<EmployeeData>", string.Empty);

                    var employeeExport = report.ImportProfileEmployee.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                    }).ToList();

                    if (employeeExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, employeeExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(employeeExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierData>", string.Empty);

                    var supplierExport = report.ImportProfileSupplier.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                    }).ToList();

                    if (supplierExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, supplierExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(supplierExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierTranportData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierTranportData>", string.Empty);

                    var transportExport = report.ImportProfileTransport.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                    }).ToList();

                    if (transportExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, transportExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(transportExport, iRangeData.Row, iRangeData.Column, false);
                }

                iRangeData = sheet.FindFirst("<SupplierCustomsData>", ExcelFindType.Text, ExcelFindOptions.MatchCase);

                //int rowStart =  
                if (iRangeData != null)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<SupplierCustomsData>", string.Empty);

                    var customsExport = report.ImportProfileCustoms.Select((o, i) => new
                    {
                        Index = i + 1,
                        o.Name,
                        o.Total,
                        o.OngoingQuantity,
                        o.SlowQuantity,
                    }).ToList();

                    if (customsExport.Count > 2)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, customsExport.Count - 2, ExcelInsertOptions.FormatAsBefore);
                    }

                    sheet.ImportData(customsExport, iRangeData.Row, iRangeData.Column, false);
                }

                IRange iRangeDataTop = sheet.FindFirst("<ChartBar-Top>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataTop.Text = iRangeDataTop.Text.Replace("<ChartBar-Top>", string.Empty);
                IRange iRangeDataRight = sheet.FindFirst("<ChartBar-Right>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataRight.Text = iRangeDataRight.Text.Replace("<ChartBar-Right>", string.Empty);
                IRange iRangeDataBottom = sheet.FindFirst("<ChartBar-Bottom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataBottom.Text = iRangeDataBottom.Text.Replace("<ChartBar-Bottom>", string.Empty);

                if (report.BarChartData.ToArray().Length != 0 && report.BarChartDataSlow.ToArray().Length != 0)
                {
                    ExportUtils.ExportToChartBar(sheet, iRangeDataTop.Row, iRangeDataTop.Column, iRangeDataRight.Column, iRangeDataBottom.Row, report.BarChartLabels.ToArray(), "Hồ sơ đúng tiến độ", report.BarChartData.Cast<object>().ToArray(), "Hồ sơ chậm tiến độ", report.BarChartDataSlow.Cast<object>().ToArray());
                }


                iRangeDataTop = sheet.FindFirst("<ChartPie-Top>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataTop.Text = iRangeDataTop.Text.Replace("<ChartPie-Top>", string.Empty);
                iRangeDataRight = sheet.FindFirst("<ChartPie-Right>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataRight.Text = iRangeDataRight.Text.Replace("<ChartPie-Right>", string.Empty);
                iRangeDataBottom = sheet.FindFirst("<ChartPie-Bottom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataBottom.Text = iRangeDataBottom.Text.Replace("<ChartPie-Bottom>", string.Empty);

                ExportUtils.ExportToChartPie(sheet, iRangeDataTop.Row, iRangeDataTop.Column, iRangeDataRight.Column, iRangeDataBottom.Row, new object[] { "Đúng tiến độ", "Chậm tiến độ" }, report.PieChartData.Cast<object>().ToArray());

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo hồ sơ nhập khẩu_" + time + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();
            }

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo hồ sơ nhập khẩu_" + time + ".xlsx";

            return resultPathClient;
        }

        private SearchResultModel<ImportProfileReportReusltModel> SearchImportProfile(ImportProfileSearchModel searchModel, bool isAll, bool isEnd)
        {
            SearchResultModel<ImportProfileReportReusltModel> searchResult = new SearchResultModel<ImportProfileReportReusltModel>();

            var importProfiles = (from d in db.ImportProfiles.AsNoTracking()
                                  join s in db.Suppliers.AsNoTracking() on d.SupplierId equals s.Id into ds
                                  from dsn in ds.DefaultIfEmpty()
                                  join t in db.Suppliers.AsNoTracking() on d.TransportSupplierId equals t.Id into dt
                                  from dtn in dt.DefaultIfEmpty()
                                  join c in db.Suppliers.AsNoTracking() on d.CustomsSupplierId equals c.Id into dc
                                  from dcn in dc.DefaultIfEmpty()
                                  join e in db.Employees.AsNoTracking() on d.EmployeeId equals e.Id
                                  select new ImportProfileReportReusltModel
                                  {
                                      Id = d.Id,
                                      Amount = d.Amount,
                                      AmountVND = d.CurrencyUnit == Constants.CurrencyUnit_VNĐ ? d.Amount : d.Amount * d.SupplierExchangeRate,
                                      Code = d.Code,
                                      DueDate = d.PRDueDate,
                                      EstimatedDeliveryDate = d.EstimatedDeliveryDate,
                                      Name = d.Name,
                                      PayDueDate = d.PayDueDate,
                                      PayStatus = d.PayStatus,
                                      PayIndex = d.PayIndex,
                                      Status = d.Step == Constants.ImportProfile_Step_Finish ? d.Status : !d.PRDueDate.HasValue || d.PRDueDate >= DateTime.Now ? 1 : 2,
                                      Step = d.Step,
                                      SupplierCode = dsn != null ? dsn.Name : string.Empty,
                                      SupplierId = d.SupplierId,
                                      EmployeeId = e.Id,
                                      EmployeeName = e.Name,
                                      TransportSupplierName = dtn != null ? dtn.Name : string.Empty,
                                      TransportSupplierId = d.TransportSupplierId,
                                      CustomsSupplierName = dcn != null ? dcn.Name : string.Empty,
                                      CustomsSupplierId = d.CustomsSupplierId,
                                      CreateDate = d.CreateDate,
                                      CustomsInlandCosts = d.CustomsInlandCosts,
                                      TransportationInternationalCostsUnit = d.TransportationInternationalCostsUnit,
                                      TransportationInternationalCosts = d.TransportationInternationalCostsUnit != 4 ? d.TransportationInternationalCosts * d.TransportExchangeRate : d.TransportationInternationalCosts,
                                      ContractFinishStatus = d.ContractFinishStatus,
                                      PayFinishStatus = d.PayFinishStatus,
                                      SupplierFinishStatus = d.SupplierFinishStatus,
                                      ProductionFinishStatus = d.ProductionFinishStatus,
                                      TransportFinishStatus = d.TransportFinishStatus,
                                      CustomsFinishStatus = d.CustomsFinishStatus,
                                      WarehouseFinishStatus = d.WarehouseFinishStatus
                                  }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                importProfiles = importProfiles.Where(r => r.Code.ToLower().Contains(searchModel.Code.ToLower()) || r.Name.ToLower().Contains(searchModel.Code.ToLower()));
            }

            if (searchModel.Step.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.Step == searchModel.Step.Value);
            }
            else
            {
                if (isEnd)
                {
                    importProfiles = importProfiles.Where(r => r.Step == Constants.ImportProfile_Step_Finish);
                }
                else
                {
                    importProfiles = importProfiles.Where(r => r.Step < Constants.ImportProfile_Step_Finish);
                }

            }

            if (searchModel.Status.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.Status == searchModel.Status.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectCode))
            {
                importProfiles = importProfiles.Where(r => r.ProjectCode.ToLower().Contains(searchModel.ProjectCode));
            }

            if (!string.IsNullOrEmpty(searchModel.SupplierId))
            {

                importProfiles = importProfiles.Where(r => r.SupplierId.ToLower().Equals(searchModel.SupplierId));
            }

            if (searchModel.DueDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.DueDate >= searchModel.DueDateFrom.Value);
            }

            if (searchModel.DueDateTo.HasValue)
            {
                var dueDateTo = searchModel.DueDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.DueDate <= dueDateTo);
            }

            if (searchModel.PayDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.PayDueDate >= searchModel.PayDateFrom.Value);
            }

            if (searchModel.PayDateTo.HasValue)
            {
                var payDateTo = searchModel.PayDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.PayDueDate <= payDateTo);
            }

            if (searchModel.DeliveryDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.EstimatedDeliveryDate >= searchModel.DeliveryDateFrom.Value);
            }

            if (searchModel.DeliveryDateTo.HasValue)
            {
                var deliveryDateTo = searchModel.DeliveryDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.EstimatedDeliveryDate <= deliveryDateTo);
            }

            if (searchModel.CreateDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.CreateDate >= searchModel.CreateDateFrom.Value);
            }

            if (searchModel.CreateDateTo.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.CreateDate <= searchModel.CreateDateTo.Value);
            }

            if (searchModel.PayStatus.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.PayStatus == searchModel.PayStatus.Value);
            }

            if (searchModel.WorkStatus.HasValue)
            {
                // Đã kết thúc
                if (searchModel.WorkStatus == 1)
                {
                    importProfiles = importProfiles.Where(r => r.Step > Constants.ImportProfile_Step_Finish);
                }
                // Đang thực hiện
                else
                {
                    importProfiles = importProfiles.Where(r => r.Step <= Constants.ImportProfile_Step_Finish);
                }
            }

            searchResult.TotalItem = importProfiles.Count();

            searchResult.ListResult = SQLHelpper.OrderBy(importProfiles, searchModel.OrderBy, searchModel.OrderType).ToList();

            return searchResult;
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
    }
}
