using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.WorkPlace;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.WorkPlace
{
    [RoutePrefix("api/WorkPlace")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040205")]
    public class WorkPlaceController : BaseController
    {
        private readonly Business.WorkPlace.WorkPlace _workPlace = new Business.WorkPlace.WorkPlace();

        [Route("SearchWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040100")]
        public HttpResponseMessage SearchWorkPlace(WorkPlaceSearchModel modelSearch)
        {
            var result = _workPlace.SearchWorkPlace(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040202;F040204")]
        public HttpResponseMessage GetWorkPlace(WorkPlaceModel model)
        {
            var result = _workPlace.GetByIdWorkPlace(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040201")]
        public HttpResponseMessage AddWorkPlace(WorkPlaceModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _workPlace.AddWorkPlace(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040202")]

        public HttpResponseMessage UpdateWorkPlace(WorkPlaceModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _workPlace.UpdateWorkPlace(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040203")]
        public HttpResponseMessage DeleteWorkPlace(WorkPlaceModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _workPlace.DeleteWorkPlace(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
;        }
    }
}
