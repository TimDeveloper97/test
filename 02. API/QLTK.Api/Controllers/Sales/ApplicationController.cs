using NTS.Model.Bussiness;
using NTS.Model.Bussiness.Application;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Bussiness
{
    [RoutePrefix("api/Application")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120504")]
    public class ApplicationController : BaseController
    {

        private readonly ApplicationService applicationService = new ApplicationService();

        [Route("SearchApplication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120500")]
        public HttpResponseMessage SearchApplication (ApplicationSearchModel modelSearch)
        {
            var result = applicationService.SearchApplication(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateApplication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120501")]
        public HttpResponseMessage CreateApplication (ApplicationModel model)
        {
            applicationService.CreateApplication(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetInforApplication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120502;F120504")]
        public HttpResponseMessage GetApplicationInfo (string id)
        {
            var result = applicationService.GetInfoApplication(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateApplication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120502")]
        public HttpResponseMessage UpdateApplication (ApplicationModel model)
        {
            applicationService.UpdateApplication(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteApplication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120503")]
        public HttpResponseMessage DeleteApplication (string id)
        {
            applicationService.DeleteApplication(id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
