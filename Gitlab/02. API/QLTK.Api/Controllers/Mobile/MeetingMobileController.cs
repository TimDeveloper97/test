using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Meeting;
using System.Net;
using NTS.Model.SaleGroups;
using QLTK.Business.SaleGroups;

namespace QLTK.Api.Controllers.Solutions
{
    [RoutePrefix("api/Meetingmobile")]
    [ApiHandleExceptionSystem]
    public class MeetingMobileController : BaseController
    {
        private readonly MeetingBussiness _business = new MeetingBussiness();
        private readonly SaleGroupBussiness _saleGroup = new SaleGroupBussiness();

        [Route("SearchMeeting")]
        [HttpPost]
        public HttpResponseMessage SearchMeeting(MeetingSearchModel modelSearch)
        {
            modelSearch.IsBGĐ = CheckPermission("F121803");
            modelSearch.IsTrPhong = CheckPermission("F121802");
            modelSearch.UserId = this.GetUserIdByRequest();
            modelSearch.DepartmentId = this.GetDepartmentIdByRequest();

            var result = _business.SearchMeeting(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("statistic")]
        [HttpGet]
        public HttpResponseMessage StatisticMeeting()
        {
            MeetingSearchModel modelSearch = new MeetingSearchModel();
            modelSearch.IsBGĐ = CheckPermission("F121803");
            modelSearch.IsTrPhong = CheckPermission("F121802");
            modelSearch.UserId = this.GetUserIdByRequest();
            modelSearch.DepartmentId = this.GetDepartmentIdByRequest();

            var result = _business.Statistic(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("get-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetMeetingInfoById(string id)
        {
            var result = _business.GetMeetingInfoById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sửa meeting
        /// </summary>
        /// <returns></returns>
        [Route("performing/update")]
        [HttpPost]
        public HttpResponseMessage UpdateMeeting(MeetingPerformingEntity model)
        {
            _business.UpdateMeetingPerforming(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật thông tin ở Tab Tạo meeting
        /// </summary>
        /// <returns></returns>
        [Route("info/update")]
        [HttpPost]
        public HttpResponseMessage UpdateInfo(MeetingInfoEntity model)
        {
            _business.UpdateInfo(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Xác nhận thời gian Meeting
        /// </summary>
        /// <returns></returns>
        [Route("plan/update")]
        [HttpPost]
        public HttpResponseMessage UpdatePlan(MeetingPlanEntity model)
        {
            _business.UpdatePlan(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListEmployee")]
        [HttpPost]
        public HttpResponseMessage GetListEmployee(EmployeeSearchModel modelSearch)
        {
            var result = _saleGroup.GetListEmployee(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreateMeeting(MeetingCreateModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CreateMeeting(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("cancel")]
        [HttpPost]
        public HttpResponseMessage CancelMeeting(MeetingCancelModel model)
        {
            _business.CancelMeeting(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("finish/{id}")]
        [HttpPost]
        public HttpResponseMessage FinishMeeting(string id)
        {
            _business.FinishMeeting(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("request/create")]
        [HttpPost]
        public HttpResponseMessage CreateRequest(MeetingRequirementEntity model)
        {
            var userId = GetUserIdByRequest();
            _business.CreateRequirement(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("request/update")]
        [HttpPost]
        public HttpResponseMessage UpdateRequest(MeetingRequirementEntity model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateRequirement(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("request/delete/{id}")]
        [HttpPost]
        public HttpResponseMessage DeleteRequest(string id)
        {
            _business.DeleteRequirement(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("request/detail/{id}")]
        [HttpPost]
        public HttpResponseMessage GetDetailRequest(string id)
        {
            var result = _business.GetDetailRequirement(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Tự động tạo mã meeting
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCode")]
        [HttpPost]
        public HttpResponseMessage GenerateCode(MeetingCodeCharModel model)
        {
            var result = _business.GenerateCode(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}