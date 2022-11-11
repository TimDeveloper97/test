using QLTK.Api.Attributes;
using QLTK.Business.ForecastProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ForecastProject
{
    [RoutePrefix("api/ForecastProject")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class ForecastProjectsController : ApiController
    {
        private readonly ForecastProjectBussiness _business = new ForecastProjectBussiness();

        [Route("GetForecastProjects")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100002")]
        public HttpResponseMessage GetForecastProjects()
        {
            var result = _business.GetForecastProjects();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
