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
    public class TiviReportErrorBussiness
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

            if (searchModel.DateFrom != null)
            {
                dataQuery = dataQuery.Where(r => r.CreateDate >= searchModel.DateFrom);
                dataQueryNotPlan = dataQueryNotPlan.Where(r => r.CreateDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo != null)
            {
                dataQuery = dataQuery.Where(r => r.CreateDate <= searchModel.DateTo);
                dataQueryNotPlan = dataQueryNotPlan.Where(r => r.CreateDate <= searchModel.DateTo);
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

            var errorStages = errors.Where(r => !string.IsNullOrEmpty(r.StageId)).GroupBy(g => g.StageId).Select(s => s.Key).ToList();

            var stages = (from a in db.Stages.AsNoTracking()
                          where errorStages.Contains(a.Id)
                          orderby a.Name
                          select new { a.Id, a.Name }).ToList();

            result.Stages = stages.Select(s => s.Name).ToList();

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

                foreach (var stage in stages)
                {
                    var errorByStages = errors.Where(r => stage.Id.Equals(r.StageId) && department.FixDepartmentId.Equals(r.FixDepartmentId)).ToList();
                    valueModel = new ReportErrorResultSumModel()
                    {
                        Id = stage.Id,
                        ErrorQuantity = errorByStages.GroupBy(g => g.Id).Count(),
                        Deadline = errorByStages.Select(s => s.Deadline).DefaultIfEmpty(0).Max(),
                        WorkQuantity = errorByStages.Count,
                        WorkDelay = errorByStages.Count(c => c.Deadline > 0),
                        WorkToDo = errorByStages.Count(c => c.FixStatus != Constants.ErrorFix_Status_Finish && c.DateFrom <= dateNow),
                        Name = stage.Name
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
                    var errorByStages = errors.Where(r => department.FixDepartmentId.Equals(r.FixDepartmentId) && employee.EmployeeFixId.Equals(r.EmployeeFixId)).ToList();
                    valueModel = new ReportErrorResultSumModel()
                    {
                        Id = department.FixDepartmentId,
                        ErrorQuantity = errorByStages.GroupBy(g => g.Id).Count(),
                        Deadline = errorByStages.Select(s => s.Deadline).DefaultIfEmpty(0).Max(),
                        WorkQuantity = errorByStages.Count,
                        WorkDelay = errorByStages.Count(c => c.Deadline > 0),
                        WorkToDo = errorByStages.Count(c => c.FixStatus != Constants.ErrorFix_Status_Finish && c.DateFrom <= dateNow)
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
            DateTime dateThree = dateNow.AddDays(-3);
            DateTime dateSeven = dateNow.AddDays(-7);
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
                             
                project.ErrorNoPlan1 = noPlans.Count(r => r.Key.CreateDate >= dateThree && r.Key.CreateDate < dateNow);
                project.ErrorNoPlan2 = noPlans.Count(r => r.Key.CreateDate >= dateSeven && r.Key.CreateDate < dateThree);
                project.ErrorNoPlan3 = noPlans.Count(r => r.Key.CreateDate < dateSeven);

                result.ErrorProjects.Add(project);
            }

            result.ErrorProjects = result.ErrorProjects.OrderByDescending(r => r.ErrorQuantity).ThenByDescending(r => r.ProjectAmount).ToList();
            return result;
        }

        public SearchResultModel<ReportErrorWorkModel> GetWork(ReportErrorSearchModel searchModel)
        {
            DateTime dateNow = DateTime.Now.Date;
            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                             join d in db.Modules.AsNoTracking() on a.ObjectId equals d.Id into ad
                             from adn in ad.DefaultIfEmpty()
                             join e in db.Products.AsNoTracking() on a.ObjectId equals e.Id into ae
                             from aen in ae.DefaultIfEmpty()
                             join f in db.Employees.AsNoTracking() on c.EmployeeFixId equals f.Id into af
                             from afn in af.DefaultIfEmpty()
                             join g in db.Employees.AsNoTracking() on c.SupportId equals g.Id into ag
                             from agn in ag.DefaultIfEmpty()
                             join h in db.Employees.AsNoTracking() on c.ApproveId equals h.Id into ah
                             from ahn in ah.DefaultIfEmpty()
                             join k in db.Employees.AsNoTracking() on c.AdviseId equals k.Id into ak
                             from akn in ak.DefaultIfEmpty()
                             join l in db.Employees.AsNoTracking() on c.NotifyId equals l.Id into al
                             from aln in al.DefaultIfEmpty()
                             join m in db.Departments.AsNoTracking() on c.DepartmentId equals m.Id into cm
                             from cmn in cm.DefaultIfEmpty()
                             join s in db.Stages.AsNoTracking() on a.StageId equals s.Id into ast
                             from astn in ast.DefaultIfEmpty()
                             where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && c.Status != Constants.ErrorFix_Status_Finish
                             orderby c.DateFrom
                             select new ReportErrorWorkModel
                             {
                                 Id = a.Id,
                                 PlanFinishDate = a.PlanFinishDate,
                                 PlanStartDate = a.PlanStartDate,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 StageId = a.StageId,
                                 ProjectId = a.ProjectId,
                                 Status = c.Status,
                                 Deadline = c.DateTo.HasValue && dateNow > c.DateTo && c.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                                 ProjectStatus = b.Status,
                                 FixByName = afn != null ? afn.Name : string.Empty,
                                 EmployeeFixId = c.EmployeeFixId,
                                 DepartmentId = c.DepartmentId,
                                 DepartmentName = cmn != null ? cmn.Name : string.Empty,
                                 AdviseName = akn != null ? akn.Name : string.Empty,
                                 ApproveName = ahn != null ? ahn.Name : string.Empty,
                                 SupportName = agn != null ? agn.Name : string.Empty,
                                 NotifyName = aln != null ? aln.Name : string.Empty,
                                 ErrorCode = a.Code,
                                 Subject = a.Subject,
                                 ModuleCode = adn != null ? adn.Code : string.Empty,
                                 ModuleName = adn != null ? adn.Name : string.Empty,
                                 ProductCode = aen != null ? aen.Code : string.Empty,
                                 ProductName = aen != null ? aen.Name : string.Empty,
                                 ProjectCode = b.Code,
                                 ProjectName = b.Name,
                                 EstimateTime = c.EstimateTime,
                                 Solution = c.Solution,
                                 DepartmentManageId = b.DepartmentId,
                                 CustomerFinalId = b.CustomerFinalId,
                                 CustomerId = b.CustomerId,
                                 StageName = astn != null ? astn.Name : string.Empty
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                dataQuery = dataQuery.Where(u => u.ProjectId.Equals(searchModel.ProjectId));
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectStatus))
            {
                dataQuery = dataQuery.Where(u => u.ProjectStatus.Equals(searchModel.ProjectStatus));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.EmployeeFixId));
            }

            if (!string.IsNullOrEmpty(searchModel.StageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.StageId.Equals(u.StageId));
            }

            if (searchModel.DateFrom != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate <= searchModel.DateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerFinalId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerFinalId.Equals(u.CustomerFinalId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentManageId.Equals(u.DepartmentManageId));
            }

            if (searchModel.WorkType == 1)
            {
                dataQuery = dataQuery.Where(r => r.Deadline == searchModel.Deadline);
            }
            else if (searchModel.WorkType == 3)
            {
                dataQuery = dataQuery.Where(r => r.Deadline > 0 && r.Status != Constants.ErrorFix_Status_Finish);
            }
            else if (searchModel.WorkType == 4)
            {
                dataQuery = dataQuery.Where(r => r.Status != Constants.ErrorFix_Status_Finish && r.DateFrom <= dateNow);
            }
            SearchResultModel<ReportErrorWorkModel> result = new SearchResultModel<ReportErrorWorkModel>();

            result.TotalItem = dataQuery.Count();
            result.ListResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

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
