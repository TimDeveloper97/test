using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Error;
using NTS.Model.Project;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ProjectErrors
{
    public class ProjectErrorBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchProjectErrorResultModel SearchModuleErrors(ErrorSearchModel modelSearch)
        {
            SearchProjectErrorResultModel searchResult = new SearchProjectErrorResultModel();

            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
                             from g in ag.DefaultIfEmpty()
                             join i in db.Employees.AsNoTracking() on a.FixBy equals i.Id into ai
                             from i in ai.DefaultIfEmpty()
                             join h in db.Employees.AsNoTracking() on a.ErrorBy equals h.Id 
                             join j in db.Departments.AsNoTracking() on a.DepartmentProcessId equals j.Id into aj
                             from j in aj.DefaultIfEmpty()
                             where modelSearch.ProjectId.Equals(a.ProjectId)
                             orderby a.PlanStartDate descending, a.Code descending
                             select new ErrorModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 ErrorGroupName = b.Name,
                                 AuthorId = a.AuthorId,
                                 AuthorName = c.Name,
                                 PlanStartDate = a.PlanStartDate,
                                 PlanFinishDate = a.PlanFinishDate,
                                 ActualFinishDate = a.ActualFinishDate,
                                 ObjectId = a.ObjectId,
                                 ProjectId = a.ProjectId,
                                 ProjectName = d.Name,
                                 Statuss = a.Status == 1 ? "Đang tạo" : a.Status == 2 ? "Chờ xác nhận" : a.Status == 3 ? "Chưa có kế hoạch"
                                 : a.Status == 4 ? "Đang chờ xử lý" : a.Status == 5 ? "Đang xử lý" : a.Status == 6 ? "Đang QC"
                                 : a.Status == 7 ? "QC đạt" : a.Status == 8 ? "QC không đạt" : a.Status == 9 ? "Đóng vấn đề của dự án"
                                 : "Đã khắc phục triệt để",
                                 ModuleErrorVisualId = a.ModuleErrorVisualId,
                                 ModuleErrorVisualCode = g.Code,
                                 ModuleErrorVisualName = g.Name,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = e.Name,
                                 ErrorBy = a.ErrorBy,
                                 FixByName = h.Name,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 DepartmentProcessName = j.Name,
                                 StageId = a.StageId,
                                 StageName = f.Name,
                                 FixBy = a.FixBy,
                                 //FixByName = i.Name,
                                 Note = a.Note,
                                 ErrorCost = a.ErrorCost,
                                 Description = a.Description,
                                 Type = a.Type,
                                 TypeName = a.Type == 1 ? "Lỗi" : "Yêu cầu",
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 Status = a.Status,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ProjectName))
            {
                dataQuery = dataQuery.Where(u => u.ProjectName.Contains(modelSearch.ProjectName));
            }

            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(u => u.Status == modelSearch.Status);
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorGroupId))
            {
                dataQuery = dataQuery.Where(u => u.ErrorGroupId.Equals(modelSearch.ErrorGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorBy))
            {
                dataQuery = dataQuery.Where(u => u.ErrorBy.Equals(modelSearch.ErrorBy));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentProcessId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentProcessId.Equals(modelSearch.DepartmentProcessId));
            }

            if (!string.IsNullOrEmpty(modelSearch.FixBy))
            {
                dataQuery = dataQuery.Where(u => u.FixBy.Equals(modelSearch.FixBy));
            }

            if (!string.IsNullOrEmpty(modelSearch.ObjectId))
            {
                dataQuery = dataQuery.Where(u => u.ObjectId.Equals(modelSearch.ObjectId));
            }

            if (!string.IsNullOrEmpty(modelSearch.StageId))
            {
                dataQuery = dataQuery.Where(u => u.StageId.Equals(modelSearch.StageId));
            }

            if (modelSearch.Type != 0)
            {
                if (modelSearch.Type == Constants.Error_Type_Error)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Error);
                }
                else if (modelSearch.Type == Constants.Error_Type_Issue)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Issue);
                }
            }

            if (modelSearch.DateOpen != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate >= modelSearch.DateOpen);
            }

            if (modelSearch.DateEnd != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate <= modelSearch.DateEnd);
            }

            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(r => r.Status == modelSearch.Status);
            }

            searchResult.TotalItem = dataQuery.Where(r => r.Type == Constants.Error_Type_Issue).Count();
            searchResult.TotalError = dataQuery.Where(r => r.Type == Constants.Error_Type_Error).Count();
            searchResult.Status1 = dataQuery.Where(r => r.Status == 1).Count();
            searchResult.Status2 = dataQuery.Where(r => r.Status == 2).Count();
            searchResult.Status3 = dataQuery.Where(r => r.Status == 3).Count();
            searchResult.Status4 = dataQuery.Where(r => r.Status == 4).Count();
            searchResult.Status5 = dataQuery.Where(r => r.Status == 5).Count();
            searchResult.Status6 = dataQuery.Where(r => r.Status == 6).Count();
            searchResult.Status7 = dataQuery.Where(r => r.Status == 7).Count();
            searchResult.Status8 = dataQuery.Where(r => r.Status == 8).Count();
            searchResult.Status9 = dataQuery.Where(r => r.Status == 9).Count();

            searchResult.MaxDeliveryDay = dataQuery.Where(r => r.Status == 5).Count();
            var listResult = dataQuery.ToList();

            var errorIds = listResult.Select(s => s.Id).ToList();
            var errorFixResults = (from a in db.ErrorFixs.AsNoTracking()
                                   where errorIds.Contains(a.ErrorId)
                                   join b in db.Employees.AsNoTracking() on a.EmployeeFixId equals b.Id into ab
                                   from abn in ab.DefaultIfEmpty()
                                   join c in db.Employees.AsNoTracking() on a.SupportId equals c.Id into ac
                                   from acn in ac.DefaultIfEmpty()
                                   join d in db.Employees.AsNoTracking() on a.ApproveId equals d.Id into ad
                                   from adn in ad.DefaultIfEmpty()
                                   join e in db.Employees.AsNoTracking() on a.AdviseId equals e.Id into ae
                                   from aen in ae.DefaultIfEmpty()
                                   join f in db.Employees.AsNoTracking() on a.NotifyId equals f.Id into af
                                   from afn in af.DefaultIfEmpty()
                                   join j in db.Departments.AsNoTracking() on a.DepartmentId equals j.Id into aj
                                   from ajn in aj.DefaultIfEmpty()
                                   select new ErrorFixResultModel
                                   {
                                       AdviseName = aen != null ? aen.Name : string.Empty,
                                       ApproveName = adn != null ? adn.Name : string.Empty,
                                       SupportName = acn != null ? acn.Name : string.Empty,
                                       NotifyName = afn != null ? afn.Name : string.Empty,
                                       FixByName = abn != null ? abn.Name : string.Empty,
                                       DepartmentName = ajn != null ? ajn.Name : string.Empty,
                                       DateFrom = a.DateFrom,
                                       DateTo = a.DateTo,
                                       ErrorId = a.ErrorId,
                                       Id = a.Id,
                                       Solution = a.Solution,
                                       Status = a.Status,
                                       EstimateTime = a.EstimateTime,
                                       Done = a.Done,
                                       ErrorByName = abn.Name
                                   }).ToList();
            var employee = db.Employees.ToList();
            List<ErrorModel> listErrorFixs = new List<ErrorModel>();
            foreach (var item in listResult.ToList())
            {
                var query = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id)).OrderBy(a => a.Solution).Select(a => new ErrorModel
                {
                    Id = a.Id,
                    FinishDate = a.FinishDate,
                    Status = a.Status,
                    Solution = a.Solution,
                    ErrorId = item.Id,
                    DateFrom = a.DateFrom,
                    Done = a.Status == 2 ? "1": "0",
                    EmployeeFixId = a.EmployeeFixId
                }).ToList();
                if (query.Count > 0)
                {
                    foreach(var ite in query.ToList())
                    {
                        if (!string.IsNullOrEmpty(ite.EmployeeFixId))
                        {
                           var name = employee.Where(r => r.Id.Equals(ite.EmployeeFixId)).FirstOrDefault();
                            if (name != null)
                            {
                                ite.FixByName = name.Name;
                            }
                        }
                        
                    }
                }
                var querycomplete = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id) && r.Status == 2).Count();
                item.Done = $"{querycomplete}/{query.Count()}";
                listErrorFixs.AddRange(query);
                item.FinishDate = query.Count > 0 ? query.Max(r => r.FinishDate) : null;

            }
            List<ErrorModel> listError = new List<ErrorModel>();
            listError.AddRange(listResult);
            listError.AddRange(listErrorFixs);

            searchResult.Status10 = listErrorFixs.Count();
            searchResult.Status11 = listErrorFixs.Where(r => r.Status == 2).Count();
            searchResult.Status12 = listErrorFixs.Where(r => r.DateFrom < DateTime.Now && r.Status == 1).Count();
            searchResult.Status13 = listErrorFixs.Where(r => r.DateFrom > DateTime.Now && r.Status == 1).Count();


            searchResult.Errors = listError;

            List<ErrorFixResultModel> errorFixs;
            foreach (var item in listResult.ToList())
            {
                errorFixs = errorFixResults.Where(r => r.ErrorId.Equals(item.Id)).Select(a => new ErrorFixResultModel
                {
                    AdviseName = a.AdviseName,
                    ApproveName = a.ApproveName,
                    SupportName = a.SupportName,
                    NotifyName = a.NotifyName,
                    FixByName = a.FixByName,
                    DepartmentName = a.DepartmentName,
                    DateFrom = a.DateFrom,
                    DateTo = a.DateTo,
                    ErrorId = a.ErrorId,
                    Id = a.Id,
                    Solution = a.Solution,
                    Status = a.Status,
                    EstimateTime = a.EstimateTime,
                    ErrorCode = item.Code,
                    Subject = item.Subject,
                    ModuleCode = item.ModuleErrorVisualCode,
                    ModuleName = item.ModuleErrorVisualName,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProjectCode = item.ProjectCode,
                    ProjectName = item.ProjectName,
                    Done = a.Done,
                }).ToList();
                searchResult.ErrorFixs.AddRange(errorFixs);
            }

            return searchResult;
        }

        public ErrorModel GetErrorInfo(ErrorModel model)
        {
            var resultInfo = db.Errors.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(a => new ErrorModel
            {
                Id = a.Id,
                Subject = a.Subject,
                Code = a.Code,
                ProjectId = a.ProjectId,
                ErrorGroupId = a.ErrorGroupId,
                AuthorId = a.AuthorId,
                PlanStartDate = a.PlanStartDate,
                ObjectId = a.ObjectId,
                ModuleErrorVisualId = a.ModuleErrorVisualId,
                DepartmentId = a.DepartmentId,
                ErrorBy = a.ErrorBy,
                DepartmentProcessId = a.DepartmentProcessId,
                StageId = a.StageId,
                FixBy = a.FixBy,
                Note = a.Note,
                ErrorCost = a.ErrorCost,
                Description = a.Description,
                Status = a.Status,
                Solution = a.Solution,
                ActualStartDate = a.ActualStartDate,
                ActualFinishDate = a.ActualFinishDate,
                Type = a.Type,
            }).FirstOrDefault();

            //Nhóm vấn đề 
            if (!string.IsNullOrEmpty(resultInfo.ErrorGroupId))
            {
                var errorGroup = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ErrorGroupId));
                if (errorGroup != null)
                {
                    resultInfo.ErrorGroupName = errorGroup.Name;
                }
            }

            //Phòng ban gây lỗi
            if (!string.IsNullOrEmpty(resultInfo.DepartmentId))
            {
                var deparment = db.Departments.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.DepartmentId));
                if (deparment != null)
                {
                    resultInfo.DepartmentName = deparment.Name;
                }
            }

            //Người phát hiện
            if (!string.IsNullOrEmpty(resultInfo.AuthorId))
            {
                var employee = db.Employees.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.AuthorId));
                if (employee != null)
                {
                    resultInfo.AuthorName = employee.Name;
                }
            }

            //Dự án
            if (!string.IsNullOrEmpty(resultInfo.ProjectId))
            {
                var project = db.Projects.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ProjectId));
                if (project != null)
                {
                    resultInfo.ProjectName = project.Name;
                }
            }

            //Module
            if (!string.IsNullOrEmpty(resultInfo.ObjectId))
            {
                var module = db.Modules.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ObjectId));
                if (module != null)
                {
                    resultInfo.ObjectName = module.Name;
                }
            }

            //Loại yêu cầu
            if (resultInfo.Type != 0)
            {
                resultInfo.TypeName = resultInfo.Type == Constants.Error_Type_Error ? "Lỗi" : "Vấn đề";
            }

            //Người chịu trách nhiệm
            if (!string.IsNullOrEmpty(resultInfo.ErrorBy))
            {
                var employee = db.Employees.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ErrorBy));
                if (employee != null)
                {
                    resultInfo.ErrorByName = employee.Name;
                }
            }

            //Bộ phận khắc phục
            if (!string.IsNullOrEmpty(resultInfo.DepartmentProcessId))
            {
                var department = db.Departments.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.DepartmentProcessId));
                if (department != null)
                {
                    resultInfo.DepartmentProcessName = department.Name;
                }
            }

            //Người khắc phục
            if (!string.IsNullOrEmpty(resultInfo.FixBy))
            {
                var employee = db.Employees.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.FixBy));
                if (employee != null)
                {
                    resultInfo.FixByName = employee.Name;
                }
            }

            //Công đoạn xảy ra vấn đề
            if (!string.IsNullOrEmpty(resultInfo.StageId))
            {
                var stage = db.Stages.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.StageId));
                if (stage != null)
                {
                    resultInfo.StageName = stage.Name;
                }
            }


            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProblemExist);
            }

            return resultInfo;
        }


        //public string ExportExcelError(string id)
        //{
        //    var dataQuery = (from a in db.Errors.AsNoTracking()
        //                     join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
        //                     join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
        //                     from c in ac.DefaultIfEmpty()
        //                     join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
        //                     from d in ad.DefaultIfEmpty()
        //                     join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
        //                     from e in ae.DefaultIfEmpty()
        //                     join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
        //                     from f in af.DefaultIfEmpty()
        //                     join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
        //                     from g in ag.DefaultIfEmpty()
        //                     join i in db.Employees.AsNoTracking() on a.FixBy equals i.Id into ai
        //                     from i in ai.DefaultIfEmpty()
        //                     join h in db.Employees.AsNoTracking() on a.ErrorBy equals h.Id into ah
        //                     from h in ah.DefaultIfEmpty()
        //                     join j in db.Departments.AsNoTracking() on a.DepartmentProcessId equals j.Id into aj
        //                     from j in aj.DefaultIfEmpty()
        //                     where id.Equals(a.ProjectId)
        //                     orderby a.PlanStartDate descending, a.Code descending
        //                     select new ErrorModel
        //                     {
        //                         Id = a.Id,
        //                         Subject = a.Subject,
        //                         Code = a.Code,
        //                         ErrorGroupId = a.ErrorGroupId,
        //                         ErrorGroupName = b.Name,
        //                         AuthorId = a.AuthorId,
        //                         AuthorName = c.Name,
        //                         PlanStartDate = a.PlanStartDate,
        //                         PlanFinishDate = a.PlanFinishDate,
        //                         ActualFinishDate = a.ActualFinishDate,
        //                         ObjectId = a.ObjectId,
        //                         ProjectId = a.ProjectId,
        //                         ProjectName = d.Name,
        //                         Status = a.Status,
        //                         ModuleErrorVisualId = a.ModuleErrorVisualId,
        //                         ModuleErrorVisualCode = g.Code,
        //                         ModuleErrorVisualName = g.Name,
        //                         DepartmentId = a.DepartmentId,
        //                         DepartmentName = e.Name,
        //                         ErrorBy = a.ErrorBy,
        //                         ErrorByName = h.Name,
        //                         DepartmentProcessId = a.DepartmentProcessId,
        //                         DepartmentProcessName = j.Name,
        //                         StageId = a.StageId,
        //                         StageName = f.Name,
        //                         FixBy = a.FixBy,
        //                         FixByName = i.Name,
        //                         Note = a.Note,
        //                         ErrorCost = a.ErrorCost,
        //                         Description = a.Description,
        //                         Type = a.Type,
        //                         TypeName = a.Type == 1 ? "Lỗi" : "Yêu cầu",
        //                         CreateDate = a.CreateDate,
        //                         CreateBy = a.CreateBy,
        //                         UpdateBy = a.UpdateBy,
        //                         UpdateDate = a.UpdateDate
        //                     }).AsQueryable();
        //    var listResult = dataQuery.ToList();
        //    List<ErrorModel> listErrorFixs = new List<ErrorModel>();
        //    List<ErrorModel> listError = new List<ErrorModel>();
        //    foreach (var item in listResult.ToList())
        //    {
        //        var query = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id)).OrderBy(a => a.Solution).Select(a => new ErrorModel
        //        {
        //            Id = a.Id,
        //            FinishDate = a.FinishDate,
        //            Done = a.Done,
        //            Status = a.Status,
        //            Solution = a.Solution,
        //            ErrorId = item.Id,
        //            DateFrom = a.DateFrom
        //        }).ToList();

        //        listError.Add(item);
        //        listError.AddRange(query);
        //    }



        //    List<ErrorModel> listModel = listError.ToList();

        //    if (listModel.Count == 0)
        //    {
        //        throw NTSException.CreateInstance("Không có dữ liệu!");
        //    }
        //    try
        //    {
        //        ExcelEngine excelEngine = new ExcelEngine();

        //        IApplication application = excelEngine.Excel;

        //        IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Danh mục vấn đề.xlsx"));

        //        IWorksheet sheet = workbook.Worksheets[0];

        //        var total = listModel.Count;

        //        IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
        //        iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

        //        var listExport = listModel.Select((a, i) => new
        //        {
        //           a.Code,
        //           a.Subject,
        //           a.TypeName,
        //           a.Note,
        //           a.Description,
        //           a.Solution,
        //           a.FinishDate,
        //           StatusType = a.Status.Equals("1") ? "Chưa hoàn thành" : "Hoàn Thành",
        //           a.Done,

        //        });

        //        sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
        //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
        //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
        //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
        //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
        //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
        //        //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;

        //        string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh mục vấn đề" + ".xls");
        //        workbook.SaveAs(pathExport);
        //        workbook.Close();
        //        excelEngine.Dispose();

        //        //Đường dẫn file lưu trong web client
        //        string resultPathClient = "Template/" + Constants.FolderExport + "Danh mục vấn đề" + ".xls";

        //        return resultPathClient;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.LogError(ex);
        //        throw new NTSLogException(id, ex);
        //    }
        //}



        public string ExportExcelError(string id)
        {
            List<ErrorResultModel> listModel = this.MakeWhereCondition(id).Where(r => r.Status == Constants.Problem_Status_NoPlan || r.Status == Constants.Problem_Status_Processed).ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            List<string> errorIds = listModel.Select(r => r.Id).ToList();

            // Lấy danh sách công việc của từng Vấn đề tồn đọng
            var errorFixs = db.ErrorFixs.Where(r => errorIds.Contains(r.ErrorId)).ToList();

            // Lấy danh sách dự án đang có vấn đề tồn đọng
            var projects = listModel
                        .Select(m => new { m.ProjectId, m.ProjectCode, m.ProjectName, m.PriceNoVAT })
                        .GroupBy(m => new { m.ProjectId, m.ProjectCode, m.ProjectName, m.PriceNoVAT })
                        .Distinct()
                        .ToList();

            List<ErrorFixExportModel> exportData = new List<ErrorFixExportModel>();
            ErrorFixExportModel errorExportModel;
            var employees = db.Employees.AsNoTracking().ToList();
            var departments = db.Departments.AsNoTracking().ToList();
            var products = db.Products.AsNoTracking().ToList();
            var payments = db.Payments.AsNoTracking().ToList();
            CultureInfo ci = new CultureInfo("en-us");

            int index = 1;
            foreach (var project in projects)
            {
                var errors = listModel.Where(r => r.ProjectId.Equals(project.Key.ProjectId)).ToList();

                // Lấy thông tin thanh toán của dự án
                var payment = payments.Where(r => r.ProjectId.Equals(project.Key.ProjectId)).ToList();
                //decimal? tongDaThu = payment == null ? 0 : payment.Sum(r => r.ActualAmount);
                //decimal? conPhaiThu = project.Key.PriceNoVAT - tongDaThu;
                //decimal tyle = project.Key.PriceNoVAT == 0 ? 0 : (decimal)tongDaThu / (decimal)project.Key.PriceNoVAT;

                foreach (var error in errors)
                {
                    var errorFix = errorFixs.Where(r => r.ErrorId.Equals(error.Id)).ToList();
                    var product = products.Where(r => r.Id.Equals(error.ObjectId)).FirstOrDefault();
                    var module = db.Modules.FirstOrDefault(a => a.Id.Equals(error.ObjectId));
                    errorExportModel = new ErrorFixExportModel();
                    errorExportModel.MaVanDe = error.Code;
                    //errorExportModel.Index = index++;
                    //errorExportModel.MaDA = project.ProjectCode;
                    //errorExportModel.TenDA = project.ProjectName;
                    //errorExportModel.TinhTrangDA = GetProjectStatusName(project.Status);
                    //errorExportModel.GiaTriHD = project.PriceNoVAT.ToString();
                    //errorExportModel.TongDaThu = tongDaThu == null ? String.Empty : tongDaThu.ToString();
                    //errorExportModel.ConPhaiThu = conPhaiThu == null ? String.Empty : conPhaiThu.ToString();
                    //errorExportModel.TyLe = tyle.ToString("P", ci);
                    if (error.ObjectType == 1)
                    {
                        errorExportModel.MaHangMuc = module?.Code;
                        errorExportModel.TenHangMuc = module?.Name;
                    }
                    else
                    {
                        errorExportModel.MaHangMuc = product?.Code;
                        errorExportModel.TenHangMuc = product?.Name;
                    }

                    errorExportModel.PhanLoai = error.Type == Constants.Error_Type_Error ? "Lỗi" : error.Type == Constants.Error_Type_Issue ? "Vấn đề" : "";
                    errorExportModel.TenVanDe = error.Subject;
                    errorExportModel.MoTa = error.Description;
                    errorExportModel.NguyenNhan = error.Note;
                    //errorExportModel.Done = error.

                    if (errorFix != null)
                    {
                        errorExportModel.NgayKetThuc = errorFix.Select(r => r.DateTo).Max().HasValue ? errorFix.Select(r => r.DateTo).Max().Value.ToString("dd/MM/yyyy") : String.Empty;
                    }

                    errorExportModel.TinhTrang = errorFix.Count() == 0 ? "Chưa có kế hoạch" : (errorFix.Select(r => r.Status == 1).Any() ? "Chưa xong" : "Đã xong");
                    errorExportModel.Done = errorFix.Count > 0 ? (int)(errorFix.Sum(a => a.Done) / errorFix.Count) : 0;
                    exportData.Add(errorExportModel);

                    foreach (var item in errorFix)
                    {
                        errorExportModel = new ErrorFixExportModel();
                        //errorExportModel.Index = index++;
                        errorExportModel.GiaiPhap = item.Solution;
                        errorExportModel.NguoiThucHien = employees.Where(r => r.Id.Equals(item.EmployeeFixId)).FirstOrDefault() != null ? employees.Where(r => r.Id.Equals(item.EmployeeFixId)).FirstOrDefault().Name : String.Empty;
                        errorExportModel.BoPhanThucHien = departments.Where(r => r.Id.Equals(item.DepartmentId)).FirstOrDefault() != null ? departments.Where(r => r.Id.Equals(item.DepartmentId)).FirstOrDefault().Name : String.Empty;
                        errorExportModel.NgayBatDau = item.DateFrom.HasValue ? item.DateFrom.Value.ToString("dd/MM/yyyy") : String.Empty;
                        errorExportModel.NgayKetThuc = item.DateTo.HasValue ? item.DateTo.Value.ToString("dd/MM/yyyy") : String.Empty;
                        errorExportModel.TinhTrang = item.Status == 1 ? "Chưa xong" : "Đã xong";
                        errorExportModel.Done = item.Done;

                        exportData.Add(errorExportModel);
                    }
                }
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ErrorExcel.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = exportData.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                sheet.ImportData(exportData, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders.Color = ExcelKnownColors.Black;
                sheet.Range["A" + 2 + ":" + "Z" + (2 + total)].AutofitRows();

                var projectName = db.Projects.Where(a => a.Id.Equals(id)).Select(a => a.Name).FirstOrDefault();
                var projectCode = db.Projects.Where(a => a.Id.Equals(id)).Select(a => a.Code).FirstOrDefault();
                iRangeData = sheet.FindFirst("<TenDA>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = projectName;
                }

                iRangeData = sheet.FindFirst("<MaDA>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (iRangeData != null)
                {
                    iRangeData.Value2 = projectCode;
                }

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vấn đề tồn đọng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vấn đề tồn đọng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private IQueryable<ErrorResultModel> MakeWhereCondition(string id)
        {
            DateTime dateNow = DateTime.Now.Date;

            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id
                             join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                             where a.ProjectId == id
                             select new ErrorResultModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 AuthorId = a.AuthorId,
                                 PlanStartDate = a.PlanStartDate,
                                 ObjectId = a.ObjectId,
                                 ObjectType = a.ObjectType,
                                 Status = a.Status,
                                 DepartmentId = a.DepartmentId,
                                 ErrorBy = a.ErrorBy,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 StageId = a.StageId,
                                 FixBy = a.FixBy,
                                 Type = a.Type,
                                 ProjectId = a.ProjectId,
                                 DepartmentCreateId = a.DepartmentCreateId,
                                 ProjectName = b.Name,
                                 ProjectCode = b.Code,
                                 DepartmentManageId = b.DepartmentId,
                                 AffectId = a.AffectId,
                                 CreateDate = a.CreateDate,
                                 AuthorDepartmentId = d.Id,
                                 PriceNoVAT = b.SaleNoVAT,
                                 Note = a.Note
                             }).AsQueryable();



            return dataQuery;
        }

        private string GetProjectStatusName(int status)
        {
            switch (status)
            {
                case 1:
                    return "Chưa kickoff";
                    break;
                case 2:
                    return "Sản xuất";
                    break;
                case 3:
                    return "Đóng dự án";
                    break;
                case 4:
                    return "Tạm dừng";
                    break;
                case 5:
                    return "Lắp đặt";
                    break;
                case 6:
                    return "Hiệu chỉnh";
                    break;
                case 7:
                    return "Đưa vào sử dụng";
                    break;
                case 8:
                    return "Thiết kế";
                    break;
                case 9:
                    return "Nghiệm thu";
                    break;
                default:
                    return string.Empty;
            }
        }
    }

}
