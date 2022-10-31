using NTS.Model.Combobox;
using NTS.Model.DashBroadProject;
using NTS.Model.Project;
using NTS.Model.Repositories;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Model.Plans;
using System.Data.Entity.SqlServer;
using System.Data.Objects;

namespace QLTK.Business.DashBroadProject
{
    public class DashBroadProjectBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public object GetDashBroadProject(DashBroadProjectSearchModel model)
        {
            DashBroadProjectResultModel result = new DashBroadProjectResultModel();
            // ListProject
            var listProject = db.Projects.OrderBy(o => o.Code).AsQueryable();
            #region đk tìm kiếm
            if (!string.IsNullOrEmpty(model.Code))
            {
                listProject = listProject.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()) || r.Name.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                listProject = listProject.Where(r => r.DepartmentId.Equals(model.DepartmentId));
            }
            else if (!string.IsNullOrEmpty(model.SBUId))
            {
                listProject = listProject.Where(r => r.SBUId.Equals(model.SBUId));
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                listProject = listProject.Where(r => r.Status.Equals(model.Status));
            }

            if (model.Type.HasValue)
            {
                listProject = listProject.Where(u => u.Type == model.Type.Value);
            }

            if (model.DateFrom.HasValue)
            {
                listProject = listProject.Where(u => u.DateFrom != null ? u.DateFrom >= model.DateFrom : u.KickOffDate >= model.DateFrom);

            }

            if (model.DateTo.HasValue)
            {
                listProject = listProject.Where(u => u.DateFrom != null ? u.DateFrom <= model.DateTo : u.KickOffDate <= model.DateTo);
            }

            if (model.DocumentStatus.HasValue)
            {
                listProject = listProject.Where(r => r.DocumentStatus == model.DocumentStatus.Value);
            }

            if (model.ErrorStatus.HasValue)
            {
                List<string> projectIds = new List<string>();

                projectIds = (from a in db.Errors.AsNoTracking()
                              where a.Status != Constants.Problem_Status_Close && a.Status != Constants.Problem_Status_Ok_QC && a.Status > Constants.Problem_Status_Awaiting_Confirm
                              group a by a.ProjectId into g
                              select g.Key).ToList();

                if (model.ErrorStatus.Value == 1)
                {
                    listProject = listProject.Where(r => !projectIds.Contains(r.Id));
                }
                else if (model.ErrorStatus.Value == 2)
                {
                    listProject = listProject.Where(r => projectIds.Contains(r.Id));
                }
            }

