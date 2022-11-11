using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Plans;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.WorkDiary;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Plan
{
    [RoutePrefix(prefix: "api/Plans")]
    [ApiHandleExceptionSystem]
    public class PlanController : BaseController
    {
        private readonly PlanBussiness _plan = new PlanBussiness();

        [Route("SearchPlans")]
        [HttpPost]
        public HttpResponseMessage SearchPlans(PlanSearchModel modelSearch)
        {
            var result = _plan.SearchPlans(modelSearch);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPlanView")]
        [HttpPost]
        public HttpResponseMessage GetPlanView(string id, int type)
        {
            if (type == 1)
            {
                var result = _plan.GetPlanView(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            if (type == 3)
            {
                var result = _plan.GetQuotationPlanView(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                var result = _plan.GetErrorFixView(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [Route("GetPlanInfo")]
        [HttpPost]
        public HttpResponseMessage GetPlanInfo(PlanModel model)
        {
            if (model.Types == 1)
            {
                var result = _plan.GetPlanInfo(model.Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            if (model.Types == 3)
            {
                var result = _plan.GetQuotationPlanInfo(model.Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                var result = _plan.GetErrorFixsInfo(model.Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        [Route("progress/update")]
        [HttpPost]
        public HttpResponseMessage UpdatePlan(PlanInfoModel model)
        {
            if (model.Type == 1)
            {
               var output =  _plan.UpdatePlan(model, GetUserIdByRequest());
                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            if (model.Type == 3)
            {
                _plan.UpdateQuotationPlan(model, GetUserIdByRequest());
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            else
            {
                _plan.UpdateErrorFix(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
        }

        [Route("ExportPlan")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060706")]
        public HttpResponseMessage ExportExcel(PlanSearchModel model)
        {
            try
            {
                string path = _plan.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SearchWorkingTime")]
        [HttpPost]
        //  [NTSAuthorize(AllowFeature = "F060800")]
        public HttpResponseMessage SearchWorkingTime(WorkingTimeSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060802))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _plan.SearchWorkingTime(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployee")]
        [HttpPost]
        //    [NTSAuthorize(AllowFeature = "F060701;F060702;F060704;F060705")]
        public HttpResponseMessage GetEmployee(string DepartmentId)
        {
            var result = _plan.GetEmployee(DepartmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProjectProductInfo")]
        [HttpPost]
        //   [NTSAuthorize(AllowFeature = "F060701;F060702;F060704;F060705")]
        public HttpResponseMessage getProjectProductInfo(string projectProductId)
        {
            var result = _plan.GetProjectProductInfo(projectProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddWorkDiary")]
        [HttpPost]
        //   [NTSAuthorize(AllowFeature = "F110703;F060708")]
        public HttpResponseMessage AddWorkDiary(WorkDiaryModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.SBUId = GetSBUIdByRequest();
            model.DepartmentId = GetDepartmentIdByRequest();
            var employeeId = GetEmployeeIdByRequest();

            bool outOfDate = false;
            if (this.CheckPermission(Constants.Permission_Code_F080809))
            {
                outOfDate = true;
            }

            _plan.AddWorkDiary(model, employeeId, outOfDate);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetHolidayInPlan")]
        [HttpPost]
        public HttpResponseMessage GetHolidayInPlan(PlanEndDateModel model)
        {
            var result = _plan.GetDayEndDate(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListPlan")]
        [HttpPost]
        public HttpResponseMessage GetListPlan(List<string> plansId, string EmployeeCode)
        {
            var result = _plan.GetListPlan(plansId, EmployeeCode);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListTask")]
        [HttpPost]
        public HttpResponseMessage GetListTask()
        {
            var result = _plan.GetListTask();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployeeInfor")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeInfor(string EmployeeId, int month, int year)
        {
            var result = _plan.GetEmployeeInfor(EmployeeId, month, year);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListPlanHistory")]
        [HttpPost]
        public HttpResponseMessage GetPlanHistory(string projectId)
        {
            var result = _plan.GetPlanHistory(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateWorkTime")]
        [HttpPost]
        public HttpResponseMessage UpdateWorkTime(TaskAssignModel taskAssignModel)
        {
            var createBy = GetUserIdByRequest();
            _plan.UpdateWorkTime(taskAssignModel, createBy);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetWorkEmployeeByDate")]
        [HttpPost]
        public HttpResponseMessage GetWorkEmployeeByDate(string EmployeeId, DateTime date)
        {
            var result = _plan.GetWorkEmployeeByDate(EmployeeId, date);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
