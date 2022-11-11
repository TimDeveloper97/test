using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Plans;
using NTS.Model.ProjectProducts;
using NTS.Model.Projects.Project;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.TaskTimeStandardModel;
using QLTK.Business.Plans;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;
using Newtonsoft.Json;
using NTS.Model.Holiday;
using NTS.Model.PlanHistory;
using QLTK.Business.AutoMappers;
using NTS.Model.HistoryVersion;
using QLTK.Business.Users;
using System.Diagnostics;
using System.Data.Entity;
using Syncfusion.UI.Xaml.Charts;
using QLTK.Business.ProjectProducts;
using RabbitMQ.Client.Framing.Impl;
using NTS.Model.Error;
using System.Threading;
using System.Drawing.Imaging;
using System.Globalization;
using NTS.Model.GanttChart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Syncfusion.CompoundFile.DocIO.Native;
using Syncfusion.DocIO.DLS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Microsoft.VisualBasic.ApplicationServices;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace QLTK.Business.ScheduleProject
{
    public class ScheduleProjectBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public ProjectProductBusiness pp = new ProjectProductBusiness();
        public ResultModel GetListPlanByProjectId(ScheduleProjectSearchModel modelSearch)
        {
            // Lấy dữ liệu kế hoạch trong danh mục sản phẩm của dự án
            var projectProducts = (from a in db.ProjectProducts.AsNoTracking().Where(r => r.ProjectId.Equals(modelSearch.ProjectId))
                                   join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into
                                   ab
                                   from ba in ab.DefaultIfEmpty()
                                   join c in db.Products.AsNoTracking() on a.ProductId equals c.Id into
                                   ac
                                   from ca in ac.DefaultIfEmpty()
                                   select new ScheduleEntity()
                                   {
                                       Id = a.Id,
                                       ParentId = a.ParentId,
                                       RealQuantity = a.RealQuantity,
                                       Weight = a.Weight,
                                       ContractCode = string.IsNullOrEmpty(ba.Code) ? a.ContractCode : ba.Code,
                                       ContractName = a.ContractName,
                                       NameView = a.ContractName,
                                       ContractIndex = a.ContractIndex,
                                       ContractStartDate = a.ContractStartDate,
                                       ContractDueDate = a.ContractDueDate,
                                       PlanStartDate = a.PlanStartDate,
                                       PlanDueDate = a.PlanDueDate,
                                       Duration = a.Duration,
                                       DoneRatio = a.DoneRatio,
                                       DataType = a.DataType,
                                       Status = a.Status,
                                       ModuleStatus = a.ModuleStatus
                                   }
                           ).ToList();

            // Lấy danh sách công việc chi tiết
            var plans = (from a in db.Plans.AsNoTracking().Where(r => r.ProjectId.Equals(modelSearch.ProjectId))
                         join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                         select new ScheduleEntity()
                         {
                             Id = a.Id,
                             ProjectProductId = a.ProjectProductId,
                             ParentId = a.ParentId != null ? a.ParentId : a.ProjectProductId,
                             StageName = a.ParentId == null ? a.Name.ToUpper() : string.Empty,
                             PlanName = a.ParentId != null ? a.Name.ToUpper() : string.Empty,
                             BackgroundColor = b.Color,
                             StageId = a.StageId,
                             ContractStartDate = a.ContractStartDate,
                             ContractDueDate = a.ContractDueDate,
                             PlanStartDate = a.PlanStartDate,
                             PlanDueDate = a.PlanDueDate,
                             Duration = a.Duration,
                             DoneRatio = a.DoneRatio,
                             Color = b.Color,
                             Weight = a.Weight,
                             IsPlan = a.IsPlan,
                             EstimateTime = a.EstimateTime,
                             Status = a.Status,
                             SupplierId = a.SupplierId,
                             Type = a.Type,
                             Index = b.index,
                             IndexPlan = a.Index,
                             CreateDate = a.CreateDate,
                             Description = a.Description
                         }).ToList();

            var count = 0;
            List<ScheduleEntity> listPlan = new List<ScheduleEntity>();
            listPlan.AddRange(plans);
            List<ScheduleEntity> listStage = new List<ScheduleEntity>();
            // Ngày bắt đầu hợp đồng
            if (modelSearch.ContractStartDateFromV != null)
            {
                count++;
                modelSearch.ContractStartDateFromV = DateTimeUtils.ConvertDateFrom(modelSearch.ContractStartDateFromV);
                DateTime date = ((DateTime)modelSearch.ContractStartDateFromV).AddDays(1);
                listPlan = listPlan.Where(u => u.ContractStartDate < date && u.IsPlan == true).ToList();
            }
            if (modelSearch.ContractStartDateToV != null)
            {
                modelSearch.ContractStartDateToV = DateTimeUtils.ConvertDateTo(modelSearch.ContractStartDateToV);
                count++;
                DateTime date = ((DateTime)modelSearch.ContractStartDateToV).AddDays(-1);
                listPlan = listPlan.Where(u => u.ContractStartDate > date && u.IsPlan == true).ToList();
            }

            // Ngày kết thúc hợp đồng
            if (modelSearch.ContractDueDateFromV != null)
            {
                count++;
                modelSearch.ContractDueDateFromV = DateTimeUtils.ConvertDateFrom(modelSearch.ContractDueDateFromV);
                DateTime date = ((DateTime)modelSearch.ContractDueDateFromV).AddDays(1);
                listPlan = listPlan.Where(u => u.ContractDueDate < date && u.IsPlan == true).ToList();
            }
            if (modelSearch.ContractDueDateToV != null)
            {
                count++;
                modelSearch.ContractDueDateToV = DateTimeUtils.ConvertDateTo(modelSearch.ContractDueDateToV);
                DateTime date = ((DateTime)modelSearch.ContractDueDateToV).AddDays(-1);
                listPlan = listPlan.Where(u => u.ContractDueDate > date && u.IsPlan == true).ToList();
            }

            // Ngày bắt đầu triển khai
            if (modelSearch.PlanStartDateFromV != null)
            {
                count++;
                modelSearch.PlanStartDateFromV = DateTimeUtils.ConvertDateFrom(modelSearch.PlanStartDateFromV);
                DateTime date = ((DateTime)modelSearch.PlanStartDateFromV).AddDays(1);
                listPlan = listPlan.Where(u => u.PlanStartDate < date && u.IsPlan == true).ToList();
            }
            if (modelSearch.PlanStartDateToV != null)
            {
                count++;
                modelSearch.PlanStartDateToV = DateTimeUtils.ConvertDateTo(modelSearch.PlanStartDateToV);
                DateTime date = ((DateTime)modelSearch.PlanStartDateToV).AddDays(-1);
                listPlan = listPlan.Where(u => u.PlanStartDate > date && u.IsPlan == true).ToList();
            }

            // Ngày kết thúc triển khai
            if (modelSearch.PlanDueDateFromV != null)
            {
                count++;
                modelSearch.PlanDueDateFromV = DateTimeUtils.ConvertDateFrom(modelSearch.PlanDueDateFromV);
                DateTime date = ((DateTime)modelSearch.PlanDueDateFromV).AddDays(1);
                listPlan = listPlan.Where(u => u.PlanDueDate < date && u.IsPlan == true).ToList();
            }
            if (modelSearch.PlanDueDateToV != null)
            {
                count++;
                modelSearch.PlanDueDateToV = DateTimeUtils.ConvertDateTo(modelSearch.PlanDueDateToV);
                DateTime date = ((DateTime)modelSearch.PlanDueDateToV).AddDays(-1);
                listPlan = listPlan.Where(u => u.PlanDueDate > date && u.IsPlan == true).ToList();
            }

            if (modelSearch.WorkClassify != 0)
            {
                count++;
                listPlan = listPlan.Where(u => u.Type == (modelSearch.WorkClassify) && u.IsPlan == true).ToList();
            }
            if (!string.IsNullOrEmpty(modelSearch.ImplementingAgenciesCode))
            {
                count++;
                listPlan = listPlan.Where(u => modelSearch.ImplementingAgenciesCode.Equals(u.SupplierId) && u.IsPlan == true).ToList();
            }
            if (modelSearch.WorkStatus != 0)
            {
                count++;
                listPlan = listPlan.Where(u => u.Status.Equals(modelSearch.WorkStatus) && u.IsPlan == true).ToList();
            }

            var countStage = 0;
            if (!string.IsNullOrEmpty(modelSearch.StageId) && !modelSearch.StageId.Equals("Tất cả"))
            {
                countStage++;
                listStage = plans.Where(a => a.StageId.Equals(modelSearch.StageId) && a.IsPlan == false).ToList();

            }
            if (count > 0 && countStage == 0)
            {
                foreach (var item in plans.ToList())
                {
                    if (item.IsPlan == true)
                    {
                        plans.Remove(item);
                    }
                }
                plans.AddRange(listPlan);
                foreach (var item in plans.ToList())
                {
                    if (item.IsPlan != true)
                    {
                        var planx = plans.Where(a => a.ParentId.Equals(item.Id)).ToList();
                        if (planx.Count == 0)
                        {
                            plans.Remove(item);
                        }
                    }
                }
            }
            if (countStage > 0)
            {
                var countPlans = plans.Count();
                foreach (var item in plans.ToList())
                {
                    if (item.IsPlan != true && !string.IsNullOrEmpty(item.StageName))
                    {
                        plans.Remove(item);
                    }
                }
                plans.AddRange(listStage);
                if (count > 0)
                {
                    foreach (var item in plans.ToList())
                    {
                        if (item.IsPlan == true)
                        {
                            plans.Remove(item);
                        }
                    }
                    plans.AddRange(listPlan);
                }
            }

            // Thực hiện sắp xếp thứ tự sản phẩm, module
            if (projectProducts.Count() > 0)
            {
                int maxLen = projectProducts.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                projectProducts = projectProducts
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            // Update lại tên của Sản phẩm/ Module trên Plan
            foreach (var item in projectProducts)
            {
                string nameView = string.Empty;
                if (item.DataType == Constants.ProjectProduct_DataType_Practice)
                {
                    nameView = item.ContractIndex + " - BTH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    nameView = item.ContractIndex + " - SP - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    nameView = item.ContractIndex + " - M - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    nameView = item.ContractIndex + " - MH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.NameView = nameView.NTSTrim();
                }
            }

            if (modelSearch.DateFrom.HasValue)
            {
                modelSearch.DateStart = modelSearch.DateFrom.Value;
            }
            else
            {
                if (projectProducts.FirstOrDefault(a => a.PlanStartDate.HasValue) != null)
                {
                    modelSearch.DateStart = projectProducts.Where(a => a.PlanStartDate.HasValue).Min(a => a.PlanStartDate.Value);
                    if (modelSearch.DateStart < DateTime.Now.AddYears(-1))
                    {
                        modelSearch.DateStart = DateTime.Now.AddYears(-1);
                    }
                }
                else
                {
                    modelSearch.DateStart = DateTime.Now.AddMonths(-1);
                }
            }

            if (modelSearch.DateTo.HasValue)
            {
                modelSearch.DateEnd = modelSearch.DateTo.Value;
            }
            else
            {
                if (projectProducts.FirstOrDefault(a => a.PlanDueDate.HasValue) != null)
                {
                    modelSearch.DateEnd = projectProducts.Where(a => a.PlanDueDate.HasValue).Max(a => a.PlanDueDate.Value);
                    if (modelSearch.DateEnd > DateTime.Now.AddYears(1))
                    {
                        modelSearch.DateEnd = DateTime.Now.AddYears(1);
                    }
                }
                else
                {
                    modelSearch.DateEnd = DateTime.Now.AddMonths(1);
                }
            }
            DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

            var planAssignment = db.PlanAssignments.AsNoTracking().ToList();
            var users = db.Users.AsNoTracking().ToList();

            // Sắp xếp thứ tự hiển thị chuẩn
            List<ScheduleEntity> listData = projectProducts.Union(plans).ToList();
            var listParent = projectProducts.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            List<ScheduleEntity> schedules = new List<ScheduleEntity>();
            List<ScheduleEntity> listChild = new List<ScheduleEntity>();

            foreach (var parent in listParent)
            {
                listChild = GetScheduleProjectChildIndex(parent, listData);

                // Trường hợp là sản phẩm, module
                if (!parent.IsPlan)
                {
                    if (parent.Status == (int)Constants.ScheduleStatus.Closed)
                    {
                        parent.InternalStatus = "OK";
                    }
                    else if (listChild.Where(r => !r.IsPlan && !r.ContractStartDate.HasValue).Any() || listChild.Where(r => r.InternalStatus.Equals("TRỐNG BẮT ĐẦU HĐ")).Any())
                    {
                        parent.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
                    }
                    else if (listChild.Where(r => !r.IsPlan && !r.ContractDueDate.HasValue).Any() || listChild.Where(r => r.InternalStatus.Equals("TRỐNG KẾT THÚC HĐ")).Any())
                    {
                        parent.InternalStatus = "TRỐNG KẾT THÚC HĐ";
                    }
                    else if (listChild.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || listChild.Where(r => r.InternalStatus.Equals("THIẾU NGÀY TRIỂN KHAI")).Any())
                    {
                        parent.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                    }
                    else if (parent.ContractDueDate.HasValue && (parent.PlanDueDate > parent.ContractDueDate) || listChild.Where(r => r.InternalStatus.Equals("QUÁ HẠN HỢP ĐỒNG")).Any())
                    {
                        parent.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                    }
                    else if (parent.PlanDueDate.HasValue && today > parent.PlanDueDate.Value && (parent.DoneRatio < 100) || listChild.Where(r => r.InternalStatus.Equals("QUÁ HẠN HOÀN THÀNH")).Any())
                    {
                        parent.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                    else
                    {
                        parent.InternalStatus = "OK";
                    }
                }
                schedules.Add(parent);
                schedules.AddRange(listChild);
            }

            List<ScheduleEntity> listScheduleEntity = new List<ScheduleEntity>();
            int index = 0;
            foreach (var item in schedules)
            {
                item.LoadIndex = index;
                if (!item.IsPlan)
                {
                    item.EstimateTime = null;
                }

                // Nếu là Công việc
                if (item.IsPlan)
                {

                    item.ListIdUserId = planAssignment.Where(a => a.PlanId.Equals(item.Id)).OrderBy(a => a.IsMain).Select(a => a.UserId).ToList();
                    item.ResponsiblePersionName = string.Join(", ", (from s in planAssignment
                                                                     where s.PlanId.Equals(item.Id)
                                                                     join m in users on s.UserId equals m.Id
                                                                     orderby s.IsMain descending
                                                                     select m.UserName).ToArray());

                    item.MainUserId = planAssignment.Where(r => r.PlanId.Equals(item.Id) && r.IsMain).FirstOrDefault() != null ? planAssignment.Where(r => r.PlanId.Equals(item.Id) && r.IsMain).FirstOrDefault().UserId : string.Empty;
                }
                index++;
            }
            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                count++;
                var ListUsers = db.Users.ToList();
                var ListEmps = db.Employees.ToList();
                foreach (var item in schedules.ToList())
                {
                    if (item.IsPlan == true)
                    {
                        List<string> deparmentIds = new List<string>();
                        foreach (var u in item.ListIdUserId.ToList())
                        {
                            var user = ListUsers.FirstOrDefault(a => a.Id.Equals(u));
                            var emp = ListEmps.FirstOrDefault(a => a.Id.Equals(user.EmployeeId));
                            if (emp != null)
                            {
                                deparmentIds.Add(emp.DepartmentId);
                            }
                        }
                        var de = deparmentIds.FirstOrDefault(a => a.Equals(modelSearch.DepartmentId));
                        if (de == null)
                        {
                            schedules.Remove(item);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                count++;
                if (!modelSearch.EmployeeId.Equals("thieunv"))
                {
                    var user = db.Users.FirstOrDefault(a => a.EmployeeId.Equals(modelSearch.EmployeeId));
                    if (user != null)
                    {
                        foreach (var item in schedules.ToList())
                        {
                            var emp = item.ListIdUserId.FirstOrDefault(a => a.Equals(user.Id));
                            if (emp == null && item.IsPlan == true)
                            {
                                schedules.Remove(item);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schedules.ToList())
                    {
                        if (item.ListIdUserId.Count() !=0)
                        {
                            schedules.Remove(item);
                        }
                    }
                }
            }
            if (modelSearch.PlanStatus != 0)
            {
                count++;
                var ListPlanStatus = new Dictionary<int, string>  {
                { 1, "OK"},
                { 2, "QUÁ HẠN HỢP ĐỒNG" },
                { 3, "QUÁ HẠN HOÀN THÀNH" },
                { 4, "THIẾU NGÀY TRIỂN KHAI" },
                }.ToList();
                var planByStatus = schedules.Where(a => ListPlanStatus.FirstOrDefault(b => b.Key == modelSearch.PlanStatus).Value.Equals(a.InternalStatus)).ToList();
                foreach (var item in schedules.ToList())
                {
                    if (item.IsPlan == true)
                    {
                        schedules.Remove(item);
                    }
                }
                schedules.AddRange(planByStatus);
            }
            var searchProduct = false;
            List<ScheduleEntity> Problems = new List<ScheduleEntity>();
            if (modelSearch.ErrorProduct != 0)
            {
                var planByStatus = schedules.Where(a => a.DataType == Constants.ProjectProduct_DataType_ProjectProduct).ToList();
                if (modelSearch.ErrorProduct == 1)
                {
                    planByStatus = planByStatus.Where(a => a.PlanStartDate == null || a.PlanDueDate == null).ToList();
                }
                else if (modelSearch.ErrorProduct == 2)
                {
                    planByStatus = planByStatus.Where(a => a.ContractStartDate == null || a.ContractDueDate == null).ToList();
                }
                else if (modelSearch.ErrorProduct == 3)
                {
                    planByStatus = planByStatus.Where(a => a.ModuleStatus == 2 || a.ModuleStatus == 3).ToList();
                }
                foreach (var item in planByStatus)
                {
                    var p = Problems.FirstOrDefault(a => a.Id.Equals(item.Id));
                    if (p == null)
                    {
                        Problems.Add(item);
                    }
                }
                if (Problems.Count == 0)
                {
                    searchProduct = true;
                }
            }
            if (modelSearch.ErrorModule != 0)
            {
                var planByStatus = schedules.Where(a => a.DataType == Constants.ProjectProduct_DataType_Module || a.DataType == Constants.ProjectProduct_DataType_Paradigm).ToList();
                if (modelSearch.ErrorModule == 1)
                {
                    planByStatus = planByStatus.Where(a => a.PlanStartDate == null || a.PlanDueDate == null).ToList();
                }
                else if (modelSearch.ErrorModule == 2)
                {
                    planByStatus = planByStatus.Where(a => a.ContractStartDate == null || a.ContractDueDate == null).ToList();
                }
                else if (modelSearch.ErrorModule == 3)
                {
                    planByStatus = planByStatus.Where(a => a.ModuleStatus == 2 || a.ModuleStatus == 3).ToList();
                }
                foreach (var item in planByStatus)
                {
                    var p = Problems.FirstOrDefault(a => a.Id.Equals(item.Id));
                    if (p == null)
                    {
                        Problems.Add(item);
                    }
                }
                if (Problems.Count == 0)
                {
                    searchProduct = true;
                }

            }
            if (modelSearch.ErrorStage != 0)
            {
                countStage++;
                var planByStatus = schedules.Where(a => !string.IsNullOrEmpty(a.StageName)).ToList();
                if (modelSearch.ErrorStage == 1)
                {
                    planByStatus = planByStatus.Where(a => a.PlanStartDate == null || a.PlanDueDate == null).ToList();
                }
                else if (modelSearch.ErrorStage == 2)
                {
                    planByStatus = planByStatus.Where(a => a.ContractStartDate == null || a.ContractDueDate == null).ToList();
                }
                else if (modelSearch.ErrorStage == 3)
                {
                    planByStatus = planByStatus.Where(a => a.ContractDueDate.HasValue && a.PlanDueDate.HasValue && a.PlanDueDate > a.ContractDueDate).ToList();
                }
                foreach (var item in schedules.ToList())
                {
                    if (!string.IsNullOrEmpty(item.StageName))
                    {
                        schedules.Remove(item);
                    }
                }
                schedules.AddRange(planByStatus);
            }
            if (!string.IsNullOrEmpty(modelSearch.NameProduct))
            {
                var planByStatus = schedules.Where(a => a.DataType != 0).ToList();
                planByStatus = planByStatus.Where(a => a.NameView.ToUpper().Contains(modelSearch.NameProduct.ToUpper())).ToList();
                if (Problems.Count == 0)
                {
                    foreach (var item in planByStatus)
                    {
                        Problems.Add(item);
                    }
                }
                else
                {
                    List<ScheduleEntity> list = new List<ScheduleEntity>();
                    foreach (var item in planByStatus)
                    {
                        var p = Problems.FirstOrDefault(a => a.Id.Equals(item.Id));
                        if (p != null)
                        {
                            list.Add(p);
                        }
                    }
                    Problems = list;
                }

                if (Problems.Count == 0)
                {
                    searchProduct = true;
                }

            }
            if (countStage > 0)
            {
                List<ScheduleEntity> results = new List<ScheduleEntity>();
                var stages = schedules.Where(a => a.IsPlan == false && !string.IsNullOrEmpty(a.StageName)).ToList();
                results.AddRange(stages);
                foreach (var item in stages)
                {
                    var chidldPlans = schedules.Where(a => item.Id.Equals(a.ParentId)).ToList();
                    results.AddRange(chidldPlans);

                    var parent = schedules.FirstOrDefault(a => a.Id.Equals(item.ParentId));
                    if (results.FirstOrDefault(a => parent.Id.Equals(a.Id)) == null)
                    {
                        results.Add(parent);
                    }
                    if (!string.IsNullOrEmpty(parent.ParentId))
                    {
                        removeChild(schedules, parent, results);
                    }
                }
                schedules = results;
            }
            else if (count > 0)
            {
                List<ScheduleEntity> results = new List<ScheduleEntity>();
                var planChilds = schedules.Where(a => a.IsPlan == true).ToList();
                results.AddRange(planChilds);
                foreach (var item in planChilds)
                {
                    var parent = schedules.FirstOrDefault(a => a.Id.Equals(item.ParentId));
                    if (results.FirstOrDefault(a => parent.Id.Equals(a.Id)) == null)
                    {
                        results.Add(parent);
                    }
                    if (!string.IsNullOrEmpty(parent.ParentId))
                    {
                        removeChild(schedules, parent, results);
                    }
                }
                schedules = results;
            }
            if (Problems.Count > 0)
            {
                schedules = searchProblem(Problems, schedules);
            }
            if (searchProduct == true)
            {
                schedules = new List<ScheduleEntity>();
            }
            schedules = schedules.OrderBy(a => a.LoadIndex).ToList();
            return new ResultModel { listResult = schedules };
        }

        public List<ScheduleEntity> searchProblem(List<ScheduleEntity> Problems, List<ScheduleEntity> schedules)
        {
            List<ScheduleEntity> listResult = new List<ScheduleEntity>();
            foreach (var item in Problems)
            {
                if (string.IsNullOrEmpty(item.ParentId))
                {
                    var itemCheck = listResult.FirstOrDefault(a => a.Id.Equals(item.Id));
                    if (itemCheck == null)
                    {
                        listResult.Add(item);
                    }
                    var listChild = GetScheduleProjectChild(item.Id, schedules);
                    foreach (var child in listChild)
                    {
                        var childCheck = listResult.FirstOrDefault(a => a.Id.Equals(child.Id));
                        if (childCheck == null)
                        {
                            listResult.Add(child);
                        }
                    }
                }
                else
                {
                    var listParent = GetScheduleProjectParent(item.ParentId, schedules);
                    foreach (var parent in listParent)
                    {
                        var parentCheck = listResult.FirstOrDefault(a => a.Id.Equals(parent.Id));
                        if (parentCheck == null)
                        {
                            listResult.Add(parent);
                        }
                    }
                    var itemCheck = listResult.FirstOrDefault(a => a.Id.Equals(item.Id));
                    if (itemCheck == null)
                    {
                        listResult.Add(item);
                    }
                    var listChild = GetScheduleProjectChild(item.Id, schedules);
                    foreach (var child in listChild)
                    {
                        var childCheck = listResult.FirstOrDefault(a => a.Id.Equals(child.Id));
                        if (childCheck == null)
                        {
                            listResult.Add(child);
                        }
                    }
                }
            }
            return listResult;
        }

        public void removeChild(List<ScheduleEntity> schedules, ScheduleEntity parent, List<ScheduleEntity> results)
        {
            var parentProduct = schedules.FirstOrDefault(a => a.Id.Equals(parent.ParentId));
            if (results.FirstOrDefault(a => parentProduct.Id.Equals(a.Id)) == null)
            {
                results.Add(parentProduct);
            }
            if (!string.IsNullOrEmpty(parentProduct.ParentId))
            {
                removeChild(schedules, parentProduct, results);
            }

        }

        /// <summary>
        /// Add công đoạn cho sản phẩm
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NTSLogException"></exception>
        public List<ScheduleProjectResultModel> ChooseStage(string userId, ProduceStageModel model)
        {
            List<ScheduleProjectResultModel> listResult = new List<ScheduleProjectResultModel>();
            List<Plan> listPlan = new List<Plan>();

            // Lấy ID của Sản phẩm cha
            string parentProductId = db.ProjectProducts.AsNoTracking().Where(r => r.Id.Equals(model.Id)).Select(r => r.ParentId).FirstOrDefault();

            // Lấy danh sách Công đoạn của Sản phẩm cha
            var parentStages = (from r in db.Plans.AsNoTracking()
                                where r.ProjectProductId.Equals(parentProductId) && r.IsPlan == false
                                select r).ToList();
            Plan parentStage;

            if (model.ListIdSelect.Count > 0)
            {
                foreach (var item in model.ListIdSelect)
                {
                    var stage = db.Stages.FirstOrDefault(t => t.Id.Equals(item));

                    if (stage != null)
                    {
                        parentStage = parentStages.Where(r => r.StageId.Equals(item)).FirstOrDefault();
                        Plan plan = new Plan()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectId = model.ProjectId,
                            StageId = item,
                            ProjectProductId = model.Id,
                            TrackerType = 1,
                            Weight = 1,
                            Status = (int)Constants.ScheduleStatus.Open,
                            DoneRatio = 0,
                            IsPlan = false,
                            Name = stage.Name,
                            ContractStartDate = parentStage != null ? parentStage.ContractStartDate : null,
                            ContractDueDate = parentStage != null ? parentStage.ContractDueDate : null,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        listResult.Add(new ScheduleProjectResultModel
                        {
                            Id = plan.Id,
                            ProjectId = plan.ProjectId,
                            ProjectProductId = plan.ProjectProductId,
                            ParentId = plan.ProjectProductId,
                            StageName = plan.Name,
                            BackgroundColor = stage.Color,
                            StageId = plan.StageId,
                            ContractStartDate = plan.ContractStartDate,
                            ContractDueDate = plan.ContractDueDate,
                            PlanStartDate = plan.PlanStartDate,
                            PlanDueDate = plan.PlanDueDate,
                            DoneRatio = plan.DoneRatio,
                            Color = stage.Color,
                            Weight = plan.Weight,
                            IsPlan = plan.IsPlan,
                            EstimateTime = plan.EstimateTime,
                            Status = plan.Status,
                            SupplierId = plan.SupplierId,
                            Type = plan.Type,
                            Index = stage.index,
                            CreateDate = plan.CreateDate
                        });

                        listPlan.Add(plan);
                    }
                }

            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Plans.AddRange(listPlan);
                    db.SaveChanges();
                    this.ReCalculateDoneRatioProjectProduct(model.Id);
                    this.UpdateProjectProductDate(model.Id);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }

            }

            return listResult;
        }

        /// <summary>
        /// Thêm mới Công việc
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userLoginId"></param>
        /// <returns></returns>
        /// <exception cref="NTSLogException"></exception>
        public ScheduleEntity CreatePlan(PlanModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectProduct = db.ProjectProducts.AsNoTracking().FirstOrDefault(a => a.Id.Equals(model.ProjectProductId));

                    if (projectProduct == null)
                    {
                        return null;
                    }

                    var indexs = db.Plans.Where(a => a.IsPlan && a.ProjectProductId.Equals(model.ProjectProductId) && a.StageId.Equals(model.StageId)).ToList();
                    var maxIndex = 1;
                    if (indexs.FirstOrDefault(i => i.Index == model.Index) != null)
                    {
                        if (indexs.Count > 0)
                        {
                            maxIndex = indexs.Select(i => i.Index).Max();
                        }

                        if (model.Index <= maxIndex)
                        {
                            int modelIndex = model.Index;
                            var listOrder = indexs.Where(i => i.Index >= modelIndex).ToList();
                            if (listOrder.Count > 0 && listOrder != null)
                            {
                                foreach (var item in listOrder)
                                {
                                    item.Index++;
                                }
                            }
                        }
                    }

                    NTS.Model.Repositories.Plan newPlan = new NTS.Model.Repositories.Plan
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = projectProduct.ProjectId,
                        ProjectProductId = projectProduct.Id,
                        StageId = model.StageId,
                        ParentId = model.ParentId,
                        Name = model.Name.ToUpper(),
                        IsPlan = true,
                        DoneRatio = 0,
                        Type = model.DataType,
                        Status = (int)Constants.ScheduleStatus.Open,
                        SupplierId = Constants.TPASupplierId,
                        Description = model.Description,
                        Weight = 1,
                        EstimateTime = 8,
                        Index = model.Index,
                        CreateBy = userLoginId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userLoginId,
                        UpdateDate = DateTime.Now
                    };

                    if (projectProduct.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project)
                    {
                        newPlan.Type = Constants.Plan_Type_Project;
                    }
                    else if (projectProduct.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Additional)
                    {
                        newPlan.Type = Constants.Plan_Type_Additional;
                    }
                    else if (projectProduct.ModuleStatus == Constants.ProjectProduct_ModuleStatus_AdditionalNoPrice)
                    {
                        newPlan.Type = Constants.Plan_Type_AdditionalNoPrice;
                    }

                    db.Plans.Add(newPlan);
                    db.SaveChanges();
                    this.ReCalculateDoneRatio(model.ParentId);

                    trans.Commit();

                    return new ScheduleEntity()
                    {
                        Id = newPlan.Id,
                        ProjectProductId = newPlan.ProjectProductId,
                        StageId = newPlan.StageId,
                        ParentId = newPlan.ParentId,
                        PlanName = newPlan.Name,
                        IsPlan = newPlan.IsPlan,
                        Status = newPlan.Status,
                        DoneRatio = newPlan.DoneRatio,
                        Description = newPlan.Description,
                        Weight = newPlan.Weight,
                        EstimateTime = newPlan.EstimateTime,
                        IndexPlan = newPlan.Index,
                        SupplierId = newPlan.SupplierId,
                        Type = newPlan.Type,
                        InternalStatus = "THIẾU NGÀY TK",
                    };
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin kế hoạch (Popup thông tin kế hoạch)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userLoginId"></param>
        /// <returns></returns>
        /// <exception cref="NTSLogException"></exception>
        public ScheduleEntity UpdatePlan(PlanModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newPlan = db.Plans.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    if (newPlan == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
                    }

                    if (newPlan.Index != model.Index)
                    {
                        var plans = db.Plans.Where(a => !a.Id.Equals(model.Id) && a.IsPlan && a.ProjectProductId.Equals(newPlan.ProjectProductId) && a.StageId.Equals(newPlan.StageId)).OrderBy(r => r.Index).ToList();

                        for (int i = 0; i < plans.Count; i++)
                        {
                            if (model.Index > newPlan.Index && plans[i].Index <= model.Index)
                            {
                                plans[i].Index = i + 1;
                            }
                            else if (model.Index < newPlan.Index && plans[i].Index >= model.Index)
                            {
                                plans[i].Index = i + 2;
                            }
                        }
                    }

                    newPlan.Name = model.Name;
                    newPlan.Description = model.Description;
                    newPlan.Index = model.Index;
                    newPlan.UpdateBy = userLoginId;
                    newPlan.UpdateDate = DateTime.Now;

                    db.SaveChanges();
                    trans.Commit();

                    ScheduleEntity scheduleEntity = new ScheduleEntity()
                    {
                        Id = newPlan.Id,
                        ProjectProductId = newPlan.ProjectProductId,
                        StageId = newPlan.StageId,
                        ParentId = newPlan.ParentId,
                        PlanName = newPlan.Name.ToUpper(),
                        IsPlan = newPlan.IsPlan,
                        Status = newPlan.Status,
                        EstimateTime = newPlan.EstimateTime,
                        Description = newPlan.Description,
                        Weight = newPlan.Weight,
                        DoneRatio = newPlan.DoneRatio,
                        Type = newPlan.Type,
                        IndexPlan = newPlan.Index,
                        SupplierId = newPlan.SupplierId,
                    };

                    DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

                    if (!scheduleEntity.PlanStartDate.HasValue || !scheduleEntity.PlanDueDate.HasValue)
                    {
                        scheduleEntity.InternalStatus = "THIẾU NGÀY TK";
                    }
                    else if (today > scheduleEntity.PlanDueDate && scheduleEntity.DoneRatio < 100)
                    {
                        scheduleEntity.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                    else
                    {
                        scheduleEntity.InternalStatus = "OK";
                    }
                    return scheduleEntity;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        /// <summary>
        /// Chỉnh sửa Công việc
        /// </summary>
        /// <param name="listPlan"></param>
        public void ModifyPlan(List<ScheduleEntity> listPlan)
        {
            Plan plan = new Plan();
            bool isNeedUpdateDate = false;
            bool isNeedUpdateDoneRatio = false;
            foreach (var item in listPlan)
            {
                // Trường hợp update dữ liệu Công đoạn hoặc Công việc
                if (!string.IsNullOrEmpty(item.StageId))
                {
                    plan = new Plan();
                    plan = db.Plans.FirstOrDefault(a => a.Id.Equals(item.Id));

                    // Trường hợp là công việc
                    if (plan != null && plan.IsPlan)
                    {
                        isNeedUpdateDate = plan.PlanStartDate != item.PlanStartDate || plan.PlanDueDate != item.PlanDueDate;
                        isNeedUpdateDoneRatio = plan.Weight != item.Weight;

                        plan.Weight = item.Weight;
                        plan.Type = item.Type;
                        plan.Name = item.PlanName;
                        plan.ContractStartDate = item.ContractStartDate;
                        plan.ContractDueDate = item.ContractDueDate;
                        plan.PlanStartDate = item.PlanStartDate;
                        plan.PlanDueDate = item.PlanDueDate;
                        plan.EstimateTime = item.EstimateTime.HasValue ? item.EstimateTime.Value : 0;
                        plan.SupplierId = item.SupplierId;
                        plan.Duration = (plan.PlanDueDate.HasValue && plan.PlanStartDate.HasValue) ? (int)(plan.PlanDueDate - plan.PlanStartDate).Value.TotalDays + 1 : 0;
                        db.SaveChanges();

                        // Update thông tin của Công đoạn: Ngày bắt đầu triển khai, Ngày kết thúc triển khai, Duration
                        if (isNeedUpdateDate)
                        {
                            this.UpdateStageDate(plan.ParentId);
                        }

                        if (isNeedUpdateDoneRatio)
                        {
                            this.ReCalculateDoneRatio(plan.ParentId);
                        }
                    }
                    // Trường hợp là công đoạn
                    else if (plan != null && !plan.IsPlan)
                    {
                        plan.ContractStartDate = item.ContractStartDate;
                        plan.ContractDueDate = item.ContractDueDate;
                        plan.Weight = item.Weight;

                        isNeedUpdateDoneRatio = plan.Weight != item.Weight;

                        plan.Duration = (plan.PlanDueDate.HasValue && plan.PlanStartDate.HasValue) ? (int)(plan.PlanDueDate - plan.PlanStartDate).Value.TotalDays + 1 : 0;
                        db.SaveChanges();
                        // Update thông tin của Module
                        this.UpdateProjectProductDate(plan.ProjectProductId);
                        if (isNeedUpdateDoneRatio)
                        {
                            this.ReCalculateDoneRatio(plan.Id);
                        }
                    }
                }
                // Trường hợp update Module, Sản phẩm
                else
                {
                    // Trường hợp 
                    var projectProduct = db.ProjectProducts.FirstOrDefault(a => a.Id.Equals(item.Id));
                    if (projectProduct != null)
                    {
                        projectProduct.Weight = item.Weight;
                        this.ReCalculateDoneRatioProjectProduct(projectProduct.Id);
                    }
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Chỉnh sửa Công việc
        /// </summary>
        /// <param name="listPlan"></param>
        public void Pending(string planId)
        {
            var plan = db.Plans.FirstOrDefault(a => a.Id.Equals(planId));

            if (plan == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            //  Trường hợp là công việc
            if (plan.IsPlan)
            {
                plan.Status = (int)Constants.ScheduleStatus.Stop;
                db.SaveChanges();

                ReCalculateDoneRatio(plan.ParentId);
            }
            // Trường hợp là công đoạn
            else
            {
                var plans = db.Plans.Where(a => a.ParentId.Equals(planId));
                foreach (var item in plans)
                {
                    item.Status = (int)Constants.ScheduleStatus.Stop;
                }
                db.SaveChanges();

                ReCalculateDoneRatio(planId);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Hủy công việc
        /// </summary>
        /// <param name="listPlan"></param>
        public void Cancel(string planId)
        {
            var plan = db.Plans.FirstOrDefault(a => a.Id.Equals(planId));

            if (plan == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            //  Trường hợp là công việc
            if (plan.IsPlan)
            {
                plan.Status = (int)Constants.ScheduleStatus.Cancel;
                db.SaveChanges();

                ReCalculateDoneRatio(plan.ParentId);
            }
            // Trường hợp là công đoạn
            else
            {
                var plans = db.Plans.Where(a => a.ParentId.Equals(planId));
                foreach (var item in plans)
                {
                    item.Status = (int)Constants.ScheduleStatus.Cancel;
                }
                db.SaveChanges();

                ReCalculateDoneRatio(planId);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Chỉnh sửa Công việc
        /// </summary>
        /// <param name="listPlan"></param>
        public void Resume(string planId)
        {
            var plan = db.Plans.FirstOrDefault(a => a.Id.Equals(planId));

            if (plan == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            //  Trường hợp là công việc
            if (plan.IsPlan)
            {
                plan.Status = plan.DoneRatio == 100 ? (int)Constants.ScheduleStatus.Closed : (plan.DoneRatio == 0 ? (int)Constants.ScheduleStatus.Open : (int)Constants.ScheduleStatus.Ongoing);
                db.SaveChanges();
                ReCalculateDoneRatio(plan.ParentId);

            }
            // Trường hợp là công đoạn
            else
            {
                var plans = db.Plans.Where(a => a.ParentId.Equals(planId));
                foreach (var item in plans)
                {
                    item.Status = item.DoneRatio == 100 ? (int)Constants.ScheduleStatus.Closed : (item.DoneRatio == 0 ? (int)Constants.ScheduleStatus.Open : (int)Constants.ScheduleStatus.Ongoing);
                }
                db.SaveChanges();

                ReCalculateDoneRatio(planId);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa công việc trên Schedule
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userLoginId"></param>
        public void DeletePlan(string planId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _plan = db.Plans.FirstOrDefault(a => a.Id.Equals(planId));
                    if (_plan == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
                    }

                    var indexs = db.Plans.Where(a => a.IsPlan && a.ProjectProductId.Equals(_plan.ProjectProductId) && a.StageId.Equals(_plan.StageId)).ToList();
                    var maxIndex = 1;
                    if (indexs.Count > 0)
                    {
                        maxIndex = indexs.Select(i => i.Index).Max();
                    }

                    if (_plan.Index < maxIndex)
                    {
                        int modelIndex = _plan.Index;
                        var listOrder = indexs.Where(i => i.Index > modelIndex).ToList();
                        if (listOrder.Count > 0 && listOrder != null)
                        {
                            foreach (var item in listOrder)
                            {
                                item.Index--;
                            }
                        }
                    }

                    // Trường hợp đã Logtime thì không được phép xóa công việc
                    var workDiary = db.WorkDiaries.Where(i => i.ObjectId.Equals(planId)).ToList();
                    if (workDiary.Count > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0088, TextResourceKey.WorkDiary);

                        // db.WorkDiaries.RemoveRange(workDiary);
                    }

                    var jsonApter = AutoMapperConfig.Mapper.Map<PlanHistoryModel>(_plan);
                    UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Plan, _plan.Id, _plan.Name, jsonApter);

                    db.PlanAssignments.RemoveRange(db.PlanAssignments.Where(a => a.PlanId.Equals(planId)));
                    db.Plans.Remove(_plan);
                    db.SaveChanges();

                    // Update thông tin của Công đoạn: Ngày bắt đầu triển khai, Ngày kết thúc triển khai, Duration
                    UpdateStageDate(_plan.ParentId);
                    this.ReCalculateDoneRatio(_plan.ParentId);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa công đoạn trên Schedule
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userLoginId"></param>
        public void DeleteStage(string stageId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var stage = db.Plans.FirstOrDefault(a => a.Id.Equals(stageId));
                    if (stage == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
                    }

                    var plans = db.Plans.Where(a => a.ParentId.Equals(stageId));

                    var jsonApter = AutoMapperConfig.Mapper.Map<PlanHistoryModel>(stage);
                    UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Plan, stage.Id, stage.Name, jsonApter);

                    foreach (var item in plans)
                    {
                        db.PlanAssignments.RemoveRange(db.PlanAssignments.Where(a => a.PlanId.Equals(item.Id)));
                        db.Plans.Remove(item);

                        // Trường hợp đã log time thì không xóa
                        var workDiary = db.WorkDiaries.Where(i => i.ObjectId.Equals(item.Id)).ToList();
                        if (workDiary.Count > 0)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0088, TextResourceKey.WorkDiary);
                            // db.WorkDiaries.RemoveRange(workDiary);
                        }
                    }
                    db.Plans.Remove(stage);

                    db.SaveChanges();

                    // Update thông tin của Công đoạn: Ngày bắt đầu triển khai, Ngày kết thúc triển khai, Duration
                    this.UpdateProjectProductDate(stage.ProjectProductId);
                    this.ReCalculateDoneRatioProjectProduct(stage.ProjectProductId);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Thực hiện update Ngày thực hiện, cho Công đoạn và Sản phẩm
        /// </summary>
        /// <param name="stageId"></param>
        private void UpdateStageDate(string stageId)
        {
            // Lấy thông tin Stage
            var stage = db.Plans.FirstOrDefault(a => a.Id.Equals(stageId));
            if (stage != null)
            {
                // Update thông tin Stage
                stage.PlanStartDate = db.Plans.Where(r => r.ParentId.Equals(stage.Id)).Select(r => r.PlanStartDate).Min();
                stage.PlanDueDate = db.Plans.Where(r => r.ParentId.Equals(stage.Id)).Select(r => r.PlanDueDate).Max();
                stage.Duration = (stage.PlanDueDate.HasValue && stage.PlanStartDate.HasValue) ? (int)(stage.PlanDueDate - stage.PlanStartDate).Value.TotalDays + 1 : 0;
                db.SaveChanges();

                UpdateProjectProductDate(stage.ProjectProductId);
            }
        }

        private void UpdateProjectProductDate(string projectProductId)
        {
            // Lấy thông tin Stage
            var product = db.ProjectProducts.FirstOrDefault(a => a.Id.Equals(projectProductId));
            if (product != null)
            {
                // Chỉ lấy ra công đoạn
                var plan = db.Plans.AsNoTracking().Where(r => r.ProjectProductId.Equals(product.Id) && r.IsPlan == false).ToList();

                if (plan.Count > 0)
                {
                    var products = db.ProjectProducts.AsNoTracking().Where(r => r.ParentId.Equals(product.Id)).ToList();
                    product.PlanStartDate = products.Select(r => r.PlanStartDate).Union(plan.Select(r => r.PlanStartDate)).Min();
                    product.PlanDueDate = products.Select(r => r.PlanDueDate).Union(plan.Select(r => r.PlanDueDate)).Max();
                    product.ContractStartDate = products.Select(r => r.ContractStartDate).Union(plan.Select(r => r.ContractStartDate)).Min();
                    product.ContractDueDate = products.Select(r => r.ContractDueDate).Union(plan.Select(r => r.ContractDueDate)).Max();
                }
                else
                {
                    var products = db.ProjectProducts.AsNoTracking().Where(r => r.ParentId.Equals(product.Id)).ToList();
                    product.PlanStartDate = products.Select(r => r.PlanStartDate).Min();
                    product.PlanDueDate = products.Select(r => r.PlanDueDate).Max();
                    product.ContractStartDate = products.Select(r => r.ContractStartDate).Min();
                    product.ContractDueDate = products.Select(r => r.ContractDueDate).Max();
                }

                product.Duration = (product.PlanDueDate.HasValue && product.PlanStartDate.HasValue) ? (int)(product.PlanDueDate - product.PlanStartDate).Value.TotalDays + 1 : 0;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(product.ParentId))
                {
                    this.UpdateProjectProductDate(product.ParentId);
                }
            }
        }

        private void ReCalculateDoneRatio(string stageId)
        {
            // Lấy thông tin Stage
            var stage = db.Plans.FirstOrDefault(a => a.Id.Equals(stageId));

            if (stage != null)
            {
                var plans = db.Plans.AsNoTracking().Where(r => r.ParentId.Equals(stage.Id)).ToList();
                int numOfPlan = plans.Count();
                int numOfOpen = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                int numOfOngoing = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                int numOfClosed = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                int numOfStop = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                int numOfCancel = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();

                // Update thông tin Stage
                stage.DoneRatio = plans.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 ? 0 : plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() / plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum();

                // Trường hợp chưa có công việc thì là Open (phòng trường hợp xóa công việc)
                if (numOfPlan == 0)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp có bất kỳ công việc nào đang Ongoing thì là Ongoing
                else if (numOfOngoing > 0)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Ongoing;
                }
                // Trường hợp tất cả công việc là Open
                else if (numOfPlan == numOfOpen)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp tất cả công việc là Close
                else if (numOfPlan == numOfClosed)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Closed;
                }
                // Trường hợp tất cả công việc là Stop
                else if (numOfPlan == numOfStop)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Stop;
                }
                // Trường hợp tất cả công việc là Cancel
                else if (numOfPlan == numOfCancel)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Cancel;
                }
                else
                {
                    if (numOfOpen > 0)
                    {
                        if (numOfClosed > 0)
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Ongoing;
                        }
                        else
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Open;
                        }

                    }
                    else
                    {
                        if (numOfClosed > 0)
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Closed;
                        }
                        else
                        {
                            if (numOfStop > 0)
                            {
                                stage.Status = (int)Constants.ScheduleStatus.Stop;
                            }
                        }
                    }
                }

                db.SaveChanges();

                ReCalculateDoneRatioProjectProduct(stage.ProjectProductId);
            }
        }

        private void ReCalculateDoneRatioProjectProduct(string projectProductId)
        {
            // Thông tin của sản phẩm cha cần update
            var product = db.ProjectProducts.FirstOrDefault(a => a.Id.Equals(projectProductId));

            // Danh sách tất cả công đoạn của sản phẩm
            var plans = db.Plans.AsNoTracking().Where(r => r.ProjectProductId.Equals(projectProductId) && r.IsPlan == false).ToList();

            if (product != null)
            {
                int numOfPlan;
                int numOfOpen;
                int numOfOngoing;
                int numOfClosed;
                int numOfStop;
                int numOfCancel;

                // Trường hợp có công đoạn
                if (plans.Count > 0)
                {
                    // Danh sách tất cả sản phẩm con
                    var products = db.ProjectProducts.Where(a => a.ParentId.Equals(projectProductId)).ToList();

                    product.DoneRatio = (plans.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 && products.Where(r => r.Status != 4 & r.Status != 5).Count() == 0)
                        ? 0
                        : (plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() + products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum()) / (plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum() + products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum());

                    numOfPlan = plans.Count() + products.Count();
                    numOfOpen = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                    numOfOngoing = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                    numOfClosed = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                    numOfStop = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                    numOfCancel = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();

                }
                else
                {
                    // Danh sách tất cả sản phẩm con
                    var products = db.ProjectProducts.Where(a => a.ParentId.Equals(projectProductId)).ToList();

                    product.DoneRatio = products.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 ? 0 : products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() / products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum();

                    numOfPlan = products.Count();
                    numOfOpen = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                    numOfOngoing = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                    numOfClosed = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                    numOfStop = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                    numOfCancel = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();
                }

                // Trường hợp chưa có công việc thì là Open (phòng trường hợp xóa công việc)
                if (numOfPlan == 0)
                {
                    product.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp có bất kỳ công việc nào đang Ongoing thì là Ongoing
                else if (numOfOngoing > 0)
                {
                    product.Status = (int)Constants.ScheduleStatus.Ongoing;
                }
                // Trường hợp tất cả công việc là Open
                else if (numOfPlan == numOfOpen)
                {
                    product.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp tất cả công việc là Close
                else if (numOfPlan == numOfClosed)
                {
                    product.Status = (int)Constants.ScheduleStatus.Closed;
                }
                // Trường hợp tất cả công việc là Stop
                else if (numOfPlan == numOfStop)
                {
                    product.Status = (int)Constants.ScheduleStatus.Stop;
                }
                // Trường hợp tất cả công việc là Cancel
                else if (numOfPlan == numOfCancel)
                {
                    product.Status = (int)Constants.ScheduleStatus.Cancel;
                }
                else
                {
                    if (numOfOpen > 0)
                    {
                        if (numOfClosed > 0)
                        {
                            product.Status = (int)Constants.ScheduleStatus.Ongoing;
                        }
                        else
                        {
                            product.Status = (int)Constants.ScheduleStatus.Open;
                        }

                    }
                    else
                    {
                        if (numOfClosed > 0)
                        {
                            product.Status = (int)Constants.ScheduleStatus.Closed;
                        }
                        else
                        {
                            if (numOfStop > 0)
                            {
                                product.Status = (int)Constants.ScheduleStatus.Stop;
                            }
                        }
                    }
                }

                db.SaveChanges();

                if (!string.IsNullOrEmpty(product.ParentId))
                {
                    this.ReCalculateDoneRatioProjectProduct(product.ParentId);
                }
            }
        }

        public string ExportExcelProjectPlan(ScheduleProjectSearchModel model)
        {
            ResultModel data = new ResultModel();
            if (!model.IsHistory)
            {
                data = GetListPlanByProjectId(model);
                var listProjectProductParent = data.listResult.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
                List<ScheduleEntity> listResult = new List<ScheduleEntity>();
                List<ScheduleEntity> listChild = new List<ScheduleEntity>();

                foreach (var parent in listProjectProductParent)
                {
                    listChild = GetScheduleProjectChild(parent.Id, data.listResult.ToList());
                    listResult.Add(parent);
                    listResult.AddRange(listChild);
                }
                data.listResult = listResult;
            }
            else
            {
                var getData = GetPlanHistoryInfo(model.PlanHistoryId);
                data = getData.Result;
            }

            var ProjectPlans = data.listResult.ToList();
            if (ProjectPlans.Count() == 0)
            {
                throw NTSException.CreateInstance("Không có tài liệu để export!");
            }
            var suppliers = db.Suppliers.ToList();
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProjectPlan.xlsm"));

            IWorksheet sheet = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets[4];
            IWorksheet sheet3 = workbook.Worksheets[13];
            IWorksheet sheet4 = workbook.Worksheets[2];

            IChart chart0 = sheet.Charts[0];
            IChart chart1 = sheet.Charts[1];

            var project = db.Projects.FirstOrDefault(a => a.Id.Equals(model.ProjectId));
            var customer = db.Customers.FirstOrDefault(a => a.Id.Equals(project.CustomerFinalId));
            var department = db.Departments.FirstOrDefault(a => a.Id.Equals(project.DepartmentId));
            var emplloyee = db.Employees.FirstOrDefault(a => a.Id.Equals(project.ManageId));

            sheet3.Range["O19"].Value = project.Name;

            sheet.Range["F4"].Value = project.Code;
            sheet.Range["F5"].Value = customer == null ? "" : customer.Name;
            sheet.Range["F6"].Value = project.DateFrom != null ? ((DateTime)project.DateFrom).ToString("dd-MM-yyyy") : null;
            sheet.Range["F7"].Value = department == null ? "" : department.Name;
            sheet.Range["F8"].Value = emplloyee == null ? "" : emplloyee.Name;

            sheet.Range["F11"].Value = project.Type == 1 ? "Dự án thông thường" : (project.Type == 2 ? "Ban quản lý dự án" : "Làm giải pháp");


            var total = ProjectPlans.Count;
            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
            var listExport = ProjectPlans.Select((a, i) => new
            {
                STT = "",
                NameViewLv1 = a.ParentId == null ? (a.ContractCode + " " + a.ContractName) : "       " + (a.ContractCode + " " + a.ContractName),
                Weigth = a.Weight,
                StageName = a.StageName,
                PlanName = a.PlanName,
                DailyWork = "",
                Type = a.IsPlan == false ? null : (a.Type == 0 ? "KH" : (a.Type == 1 ? "PC" : "PS")),
                ContractStartDate = a.ContractStartDate,
                ContractDueDate = a.ContractDueDate,
                PlanStartDate = a.PlanStartDate,
                PlanDueDate = a.PlanDueDate,
                WorkTime = a.Duration,
                //EstimateTime =a.EstimateTime,
                SupplierName = suppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)) == null ? ""
                : (suppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias == null ? suppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Name : suppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias),
                PlanAssignmentName = a.ResponsiblePersionName,
                InternalStatus = a.InternalStatus,
                Status = a.Status == 1 ? "Chưa triển khai" : (a.Status == 2 ? "Đang làm" : (a.Status == 3 ? "Đã xong" : "Dừng")),
                DoneRatio = (decimal)(a.DoneRatio) / (decimal)100
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 3, iRangeData.Row + total - 1, 19].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 3, iRangeData.Row + total - 1, 19].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 3, iRangeData.Row + total - 1, 19].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 3, iRangeData.Row + total - 1, 19].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 3, iRangeData.Row + total - 1, 19].Borders.Color = ExcelKnownColors.Black;
            for (int i = 0; i < ProjectPlans.Count(); i++)
            {
                if (!string.IsNullOrEmpty(ProjectPlans[i].Color))
                {
                    if (ProjectPlans[i].IsPlan == false && string.IsNullOrEmpty(ProjectPlans[i].NameView))
                    {
                        Color color = ColorTranslator.FromHtml(ProjectPlans[i].Color);
                        sheet.Range[iRangeData.Row + i, 6, iRangeData.Row + i, 18].CellStyle.Color = color;
                    }
                }
                if (!string.IsNullOrEmpty((ProjectPlans[i].NameView)) && string.IsNullOrEmpty(ProjectPlans[i].ParentId))
                {
                    sheet.Range[iRangeData.Row + i, 3, iRangeData.Row + i, 18].CellStyle.Color = ColorTranslator.FromHtml("#66FFFF");
                }
                if (!string.IsNullOrEmpty((ProjectPlans[i].NameView)) && !string.IsNullOrEmpty(ProjectPlans[i].ParentId))
                {
                    sheet.Range[iRangeData.Row + i, 3, iRangeData.Row + i, 18].CellStyle.Color = ColorTranslator.FromHtml("#FFFFCC");
                }

            }

            //xuất dashboard dự  án sheet 1 

            //Add sample data for pie chart
            //Add headings in A1 and B1.
            var dataChar = GetRatioDoneOfProject(model);
            sheet3.Range["O8"].Value = dataChar.Done.ToString();
            sheet3.Range["O9"].Value = dataChar.NoImplementation.ToString();
            sheet3.Range["O11"].Value = dataChar.Implementation.ToString();


            //Add sample data for bar chart
            //Add headings in A1 and B1.
            var dataChart1 = GetWorkOfProjectPlan(model.ProjectId);

            var projectPercen = pp.GetImplementationPlanVersusReality(model.ProjectId);
            sheet.Range["S17"].Value = projectPercen.DoneRatio;

            //set week
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            sheet.Range["R16"].Value = (cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - 10).ToString();

            //Add sample data for bar chart
            //Add headings in A1 and B1.
            var dataChar2 = GetNumberErrorOfProjectPlan(model.ProjectId);
            sheet3.Range["O15"].Value = dataChar2.Implementation.ToString();
            sheet3.Range["O16"].Value = dataChar2.NoImplementation.ToString();
            sheet3.Range["O17"].Value = dataChar2.RiskNoAction.ToString();

            //bảng tiến độ sp
            var productPlan = pp.GetProjectProductByProjectId(model.ProjectId);
            var total1 = productPlan.ProductPlans.Count;
            IRange iRangeData2 = sheet1.FindFirst("<Data2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData2.Text = iRangeData2.Text.Replace("<Data2>", string.Empty);
            if (productPlan.StageModels.Count > 1)
            {
                sheet1.InsertColumn(5, productPlan.StageModels.Count - 1, ExcelInsertOptions.FormatAsBefore);
            }
            StageModel stageModel = new StageModel();
            for (var i = 0; i < productPlan.StageModels.Count; i++)
            {
                sheet1.Range[iRangeData2.Row, iRangeData2.Column + i].Value = productPlan.StageModels[i].StageName.ToString();
            }
            IRange iRangeData0 = sheet1.FindFirst("<Data1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData0.Text = iRangeData0.Text.Replace("<Data1>", string.Empty);
            if (productPlan.ProductPlans.Count > 1)
            {
                sheet1.InsertRow(iRangeData0.Row + 1, productPlan.ProductPlans.Count - 1, ExcelInsertOptions.FormatAsBefore);
            }
            for (int x = 7; x < productPlan.ProductPlans.Count + 7; x++)
            {
                var k = 4;
                for (var y = 4; y < productPlan.ProductPlans[x - 7].StageModels.Count + 4; y++)
                {
                    sheet1.Range[x, k].Value = (productPlan.ProductPlans[x - 7].StageModels[y - 4].Status == 1) ? "Chưa triển khai" :
                        (productPlan.ProductPlans[x - 7].StageModels[y - 4].Status == 2 ? (productPlan.ProductPlans[x - 7].StageModels[y - 4].DoneRatio / (decimal)100).ToString() : (productPlan.ProductPlans[x - 7].StageModels[y - 4].Status == 3 ? "Hoàn thành" : "Không triển khai"));
                    sheet1.Range[x, k].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet1.Range[x, k].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet1.Range[x, k].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet1.Range[x, k].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet1.Range[x, k].Borders.Color = ExcelKnownColors.Black;
                    k = k + 1;

                }
            }

            IRange iRangeData1 = sheet1.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData1.Text = iRangeData1.Text.Replace("<Data>", string.Empty);
            var listExport1 = productPlan.ProductPlans.Select((a, i) => new
            {
                Index = i + 1,
                ProductName = a.ProductName
            });
            if (listExport1.Count() > 1)
            {
                //sheet1.InsertRow(iRangeData1.Row + 1, listExport1.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet1.ImportData(listExport1, iRangeData1.Row, iRangeData1.Column, false);
            sheet1.Range[iRangeData1.Row, 2, iRangeData1.Row + listExport1.Count() - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData1.Row, 2, iRangeData1.Row + listExport1.Count() - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData1.Row, 2, iRangeData1.Row + listExport1.Count() - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData1.Row, 2, iRangeData1.Row + listExport1.Count() - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData1.Row, 2, iRangeData1.Row + listExport1.Count() - 1, 3].Borders.Color = ExcelKnownColors.Black;

            var listProduct = ProjectPlans.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            foreach (var item in productPlan.ProductPlans.ToList())
            {
                var product = listProduct.FirstOrDefault(a => a.Id.Equals(item.PlanId));
                if (product != null)
                {
                    item.HDEndDate = product.ContractDueDate;
                    item.TKEndDate = product.PlanDueDate;
                }
            }

            IRange iRangeData3 = sheet1.FindFirst("<Data3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData3.Text = iRangeData3.Text.Replace("<Data3>", string.Empty);
            var listExport2 = productPlan.ProductPlans.Select((a, i) => new
            {
                ContractEndDate = a.HDEndDate != null ? ((DateTime)a.HDEndDate).Date : a.HDEndDate,
                PlanEndDate = a.TKEndDate != null ? ((DateTime)a.TKEndDate).Date : a.TKEndDate,
                ChenhLech = (a.HDEndDate == null || a.TKEndDate == null) ? null : (a.HDEndDate == a.TKEndDate ? "-" : (((DateTime)a.TKEndDate) - ((DateTime)a.HDEndDate)).Days.ToString())
            }).ToList();
            sheet1.ImportData(listExport2, iRangeData3.Row, iRangeData3.Column, false);
            sheet1.Range[iRangeData3.Row, iRangeData3.Column, iRangeData3.Row + listExport2.Count() - 1, iRangeData3.Column + 2].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData3.Row, iRangeData3.Column, iRangeData3.Row + listExport2.Count() - 1, iRangeData3.Column + 2].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData3.Row, iRangeData3.Column, iRangeData3.Row + listExport2.Count() - 1, iRangeData3.Column + 2].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData3.Row, iRangeData3.Column, iRangeData3.Row + listExport2.Count() - 1, iRangeData3.Column + 2].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet1.Range[iRangeData3.Row, iRangeData3.Column, iRangeData3.Row + listExport2.Count() - 1, iRangeData3.Column + 2].Borders.Color = ExcelKnownColors.Black;

            for (int i = 0; i < listExport2.Count(); i++)
            {
                try
                {
                    var delay = int.Parse(listExport2[i].ChenhLech);
                    if (delay < 0)
                    {
                        sheet1.Range[iRangeData3.Row + i, iRangeData3.Column + 2].CellStyle.Color = ColorTranslator.FromHtml("#FF0000");
                    }
                }
                catch
                {

                }
            }

            //xuat data bảng lệch (sheet chart tong quan)
            ChartTongQuan(sheet3, model.ProjectId, ProjectPlans);
            //xuat báo cáo theo khoảng ngày
            ReportPlan(model, sheet2, ProjectPlans);
            //isue
            GetError(model.ProjectId, sheet4);

            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Kế hoạch dự án" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Kế hoạch dự án" + ".xlsx";

            return resultPathClient;
        }

        private void ChartTongQuan(IWorksheet sheet3, string id, List<ScheduleEntity> ProjectPlans)
        {
            var allStageOfProject = (from a in db.Plans.AsNoTracking()
                                     join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                     where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && a.ProjectId.Equals(id)
                                     orderby b.index
                                     select new PlanImplementReality
                                     {
                                         StageId = b.Id,
                                         StageName = b.Name,
                                         StageCode = b.Code,
                                         ContractDate = a.ContractDueDate,
                                         RealDate = a.PlanDueDate,
                                         DateDelay = 0,
                                         PlanId = a.Id
                                     }).ToList();

            List<PlanImplementReality> stages = new List<PlanImplementReality>();
            foreach (var item in allStageOfProject)
            {
                var stage = stages.FirstOrDefault(a => a.StageId.Equals(item.StageId));
                if (stage == null)
                {
                    stages.Add(item);
                }
            }
            foreach (var item in stages)
            {
                var listStage = ProjectPlans.Where(a => item.StageId.Equals(a.StageId)).ToList();
                DateTime? maxDateC = default(DateTime);
                DateTime? maxDateP = default(DateTime);
                foreach (var item1 in listStage)
                {
                    if (item1.ContractDueDate != null)
                    {
                        if (item1.ContractDueDate >= maxDateC)
                        {
                            maxDateC = item1.ContractDueDate;
                        }
                    }
                    if (item1.PlanDueDate != null)
                    {
                        if (item1.PlanDueDate >= maxDateP)
                        {
                            maxDateP = item1.PlanDueDate;
                        }
                    }
                }
                item.ContractDate = maxDateC == default(DateTime) ? null : maxDateC;
                item.RealDate = maxDateP == default(DateTime) ? null : maxDateP;
            }
            IRange iRangeData = sheet3.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
            foreach (var item in stages.ToList())
            {
                if (item.ContractDate == null || item.RealDate == null)
                {
                    stages.Remove(item);
                }
            }

            var listExport = stages.Select((a, i) => new
            {
                Name = a.StageName,
                Code = a.StageCode,
                ContractDate = a.ContractDate,
                text1 = "",
                text2 = "",
                text3 = 0,
                PlanDate = a.RealDate,
                text4 = "",
                text5 = "",
                text6 = "",
                text7 = ""
            }).ToList();
            //sheet3.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            for (int i = iRangeData.Row; i < listExport.Count + iRangeData.Row; i++)
            {
                sheet3.Range[i, iRangeData.Column].Value = listExport[i - iRangeData.Row].Name;
                sheet3.Range[i, iRangeData.Column + 1].Value = listExport[i - iRangeData.Row].Code;
                sheet3.Range[i, iRangeData.Column + 2].Value = listExport[i - iRangeData.Row].ContractDate.ToString();
                sheet3.Range[i, iRangeData.Column + 5].Value = "0";
                sheet3.Range[i, iRangeData.Column + 6].Value = listExport[i - iRangeData.Row].PlanDate.ToString();
                sheet3.Range[i, iRangeData.Column + 3].Formula = "=+C" + i + "&CHAR(10)&TEXT(D" + i + "," + "\"dd / mmm\"" + ")";
                sheet3.Range[i, iRangeData.Column + 7].Formula = "=+C" + i + "&CHAR(10)&TEXT(H" + i + "," + "\"dd / mmm\"" + ")";
                sheet3.Range[i, iRangeData.Column + 9].Formula = "=+D" + i + "-H" + i + "";
                sheet3.Range[i, iRangeData.Column + 10].Formula = "=+C" + i + "&CHAR(10)&K" + i + "";
            }
        }

        public void ReportPlan(ScheduleProjectSearchModel model, IWorksheet sheet2, List<ScheduleEntity> projectPlans)
        {
            var project = db.Projects.FirstOrDefault(a => a.Id.Equals(model.ProjectId));
            sheet2.Range["D5"].Value = project.Code;
            if (project.DateFrom != null)
            {
                sheet2.Range["D6"].Value = ((DateTime)project.DateFrom).ToStringDDMMYY();
            }
            else
            {
                sheet2.Range["D6"].Value = null;
            }
            sheet2.Range["D9"].DateTime = model.PlanStartDate;
            sheet2.Range["D10"].DateTime = model.PlanEndDate;
            List<string> users = new List<string>();
            foreach (var item in projectPlans)
            {
                users.AddRange(item.ListIdUserId);
            }
            List<string> userResults = new List<string>();
            List<Object> employeeResults = new List<Object>();

            foreach (var sup in users)
            {
                if (userResults.FirstOrDefault(a => a.Equals(sup)) == null)
                {
                    userResults.Add(sup);
                    var employee = (from a in db.Users.AsNoTracking()
                                    join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                                    join c in db.EmployeeGroups.AsNoTracking() on b.EmployeeGroupId equals c.EmployeeGroupId
                                    where a.Id.Equals(sup)
                                    select new
                                    {
                                        EmployeeId = b.Id,
                                        EmployeeGroupId = b.EmployeeGroupId,
                                        EmployeeGroupName = c.Name
                                    }).AsQueryable();
                    employeeResults.Add(employee);
                }
            }
            List<string> suppliers = new List<string>();
            var listSuppliers = db.Suppliers.ToList();
            foreach (var item in projectPlans)
            {
                if (item.SupplierId != null)
                {
                    var supplierName = listSuppliers.FirstOrDefault(a => item.SupplierId.Equals(a.Id)).Alias == null
                        ? listSuppliers.FirstOrDefault(a => item.SupplierId.Equals(a.Id)).Name.ToUpper()
                        : listSuppliers.FirstOrDefault(a => item.SupplierId.Equals(a.Id)).Alias.ToUpper();
                    if (!supplierName.Equals("ETEK"))
                    {
                        if (suppliers.FirstOrDefault(a => a.Equals(supplierName)) == null)
                        {
                            suppliers.Add(supplierName);
                        }
                    }
                }
            }

            IRange iRangeData = sheet2.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
            var listExport = suppliers.Select((a, i) => new
            {
                Name = a,
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport.Count() > 1)
            {
                sheet2.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet2.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);

            var listProduct = db.ProjectProducts.ToList();
            var products = db.Products.ToList();
            var modules = db.Modules.ToList();
            model.PlanEndDate = model.PlanEndDate.AddDays(1);
            var listPlan = (from a in db.Projects.AsNoTracking()
                            join b in db.Plans.AsNoTracking() on a.Id equals b.ProjectId
                            join c in db.Stages.AsNoTracking() on b.StageId equals c.Id
                            join d in db.ProjectProducts.AsNoTracking() on b.ProjectProductId equals d.Id
                            where b.ProjectId.Equals(model.ProjectId) && b.IsPlan == true && b.PlanDueDate >= model.PlanStartDate && b.PlanDueDate < model.PlanEndDate
                            select new ScheduleProjectResultModel
                            {
                                Id = b.Id,
                                StageName = c.Name.ToUpper(),
                                PlanName = b.Name.ToUpper(),
                                Type = b.Type,
                                ContractStartDate = b.ContractStartDate,
                                ContractDueDate = b.ContractDueDate,
                                PlanStartDate = b.PlanStartDate,
                                PlanDueDate = b.PlanDueDate,
                                EstimateTime = b.Duration,
                                SupplierId = b.SupplierId,
                                Status = b.Status,
                                DoneRatio = b.DoneRatio,
                                ProjectProductId = d.Id
                            }).ToList();
            //xuất ngày kết thucs trong khoảng đang triển khai
            var planAssignment = db.PlanAssignments.AsNoTracking().ToList();
            var listUsers = db.Users.AsNoTracking().ToList();
            var employees = db.Employees.AsNoTracking().ToList();
            foreach (var item in listPlan)
            {
                item.ListIdUserId = planAssignment.Where(a => a.PlanId.Equals(item.Id)).OrderBy(a => a.IsMain).Select(a => a.UserId).ToList();
                item.ResponsiblePersionName = string.Join(", ", (from s in planAssignment
                                                                 where s.PlanId.Equals(item.Id)
                                                                 join m in listUsers on s.UserId equals m.Id
                                                                 join e in employees on m.EmployeeId equals e.Id
                                                                 orderby s.IsMain descending
                                                                 select e.Name).ToArray());
                item.NameView = GetProductName(item.ProjectProductId, listProduct, products, modules);
            }

            //
            var listPlan1 = listPlan.Where(a => a.Status == 3).ToList();

            iRangeData = sheet2.FindFirst("<Data1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data1>", string.Empty);
            var listExport1 = listPlan1.Select((a, i) => new
            {
                STT = i + 1,
                Name = a.NameView,
                a.StageName,
                a.PlanName,
                Type = a.Type == 1 ? "Kế hoạch" : (a.Type == 2 ? "Phát sinh" : (a.Type == 3 ? "Phát sinh" : "")),
                a.ContractStartDate,
                a.ContractDueDate,
                a.PlanStartDate,
                a.PlanDueDate,
                a.EstimateTime,
                SupplierName = a.SupplierId == null ? "" : (listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias == null
                ? listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Name.ToUpper() : listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias.ToUpper()),
                a.ResponsiblePersionName,
                Status = "Close",
                a.DoneRatio

            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport1.Count() > 1)
            {
                sheet2.InsertRow(iRangeData.Row + 1, listExport1.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet2.ImportData(listExport1, iRangeData.Row, iRangeData.Column, false);

            //xuất ngày kết thucs trong khoảng đang tồn
            var listPlan2 = listPlan.Where(a => a.Status == 1 || a.Status == 2).ToList();

            iRangeData = sheet2.FindFirst("<Data2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data2>", string.Empty);
            var listExport2 = listPlan2.Select((a, i) => new
            {
                STT = i + 1,
                Name = a.NameView,
                a.StageName,
                a.PlanName,
                Type = a.Type == 1 ? "Kế hoạch" : (a.Type == 2 ? "Phát sinh" : (a.Type == 3 ? "Phát sinh" : "")),
                a.ContractStartDate,
                a.ContractDueDate,
                a.PlanStartDate,
                a.PlanDueDate,
                a.EstimateTime,
                SupplierName = a.SupplierId == null ? "" : (listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias == null
                ? listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Name.ToUpper() : listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias.ToUpper()),
                a.ResponsiblePersionName,
                Status = a.Status == 1 ? "Open" : "On-Going",
                a.DoneRatio

            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport2.Count() > 1)
            {
                sheet2.InsertRow(iRangeData.Row + 1, listExport2.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet2.ImportData(listExport2, iRangeData.Row, iRangeData.Column, false);


            //xuất ngày kết thucs trong tuần tới
            var start = model.PlanEndDate;
            var end = model.PlanEndDate.AddDays(8);
            var listPlan3 = (from a in db.Projects.AsNoTracking()
                             join b in db.Plans.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Stages.AsNoTracking() on b.StageId equals c.Id
                             join d in db.ProjectProducts.AsNoTracking() on b.ProjectProductId equals d.Id
                             where b.ProjectId.Equals(model.ProjectId) && b.IsPlan == true && b.PlanDueDate >= start && b.PlanDueDate <= end
                             select new ScheduleProjectResultModel
                             {
                                 Id = b.Id,
                                 StageName = c.Name.ToUpper(),
                                 PlanName = b.Name.ToUpper(),
                                 Type = b.Type,
                                 ContractStartDate = b.ContractStartDate,
                                 ContractDueDate = b.ContractDueDate,
                                 PlanStartDate = b.PlanStartDate,
                                 PlanDueDate = b.PlanDueDate,
                                 EstimateTime = b.Duration,
                                 SupplierId = b.SupplierId,
                                 Status = b.Status,
                                 ProjectProductId = d.Id
                             }).ToList();
            //xuất ngày kết thucs trong khoảng đang triển khai
            foreach (var item in listPlan3)
            {
                item.ListIdUserId = planAssignment.Where(a => a.PlanId.Equals(item.Id)).OrderBy(a => a.IsMain).Select(a => a.UserId).ToList();
                item.ResponsiblePersionName = string.Join(", ", (from s in planAssignment
                                                                 where s.PlanId.Equals(item.Id)
                                                                 join m in listUsers on s.UserId equals m.Id
                                                                 join e in employees on m.EmployeeId equals e.Id
                                                                 orderby s.IsMain descending
                                                                 select e.Name).ToArray());
                item.NameView = GetProductName(item.ProjectProductId, listProduct, products, modules);
            }

            iRangeData = sheet2.FindFirst("<Data3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data3>", string.Empty);
            var listExport3 = listPlan3.Select((a, i) => new
            {
                STT = i + 1,
                Name = a.NameView,
                a.StageName,
                a.PlanName,
                Type = a.Type == 1 ? "Kế hoạch" : (a.Type == 2 ? "Phát sinh" : (a.Type == 3 ? "Phát sinh" : "")),
                a.ContractStartDate,
                a.ContractDueDate,
                a.PlanStartDate,
                a.PlanDueDate,
                a.EstimateTime,
                SupplierName = a.SupplierId == null ? "" : (listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias == null
                ? listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Name.ToUpper() : listSuppliers.FirstOrDefault(b => b.Id.Equals(a.SupplierId)).Alias.ToUpper()),
                a.ResponsiblePersionName,
                Status = a.Status == 1 ? "Open" : (a.Status == 2 ? "On-Going" : "Close")
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport3.Count() > 1)
            {
                sheet2.InsertRow(iRangeData.Row + 1, listExport3.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet2.ImportData(listExport3, iRangeData.Row, iRangeData.Column, false);


            ITextBoxShape textBox = sheet2.TextBoxes.AddTextBox(3, 17, 300, 400);
            textBox.Text = project.Progress == null ? "" : project.Progress;
            //Format the control
            textBox.Fill.ForeColor = Color.Pink;
            textBox.Fill.BackColor = Color.Black;

            //
            List<ErrorModel> errorModels = new List<ErrorModel>();
            var errors = db.Errors.Where(a => a.ProjectId.Equals(project.Id)).ToList();
            var module = db.Modules.ToList();
            var errorFixs = db.ErrorFixs.ToList();
            var stages = db.Stages.ToList();
            foreach (var item in errors)
            {
                var errorFixOfErrors = errorFixs.Where(a => a.ErrorId.Equals(item.Id) && a.DateTo != null && a.DateTo >= model.PlanStartDate && a.DateTo <= model.PlanEndDate).ToList();
                DateTime? date = errorFixOfErrors.Select(a => a.DateTo).Max();
                if ((date != null && date >= model.PlanStartDate && date <= model.PlanEndDate))
                {
                    ErrorModel errorModel = new ErrorModel();
                    errorModel.ObjectName = module.FirstOrDefault(a => a.Id.Equals(item.ObjectId)) == null ? null : module.FirstOrDefault(a => a.Id.Equals(item.ObjectId)).Code.ToUpper() + "."
                                        + module.FirstOrDefault(a => a.Id.Equals(item.ObjectId)).Name.ToUpper();
                    errorModel.StageName = !string.IsNullOrEmpty(item.StageId) ? stages.FirstOrDefault(a => a.Id.Equals(item.StageId)).Name.ToUpper() : null;
                    errorModel.Subject = item.Subject;
                    errorModel.Note = item.Note;
                    errorModel.ActualFinishDate = date;
                    var solutions = errorFixOfErrors.Select(b => b.Solution).ToList();
                    errorModel.Solution = string.Join("\n ", solutions.ToArray());
                    errorModel.Status = item.Status;
                    errorModels.Add(errorModel);
                }

            }
            var ListError = new Dictionary<int, string>  {
                { 1, "Đang tạo"},
                { 2, "Chờ xác nhận" },
                { 4, "Chưa có kế hoạch" },
                { 5, "Đang xử lý" },
                { 6, "Đang QC" },
                { 7, "QC đạt" },
                { 8, "QC không đạt" },
                { 9, "Đóng vấn đề của dự án" },
                { 10,"Đã khắc phục triệt để" }
                }.ToList();

            iRangeData = sheet2.FindFirst("<Data4>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data4>", string.Empty);
            var listExport4 = errorModels.Select((a, i) => new
            {
                STT = i + 1,
                a.ObjectName,
                a.StageName,
                a.Subject,
                a.Note,
                a.Solution,
                a.ActualFinishDate,
                Test = "",
                Comment = "",
                Status = ListError.FirstOrDefault(b => b.Key == a.Status).Value
            });
            //listExport = listExport.OrderByDescending(a => a.type).ToList();
            if (listExport4.Count() > 1)
            {
                sheet2.InsertRow(iRangeData.Row + 1, listExport4.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet2.ImportData(listExport4, iRangeData.Row, iRangeData.Column, false);


        }

        public object GetListPlanByMonth(ScheduleProjectSearchModel modelSearch)
        {
            //List<DateTime> daysOfMonth = Enumerable.Range(1, DateTime.DaysInMonth(modelSearch.Date.Year, modelSearch.Date.Month))  // Days: 1, 2 ... 31 etc.
            //                            .Select(day => new DateTime(modelSearch.Date.Year, modelSearch.Date.Month, day)) // Map each day to a date
            //                            .ToList();

            List<DateTime> daysOfMonth = Enumerable.Range(0, modelSearch.DateEnd.Subtract(modelSearch.DateStart).Days + 1).Select(d => modelSearch.DateStart.AddDays(d)).ToList();

            List<int> dayOfWeek = new List<int>();

            foreach (var item in daysOfMonth)
            {
                dayOfWeek.Add((int)item.DayOfWeek + 1);
            }

            List<DateTimeModel> holidays = new List<DateTimeModel>();
            var listHoliday = db.Holidays.AsNoTracking().Where(a => a.HolidayDate <= modelSearch.DateEnd && a.HolidayDate >= modelSearch.DateStart).ToList();
            DateTimeModel dateTimeModel;
            if (listHoliday.Count > 0)
            {
                foreach (var item in daysOfMonth)
                {
                    dateTimeModel = new DateTimeModel();
                    if (listHoliday.FirstOrDefault(a => a.HolidayDate == item) != null)
                    {
                        dateTimeModel.IsHoliday = true;
                    }
                    else
                    {
                        dateTimeModel.IsHoliday = false;
                    }
                    dateTimeModel.DateTime = item;
                    holidays.Add(dateTimeModel);
                }
            }
            else
            {
                foreach (var item in daysOfMonth)
                {
                    dateTimeModel = new DateTimeModel();
                    dateTimeModel.DateTime = item;
                    dateTimeModel.IsHoliday = false;
                    holidays.Add(dateTimeModel);
                }
            }


            return new { holidays, daysOfMonth, dayOfWeek };
        }

        private List<ScheduleEntity> GetScheduleProjectChildIndex(ScheduleEntity parent,
          List<ScheduleEntity> listSchedulePrject)
        {
            List<ScheduleEntity> listResult = new List<ScheduleEntity>();
            List<ScheduleEntity> listChild = new List<ScheduleEntity>();

            var listData = listSchedulePrject.Where(r => parent.Id.Equals(r.ParentId)).ToList();

            listChild = listData.Where(a => !string.IsNullOrEmpty(a.StageId)).OrderBy(r => r.Index).ThenBy(r => r.IndexPlan).ToList();
            listChild.AddRange(listData.Where(a => string.IsNullOrEmpty(a.StageId)).ToList());

            List<ScheduleEntity> listChildChild = new List<ScheduleEntity>();
            DateTime dateTime = DateTime.Today;
            DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

            foreach (ScheduleEntity child in listChild)
            {
                listChildChild = GetScheduleProjectChildIndex(child, listSchedulePrject);

                // Trường hợp là sản phẩm, module
                if (!child.IsPlan)
                {
                    // Trường hợp là sản phẩm
                    if (string.IsNullOrEmpty(child.StageId))
                    {
                        if (child.Status == (int)Constants.ScheduleStatus.Closed)
                        {
                            child.InternalStatus = "OK";
                        }
                        else if (listChildChild.Where(r => !r.IsPlan && !r.ContractStartDate.HasValue).Any())
                        {
                            child.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
                        }
                        else if (listChildChild.Where(r => !r.IsPlan && !r.ContractDueDate.HasValue).Any())
                        {
                            child.InternalStatus = "TRỐNG KẾT THÚC HĐ";
                        }
                        else if (listChildChild.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || !child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                        {
                            child.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                        }
                        else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                        {
                            child.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                        }
                        else if (child.PlanDueDate.HasValue && today > child.PlanDueDate.Value && (child.DoneRatio < 100))
                        {
                            child.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                        }
                        else
                        {
                            child.InternalStatus = "OK";
                        }
                    }
                    else
                    {
                        if (child.Status == (int)Constants.ScheduleStatus.Closed)
                        {
                            child.InternalStatus = "OK";
                        }
                        else if (!child.ContractStartDate.HasValue)
                        {
                            child.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
                        }
                        else if (!child.ContractDueDate.HasValue)
                        {
                            child.InternalStatus = "TRỐNG KẾT THÚC HĐ";
                        }
                        else if (listChildChild.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || !child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                        {
                            child.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                        }
                        else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                        {
                            child.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                        }
                        else if (child.PlanDueDate.HasValue && today > child.PlanDueDate.Value && (child.DoneRatio < 100))
                        {
                            child.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                        }
                        else
                        {
                            child.InternalStatus = "OK";
                        }
                    }
                }
                else
                {
                    if (child.Status == (int)Constants.ScheduleStatus.Closed)
                    {
                        child.InternalStatus = "OK";
                    }
                    else if (!child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                    {
                        child.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                    }
                    else if (today > child.PlanDueDate.Value && child.DoneRatio < 100)
                    {
                        child.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                    else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                    {
                        child.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                    }
                    else
                    {
                        child.InternalStatus = "OK";
                    }
                }


                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }
            return listResult;
        }

        private List<ScheduleEntity> GetScheduleProjectChild(string parentId,
          List<ScheduleEntity> listSchedulePrject)
        {
            List<ScheduleEntity> listResult = new List<ScheduleEntity>();
            var listChild = listSchedulePrject.Where(r => parentId.Equals(r.ParentId)).ToList();

            List<ScheduleEntity> listChildChild = new List<ScheduleEntity>();
            DateTime dateTime = DateTime.Today;
            foreach (var child in listChild)
            {
                listChildChild = GetScheduleProjectChild(child.Id, listSchedulePrject);

                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }
            return listResult;
        }

        private List<ScheduleEntity> GetScheduleProjectParent(string childId,
          List<ScheduleEntity> listSchedulePrject)
        {
            List<ScheduleEntity> listResult = new List<ScheduleEntity>();
            var parent = listSchedulePrject.FirstOrDefault(r => childId.Equals(r.Id));
            List<ScheduleEntity> listParent = new List<ScheduleEntity>();
            if (parent != null)
            {
                if (!string.IsNullOrEmpty(parent.ParentId))
                {
                    listParent = GetScheduleProjectParent(parent.ParentId, listSchedulePrject);
                }
                listResult.Add(parent);
                listResult.AddRange(listParent);
            }
            return listResult;
        }

        public string GetProductName(string productId, List<ProjectProduct> ProjectProducts, List<Product> products, List<Module> modules)
        {
            var projectproduct = ProjectProducts.FirstOrDefault(a => productId.Equals(a.Id));
            var parent = ProjectProducts.FirstOrDefault(a => a.Id.Equals(projectproduct.ParentId));
            if (parent == null)
            {
                var product = products.FirstOrDefault(a => a.Id.Equals(projectproduct.ProductId));
                var module = modules.FirstOrDefault(a => a.Id.Equals(projectproduct.ModuleId));
                return projectproduct.ContractCode.ToUpper() + " " + (product != null ? product.Name.ToUpper() : (module != null ? module.Name.ToUpper() : ""));
            }
            if (string.IsNullOrEmpty(parent.ParentId))
            {
                var product = products.FirstOrDefault(a => a.Id.Equals(parent.ProductId));
                var module = modules.FirstOrDefault(a => a.Id.Equals(parent.ModuleId));
                return parent.ContractCode.ToUpper() + " " + (product != null ? product.Name.ToUpper() : (module != null ? module.Name.ToUpper() : ""));
            }
            else
            {
                GetProductName(parent.ParentId, ProjectProducts, products, modules);
            }
            return null;

        }
        public object GetListProjectProductByProjectId(string projectId)
        {
            List<ProjectProductsModel> searchResult = new List<ProjectProductsModel>();
            try
            {
                var ListModel = (from a in db.ProjectProducts.AsNoTracking()
                                 join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                                 join c in db.Modules.AsNoTracking() on a.ModuleId equals c.Id into ac
                                 from acn in ac.DefaultIfEmpty()
                                 where a.ProjectId.Equals(projectId)
                                 select new ProjectProductsModel()
                                 {
                                     Id = a.Id,
                                     Name = acn != null ? acn.Name : a.ContractName,
                                     Code = acn != null ? acn.Code : a.ContractCode,
                                     CodeView = acn != null ? acn.Code : (!string.IsNullOrEmpty(a.ContractCode) ? a.ContractCode : a.ContractName),
                                     ParentId = a.ParentId,
                                     ExpectedDesignFinishDate = a.ExpectedDesignFinishDate,
                                     ExpectedMakeFinishDate = a.ExpectedMakeFinishDate,
                                     ExpectedTransferDate = a.ExpectedTransferDate,
                                     ModuleId = a.ModuleId,
                                     ModuleGroupId = acn != null ? acn.ModuleGroupId : string.Empty,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public object GetTaskTimeStandardByTaskId(TaskTimeStandardSearchModel modelSearch)
        {
            var result = (from a in db.TaskTimeStandards.AsNoTracking()
                          where a.EmployeeId.Equals(modelSearch.EmployeeId) && a.ModuleGroupId.Equals(modelSearch.ModuleGroupId)
                          && a.TaskId.Equals(modelSearch.TaskId)
                          select new TaskTimeStandardModel
                          {
                              TimeStandard = a.TimeStandard,
                          }).FirstOrDefault();
            var data = (from a in db.TaskModuleGroups.AsNoTracking()
                        where a.ModuleGroupId.Equals(modelSearch.ModuleGroupId) && a.TaskId.Equals(modelSearch.TaskId)
                        select new TaskTimeStandardModel
                        {
                            Index = a.Index
                        }).FirstOrDefault();
            if (data != null && result != null)
            {
                result.Index = data.Index;
            }

            var moduleGroup = db.ModuleGroupTimeStandards.AsNoTracking().FirstOrDefault(i => i.DepartmentId.Equals(modelSearch.DepartmentId) && i.ModuleGroupId.Equals(modelSearch.ModuleGroupId));

            return new
            {
                TimeStandard = result != null ? result.TimeStandard : 0,
                Index = result != null ? result.Index : 0,
                Status = moduleGroup != null ? moduleGroup.Status : 0
            };
        }

        public StatisticalProjectModel GetStatisticalProject(ScheduleProjectSearchModel searchModel)
        {
            StatisticalProjectModel statisticalProjectModel = new StatisticalProjectModel();
            var queryProject = (from a in db.Projects.AsNoTracking()
                                select new ScheduleProjectResultModel()
                                {
                                    Id = a.Id,
                                    ProjectId = a.Id,
                                    ParentId = null,
                                    PlanCode = a.Code + " - " + a.Name,
                                    DepartmentId = a.DepartmentId,
                                    SbuId = a.SBUId,
                                    NameView = "DA - " + a.Code + " - " + a.Name,
                                    StatusProject = a.Status,
                                    StartDate = a.DateFrom,
                                    KickOffDate = a.KickOffDate
                                }).AsQueryable();

            if (searchModel.DateFrom.HasValue)
            {
                queryProject = queryProject.Where(u => u.StartDate != null ? u.StartDate >= searchModel.DateFrom : u.KickOffDate >= searchModel.DateFrom);

            }

            if (searchModel.DateTo.HasValue)
            {
                queryProject = queryProject.Where(u => u.StartDate != null ? u.StartDate <= searchModel.DateTo : u.KickOffDate <= searchModel.DateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                if (queryProject.ToList().Count() > 0)
                {
                    statisticalProjectModel.TotalProject = queryProject.Where(a => a.DepartmentId.Equals(searchModel.DepartmentId)).Count();
                    statisticalProjectModel.TotalProjectDoing = queryProject.Where(a => a.DepartmentId.Equals(searchModel.DepartmentId) && !a.StatusProject.Equals(Constants.Prooject_Status_Finish)).Count();
                    statisticalProjectModel.TotalProjectDone = queryProject.Where(a => a.DepartmentId.Equals(searchModel.DepartmentId) && a.StatusProject.Equals(Constants.Prooject_Status_Finish)).Count();
                }

            }
            return statisticalProjectModel;
        }
        public string ExportExcelProjectSchedule(ScheduleProjectSearchModel modelSearch, string sbuId)
        {
            var data = GetListPlanByProjectId(modelSearch);

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_ProjectSchedule.xlsx"));


            var listTask = db.Tasks.AsNoTracking().Select(i => i.Name).ToList();
            var listEmploye = (from e in db.Employees.AsNoTracking()
                               join d in db.Departments.AsNoTracking() on e.DepartmentId equals d.Id
                               join u in db.Users.AsNoTracking() on e.Id equals u.EmployeeId
                               where (string.IsNullOrEmpty(sbuId) || d.SBUId.Equals(sbuId))
                               select u.UserName).ToList();

            IWorksheet sheet = workbook.Worksheets[0];

            //var total = data.Count();

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listParent = data.listResult.Where(a => string.IsNullOrEmpty(a.ParentId));
            List<ScheduleProjectResultModel> listResult = new List<ScheduleProjectResultModel>();
            List<ScheduleProjectResultModel> listChild = new List<ScheduleProjectResultModel>();
            //var listExport ;
            //foreach (var parent in listParent)
            //{
            //    listChild = GetScheduleProjectChild(parent.Id, data.listResult);
            //    foreach (var item in listChild.Where(a => string.IsNullOrEmpty(a.StageId)))
            //    {
            //        listExport.add(new ExportScheduleProjectModel()
            //        {       
            //            ContractCode = parent.ContractCode,
            //            ContractName = item.NameView,
            //            ModuleCode = parent.ModuleCode,
            //            Design = listChild.FirstOrDefault(a => a.StageName.Equals("Thiết kế")) != null ? item.Status : "NA",

            //        });
            //    }

            //}




            //if (listExport.Count() > 1)
            //{
            //    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            //}

            //IWorksheet sheet1 = workbook.Worksheets[1];
            //IWorksheet sheet2 = workbook.Worksheets["nts-data"];

            //var listExport_1 = listTask.OrderBy(a => a).Select((o, i) => new
            //{
            //    o,
            //});

            //var listExport_2 = listEmploye.OrderBy(a => a).Select((o, i) => new
            //{
            //    o,
            //});

            //sheet.Range["I2:I6000"].DataValidation.DataRange = sheet1.Range["A1:A" + listExport_1.Count()];
            //sheet.Range["K2:K6000"].DataValidation.DataRange = sheet1.Range["B1:B" + listExport_2.Count()];

            //IRange iRangeData_1 = sheet1.FindFirst("<taskName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //iRangeData_1.Text = iRangeData_1.Text.Replace("<taskName>", string.Empty);
            //IRange iRangeData_2 = sheet2.FindFirst("<employee>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //iRangeData_2.Text = iRangeData_2.Text.Replace("<employee>", string.Empty);

            ////if (data.Count() > 1)
            ////{
            ////    sheet.InsertRow(iRangeData.Row + 1, data.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            ////}
            //sheet1.ImportData(listExport_1, iRangeData_1.Row, iRangeData_1.Column, false);
            //sheet1.ImportData(listExport_2, iRangeData_2.Row, iRangeData_2.Column, false);
            //sheet.ImportData(data, iRangeData.Row, iRangeData.Column, false);

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách kế hoạch thiết kế_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;
        }

        public string ExportExcel(ScheduleProjectSearchModel modelSearch, string sbuId)
        {
            var projectProducts = (from p in db.ProjectProducts.AsNoTracking()
                                   join pr in db.Projects.AsNoTracking() on p.ProjectId equals pr.Id
                                   join m in db.Modules.AsNoTracking() on p.ModuleId equals m.Id into pm
                                   from pmn in pm.DefaultIfEmpty()
                                   join pp in db.Products.AsNoTracking() on p.ProductId equals pp.Id into ppp
                                   from ppn in ppp.DefaultIfEmpty()
                                   where !pr.Status.Equals(Constants.Prooject_Status_Finish) && (string.IsNullOrEmpty(sbuId) || pr.SBUId.Equals(sbuId))
                                   select new
                                   {
                                       p.ContractIndex,
                                       p.ContractName,
                                       p.ContractCode,
                                       ProjectCode = pr.Code,
                                       ProjectName = pr.Name,
                                       DesignName = pmn != null ? pmn.Name : ppn != null ? ppn.Name : string.Empty,
                                       DesignCode = pmn != null ? pmn.Code : ppn != null ? ppn.Code : string.Empty,
                                       ProjectProductId = p.Id,
                                   }).ToList();

            if (projectProducts.Count() > 0)
            {
                int maxLen = projectProducts.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                projectProducts = projectProducts
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            projectProducts = projectProducts.OrderBy(o => o.ProjectCode).ToList();


            //var data = (from a in projectProducts
            //            join b in plans on a.ProjectProductId equals b.ProjectProductId into ab
            //            from abn in ab.DefaultIfEmpty()
            //            select new
            //            {
            //                a.ProjectProductId,
            //                PlanId = abn != null ? abn.Id : string.Empty,
            //                a.ContractIndex,
            //                a.ProjectCode,
            //                a.ProjectName,
            //                Medium = string.Empty,
            //                a.ContractName,
            //                a.DesignCode,
            //                PlanName = abn != null ? abn.Name : string.Empty,
            //                PlanStatus = abn != null ? abn.Status : string.Empty,
            //                PlanBy = abn != null ? abn.EmployeeName : string.Empty,
            //                PlanDepartment = string.Empty,
            //                PlanEstimate = abn != null ? (decimal?)abn.EsimateTime : null,
            //                PlanExecutionTime = abn != null ? (decimal?)abn.ExecutionTime : null,
            //                PlanStandardTime = (decimal?)null,
            //                StartDate = abn != null ? abn.StartDate : null,
            //                EndDate = abn != null ? abn.EndDate : null,
            //            }).ToList();

            //if (data.Count == 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0039, TextResourceKey.ScheduleProject);
            //}

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_Schedule_Project.xlsx"));


            var listTask = db.Tasks.AsNoTracking().Select(i => i.Name).ToList();
            var listEmploye = (from e in db.Employees.AsNoTracking()
                               join d in db.Departments.AsNoTracking() on e.DepartmentId equals d.Id
                               join u in db.Users.AsNoTracking() on e.Id equals u.EmployeeId
                               where (string.IsNullOrEmpty(sbuId) || d.SBUId.Equals(sbuId))
                               select u.UserName).ToList();

            IWorksheet sheet = workbook.Worksheets[0];

            //var total = data.Count();

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets["nts-data"];

            var listExport_1 = listTask.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            var listExport_2 = listEmploye.OrderBy(a => a).Select((o, i) => new
            {
                o,
            });

            sheet.Range["I2:I6000"].DataValidation.DataRange = sheet1.Range["A1:A" + listExport_1.Count()];
            sheet.Range["K2:K6000"].DataValidation.DataRange = sheet1.Range["B1:B" + listExport_2.Count()];

            IRange iRangeData_1 = sheet1.FindFirst("<taskName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData_1.Text = iRangeData_1.Text.Replace("<taskName>", string.Empty);
            IRange iRangeData_2 = sheet2.FindFirst("<employee>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData_2.Text = iRangeData_2.Text.Replace("<employee>", string.Empty);

            //if (data.Count() > 1)
            //{
            //    sheet.InsertRow(iRangeData.Row + 1, data.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            //}
            sheet1.ImportData(listExport_1, iRangeData_1.Row, iRangeData_1.Column, false);
            sheet1.ImportData(listExport_2, iRangeData_2.Row, iRangeData_2.Column, false);
            //sheet.ImportData(data, iRangeData.Row, iRangeData.Column, false);

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách kế hoạch thiết kế_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;

        }


        /// <summary>
        /// Báo cáo công việc
        /// </summary>
        public SearchResultModel<WorkingReportModel> SearchWorkingReport(WorkingReportModel modelSearch)
        {
            SearchResultModel<WorkingReportModel> searchResult = new SearchResultModel<WorkingReportModel>();

            var dataQuery = (from a in db.Plans.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id into ab
                             from abc in ab.DefaultIfEmpty()
                             join c in db.Stages.AsNoTracking() on a.StageId equals c.Id into ac
                             from acc in ac.DefaultIfEmpty()
                             join d in db.Suppliers.AsNoTracking() on a.SupplierId equals d.Id into ad
                             from adc in ad.DefaultIfEmpty()
                             where a.IsPlan
                             orderby abc.ContractName
                             select new WorkingReportModel()
                             {

                                 Id = a.Id,
                                 ContractName = abc.ContractName,
                                 StageName = acc.Name,
                                 Name = a.Name,
                                 Type = a.Type,
                                 ContractDueDate = a.ContractDueDate,
                                 ContractStartDate = a.ContractStartDate,
                                 ActualStartDate = a.ActualStartDate,
                                 ActualEndDate = a.ActualEndDate,
                                 ContractorName = adc.Name,
                                 Status = a.Status,
                                 DoneRatio = a.DoneRatio,
                                 ProjectId = a.ProjectId,
                                 PlanStartDate = a.PlanStartDate,
                                 PlanDueDate = a.PlanDueDate,

                             }).AsQueryable();
            var listHolidayDataBase = db.Holidays.AsNoTracking().ToList();
            List<string> listUser;

            if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            {
                dataQuery = dataQuery.Where(r => r.ProjectId.Equals(modelSearch.ProjectId));
            }

            if (modelSearch.DateFromActualStartDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanStartDate >= modelSearch.DateFromActualStartDate);

            }

            if (modelSearch.DateToActualStartDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanStartDate <= modelSearch.DateToActualStartDate);
            }


            if (modelSearch.DateFromActualEndDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanDueDate >= modelSearch.DateFromActualEndDate);

            }

            if (modelSearch.DateToActualEndDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanDueDate <= modelSearch.DateToActualEndDate);
            }



            searchResult.ListResult = dataQuery.ToList();
            foreach (var item in searchResult.ListResult)
            {
                int a = 0;
                if (item.PlanDueDate != null && item.PlanStartDate != null)
                {
                    TimeSpan time = (TimeSpan)(item.PlanDueDate - item.PlanStartDate);
                    int number = time.Days;

                    foreach (var ite in listHolidayDataBase)
                    {
                        if (item.PlanStartDate <= ite.HolidayDate && item.PlanDueDate >= ite.HolidayDate)
                        {
                            a++;
                        }
                    }
                    item.NumberDate = number - a + 1;
                }
                else
                {
                    item.NumberDate = 0;
                }



                item.PlanUser = string.Join(", ", (from s in db.PlanAssignments
                                                   where s.PlanId.Equals(item.Id)
                                                   join m in db.Users on s.UserId equals m.Id
                                                   select m.UserName).ToList());

            }
            return searchResult;
        }

        public string ExportExcelWorkingReport(WorkingReportModel modelSearch)
        {
            var dataQuery = (from a in db.Plans.AsNoTracking()
                             where a.ProjectId.Equals(modelSearch.ProjectId)
                             join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id into ab
                             from abc in ab.DefaultIfEmpty()
                             join c in db.Stages.AsNoTracking() on a.StageId equals c.Id into ac
                             from acc in ac.DefaultIfEmpty()
                             join d in db.Suppliers.AsNoTracking() on a.SupplierId equals d.Id into ad
                             from adc in ad.DefaultIfEmpty()
                             orderby abc.ContractName
                             where a.IsPlan
                             select new WorkingReportModel()
                             {

                                 Id = a.Id,
                                 ContractName = abc.ContractName,
                                 StageName = acc.Name,
                                 Name = a.Name,
                                 Types = a.Type == 1 ? "Dự án" : a.Type == 2 ? "Bổ sung tính phí" : a.Type == 3 ? "Bổ sung không tính phí" : "",
                                 ContractDueDate = a.ContractDueDate,
                                 ContractStartDate = a.ContractStartDate,
                                 ActualStartDate = a.ActualStartDate,
                                 ActualEndDate = a.ActualEndDate,
                                 PlanStartDate = a.PlanStartDate,
                                 PlanDueDate = a.PlanDueDate,
                                 ContractorName = adc.Name,
                                 Statuss = a.Status == 1 ? "Chưa thực hiện" : a.Status == 2 ? "Đang thực hiện" : a.Status == 3 ? "Đã hoàn thành" : a.Status == 4 ? "Đóng" : "",
                                 DoneRatio = a.DoneRatio,
                             }).AsQueryable();

            if (modelSearch.DateFromActualStartDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanStartDate >= modelSearch.DateFromActualStartDate);

            }

            if (modelSearch.DateToActualStartDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanStartDate <= modelSearch.DateToActualStartDate);
            }


            if (modelSearch.DateFromActualEndDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanDueDate >= modelSearch.DateFromActualEndDate);

            }

            if (modelSearch.DateToActualEndDate.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PlanDueDate <= modelSearch.DateToActualEndDate);
            }
            var listHolidayDataBase = db.Holidays.AsNoTracking().ToList();
            var lisplan = dataQuery.ToList();
            List<string> listUser;
            foreach (var item in lisplan)
            {
                int a = 0;
                if (item.PlanDueDate != null && item.PlanStartDate != null)
                {
                    TimeSpan time = (TimeSpan)(item.PlanDueDate - item.PlanStartDate);
                    int number = time.Days;

                    foreach (var ite in listHolidayDataBase)
                    {
                        if (item.PlanStartDate <= ite.HolidayDate && item.PlanDueDate >= ite.HolidayDate)
                        {
                            a++;
                        }
                    }
                    item.NumberDate = number - a + 1;
                }
                else
                {
                    item.NumberDate = 0;
                }
                listUser = (from s in db.PlanAssignments.AsNoTracking()
                            where s.PlanId.Equals(item.Id)
                            join m in db.Users.AsNoTracking() on s.UserId equals m.Id
                            select m.UserName).ToList();

                item.PlanUser = string.Join(", ", listUser);
            }


            List<WorkingReportModel> listModel = lisplan;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/WorkingReport.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ContractName,
                    a.StageName,
                    a.Name,
                    a.Types,
                    a.ContractDueDate,
                    a.ContractStartDate,
                    a.PlanStartDate,
                    a.PlanDueDate,
                    a.ContractorName,
                    a.Statuss,
                    a.DoneRatio,
                    a.PlanUser,
                    a.NumberDate,
                    a.Comment,
                });

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo công việc" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo công việc" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new NTSLogException(modelSearch, ex);
            }
        }

        public void CreatePlanAdjustment(PlanAdjustmentModel model)
        {
            var body = string.Empty;
            ScheduleProjectSearchModel modelSearch = new ScheduleProjectSearchModel()
            {
                ProjectId = model.ProjectId
            };
            var data = GetListPlanByProjectId(modelSearch);

            body = JsonConvert.SerializeObject(data, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

            var planHistories = db.PlanHistories.AsNoTracking().Where(r => r.ProjectId.Equals(model.ProjectId)).ToList();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    int version = planHistories.Count > 0 ? planHistories.Max(r => r.Version) + 1 : 1;
                    PlanHistory planHistory = new PlanHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = model.ProjectId,
                        Version = version,
                        Status = model.Status,
                        AcceptDate = model.AcceptDate,
                        Content = body,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        Description = model.Description,
                    };

                    db.PlanHistories.Add(planHistory);

                    if (model.ListAttach.Count > 0)
                    {
                        List<PlanHistoryAttach> listFileEntity = new List<PlanHistoryAttach>();
                        foreach (var item in model.ListAttach)
                        {
                            if (item.FilePath != null && item.FilePath != "")
                            {
                                PlanHistoryAttach fileEntity = new PlanHistoryAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PlanHistoryId = planHistory.Id,
                                    Type = item.Type,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now,
                                    CreateBy = model.CreateBy,
                                    UpdateBy = model.CreateBy,
                                    FileName = item.FileName,
                                    Path = item.FilePath,
                                    UpdateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }
                        }
                        db.PlanHistoryAttaches.AddRange(listFileEntity);
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

        public void DeleteMultiPlan(PlanCopyCreateModel model, string userId)
        {
            var listId = model.ListCopy.Select(a => a.Id).ToList();
            var plans = db.Plans.Where(a => listId.Contains(a.Id)).ToList();

            foreach (var item in plans)
            {
                if (item.IsPlan)
                {
                    var indexs = db.Plans.Where(a => a.IsPlan && a.ProjectProductId.Equals(item.ProjectProductId) && a.StageId.Equals(item.StageId)).ToList();
                    var maxIndex = 1;
                    if (indexs.Count > 0)
                    {
                        maxIndex = indexs.Select(i => i.Index).Max();
                    }

                    if (item.Index < maxIndex)
                    {
                        int modelIndex = item.Index;
                        var listOrder = indexs.Where(i => i.Index > modelIndex).ToList();
                        if (listOrder.Count > 0 && listOrder != null)
                        {
                            foreach (var order in listOrder)
                            {
                                order.Index--;
                            }
                        }
                    }
                    // Đã Log time thì không cho phép xóa
                    var workDiary = db.WorkDiaries.Where(i => i.ObjectId.Equals(item.Id)).ToList();
                    if (workDiary.Count > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0088, TextResourceKey.WorkDiary);
                    }
                }
            }

            db.Plans.RemoveRange(plans);
            db.PlanAssignments.RemoveRange(db.PlanAssignments.Where(a => listId.Contains(a.PlanId)));
            db.SaveChanges();

            foreach (var item in plans)
            {
                UpdateStageDate(item.ParentId);
                this.ReCalculateDoneRatio(item.ParentId);
            }
        }

        public List<EmployeePlanModel> GetListEmployee(List<string> listIdUser)
        {
            var result = (from a in db.Employees.AsNoTracking()
                          join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId
                          where /*!listIdUser.Contains(b.Id) &&*/ a.Status == Constants.Employee_Status_Use
                          orderby a.Name
                          select new EmployeePlanModel
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Name = a.Name,
                              UserId = b.Id
                          }).ToList();

            return result;
        }

        public List<EmployeePlanModel> GetListPlanAdjustment(string planId)
        {
            var result = (from a in db.PlanAssignments.AsNoTracking()
                          where a.PlanId.Equals(planId)
                          select new EmployeePlanModel()
                          {
                              Id = a.Id,
                              UserId = a.UserId,
                              IsMain = a.IsMain
                          }).ToList();

            return result;
        }

        public void UpdatePlanAdjustment(PlanAdjustmentCreateModel model)
        {
            var list = db.PlanAssignments.Where(a => a.PlanId.Equals(model.PlanId)).ToList();
            db.PlanAssignments.RemoveRange(list);

            List<PlanAssignment> planAssignments = new List<PlanAssignment>();
            var users = db.Users.AsNoTracking().ToList();

            foreach (var item in model.ListPlanAdjustment)
            {
                planAssignments.Add(new PlanAssignment()
                {
                    Id = Guid.NewGuid().ToString(),
                    PlanId = model.PlanId,
                    UserId = item.UserId,
                    IsMain = item.IsMain,
                    EmployeeId = users.Where(r => r.Id.Equals(item.UserId)).Select(r => r.EmployeeId).FirstOrDefault(),
                });
            }

            db.PlanAssignments.AddRange(planAssignments);
            db.SaveChanges();
        }

        public object GetDataCopy(ScheduleProjectResultModel model)
        {
            List<string> listHeard = (from a in db.Plans.AsNoTracking()
                                      join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                      where a.ProjectId.Equals(model.ProjectId) && !a.IsPlan
                                      group a by new { a.StageId, b.Name } into g
                                      select g.Key.Name).OrderBy(a => a).ToList();

            var dataQuery = (from a in db.ProjectProducts.AsNoTracking()
                             where a.ProjectId.Equals(model.ProjectId)
                             select new PlanCopyModel()
                             {
                                 Id = a.Id,
                                 Name = a.ContractName,
                                 ContractIndex = a.ContractIndex,
                                 ParentId = a.ParentId != null ? a.ParentId : null,
                                 DataType = a.DataType,
                             }).AsQueryable();

            var queryProject = (from a in db.Projects.AsNoTracking()
                                where a.Id.Equals(model.ProjectId)
                                select new PlanCopyModel()
                                {
                                    Id = a.Id,
                                    ParentId = null,
                                    Name = "DA - " + a.Code + " - " + a.Name,
                                }).AsQueryable();

            var listDataQuery = dataQuery.ToList();


            if (listDataQuery.Count() > 0)
            {
                int maxLen = listDataQuery.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                listDataQuery = listDataQuery
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }
            //listDataQuery.AddRange(queryPlan);

            foreach (var item in listDataQuery)
            {
                if (item.DataType == Constants.ProjectProduct_DataType_Practice)
                {
                    item.Name = item.ContractIndex + " - BTH - " + item.Name;
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    item.Name = item.ContractIndex + " - SP - " + item.Name;
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    item.Name = item.ContractIndex + " - M - " + item.Name;
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    item.Name = item.ContractIndex + " - MH - " + item.Name;
                }

                for (int i = 0; i < listHeard.Count; i++)
                {
                    item.ListCheck.Add(new PlanCopyCheckModel()
                    {
                        ProjectId = model.ProjectId,
                        ProjectProductId = item.Id,
                        StageId = model.StageId
                    });
                }
            }

            var listProjectProductParent = listDataQuery.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            List<PlanCopyModel> listResult = new List<PlanCopyModel>();
            List<PlanCopyModel> listChild = new List<PlanCopyModel>();

            foreach (var parent in listProjectProductParent)
            {
                listChild = GetScheduleProjectChild(parent.Id, listDataQuery);

                listResult.Add(parent);

                listResult.AddRange(listChild);
            }

            return new
            {
                ListHeard = listHeard,
                ListData = listResult
            };
        }

        private List<PlanCopyModel> GetScheduleProjectChild(string parentId,
          List<PlanCopyModel> listSchedulePrject)
        {
            List<PlanCopyModel> listResult = new List<PlanCopyModel>();
            var listChild = listSchedulePrject.Where(r => parentId.Equals(r.ParentId)).ToList();

            List<PlanCopyModel> listChildChild = new List<PlanCopyModel>();
            foreach (var child in listChild)
            {
                listChildChild = GetScheduleProjectChild(child.Id, listSchedulePrject);

                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }
        public TotalTypePlan GetNumberWorkInProject(string id)
        {

            var plans = db.Plans.Where(a => a.ProjectId.Equals(id) && a.IsPlan == true).ToList();
            var countWorkEmptyStart = 0;
            var countWorkEmptyEnd = 0;
            var countWorkLate = 0;
            var WorkIncurred = 0;
            foreach (var item in plans)
            {
                if (item.ActualStartDate == null || item.ContractStartDate == null)
                {
                    countWorkEmptyStart++;
                }
                if (item.ActualEndDate == null || item.ContractDueDate == null)
                {
                    countWorkEmptyEnd++;
                }
                if (item.ActualEndDate != null)
                {
                    var dateDeley = ((DateTime)item.ActualEndDate - DateTime.Now).Days;
                    if (dateDeley >= 0 && item.Status != 3)
                    {
                        countWorkLate++;
                    }
                }
                if (item.Type == 1 || item.Type == 2)
                {
                    WorkIncurred++;
                }
            }
            TotalTypePlan totalTypePlan = new TotalTypePlan();
            totalTypePlan.WorkLate = countWorkLate;
            totalTypePlan.WorkEmptyStart = countWorkEmptyStart;
            totalTypePlan.WorkEmptyEnd = countWorkEmptyEnd;
            totalTypePlan.WorkIncurred = WorkIncurred;

            return totalTypePlan;

        }

        public ProjectStatusWork GetWorkOfProject(string id)
        {
            ProjectStatusWork psw = new ProjectStatusWork();
            var data = db.Plans.Where(a => a.IsPlan.Equals(true) && a.ProjectId.Equals(id));
            psw.WorkDone = data.Where(a => a.Status == 3).Count();
            psw.WorkImplement = data.Where(a => a.Status == 2).Count();
            psw.WorkNoImplement = data.Where(a => a.Status == 0).Count();

            return psw;
        }

        public ProjectProblem GetNumberErrorOfProject(string id)
        {
            var data = db.Errors.Where(a => a.ProjectId.Equals(id)).ToList();
            ProjectProblem result = new ProjectProblem();
            result.RiskNoAction = data.Where(a => a.Status == 1 || a.Status == 2).Count();
            result.Done = data.Where(a => a.Status == 7).Count();
            result.Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
            result.NoImplementation = data.Where(a => a.Status == 3).Count();
            return result;
        }

        public List<ScheduleEntity> CreatePlanCopy(PlanCopyCreateModel model, string userId)
        {
            int max = 1;
            var targetStage = db.Plans.AsNoTracking().Where(r => r.Id.Equals(model.ScheduleProject.Id)).FirstOrDefault();
            if (targetStage == null)
            {
                return null;
            }

            var data = db.Plans.AsNoTracking().Where(a => a.IsPlan && a.ProjectProductId.Equals(targetStage.ProjectProductId) && a.StageId.Equals(targetStage.StageId)).ToList();
            if (data.Count > 0)
            {
                max = data.Max(a => a.Index) + 1;
            }

            List<PlanAssignment> planAssignments = new List<PlanAssignment>();
            List<PlanAssignment> planAssignmentCopys = new List<PlanAssignment>();
            Plan plan;
            List<ScheduleEntity> returnValue = new List<ScheduleEntity>();
            ScheduleEntity scheduleEntity;

            var users = db.Users.AsNoTracking().ToList();

            foreach (var item in model.ListCopy)
            {
                plan = new Plan()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.PlanName,
                    Weight = item.Weight,
                    ProjectProductId = targetStage.ProjectProductId,
                    ProjectId = targetStage.ProjectId,
                    StageId = targetStage.StageId,
                    ParentId = targetStage.Id,
                    Type = item.Type,
                    ContractStartDate = item.ContractStartDate,
                    ContractDueDate = item.ContractDueDate,
                    PlanStartDate = item.PlanStartDate,
                    PlanDueDate = item.PlanDueDate,
                    SupplierId = item.SupplierId,
                    EstimateTime = item.EstimateTime.HasValue ? (decimal)item.EstimateTime : 0,
                    Duration = item.Duration,
                    Status = (int)Constants.ScheduleStatus.Open,
                    IsPlan = true,
                    DoneRatio = 0,
                    Description = item.Description,
                    Index = max++,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now
                };

                db.Plans.Add(plan);

                scheduleEntity = new ScheduleEntity()
                {
                    Id = plan.Id,
                    ProjectProductId = plan.ProjectProductId,
                    StageId = plan.StageId,
                    ParentId = plan.ParentId,
                    PlanName = plan.Name.ToUpper(),
                    IsPlan = plan.IsPlan,
                    Status = plan.Status,
                    EstimateTime = plan.EstimateTime,
                    Description = plan.Description,
                    Weight = plan.Weight,
                    DoneRatio = plan.DoneRatio,
                    Type = plan.Type,
                    IndexPlan = plan.Index,
                    SupplierId = plan.SupplierId,
                    ContractStartDate = plan.ContractStartDate,
                    ContractDueDate = plan.ContractDueDate,
                    PlanStartDate = plan.PlanStartDate,
                    PlanDueDate = plan.PlanDueDate,
                };

                DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

                if (!scheduleEntity.PlanStartDate.HasValue || !scheduleEntity.PlanDueDate.HasValue)
                {
                    scheduleEntity.InternalStatus = "THIẾU NGÀY TK";
                }
                else if (today > scheduleEntity.PlanDueDate && scheduleEntity.DoneRatio < 100)
                {
                    scheduleEntity.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                }
                else
                {
                    scheduleEntity.InternalStatus = "OK";
                }

                planAssignments = new List<PlanAssignment>();
                planAssignments = db.PlanAssignments.Where(a => a.PlanId.Equals(item.Id)).ToList();

                planAssignmentCopys = new List<PlanAssignment>();
                foreach (var planAssignment in planAssignments)
                {
                    planAssignmentCopys.Add(new PlanAssignment
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlanId = plan.Id,
                        UserId = planAssignment.UserId,
                        IsMain = planAssignment.IsMain,
                        EmployeeId = planAssignment.EmployeeId
                    });

                    scheduleEntity.ListIdUserId.Add(planAssignment.UserId);

                    if (planAssignment.IsMain)
                    {
                        scheduleEntity.MainUserId = planAssignment.UserId;
                    }
                }

                scheduleEntity.ResponsiblePersionName = string.Join(", ", (from s in planAssignments
                                                                           join m in users on s.UserId equals m.Id
                                                                           orderby s.IsMain descending
                                                                           select m.UserName).ToArray());

                db.PlanAssignments.AddRange(planAssignmentCopys);

                // Trả về các công việc được copy
                returnValue.Add(scheduleEntity);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();

                    this.UpdateStageDate(model.ScheduleProject.Id);
                    this.ReCalculateDoneRatio(model.ScheduleProject.Id);

                    trans.Commit();

                    return returnValue;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public List<ScheduleEntity> CreateStageCopy(PlanCopyCreateModel model, string userId)
        {
            List<Plan> plans = new List<Plan>();
            List<Plan> planCopys = new List<Plan>();
            List<PlanAssignment> planAssignments = new List<PlanAssignment>();
            List<PlanAssignment> planAssignmentCopys = new List<PlanAssignment>();
            Plan plan;
            Plan planChild;
            int max = 0;

            List<ScheduleEntity> returnValue = new List<ScheduleEntity>();
            ScheduleEntity scheduleEntity;
            var stages = db.Stages.AsNoTracking().ToList();
            var ListCopyStage = model.ListCopyStage.Where(a => !string.IsNullOrEmpty(a.StageName)).ToList();

            foreach (var item in ListCopyStage)
            {
                plan = new Plan()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.StageName,
                    Weight = item.Weight,
                    ProjectProductId = model.ScheduleProject.Id,
                    ProjectId = model.ScheduleProject.ProjectId,
                    StageId = item.StageId,
                    Type = item.Type,
                    ContractStartDate = item.ContractStartDate,
                    ContractDueDate = item.ContractDueDate,
                    PlanStartDate = item.PlanStartDate,
                    PlanDueDate = item.PlanDueDate,
                    SupplierId = item.SupplierId,
                    Duration = item.Duration,
                    Status = (int)Constants.ScheduleStatus.Open,
                    DoneRatio = 0,
                    IsPlan = false,
                    Description = item.Description,
                    Index = item.Index,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now
                };

                db.Plans.Add(plan);

                scheduleEntity = new ScheduleEntity()
                {
                    Id = plan.Id,
                    ProjectProductId = plan.ProjectProductId,
                    StageId = plan.StageId,
                    ParentId = plan.ParentId != null ? plan.ParentId : plan.ProjectProductId,
                    IsPlan = plan.IsPlan,
                    Status = plan.Status,
                    EstimateTime = plan.EstimateTime,
                    Description = plan.Description,
                    Weight = plan.Weight,
                    DoneRatio = plan.DoneRatio,
                    Type = plan.Type,
                    IndexPlan = plan.Index,
                    Index = plan.Index,
                    SupplierId = plan.SupplierId,
                    ProjectId = plan.ProjectId,
                    StageName = plan.Name.ToUpper(),
                    BackgroundColor = stages.FirstOrDefault(r => r.Id.Equals(item.StageId)).Color,
                    ContractStartDate = plan.ContractStartDate,
                    ContractDueDate = plan.ContractDueDate,
                    PlanStartDate = plan.PlanStartDate,
                    PlanDueDate = plan.PlanDueDate,
                    Color = stages.FirstOrDefault(r => r.Id.Equals(item.StageId)).Color,
                    CreateDate = plan.CreateDate
                };

                // Trả về các công đoạn được copy
                returnValue.Add(scheduleEntity);

                plans = new List<Plan>();
                //plans = db.Plans.AsNoTracking().Where(a => a.ParentId.Equals(item.Id)).ToList();
                plans = model.ListCopyStage.Where(a => item.Id.Equals(a.ParentId)).Select(b => new Plan
                {
                    Id = b.Id,
                    Name = b.PlanName,
                    Weight = b.Weight,
                    ProjectProductId = model.ScheduleProject.Id,
                    ProjectId = model.ScheduleProject.ProjectId,
                    StageId = b.StageId,
                    ParentId = b.ParentId,
                    Type = b.Type,
                    ContractStartDate = b.ContractStartDate,
                    ContractDueDate = b.ContractDueDate,
                    PlanStartDate = b.PlanStartDate,
                    PlanDueDate = b.PlanDueDate,
                    SupplierId = b.SupplierId,
                    EstimateTime = b.EstimateTime.HasValue ? (decimal)b.EstimateTime : 0,
                    Duration = b.Duration,
                    Status = (int)Constants.ScheduleStatus.Open,
                    IsPlan = true,
                    Description = b.Description,
                    Index = max++,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now
                }).ToList();
                planCopys = new List<Plan>();
                foreach (var planData in plans)
                {
                    planChild = new Plan()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = planData.Name,
                        Weight = planData.Weight,
                        ProjectProductId = planData.ProjectProductId,
                        ProjectId = planData.ProjectId,
                        StageId = planData.StageId,
                        ParentId = plan.Id,
                        Type = planData.Type,
                        ContractStartDate = planData.ContractStartDate,
                        ContractDueDate = planData.ContractDueDate,
                        PlanStartDate = planData.PlanStartDate,
                        PlanDueDate = planData.PlanDueDate,
                        SupplierId = planData.SupplierId,
                        Duration = planData.Duration,
                        IsPlan = true,
                        Status = (int)Constants.ScheduleStatus.Open,
                        DoneRatio = 0,
                        Description = planData.Description,
                        Index = planData.Index,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };
                    planCopys.Add(planChild);

                    scheduleEntity = new ScheduleEntity()
                    {
                        Id = planChild.Id,
                        ProjectProductId = planChild.ProjectProductId,
                        StageId = planChild.StageId,
                        ParentId = planChild.ParentId,
                        PlanName = planChild.Name.ToUpper(),
                        IsPlan = planChild.IsPlan,
                        Status = planChild.Status,
                        EstimateTime = planChild.EstimateTime,
                        Description = planChild.Description,
                        Weight = planChild.Weight,
                        DoneRatio = planChild.DoneRatio,
                        Type = planChild.Type,
                        IndexPlan = planChild.Index,
                        SupplierId = planChild.SupplierId,
                        ContractStartDate = planChild.ContractStartDate,
                        ContractDueDate = planChild.ContractDueDate,
                        PlanDueDate = planChild.PlanDueDate,
                        PlanStartDate = planChild.PlanStartDate,
                        Duration = planChild.Duration,

                    };

                    DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

                    if (!scheduleEntity.PlanStartDate.HasValue || !scheduleEntity.PlanDueDate.HasValue)
                    {
                        scheduleEntity.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                    }
                    else if (today > scheduleEntity.PlanDueDate && scheduleEntity.DoneRatio < 100)
                    {
                        scheduleEntity.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                    }
                    else
                    {
                        scheduleEntity.InternalStatus = "OK";
                    }


                    planAssignments = new List<PlanAssignment>();
                    planAssignments = db.PlanAssignments.Where(a => a.PlanId.Equals(planData.Id)).ToList();

                    planAssignmentCopys = new List<PlanAssignment>();
                    foreach (var planAssignment in planAssignments)
                    {
                        planAssignmentCopys.Add(new PlanAssignment
                        {
                            Id = Guid.NewGuid().ToString(),
                            PlanId = planChild.Id,
                            UserId = planAssignment.UserId,
                            IsMain = planAssignment.IsMain,
                            EmployeeId = planAssignment.EmployeeId
                        });
                    }

                    db.PlanAssignments.AddRange(planAssignmentCopys);
                    scheduleEntity.MainUserId = planAssignmentCopys.Where(r => r.IsMain).Select(r => r.UserId).FirstOrDefault();

                    // Trả về các công việc được copy
                    returnValue.Add(scheduleEntity);

                }
                db.Plans.AddRange(planCopys);
                db.SaveChanges();
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    this.UpdateProjectProductDate(model.ScheduleProject.Id);
                    this.ReCalculateDoneRatioProjectProduct(model.ScheduleProject.Id);
                    trans.Commit();
                    return returnValue = SortPlanReturn(returnValue, model.ScheduleProject.Id);
                    ;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public List<HolidayInfoModel> GetListHoliday()
        {
            var result = (from a in db.Holidays.AsNoTracking()
                          orderby a.HolidayDate
                          select new HolidayInfoModel
                          {
                              Id = a.Id,
                              Year = a.Year,
                              HolidayDate = a.HolidayDate
                          }).ToList();

            return result;
        }

        public object GetPlanAssignments(string planId)
        {
            var assignment = db.PlanAssignments.AsNoTracking().Where(a => a.PlanId.Equals(planId)).OrderBy(a => a.IsMain);

            var ListIdUserId = assignment.Select(a => a.UserId).ToList();

            var ResponsiblePersionName = string.Join(", ", (from s in assignment
                                                            join m in db.Users.AsNoTracking() on s.UserId equals m.Id
                                                            select m.UserName).ToArray());

            var MainUserId = assignment.Where(r => r.PlanId.Equals(planId) && r.IsMain).FirstOrDefault() != null ? assignment.Where(r => r.PlanId.Equals(planId) && r.IsMain).FirstOrDefault().UserId : string.Empty;

            return new
            {
                ListIdUserId,
                ResponsiblePersionName,
                MainUserId,
            };
        }

        /// <summary>
        /// Lấy list order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetListOrder(string projectProductId, string stageId, int type)
        {
            List<int> result = new List<int>();
            int max = 1;
            int numOfPlan = db.Plans.AsNoTracking().Where(a => a.IsPlan && a.ProjectProductId.Equals(projectProductId) && a.StageId.Equals(stageId)).Count();

            if (numOfPlan > 0)
            {
                for (int i = 0; i < numOfPlan; i++)
                {
                    result.Add(i + 1);
                }

                if (type == 1)
                {
                    result.Add(numOfPlan + 1);
                }
            }
            else
            {
                result.Add(max);
            }
            return result;
        }

        public int GetStatusOfPlan(List<ScheduleProjectResultModel> listPlan)
        {
            var countOpenPlan = 0;
            var countOnGoingPlan = 0;
            var countClosePlan = 0;
            var countStopPlan = 0;
            foreach (var plan in listPlan)
            {
                if (plan.Status == 1)
                {
                    countOpenPlan++;
                }
                else if (plan.Status == 2)
                {
                    countOnGoingPlan++;
                }
                else if (plan.Status == 3)
                {
                    countClosePlan++;
                }
                else if (plan.Status == 4)
                {
                    countStopPlan++;
                }
            }
            if (countOnGoingPlan > 0)
            {
                return 2;
            }
            else if (countOpenPlan > 0 && countClosePlan == 0)
            {
                return 1;
            }
            else if (countOpenPlan > 0 && countClosePlan > 0)
            {
                return 1;
            }
            else if (countClosePlan > 0 && countStopPlan == 0)
            {
                return 3;
            }
            else if (countClosePlan == 0 && countStopPlan > 0)
            {
                return 4;
            }
            return 1;
        }

        public PlanHistoryViewModel GetPlanHistoryInfo(string id)
        {
            var planHistory = db.PlanHistories.AsNoTracking().FirstOrDefault(a => a.Id.Equals(id));
            PlanHistoryViewModel result = new PlanHistoryViewModel()
            {
                ProjectName = db.Projects.AsNoTracking().FirstOrDefault(a => a.Id.Equals(planHistory.ProjectId))?.Name,
                Version = planHistory.Version,
                CreateDate = planHistory.CreateDate
            };

            if (planHistory != null)
            {
                result.Result = JsonConvert.DeserializeObject<ResultModel>(planHistory.Content);
            }

            return result;
        }

        public List<ComboboxSupplierModel> GetListSuppliers()
        {
            var listSupplier = (from a in db.Suppliers.AsNoTracking()
                                where !string.IsNullOrEmpty(a.Alias)
                                orderby a.Code
                                select new ComboboxSupplierModel
                                {
                                    Id = a.Id,
                                    Name = a.Alias + " - " + a.Name,
                                    Alias = a.Alias.ToUpper()
                                }).ToList();

            return listSupplier;
        }

        public List<ScheduleProjectResultModel> SearchPlan(List<ScheduleProjectResultModel> results)
        {
            List<ScheduleProjectResultModel> data = new List<ScheduleProjectResultModel>();
            var listProductParent = results.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            foreach (var parentProduct in listProductParent)
            {
                var productChilds = results.Where(a => parentProduct.Id.Equals(a.ParentId)).ToList();
                foreach (var productChild in productChilds)
                {
                    var stages = results.Where(a => productChild.Id.Equals(a.ParentId)).ToList();
                    if (stages.Count == 0)
                    {
                        results.Remove(productChild);
                    }
                }
                var productChildAfterDelete = results.Where(a => parentProduct.Id.Equals(a.ParentId)).ToList();
                if (productChildAfterDelete.Count == 0)
                {
                    results.Remove(parentProduct);
                }
            }
            data.AddRange(results);
            return data;
        }

        public DoneRatioProject GetRatioDoneOfProject(ScheduleProjectSearchModel modelSearch)
        {
            DoneRatioProject drp = new DoneRatioProject();
            var listProjectProduct = GetListPlanByProjectId(modelSearch).listResult;
            var listProjectProductParent = listProjectProduct.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            var totalOpenPlan = 0;
            var totalOnGoingPlan = 0;
            var totalClosePlan = 0;
            foreach (var ParentProjectProduct in listProjectProductParent)
            {
                if (ParentProjectProduct.Status == 1)
                {
                    totalOpenPlan = totalOpenPlan + ParentProjectProduct.Weight;
                }
                else if (ParentProjectProduct.Status == 2)
                {
                    totalOnGoingPlan = totalOnGoingPlan + ParentProjectProduct.Weight;
                }
                else if (ParentProjectProduct.Status == 3)
                {
                    totalClosePlan = totalClosePlan + ParentProjectProduct.Weight;
                }

            }
            drp.Done = totalClosePlan;
            drp.NoImplementation = totalOpenPlan;
            drp.Implementation = totalOnGoingPlan;
            return drp;
        }

        public ProjectProblem GetNumberErrorOfProjectPlan(string id)
        {
            DateTime dateNow = DateTime.Now;

            var data = db.Errors.Where(a => a.ProjectId.Equals(id)).ToList();
            ProjectProblem result = new ProjectProblem();
            //result.RiskNoAction = data.Where(a => a.Status == 1 || a.Status == 2).Count();
            //result.Done = data.Where(a => a.Status == 7|| a.Status ==9 || a.Status ==10).Count();
            result.Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
            result.NoImplementation = data.Where(a => a.Status == 3).Count();
            var errorDelays = (from a in db.Errors.AsNoTracking()
                               join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                               where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && c.Status != Constants.ErrorFix_Status_Finish && a.ProjectId.Equals(id)
                               select new
                               {
                                   Id = a.Id,
                                   Status = a.Status,
                                   FixStatus = c.Status,
                                   Deadline = (c.DateTo.HasValue && (dateNow > c.DateTo)) && (c.Status != Constants.ErrorFix_Status_Finish) ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                               }).AsQueryable();
            result.ErrorDelay = errorDelays.Where(r => r.Status >= Constants.Problem_Status_Processed && r.Deadline > 0 && r.FixStatus != Constants.ErrorFix_Status_Finish).GroupBy(g => g.Id).Count();

            return result;
        }

        public ProjectStatusWork GetWorkOfProjectPlan(string id)
        {
            ProjectStatusWork psw = new ProjectStatusWork();
            var data = db.Plans.Where(a => a.IsPlan.Equals(true) && a.ProjectId.Equals(id));
            psw.WorkDone = data.Where(a => a.Status == 3).Count();
            psw.WorkImplement = data.Where(a => a.Status == 2).Count();
            psw.WorkNoImplement = data.Where(a => a.Status == 1).Count();

            return psw;
        }

        public void GetError(string id, IWorksheet sheet)
        {
            List<ErrorResultModel> listModel = this.MakeWhereCondition(id).Where(r => r.Status == Constants.Problem_Status_NoPlan || r.Status == Constants.Problem_Status_Processed).ToList();


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
                    var errorFix = errorFixs.Where(r => r.ErrorId.Equals(error.Id)).OrderBy(r => r.DateFrom).ToList();
                    var product = products.Where(r => r.Id.Equals(error.ObjectId)).FirstOrDefault();
                    var module = db.Modules.FirstOrDefault(a => a.Id.Equals(error.ObjectId));
                    errorExportModel = new ErrorFixExportModel();
                    errorExportModel.MaVanDe = error.Code;
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

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport1 = exportData.Select((a, i) => new
                {
                    a.MaVanDe,
                    a.MaHangMuc,
                    a.TenHangMuc,
                    a.PhanLoai,
                    a.TenVanDe,
                    a.MoTa,
                    a.NguyenNhan,
                    a.GiaiPhap,
                    a.NguoiThucHien,
                    a.BoPhanThucHien,
                    a.NgayBatDau,
                    a.NgayKetThuc,
                    a.DanhGia,
                    a.TinhTrang,
                    a.Done

                });
                //listExport = listExport.OrderByDescending(a => a.type).ToList();
                if (listExport1.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport1.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport1, iRangeData.Row, iRangeData.Column, false);

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

        public void ReCorrectData()
        {
            var stages = db.Plans.AsNoTracking().Where(r => r.IsPlan == false).ToList();
            int index = 0;

            foreach (var item in stages)
            {
                index++;
                this.ReCalculateDoneRatio(item.Id);

                Debug.WriteLine($"Xử lý đến phần tử thứ {index} số lượng còn lại: {stages.Count - index}");
            }

        }

        public object GanttChart(ScheduleProjectSearchModel modelSearch)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Lấy dữ liệu kế hoạch trong danh mục sản phẩm của dự án
            var projectProducts = (from a in db.ProjectProducts.AsNoTracking().Where(r => r.ProjectId.Equals(modelSearch.ProjectId))
                                   join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into ab
                                   from ba in ab.DefaultIfEmpty()
                                   join c in db.Products.AsNoTracking() on a.ProductId equals c.Id into ac
                                   from ca in ac.DefaultIfEmpty()
                                   select new GanttChartModel()
                                   {
                                       Id = a.Id,
                                       ParentId = a.ParentId,
                                       PlanStartDate = a.PlanStartDate,
                                       PlanDueDate = a.PlanDueDate,
                                       DoneRatio = a.DoneRatio,
                                       ContractIndex = a.ContractIndex,
                                       DataType = a.DataType,
                                       ContractCode = string.IsNullOrEmpty(ba.Code) ? a.ContractCode : ba.Code,
                                       ContractName = a.ContractName,
                                   }
                           ).ToList();

            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 1: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            // Lấy danh sách công việc chi tiết
            var plans = (from a in db.Plans.AsNoTracking().Where(r => r.ProjectId.Equals(modelSearch.ProjectId))
                         join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                         select new GanttChartModel()
                         {
                             Id = a.Id,
                             ProjectProductId = a.ProjectProductId,
                             ParentId = a.ParentId != null ? a.ParentId : a.ProjectProductId,
                             Name = a.Name,
                             PlanStartDate = a.PlanStartDate,
                             PlanDueDate = a.PlanDueDate,
                             DoneRatio = a.DoneRatio,
                             Color = b.Color,
                             StageId = a.StageId,
                             Index = b.index,
                             IndexPlan = a.Index,
                             IsPlan = a.IsPlan,
                             SupplierId = a.SupplierId,
                             Status = a.Status,
                             ContractStartDate = a.ContractStartDate,
                             ContractDueDate = a.ContractDueDate,
                         }).ToList();


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 2: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();


            // Thực hiện sắp xếp thứ tự sản phẩm, module
            if (projectProducts.Count() > 0)
            {
                int maxLen = projectProducts.Where(r => !string.IsNullOrEmpty(r.ContractIndex)).Select(s => s.ContractIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                projectProducts = projectProducts
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.ContractIndex) ? s.ContractIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 3: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();


            // Update lại tên của Sản phẩm/ Module trên Plan
            foreach (var item in projectProducts)
            {
                string nameView = string.Empty;
                if (item.DataType == Constants.ProjectProduct_DataType_Practice)
                {
                    nameView = item.ContractIndex + " - BTH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.Name = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    nameView = item.ContractIndex + " - SP - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.Name = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module)
                {
                    nameView = item.ContractIndex + " - M - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.Name = nameView.NTSTrim();
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    nameView = item.ContractIndex + " - MH - " + (!string.IsNullOrEmpty(item.ContractCode) ? item.ContractCode.ToUpper() + " - " + item.ContractName.ToUpper() : item.ContractName.ToUpper());
                    item.Name = nameView.NTSTrim();
                }

                if (string.IsNullOrEmpty(item.ParentId))
                {
                    item.Color = "#A5CDFF";
                }
                else
                {
                    item.Color = "#FEFFDC";
                }

                item.PlanStartDate = DateTimeUtils.ConvertDateFrom(item.PlanStartDate);
                item.PlanDueDate = DateTimeUtils.ConvertDateTo(item.PlanDueDate);
            }


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 4: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();


            if (modelSearch.DateFrom.HasValue)
            {
                modelSearch.DateStart = modelSearch.DateFrom.Value;
            }
            else
            {
                if (projectProducts.FirstOrDefault(a => a.PlanStartDate.HasValue) != null)
                {
                    modelSearch.DateStart = projectProducts.Where(a => a.PlanStartDate.HasValue).Min(a => a.PlanStartDate.Value);
                    if (modelSearch.DateStart < DateTime.Now.AddYears(-1))
                    {
                        modelSearch.DateStart = DateTime.Now.AddYears(-1);
                    }
                }
                else
                {
                    modelSearch.DateStart = DateTime.Now.AddMonths(-1);
                }
            }

            if (modelSearch.DateTo.HasValue)
            {
                modelSearch.DateEnd = modelSearch.DateTo.Value;
            }
            else
            {
                if (projectProducts.FirstOrDefault(a => a.PlanDueDate.HasValue) != null)
                {
                    modelSearch.DateEnd = projectProducts.Where(a => a.PlanDueDate.HasValue).Max(a => a.PlanDueDate.Value);
                    if (modelSearch.DateEnd > DateTime.Now.AddYears(1))
                    {
                        modelSearch.DateEnd = DateTime.Now.AddYears(1);
                    }
                }
                else
                {
                    modelSearch.DateEnd = DateTime.Now.AddMonths(1);
                }
            }
            bool search = false;

            //seach gantt
            if (!string.IsNullOrEmpty(modelSearch.NameProduct))
            {
                search = true;
                var listProjectProduct = projectProducts.Where(a => a.Name.ToUpper().Contains(modelSearch.NameProduct.ToUpper())).ToList();
                if (projectProducts.Count != 0)
                {
                    List<GanttChartModel> listProduct = new List<GanttChartModel>();
                    foreach (var p in listProjectProduct)
                    {
                        GetListProduct(listProduct, p, projectProducts);
                    }
                    projectProducts = listProjectProduct;
                    projectProducts.AddRange(listProduct);
                }
                else
                {
                    plans = new List<GanttChartModel>();
                    projectProducts = new List<GanttChartModel>();
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.SupplierId))
            {
                search = true;
                var listPlan = plans.Where(a => modelSearch.SupplierId.Equals(a.SupplierId) && a.IsPlan ==true).ToList();
                if(listPlan.Count !=0)
                {
                    List<GanttChartModel> listStage = new List<GanttChartModel>();
                    foreach (var  p in listPlan)
                    {
                        var s = plans.FirstOrDefault(a => p.ParentId.Equals(a.Id) && a.IsPlan == false);
                        if(listStage.FirstOrDefault( a => a.Id.Equals(s.Id)) == null)
                        {
                            listStage.Add(s);
                        }
                    }
                    plans = listPlan;
                    plans.AddRange(listStage);
                }
                else
                {
                    plans = new List<GanttChartModel>();
                    projectProducts = new List<GanttChartModel>();
                }
            }
            if (modelSearch.WorkStatus != 0)
            {
                search = true;
                var listPlan = plans.Where(a => modelSearch.WorkStatus.Equals(a.Status) && a.IsPlan == true).ToList();
                if (listPlan.Count != 0)
                {
                    List<GanttChartModel> listStage = new List<GanttChartModel>();
                    foreach (var p in listPlan)
                    {
                        var s = plans.FirstOrDefault(a => p.ParentId.Equals(a.Id) && a.IsPlan == false);
                        if (listStage.FirstOrDefault(a => a.Id.Equals(s.Id)) == null)
                        {
                            listStage.Add(s);
                        }
                    }
                    plans = listPlan;
                    plans.AddRange(listStage);
                }
                else
                {
                    plans = new List<GanttChartModel>();
                    projectProducts = new List<GanttChartModel>();
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.StageId))
            {
                search = true;
                var listPlan = plans.Where(a => modelSearch.StageId.Equals(a.StageId) && a.IsPlan == true).ToList();
                if (listPlan.Count != 0)
                {
                    List<GanttChartModel> listStage = new List<GanttChartModel>();
                    foreach (var p in listPlan)
                    {
                        var s = plans.FirstOrDefault(a => p.ParentId.Equals(a.Id) && a.IsPlan == false);
                        if (listStage.FirstOrDefault(a => a.Id.Equals(s.Id)) == null)
                        {
                            listStage.Add(s);
                        }
                    }
                    plans = listPlan;
                    plans.AddRange(listStage);
                }
                else
                {
                    plans = new List<GanttChartModel>();
                    projectProducts = new List<GanttChartModel>();
                }
            }
            if (modelSearch.Operator != 0)
            {
                search = true;
                List < GanttChartModel > listPlan = new  List<GanttChartModel>();
                if (modelSearch.Operator ==1)
                {
                    listPlan = plans.Where(a => (a.DoneRatio == modelSearch.Percen) && a.IsPlan == true).ToList();
                }
                if (modelSearch.Operator == 2)
                {
                    listPlan = plans.Where(a => (a.DoneRatio > modelSearch.Percen) && a.IsPlan == true).ToList();
                }
                if (modelSearch.Operator == 3)
                {
                    listPlan = plans.Where(a => (a.DoneRatio >= modelSearch.Percen) && a.IsPlan == true).ToList();
                }
                if (modelSearch.Operator == 4)
                {
                    listPlan = plans.Where(a => (a.DoneRatio < modelSearch.Percen) && a.IsPlan == true).ToList();
                }
                if (modelSearch.Operator == 5)
                {
                    listPlan = plans.Where(a => (a.DoneRatio <= modelSearch.Percen) && a.IsPlan == true).ToList();
                }
                if (listPlan.Count != 0)
                {
                    List<GanttChartModel> listStage = new List<GanttChartModel>();
                    foreach (var p in listPlan)
                    {
                        var s = plans.FirstOrDefault(a => p.ParentId.Equals(a.Id) && a.IsPlan == false);
                        if (listStage.FirstOrDefault(a => a.Id.Equals(s.Id)) == null)
                        {
                            listStage.Add(s);
                        }
                    }
                    plans = listPlan;
                    plans.AddRange(listStage);
                }
                else
                {
                    plans = new List<GanttChartModel>();
                    projectProducts = new List<GanttChartModel>();
                }
            }
            //


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 5: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            List<GanttChartModel> listData = projectProducts.Union(plans).ToList();
            List<GanttChartModel> listParent = listData.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            List<GanttChartModel> schedules = new List<GanttChartModel>();

            DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

            foreach (var parent in listParent)
            {
                var childs = GetGanttInternalStatus(parent, listData);

                // Trường hợp là sản phẩm, module
                if (!parent.IsPlan)
                {
                    if (parent.Status == (int)Constants.ScheduleStatus.Closed)
                    {
                        parent.InternalStatus = 1;
                    }
                    else if (childs.Where(r => !r.IsPlan && !r.ContractStartDate.HasValue).Any() || childs.Where(r => r.InternalStatus==4).Any())
                    {
                        parent.InternalStatus = 4;
                    }
                    else if (childs.Where(r => !r.IsPlan && !r.ContractDueDate.HasValue).Any() || childs.Where(r => r.InternalStatus==5).Any())
                    {
                        parent.InternalStatus = 5;
                    }
                    else if (childs.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || childs.Where(r => r.InternalStatus==6).Any())
                    {
                        parent.InternalStatus = 6;
                    }
                    else if (parent.ContractDueDate.HasValue && (parent.PlanDueDate > parent.ContractDueDate) || childs.Where(r => r.InternalStatus ==2).Any())
                    {
                        parent.InternalStatus = 2;
                    }
                    else if (parent.PlanDueDate.HasValue && today > parent.PlanDueDate.Value && (parent.DoneRatio < 100) || childs.Where(r => r.InternalStatus== 3).Any())
                    {
                        parent.InternalStatus = 3;
                    }
                    else
                    {
                        parent.InternalStatus = 1;
                    }
                }
                schedules.Add(parent);
                schedules.AddRange(childs);
            }


            //lọc tình trạng công việc nội bộ 
            if (modelSearch.PlanStatus != 0)
            {
                search = true;
                var listPlan = schedules.Where(a => modelSearch.PlanStatus == a.InternalStatus &&( a.IsPlan == true || !string.IsNullOrEmpty(a.StageId))).ToList();
                if (listPlan.Count != 0)
                {
                    List<GanttChartModel> listStage = new List<GanttChartModel>();
                    foreach (var p in listPlan)
                    {
                        if(p.IsPlan == true)
                        {
                            var s = plans.FirstOrDefault(a => p.ParentId.Equals(a.Id) && a.IsPlan == false);
                            if (s != null && listStage.FirstOrDefault(a => a.Id.Equals(s.Id)) == null)
                            {
                                listStage.Add(s);
                            }
                        }
                        else
                        {
                            var s = plans.Where(a => p.Id.Equals(a.ParentId) && a.IsPlan == true);
                            listStage.AddRange(s);
                        }

                    }
                    var datas = listPlan;
                    datas.AddRange(listStage);
                    schedules = schedules.Where(a =>(a.IsPlan != true && string.IsNullOrEmpty(a.StageId))).ToList();
                    schedules.AddRange(datas);
                }
                else
                {
                    schedules = new List<GanttChartModel>();
                }
            }
            //
            List<GanttChartModel> listRoot = schedules.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            List<GanttChartModel> listChilds = schedules.Where(a => !string.IsNullOrEmpty(a.ParentId)).ToList();


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 7: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();


            bool isSearch = false;
            List<GanttChartModel> listChild = new List<GanttChartModel>();
            foreach (var item in listRoot)
            {
                isSearch = true;

                if (modelSearch.DateFrom.HasValue && (!item.PlanStartDate.HasValue || item.PlanStartDate.Value < modelSearch.DateFrom))
                {
                    isSearch = false;
                }

                if (modelSearch.DateTo.HasValue && (!item.PlanDueDate.HasValue || item.PlanDueDate.Value > modelSearch.DateTo))
                {
                    isSearch = false;
                }

                listChild = new List<GanttChartModel>();
                listChild = GetScheduleProjectChildGanttIndex(item, listChilds, modelSearch);
                if (isSearch || listChild.Count > 0)
                {
                    item.ListChild = listChild;
                }
            }


            stopwatch.Stop();
            Debug.WriteLine($"Thời gian xử lý 8: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();
            //clear data empty when search
            if (search == true)
            {
                foreach(var item in listRoot.ToList())
                {
                    if(item.ListChild.Count == 0)
                    {
                        listRoot.Remove(item);
                    }
                    else
                    {
                        var check= checkData(item.ListChild);
                        if(check == true)
                        {
                            listRoot.Remove(item);
                        }
                    }
                }
            }

            return new
            {
                Schedules = listRoot,
                DateFrom = modelSearch.DateStart,
                DateTo = modelSearch.DateEnd
            };
        }

        private List<GanttChartModel> GetScheduleProjectChildGanttIndex(GanttChartModel parent,
          List<GanttChartModel> listSchedulePrject, ScheduleProjectSearchModel modelSearch)
        {
            bool isSearch = false;
            List<GanttChartModel> listChild = new List<GanttChartModel>();

            var listData = listSchedulePrject.Where(r => parent.Id.Equals(r.ParentId)).ToList();

            listChild = listData.Where(a => !string.IsNullOrEmpty(a.StageId)).OrderBy(r => r.Index).ThenBy(r => r.IndexPlan).ToList();
            listChild.AddRange(listData.Where(a => string.IsNullOrEmpty(a.StageId)).ToList());

            List<GanttChartModel> listChildChild = new List<GanttChartModel>();
            DateTime dateTime = DateTime.Today;
            foreach (GanttChartModel child in listChild)
            {
                isSearch = true;

                if (modelSearch.DateFrom.HasValue && (!child.PlanStartDate.HasValue || child.PlanStartDate.Value < modelSearch.DateFrom))
                {
                    isSearch = false;
                }

                if (modelSearch.DateTo.HasValue && (!child.PlanDueDate.HasValue || child.PlanDueDate.Value > modelSearch.DateTo))
                {
                    isSearch = false;
                }

                listChildChild = GetScheduleProjectChildGanttIndex(child, listSchedulePrject, modelSearch);

                if (isSearch || listChildChild.Count > 0)
                {
                    child.ListChild = listChildChild;
                }
            }
            return listChild;
        }

        public List<HolidayGanttModel> GetListHolidayGanttChart()
        {
            var result = (from a in db.Holidays.AsNoTracking()
                          orderby a.HolidayDate
                          select new HolidayGanttModel
                          {
                              @from = a.HolidayDate,
                              to = a.HolidayDate,
                          }).ToList();

            return result;
        }

        public bool checkData(List<GanttChartModel> datas)
        {
            var count = 0;
            foreach (var item in datas.ToList())
            {
                if (item.ListChild.Count == 0 && item.IsPlan != true)
                {
                    datas.Remove(item);
                }
                else if(item.ListChild.Count != 0 && item.IsPlan != true)
                {
                    var check =checkData(item.ListChild);
                    if(check == true)
                    {
                        datas.Remove(item);
                    }
                }
            }
            if(datas.Count ==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetListProduct(List<GanttChartModel> results, GanttChartModel data, List<GanttChartModel> datas)
        {
            if (string.IsNullOrEmpty(data.ParentId))
            {
                if(results.FirstOrDefault(a => a.Id.Equals(data.Id)) == null)
                {
                    results.Add(data);
                    GetChildProduct(data.Id, datas, results);
                }
            }
            else
            {
                if (results.FirstOrDefault(a => a.Id.Equals(data.Id)) == null)
                {
                    results.Add(data);
                    GetChildProduct(data.Id, datas, results);
                    GetParentProduct(data.ParentId, datas, results);
                }
            }

        }
        public void GetChildProduct(string Id, List<GanttChartModel> datas, List<GanttChartModel> results)
        {
            foreach(var item in datas)
            {
                if(!string.IsNullOrEmpty(item.ParentId) && item.ParentId.Equals(Id))
                {
                    if (results.FirstOrDefault(a => a.Id.Equals(item.Id)) == null)
                    {
                        results.Add(item);
                        GetChildProduct(item.Id, datas, results);
                    }
                }
            }
        }
        public void GetParentProduct(string ParentId, List<GanttChartModel> datas, List<GanttChartModel> results)
        {
            foreach (var item in datas)
            {
                if (ParentId.Equals(item.Id))
                {
                    if (results.FirstOrDefault(a => a.Id.Equals(item.Id)) == null)
                    {
                        results.Add(item);
                        if (!string.IsNullOrEmpty(item.ParentId))
                        {
                            GetParentProduct(item.ParentId, datas, results);
                        }
                    }
                }
            }
        }

        private List<GanttChartModel> GetGanttInternalStatus(GanttChartModel parent,
          List<GanttChartModel> listSchedulePrject)
        {
            List<GanttChartModel> listResult = new List<GanttChartModel>();
            List<GanttChartModel> listChild = new List<GanttChartModel>();

            var listData = listSchedulePrject.Where(r => parent.Id.Equals(r.ParentId)).ToList();

            listChild = listData.Where(a => !string.IsNullOrEmpty(a.StageId)).OrderBy(r => r.Index).ThenBy(r => r.IndexPlan).ToList();
            listChild.AddRange(listData.Where(a => string.IsNullOrEmpty(a.StageId)).ToList());

            DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

            List<GanttChartModel> listChildChild = new List<GanttChartModel>();


            foreach (GanttChartModel child in listChild)
            {
                listChildChild = GetGanttInternalStatus(child, listSchedulePrject);

                // Trường hợp là sản phẩm, module
                if (!child.IsPlan)
                {
                    // Trường hợp là sản phẩm
                    if (string.IsNullOrEmpty(child.StageId))
                    {
                        if (child.Status == (int)Constants.ScheduleStatus.Closed)
                        {
                            child.InternalStatus = 1;
                        }
                        else if (listChildChild.Where(r => !r.IsPlan && !r.ContractStartDate.HasValue).Any())
                        {
                            // trống bắt đầu hợp đồng
                            child.InternalStatus = 4;
                        }
                        else if (listChildChild.Where(r => !r.IsPlan && !r.ContractDueDate.HasValue).Any())
                        {
                            //trống kết thúc hợp đồng
                            child.InternalStatus = 5;
                        }
                        else if (listChildChild.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || !child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                        {
                            // thiếu ngày triển khai
                            child.InternalStatus = 6;
                        }
                        else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                        {
                            child.InternalStatus = 2;
                        }
                        else if (child.PlanDueDate.HasValue && today > child.PlanDueDate.Value && (child.DoneRatio < 100))
                        {
                            child.InternalStatus = 3;
                        }
                        else
                        {
                            child.InternalStatus = 1;
                        }
                    }
                    else
                    {
                        if (child.Status == (int)Constants.ScheduleStatus.Closed)
                        {
                            child.InternalStatus = 1;
                        }
                        else if (!child.ContractStartDate.HasValue)
                        {//trống bắt đầu hợp đồng
                            child.InternalStatus = 4;
                        }
                        else if (!child.ContractDueDate.HasValue)
                        {//trống kết thúc hợp đồng
                            child.InternalStatus = 5;
                        }
                        else if (listChildChild.Where(r => !r.PlanStartDate.HasValue || !r.PlanDueDate.HasValue).Any() || !child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                        {//thiếu ngày triển khai
                            child.InternalStatus = 6;
                        }
                        else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                        {
                            child.InternalStatus = 2;
                        }
                        else if (child.PlanDueDate.HasValue && today > child.PlanDueDate.Value && (child.DoneRatio < 100))
                        {
                            child.InternalStatus = 3;
                        }
                        else
                        {
                            child.InternalStatus = 1;
                        }
                    }
                }
                else
                {
                    if (child.Status == (int)Constants.ScheduleStatus.Closed)
                    {
                        child.InternalStatus = 1;
                    }
                    else if (!child.PlanStartDate.HasValue || !child.PlanDueDate.HasValue)
                    {//thiếu ngày triển khai
                        child.InternalStatus = 6;
                    }
                    else if (today > child.PlanDueDate.Value && child.DoneRatio < 100)
                    {
                        child.InternalStatus = 3;
                    }
                    else if (child.ContractDueDate.HasValue && (child.PlanDueDate > child.ContractDueDate))
                    {
                        child.InternalStatus = 2;
                    }
                    else
                    {
                        child.InternalStatus = 1;
                    }
                }


                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }
            return listResult;
        }
        public List<ScheduleEntity> SortPlanReturn(List<ScheduleEntity> data, string Id)
        {
            List<PlanAssignment> planAssignments = new List<PlanAssignment>();
            List<PlanAssignment> planAssignmentCopys = new List<PlanAssignment>();

            List<ScheduleEntity> listReturn = new List<ScheduleEntity>();
            List<ScheduleEntity> values = new List<ScheduleEntity>();
            List<string> stageIds = data.Where(a => a.IsPlan == false).Select(a => a.StageId).ToList();
            List<ScheduleEntity> stagesPasers = data.Where(a => a.IsPlan == false).ToList();
            var plans = db.Plans.AsNoTracking().ToList();
            var listPlan = (from a in db.Plans.AsNoTracking() join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                            where a.ProjectProductId.Equals(Id) && a.IsPlan == false && !stageIds.Contains(a.StageId)
                            select new ScheduleEntity
                            {
                                Id = a.Id,
                                ProjectProductId = a.ProjectProductId,
                                StageId = a.StageId,
                                ParentId = a.ParentId,
                                StageName = a.Name.ToUpper(),
                                IsPlan = a.IsPlan,
                                Status = a.Status,
                                Description = a.Description,
                                Weight = a.Weight,
                                DoneRatio = a.DoneRatio,
                                Type = a.Type,
                                IndexPlan = b.index,
                                SupplierId = a.SupplierId,
                                ContractStartDate = a.ContractStartDate,
                                ContractDueDate = a.ContractDueDate,
                                PlanDueDate = a.PlanDueDate,
                                PlanStartDate = a.PlanStartDate,
                                Duration = a.Duration,
                                Color = b.Color,
                                BackgroundColor = b.Color,
                            }).OrderBy(a =>a.IndexPlan).ToList();
            listPlan.AddRange(stagesPasers);
            listPlan = listPlan.OrderBy(a => a.IndexPlan).ToList();
            foreach (var plan in listPlan)
            {
                plan.ParentId = stagesPasers[0].ParentId;
                var listPlanOfStage = plans.Where(a => plan.Id.Equals(a.ParentId)).Select(a => new ScheduleEntity
                {
                    Id = a.Id,
                    ProjectProductId = a.ProjectProductId,
                    StageId = a.StageId,
                    ParentId = a.ParentId,
                    PlanName = a.Name.ToUpper(),
                    IsPlan = a.IsPlan,
                    Status = a.Status,
                    EstimateTime = a.EstimateTime,
                    Description = a.Description,
                    Weight = a.Weight,
                    DoneRatio = a.DoneRatio,
                    Type = a.Type,
                    IndexPlan = a.Index,
                    SupplierId = a.SupplierId,
                    ContractStartDate = a.ContractStartDate,
                    ContractDueDate = a.ContractDueDate,
                    PlanDueDate = a.PlanDueDate,
                    PlanStartDate = a.PlanStartDate,
                    Duration = a.Duration,
                }).OrderBy(a => a.IndexPlan).ToList();
                listReturn.Add(plan);
                listReturn.AddRange(listPlanOfStage);
            }
            foreach (var pos in listReturn)
            {
                DateTime today = DateTimeUtils.ConvertDateFrom(DateTime.Today);

                if (!pos.PlanStartDate.HasValue || !pos.PlanDueDate.HasValue)
                {
                    pos.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
                }
                else if (today > pos.PlanDueDate && pos.DoneRatio < 100)
                {
                    pos.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
                }
                else if (!pos.ContractStartDate.HasValue && pos.DoneRatio < 100)
                {
                    pos.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
                }
                else if (!pos.ContractDueDate.HasValue && pos.DoneRatio < 100)
                {
                    pos.InternalStatus = "TRỐNG KẾT THÚC HĐ";
                }
                else if (pos.ContractDueDate.HasValue && (pos.PlanDueDate > pos.ContractDueDate))
                {
                    pos.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
                }
                else
                {
                    pos.InternalStatus = "OK";
                }
                planAssignments = new List<PlanAssignment>();
                planAssignments = db.PlanAssignments.Where(a => a.PlanId.Equals(pos.Id)).ToList();

                planAssignmentCopys = new List<PlanAssignment>();
                foreach (var planAssignment in planAssignments)
                {
                    planAssignmentCopys.Add(new PlanAssignment
                    {
                        Id = Guid.NewGuid().ToString(),
                        PlanId = pos.Id,
                        UserId = planAssignment.UserId,
                        IsMain = planAssignment.IsMain,
                        EmployeeId = planAssignment.EmployeeId
                    });
                }

                db.PlanAssignments.AddRange(planAssignmentCopys);
                pos.MainUserId = planAssignmentCopys.Where(r => r.IsMain).Select(r => r.UserId).FirstOrDefault();
                pos.ListIdUserId = db.PlanAssignments.Where(a => a.PlanId.Equals(pos.Id)).OrderBy(a => a.IsMain).Select(a => a.UserId).ToList();
                pos.ResponsiblePersionName = string.Join(", ", (from s in db.PlanAssignments.AsNoTracking()
                where s.PlanId.Equals(pos.Id)
                                                                 join m in db.Users.AsNoTracking() on s.UserId equals m.Id
                                                                 orderby s.IsMain descending
                                                                 select m.UserName).ToArray());
            }
            return listReturn;
        } 
    }
}
