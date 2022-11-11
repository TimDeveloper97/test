using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Degree;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.Degree
{
    [RoutePrefix(prefix: "api/Degree")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040005")]
    public class DegreeController : BaseController
    {
        private readonly Business.Degree.Degree _degree = new Business.Degree.Degree();

        [Route("SearchDegree")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040000")]
        public HttpResponseMessage SearchDegree(DegreeSearchModel modelSearch)
        {
            var result = _degree.SearchDegree(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDegree")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040002;F040004")]
        public HttpResponseMessage GetDegree(DegreeModel model)
        {
            var result = _degree.GetDegreeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddDegree")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040001")]
        public HttpResponseMessage AddDegress(DegreeModel model)
        {
            string userId = GetUserIdByRequest();
            var result = _degree.AddDegree(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteDegree")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040003")]
        public HttpResponseMessage DeleteDegree(DegreeModel model)
        {
            string userId = GetUserIdByRequest();
            _degree.DeleteDegree(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("UpdateDegree")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040002")]
        public HttpResponseMessage UpdateDegree(DegreeModel model)
        {
            string userId = GetUserIdByRequest();
            _degree.UpdateDegree(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
