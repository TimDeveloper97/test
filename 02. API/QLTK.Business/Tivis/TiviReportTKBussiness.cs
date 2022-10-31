using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DashBroadProject;
using NTS.Model.Project;
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
    public class TiviReportTKBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public object GetDashBroadProject(DashBroadProjectSearchModel model)
        {
            DashBroadProjectResultModel result = new DashBroadProjectResultModel();
            // ListProject
            var listProject = db.Projects.Where(r=>r.Status == Constants.Project_Status_Finish).OrderBy(o => o.Code).AsQueryable();
            #region đk tìm kiếm

            if (!string.IsNullOrEmpty(model.SBUId))
            {
                listProject = listProject.Where(a => a.SBUId.Equals(model.SBUId));
            }

            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                listProject = listProject.Where(a => a.DepartmentId.Equals(model.DepartmentId));
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
            //if (!string.IsNullOrEmpty(model.ProjCode))
            //{
            //    listProject = listProject.Where(a => a.Name.ToLower().Contains(model.ProjCode.ToLower()));
            //}
            #endregion

            #region Các list xử lý
            // ListPlan
            //var listPlan = (from a in db.Plans.AsNoTracking()
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in listProject on d.ProjectId equals b.Id
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
            //                    ExpectedTransferDate = d.ExpectedTransferDate
            //                }).ToList();

            // ListProjectProduct
            var listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      join b in listProject on a.ProjectId equals b.Id
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
                                          MakeFinishDate = a.MakeFinishDate
                                      }).ToList();

          var listProjectInPlan = listProject.Select(gr => new DashBroadProjectModel() { ProjectId = gr.Id, ProjectCode = gr.Code, ProjectName = gr.Name }).ToList();

            #endregion

            List<DashBroadProjectModel> ObjectReporDesignProject = new List<DashBroadProjectModel>();
            List<DashBoardProjectPlanModel> planInProjects;
            List<DashBoardProjectPlanModel> listPlanDesign;
            List<DashBoardProjectPlanModel> planModules;
            List<DashBoardProjecProductModel> productInProjects;
            List<DashBoardProjecProductModel> productModules;
            List<DashBoardProjecProductModel> productParadigms;
            int totalPlanInProjectFinish;
            result.ListProjectInPlan = new List<DashBroadProjectModel>();
            foreach (var item in listProjectInPlan)
            {
                //planInProjects = listPlan.Where(r => item.ProjectId.Equals(r.ProjectId)).ToList();
                productInProjects = listProjectProduct.Where(r => r.ProjectId.Equals(item.ProjectId)).ToList();

                //listPlanDesign = planInProjects.Where(r => r.Type == Constants.Task_Design).ToList();
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

                // Sản phẩm là module
                productModules = productInProjects.Where(r => r.DataType == Constants.ProjectProduct_DataType_Module).ToList();

                // Tổng sô module thiết kê mới
                //planModules = (from a in listPlanDesign
                //               join c in productModules on a.ProjectProductId equals c.Id
                //               where c.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign
                //               select a).ToList();
                //item.TotalModuleDesign = planModules.GroupBy(g => g.ProjectProductId).Count() - planModules.Where(r => r.Status != Constants.Plan_Status_Done).GroupBy(g => g.ProjectProductId).Count();

                // Tổng số module sửa thiết kế cũ 
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

                // Số lượng công việc chưa có kế hoạch theo mô hình
                //item.TotalParadigmTaskNotPlan = (from b in listPlanDesign
                //                                 join c in productParadigms on b.ProjectProductId equals c.Id
                //                                 where (string.IsNullOrEmpty(b.ResponsiblePersion) || b.StartDate == null || b.EndDate == null)
                //                                 select b.Id).Count();

                // Tổng số lượng công việc cần làm 
                // Số lượng công việc hoàn thành theo module 
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
                ////item.TotalParadigmFinish = (from b in listPlanDesign
                ////                            join c in productParadigms on b.ProjectProductId equals c.Id
                ////                            where b.Status == Constants.Plan_Status_Done
                ////                            select b.Id).Count();

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

                if (item.TotalModuleTaskNotPlan>0 || item.TotalParadigmTaskNotPlan > 0 || item.TotalModuleMakeDesign > 0 || item.TotalParadigmMakeDesign > 0 || item.TotalModuleDelay > 0 || item.TotalParadigmDelay > 0 || item.Total_Task_Design_Delay > 0 || item.TotalDelayDay_Design_Max > 0)
                {
                    result.ListProjectInPlan.Add(item);
                }

            }


            return result;
        }

    }
}
