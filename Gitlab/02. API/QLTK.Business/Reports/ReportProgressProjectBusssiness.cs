using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Payment;
using NTS.Model.Plans;
using NTS.Model.Reports.ReportProgressProjetct;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using QLTK.Business.ProjectProducts;
using QLTK.Business.ScheduleProject;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QLTK.Business.Reports
{
    public class ReportProgressProjectBusssiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public ReportProject ReportProgressProjects(ReportProgressProjectSearch model)
        {
            SearchResultModel<List<ReportProgressProjectModel>> result = new SearchResultModel<List<ReportProgressProjectModel>>();
            List<ReportProgressProjectModel> reportProgressProjects = new List<ReportProgressProjectModel>();
            ReportProgressProjectModel report = null;
            var employees = db.Employees.AsNoTracking().ToList();
            var errors = db.Errors.AsNoTracking().ToList();

            var ListStatus = new Dictionary<string, string>  {
                { "1", "Chưa kickoff"},
                { "2", "Sản xuất" },
                { "4", "Tạm dừng" },
                { "5", "Lắp đặt" },
                { "6", "Hiệu chỉnh" },
                { "7", "Đưa vào sử dụng" },
                { "8", "Thiết kế" },
                { "9", "Nghiệm thu" },
                { "3","Đóng dự án" },
                { "10", "Đã thanh lý" },
                { "11","Vật tư" }
                }.ToList();
            var ListPaymentStatus = new Dictionary<int?, string>  {
                { 1, "Quá hạn"},
                { 0, "Không" }
                }.ToList();
            var ListMucDoUuTien = new Dictionary<int, string>  {
                { 1, "Thấp"},
                { 2, "Trung bình" },
                { 3, "Cao" },
                { 4, "Quan trọng" }
                }.ToList();
            List<StageReportModel> stages = new List<StageReportModel>();
            var stage = db.Stages.AsNoTracking().OrderBy(a => a.index).ToList();
            var payments = db.Payments.AsNoTracking().ToList();
            var dataQuery = (from b in db.Projects.AsNoTracking()
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             orderby b.DateFrom descending
                             select new ReportProgressProjectModel
                             {
                                 ProjectId = b.Id,
                                 TenVaMaDuAn = b.Code + "-" + b.Name,
                                 MaDuAn = b.Code,
                                 Type = b.Type,
                                 Priority = b.Priority,
                                 ManageId = b.ManageId,
                                 Status = b.Status,
                                 PaymentStatus = b.PaymentStatus,
                                 KeHoachHoanThanh = b.DateTo,
                                 KeHoachKickoff = b.KickOffDate,
                                 ThongTinDuAn = b.Progress,
                                 StartDate = b.DateFrom,
                                 SBUId = b.SBUId,
                                 SBUName = c.Name,
                                 DepartmentId = b.DepartmentId,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(r => r.TenVaMaDuAn.ToUpper().Contains(model.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuery = dataQuery.Where(r => r.SBUId.Equals(model.SBUId));
            }
            if (model.Priority != 0)
            {
                dataQuery = dataQuery.Where(r => r.Priority.Equals(model.Priority));
            }
            if (model.Type != 0)
            {
                dataQuery = dataQuery.Where(r => r.Type.Equals(model.Type));
            }
            if (!string.IsNullOrEmpty(model.ManageId))
            {
                dataQuery = dataQuery.Where(r => r.ManageId.Equals(model.ManageId));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(r => r.DepartmentId.Equals(model.DepartmentId));
            }
            if (model.Status != null && model.Status.Count >0)
            {
                var results = new List<ReportProgressProjectModel>();
                foreach (var s in model.Status)
                {
                    results.AddRange( dataQuery.Where(r => r.Status.Equals(s.ToString())).ToList());
                }
                dataQuery = results.AsQueryable();
            }
            if (model.PaymentStatus != null)
            {
                dataQuery = dataQuery.Where(r => r.PaymentStatus==model.PaymentStatus);
            }
            //search by date start
            if (model.DateStartFrom != null)
            {
                dataQuery = dataQuery.Where(r => r.StartDate >= model.DateStartFrom);
            }
            if (model.DateStartTo != null)
            {
                dataQuery = dataQuery.Where(r => r.StartDate <= model.DateStartTo);
            }
            //seach by plan due date
            if (model.PlanDueDateFrom != null)
            {
                dataQuery = dataQuery.Where(r => r.KeHoachHoanThanh >= model.PlanDueDateFrom);
            }
            if (model.PlanDueDateTo != null)
            {
                dataQuery = dataQuery.Where(r => r.KeHoachHoanThanh <= model.PlanDueDateTo);
            }
            //seach by kickoff date
            if (model.PlanKICKOFFFrom != null)
            {
                dataQuery = dataQuery.Where(r => r.KeHoachKickoff >= model.PlanKICKOFFFrom);
            }
            if (model.PlanKICKOFFTo != null)
            {
                dataQuery = dataQuery.Where(r => r.KeHoachKickoff <= model.PlanKICKOFFTo);
            }
            //
            if(model.VDTD != 0)
            {
                List<ReportProgressProjectModel> list = new List<ReportProgressProjectModel>();
                foreach (var d in dataQuery.ToList())
                {
                    var data = errors.Where(a => a.ProjectId.Equals(d.ProjectId)).ToList();
                    var Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
                    var NoImplementation = data.Where(a => a.Status == 3).Count();
                    var Done = data.Where(a => a.Status == 7 || a.Status == 9 || a.Status == 10).Count();
                    if (model.VDTD == 3 && Implementation==0 && NoImplementation ==0 && Done == 0)// không có vấn đề tồn đọng
                    {
                        list.Add(d);
                    }else if (model.VDTD == 2 && Implementation == 0 && NoImplementation == 0 && Done !=0)// vấn đề đã xử lý xong
                    {
                        list.Add(d);
                    }else if (model.VDTD == 1 && (Implementation != 0 || NoImplementation != 0))// đang có vấn đề
                    {
                        list.Add(d);
                    }
                }
                dataQuery = list.AsQueryable();
            }
            if (model.DelayType != 0)
            {
                List<ReportProgressProjectModel> list = new List<ReportProgressProjectModel>();
                foreach(var d in dataQuery.ToList())
                {
                    var delay = (d.KeHoachKickoff == null || d.KeHoachHoanThanh == null) ? null : ((DateTime)d.KeHoachKickoff - (DateTime)d.KeHoachHoanThanh).Days.ToString();
                    if(model.DelayType == 1 && delay != null && int.Parse(delay) == model.Delay)
                    {
                        list.Add(d);
                    }else if (model.DelayType == 2 && delay != null && int.Parse(delay) > model.Delay)
                    {
                        list.Add(d);
                    }
                    else if (model.DelayType == 3 && delay != null && int.Parse(delay) >= model.Delay)
                    {
                        list.Add(d);
                    }else if (model.DelayType == 4 && delay != null && int.Parse(delay) < model.Delay)
                    {
                        list.Add(d);
                    }else if (model.DelayType == 5 && delay != null && int.Parse(delay) <= model.Delay)
                    {
                        list.Add(d);
                    }
                }
                dataQuery = list.AsQueryable();
            }
            var allStage = (from a in db.Plans.AsNoTracking()
                        join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                        where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && (a.Status != (int)Constants.ScheduleStatus.Stop && a.Status != (int)Constants.ScheduleStatus.Cancel)
                        orderby b.index
                        select new StageReportModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Status = a.Status,
                            DoneRatio = a.DoneRatio,
                            Weight = a.Weight,
                            MaxContractDate = a.ContractDueDate,
                            Index = b.index,
                        }).ToList();
            if ( !string.IsNullOrEmpty(model.Stage) && model.StageStatus != 0)
            {
                List<ReportProgressProjectModel> list = new List<ReportProgressProjectModel>();
                // lấy ra công đoạn của 1 dự án
                foreach (var item1 in allStage)
                {
                    var s = stages.FirstOrDefault(a => a.Id.Equals(item1.Id));
                    if (s == null)
                    {
                        stages.Add(new StageReportModel
                        {
                            Id = item1.Id,
                            Name = item1.Name,
                            Index = item1.Index,
                        });
                    }
                }
                stages = stages.OrderBy(a => a.Index).ToList();

                foreach (var d in dataQuery.ToList())
                {
                    d.Stages = GetStages(d.ProjectId, stages);
                    var check = d.Stages.FirstOrDefault(a => a.Id.Equals(model.Stage));
                    if(check != null && model.StageStatus == 1 && (check.Status ==(int)Constants.ScheduleStatus.Stop || check.Status== (int)Constants.ScheduleStatus.Stop))//không triển khai
                    {
                        list.Add(d);
                    }
                    else if (check != null && model.StageStatus == 2 && check.Status == (int)Constants.ScheduleStatus.Open)// chưa triển khai
                    {
                        list.Add(d);
                    }
                    else if (check != null && model.StageStatus == 3 && check.Status == (int)Constants.ScheduleStatus.Ongoing)// đang triển khai
                    {
                        list.Add(d);
                    }
                    else if (check != null && model.StageStatus == 4 && check.Status == (int)Constants.ScheduleStatus.Closed)// hoàn thành
                    {
                        list.Add(d);
                    }
                }
                dataQuery = list.AsQueryable();
            }
            // lọc tất cả các công đoạn để đếm số lượng  công đoạn
            if (model.IsSynchronized == true)
            {
                foreach (var item1 in allStage)
                {
                    var s = stages.FirstOrDefault(a => a.Id.Equals(item1.Id));
                    if (s == null)
                    {
                        stages.Add(new StageReportModel
                        {
                            Id = item1.Id,
                            Name = item1.Name,
                            Index = item1.Index,
                        });
                    }
                }
                stages = stages.OrderBy(a => a.Index).ToList();
                List<ReportProgressProjectModel> results = new List<ReportProgressProjectModel>();
                foreach (var d in dataQuery.ToList())
                {
                    d.Stages = GetStages(d.ProjectId, stages);
                    results.Add(d);
                }
                dataQuery = results.AsQueryable();
                foreach (var s in stages)
                {
                    var countTotalStage = 0;
                    var countStageNotDone = 0;
                    foreach (var d in dataQuery.ToList())
                    {
                        if(d.Stages.Count > 0)
                        {
                            var status = d.Stages.Where(a => a.Id.Equals(s.Id)).Select(a => a.Status).FirstOrDefault();
                            if (status != 4 && status != 5 && status != 0)
                            {
                                countTotalStage++;
                                if (status != 3)
                                {
                                    countStageNotDone++;
                                }
                            }
                        }
                    }
                    s.InfoStage = "" + countStageNotDone + "/" + countTotalStage;
                }
            }

            var totalItem = dataQuery.Count();
            var countCKO = dataQuery.Where(a => a.Status.Equals("1")).Count();
            var countSx = dataQuery.Where(a => a.Status.Equals("2")).Count();
            var countDDA = dataQuery.Where(a => a.Status.Equals("3")).Count();
            var countTD = dataQuery.Where(a => a.Status.Equals("4")).Count();
            var countLD = dataQuery.Where(a => a.Status.Equals("5")).Count();
            var countHC = dataQuery.Where(a => a.Status.Equals("6")).Count();
            var countDVSD = dataQuery.Where(a => a.Status.Equals("7")).Count();
            var countTK = dataQuery.Where(a => a.Status.Equals("8")).Count();
            var countNT = dataQuery.Where(a => a.Status.Equals("9")).Count();
            var countDTL = dataQuery.Where(a => a.Status.Equals("10")).Count();
            var countVT = dataQuery.Where(a => a.Status.Equals("11")).Count();

            var datas = dataQuery.ToList();

            foreach (var item in datas)
            {
                item.TongTienHD = payments.Where(x => x.ProjectId.Equals(item.ProjectId)).Select(t => t.PlanAmount).Sum();
                item.SoTienDaThu = payments.Where(x => x.ProjectId.Equals(item.ProjectId)).Select(t => t.TotalAmount).Sum();
            }
            var TongTienHopDong = datas.Select(x => x.TongTienHD).Sum();
            var TongSoTienDaThu = datas.Select(x => x.SoTienDaThu).Sum();

            var reports = dataQuery.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            //get stage page project có
            if (!string.IsNullOrEmpty(model.Stage) && model.StageStatus != 0)
            {
                foreach (var item1 in stages.ToList())
                {
                    List<StageReportModel> StageModels = new List<StageReportModel>();
                    foreach (var item in reports.ToList())
                    {
                        var s = item.Stages.FirstOrDefault(a => a.Id.Equals(item1.Id));
                        StageModels.Add(s);
                    }
                    // đếm sô công đoạn trạng thái là stop và cancel
                    var countStageCancelAndStop = StageModels.Where(a => a.Status == 4).Count();
                    if (countStageCancelAndStop == reports.Count())
                    {
                        //tất cả sản phẩm có công đoạn là stop hoặc cancel thì loại bỏ công đoạn đó ra
                        stages.Remove(item1);
                        foreach (var item in reports.ToList())
                        {
                            var s = item.Stages.FirstOrDefault(a => a.Id.Equals(item1.Id));
                            item.Stages.Remove(s);
                        }
                    }

                }
            }
            else
            {
                foreach (var item in reports)
                {
                    // lấy ra tất cả công đoạn của dự án
                    var allStageOfProject = (from a in db.Plans.AsNoTracking()
                                             join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                             where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && a.ProjectId.Equals(item.ProjectId) && (a.Status != (int)Constants.ScheduleStatus.Stop && a.Status != (int)Constants.ScheduleStatus.Cancel)
                                             orderby b.index
                                             select new StageReportModel
                                             {
                                                 Id = b.Id,
                                                 Name = b.Name,
                                                 Status = a.Status,
                                                 DoneRatio = a.DoneRatio,
                                                 Weight = a.Weight,
                                                 MaxContractDate = a.ContractDueDate,
                                                 Index = b.index,
                                             }).ToList();
                    // lấy ra công đoạn của 1 dự án
                    List<StageReportModel> ListStage = new List<StageReportModel>();
                    foreach (var item1 in allStageOfProject)
                    {
                        var s = stages.FirstOrDefault(a => a.Id.Equals(item1.Id));
                        if (s == null)
                        {
                            stages.Add(new StageReportModel
                            {
                                Id = item1.Id,
                                Name = item1.Name,
                                Index =item1.Index,
                            });
                        }
                    }
                    stages = stages.OrderBy(a => a.Index).ToList();
                }
            }
            //tính
            foreach (var item in reports)
            {
                item.MucdoUuTien = ListMucDoUuTien.FirstOrDefault(a => a.Key.Equals(item.Type)).Value;
                item.NguoiPhuTrach = employees.FirstOrDefault(a => a.Id.Equals(item.ManageId)) == null ? null : employees.FirstOrDefault(a => a.Id.Equals(item.ManageId)).Name;
                item.TinhTrangDuAn = ListStatus.FirstOrDefault(a => a.Key.Equals(item.Status)).Value;
                item.TinhTrangCongNo = ListPaymentStatus.FirstOrDefault(a => a.Key.Equals(item.PaymentStatus)).Value;
                item.ChenhLech = (item.KeHoachKickoff == null || item.KeHoachHoanThanh == null) ? null : ((DateTime)item.KeHoachKickoff - (DateTime)item.KeHoachHoanThanh).Days.ToString();
                if (string.IsNullOrEmpty(model.Stage) || model.StageStatus == 0)
                {
                    item.Stages = GetStages(item.ProjectId, stages);

                }
                //item.VDTD = GetError(item.ProjectId);
                var data = db.Errors.Where(a => a.ProjectId.Equals(item.ProjectId)).ToList();
                //var Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
                //var NoImplementation = data.Where(a => a.Status == 3).Count();
                item.CountNoImplementError = data.Where(a => a.Status == 3).Count();
                item.CountImplementError = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
                item.TotalError = item.CountNoImplementError + item.CountImplementError;
                item.TongTienHD = payments.Where(x => x.ProjectId.Equals(item.ProjectId)).Select(t => t.PlanAmount).Sum();
                item.SoTienDaThu = payments.Where(x => x.ProjectId.Equals(item.ProjectId)).Select(t => t.TotalAmount).Sum();
                if (item.ChenhLech != null && int.Parse(item.ChenhLech) < 0)
                {
                    item.StatusChenhLech = 0;
                }
                else
                {
                    item.StatusChenhLech = 1;
                }
            }
            ReportProject reportProject = new ReportProject();
            reportProject.Stages = stages;
            reportProject.ReportProgressProjectModels.ListResult = reports;
            reportProject.ReportProgressProjectModels.TotalItem = totalItem;
            reportProject.CountVT = countVT;
            reportProject.CountTK = countTK;
            reportProject.CountDDA = countDDA;
            reportProject.CountCKO = countCKO;
            reportProject.CountDVSD = countDVSD;
            reportProject.CountHC = countHC;
            reportProject.CountLD = countLD;
            reportProject.CountSx = countSx;
            reportProject.CountTD = countTD;
            reportProject.CountNT = countNT;
            reportProject.CountDTL = countDTL;
            reportProject.TongTienHopDong = (decimal)TongTienHopDong;
            reportProject.TongSoTienDaThu = TongSoTienDaThu;

            return reportProject;
        }

        public List<StageReportModel> GetStages(string id, List<StageReportModel> stageModels)
        {
            List<StageReportModel> stages = stageModels.Select(a => new StageReportModel { Id = a.Id, Name = a.Name }).ToList();
            var ProductOfProjects = GetProjectProductByProjectId(id).ProductPlans;
            foreach (var s in stages)
            {
                decimal weight = 0;
                decimal done = 0;
                List<DateTime?> date = new List<DateTime?>();
                foreach (var pp in ProductOfProjects)
                {
                    foreach (var stage in pp.StageModels)
                    {
                        if (stage.StageId.Equals(s.Id))
                        {
                            done = done + stage.DoneRatio * pp.Weight;
                            weight = weight + pp.Weight;
                            date.Add(stage.Date);
                        }
                    }
                }
                s.MaxContractDate = date.Max();
                if (weight == 0)
                {
                    s.Status = 4;
                }
                else
                {
                    var ratio = done / weight;
                    if (ratio == 0)
                    {
                        s.Status = 1;
                        s.DoneRatio = 0;
                    }
                    else if (ratio == 100)
                    {
                        s.Status = 3;
                        s.DoneRatio = 100;
                    }
                    else
                    {
                        s.Status = 2;
                        s.DoneRatio = ratio;
                    }
                }

            }
            return stages;
        }
        public string GetError(string projectId)
        {
            DateTime dateNow = DateTime.Now;

            var data = db.Errors.Where(a => a.ProjectId.Equals(projectId)).ToList();
            var Implementation = data.Where(a => a.Status == 5 || a.Status == 6 || a.Status == 8).Count();
            var NoImplementation = data.Where(a => a.Status == 3).Count();
            var errorDelays = (from a in db.Errors.AsNoTracking()
                               join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                               where a.Status != Constants.Error_Status_Close && a.Status != Constants.Error_Status_Done_QC && c.Status != Constants.ErrorFix_Status_Finish && a.ProjectId.Equals(projectId)
                               select new
                               {
                                   Id = a.Id,
                                   Status = a.Status,
                                   FixStatus = c.Status,
                                   Deadline = (c.DateTo.HasValue && (dateNow > c.DateTo)) && (c.Status != Constants.ErrorFix_Status_Finish) ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                               }).AsQueryable();

            return "" + NoImplementation + "/" + Implementation + "/" + (Implementation + NoImplementation);
        }

        public ProjectPlan GetProjectProductByProjectId(string id)
        {
            ProjectPlan projectPlan = new ProjectPlan();
            var listProjectProduct = GetListPlanByProjectId(id);
            var listProjectProductParent = listProjectProduct.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            var allStageOfProject = (from a in db.Plans.AsNoTracking()
                                     join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                     where a.IsPlan == false && !string.IsNullOrEmpty(a.StageId) && a.ProjectId.Equals(id)
                                     orderby b.index
                                     select new StageModel
                                     {
                                         StageId = b.Id,
                                         StageName = b.Name,
                                         Date = a.PlanDueDate,
                                         PlanId = a.Id
                                     }).ToList();
            List<StageModel> stages = new List<StageModel>();
            foreach (var item in allStageOfProject)
            {
                var stage = stages.FirstOrDefault(a => a.StageId.Equals(item.StageId));
                if (stage == null)
                {
                    stages.Add(item);
                }
            }
            List<ProductPlan> productPlans = new List<ProductPlan>();
            foreach (var item in listProjectProductParent)
            {
                List<ScheduleEntity> listResult = new List<ScheduleEntity>();
                List<ScheduleEntity> listChild = new List<ScheduleEntity>();

                listChild = GetScheduleProjectChild(item.Id, listProjectProduct);
                listResult.Add(item);
                listResult.AddRange(listChild);
                var listPlanOfProject = listResult.Where(a => a.ProjectId.Equals(id)).ToList();

                ProductPlan productPlan = new ProductPlan();
                productPlan.ProductName = item.NameView;
                productPlan.PlanId = item.Id;
                List<StageModel> listStages = new List<StageModel>();
                foreach (var item1 in allStageOfProject)
                {
                    var stage = listStages.FirstOrDefault(a => item1.StageId.Equals(a.StageId));
                    if (stage == null)
                    {
                        listStages.Add(new StageModel { StageName = item1.StageName, StageId = item1.StageId });
                    }
                }
                foreach (var stage in listStages)
                {
                    var listStageOfProject = listPlanOfProject.Where(a => stage.StageId.Equals(a.StageId) && !string.IsNullOrEmpty(a.StageName)).ToList();
                    List<StageModel> stageModels = new List<StageModel>();
                    foreach (var planStage in listStageOfProject)
                    {
                        var s = CaclulatorStage(listResult, planStage);
                        stageModels.Add(s);
                    }
                    int weigth = 0;
                    decimal done = 0;
                    foreach (var item1 in stageModels)
                    {
                        if (!Constants.ScheduleStatus.Cancel.Equals(item1.Status) && !Constants.ScheduleStatus.Stop.Equals(item1.Status))
                        {
                            weigth = weigth + item1.Weight;
                            done = done + item1.DoneRatio * item1.Weight;
                        }
                    }
                    if (weigth == 0)
                    {
                        stage.Status = 4;
                    }
                    else
                    {
                        var resultDone = done / weigth;
                        stage.DoneRatio = resultDone;
                        if (resultDone == 0)
                        {
                            stage.Status = 1;
                        }
                        else if (resultDone == 100)
                        {
                            stage.Status = 3;
                        }
                        else
                        {
                            stage.Status = 2;
                        }
                        var countStopCancel = stageModels.Where(a => a.Status == 4 || a.Status == 5).ToList().Count();
                        if (countStopCancel == stageModels.Count())
                        {
                            stage.Status = 4;
                        }
                        stage.Date = stageModels.Select(a => a.Date).Max();
                    }



                }
                productPlan.StageModels = listStages;
                productPlan.Weight = item.Weight;

                productPlans.Add(productPlan);
            }
            // xóa những sp đang stop hoặc cancel
            foreach (var item in productPlans.ToList())
            {
                var countStageStopCancel = item.StageModels.Where(a => a.Status == 4).Count();
                if (countStageStopCancel == item.StageModels.Count())
                {
                    productPlans.Remove(item);
                }
            }
            foreach (var item1 in projectPlan.StageModels.ToList())
            {
                List<StageModel> StageModels = new List<StageModel>();
                foreach (var item in productPlans.ToList())
                {
                    var s = item.StageModels.FirstOrDefault(a => a.StageName.Equals(item1.StageName));
                    StageModels.Add(s);
                }
                // đếm sô công đoạn trạng thái là stop và cancel
                var countStageCancelAndStop = StageModels.Where(a => a.Status == 4).Count();
                if (countStageCancelAndStop == productPlans.Count())
                {
                    //tất cả sản phẩm có công đoạn là stop hoặc cancel thì loại bỏ công đoạn đó ra
                    projectPlan.StageModels.Remove(item1);
                    foreach (var item in productPlans.ToList())
                    {
                        var s = item.StageModels.FirstOrDefault(a => a.StageName.Equals(item1.StageName));
                        item.StageModels.Remove(s);
                    }
                }

            }
            projectPlan.ProductPlans = (productPlans);

            return projectPlan;
        }

        private StageModel CaclulatorStage(List<ScheduleEntity> listPlanOfProject, ScheduleEntity stage)
        {
            if (!string.IsNullOrEmpty(stage.ParentId))
            {
                var parentStage = listPlanOfProject.FirstOrDefault(a => a.Id.Equals(stage.ParentId));
                if (string.IsNullOrEmpty(parentStage.ParentId))
                {
                    StageModel stageModel = new StageModel();
                    stageModel.PlanId = stage.Id;
                    stageModel.StageName = stage.StageName;
                    stageModel.Status = stage.Status;
                    stageModel.Weight = stage.Weight;
                    stageModel.Date = stage.PlanDueDate;
                    stageModel.DoneRatio = stage.DoneRatio;
                    return stageModel;
                }
                else
                {
                    StageModel stageModel = new StageModel();
                    stageModel.StageName = stage.StageName;
                    stageModel.Status = stage.Status;
                    stageModel.Weight = stage.Weight;
                    stageModel.Date = stage.PlanDueDate;
                    stageModel.DoneRatio = stage.DoneRatio;

                    var weigth = CaclulatorWeigth(listPlanOfProject, parentStage, stageModel);
                    stageModel.Weight = weigth;
                    return stageModel;
                }
            }
            return null;
        }
        private List<ScheduleEntity> GetScheduleProjectChild(string parentId, List<ScheduleEntity> listSchedulePrject)
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
        private int CaclulatorWeigth(List<ScheduleEntity> listPlanOfProject, ScheduleEntity parentStage, StageModel stage)
        {
            var weigth = stage.Weight * parentStage.Weight;

            var Parent = listPlanOfProject.FirstOrDefault(a => a.Id.Equals(parentStage.ParentId));
            if (string.IsNullOrEmpty(Parent.ParentId))
            {
                return weigth;
            }
            else
            {
                stage.Weight = weigth;
                return CaclulatorWeigth(listPlanOfProject, Parent, stage);
            }
        }

        public List<ScheduleEntity> GetListPlanByProjectId(string id)
        {
            // Lấy dữ liệu kế hoạch trong danh mục sản phẩm của dự án
            var projectProducts = (from a in db.ProjectProducts.AsNoTracking().Where(r => r.ProjectId.Equals(id))
                                   join c in db.Products.AsNoTracking() on a.ProductId equals c.Id into
                                   ac
                                   from ca in ac.DefaultIfEmpty()
                                   select new ScheduleEntity()
                                   {
                                       Id = a.Id,
                                       ParentId = a.ParentId,
                                       RealQuantity = a.RealQuantity,
                                       Weight = a.Weight,
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
                                       ProjectId = id,
                                       ModuleStatus = a.ModuleStatus

                                   }
                           ).ToList();

            // Lấy danh sách công việc chi tiết
            var plans = (from a in db.Plans.AsNoTracking().Where(r => r.ProjectId.Equals(id))
                         join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                         orderby b.index, a.Index
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
                             Description = a.Description,
                             ProjectId = id
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
            List<ScheduleEntity> schedules = projectProducts.Union(plans).ToList();
            return schedules;
        }
    }
}
