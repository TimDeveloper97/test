using NTS.Model.CustomerContact;
using NTS.Model.CustomerRequirement;
using NTS.Model.Meeting;
using NTS.Model.MeetingAttach;
using NTS.Model.MeetingContent;
using NTS.Model.MeetingCustomerContact;
using NTS.Model.MeetingEmployee;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Projects;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Solutions
{
    [RoutePrefix("api/Meeting")]
    [ApiHandleExceptionSystem]
    public class MeetingController : BaseController
    {
        private readonly MeetingBussiness _business = new MeetingBussiness();
        private readonly CustomerContactBussiness _customerContactBussiness = new CustomerContactBussiness();


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

        [Route("SearchMeetingFinish")]
        [HttpPost]
        public HttpResponseMessage SearchMeetingFinish(MeetingSearchModel modelSearch)
        {
            var result = _business.SearchMeetingFinish(modelSearch);
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

        [Route("CreateMeeting")]
        [HttpPost]
        public HttpResponseMessage CreateMeeting(MeetingCreateModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CreateMeeting(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
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
        [Route("update/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateMeeting(string id, MeetingInfoModel model)
        {
            _business.UpdateMeeting(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("update-customer-requirment/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateMeetingCustomerRequirement(string id, MeetingInfoModel model)
        {
            _business.UpdateMeetingCustomerRequirement(id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _business.DeleteMeeting(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("do-meeting/{id}")]
        [HttpPost]
        public HttpResponseMessage DoMeeting(string id)
        {
            _business.DoMeeting(id);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("cancel-meeting")]
        [HttpPost]
        public HttpResponseMessage CancelMeeting(MeetingCancelModel model)
        {
            _business.CancelMeeting(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("finish-meeting/{id}")]
        [HttpPost]
        public HttpResponseMessage FinishMeeting(string id)
        {
            _business.FinishMeeting(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("add-customer-requirement/{id}")]
        [HttpPost]
        public HttpResponseMessage AddCustomerRequirement(MeetingInfoModel meetingInfoModel, string id)
        {
            _business.AddCustomerRequirement( meetingInfoModel, id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getMeetingCustomerContactInfo")]
        [HttpPost]
          public HttpResponseMessage GetMeetingCustomerContactInfo(MeetingCustomerContactModel model)
        {
           
            var result = _business.GetMeetingCustomerContactInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel/{id}")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(string id, MeetingInfoModel model)
        {
            try
            {
                var result = _business.ExportExcel(id,model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
           
        }
        [Route("UpdateMeetingRequimentNeedHandle")]
        [HttpPost]
        public HttpResponseMessage UpdateMeetingRequimentHandle(MeetingContentModel model)
        {
            try
            {
                var userId = GetUserIdByRequest();
                _business.UpdateMeetingRequimentNeedHandle(model, userId);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("DeleteMeetingRequimentNeedHandle")]
        [HttpPost]
        public HttpResponseMessage DeleteMeetingRequimentNeedHandle(MeetingContentModel model)
        {
            try
            {
                _business.DeleteMeetingRequimentNeedHandle(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("UpdateMeetingFileRequimentNeedHandle")]
        [HttpPost]
        public HttpResponseMessage UpdateMeetingFileRequimentNeedHandle(MeetingContentAttachModel model)
        {
            try
            {
                var userId = GetUserIdByRequest();
                _business.UpdateMeetingFileRequimentNeedHandle(model, userId);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("DeleteMeetingFileRequimentNeedHandle")]
        [HttpPost]
        public HttpResponseMessage DeleteMeetingFileRequimentNeedHandle(MeetingContentAttachModel model)
        {
            try
            {
                var userId = GetUserIdByRequest();
                _business.DeleteMeetingFileRequimentNeedHandle(model, userId);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("GetRequimentContent/{id}")]
        [HttpPost]
        public HttpResponseMessage GetRequimentContent(string id)
        {
            try
            {
                var data =_business.GetRequimentContent(id);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("CreateMeetingCustomerContact")]
        [HttpPost]

        public HttpResponseMessage CreateMeetingCustomerContact(CustomerContactModel model)
        {
            try
            {
                _customerContactBussiness.Create(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("CreateCustomerRequimentMeetingContent")]
        [HttpPost]

        public HttpResponseMessage CreateCustomerRequimentMeetingContent(CustomerRequirementCreateModel model)
        {
            try
            {
                model.CreateBy = GetUserIdByRequest();
                _business.CreateCustomerRequimentMeetingContent(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}