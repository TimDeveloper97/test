using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.Employees;
using NTS.Model.Repositories;
using NTS.Model.Task;
using NTS.Model.TaskModuleGroup;
using NTS.Model.TaskTimeStandardModel;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.TaskTimeStandards
{
    public class TaskTimeStandardBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public object SearchTaskTimeStandard(TaskTimeStandardSearchModel modelSearch)
        {
            List<AvgModel> listAvgTimeStand = new List<AvgModel>();
            SearchResultTaskModel<TaskTimeStandardResultModel> searchResult = new SearchResultTaskModel<TaskTimeStandardResultModel>();
            string ModuleGrTaskTimeStandId = string.Empty;
            int status = 0;
            if (modelSearch.ModuleGroupId != null && modelSearch.ModuleGroupId != "")
            {
                var dataQuery = (from a in db.Employees.AsNoTracking()
                                 join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id into ab
                                 from b in ab.DefaultIfEmpty()
                                 join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id into bc
                                 from c in bc.DefaultIfEmpty()
                                 join e in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals e.Id into ea
                                 from ae in ea.DefaultIfEmpty()
                                 where b.Id.Equals(modelSearch.DepartmentId)
                                 orderby a.Code
                                 select new TaskTimeStandardResultModel()
                                 {
                                     EmployeeName = a.Name,
                                     DepartmantName = b.Name,
                                     EmployeeCode = a.Code,
                                     WorkTypeName = ae.Name,
                                     EmployeeId = a.Id,
                                     SBUId = c.Id,
                                     DepartmantId = b.Id,
                                     SBUName = c.Name,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuery = dataQuery.Where(u => u.EmployeeName.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.SBUId))
                {
                    dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
                {
                    dataQuery = dataQuery.Where(u => u.DepartmantId.Equals(modelSearch.DepartmentId));
                }


                var workTimeModule = (from a in db.TaskModuleGroups.AsNoTracking()
                                      join b in db.Tasks.AsNoTracking() on a.TaskId equals b.Id
                                      where a.ModuleGroupId.Equals(modelSearch.ModuleGroupId)
                                      orderby b.Name
                                      select new TasksResultModel()
                                      {
                                          Id = a.Id,
                                          Name = b.Name,
                                          Type = b.Type,
                                          TaskId = b.Id,
                                          Description = b.Description,

                                      }).AsQueryable().ToList();

                var moduleGrTaskTimeStandId = db.ModuleGroupTimeStandards.AsNoTracking().Where(a => a.DepartmentId.Equals(modelSearch.DepartmentId) && a.ModuleGroupId.Equals(modelSearch.ModuleGroupId)).FirstOrDefault();
                if (moduleGrTaskTimeStandId != null)
                {
                    ModuleGrTaskTimeStandId = moduleGrTaskTimeStandId.Id;
                    status = moduleGrTaskTimeStandId.Status;
                }

                List<TaskTimeStandardResultModel> employees = dataQuery.ToList();
                List<string> listEmployId = new List<string>();
                listEmployId = employees.Select(a => a.EmployeeId).ToList();
                var taskTimeStandards = db.TaskTimeStandards.AsNoTracking();
                var task = db.Tasks.AsNoTracking();
                foreach (var item in employees)
                {
                    var taskEmployee = taskTimeStandards.Where(r => r.ModuleGroupId.Equals(modelSearch.ModuleGroupId) && r.EmployeeId.Equals(item.EmployeeId));
                    item.ListWorkType = (from t in workTimeModule
                                         join s in taskEmployee on t.TaskId equals s.TaskId into ts
                                         from stn in ts.DefaultIfEmpty()
                                         select new TaskStandardTypeModel
                                         {
                                             Name = t.Name,
                                             TaskId = t.TaskId,
                                             TimeStandard = stn != null ? stn.TimeStandard : 0
                                         }).ToList();

                }
                decimal total;
                decimal _temp;
                AvgModel avgModel;


                foreach (var item in workTimeModule)
                {
                    avgModel = new AvgModel();
                    var temp = db.TaskTimeStandards.AsNoTracking().Where(a => listEmployId.Contains(a.EmployeeId) && a.TimeStandard != 0 && a.TaskId.Equals(item.TaskId) && a.ModuleGroupId.Equals(modelSearch.ModuleGroupId)).ToList();
                    if (temp.Count == 0)
                    {
                        total = 0;
                        avgModel.Avg = 0;
                    }
                    else
                    {
                        total = temp.Sum(a => a.TimeStandard);
                        _temp = total / temp.Count;
                        avgModel.Avg = Math.Round(_temp, 2);
                    }
                    avgModel.TaskId = item.TaskId;
                    avgModel.TaskName = item.Name;
                    listAvgTimeStand.Add(avgModel);
                }

                searchResult.TotalItem = workTimeModule.Count();
                searchResult.ListResult = employees;
            }
            return new { searchResult, listAvgTimeStand, ModuleGrTaskTimeStandId, status };
        }
        public TaskTimeStandardModel GetTaskTimeStandardInfo(TaskTimeStandardModel model)
        {
            var resultInfo = db.TaskTimeStandards.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new TaskTimeStandardModel()
            {
                Id = p.Id,
                SBUId = p.SBUId,
                DepartmantId = p.DepartmentId,
                EmployeeId = p.EmployeeId,
                TimeStandard = p.TimeStandard,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TaskTimeStandard);
            }
            return resultInfo;
        }
        public void CreateTaskTimeStandard(TaskTimeStandardModel model)
        {
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.TaskTimeStandard newTaskTimeStandard = new NTS.Model.Repositories.TaskTimeStandard
                    {
                        Id = Guid.NewGuid().ToString(),
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmantId,
                        EmployeeId = model.EmployeeId,
                        UpdateDate = DateTime.Now,
                        TimeStandard = model.TimeStandard,
                    };

                    db.TaskTimeStandards.Add(newTaskTimeStandard);

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
        public void UpdateTaskTimeStandard(TaskTimeStandardModel model)
        {
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newTaskTimeStandard = db.TaskTimeStandards.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();


                    newTaskTimeStandard.TimeStandard = model.TimeStandard;
                    newTaskTimeStandard.UpdateDate = DateTime.Now;
                    newTaskTimeStandard.SBUId = model.SBUId;
                    newTaskTimeStandard.DepartmentId = model.DepartmantId;
                    newTaskTimeStandard.EmployeeId = model.EmployeeId;

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
        public void DeleteTaskTimeStandard(TaskTimeStandardModel model)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _taskTimeStandard = db.TaskTimeStandards.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_taskTimeStandard == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TaskTimeStandard);
                    }

                    db.TaskTimeStandards.Remove(_taskTimeStandard);

                    var NameOrCode = _taskTimeStandard.TimeStandard.ToString();

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
        private void CheckExistedForAdd(TaskTimeStandardModel model)
        {
            if (db.TaskTimeStandards.AsNoTracking().Where(o => o.EmployeeId.Equals(model.EmployeeId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0020, TextResourceKey.TaskTimeStandard);
            }
        }

        private void CheckExistedForUpdate(TaskTimeStandardModel model)
        {
            if (db.TaskTimeStandards.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.EmployeeId.Equals(model.EmployeeId)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0020, TextResourceKey.TaskTimeStandard);
            }
        }

        public void CreateListTaskTim(TaskTimeStandardResultModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    foreach (var item in model.ListData)
                    {
                        var listTaskModuleGroup = db.TaskTimeStandards.Where(r => r.EmployeeId.Equals(item.EmployeeId)).ToList();
                        if (listTaskModuleGroup.Count > 0)
                        {
                            db.TaskTimeStandards.RemoveRange(listTaskModuleGroup);
                        }
                        foreach (var it in item.ListWorkType)
                        {
                            NTS.Model.Repositories.TaskTimeStandard newTaskTimeStandard = new NTS.Model.Repositories.TaskTimeStandard
                            {
                                Id = Guid.NewGuid().ToString(),
                                SBUId = item.SBUId,
                                DepartmentId = item.DepartmantId,
                                EmployeeId = item.EmployeeId,
                                UpdateDate = DateTime.Now,
                                TimeStandard = it.TimeStandard,
                                TaskId = it.TaskId,
                                ModuleGroupId = model.ModuleGroupId,
                            };
                            db.TaskTimeStandards.Add(newTaskTimeStandard);
                        }
                    }

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_TaskTimeStand, string.Empty, string.Empty, string.Empty, string.Empty);

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

        public string CalculateAverageTaskTimeStandard(CalculateAverageTaskTimeStandardModel model)
        {
            var taskTimeStandards = db.TaskTimeStandards;
            var path = string.Empty;
            List<CalculateAverageTaskTimeStandardResultModel> result = new List<CalculateAverageTaskTimeStandardResultModel>();

            //var plans = (from a in db.Employees.AsNoTracking()
            //             join b in db.Plans.AsNoTracking() on a.Id equals b.ResponsiblePersion
            //             join c in db.ProjectProducts.AsNoTracking() on b.ProjectProductId equals c.Id
            //             join d in db.Modules.AsNoTracking() on c.ModuleId equals d.Id
            //             join e in db.ModuleGroups.AsNoTracking() on d.ModuleGroupId equals e.Id
            //             join f in db.Tasks.AsNoTracking() on b.TaskId equals f.Id
            //             where a.DepartmentId.Equals(model.DepartmentId) && b.ExecutionTime > 0 && string.IsNullOrEmpty(b.ReferenceId)
            //             select new
            //             {
            //                 EmployeeId = a.Id,
            //                 EmployeeName = a.Name,
            //                 EmployeeCode = a.Code,
            //                 ModuleGroupId = e.Id,
            //                 ModuleGroupCode = e.Code,
            //                 PlanId = b.Id,
            //                 b.RealStartDate,
            //                 b.RealEndDate,
            //                 b.ExecutionTime,
            //                 b.TaskId,
            //                 f.Name
            //             }).AsQueryable().ToList();

            //if (model.DateFrom.HasValue)
            //{
            //    plans = plans.Where(t => t.RealEndDate.HasValue && t.RealEndDate.Value > model.DateFrom.Value).ToList();
            //}

            //if (model.DateTo.HasValue)
            //{
            //    plans = plans.Where(t => t.RealEndDate.HasValue && t.RealEndDate.Value < model.DateTo.Value).ToList();
            //}

            //if (!string.IsNullOrEmpty(model.Name))
            //{
            //    plans = plans.Where(t => model.Name.Trim().ToLower().Equals(t.Name.Trim().ToLower())).ToList();
            //}

            //var groupPlans = plans.GroupBy(t => new { t.EmployeeId, t.TaskId, t.ModuleGroupCode }).ToList();
            //if (groupPlans.Count > 0)
            //{
            //    foreach (var item in groupPlans)
            //    {
            //        var total = item.Sum(t => t.ExecutionTime);
            //        CalculateAverageTaskTimeStandardResultModel rs = new CalculateAverageTaskTimeStandardResultModel()
            //        {
            //            EmployeeId = item.Key.EmployeeId,
            //            EmployeeName = item.FirstOrDefault().EmployeeName,
            //            EmployeeCode = item.FirstOrDefault().EmployeeCode,
            //            ModuleGroupId = item.FirstOrDefault().ModuleGroupId,
            //            ModuleGroupCode = item.FirstOrDefault().ModuleGroupCode,
            //            TaskId = item.Key.TaskId,
            //            TaskName = item.FirstOrDefault().Name,
            //            Average = Convert.ToDecimal(String.Format("{0:0.00}", total / item.Count()))
            //        };
            //        result.Add(rs);
            //    }

            //}

            if (result.Count > 0)
            {
                if (model.IsCalculate)
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in result)
                            {
                                var taskTimeStandard = taskTimeStandards.FirstOrDefault(t => t.EmployeeId.Equals(item.EmployeeId) && t.TaskId.Equals(item.TaskId) && t.ModuleGroupId.Equals(item.ModuleGroupId));
                                if (taskTimeStandard != null)
                                {
                                    taskTimeStandard.TimeStandard = item.Average;
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
                }
                if (model.IsExportExcel)
                {
                    path = ExportExcel(result);
                }
            }
            return path;
        }

        public string ExportExcel(List<CalculateAverageTaskTimeStandardResultModel> model)
        {
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/CalculateAverage.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = model.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = model.Select(t => new
                {
                    t.EmployeeName,
                    t.EmployeeCode,
                    t.ModuleGroupCode,
                    t.TaskName,
                    t.Average
                });

                listExport = listExport.OrderBy(t => t.EmployeeName);

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 5].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Trung bình thời gian công việc tiêu chuẩn" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Trung bình thời gian công việc tiêu chuẩn" + ".xlsx";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public void ImportTaskTimeStandard(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string employee, moduleGroup, timeStandard, task;
            #region
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet;
            var worksheet = workbook.Worksheets.Count;
            string name = string.Empty;
            int rowCount = 0;
            int colCount = 0;

            List<TaskTimeStandard> list = new List<TaskTimeStandard>();
            TaskTimeStandard itemC;

            List<string> rowCheckEmployee = new List<string>();
            List<int> rowCheckModuleGroup = new List<int>();
            List<int> rowCheckTaskTimeStandard = new List<int>();
            List<int> rowCheckTask = new List<int>();
            List<ReportErrorModel> listCheckModuleGroup = new List<ReportErrorModel>();

            var listEployee = db.Employees.AsNoTracking().ToList();
            var listDeparment = db.Departments.AsNoTracking().ToList();
            var listSBU = db.SBUs.AsNoTracking().ToList();
            var listTaskModuleGroup = db.TaskModuleGroups.AsNoTracking().ToList();
            var listModuleGroup = db.ModuleGroups.AsNoTracking().ToList();
            var listTask = db.Tasks.AsNoTracking().ToList();
            var listTaskTimeStandards = db.TaskTimeStandards.AsNoTracking().ToList();
            EmployeeModel employeeData;

            try
            {
                for (int a = 0; a < worksheet; a++)
                {
                    employeeData = new EmployeeModel();
                    sheet = workbook.Worksheets[a];
                    rowCount = sheet.Rows.Count();
                    colCount = sheet.Columns.Count();
                    name = sheet.Name;
                    employee = name;


                    if (!string.IsNullOrEmpty(employee))
                    {
                        employeeData = (from b in listEployee
                                        join c in listDeparment on b.DepartmentId equals c.Id
                                        join d in listSBU on c.SBUId equals d.Id
                                        where b.Code.ToUpper().Equals(employee.Trim().ToUpper())
                                        select new EmployeeModel
                                        {
                                            Id = b.Id,
                                            DepartmentId = b.DepartmentId,
                                            SBUID = c.SBUId
                                        }).FirstOrDefault();
                        if (employeeData == null)
                        {
                            rowCheckEmployee.Add(employee);
                            continue;
                        }
                    }

                    for (int i = 4; i <= rowCount; i++)
                    {
                        moduleGroup = sheet[i, 5].Value;
                        if (string.IsNullOrEmpty(moduleGroup))
                        {
                            continue;
                        }
                        var module = listModuleGroup.FirstOrDefault(e => e.Code.ToUpper().Equals(moduleGroup.Trim().ToUpper()));
                        if (module == null)
                        {
                            rowCheckModuleGroup.Add(i);
                            continue;
                        }

                        for (int y = 7; y <= colCount; y++)
                        {
                            itemC = new TaskTimeStandard();
                            itemC.Id = Guid.NewGuid().ToString();

                            timeStandard = sheet[i, y].Value;
                            task = sheet[3, y].Value;

                            if (string.IsNullOrEmpty(timeStandard) || string.IsNullOrEmpty(timeStandard))
                            {
                                continue;
                            }

                            itemC.ModuleGroupId = module.Id;

                            try
                            {
                                itemC.TimeStandard = Convert.ToDecimal(timeStandard);
                            }
                            catch
                            {
                                rowCheckTaskTimeStandard.Add(i);
                                continue;
                            }

                            var task1 = listTask.FirstOrDefault(e => e.Name.ToUpper().Equals(task.Trim().ToUpper()));
                            if (task1 != null)
                            {
                                itemC.TaskId = task1.Id;
                            }
                            else
                            {
                                rowCheckTask.Add(y);
                                continue;
                            }

                            itemC.EmployeeId = employeeData.Id;
                            itemC.SBUId = employeeData.SBUID;
                            itemC.DepartmentId = employeeData.DepartmentId;
                            itemC.UpdateDate = DateTime.Now;

                            var taskTimeStandard = listTaskTimeStandards.FirstOrDefault(e => e.EmployeeId.Equals(itemC.EmployeeId) && e.ModuleGroupId.Equals(itemC.ModuleGroupId) && e.TaskId.Equals(itemC.TaskId));
                            if (taskTimeStandard != null)
                            {
                                taskTimeStandard.TimeStandard = itemC.TimeStandard;
                            }
                            else
                            {
                                var check = listTaskModuleGroup.FirstOrDefault(e => e.TaskId.Equals(itemC.TaskId) && e.ModuleGroupId.Equals(itemC.ModuleGroupId));
                                if (check != null)
                                {
                                    list.Add(itemC);
                                }
                            }
                        }
                    }
                    if (rowCheckModuleGroup.Count > 0)
                    {
                        listCheckModuleGroup.Add(new ReportErrorModel
                        {
                            Name = name,
                            ListCheck = rowCheckModuleGroup
                        });
                        rowCheckModuleGroup = new List<int>();
                    }
                }

                #endregion

                if (rowCheckEmployee.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhân viên <" + string.Join(", ", rowCheckEmployee) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }
                if (listCheckModuleGroup.Count > 0)
                {
                    string message = string.Empty;
                    foreach (var item in listCheckModuleGroup)
                    {
                        message += "- Nhân viên " + item.Name + ":<br>" + "&nbsp; + Mã nhóm module dòng <" + string.Join(", ", item.ListCheck) + "> không tồn tại!<br>";
                    }
                    throw NTSException.CreateInstance(message);
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }
                if (rowCheckTask.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại công việc cột <" + string.Join(", ", rowCheckTask) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }
                if (rowCheckTaskTimeStandard.Count > 0)
                {
                    throw NTSException.CreateInstance("Thời gian tiêu chuẩn dòng <" + string.Join(", ", rowCheckTaskTimeStandard) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                db.TaskTimeStandards.AddRange(list);
                db.SaveChanges();
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
        }

        public void ImportExcelTaskTimeStandard(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string employeeName, employeeCode, moduleGroupCode, taskName, average;
            var employees = db.Employees.AsNoTracking();
            var moduleGroups = db.ModuleGroups.AsNoTracking();
            var tasks = db.Tasks.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<int> rowErrors = new List<int>();
            List<int> employeeErrors = new List<int>();
            List<int> employeeEmptyErrors = new List<int>();
            List<int> moduleGroupErrors = new List<int>();
            List<int> moduleGroupEmptyErrors = new List<int>();
            List<int> taskErrors = new List<int>();
            List<int> taskEmptyErrors = new List<int>();
            List<int> averageErrors = new List<int>();
            try
            {
                for (int i = 2; i <= rowCount; i++)
                {
                    employeeName = sheet[i, 1].Value;
                    employeeCode = sheet[i, 2].Value;
                    moduleGroupCode = sheet[i, 3].Value;
                    taskName = sheet[i, 4].Value;
                    average = sheet[i, 5].Value;

                    var employeeId = string.Empty;
                    var moduleGroupId = string.Empty;
                    var taskId = string.Empty;
                    decimal avg = 0;

                    try
                    {
                        if (!string.IsNullOrEmpty(employeeCode))
                        {
                            var employee = employees.FirstOrDefault(t => t.Code.ToUpper().Trim().Equals(employeeCode.ToUpper().Trim()));
                            if (employee != null)
                            {
                                employeeId = employee.Id;
                            }
                            else
                            {
                                employeeErrors.Add(i);
                            }
                        }
                        else
                        {
                            employeeEmptyErrors.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowErrors.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(moduleGroupCode))
                        {
                            var moduleGroup = moduleGroups.FirstOrDefault(t => t.Code.ToUpper().Trim().Equals(moduleGroupCode.ToUpper().Trim()));
                            if (moduleGroup != null)
                            {
                                moduleGroupId = moduleGroup.Id;
                            }
                            else
                            {
                                moduleGroupErrors.Add(i);
                            }
                        }
                        else
                        {
                            moduleGroupEmptyErrors.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowErrors.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(taskName))
                        {
                            var task = tasks.FirstOrDefault(t => t.Name.ToUpper().Trim().Equals(taskName.ToUpper().Trim()));
                            if (task != null)
                            {
                                taskId = task.Id;
                            }
                            else
                            {
                                taskErrors.Add(i);
                            }
                        }
                        else
                        {
                            taskEmptyErrors.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowErrors.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(average))
                    {
                        avg = decimal.Parse(average);
                    }
                    else
                    {
                        averageErrors.Add(i);
                    }

                    if(!string.IsNullOrEmpty(employeeCode) && !string.IsNullOrEmpty(moduleGroupCode) && !string.IsNullOrEmpty(taskName))
                    {

                        var taskTimeStandard = db.TaskTimeStandards.Where(t => t.ModuleGroupId.Equals(moduleGroupId) && t.EmployeeId.Equals(employeeId) && t.TaskId.Equals(taskId));
                        if(taskTimeStandard.Count() > 0)
                        {
                            foreach (var item in taskTimeStandard)
                            {
                                item.TimeStandard = avg;
                            }
                        }    
                        
                    }    
                }

                #endregion

                if (rowErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Dòng <" + string.Join(", ", rowErrors) + "> có lỗi xảy ra trong quá trình xử lý!");
                }

                if (employeeErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhân viên dòng <" + string.Join(", ", employeeErrors) + "> không tồn tại!");
                }

                if (moduleGroupErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhóm module dòng <" + string.Join(", ", moduleGroupErrors) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (taskErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên công việc dòng <" + string.Join(", ", taskErrors) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (employeeEmptyErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhân viên dòng <" + string.Join(", ", employeeEmptyErrors) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (moduleGroupEmptyErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhóm module dòng <" + string.Join(", ", moduleGroupEmptyErrors) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (averageErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Thời gian trung bình dòng <" + string.Join(", ", averageErrors) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (taskEmptyErrors.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên công việc dòng <" + string.Join(", ", taskEmptyErrors) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                db.SaveChanges();
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
    }
}
