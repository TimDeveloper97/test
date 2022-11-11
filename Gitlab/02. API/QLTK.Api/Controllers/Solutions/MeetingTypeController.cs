using NTS.Model.Meeting;
using NTS.Model.MeetingType;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace QLTK.Api.Controllers.Solutions
{
    [RoutePrefix("api/MeetingType")]
    [ApiHandleExceptionSystem]
    public class MeetingTypeController : BaseController
    {
        private readonly MeetingTypeBussiness _business = new MeetingTypeBussiness();

        [Route("SearchMeetingType")]
        [HttpGet]
        public HttpResponseMessage SearchMeetingType()
        {
            var result = _business.Search();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateMeetingType")]
        [HttpPost]
        public HttpResponseMessage CreateMeetingType(MeetingTypeModel model)
        {
            _business.CreateMeetingType(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateMeetingType")]
        [HttpPost]
        public HttpResponseMessage UpdateMeetingType(MeetingTypeModel model)
        {
            _business.UpdateMeetingType(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteMeetingType/{id}")]
        [HttpPost]
        public HttpResponseMessage DeleteSolutionGroup(string id)
        {
            _business.DeleteMeetingType(id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSolutionGroupInfo(string id)
        {
            var result = _business.GetById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}