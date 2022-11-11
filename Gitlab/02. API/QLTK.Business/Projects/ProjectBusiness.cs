using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Customers;
using NTS.Model.Project;
using NTS.Model.ProjectHistory;
using NTS.Model.Projects.Project;
using NTS.Model.Report;
using NTS.Model.Repositories;
using NTS.Model.TestCriteria;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using static NTS.Model.Projects.Project.MoneyCollectionProjectReportResultModel;

namespace QLTK.Business.Projects
{
    public class ProjectBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        /// <summary>
        /// Tìm kiếm dự án theo modules
        /// </summary>
        /// <param name="projectSearch"></param>
        /// <returns></returns>
        public object SearchProjectModel(ProjectSearchModel model)
        {
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join d in db.ProjectProductTestCriterias.AsNoTracking() on b.Id equals d.ProjectProductId into da
                             from dc in da.DefaultIfEmpty()
                             join e in db.Employees.AsNoTracking() on a.ManageId equals e.Id into ae
                             from aen in ae.DefaultIfEmpty()
                             where b.ModuleId.Equals(model.ModuleId)
                             orderby a.Code
                             select new ProjectModel
                             {
                                 Id = b.ModuleId,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Status = a.Status,
                                 Parameter = a.Parameter,
                                 Note = a.Note,
                                 DateFrom = a.DateFrom,
                                 DateTo = a.DateTo,
                                 CountModule = b.Quantity,
                                 ProjectId = b.ProjectId,
                                 ProjectProductId = b.Id,
                                 StatusTestCriteria = dc.ResultStatus,
                                 WarehouseCode = a.WarehouseCode,
                                 FCMPrice = a.FCMPrice,
                                 Type = a.Type,
                                 PaymentStatus = a.PaymentStatus,
                                 PhoneNumber = aen != null ? aen.PhoneNumber : string.Empty,
                                 ManageName = aen != null ? aen.Name : string.Empty
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProjectCode))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(model.ProjectCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.ProjectName))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(model.ProjectName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                dataQuery = dataQuery.Where(r => r.Status.Equals(model.Status));
            }

            if (model.PaymentStatus.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.PaymentStatus.HasValue && r.PaymentStatus == model.PaymentStatus);
            }

            if (model.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == model.Type.Value);
            }

            if (model.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom >= model.DateFrom);
            }

            if (model.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateTo <= model.DateTo);
            }

            var list = (from d in dataQuery
                            //join s in db.ProjectProducts.AsNoTracking() on d.Id equals s.ProjectId
                        group d by new { d.ProjectId, d.Name, d.Code, d.Status, d.Note, d.DateFrom, d.DateTo, d.FCMPrice } into g
                        select new
                        {
                            g.Key.ProjectId,
                            g.Key.Name,
                            g.Key.Code,
                            g.Key.Status,
                            g.Key.Note,
                            g.Key.DateFrom,
                            g.Key.DateTo,
                            g.Key.FCMPrice,
                            CountModule = g.Sum(r => r.CountModule)
                        }).ToList();

            var totalModule = list.Sum(i => i.CountModule);
            var totalProject = list.Count();
            var ListResult = list.ToList();
            return new
            {
                TotalModule = totalModule,
                TotalProject = totalProject,
                ListResult
            };
        }

        /// <summary>
        /// Tìm kiếm tiêu chí
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<TestCriteriaModel> SearchTestCriteria(TestCriterSearchModel searchModelTest)
        {
            SearchResultModel<TestCriteriaModel> searchCriteria = new SearchResultModel<TestCriteriaModel>();
            var dataQuery = (from a in db.ProjectProductTestCriterias.AsNoTracking()
                             join b in db.TestCriterias.AsNoTracking() on a.TestCriteriaId equals b.Id
                             join c in db.TestCriteriaGroups.AsNoTracking() on b.TestCriteriaGroupId equals c.Id
                             where a.ProjectProductId.Equals(searchModelTest.Id)
                             select new TestCriteriaModel
                             {
                                 Id = a.Id,
                                 TestCriteriaGroupId = b.TestCriteriaGroupId,
                                 TestCriteriaGroupName = c.Name,
                                 Code = b.Code,
                                 Name = b.Name,
                                 TechnicalRequirements = b.TechnicalRequirements,
                                 Note = b.Note,
                                 ResuldStatusTest = a.ResultStatus,
                                 NoteResuld = a.Note,
                             }
                             ).AsQueryable();
            if (!string.IsNullOrEmpty(searchModelTest.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(searchModelTest.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModelTest.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(searchModelTest.Name.ToUpper()));
            }
            searchCriteria.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModelTest.OrderBy, searchModelTest.OrderType).Skip((searchModelTest.PageNumber - 1) * searchModelTest.PageSize).Take(searchModelTest.PageSize).ToList();
            searchCriteria.ListResult = listResult;

            return searchCriteria;
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(TestCriterSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.ProjectProductTestCriterias.AsNoTracking()
                             join b in db.TestCriterias.AsNoTracking() on a.TestCriteriaId equals b.Id
                             join c in db.TestCriteriaGroups.AsNoTracking() on b.TestCriteriaGroupId equals c.Id
                             select new TestCriteriaModel
                             {
                                 Id = a.Id,
                                 TestCriteriaGroupId = b.TestCriteriaGroupId,
                                 TestCriteriaGroupName = c.Name,
                                 Code = b.Code,
                                 Name = b.Name,
                                 TechnicalRequirements = b.TechnicalRequirements,
                                 Note = b.Note,
                                 ResuldStatusTest = a.ResultStatus,
                                 NoteResuld = a.Note,
                             }).AsQueryable();
            List<TestCriteriaModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProjectModule.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.TestCriteriaGroupName,
                    a.Name,
                    a.Code,
                    a.TechnicalRequirements,
                    a.Note,
                    View = a.ResuldStatusTest != true ? "Không đạt" : "Đạt",
                    a.NoteResuld,

                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách tiêu chí kiểm tra module dự án" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách tiêu chí kiểm tra module dự án" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// danh sách dự án
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public SearchResultProjectModel<ProjectResultModel> SearchProject(ProjectSearchModel searchModel)
        {
            SearchResultProjectModel<ProjectResultModel> searchResultObject = new SearchResultProjectModel<ProjectResultModel>();

            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.CustomerTypes.AsNoTracking() on b.CustomerTypeId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             orderby a.Code
                             select new ProjectResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 DateFrom = a.DateFrom,
                                 SBUId = a.SBUId,
                                 SBUName = c.Name,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = d.Name,
                                 CustomerTypeId = e.Id,
                                 CustomerType = e.Name,
                                 CustomerId = b.Id,
                                 CustomerName = b.Name,
                                 DateTo = a.DateTo,
                                 Status = a.Status,
                                 Parameter = a.Parameter,
                                 SaleNoVAT = a.SaleNoVAT,
                                 WarehouseCode = a.WarehouseCode,
                                 KickOffDate = a.KickOffDate,
                                 CreateDate = a.CreateDate,
                                 FCMPrice = a.FCMPrice,
                                 //DocumentStatus = a.DocumentStatus,

                                 Type = a.Type,
                                 PaymentStatus = a.PaymentStatus,
                                 Priority = a.Priority,
                             }).AsQueryable();
            var listProjectProduct = db.ProjectProducts.AsNoTracking().ToList();


            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(searchModel.Code.ToUpper()) || r.Name.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (searchModel.PaymentStatus.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.PaymentStatus.HasValue && r.PaymentStatus == searchModel.PaymentStatus);
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(r => r.DepartmentId.Equals(searchModel.DepartmentId));
            }
            else if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(r => r.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.Status))
            {
                dataQuery = dataQuery.Where(r => r.Status.Equals(searchModel.Status));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerName))
            {
                dataQuery = dataQuery.Where(r => r.CustomerName.ToUpper().Contains(searchModel.CustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerTypeId))
            {
                dataQuery = dataQuery.Where(r => r.CustomerTypeId.Equals(searchModel.CustomerTypeId));
            }

            if (searchModel.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == searchModel.Type.Value);
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom >= searchModel.DateFrom : u.KickOffDate >= searchModel.DateFrom);

            }

            if (searchModel.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom <= searchModel.DateTo : u.KickOffDate <= searchModel.DateTo);
            }

            if (searchModel.ErrorStatus.HasValue)
            {
                List<string> projectIds = new List<string>();

                projectIds = (from a in db.Errors.AsNoTracking()
                              join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                              where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && a.Status >= Constants.Problem_Status_Processed
                                && c.Status != Constants.ErrorFix_Status_Finish
                              group a by a.ProjectId into g
                              select g.Key).ToList();

                if (searchModel.ErrorStatus.Value == 1)
                {
                    dataQuery = dataQuery.Where(r => !projectIds.Contains(r.Id));
                }
                else if (searchModel.ErrorStatus.Value == 2)
                {
                    dataQuery = dataQuery.Where(r => projectIds.Contains(r.Id));
                }
            }

            var list = dataQuery.ToList();

            searchResultObject.Status1 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_NotStartedYet)).Count();
            searchResultObject.Status2 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_Processing)).Count();
            searchResultObject.Status3 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_Finish)).Count();
            searchResultObject.TotalItem = list.Count();

            var listResult = list.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResultObject.ListResult = listResult;

            foreach (var item in listResult)
            {
                var projectSolution = db.ProjectSolutions.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectSolution != null)
                {
                    item.IsSolution = true;
                }
                var projectTransferAttach = db.ProjectTransferAttaches.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectTransferAttach != null)
                {
                    item.IsTransfer = true;
                }

                var listprojectAttach = db.ProjectAttaches.Where(m => m.ProjectId.Equals(item.Id)).ToList();
                if (listprojectAttach != null)
                {
                    int a = 0;
                    foreach (var ite in listprojectAttach)
                    {

                        if (ite.Path != null && ite.IsRequired)
                        {
                            a++;
                        }
                        if (a == listprojectAttach.Count)
                        {
                            item.DocumentStatus = 1;
                        }
                        else if (a == 0)
                        {
                            item.DocumentStatus = 3;
                        }
                        else
                        {
                            item.DocumentStatus = 2;
                        }
                    }
                }
                else
                {
                    item.DocumentStatus = 0;
                }

            }
            if (searchModel.DocumentStatus.HasValue)
            {
                listResult = (List<ProjectResultModel>)listResult.Where(r => r.DocumentStatus == searchModel.DocumentStatus.Value);
            }
            return searchResultObject;

        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Excel(ProjectSearchModel model)
        {
            //model.IsExport = true;
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.CustomerTypes.AsNoTracking() on b.CustomerTypeId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             orderby a.CreateDate
                             select new ProjectResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 DateFrom = a.DateFrom,
                                 SBUId = a.SBUId,
                                 SBUName = c.Name,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = d.Name,
                                 CustomerTypeId = e.Id,
                                 CustomerType = e.Name,
                                 CustomerId = b.Id,
                                 CustomerName = b.Name,
                                 DateTo = a.DateTo,
                                 Status = a.Status,
                                 Parameter = a.Parameter,
                                 SaleNoVAT = a.SaleNoVAT,
                                 WarehouseCode = a.WarehouseCode,
                                 KickOffDate = a.KickOffDate,
                                 CreateDate = a.CreateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuery = dataQuery.Where(r => r.SBUId.Equals(model.SBUId));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(r => r.DepartmentId.Equals(model.DepartmentId));
            }
            if (!string.IsNullOrEmpty(model.Status))
            {
                dataQuery = dataQuery.Where(r => r.Status.Equals(model.Status));
            }
            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                dataQuery = dataQuery.Where(r => r.CustomerName.ToUpper().Contains(model.CustomerName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.CustomerTypeId))
            {
                dataQuery = dataQuery.Where(r => r.CustomerTypeId.Equals(model.CustomerTypeId));
            }

            if (model.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom >= model.DateFrom : u.KickOffDate >= model.DateFrom);
            }

            if (model.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.DateFrom != null ? u.DateFrom <= model.DateTo : u.KickOffDate <= model.DateTo);
            }
            var list = dataQuery.ToList();
            foreach (var item in list)
            {
                var projectSolution = db.ProjectSolutions.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectSolution != null)
                {
                    item.IsSolution = true;
                }
                var projectTransferAttach = db.ProjectTransferAttaches.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectTransferAttach != null)
                {
                    item.IsTransfer = true;
                }
            }

            List<ProjectResultModel> listModel = list;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/project.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    View = a.Status.Equals(Constants.Prooject_Status_NotStartedYet) ? "Chưa bắt đầu" :
                        a.Status.Equals(Constants.Prooject_Status_Processing) ? "Đang triển khai" :
                        a.Status.Equals(Constants.Prooject_Status_Finish) ? "Đã hoàn thành" :
                        a.Status.Equals(Constants.Prooject_Status_Stop) ? "Tạm dừng" :
                         a.Status.Equals(Constants.Prooject_Status_Setup) ? "Đang lắp đặt tại khách hàng" :
                          a.Status.Equals(Constants.Prooject_Status_Problem) ? "Đang xử lý vấn đề tồn đọng" : "",
                    a.Code,
                    a.Name,
                    isSolution = a.IsSolution == true ? "Có" : "Không",
                    isTransfer = a.IsTransfer == true ? "Có" : "Không",
                    a.SBUName,
                    a.DepartmentName,
                    a.CustomerType,
                    a.CustomerName,
                    viewDateForm = a.DateFrom.HasValue ? DateTimeHelper.ToStringDDMMYY(a.DateFrom.Value) : string.Empty,
                    viewDateTo = a.DateTo.HasValue ? DateTimeHelper.ToStringDDMMYY(a.DateTo.Value) : "",
                    a.SaleNoVAT,
                    a.WarehouseCode,
                });
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 14].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách dự án" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách dự án" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Xóa dự án
        /// </summary>
        /// <param name="model"></param>
        public void Delete(ProjectResultModel model, string departmentId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.ProjectProducts.AsNoTracking().Where(r => r.ProjectId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Project);
                }
                if (db.ProjectSolutions.AsNoTracking().Where(r => r.ProjectId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Project);
                }
                if (db.ProjectErrors.AsNoTracking().Where(r => r.ProjectId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Project);
                }
                var projectAttach = db.ProjectAttaches.Where(m => m.ProjectId.Equals(model.Id)).ToList();
                try
                {
                    var project = db.Projects.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (project == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
                    }
                    if (!string.IsNullOrEmpty(project.DepartmentId) && !project.DepartmentId.Equals(departmentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.Project);
                    }
                    db.ProjectAttaches.RemoveRange(projectAttach);
                    db.Projects.Remove(project);

                    var NameOrCode = project.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ProjectHistoryModel>(project);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Project, project.Id, NameOrCode, jsonApter);

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

        /// <summary>
        /// Thêm mới dự án
        /// </summary>
        /// <param name="productModel"></param>
        public void AddProject(ProjectResultModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            if (db.Projects.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Project);
            }

            if (model.DateFrom > model.DateTo)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0012, TextResourceKey.Project);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    Project newProject = new Project()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        DateFrom = model.DateFrom,
                        DateTo = model.DateTo,
                        Note = model.Note.NTSTrim(),
                        Status = model.Status,
                        CustomerId = model.CustomerId,
                        Parameter = model.Parameter.NTSTrim(),
                        KickOffDate = model.KickOffDate,
                        Price = model.Price,
                        DesignPrice = model.DesignPrice,
                        SaleNoVAT = model.SaleNoVAT,
                        WarehouseCode = model.WarehouseCode,
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = model.CreateBy,
                        CustomerFinalId = model.CustomerFinalId,
                        FCMPrice = model.FCMPrice,
                        Type = model.Type,
                        ManageId = model.ManageId,
                        Priority = model.Priority,
                    };

                    db.Projects.Add(newProject);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newProject.Code, newProject.Id, Constants.LOG_Project);

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

        /// <summary>
        /// Lấy thông tin dự án
        /// </summary>
        /// <param name="productModel"></param>
        public object GetProjectInfo(ProjectResultModel model)
        {
            var projectInfo = (from a in db.Projects.AsNoTracking()
                               where model.Id.Equals(a.Id)
                               join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                               from b in ab.DefaultIfEmpty()
                               join e in db.Employees.AsNoTracking() on a.ManageId equals e.Id into ae
                               from aen in ae.DefaultIfEmpty()
                               select new ProjectResultModel
                               {
                                   Id = a.Id,
                                   SBUId = a.SBUId,
                                   DepartmentId = a.DepartmentId,
                                   Name = a.Name,
                                   Code = a.Code,
                                   CustomerId = a.CustomerId,
                                   CustomerName = b.Name,
                                   DateFrom = a.DateFrom,
                                   DateTo = a.DateTo,
                                   Status = a.Status,
                                   Parameter = a.Parameter,
                                   Note = a.Note,
                                   KickOffDate = a.KickOffDate,
                                   Price = a.Price,
                                   DesignPrice = a.DesignPrice,
                                   CreateBy = a.CreateBy,
                                   CreateDate = a.CreateDate,
                                   UpdateBy = a.UpdateBy,
                                   UpdateDate = a.UpdateDate,
                                   SaleNoVAT = a.SaleNoVAT,
                                   WarehouseCode = a.WarehouseCode,
                                   CustomerFinalId = a.CustomerFinalId,
                                   FCMPrice = a.FCMPrice,
                                   Type = a.Type,
                                   PhoneNumber = aen != null ? aen.PhoneNumber : string.Empty,
                                   ManageId = a.ManageId,
                                   Priority = a.Priority,
                                   PaymentStatus = a.PaymentStatus,
                                   IsBadDebt = a.IsBadDebt,
                                   BadDebtDate = a.BadDebtDate
                               }).FirstOrDefault();

            if (projectInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
            }

            return projectInfo;
        }

        public CustomersModel GetCustomerTypeInfo(CustomersModel model)
        {
            var customerType = (from a in db.Customers.AsNoTracking()
                                join b in db.CustomerTypes.AsNoTracking() on a.CustomerTypeId equals b.Id into ab
                                from b in ab.DefaultIfEmpty()
                                where model.Id.Equals(a.Id)
                                orderby a.Code
                                select new CustomersModel
                                {
                                    Id = a.Id,
                                    CustomerTypeId = a.CustomerTypeId,
                                }).FirstOrDefault();
            return customerType;
        }

        /// <summary>
        /// Cập nhật dự án
        /// </summary>
        /// <param name="productModel"></param>
        public void UpdateProject(ProjectResultModel model, string departmentId)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            // kiểm tra mã 
            if (db.Projects.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Project);
            }

            var project = db.Projects.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

            if (project == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
            }

            if (!departmentId.Equals(project.DepartmentId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.Project);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //var jsonApter = AutoMapperConfig.Mapper.Map<ProjectHistoryModel>(project);

                    project.SBUId = model.SBUId;
                    project.DepartmentId = model.DepartmentId;
                    project.Code = model.Code.NTSTrim();
                    project.Status = model.Status;
                    project.Name = model.Name.NTSTrim();
                    project.DateFrom = model.DateFrom;
                    project.Parameter = model.Parameter.NTSTrim();
                    project.Note = model.Note.NTSTrim();
                    project.DateTo = model.DateTo;
                    project.KickOffDate = model.KickOffDate;
                    project.Price = model.Price;
                    project.DesignPrice = model.DesignPrice;
                    project.UpdateBy = model.UpdateBy;
                    project.UpdateDate = DateTime.Now;
                    project.CustomerId = model.CustomerId;
                    project.SaleNoVAT = model.SaleNoVAT;
                    project.WarehouseCode = model.WarehouseCode;
                    project.CustomerFinalId = model.CustomerFinalId;
                    project.FCMPrice = model.FCMPrice;
                    project.Type = model.Type;
                    project.ManageId = model.ManageId;
                    project.Priority = model.Priority;
                    project.PaymentStatus = model.PaymentStatus;
                    if (model.DateFrom > model.DateTo)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0012, TextResourceKey.Project);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ProjectHistoryModel>(project);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Project, project.Id, project.Code, jsonBefor, jsonApter);

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

        public MoneyCollectionProjectReportResultModel Report(MoneyCollectionProjectReportSearchModel searchModel)
        {
            MoneyCollectionProjectReportResultModel result = new MoneyCollectionProjectReportResultModel();

            var paymentRecords = (from r in db.Departments.AsNoTracking()
                                  join a in db.Projects.AsNoTracking() on r.Id equals a.DepartmentId
                                  join p in db.Payments.AsNoTracking() on a.Id equals p.ProjectId
                                  join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id
                                  into ca
                                  from c in ca.DefaultIfEmpty()
                                  orderby p.PaymentMilestone
                                  select new
                                  {
                                      DepartmentId = r.Id,
                                      DepartmentName = r.Name,
                                      ProjectId = a.Id,
                                      CustomerId = a.CustomerId,
                                      CustomerCode = c.Code,
                                      CustomerName = c.Name,
                                      PlanPaymentDate = p.PlanPaymentDate,
                                      PlanAmount = p.PlanAmount.HasValue ? p.PlanAmount : 0,
                                      TotalAmount = p.TotalAmount,
                                      IsBadDebt = a.IsBadDebt,
                                      BadDebtDate = a.BadDebtDate,
                                      Name = p.Name,
                                  }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                paymentRecords = paymentRecords.Where(u => searchModel.ProjectId.Equals(u.ProjectId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                paymentRecords = paymentRecords.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                paymentRecords = paymentRecords.Where(u => searchModel.Name.Equals(u.Name));
            }

            var department = (from d in paymentRecords
                              group d by new { d.DepartmentId, d.DepartmentName } into g
                              orderby g.Key.DepartmentName
                              select new
                              {
                                  DepartmentName = g.Key.DepartmentName,
                                  DepartmentId = g.Key.DepartmentId
                              }).ToList();

            ReportMoneyCollectionResultObjectModel data;
            ReportMoneyCollectionResultLabelModel months = new ReportMoneyCollectionResultLabelModel();
            List<ReportMoneyCollectionResultLabelModel> listMonths = new List<ReportMoneyCollectionResultLabelModel>();


            //dữ liệu các năm trước ko có nợ xấu
            var groupProject = (from a in paymentRecords
                                where a.IsBadDebt == false && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year < searchModel.Year.Value
                                group a by new { a.ProjectId, a.DepartmentId } into g
                                select new
                                {
                                    g.Key.ProjectId,
                                    g.Key.DepartmentId,
                                    PlanAmount = g.Sum(a => a.PlanAmount),
                                    TotalAmount = g.Sum(a => a.TotalAmount)
                                }).ToList();
            //dữ liệu các năm trước là nợ xấu
            var groupProjectBadDebt = (from a in paymentRecords
                                       where a.IsBadDebt == true && a.PlanAmount.HasValue && a.BadDebtDate.Value.Year == searchModel.Year.Value
                                       group a by new { a.ProjectId, a.DepartmentId } into g
                                       select new
                                       {
                                           g.Key.ProjectId,
                                           g.Key.DepartmentId,
                                           PlanAmount = g.Sum(a => a.PlanAmount),
                                           TotalAmount = g.Sum(a => a.TotalAmount)
                                       }).ToList();

            var TotalGroup = groupProject;
            TotalGroup.AddRange(groupProjectBadDebt);

            int RAllProjects = 0;
            decimal RAllPlanAmouts = 0;
            decimal RAllReceivables = 0;
            if (!searchModel.IsExport)
            {
                months = new ReportMoneyCollectionResultLabelModel()
                {
                    Title = "Tồn kỳ trước",
                };

                foreach (var item in department)
                {
                    data = new ReportMoneyCollectionResultObjectModel();
                    data.IsExist = true;
                    data.DepartmentId = item.DepartmentId;
                    //Tổng dự án 
                    var TotalProjectT = TotalGroup.Where(a => a.DepartmentId.Equals(item.DepartmentId) && (a.PlanAmount - a.TotalAmount) > 0).Select(r => r.ProjectId).Distinct().Count();

                    data.TotalProject = TotalProjectT;
                    data.ListProject = TotalGroup.Where(a => a.DepartmentId.Equals(item.DepartmentId) && (a.PlanAmount - a.TotalAmount) > 0).Select(r => r.ProjectId).ToList();

                    //phải thu 
                    decimal TotalPlan1 = TotalGroup.Where(a => a.DepartmentId.Equals(item.DepartmentId) && (a.PlanAmount - a.TotalAmount) > 0).ToList().Sum(p => p.PlanAmount.Value);
                    data.TotalPlanAmout = TotalPlan1;
                    //đã thu
                    decimal TotalAmount1 = TotalGroup.Where(a => a.DepartmentId.Equals(item.DepartmentId) && (a.PlanAmount - a.TotalAmount) > 0).ToList().Sum(p => p.TotalAmount);
                    data.TotalAmount = TotalAmount1;

                    //Tổng còn phải thu = Phải thu - Đã thu
                    data.TotalReceivables = data.TotalPlanAmout - data.TotalAmount;

                    RAllProjects = RAllProjects + data.TotalProject;
                    RAllPlanAmouts = RAllPlanAmouts + data.TotalPlanAmout;
                    RAllReceivables = RAllReceivables + data.TotalReceivables;

                    months.Result.Add(data);
                }

                months.AllProjects = RAllProjects;
                months.AllPlanAmouts = RAllPlanAmouts;
                months.AllReceivables = RAllReceivables;

                listMonths.Add(months);
            }

            for (int t = searchModel.StartYear; t <= searchModel.EndYear; t++)
            {
                for (int x = 1; x <= 12; x++)
                {
                    RAllProjects = 0;
                    RAllPlanAmouts = 0;
                    RAllReceivables = 0;

                    months = new ReportMoneyCollectionResultLabelModel()
                    {
                        Title = "Tháng " + x,
                    };

                    foreach (var item in department)
                    {

                        data = new ReportMoneyCollectionResultObjectModel();

                        //Vẫn lấy hết dữ liệu cả nợ xâu

                        data.TotalProject = paymentRecords.Where(a => a.DepartmentId.Equals(item.DepartmentId) && a.PlanPaymentDate.HasValue && a.PlanPaymentDate.Value.Month == x && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year == t).Select(r => r.ProjectId).Distinct().Count();
                        data.ListProject = paymentRecords.Where(a => a.DepartmentId.Equals(item.DepartmentId) && a.PlanPaymentDate.HasValue && a.PlanPaymentDate.Value.Month == x && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year == t).Select(r => r.ProjectId).ToList();

                        //Tổng phải thu
                        data.TotalPlanAmout = paymentRecords.Where(a => a.DepartmentId.Equals(item.DepartmentId) && a.PlanPaymentDate.HasValue && a.PlanPaymentDate.Value.Month == x && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year == t).ToList().Sum(p => p.PlanAmount.Value);
                        //Tổng đã thu
                        data.TotalAmount = paymentRecords.Where(a => a.DepartmentId.Equals(item.DepartmentId) && a.PlanPaymentDate.HasValue && a.PlanPaymentDate.Value.Month == x && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year == t).ToList().Sum(a => a.TotalAmount);
                        //Tổng còn phải thu
                        data.TotalReceivables = data.TotalPlanAmout - data.TotalAmount;

                        data.DepartmentId = item.DepartmentId;
                        data.IsExist = false;
                        data.Month = x;

                        RAllProjects = RAllProjects + data.TotalProject;
                        RAllPlanAmouts = RAllPlanAmouts + data.TotalPlanAmout;
                        RAllReceivables = RAllReceivables + data.TotalReceivables;

                        months.Result.Add(data);
                    }

                    months.AllProjects = RAllProjects;
                    months.AllPlanAmouts = RAllPlanAmouts;
                    months.AllReceivables = RAllReceivables;
                    listMonths.Add(months);
                }
            }

            result.Departments = department.Select(a => a.DepartmentName).ToList();
            result.Months = listMonths;

            return result;
        }


        public SearchResultReportProjectModel<ReportProjectSearchModel> GetListReportProject(ReportProjectSearchModel searchModel)
        {
            if (searchModel.IsExist == true)
            {
                var paymentRecords = (from r in db.Departments.AsNoTracking()
                                      join a in db.Projects.AsNoTracking() on r.Id equals a.DepartmentId
                                      join p in db.Payments.AsNoTracking() on a.Id equals p.ProjectId
                                      join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id
                                      into ca
                                      from c in ca.DefaultIfEmpty()
                                      orderby p.PaymentMilestone
                                      select new
                                      {
                                          DepartmentId = r.Id,
                                          DepartmentName = r.Name,
                                          ProjectId = a.Id,
                                          CustomerId = a.CustomerId,
                                          CustomerCode = c.Code,
                                          CustomerName = c.Name,
                                          PlanPaymentDate = p.PlanPaymentDate,
                                          PlanAmount = p.PlanAmount.HasValue ? p.PlanAmount : 0,
                                          TotalAmount = p.TotalAmount,
                                          IsBadDebt = a.IsBadDebt,
                                          BadDebtDate = a.BadDebtDate,
                                          Name = p.Name,
                                      }).AsQueryable();

                if (!string.IsNullOrEmpty(searchModel.ProjectId))
                {
                    paymentRecords = paymentRecords.Where(u => searchModel.ProjectId.Equals(u.ProjectId));
                }

                if (!string.IsNullOrEmpty(searchModel.CustomerId))
                {
                    paymentRecords = paymentRecords.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
                }

                if (!string.IsNullOrEmpty(searchModel.Name))
                {
                    paymentRecords = paymentRecords.Where(u => searchModel.Name.Equals(u.Name));
                }
                //dữ liệu các năm trước ko có nợ xấu
                var groupProject = (from a in paymentRecords
                                    where a.IsBadDebt == false && a.PlanAmount.HasValue && a.PlanPaymentDate.Value.Year < searchModel.Year
                                    group a by new { a.ProjectId, a.DepartmentId } into g
                                    select new
                                    {
                                        g.Key.ProjectId,
                                        g.Key.DepartmentId,
                                        PlanAmount = g.Sum(a => a.PlanAmount),
                                        TotalAmount = g.Sum(a => a.TotalAmount)
                                    }).ToList();
                //dữ liệu các năm trước là nợ xấu
                var groupProjectBadDebt = (from a in paymentRecords
                                           where a.IsBadDebt == true && a.PlanAmount.HasValue && a.BadDebtDate.Value.Year == searchModel.Year
                                           group a by new { a.ProjectId, a.DepartmentId } into g
                                           select new
                                           {
                                               g.Key.ProjectId,
                                               g.Key.DepartmentId,
                                               PlanAmount = g.Sum(a => a.PlanAmount), //phải thu
                                               TotalAmount = g.Sum(a => a.TotalAmount) //đã thu
                                           }).ToList();

                var TotalGroup = groupProject;
                TotalGroup.AddRange(groupProjectBadDebt);

                var listProject = TotalGroup.Where(a => (a.PlanAmount - a.TotalAmount) > 0).Select(a => a.ProjectId).ToList();

                var listId = listProject.Where(a => searchModel.ListProject.Contains(a)).GroupBy(a => a).ToList();
                ReportMoneyCollectionResultObjectModel data;
                List<ReportProjectSearchModel> ListProject = new List<ReportProjectSearchModel>();
                foreach (var item in listId)
                {
                    data = new ReportMoneyCollectionResultObjectModel();
                    //phải thu
                    decimal TotalPlan1 = TotalGroup.Where(a => a.ProjectId.Equals(item.Key) && (a.PlanAmount - a.TotalAmount) > 0).ToList().Sum(p => p.PlanAmount.Value);
                    data.TotalPlanAmout = TotalPlan1;
                    //đã thu
                    decimal TotalAmount1 = TotalGroup.Where(a => a.ProjectId.Equals(item.Key) && (a.PlanAmount - a.TotalAmount) > 0).ToList().Sum(p => p.TotalAmount);
                    data.TotalAmount = TotalAmount1;

                    //Tổng còn phải thu = Phải thu - Đã thu 
                    data.TotalReceivables = data.TotalPlanAmout - data.TotalAmount;

                    var dataQuery = (from a in db.Projects.AsNoTracking()
                                     join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                     join c in db.Payments.AsNoTracking() on a.Id equals c.ProjectId
                                     join d in db.Customers.AsNoTracking() on a.CustomerId equals d.Id into da
                                     from d in da.DefaultIfEmpty()
                                     where b.Id.Equals(searchModel.DepartmentId) && a.Id.Equals(item.Key)
                                     select new ReportProjectSearchModel
                                     {
                                         ProjectId = a.Id,
                                         ProjectCode = a.Code,
                                         ProjectName = a.Name,
                                         CustomerCode = d.Code,
                                         CustomerId = d.Id,
                                         CustomerName = d.Name,
                                         Status = a.Status,
                                         Receivables = data.TotalPlanAmout, //Phải thu
                                         Exist = data.TotalReceivables, //Còn phải thu
                                         Collected = data.TotalAmount// Đã thu
                                     }).Distinct().ToList();

                    ListProject.AddRange(dataQuery);
                }

                var dataQuerys = ListProject.AsQueryable();

                SearchResultReportProjectModel<ReportProjectSearchModel> result = new SearchResultReportProjectModel<ReportProjectSearchModel>();

                result.TotalItem = listId.Count();
                result.ListResult = SQLHelpper.OrderBy(dataQuerys, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();


                return result;
            }
            else
            {
                var dataQuery = (from a in db.Projects.AsNoTracking()
                                 join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                 join c in db.Payments.AsNoTracking() on a.Id equals c.ProjectId
                                 join d in db.Customers.AsNoTracking() on a.CustomerId equals d.Id into da
                                 from d in da.DefaultIfEmpty()
                                 where b.Id.Equals(searchModel.DepartmentId)
                                 && c.PlanPaymentDate.Value.Year == searchModel.Year
                                 && c.PlanPaymentDate.Value.Month == searchModel.Month
                                 select new ReportProjectSearchModel
                                 {
                                     ProjectId = a.Id,
                                     ProjectCode = a.Code,
                                     PaymentName = c.Name,
                                     ProjectName = a.Name,
                                     CustomerCode = d.Code,
                                     CustomerId = d.Id,
                                     CustomerName = d.Name,
                                     Status = a.Status,
                                     Receivables = c.PlanAmount,
                                     Exist = c.PlanAmount - c.TotalAmount,
                                     Collected = c.TotalAmount
                                 }).AsQueryable();


                if (!string.IsNullOrEmpty(searchModel.ProjectId))
                {
                    dataQuery = dataQuery.Where(u => searchModel.ProjectId.Equals(u.ProjectId));
                }

                if (!string.IsNullOrEmpty(searchModel.CustomerId))
                {
                    dataQuery = dataQuery.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
                }

                SearchResultReportProjectModel<ReportProjectSearchModel> result = new SearchResultReportProjectModel<ReportProjectSearchModel>();

                result.TotalItem = dataQuery.Count();
                result.ListResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

                return result;
            }

        }

        public void UpdateBedDebt(ProjectResultModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var project = db.Projects.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    if (project == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
                    }
                    if (model.IsBadDebt == true)
                    {
                        var Date = DateTime.Now;
                        project.BadDebtDate = Date;
                    }
                    else
                    {
                        project.BadDebtDate = null;
                    }
                    project.IsBadDebt = model.IsBadDebt;
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

        public void UpdateBadDebtDate(ProjectResultModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var project = db.Projects.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    if (project == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
                    }
                    project.BadDebtDate = model.BadDebtDate;
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

        public string ExportExcelReport(MoneyCollectionProjectReportSearchModel model)
        {
            //lấy 1 cái list
            //var listEmp = Report(model);
            //Khởi tạo ex
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BaoCaoThuTienDuAn.xlsx"));
                //khởi tạo sheet 1
                IWorksheet sheet = workbook.Worksheets[0];

                //var total1 = listEmp.Count;
                int i;
                //Tìm vtri đổ dữ liệu

                int x1 = 3;
                int y1 = 1;
                int x2 = 39;
                int y2 = 4;
                IRange range = sheet.Range[x1, y1, x2, y2];
                var start = model.StartYear;
                var end = model.EndYear;
                var countTitle = 0;
                for (i = start; i <= end; i++)
                {
                    if (i < end)
                    {
                        x1 += 37;
                        x2 += 37;
                        IRange rangeCopy = sheet.Range[x1, y1, x2, y2];
                        range.CopyTo(rangeCopy);

                        range = sheet.Range[x1, y1, x2, y2];
                    }

                    model.Year = i;
                    model.StartYear = i;
                    model.EndYear = i;
                    model.IsExport = true;
                    var listEmp = Report(model);
                    if (i == start)
                    {
                        IRange iRangeDataTitle = sheet.FindFirst("<DataTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                        iRangeDataTitle.Text = iRangeDataTitle.Text.Replace("<DataTitle>", string.Empty);

                        var ListTiTle = listEmp.Departments;

                        if (ListTiTle.Count > 1)
                        {
                            sheet.InsertColumn(iRangeDataTitle.Column + 1, ListTiTle.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                        }
                        sheet.ImportArray(ListTiTle.ToArray(), iRangeDataTitle.Row, iRangeDataTitle.Column, false);

                        sheet.Range[1, 1, 1, 4 + listEmp.Departments.Count - 1].Merge();
                        countTitle = 4 + listEmp.Departments.Count - 1;
                    }

                    IRange iRangeDataYear = sheet.FindFirst("<DataYear>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeDataYear.Text = iRangeDataYear.Text.Replace("<DataYear>", i.ToString());

                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                    IRange iRangeDataTotal = sheet.FindFirst("<DataTotal>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeDataTotal.Text = iRangeDataTotal.Text.Replace("<DataTotal>", string.Empty);


                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

                    var Year = i;

                    int index = 0;
                    int column = listEmp.Departments.Count;
                    foreach (var item in listEmp.Months)
                    {
                        var TotalProject = item.Result.Select(a => a.TotalProject.ToString()).ToArray();
                        var TotalPlanAmout = item.Result.Select(a => a.TotalPlanAmout.ToString()).ToArray();
                        var TotalReceivables = item.Result.Select(a => a.TotalReceivables.ToString()).ToArray();

                        sheet.ImportArray(TotalProject, iRangeData.Row + index, iRangeData.Column, false);
                        sheet.Range[iRangeData.Row + index, iRangeData.Column + column].Value = item.AllProjects.ToString();
                        index++;

                        sheet.ImportArray(TotalPlanAmout, iRangeData.Row + index, iRangeData.Column, false);
                        sheet.Range[iRangeData.Row + index, iRangeData.Column + column].Value = item.AllPlanAmouts.ToString();
                        index++;

                        sheet.ImportArray(TotalReceivables, iRangeData.Row + index, iRangeData.Column, false);
                        sheet.Range[iRangeData.Row + index, iRangeData.Column + column].Value = item.AllReceivables.ToString();
                        index++;

                    }
                }
                sheet.Range[4, 5, x2, countTitle].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[4, 5, x2, countTitle].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[4, 5, x2, countTitle].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[4, 5, x2, countTitle].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[4, 5, x2, countTitle].Borders.Color = ExcelKnownColors.Black;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo thu tiền dự án" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();
                //    //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo thu tiền dự án" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public object GetMinYear()
        {
            var data = db.Payments.Min(a => a.PlanPaymentDate);
            var minYear = data.Value.Year;
            var data1 = db.Payments.Max(a => a.PlanPaymentDate);
            var maxYear = data1.Value.Year;
            return new { minYear, maxYear };
        }

        public object GetTotalBadDebt(int year)
        {
            var totalBadDebtInYear = (from a in db.Projects.AsNoTracking()
                                      join b in db.Payments.AsNoTracking() on a.Id equals b.ProjectId into ab
                                      from b in ab.DefaultIfEmpty()
                                      where a.IsBadDebt.Equals(true) && a.BadDebtDate.Value.Year.Equals(year)
                                      select (b.PlanAmount - b.TotalAmount)).Sum();

            var totalBadDebt = (from a in db.Projects.AsNoTracking()
                                join b in db.Payments.AsNoTracking() on a.Id equals b.ProjectId into ab
                                from b in ab.DefaultIfEmpty()
                                where a.IsBadDebt.Equals(true)
                                select b.PlanAmount).Sum();
            return new
            {
                totalBadDebtInYear,
                totalBadDebt,
            };
        }
    }
}