            //
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                listProject = listProject.Where(a => a.SBUId.Equals(model.SBUId));
            }

            if (model.DateFrom.HasValue)
            {
                listProject = listProject.Where(a => a.KickOffDate >= model.DateFrom);
            }
            if (model.DateTo.HasValue)
            {
                listProject = listProject.Where(a => a.KickOffDate <= model.DateTo);
            }

            if (!string.IsNullOrEmpty(model.ProjCode))
            {
                listProject = listProject.Where(a => a.Code.ToLower().Contains(model.ProjCode.ToLower()) || a.Name.ToLower().Contains(model.ProjCode.ToLower()));
            }
            #endregion
            #region Table Project
            var projects = (from a in listProject
                            join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                            from b in ab.DefaultIfEmpty()
                            join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                            from c in ac.DefaultIfEmpty()
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                            from d in ad.DefaultIfEmpty()
                            select new ProjectResultModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                //CustomerId = ba.Id,
                                CustomerName = b.Name,
                                //SBUId = ca.Id,
                                SBUName = c.Name,
                                //DepartmentId = da.Id,
                                DepartmentName = d.Name,
                                DateFrom = a.DateFrom,
                                DateTo = a.DateTo,
                                KickOffDate = a.KickOffDate,
                                Type = a.Type,
                                StatusProject = a.Status,
                                DocumentStatus = a.DocumentStatus,
                                CustomerTypeId = b.CustomerTypeId,
                                CustomerType = db.CustomerTypes.Where(ct => ct.Id.Equals(b.CustomerTypeId)).FirstOrDefault().Name,
                                Priority = a.Priority
                            }).AsQueryable();
            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                projects = projects.Where(r => r.CustomerName.Contains(model.CustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.CustomerTypeId))
            {
                projects = projects.Where(r => r.CustomerTypeId.Equals(model.CustomerTypeId));
            }

            #region Các list xử lý
            // ListPlan
            //var listPlan = (from a in db.Plans.AsNoTracking()
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in projects on d.ProjectId equals b.Id
            //                join c in db.Tasks.AsNoTracking() on a.TaskId equals c.Id
            //                where string.IsNullOrEmpty(a.ReferenceId)
            //                select new DashBoardProjectPlanModel
            //                {
            //                    Id = a.Id,
            //                    ProjectId = a.ProjectId,
            //                    ProjectProductId = a.ProjectProductId,
            //                    Type = c.Type,
            //                    RealStartDate = a.RealStartDate,
            //                    RealEndDate = a.RealEndDate,
            //                    StartDate = a.StartDate,
            //                    ResponsiblePersion = a.ResponsiblePersion,
            //                    EndDate = a.EndDate,
            //                    Status = a.Status,
            //                    ExpectedDesignFinishDate = d.ExpectedDesignFinishDate,
            //                    ExpectedMakeFinishDate = d.ExpectedMakeFinishDate,
            //                    ExpectedTransferDate = d.ExpectedTransferDate,
            //                    Plan = (a.RealEndDate != null && a.EndDate != null) ? (a.RealEndDate <= a.EndDate ? 1: 2) :0,
            //                    KickOff = (a.EndDate != null && d.ExpectedDesignFinishDate != null) ? (a.EndDate <= d.ExpectedDesignFinishDate ? 1 : 2) :0,

            //                }).ToList();

            // ListProjectProduct
            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      join b in projects on a.ProjectId equals b.Id
                                      select new DashBoardProjecProductModel
                                      {
                                          DataType = a.DataType,
                                          DesignStatus = a.DesignStatus,
                                          Id = a.Id,
                                          ProjectId = a.ProjectId,
                                          DesignFinishDate = a.DesignFinishDate,
                                          ExpectedDesignFinishDate = a.ExpectedDesignFinishDate,
                                          ExpectedMakeFinishDate = a.ExpectedMakeFinishDate,
                                          ExpectedTransferDate = a.ExpectedTransferDate,
                                          MakeFinishDate = a.MakeFinishDate,
                                          KickOffDate = b.KickOffDate
                                      }).ToList();

            result.ListProjectInPlan = projects.Select(gr => new DashBroadProjectModel() { ProjectId = gr.Id, ProjectCode = gr.Code, ProjectName = gr.Name, Priority = gr.Priority }).ToList();

            #endregion

            //Tổng số dự án đang triển khai
            var dataProject = projects.Where(a => !a.StatusProject.Equals(Constants.Prooject_Status_Finish) && !a.StatusProject.Equals(Constants.Prooject_Status_NotStartedYet)).ToList();

            result.TotalProject = dataProject.Count();

            // Tổng số dự án đã được hoàn thành
            var dataProjectFinish = projects.Where(a => a.StatusProject.Equals(Constants.Prooject_Status_Finish)).ToList();

            result.TotalProjectFinish = dataProjectFinish.Count();

            #region Số lượng dự án đang triển khai


            // Số lượng dự án dự kiến chậm
            //var dataProjectLastdeadline = (from b in listPlan
            //                               join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                               where (((b.Type == Constants.Task_Doc) && b.RealEndDate > c.ExpectedMakeFinishDate) ||
            //                               (b.Type == (Constants.Task_Transfer) && b.RealEndDate > c.ExpectedTransferDate) ||
            //                               (b.Type == (Constants.Task_Design) && b.RealEndDate > c.ExpectedDesignFinishDate)) ||
            //                               string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null
            //                               group b.ProjectId by b.ProjectId into g
            //                               select new { ProjectId = g.Key }).ToList();

            //result.TotalProjectDelayDeadline = dataProjectLastdeadline.Count();



            //// Số lượng dự án đúng tiến độ

            //result.TotalProjectOnSchedule = projects.Count() - dataProjectLastdeadline.Count();

            //// số lượng dự án có hạng mục nhưng chưa lập kế hoạch

            //var dataProjectNotPlan = (from b in listPlan
            //                          where string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null
            //                          group b.ProjectId by b.ProjectId).ToList();

            //result.TotalProjectNotPlan = dataProjectNotPlan.Count();
            #endregion


            var results = projects.ToList();
            List<Error> errorInProjects;
            foreach (var item in results)
            {
                errorInProjects = db.Errors.AsNoTracking().Where(a => a.ProjectId.Equals(item.Id)).ToList();
                item.ErorrTotalDone = errorInProjects.Where(a => a.Type == Constants.Error_Type_Error && a.Status == Constants.Error_Status_Done_QC).Count();
                item.ErorrTotal = errorInProjects.Where(a => a.Type == Constants.Error_Type_Error && a.Status > Constants.Error_Status_Pending).Count();
                item.IssueTotalDone = errorInProjects.Where(a => a.Type == Constants.Error_Type_Issue && a.Status == Constants.Error_Status_Done_QC).Count();
                item.IssueTotal = errorInProjects.Where(a => a.Type == Constants.Error_Type_Issue && a.Status > Constants.Error_Status_Pending).Count();

                //var project_1 = dataProjectLastdeadline.FirstOrDefault(a => a.ProjectId.Equals(item.Id));
                var project_2 = dataProjectFinish.FirstOrDefault(a => a.Id.Equals(item.Id));
                //if (project_1 != null)
                //{
                //    item.Status = "Dự kiến chậm tiến độ";

                //}
                //else if (project_2 != null)
                //{
                //    item.Status = "Đã được hoàn thành";
                //}
                //else
                //{
                //    item.Status = "Đúng tiến độ";
                //}

            }
            result.Projects = results;
            #endregion

            List<DashBroadProjectModel> ObjectReporDesignProject = new List<DashBroadProjectModel>();
            List<DashBoardProjectPlanModel> planInProjects;
            List<DashBoardProjectPlanModel> listPlanDesign;
            List<DashBoardProjectPlanModel> listPlanDoc;
            List<DashBoardProjectPlanModel> listPlanTransfer;
            List<DashBoardProjectPlanModel> planModules;
            List<DashBoardProjectPlanModel> planDocs;
            List<DashBoardProjecProductModel> productInProjects;
            List<DashBoardProjecProductModel> productModules;
            List<DashBoardProjecProductModel> productParadigms;
            int totalProjectDesign, totalProductDoc, totalProductTransfer;
            int totalPlanInProjectFinish;
            foreach (var item in result.ListProjectInPlan)
            {
                //planInProjects = listPlan.Where(r => item.ProjectId.Equals(r.ProjectId)).ToList();
                productInProjects = listProjectProduct.Where(r => r.ProjectId.Equals(item.ProjectId)).ToList();

                #region Báo cáo tiến độ về mặt thiết kế

                //listPlanDesign = planInProjects.Where(r => r.Type == Constants.Task_Design).ToList();

                //// Tổng số Design (Project Design)
                //totalProjectDesign = (from b in listPlanDesign group b.ProjectProductId by b.ProjectProductId).Count();

                //// Tiến độ dự án về mặt thiết kế - dự kiến chậm
                //item.TotalDelayProjectDesign = (from b in listPlanDesign
                //                                where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null || b.RealEndDate > b.ExpectedDesignFinishDate)
                //                                group b.ProjectProductId by b.ProjectProductId).Count();

                //// Tiến độ dự án về mặt thiết kế có hạng mục - chưa lập kế hoạch 
                //item.TotalProjectDesignNotPlan = (from b in listPlanDesign
                //                                  where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                                  group b.ProjectProductId by b.ProjectProductId).Count();

                ////Số lượng thiết kế  đang đúng tiền độ	
                //item.TotalProjectDesignDoneBeforPlan = totalProjectDesign - item.TotalDelayProjectDesign;

                #endregion

                #region Báo cáo tiến độ về mặt tài liệu
                //listPlanDoc = planInProjects.Where(r => r.Type == Constants.Task_Doc).ToList();

                //// Tổng số sp (Project TL)
                //totalProductDoc = (from b in listPlanDoc group b.ProjectProductId by b.ProjectProductId).Count();

                //// Tiến độ dự án về mặt tài liệu -  dự kiến chậm
                //item.TotalDelayProjectDoc = (from b in listPlanDoc
                //                             where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null || b.RealEndDate > b.ExpectedMakeFinishDate)
                //                             group b.ProjectProductId by b.ProjectProductId).Count();


                ////  Tiến độ dự án về mặt tài liệu có hạng mục - chưa lập kế hoạch 
                //item.TotalProjectDocNotPlan = (from b in listPlanDoc
                //                               where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                               group b.ProjectProductId by b.ProjectProductId).Count();

                ////Số lượng tài liệu đang đúng tiền độ	
                //item.TotalProjectDocDoneBeforPlan = totalProductDoc - item.TotalDelayProjectDoc;
                #endregion

                #region Báo cáo tiến độ về mặt chuyển giao

                //listPlanTransfer = planInProjects.Where(r => r.Type == Constants.Task_Transfer).ToList();

                //// Tổng số sp (Project Transfer)
                //totalProductTransfer = (from b in listPlanTransfer group b.ProjectProductId by b.ProjectProductId).Count();

                //// Tiến độ dự án về mặt chuyển giao dự kiến chậm
                //item.TotalDelayProjectTransfer = (from b in listPlanTransfer
                //                                  where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null || b.RealEndDate > b.ExpectedTransferDate)
                //                                  group b.ProjectProductId by b.ProjectProductId).Count();

                ////Số lượng chuyển giao có hạng mục chưa lập kế hoạch
                //item.TotalProjectTransferNotPlan = (from b in listPlanTransfer
                //                                    where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                                    group b.ProjectProductId by b.ProjectProductId).Count();

                ////Số lượng chuyển giao đang đúng tiền độ	
                //item.TotalProjectTransferDoneBeforPlan = totalProductTransfer - item.TotalDelayProjectTransfer;
                #endregion


                #region Báo cáo tổng thể các dự án về mặt thiết kế 
                // % của tổng số dự án 
                //if (listPlanDesign.Count > 0)
                //{
                //    // số lượng công việc đã hoàn thành
                //    totalPlanInProjectFinish = listPlanDesign.Where(b => b.Status == Constants.Plan_Status_Done).Count();

                //    item.PercentDesign = Math.Round(((decimal)totalPlanInProjectFinish / (decimal)listPlanDesign.Count) * 100, 2);
                //}
                //else
                //{
                //    item.PercentDesign = 0;
                //}

                //// Sản phẩm là module
                //productModules = productInProjects.Where(r => r.DataType == Constants.ProjectProduct_DataType_Module).ToList();

                //// Tổng sô module thiết kê mới
                //planModules = (from a in listPlanDesign
                //               join c in productModules on a.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign
                //               select a).ToList();
                //item.TotalModuleDesign = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số module sửa thiết kế cũ 
                //planModules = (from b in listPlanDesign
                //               join c in productModules on b.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign
                //               select b).ToList();

                //item.TotalModuleUpdateDesign = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số module tận dụng có kế hoạch
                //planModules = (from b in listPlanDesign
                //               join c in productModules on b.ProjectProductId equals c.Id
                //               where c.DesignStatus == (Constants.ProjectProduct_DesignStatus_Use)
                //               select b).ToList();

                //item.TotalModuleUse = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();
                //item.TotalModuleUse += productModules.Where(r => r.DesignStatus == (Constants.ProjectProduct_DesignStatus_Use)).Count() - planModules.GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số mô hình thiết kế
                //productParadigms = productInProjects.Where(r => r.DataType == Constants.ProjectProduct_DataType_Paradigm).ToList();
                //planModules = (from b in listPlanDesign
                //               join c in productParadigms on b.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign
                //               select b).ToList();

                //item.TotalParadigmDesign = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số mô hình sửa thiết kế cũ 
                //planModules = (from b in listPlanDesign
                //               join c in productParadigms on b.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign
                //               select b).ToList();

                //item.TotalParadigmUpdateDesign = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số mô hình tận dụng có kế hoạch
                //planModules = (from b in listPlanDesign
                //               join c in productParadigms on b.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_Use
                //               select b).ToList();

                //item.TotalParadigmUse = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();
                //item.TotalParadigmUse += productParadigms.Where(r => r.DesignStatus == (Constants.ProjectProduct_DesignStatus_Use)).Count() - planModules.GroupBy(g => g.ProjectProductId).Count();

                //// Số lượng công việc chưa có kế hoạch
                //// Số lượng công việc chưa có kế hoạch theo module 
                //item.TotalModuleTaskNotPlan = (from b in listPlanDesign
                //                               join c in productModules on b.ProjectProductId equals c.Id
                //                               where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                               select b.Id).Count();

                //// Số lượng công việc chưa có kế hoạch theo mô hình
                //item.TotalParadigmTaskNotPlan = (from b in listPlanDesign
                //                                 join c in productParadigms on b.ProjectProductId equals c.Id
                //                                 where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
                //                                 select b.Id).Count();

                //// Tổng số lượng công việc cần làm 
                //// Số lượng công việc hoàn thành theo module 
                //item.TotalModuleFinish = (from b in listPlanDesign
                //                          join c in productModules on b.ProjectProductId equals c.Id
                //                          where b.Status == Constants.Plan_Status_Done
                //                          select b.Id).Count();

                //// Số lượng công việc chưa(đang) hoàn thành theo module
                //item.TotalModuleMakeDesign = (from b in listPlanDesign
                //                              join c in productModules on b.ProjectProductId equals c.Id
                //                              where b.Status != Constants.Plan_Status_Done
                //                              select b.Id).Count();

                //// Số lượng công việc hoàn thành theo mô hình
                //item.TotalParadigmFinish = (from b in listPlanDesign
                //                            join c in productParadigms on b.ProjectProductId equals c.Id
                //                            where b.Status == Constants.Plan_Status_Done
                //                            select b.Id).Count();

                //// số lượng công việc chưa(đang) hoàn thành theo mô hình
                //item.TotalParadigmMakeDesign = (from b in listPlanDesign
                //                                join c in productParadigms on b.ProjectProductId equals c.Id
                //                                where b.Status != Constants.Plan_Status_Done
                //                                select b.Id).Count();

                //// Tổng số lượng công việc bị chậm so với deadline sản phẩm

                //// Số lượng công việc chậm theo module (So ngày kết thúc dự kiến với hạn hoàn thành thiết kế)
                //item.TotalModuleDelay = (from b in listPlanDesign
                //                         join c in productModules on b.ProjectProductId equals c.Id
                //                         where b.EndDate > c.DesignFinishDate
                //                         select b.Id).Count();

                //// Số lượng công việc chậm theo mô hình
                //item.TotalParadigmDelay = (from b in listPlanDesign
                //                           join c in productParadigms on b.ProjectProductId equals c.Id
                //                           where b.EndDate > c.DesignFinishDate
                //                           select b.Id).Count();

                //// Công việc đến deadline kickoff nhưng chưa hoàn thành của thiết kế
                //item.Total_Task_Design_Delay = (from b in listPlanDesign
                //                                join c in productInProjects on b.ProjectProductId equals c.Id
                //                                where DateTime.Now > b.ExpectedDesignFinishDate && b.Status != Constants.Plan_Status_Done && !string.IsNullOrEmpty(b.ResponsiblePersion)
                //                                select b.Id).Count();

                //// Số ngày chậm lớn nhất ( Thiết kế)
                //var delays = (from b in listPlanDesign.AsEnumerable()
                //              join c in productInProjects.AsEnumerable() on b.ProjectProductId equals c.Id
                //              where b.EndDate.HasValue && c.ExpectedDesignFinishDate.HasValue && b.EndDate > c.ExpectedDesignFinishDate
                //              select new
                //              {
                //                  b.EndDate,
                //                  c.ExpectedDesignFinishDate
                //              }).ToList();

                //int delayMax = 0;
                //int day = 0;
                //foreach (var delay in delays)
                //{
                //    day = (delay.EndDate - delay.ExpectedDesignFinishDate).Value.Days;
                //    if (day > delayMax)
                //    {
                //        delayMax = day;
                //    }
                //}

                //item.TotalDelayDay_Design_Max = delayMax;

                #endregion

                #region Báo cáo tổng thể các dự án về mặt tài liệu
                // % hoàn thành tài liệu
                //if (listPlanDoc.Count > 0)
                //{
                //    totalPlanInProjectFinish = listPlanDoc.Where(b => b.Status == Constants.Plan_Status_Done).Count();
                //    item.PercentDoc = Math.Round((decimal)(totalPlanInProjectFinish / (decimal)listPlanDoc.Count) * 100, 2);
                //}
                //else
                //{
                //    item.PercentDoc = 0;
                //}

                //// Tổng sô tài liệu thiết kê mới (Tài liệu)
                //planDocs = (from b in listPlanDoc
                //            join c in productInProjects on b.ProjectProductId equals c.Id
                //            where c.DesignStatus == (Constants.ProjectProduct_DesignStatus_NewDesign)
                //            select b).ToList();

                //item.TotalDocDesign = planDocs.GroupBy(g => g.ProjectProductId).Count() - planDocs.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số tài liệu sửa  (Tài liệu)
                //planDocs = (from b in listPlanDoc
                //            join c in productInProjects on b.ProjectProductId equals c.Id
                //            where c.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign
                //            select b).ToList();

                //item.TotalDocUpdateDesign = planDocs.GroupBy(g => g.ProjectProductId).Count() - planDocs.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                //// Tổng số tài liệu tận dụng có kế hoạch(Tài liệu)
                //planDocs = (from b in listPlanDoc
                //            join c in productInProjects on b.ProjectProductId equals c.Id
                //            where c.DesignStatus == (Constants.ProjectProduct_DesignStatus_Use)
                //            select b).ToList();

                //// tổng số tài liệu tận dụng 
                //item.TotalParadigmUse = planDocs.GroupBy(g => g.ProjectProductId).Count() - planDocs.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();
                //item.TotalParadigmUse += productInProjects.Where(r => r.DesignStatus == (Constants.ProjectProduct_DesignStatus_Use)).Count() - planDocs.GroupBy(g => g.ProjectProductId).Count();


                //// Số lượng công việc chưa có kế hoạch (Tài liệu)
                //// số lượng công việc chưa có kế hoạch theo module (Tài liệu)
                //item.TotalModuleTaskNotPlan_Doc = (from b in listPlanDoc
                //                                   join c in productModules on b.ProjectProductId equals c.Id
                //                                   where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                                   select b.Id).Count();

                //// số lượng công việc chưa có kế hoạch theo mô hình (Tài liệu)
                //item.TotalParadigmTaskNotPlan_Doc = (from b in listPlanDoc
                //                                     join c in productParadigms on b.ProjectProductId equals c.Id
                //                                     where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
                //                                     select b.Id).Count();

                //// Tổng số lượng công việc cần làm:
                //// Số lượng công việc hoàn thành theo module (Tài liệu)
                //item.TotalModuleFinish_Doc = (from b in listPlanDoc
                //                              join c in productModules on b.ProjectProductId equals c.Id
                //                              where b.Status == Constants.Plan_Status_Done
                //                              select b.Id).Count();

                //// Số lượng công việc chưa(đang) hoàn thành theo module (Tài liệu)
                //item.TotalModuleMakeDesign_Doc = (from b in listPlanDoc
                //                                  join c in productModules on b.ProjectProductId equals c.Id
                //                                  where b.Status != Constants.Plan_Status_Done
                //                                  select b.Id).Count();

                //// số lượng công việc hoàn thành theo mô hình (Tài liệu)
                //item.TotalParadigmFinish_Doc = (from b in listPlanDoc
                //                                join c in productParadigms on b.ProjectProductId equals c.Id
                //                                where b.Status == Constants.Plan_Status_Done
                //                                select b.Id).Count();

                //// số lượng công việc chưa(đang) hoàn thành theo mô hình (Tài liệu)
                //item.TotalParadigmMakeDesign_Doc = (from b in listPlanDoc
                //                                    join c in productParadigms on b.ProjectProductId equals c.Id
                //                                    where b.Status != Constants.Plan_Status_Done
                //                                    select b.Id).Count();

                //// Tổng số lượng công việc bị châm so với lịch kích off của dự án
                //// Số lượng công việc chậm theo module
                //item.TotalModuleDelay_Doc = (from b in listPlanDoc
                //                             join c in productModules on b.ProjectProductId equals c.Id
                //                             where b.EndDate > c.MakeFinishDate
                //                             select b.Id).Count();

                //// Số lượng công việc chậm theo mô hình
                //item.TotalParadigmDelay_Doc = (from b in listPlanDoc
                //                               join c in productParadigms on b.ProjectProductId equals c.Id
                //                               where b.EndDate > c.MakeFinishDate
                //                               select b.Id).Count();

                //// Công việc đến deadline kickoff nhưng chưa hoàn thành của tài liệu
                //item.Total_Task_Doc_Delay = (from b in listPlanDoc
                //                             join c in productInProjects on b.ProjectProductId equals c.Id
                //                             where DateTime.Now > b.ExpectedMakeFinishDate && b.Status != Constants.Plan_Status_Done && !string.IsNullOrEmpty(b.ResponsiblePersion)
                //                             select b.Id).Count();

                //// số ngày chậm lớn nhất ( Tài liệu)
                //delays = (from b in listPlanDoc
                //          join c in productInProjects on b.ProjectProductId equals c.Id
                //          where b.EndDate.HasValue && c.ExpectedDesignFinishDate.HasValue && b.EndDate > c.ExpectedDesignFinishDate
                //          select new
                //          {
                //              b.EndDate,
                //              c.ExpectedDesignFinishDate
                //          }).ToList();

                //delayMax = 0;
                //day = 0;
                //foreach (var delay in delays)
                //{
                //    day = (delay.EndDate - delay.ExpectedDesignFinishDate).Value.Days;
                //    if (day > delayMax)
                //    {
                //        delayMax = day;
                //    }
                //}

                //item.TotalDelayDay_Doc_Max = delayMax;
                #endregion

                #region Báo cáo tổng thế các dự án về mặt chuyển giao
                // % của tổng số dự án về mặt chuyển giao
                //if (listPlanTransfer.Count > 0)
                //{
                //    totalPlanInProjectFinish = listPlanTransfer.Where(b => b.Status == Constants.Plan_Status_Done).Count();
                //    item.PercentTransfer = Math.Round(((decimal)totalPlanInProjectFinish / (decimal)listPlanTransfer.Count) * 100, 2);
                //}
                //else
                //{
                //    item.PercentTransfer = 0;
                //}

                //// Số lượng công việc chưa có kế hoạch
                //item.TotalProjectIsNotPlan_Transfer = (from b in listPlanTransfer
                //                                       join c in productInProjects on b.ProjectProductId equals c.Id
                //                                       where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null) && Constants.Plan_Status_Done != b.Status
                //                                       select b.Id).Count();

                //// công việc đến deadline kickoff nhưng chưa hoàn thành của chuyển giao
                //item.Total_Task_Transfer_Delay = (from b in listPlanTransfer
                //                                  join c in productInProjects on b.ProjectProductId equals c.Id
                //                                  where DateTime.Now > b.ExpectedTransferDate && b.Status != Constants.Plan_Status_Done && !string.IsNullOrEmpty(b.ResponsiblePersion)
                //                                  select b.Id).Count();
                //// Số lượng công việc có kế hoạch  thỏa mãn deadline trong kế hoạch
                //item.TotalTaskInPlanSatisfy = (from b in listPlanDesign
                //                               where b.Plan ==1 && b.KickOff ==1
                //                               select b.Id).Count();
                //// Số lượng công việc có kế hoạch  thỏa mãn deadline trễ kế hoạch
                //item.TotalTaskOutPlanSatisfy = (from b in listPlanDesign
                //                                where b.KickOff == 1 && b.Plan == 2
                //                                select b.Id).Count();
                //// Số lượng công việc có kế hoạch không thỏa mãn deadline trong kế hoạch
                //item.TotalTaskInPlanNotSatisfy = (from b in listPlanDesign
                //                                  where  b.KickOff == 2 && b.Plan == 1
                //                                  select b.Id).Count();
                //// Số lượng công việc có kế hoạch không  thỏa mãn deadline trễ kế hoạch
                //item.TotalTaskOutPlanNotSatisfy = (from b in listPlanDesign
                //                                   where b.KickOff == 2 && b.Plan == 2
                //                                   select b.Id).Count();
                //// Số lượng công việc có kế hoạch không  thỏa mãn deadline trễ kế hoạch <= 3day
                //item.TotalTaskOutPlanNotSatisfyLessThanThreeDay = (from b in listPlanDesign
                //                                                   where  b.KickOff == 2 && b.EndDate != null && b.ExpectedDesignFinishDate != null && ((DateTime)(b.EndDate) - (DateTime)(b.ExpectedDesignFinishDate)).Days <= 3
                //                                                   select b.Id).Count();
                //// Số lượng công việc có kế hoạch không  thỏa mãn deadline trễ kế hoạch 3 - 7day
                //item.TotalTaskOutPlanNotSatisfyThreeToSevenDay = (from b in listPlanDesign
                //                                                  where  b.KickOff == 2 && b.EndDate != null && b.ExpectedDesignFinishDate != null && ((DateTime)(b.EndDate) - (DateTime)(b.ExpectedDesignFinishDate)).Days > 3 
                //                                                  && ((DateTime)(b.EndDate) - (DateTime)(b.ExpectedDesignFinishDate)).Days <= 7
                //                                                  select b.Id).Count();
                //// Số lượng công việc có kế hoạch không  thỏa mãn deadline trễ kế hoạch > 7
                //item.TotalTaskOutPlanNotSatisfyGreaterThanSevenDay =
                //                                   (from b in listPlanDesign  
                //                                    where  b.KickOff == 2 && b.EndDate != null && b.ExpectedDesignFinishDate != null && ((DateTime)(b.EndDate) - (DateTime)(b.ExpectedDesignFinishDate)).Days > 7 && b.RealEndDate != null
                //                                    select b.Id).Count();
                #endregion
            }


            return result;
        }

        #region view detail design
        public object ViewDetailDesign(string projectId, int value)
        {
            object List_Design = null;


            //var listPlan = (from a in db.Plans.AsNoTracking().Where(r => string.IsNullOrEmpty(r.ReferenceId))
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in db.Projects.AsNoTracking().Where(r => r.Id.Equals(projectId)) on d.ProjectId equals b.Id
            //                join c in db.Tasks.AsNoTracking().Where(r => r.Type == Constants.Task_Design) on a.TaskId equals c.Id
            //                join e in db.Employees.AsNoTracking() on a.ResponsiblePersion equals e.Id into ae
            //                from ea in ae.DefaultIfEmpty()
            //                join f in db.Users.AsNoTracking() on a.CreateBy equals f.Id into af
            //                from fa in af.DefaultIfEmpty()
            //                join g in db.Employees.AsNoTracking() on fa.EmployeeId equals g.Id into ag
            //                from ga in ag.DefaultIfEmpty()
            //                join k in db.Modules.AsNoTracking() on d.ModuleId equals k.Id into dk
            //                from kd in dk.DefaultIfEmpty()
            //                join m in db.Products.AsNoTracking() on d.ProductId equals m.Id into md
            //                from dm in md.DefaultIfEmpty()
            //                join i in db.ModuleGroups.AsNoTracking() on kd.ModuleGroupId equals i.Id into kdi
            //                from idk in kdi.DefaultIfEmpty()
            //                join q in db.Industries.AsNoTracking() on idk.IndustryId equals q.Id into qidk
            //                from idkq in qidk.DefaultIfEmpty()
            //                orderby kd.Code
            //                select new
            //                {
            //                    Name = c != null ? c.Name : a.Name, // Kế hoạch
            //                    ResponsiblePersion = ea != null ? ea.Name : string.Empty, // Người phụ trách
            //                    CreateByName = ga != null ? ga.Name : string.Empty, // Người tạo
            //                    a.EsimateTime, //  Thời gian thực hiện (h)
            //                    a.RealStartDate, // Thời gian bđ thực tế
            //                    a.RealEndDate, // Thời gian kết thúc thực tế
            //                    a.StartDate, // Ngày bắt đầu dự kiến 
            //                    a.EndDate, // Ngày kết thúc dự kiến 
            //                    a.Status,// Tình trạng công việc
            //                    a.ExecutionTime, // thời gian thực tế
            //                    d.ContractCode, // Mã theo hợp đồng
            //                    d.ContractName, // Tên theo hợp đồng
            //                    DesignCode = kd != null ? kd.Code : dm != null ? dm.Code : string.Empty, // Mã theo thiết kế 
            //                    DesignName = kd != null ? kd.Name : dm != null ? dm.Name : string.Empty, // Tên theo thiết kế
            //                    IndustryCode = idkq != null ? idkq.Code : string.Empty,  // mã nghành hàng
            //                    d.Quantity, // số lượng
            //                    PricingModule = kd != null ? kd.Pricing : 0, // Giá tổng hợp thiết kế
            //                    DataType = d.DataType == Constants.ProjectProduct_DataType_Module ? "Module" : (d.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình" : (d.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : "Sản phẩm")), // Kiểu dữ liệu
            //                    ModuleStatusView = d.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung", // Tình trạng bổ sung ngoài hợp đồng
            //                    DesignStatusView = d.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : (d.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : "Tận dụng"), // TÌnh trạng thiết kế
            //                    b.KickOffDate, // ngày kick offf
            //                    d.ExpectedDesignFinishDate,
            //                    d.ExpectedMakeFinishDate,
            //                    d.ExpectedTransferDate,
            //                    a.Id, // planId
            //                    TaskId = c.Id,
            //                    TaskName = c.Name,
            //                    a.ProjectId,
            //                    ProjectName = b.Name,
            //                    ProjectCode = b.Code,
            //                    a.ProjectProductId,
            //                    c.Type,
            //                }).ToList();

            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ProjectId.Equals(projectId)
                                      select a).ToList();
            //switch (value)
            //{
            //    case Constants.ViewDesign_Module_TaskNotPlan:
            //        {
            //            // Công việc chưa có kế hoạch theo module 
            //            var module_TaskNotPlan = (from b in listPlan
            //                                      join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                      where c.DataType == (Constants.ProjectProduct_DataType_Module) &&
            //                                      (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
            //                                      && Constants.Plan_Status_Done != b.Status
            //                                      select b).ToList();
            //            List_Design = module_TaskNotPlan;
            //            break;
            //        }
            //    case Constants.ViewDesign_Paradigm_TaskNotPlan:
            //        {
            //            // Công việc chưa có kế hoạch theo mô hình
            //            var paradigm_TaskNotPlan = (from b in listPlan
            //                                        join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                        where c.DataType == (Constants.ProjectProduct_DataType_Paradigm) &&
            //                                        (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
            //                                        && Constants.Plan_Status_Done != b.Status
            //                                        select b).ToList();
            //            List_Design = paradigm_TaskNotPlan;
            //            break;
            //        }
            //    case Constants.ViewDesign_Module_finish:
            //        {
            //            // Công việc hoàn thành theo module 
            //            var module_finish = (from b in listPlan
            //                                 join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                 where b.Status == Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                 select b).ToList();
            //            List_Design = module_finish;
            //            break;
            //        }
            //    case Constants.ViewDesign_Module_makeDesign:
            //        {
            //            // Công việc chưa(đang) hoàn thành theo module
            //            var module_makeDesign = (from b in listPlan
            //                                     join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                     where b.Status != Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                     select b).ToList();
            //            List_Design = module_makeDesign;
            //            break;
            //        }
            //    case Constants.ViewDesign_Paradigm_finish:
            //        {
            //            // Công việc hoàn thành theo mô hình
            //            var paradigm_finish = (from b in listPlan
            //                                   join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                   where b.Status == Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                   select b).ToList();
            //            List_Design = paradigm_finish;
            //            break;
            //        }
            //    case Constants.ViewDesign_Paradigm_makeDesign:
            //        {
            //            // Công việc chưa(đang) hoàn thành theo mô hình
            //            var paradigm_makeDesign = (from b in listPlan
            //                                       join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                       where b.Status != Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                       select b).ToList();
            //            List_Design = paradigm_makeDesign;
            //            break;
            //        }
            //    case Constants.ViewDesign_Module_delay:
            //        {
            //            // tổng số lượng công việc bị chậm so với deadline sản phẩm

            //            // Công việc chậm theo module (So ngày kết thúc dự kiến với hạn hoàn thành thiết kế)
            //            var module_delay = (from b in listPlan
            //                                join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                where c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                 && b.EndDate > c.DesignFinishDate
            //                                select b).ToList();
            //            List_Design = module_delay;
            //            break;
            //        }
            //    case Constants.ViewDesign_Paradigm_delay:
            //        {
            //            // Công việc chậm theo mô hình
            //            var paradigm_delay = (from b in listPlan
            //                                  join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                  where b.EndDate > c.DesignFinishDate && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                  select b).ToList();
            //            List_Design = paradigm_delay;
            //            break;
            //        }
            //    case Constants.ViewDesign_Task_Design_delay:
            //        {
            //            // công việc đến deadline kickoff nhưng chưa hoàn thành của thiết kế
            //            var task_Design_delay = (from b in listPlan
            //                                     join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                     where DateTime.Now > b.ExpectedDesignFinishDate && b.Status != Constants.Plan_Status_Done && !string.IsNullOrEmpty(b.ResponsiblePersion)
            //                                     select b).ToList();
            //            List_Design = task_Design_delay;
            //            break;
            //        }
            //    default:
            //        {
            //            break;
            //        }

            //}

            return List_Design;
        }
        #endregion

        #region view detail document
        public object ViewDetailDocument(string projectId, int value)
        {
            object List_Document = null;

            //var listPlan = (from a in db.Plans.AsNoTracking().Where(r => string.IsNullOrEmpty(r.ReferenceId))
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in db.Projects.AsNoTracking().Where(r => r.Id.Equals(projectId)) on d.ProjectId equals b.Id
            //                join c in db.Tasks.AsNoTracking().Where(r => r.Type == Constants.Task_Doc) on a.TaskId equals c.Id
            //                join e in db.Employees.AsNoTracking() on a.ResponsiblePersion equals e.Id into ae
            //                from ea in ae.DefaultIfEmpty()
            //                join f in db.Users.AsNoTracking() on a.CreateBy equals f.Id into af
            //                from fa in af.DefaultIfEmpty()
            //                join g in db.Employees.AsNoTracking() on fa.EmployeeId equals g.Id into ag
            //                from ga in ag.DefaultIfEmpty()
            //                join k in db.Modules.AsNoTracking() on d.ModuleId equals k.Id into dk
            //                from kd in dk.DefaultIfEmpty()
            //                join m in db.Products.AsNoTracking() on d.ProductId equals m.Id into md
            //                from dm in md.DefaultIfEmpty()
            //                join i in db.ModuleGroups.AsNoTracking() on kd.ModuleGroupId equals i.Id into kdi
            //                from idk in kdi.DefaultIfEmpty()
            //                join q in db.Industries.AsNoTracking() on idk.IndustryId equals q.Id into qidk
            //                from idkq in qidk.DefaultIfEmpty()
            //                orderby kd.Code
            //                select new
            //                {
            //                    Name = c != null ? c.Name : a.Name, // Kế hoạch
            //                    ResponsiblePersion = ea != null ? ea.Name : string.Empty, // Người phụ trách
            //                    CreateByName = ga != null ? ga.Name : string.Empty, // Người tạo
            //                    a.EsimateTime, //  Thời gian thực hiện (h)
            //                    a.RealStartDate, // Thời gian bđ thực tế
            //                    a.RealEndDate, // Thời gian kết thúc thực tế
            //                    a.StartDate, // Ngày bắt đầu dự kiến 
            //                    a.EndDate, // Ngày kết thúc dự kiến 
            //                    a.Status,// Tình trạng công việc
            //                    a.ExecutionTime, // thời gian thực tế
            //                    d.ContractCode, // Mã theo hợp đồng
            //                    d.ContractName, // Tên theo hợp đồng
            //                    DesignCode = kd != null ? kd.Code : dm != null ? dm.Code : string.Empty, // Mã theo thiết kế 
            //                    DesignName = kd != null ? kd.Name : dm != null ? dm.Name : string.Empty, // Tên theo thiết kế
            //                    IndustryCode = idkq != null ? idkq.Code : string.Empty,  // mã nghành hàng
            //                    d.Quantity, // số lượng
            //                    PricingModule = kd != null ? kd.Pricing : 0, // Giá tổng hợp thiết kế
            //                    DataType = d.DataType == Constants.ProjectProduct_DataType_Module ? "Module" : (d.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình" : (d.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : "Sản phẩm")), // Kiểu dữ liệu
            //                    ModuleStatusView = d.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung", // Tình trạng bổ sung ngoài hợp đồng
            //                    DesignStatusView = d.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : (d.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : "Tận dụng"), // TÌnh trạng thiết kế
            //                    b.KickOffDate, // ngày kick offf


            //                    d.ExpectedDesignFinishDate,
            //                    d.ExpectedMakeFinishDate,
            //                    d.ExpectedTransferDate,

            //                    a.Id, // planId
            //                    TaskId = c.Id,
            //                    TaskName = c.Name,
            //                    a.ProjectId,
            //                    ProjectName = b.Name,
            //                    ProjectCode = b.Code,
            //                    a.ProjectProductId,
            //                    c.Type,

            //                }).ToList();

            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ProjectId.Equals(projectId)
                                      select a).ToList();
            //switch (value)
            //{
            //    case Constants.ViewDocumnet_Module_TaskNotPlan_Doc:
            //        {
            //            // Công việc chưa có kế hoạch - Module
            //            var module_TaskNotPlan_Doc = (from b in listPlan
            //                                          join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                          where c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                          && (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
            //                                            && Constants.Plan_Status_Done != b.Status
            //                                          select b).ToList();
            //            List_Document = module_TaskNotPlan_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Paradigm_TaskNotPlan_Doc:
            //        {
            //            // Công việc chư có kế hoạch - Mô hình
            //            var paradigm_TaskNotPlan_Doc = (from b in listPlan
            //                                            join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                            where c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                            && (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
            //                                             && Constants.Plan_Status_Done != b.Status
            //                                            select b).ToList();
            //            List_Document = paradigm_TaskNotPlan_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Module_finish_Doc:
            //        {
            //            // Tổng số lượng công việc cần làm:
            //            // Công việc hoàn thành theo module (Tài liệu)
            //            var module_finish_Doc = (from b in listPlan
            //                                     join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                     where b.Status == Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                     select b).ToList();
            //            List_Document = module_finish_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Module_makeDesign_Doc:
            //        {
            //            // Công việc chưa(đang) hoàn thành theo module (Tài liệu)
            //            var module_makeDesign_Doc = (from b in listPlan
            //                                         join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                         where b.Status != Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                         select b).ToList();
            //            List_Document = module_makeDesign_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Paradigm_finish_Doc:
            //        {
            //            // Công việc hoàn thành theo mô hình (Tài liệu)
            //            var paradigm_finish_Doc = (from b in listPlan
            //                                       join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                       where b.Status == Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                       select b).ToList();
            //            List_Document = paradigm_finish_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Paradigm_makeDesign_Doc:
            //        {
            //            // Công việc chưa(đang) hoàn thành theo mô hình (Tài liệu)
            //            var paradigm_makeDesign_Doc = (from b in listPlan
            //                                           join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                           where b.Status != Constants.Plan_Status_Done && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                           select b).ToList();
            //            List_Document = paradigm_makeDesign_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Module_delay_Doc:
            //        {
            //            // Tổng số lượng công việc bị châm so với lịch kích off của dự án
            //            // công việc chậm theo module
            //            var module_delay_Doc = (from b in listPlan
            //                                    join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                    where b.EndDate > c.MakeFinishDate && c.DataType == (Constants.ProjectProduct_DataType_Module)
            //                                    select b).ToList();
            //            List_Document = module_delay_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Paradigm_delay_Doc:
            //        {
            //            // Số lượng công việc chậm theo mô hình
            //            var paradigm_delay_Doc = (from b in listPlan
            //                                      join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                      where b.EndDate > c.MakeFinishDate && c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                      select b).ToList();
            //            List_Document = paradigm_delay_Doc;
            //            break;
            //        }
            //    case Constants.ViewDocumnet_Total_task_Doc_delay:
            //        {
            //            // công việc đến deadline kickoff nhưng chưa hoàn thành của tài liệu
            //            var total_task_Doc_delay = (from b in listPlan
            //                                        join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                        where DateTime.Now > b.ExpectedMakeFinishDate && b.Status != Constants.Plan_Status_Done
            //                                        && !string.IsNullOrEmpty(b.ResponsiblePersion)
            //                                        select b).ToList();
            //            List_Document = total_task_Doc_delay;
            //            break;
            //        }
            //    default:
            //        {
            //            break;
            //        }

            //}

            return List_Document;
        }
        #endregion

        #region view detail transfer
        public object ViewDetailTransfer(string projectId, int value)
        {
            object List_Transfer = null;


            //var listPlan = (from a in db.Plans.AsNoTracking().Where(r => string.IsNullOrEmpty(r.ReferenceId))
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in db.Projects.AsNoTracking().Where(r => r.Id.Equals(projectId)) on d.ProjectId equals b.Id
            //                join c in db.Tasks.AsNoTracking().Where(r => r.Type == Constants.Task_Transfer) on a.TaskId equals c.Id
            //                join e in db.Employees.AsNoTracking() on a.ResponsiblePersion equals e.Id into ae
            //                from ea in ae.DefaultIfEmpty()
            //                join f in db.Users.AsNoTracking() on a.CreateBy equals f.Id into af
            //                from fa in af.DefaultIfEmpty()
            //                join g in db.Employees.AsNoTracking().AsNoTracking() on fa.EmployeeId equals g.Id into ag
            //                from ga in ag.DefaultIfEmpty()
            //                join k in db.Modules.AsNoTracking() on d.ModuleId equals k.Id into dk
            //                from kd in dk.DefaultIfEmpty()
            //                join m in db.Products.AsNoTracking() on d.ProductId equals m.Id into md
            //                from dm in md.DefaultIfEmpty()
            //                join i in db.ModuleGroups.AsNoTracking() on kd.ModuleGroupId equals i.Id into kdi
            //                from idk in kdi.DefaultIfEmpty()
            //                join q in db.Industries.AsNoTracking() on idk.IndustryId equals q.Id into qidk
            //                from idkq in qidk.DefaultIfEmpty()
            //                orderby kd.Code
            //                select new
            //                {
            //                    Name = c != null ? c.Name : a.Name, // Kế hoạch
            //                    ResponsiblePersion = ea != null ? ea.Name : string.Empty, // Người phụ trách
            //                    CreateByName = ga != null ? ga.Name : string.Empty, // Người tạo
            //                    a.EsimateTime, //  Thời gian thực hiện (h)
            //                    a.RealStartDate, // Thời gian bđ thực tế
            //                    a.RealEndDate, // Thời gian kết thúc thực tế
            //                    a.StartDate, // Ngày bắt đầu dự kiến 
            //                    a.EndDate, // Ngày kết thúc dự kiến 
            //                    a.Status,// Tình trạng công việc
            //                    a.ExecutionTime, // thời gian thực tế
            //                    d.ContractCode, // Mã theo hợp đồng
            //                    d.ContractName, // Tên theo hợp đồng
            //                    DesignCode = kd != null ? kd.Code : dm != null ? dm.Code : string.Empty, // Mã theo thiết kế 
            //                    DesignName = kd != null ? kd.Name : dm != null ? dm.Name : string.Empty, // Tên theo thiết kế
            //                    IndustryCode = idkq != null ? idkq.Code : string.Empty,  // mã nghành hàng
            //                    d.Quantity, // số lượng
            //                    PricingModule = kd != null ? kd.Pricing : 0, // Giá tổng hợp thiết kế
            //                    DataType = d.DataType == Constants.ProjectProduct_DataType_Module ? "Module" : (d.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình" : (d.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : "Sản phẩm")), // Kiểu dữ liệu
            //                    ModuleStatusView = d.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung", // Tình trạng bổ sung ngoài hợp đồng
            //                    DesignStatusView = d.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : (d.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : "Tận dụng"), // TÌnh trạng thiết kế
            //                    b.KickOffDate, // ngày kick offf


            //                    d.ExpectedDesignFinishDate,
            //                    d.ExpectedMakeFinishDate,
            //                    d.ExpectedTransferDate,

            //                    a.Id, // planId
            //                    TaskId = c.Id,
            //                    TaskName = c.Name,
            //                    a.ProjectId,
            //                    ProjectName = b.Name,
            //                    ProjectCode = b.Code,
            //                    a.ProjectProductId,
            //                    c.Type,

            //                }).ToList();



            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ProjectId.Equals(projectId)
                                      select a).ToList();
            //switch (value)
            //{
            //    case Constants.ViewTranfer_ProjectIsNotPlan_Transfer:
            //        {
            //            // Công việc chưa có kế hoạch
            //            var projectIsNotPlan_Transfer = (from b in listPlan
            //                                             join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                             where c.DataType == (Constants.ProjectProduct_DataType_Paradigm)
            //                                             && (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
            //                                             && Constants.Plan_Status_Done != b.Status
            //                                             select b).ToList();
            //            List_Transfer = projectIsNotPlan_Transfer;
            //            break;
            //        }
            //    case Constants.ViewTranfer_Task_Transfer_delay:
            //        {
            //            // công việc đến deadline kickoff nhưng chưa hoàn thành của chuyển giao
            //            var task_Transfer_delay = (from b in listPlan
            //                                       join c in listProjectProduct on b.ProjectProductId equals c.Id
            //                                       where DateTime.Now > b.ExpectedTransferDate && b.Status != Constants.Plan_Status_Done && !string.IsNullOrEmpty(b.ResponsiblePersion)
            //                                       select b).ToList();
            //            List_Transfer = task_Transfer_delay;
            //            break;
            //        }
            //    default:
            //        {
            //            break;
            //        }

            //}

            return List_Transfer;
        }
        #endregion
    }
}
