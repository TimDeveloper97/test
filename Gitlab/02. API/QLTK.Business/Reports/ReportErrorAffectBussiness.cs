using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Report;
using NTS.Model.ReportStatusModule;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ReportStatusModule
{
    public class ReportErrorAffectBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public ReportErrorResultModel Report(ReportErrorSearchModel searchModel)
        {
            ReportErrorResultModel result = new ReportErrorResultModel();

            DateTime dateNow = DateTime.Now.Date;
            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join m in db.Departments.AsNoTracking() on b.DepartmentId equals m.Id into bm
                             from bmn in bm.DefaultIfEmpty()
                             join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                             join d in db.Employees.AsNoTracking() on c.EmployeeFixId equals d.Id into cd
                             from cdn in cd.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on c.DepartmentId equals e.Id into ce
                             from cen in ce.DefaultIfEmpty()
                             where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && a.Status >= Constants.Problem_Status_Processed
                             && c.Status != Constants.ErrorFix_Status_Finish
                             select new ReportErrorInfoModel
                             {
                                 Id = a.Id,
                                 PlanFinishDate = a.PlanFinishDate,
                                 PlanStartDate = a.PlanStartDate,
                                 CreateDate = a.CreateDate,
                                 StageId = a.StageId,
                                 AffectId = a.AffectId,
                                 ProjectId = a.ProjectId,
                                 Status = a.Status,
                                 FixStatus = c.Status,
                                 Deadline = c.DateTo.HasValue && dateNow > c.DateTo && c.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                                 ProjectStatus = b.Status,
                                 EmployeeName = cdn != null ? cdn.Name : string.Empty,
                                 EmployeeFixId = c.EmployeeFixId,
                                 FixDepartmentId = c.DepartmentId,
                                 FixDepartmentName = cen != null ? cen.Name : string.Empty,
                                 SBUManageId = bmn != null ? bmn.SBUId : string.Empty,
                                 DepartmentManageId = b.DepartmentId,
                                 DepartmentManageName = bmn != null ? bmn.Name : string.Empty,
                                 ProjectAmount = b.SaleNoVAT,
                                 DateFrom = c.DateFrom,
                                 CustomerFinalId = b.CustomerFinalId,
                                 CustomerId = b.CustomerId,
                                 DepartmentId = a.DepartmentId
                             }).AsQueryable();

            var dataQueryNotPlan = (from a in db.Errors.AsNoTracking()
                                    join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                                    join m in db.Departments.AsNoTracking() on b.DepartmentId equals m.Id into bm
                                    from bmn in bm.DefaultIfEmpty()
                                        //join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId into ac
                                        //from acn in ac.DefaultIfEmpty()
                                    where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && a.Status < Constants.Problem_Status_Processed
                                    select new ReportErrorInfoModel
                                    {
                                        Id = a.Id,
                                        PlanFinishDate = a.PlanFinishDate,
                                        PlanStartDate = a.PlanStartDate,
                                        CreateDate = a.CreateDate,
                                        StageId = a.StageId,
                                        ProjectId = a.ProjectId,
                                        Status = a.Status,
                                        ProjectStatus = b.Status,
                                        SBUManageId = bmn != null ? bmn.SBUId : string.Empty,
                                        DepartmentManageId = b.DepartmentId,
                                        DepartmentManageName = bmn != null ? bmn.Name : string.Empty,
                                        ProjectAmount = b.SaleNoVAT,
                                        CustomerFinalId = b.CustomerFinalId,
                                        CustomerId = b.CustomerId,
                                        DepartmentId = a.DepartmentId
                                    }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                dataQuery = dataQuery.Where(u => u.ProjectId.Equals(searchModel.ProjectId));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => u.ProjectId.Equals(searchModel.ProjectId));
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectStatus))
            {
                dataQuery = dataQuery.Where(u => u.ProjectStatus.Equals(searchModel.ProjectStatus));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => u.ProjectStatus.Equals(searchModel.ProjectStatus));
            }

            if (!string.IsNullOrEmpty(searchModel.FixDepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.FixDepartmentId.Equals(u.FixDepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.EmployeeFixId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentManageId.Equals(u.DepartmentManageId));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => searchModel.DepartmentManageId.Equals(u.DepartmentManageId));
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate >= searchModel.DateFrom);
                dataQueryNotPlan = dataQueryNotPlan.Where(r => r.PlanStartDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate <= searchModel.DateTo);
                dataQueryNotPlan = dataQueryNotPlan.Where(r => r.PlanStartDate <= searchModel.DateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerFinalId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerFinalId.Equals(u.CustomerFinalId));
                dataQueryNotPlan = dataQueryNotPlan.Where(u => searchModel.CustomerFinalId.Equals(u.CustomerFinalId));
            }

            var errors = dataQuery.ToList();
            var errorNoPlans = dataQueryNotPlan.ToList();

            var errorAffects = errors.Where(r => r.AffectId.HasValue).GroupBy(g => g.AffectId).Select(s => s.Key).ToList();

            var affects = (from a in db.ErrorAffects.AsNoTracking()
                           where errorAffects.Contains(a.Id)
                           orderby a.Id
                           select new { a.Id, a.Name }).ToList();

            result.Stages = affects.Select(s => s.Name).ToList();

            var errorDeparments = errors.Where(r => !string.IsNullOrEmpty(r.FixDepartmentId)).GroupBy(g => new { g.FixDepartmentId, g.FixDepartmentName }).Select(s => new { s.Key.FixDepartmentName, s.Key.FixDepartmentId }).OrderBy(o => o.FixDepartmentName).ToList();
            var errorEmployees = errors.Where(r => !string.IsNullOrEmpty(r.EmployeeFixId)).GroupBy(g => new { g.EmployeeFixId, g.EmployeeName }).Select(s => new { s.Key.EmployeeName, s.Key.EmployeeFixId }).ToList();

            ReportErrorResultObjectModel objectModel;
            ReportErrorResultSumModel valueModel;
            foreach (var department in errorDeparments)
            {
                objectModel = new ReportErrorResultObjectModel()
                {
                    Id = department.FixDepartmentId,
                    Name = !string.IsNullOrEmpty(department.FixDepartmentName) ? department.FixDepartmentName : "Chưa có bộ phận khắc phục"
                };

                foreach (var affect in affects)
                {
                    var errorByAffects = errors.Where(r => affect.Id.Equals(r.AffectId) && department.FixDepartmentId.Equals(r.FixDepartmentId)).ToList();
                    valueModel = new ReportErrorResultSumModel()
                    {
                        Id = affect.Id.ToString(),
                        ErrorQuantity = errorByAffects.GroupBy(g => g.Id).Count(),
                        Deadline = errorByAffects.Select(s => s.Deadline).DefaultIfEmpty(0).Max(),
                        WorkQuantity = errorByAffects.Count,
                        WorkDelay = errorByAffects.Count(c => c.Deadline > 0),
                        WorkToDo = errorByAffects.Count(c => c.FixStatus != Constants.ErrorFix_Status_Finish && c.DateFrom <= dateNow)
                    };
                    objectModel.TotalWorkDelay = objectModel.TotalWorkDelay + valueModel.WorkDelay;
                    objectModel.Values.Add(valueModel);
                }

                var errorByDeparments = errors.Where(r => department.FixDepartmentId.Equals(r.FixDepartmentId)).ToList();
                objectModel.ErrorQuantity = errorByDeparments.GroupBy(g => g.Id).Count();
                objectModel.Deadline = errorByDeparments.Select(s => s.Deadline).DefaultIfEmpty(0).Max();
                objectModel.WorkQuantity = errorByDeparments.Count;
                objectModel.WorkDelay = errorByDeparments.Count(c => c.Deadline > 0);
                objectModel.TotalWorkToDo = objectModel.Values.Sum(s => s.WorkToDo);

                result.ErrorFixs.Add(objectModel);
                result.Departments.Add(objectModel.Name);
            }


            foreach (var employee in errorEmployees)
            {
                objectModel = new ReportErrorResultObjectModel()
                {
                    Id = employee.EmployeeFixId,
                    Name = !string.IsNullOrEmpty(employee.EmployeeName) ? employee.EmployeeName : "Chưa có người khắc phục"
                };

                foreach (var department in errorDeparments)
                {
                    var errorByEmployees = errors.Where(r => department.FixDepartmentId.Equals(r.FixDepartmentId) && employee.EmployeeFixId.Equals(r.EmployeeFixId)).ToList();
                    valueModel = new ReportErrorResultSumModel()
                    {
                        Id = department.FixDepartmentId,
                        ErrorQuantity = errorByEmployees.GroupBy(g => g.Id).Count(),
                        Deadline = errorByEmployees.Select(s => s.Deadline).DefaultIfEmpty(0).Max(),
                        WorkQuantity = errorByEmployees.Count,
                        WorkDelay = errorByEmployees.Count(c => c.Deadline > 0),
                        WorkToDo = errorByEmployees.Count(c => c.FixStatus != Constants.ErrorFix_Status_Finish && c.DateFrom <= dateNow)
                    };

                    objectModel.Values.Add(valueModel);
                }

                var errorEmplyeess = errors.Where(r => employee.EmployeeFixId.Equals(r.EmployeeFixId)).ToList();
                objectModel.ErrorQuantity = errorEmplyeess.GroupBy(g => g.Id).Count();
                objectModel.Deadline = errorEmplyeess.Select(s => s.Deadline).DefaultIfEmpty(0).Max();
                objectModel.WorkQuantity = errorEmplyeess.Count;
                objectModel.WorkDelay = errorEmplyeess.Count(c => c.Deadline > 0);

                result.ErrorFixBys.Add(objectModel);
            }

            result.ErrorFixBys = result.ErrorFixBys.OrderByDescending(r => r.WorkDelay).ToList();
            result.ErrorFixs = result.ErrorFixs.OrderByDescending(r => r.WorkDelay).ToList();

            errors.AddRange(errorNoPlans);
            var departmentManages = errors.GroupBy(g => new { g.DepartmentManageId, g.DepartmentManageName, g.SBUManageId }).ToList();

            ReportErrorResultDepartmentManageModel project;
            foreach (var item in departmentManages)
            {
                var departmentBys = errors.Where(r => r.DepartmentManageId.Equals(item.Key.DepartmentManageId)).ToList();
                var projects = departmentBys.GroupBy(g => new { g.ProjectId, g.ProjectAmount }).ToList();

                project = new ReportErrorResultDepartmentManageModel()
                {
                    ErrorDelay = departmentBys.Where(r => r.Status >= Constants.Problem_Status_Processed && r.Deadline > 0 && r.FixStatus != Constants.ErrorFix_Status_Finish).GroupBy(g => g.Id).Count(),
                    ErrorQuantity = departmentBys.Where(r => r.Status >= Constants.Problem_Status_Processed && r.FixStatus != Constants.ErrorFix_Status_Finish).GroupBy(g => g.Id).Count(),
                    Name = item.Key.DepartmentManageName,
                    Id = item.Key.DepartmentManageId,
                    SBUId = item.Key.SBUManageId,
                    ProjectAmount = projects.Sum(s => s.Key.ProjectAmount),
                    ProjectQuantity = projects.Count
                };

                var noPlans = departmentBys.Where(r => r.Status < Constants.Problem_Status_Processed).GroupBy(g => new { g.Id, g.CreateDate });

                DateTime dateThree = dateNow.AddDays(-3);
                DateTime dateSeven = dateNow.AddDays(-7);
                project.ErrorNoPlan1 = noPlans.Count(r => r.Key.CreateDate >= dateThree && r.Key.CreateDate < dateNow);
                project.ErrorNoPlan2 = noPlans.Count(r => r.Key.CreateDate >= dateSeven && r.Key.CreateDate < dateThree);
                project.ErrorNoPlan3 = noPlans.Count(r => r.Key.CreateDate < dateSeven);

                result.ErrorProjects.Add(project);
            }

            result.ErrorProjects = result.ErrorProjects.OrderByDescending(r => r.ErrorQuantity).ThenByDescending(r => r.ProjectAmount).ToList();
            return result;
        }


        public string ExportExcel(ReportErrorSearchModel searchModel)
        {
            //var data = Report(searchModel);
            //List<ReportStatusModuleModel> listModel = data.ListModule;

            //if (listModel.Count == 0)
            //{
            //    throw NTSException.CreateInstance("Không có dữ liệu!");
            //}
            //try
            //{
            //    ExcelEngine excelEngine = new ExcelEngine();

            //    IApplication application = excelEngine.Excel;

            //    IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusModule.xlsx"));

            //    IWorksheet sheet = workbook.Worksheets[0];

            //    var total = listModel.Count;

            //    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            //    var listExport = listModel.Select((a, i) => new
            //    {
            //        Index = i + 1,
            //        a.ModuleName,
            //        a.ModuleCode,
            //        a.TotalModule,
            //        a.TotalModuleInProject,
            //        ViewStatus = a.Status == 1 ? "Chỉ sử dụng một lần" : a.Status == 2 ? "Module chuẩn" : a.Status == 3 ? "Module ngừng sử dụng" : "",
            //    });


            //    if (listExport.Count() > 1)
            //    {
            //        sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            //    }
            //    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
            //    //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;


            //    string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls");
            //    workbook.SaveAs(pathExport);
            //    workbook.Close();
            //    excelEngine.Dispose();

            //    //Đường dẫn file lưu trong web client
            //    string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls";

            //    return resultPathClient;
            //}
            //catch (Exception ex)
            //{
            //    //Log.LogError(ex);
            //    throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            //}

            return string.Empty;
        }
    }
}
