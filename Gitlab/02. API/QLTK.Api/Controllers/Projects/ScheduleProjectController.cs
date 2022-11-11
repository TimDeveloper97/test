using NTS.Common;
using NTS.Model;
using NTS.Model.Plans;
using NTS.Model.Projects.Project;
using NTS.Model.ScheduleProject;
using NTS.Model.TaskTimeStandardModel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.ScheduleProject
{
    [RoutePrefix("api/ScheduleProject")]
    [ApiHandleExceptionSystem]
    public class ScheduleProjectController : BaseController
    {
        private readonly ScheduleProjectBusiness _scheduleProject = new ScheduleProjectBusiness();

        /// <summary>
        /// Add công đoạn cho sản phẩm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("stage/choose")]
        [HttpPost]
        public HttpResponseMessage ChooseStage(ProduceStageModel model)
        {
            var userId = GetUserIdByRequest();
            var result = _scheduleProject.ChooseStage(userId, model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("plan/create")]
        [HttpPost]
        public HttpResponseMessage CreatePlan(PlanModel model)
        {
            var result = _scheduleProject.CreatePlan(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("plan/update")]
        [HttpPost]
        public HttpResponseMessage UpdatePlan(PlanModel model)
        {
            var result = _scheduleProject.UpdatePlan(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("plan/delete")]
        [HttpPost]
        public HttpResponseMessage DeletePlan(PlanModel model)
        {
            _scheduleProject.DeletePlan(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("stage/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteStage(PlanModel model)
        {
            _scheduleProject.DeleteStage(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("plan/modify")]
        [HttpPost]
        public HttpResponseMessage ModifyPlan(List<ScheduleEntity> model)
        {
            _scheduleProject.ModifyPlan(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Tạm dừng công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("plan/pending")]
        [HttpPost]
        public HttpResponseMessage Pending(string planId = "")
        {
            _scheduleProject.Pending(planId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("plan/cancel")]
        [HttpPost]
        public HttpResponseMessage Cancel(string planId = "")
        {
            _scheduleProject.Cancel(planId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Khôi phục lại công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("plan/resume")]
        [HttpPost]
        public HttpResponseMessage Resume(string planId = "")
        {
            _scheduleProject.Resume(planId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("plan/delete/multiple")]
        [HttpPost]
        public HttpResponseMessage DeleteMultiPlan(PlanCopyCreateModel model)
        {
            _scheduleProject.DeleteMultiPlan(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListPlanByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetListPlanByProjectId(ScheduleProjectSearchModel modelSearch)
        {
            var result = _scheduleProject.GetListPlanByProjectId(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListPlanByMonth")]
        [HttpPost]
        public HttpResponseMessage GetListPlanByMonth(ScheduleProjectSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060705))
            {
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _scheduleProject.GetListPlanByMonth(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListProjectProductByProjectId")]
        [HttpPost]
        public HttpResponseMessage GetListProjectProductByProjectId(string projectId)
        {
            var result = _scheduleProject.GetListProjectProductByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetTaskTimeStandardByTaskId")]
        [HttpPost]
        public HttpResponseMessage GetTaskTimeStandardByTaskId(TaskTimeStandardSearchModel modelSearch)
        {

            var result = _scheduleProject.GetTaskTimeStandardByTaskId(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetStatisticalProject")]
        [HttpPost]
        public HttpResponseMessage GetStatisticalProject(ScheduleProjectSearchModel modelSearch)
        {
            modelSearch.DepartmentId = GetDepartmentIdByRequest();
            var result = _scheduleProject.GetStatisticalProject(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("ExportExcel")]
        [HttpPost]
     //   [NTSAuthorize(AllowFeature = "F060706")]
        public HttpResponseMessage ExportExcel(ScheduleProjectSearchModel modelSearch)
        {
            string sbuId = string.Empty;
            if (!this.CheckPermission(Constants.Permission_Code_F060705))
            {
                sbuId = this.GetSBUIdByRequest();
            }

            var result = _scheduleProject.ExportExcel(modelSearch, sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelProjectSchedule")]
        [HttpPost]
      //  [NTSAuthorize(AllowFeature = "F060706")]
        public HttpResponseMessage ExportExcelProjectSchedule(ScheduleProjectSearchModel modelSearch)
        {
            string sbuId = string.Empty;
            if (!this.CheckPermission(Constants.Permission_Code_F060705))
            {
                sbuId = this.GetSBUIdByRequest();
            }

            var result = _scheduleProject.ExportExcelProjectSchedule(modelSearch, sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchWorkingReport")]
        [HttpPost]
        public HttpResponseMessage SearchWorkingReport(WorkingReportModel modelSearch)
        {
            var result = _scheduleProject.SearchWorkingReport(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xuất báo cáo công việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("ExportExcelWorkingReport")]
        [HttpPost]
        public HttpResponseMessage ExportExcelWorkingReport(WorkingReportModel modelSearch)
        {

            var result = _scheduleProject.ExportExcelWorkingReport(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[Route("getVersionName")]
        //[HttpPost]
        //public HttpResponseMessage getVersionName()
        //{
        //    var result = _scheduleProject.getVersionName();
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        /// <summary>
        /// Lưu điều chỉnh kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("createPlanAdjustment")]
        [HttpPost]
        public HttpResponseMessage AddCustomer(PlanAdjustmentModel model)
        {
            //Thêm nhân sự ngoài thì vào đây xem nha
            model.CreateBy = GetUserIdByRequest();
            _scheduleProject.CreatePlanAdjustment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }



        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="listIdUser"></param>
        /// <returns></returns>
        [Route("GetListEmployee")]
        [HttpPost]
        public HttpResponseMessage GetListEmployee(List<string> listIdUser)
        {
            var result = _scheduleProject.GetListEmployee(listIdUser);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Danh sách người phụ trách
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [Route("GetListPlanAdjustment")]
        [HttpPost]
        public HttpResponseMessage GetListPlanAdjustment(string planId)
        {
            var result = _scheduleProject.GetListPlanAdjustment(planId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelProjectPlan")]
        [HttpPost]
        public HttpResponseMessage ExportExcelProjectPlan(ScheduleProjectSearchModel model)
        {
            var result = _scheduleProject.ExportExcelProjectPlan(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật nhân viên phụ trách
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdatePlanAssignment")]
        [HttpPost]
        public HttpResponseMessage UpdatePlanAdjustment(PlanAdjustmentCreateModel model)
        {
            _scheduleProject.UpdatePlanAdjustment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy dữ liệu copy kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetDataCopy")]
        [HttpPost]
        public HttpResponseMessage GetDataCopy(ScheduleProjectResultModel model)
        {
            var result = _scheduleProject.GetDataCopy(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Copy công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CreatePlanCopy")]
        [HttpPost]
        public HttpResponseMessage CreatePlanCopy(PlanCopyCreateModel model)
        {
            var returnvalue = _scheduleProject.CreatePlanCopy(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, returnvalue);
        }

        /// <summary>
        /// Copy công đoạn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CreateStageCopy")]
        [HttpPost]
        public HttpResponseMessage CreateStageCopy(PlanCopyCreateModel model)
        {
           var returnvalue = _scheduleProject.CreateStageCopy(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, returnvalue);
        }

        /// <summary>
        /// Lấy thông tin ngày nghỉ
        /// </summary>
        /// <returns></returns>
        [Route("GetListHoliday")]
        [HttpPost]
        public HttpResponseMessage GetListHoliday()
        {
            var result = _scheduleProject.GetListHoliday();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin ngày nghỉ
        /// </summary>
        /// <returns></returns>
        [Route("GetListHolidayGanttChart")]
        [HttpPost]
        public HttpResponseMessage GetListHolidayGanttChart()
        {
            var result = _scheduleProject.GetListHolidayGanttChart();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin người phụ trách
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [Route("get-plan-assignment")]
        [HttpPost]
        public HttpResponseMessage GetPlanAssignments(string planId)
        {
            var result = _scheduleProject.GetPlanAssignments(planId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thứ tự hiển thị
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [Route("GetListOrder")]
        [HttpPost]
        public HttpResponseMessage GetListOrder(string projectProductId, string stageId, int type)
        {
            var result = _scheduleProject.GetListOrder(projectProductId, stageId, type);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin lịch sử thay đổi
        /// </summary>
        /// <param name="projectProductId"></param>
        /// <param name="stageId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("GetPlanHistoryInfo")]
        [HttpPost]
        public HttpResponseMessage GetPlanHistoryInfo(string id)
        {
            var result = _scheduleProject.GetPlanHistoryInfo(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy thông tin lịch sử thay đổi
        /// </summary>
        /// <param name="projectProductId"></param>
        /// <param name="stageId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("GetListSuppliers")]
        [HttpPost]
        public HttpResponseMessage GetListSuppliers()
        {
            var result = _scheduleProject.GetListSuppliers();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        /// <summary>
        /// Lấy thông tin lịch sử thay đổi
        /// </summary>
        /// <param name="projectProductId"></param>
        /// <param name="stageId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("ReCorrectData")]
        [HttpPost]
        public HttpResponseMessage ReCorrectData()
        {
            _scheduleProject.ReCorrectData();
            return Request.CreateResponse(HttpStatusCode.OK, "Kết thúc xử lý");
        }


        [Route("GanttChart")]
        [HttpPost]
        public HttpResponseMessage GanttChart(ScheduleProjectSearchModel modelSearch)
        {
            var result = _scheduleProject.GanttChart(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
