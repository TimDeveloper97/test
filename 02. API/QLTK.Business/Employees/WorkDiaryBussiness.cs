using NTS.Caching;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.Employees;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using NTS.Model.WorkDiary;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace QLTK.Business.WorkDiarys
{
    public class WorkDiaryBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<WorkDiaryModel> SearchWorkDiary(WorkDiarySearchModel modelSearch)
        {
            SearchResultModel<WorkDiaryModel> searchResult = new SearchResultModel<WorkDiaryModel>();

            var dataQuery = (from a in db.WorkDiaries.AsNoTracking()
                             join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join k in db.Users.AsNoTracking() on a.CreateBy equals k.Id
                             join d in db.Employees.AsNoTracking() on k.EmployeeId equals d.Id
                             join g in db.Departments.AsNoTracking() on a.DepartmentId equals g.Id
                             join h in db.SBUs.AsNoTracking() on a.SBUId equals h.Id
                             orderby a.WorkDate descending
                             select new WorkDiaryModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 WorkDate = a.WorkDate,
                                 TotalTime = a.TotalTime,
                                 Done = a.Done,
                                 Address = a.Address,
                                 EmployeeName = d.Name,
                                 ProjectId = a.ProjectId,
                                 ProjectName = ca != null ? ca.Name : "Không thuộc dự án nào",
                                 ProjectCode = ca != null ? ca.Code : "",
                                 DepartmentId = g.Id,
                                 SBUId = h.Id,
                                 SBUName = h.Name,
                                 EmployeeCode = d.Code,
                                 DepartmentName = g.Name,
                                 EmployeeId = d.Id,
                                 CreateBy = a.CreateBy,
                                 EndTime = a.EndTime,
                                 StartTime = a.StartTime
                             }).AsQueryable();


            if (modelSearch.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.WorkDate >= modelSearch.DateFrom);
            }

            if (modelSearch.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.WorkDate <= modelSearch.DateTo);
            }

            if (!string.IsNullOrEmpty(modelSearch.EmployeeName))
            {
                dataQuery = dataQuery.Where(u => u.EmployeeName.ToUpper().Contains(modelSearch.EmployeeName.ToUpper()) || u.EmployeeCode.ToUpper().Contains(modelSearch.EmployeeName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => u.EmployeeId.Equals(modelSearch.EmployeeId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            searchResult.TotalItem = dataQuery.Count();


            if (modelSearch.IsExport)
            {
                searchResult.ListResult = dataQuery.ToList();
            }
            else
            {
                searchResult.ListResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }

            return searchResult;
        }

        public void AddWorkDiary(WorkDiaryModel model, string userLoginId, string employeeId, bool outOfDate)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (model.ProjectId == "-")
                {
                    model.ProjectId = null;
                }

                try
                {
                    var day = GetConfigDayWorkDiary();
                    
                    DateTime check = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (day > 0 && (startMonth.AddMonths(-1).Date > model.WorkDate.Date || (DateTime.Now.Date> check.Date && model.WorkDate.Date < startMonth.Date && !outOfDate )))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0074, check.ToStringDDMMYY());
                    }


                    WorkDiary newWorkSkill = new WorkDiary()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = model.ProjectId,
                        Name = model.Name,
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        ObjectId = model.ObjectId,
                        EmployeeId = employeeId,
                        ObjectType = model.ObjectType,
                        Note = model.Note,
                        WorkDate = model.WorkDate,
                        TotalTime = model.TotalTime,
                        Done = model.Done,
                        Address = model.Address,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        CreateBy = userLoginId,
                        UpdateBy = userLoginId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    };

                    db.WorkDiaries.Add(newWorkSkill);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, newWorkSkill.Name, newWorkSkill.Id, Constants.LOG_WorkDiary);
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

        public void DeleteWorkDiary(WorkDiaryModel model, string userLoginId, bool outOfDate)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var workSkill = db.WorkDiaries.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (workSkill == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkDiary);
                    }

                    var day = GetConfigDayWorkDiary();

                    DateTime check = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (day > 0 && (startMonth.AddMonths(-1).Date > workSkill.WorkDate.Date || (DateTime.Now.Date > check.Date && workSkill.WorkDate.Date < startMonth.Date && !outOfDate)))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0074, check.ToStringDDMMYY());
                    }


                    if (!string.IsNullOrEmpty(workSkill.ObjectId) && !string.IsNullOrEmpty(workSkill.ProjectId))
                    {
                        var plan = db.Plans.FirstOrDefault(i => i.Id.Equals(workSkill.ObjectId) && i.ProjectId.Equals(workSkill.ProjectId));
                        if (plan != null)
                        {
                            //plan.ExecutionTime = plan.ExecutionTime - workSkill.TotalTime;

                            //var listPlanReference = db.Plans.Where(t => plan.Id.Equals(t.ReferenceId));
                            //if (listPlanReference.Count() > 0)
                            //{
                            //    foreach (var item in listPlanReference)
                            //    {
                            //        item.ExecutionTime = plan.ExecutionTime;
                            //    }
                            //}
                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkDiaryHistoryModel>(workSkill);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_WorkDiary, workSkill.Id, workSkill.Name, jsonBefor);

                    db.WorkDiaries.Remove(workSkill);
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

        public void UpdateWorkDiary(WorkDiaryModel model, string userLoginId, bool outOfDate)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var day = GetConfigDayWorkDiary();

                    DateTime check = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (day > 0 && (startMonth.AddMonths(-1).Date > model.WorkDate.Date || (DateTime.Now.Date > check.Date && model.WorkDate.Date < startMonth.Date && !outOfDate)))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0074, check.ToStringDDMMYY());
                    }

                    var newWorkPlace = db.WorkDiaries.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkDiaryHistoryModel>(newWorkPlace);
                    if (newWorkPlace == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkDiary);
                    }

                    if (!newWorkPlace.CreateBy.Equals(userLoginId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.WorkDiary);
                    }

                    if (model.ProjectId == "-")
                    {
                        model.ProjectId = null;
                    }

                    newWorkPlace.Name = model.Name;
                    newWorkPlace.Note = model.Note;
                    newWorkPlace.WorkDate = model.WorkDate;
                    newWorkPlace.TotalTime = model.TotalTime;
                    newWorkPlace.Address = model.Address;
                    newWorkPlace.Done = model.Done;
                    newWorkPlace.StartTime = model.StartTime;
                    newWorkPlace.EndTime = model.EndTime;
                    newWorkPlace.UpdateBy = userLoginId;
                    newWorkPlace.CreateDate = DateTime.Now;
                    newWorkPlace.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkDiaryHistoryModel>(newWorkPlace);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_WorkDiary, newWorkPlace.Id, newWorkPlace.Name, jsonBefor, jsonApter);

                    if (!string.IsNullOrEmpty(model.ObjectId) && !string.IsNullOrEmpty(model.ProjectId))
                    {
                        var plan = db.Plans.FirstOrDefault(i => i.Id.Equals(model.ObjectId) && i.ProjectId.Equals(model.ProjectId));
                        if (plan != null)
                        {
                            var planWorks = db.WorkDiaries.Where(r => plan.Id.Equals(r.ObjectId) && !r.Id.Equals(model.Id)).OrderByDescending(o => o.WorkDate);

                            if (planWorks.Count() > 0)
                            {
                                //plan.ExecutionTime = model.TotalTime + planWorks.Sum(r => r.TotalTime);
                                //plan.Done = planWorks.FirstOrDefault().WorkDate > model.WorkDate ? planWorks.FirstOrDefault().Done : model.Done;
                            }
                            else
                            {
                                //plan.ExecutionTime = model.TotalTime;
                                //plan.Done = model.Done;
                            }

                            //var listPlanReference = db.Plans.Where(t => plan.Id.Equals(t.ReferenceId));
                            //if (listPlanReference.Count() > 0)
                            //{
                            //    foreach (var item in listPlanReference)
                            //    {
                            //        item.Done = plan.Done;
                            //        item.ExecutionTime = plan.ExecutionTime;
                            //    }
                            //}
                        }
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

        public WorkDiaryModel GetByIdWorkDiary(WorkDiaryModel model)
        {
            var resultInfo = (from p in db.WorkDiaries.AsNoTracking()
                              where p.Id.Equals(model.Id)
                              join s in db.SBUs.AsNoTracking() on p.SBUId equals s.Id
                              join d in db.Departments.AsNoTracking() on p.DepartmentId equals d.Id
                              join e in db.Employees.AsNoTracking() on p.EmployeeId equals e.Id
                              select new WorkDiaryModel()
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  ProjectId = p.ProjectId,
                                  SBUId = p.SBUId,
                                  SBUName = s.Name,
                                  Note = p.Note,
                                  DepartmentId = p.DepartmentId,
                                  DepartmentName = d.Name,
                                  ObjectId = p.ObjectId,
                                  EmployeeId = p.EmployeeId,
                                  EmployeeName = e.Name,
                                  EmployeeCode = e.Code,
                                  WorkDate = p.WorkDate,
                                  Address = p.Address,
                                  CreateBy = p.CreateBy,
                                  TotalTime = p.TotalTime,
                                  Done = p.Done,
                                  StartTime = p.StartTime,
                                  EndTime = p.EndTime
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkPlace);
            }

            if (resultInfo.ProjectId == null)
            {
                resultInfo.ProjectId = "-";
            }

            return resultInfo;
        }

        public WorkDiaryModel GetWorkDiaryView(string id)
        {
            var resultInfo = (from p in db.WorkDiaries.AsNoTracking()
                              where p.Id.Equals(id)
                              join s in db.SBUs.AsNoTracking() on p.SBUId equals s.Id
                              join d in db.Departments.AsNoTracking() on p.DepartmentId equals d.Id
                              join e in db.Employees.AsNoTracking() on p.EmployeeId equals e.Id
                              join t in db.Projects.AsNoTracking() on p.ProjectId equals t.Id into pt
                              from ptn in pt.DefaultIfEmpty()
                              select new WorkDiaryModel()
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  ProjectCode = ptn != null ? ptn.Code : "Không thuộc thự án",
                                  ProjectName = ptn != null ? ptn.Code : "Không thuộc thự án",
                                  SBUName = s.Name,
                                  Note = p.Note,
                                  DepartmentName = d.Name,
                                  ObjectId = p.ObjectId,
                                  EmployeeName = e.Name,
                                  EmployeeCode = e.Code,
                                  WorkDate = p.WorkDate,
                                  Address = p.Address,
                                  CreateBy = p.CreateBy,
                                  TotalTime = p.TotalTime,
                                  Done = p.Done,
                                  StartTime = p.StartTime,
                                  EndTime = p.StartTime
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkDiary);
            }

            return resultInfo;
        }

        public string ExportExcel(WorkDiarySearchModel model)
        {
            model.IsExport = true;

            var result = SearchWorkDiary(model);


            if (result.ListResult.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/workdiary.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = result.ListResult.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = result.ListResult.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.TotalTime,
                    work = DateTimeHelper.ToStringDDMMYY(a.WorkDate),
                    a.Done,
                    a.StartTime,
                    a.EndTime,
                    a.Address,
                    a.EmployeeName,
                    a.EmployeeCode,
                    a.DepartmentName,
                    a.SBUName,
                    a.ProjectName,
                });

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders.Color = ExcelKnownColors.Black;
                sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 13].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách nhật kí công việc" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách nhật kí công việc" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public SearchResultModel<EmployessWordDiaryModel> GetCbbEmployeeByUser(String UserId)
        {
            SearchResultModel<EmployessWordDiaryModel> searchResultModel = new SearchResultModel<EmployessWordDiaryModel>();
            try
            {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                 join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                 join d in db.Users.AsNoTracking() on a.Id equals d.EmployeeId
                                 orderby a.Name ascending
                                 where d.Id.Equals(UserId)
                                 select new EmployessWordDiaryModel()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     DepartmentId = b.Id,
                                     Code = a.Code,
                                     SBUId = c.Id,
                                     SBUName = c.Name,
                                     DepartmentName = b.Name,
                                 }).AsQueryable();
                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }

        public SearchResultModel<ProjectWordDiaryModel> GetCbbprojectByUser(String UserId)
        {
            SearchResultModel<ProjectWordDiaryModel> searchResultModel = new SearchResultModel<ProjectWordDiaryModel>();
            try
            {
                var ListModel = (from a in db.Projects.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ProjectWordDiaryModel()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).AsQueryable();

                searchResultModel.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResultModel;
        }

        public object SearchWorkingTime(SearchWorkDiaryModel search)
        {
            SearchResultModel<WorkDiaryModel> searchResult = new SearchResultModel<WorkDiaryModel>();

            var dateStart = new DateTime(search.DateStart.Year, search.DateStart.Month, 1);

            var dateMonth = dateStart.AddMonths(1);
            var dateEnd = dateMonth.AddDays(-1);

            // Lấy list công việc có objectId == null trong tháng truyền xuống
            var dataQuery = (from a in db.WorkDiaries.AsNoTracking()
                             where string.IsNullOrEmpty(a.ObjectId) && search.DateStart <= a.WorkDate && a.WorkDate <= search.DateEnd
                             select new WorkDiaryTimeModel()
                             {
                                 Id = a.Id,
                                 TotalTime = a.TotalTime,
                                 WorkDate = a.WorkDate,
                                 DepartmentId = a.DepartmentId,
                                 SBUId = a.SBUId,
                                 EmployeeId = a.EmployeeId,
                                 Name = a.Name,
                                 ObjectId = a.ObjectId,
                                 ProjectId = a.ProjectId,
                                 CreateBy = a.CreateBy
                             }).ToList();

            if (!string.IsNullOrEmpty(search.DepartmentId))
            {
                dataQuery = dataQuery.Where(r => r.DepartmentId.Equals(search.DepartmentId)).ToList();
            }

            if (!string.IsNullOrEmpty(search.SBUId))
            {
                dataQuery = dataQuery.Where(r => r.SBUId.Equals(search.SBUId)).ToList();
            }


            if (!string.IsNullOrEmpty(search.EmployeeId))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeId.Equals(search.EmployeeId)).ToList();
            }

            // Lấy list công việc có objectId != null trong tháng truyền xuống
            //var dataListWork = (from a in db.WorkDiaries.AsNoTracking()
            //                    where !string.IsNullOrEmpty(a.ObjectId) && search.DateStart <= a.WorkDate && a.WorkDate <= search.DateEnd
            //                    join b in db.Plans.AsNoTracking() on a.ObjectId equals b.Id
            //                    join c in db.ProjectProducts.AsNoTracking() on b.ProjectProductId equals c.Id
            //                    join d in db.Modules.AsNoTracking() on c.ModuleId equals d.Id into cd
            //                    from cdn in cd.DefaultIfEmpty()
            //                    select new WorkDiaryTimeModel()
            //                    {
            //                        Id = a.Id,
            //                        TotalTime = a.TotalTime,
            //                        WorkDate = a.WorkDate,
            //                        Name = a.Name,
            //                        DepartmentId = a.DepartmentId,
            //                        SBUId = a.SBUId,
            //                        EmployeeId = a.EmployeeId,
            //                        ObjectId = a.ObjectId,
            //                        ProjectId = a.ProjectId,
            //                        CreateBy = a.CreateBy,
            //                        ModuleCode = cdn != null ? cdn.Code : string.Empty,
            //                        ModuleName = cdn != null ? cdn.Name : string.Empty,
            //                    }).ToList();

            //if (!string.IsNullOrEmpty(search.DepartmentId))
            //{
            //    dataListWork = dataListWork.Where(r => r.DepartmentId.Equals(search.DepartmentId)).ToList();
            //}

            //if (!string.IsNullOrEmpty(search.SBUId))
            //{
            //    dataListWork = dataListWork.Where(r => r.SBUId.Equals(search.SBUId)).ToList();
            //}


            //if (!string.IsNullOrEmpty(search.EmployeeId))
            //{
            //    dataListWork = dataListWork.Where(r => r.EmployeeId.Equals(search.EmployeeId)).ToList();
            //}

            int year = search.DateStart.Year;
            int month = search.DateStart.Month;
            int days = search.DateStart.Day;

            // Danh sách ngày nghỉ trong tháng
            var listHoliday = db.Holidays.AsNoTracking().Where(x => x.Year == year && x.HolidayDate.Month == month).ToList();

            // danh sách ngày trong tháng
            List<DayOfMonthModel> dayofMonths = new List<DayOfMonthModel>();

            for (int i = 0; i <= 30; i++)
            {
                dayofMonths.Add(new DayOfMonthModel
                {
                    DateTime = search.DateStart.AddDays(i),
                    TotalTime = 0,
                    TotalTimeDay = 0,
                    TimeDay = 0,
                });
            }


            // Danh sách ngày trong tháng kèm thông tin ngày nghỉ
            dayofMonths = (from d in dayofMonths
                           join h in listHoliday on d.DateTime equals h.HolidayDate into dh
                           from dhn in dh.DefaultIfEmpty()
                           select new DayOfMonthModel
                           {
                               DateTime = d.DateTime,
                               TotalTime = 0,
                               TotalTimeDay = 0,
                               TimeDay = 0,
                               IsHoliday = dhn != null
                           }).ToList();

            // Danh sách ngày trong tuần
            List<DayOfMonthModel> dayofweeks = new List<DayOfMonthModel>();
            foreach (var item in dayofMonths)
            {
                dayofweeks.Add(new DayOfMonthModel
                {
                    Day = (int)item.DateTime.DayOfWeek + 1,
                    IsHoliday = item.IsHoliday
                });
            }

            List<WorkDiaryTimeModel> listRs = new List<WorkDiaryTimeModel>();

            //listRs = (from d in dataListWork
            //          group d by new { d.Name, d.ObjectId, d.ProjectId, d.ModuleName, d.ModuleCode } into s
            //          select new WorkDiaryTimeModel
            //          {
            //              Name = s.Key.Name,
            //              ObjectId = s.Key.ObjectId,
            //              ProjectId = s.Key.ProjectId,
            //              ModuleCode = s.Key.ModuleCode,
            //              ModuleName = s.Key.ModuleName
            //          }).ToList();

            // Add list object null và object khác null vào chung 1 list
            List<WorkDiaryTimeModel> listWorkTotalTime = new List<WorkDiaryTimeModel>();
            listWorkTotalTime.AddRange(dataQuery);
            listWorkTotalTime.AddRange(listRs);


            decimal timeInDay;
            decimal TotalMonth = 0;

            List<WorkDiaryTimeModel> dailyInDay = new List<WorkDiaryTimeModel>();
            foreach (var date in dayofMonths)
            {
                date.TotalTimeDay = 0;
                foreach (var item in listWorkTotalTime)
                {
                    //dailyInDay = dataListWork.Where(r => r.ObjectId.Equals(item.ObjectId) && r.WorkDate.Value.Date == date.DateTime.Date).ToList();
                    dailyInDay.AddRange(dataQuery.Where(r => r.Id.Equals(item.Id) && r.WorkDate.Value.Date == date.DateTime.Date).ToList());
                    timeInDay = dailyInDay.Sum(s => s.TotalTime);
                    item.ListWorkingTime.Add(new DayOfMonthModel
                    {
                        DateTime = date.DateTime,
                        TotalTime = timeInDay,
                        IsHoliday = date.IsHoliday,
                        ExitDay = dailyInDay.Count > 0
                    });
                    item.TotalWorkTime += timeInDay;
                    date.TotalTimeDay += timeInDay;
                    item.IsHoliday = date.IsHoliday;
                }
                TotalMonth += date.TotalTimeDay;
            }

            var projectIds = listWorkTotalTime.Where(r => !string.IsNullOrEmpty(r.ProjectId)).GroupBy(g => g.ProjectId).Select(s => s.Key).ToList();

            var projects = (from p in db.Projects.AsNoTracking()
                            where projectIds.Contains(p.Id)
                            select new ProjectWorkTimeModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                            }
                           ).ToList();

            var projectNoId = new ProjectWorkTimeModel
            {
                Id = null,
                Name = "Không thuộc dự án",
            };

            projects.Add(projectNoId);

            foreach (var project in projects)
            {
                if (project.Id != null)
                {
                    project.ListWorkDiaryTime = listWorkTotalTime.Where(r => !string.IsNullOrEmpty(r.ProjectId) && r.ProjectId.Equals(project.Id)).ToList();
                    foreach (var date in dayofMonths)
                    {
                        //timeInDay = dataListWork.Where(r => !string.IsNullOrEmpty(r.ProjectId) && r.ProjectId.Equals(project.Id) && r.WorkDate.Value.Date == date.DateTime.Date).Sum(s => s.TotalTime);
                        //timeInDay += dataQuery.Where(r => !string.IsNullOrEmpty(r.ProjectId) && r.ProjectId.Equals(project.Id) && r.WorkDate.Value.Date == date.DateTime.Date).Sum(s => s.TotalTime);
                        //project.ListMondayWorkDiaryTime.Add(timeInDay);
                        //project.TotalWorkTime += timeInDay;
                    }
                }
                else
                {
                    project.ListWorkDiaryTimeNoProjectId = listWorkTotalTime.Where(r => string.IsNullOrEmpty(r.ProjectId)).ToList();
                    foreach (var date in dayofMonths)
                    {
                        //timeInDay = dataListWork.Where(r => string.IsNullOrEmpty(r.ProjectId) && r.WorkDate.Value.Date == date.DateTime.Date).Sum(s => s.TotalTime);
                        //timeInDay += dataQuery.Where(r => string.IsNullOrEmpty(r.ProjectId) && r.WorkDate.Value.Date == date.DateTime.Date).Sum(s => s.TotalTime);
                        //project.ListMondayWorkDiaryTime.Add(timeInDay);
                        //project.TotalWorkTime += timeInDay;
                    }

                }
                project.ListWorkDiaryTime.AddRange(project.ListWorkDiaryTimeNoProjectId);


            }

            return new
            {
                ListResult = projects,
                DayofMonths = dayofMonths,
                DayOfWeeks = dayofweeks,
                TotalMonth = TotalMonth
            };

        }

        public int GetConfigDayWorkDiary()
        {
            int day = 0;
            RedisService<ConfigModel> redisService = RedisService<ConfigModel>.GetInstance();

            // Key config nhật ký công việc
            string keyConfig = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheConfig"] + Constants.Config_WorkDiary_Day;
            if (redisService.Exists(keyConfig))
            {
                day = redisService.Get<int>(keyConfig);
            }
            else
            {
                var config = db.Configs.AsNoTracking().Where(r => r.Code.Equals(Constants.Config_WorkDiary_Day)).FirstOrDefault();

                day = int.Parse(config.Value);

                // Lưu cache vật tư
                redisService.Add<int>(keyConfig, day);
            }

            return day;
        }

        public DateTime ChangeTime(DateTime dateTime, int day)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond,
                dateTime.Kind);
        }
    }
}
