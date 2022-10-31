
using NTS.Model.WorkTIme;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkTime
{
    [RoutePrefix("api/WorkTime")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080705")]
    public class WorkTimeController : BaseController
    {
        private readonly WorkTimeBussiness _business = new WorkTimeBussiness();

        [Route("SearchWorkTime")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080700")]
        public HttpResponseMessage SearchWorkTime(WorkTimeSearchModel modelSearch)
        {
            var result = _business.SearchWorkTime(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteWorkTime")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080703")]
        public HttpResponseMessage DeleteWorkTime(WorkTimeModel model)
        {
            _business.DeleteWorkTime(model,GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddWorkTime")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080701")]
        public HttpResponseMessage AddWorkTime(WorkTimeModel model)
        {
            _business.AddWorkTime(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateWorkTime")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080702")]
        public HttpResponseMessage UpdateWorkTime(WorkTimeModel model)
        {
            _business.UpdateWorkTime(model,GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetWorkTimeInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080702;F080704")]
        public HttpResponseMessage GetWorkTimeInfo(WorkTimeModel model)
        {
            var result = _business.GetWorkTimeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
