using NTS.Model.FuturePersonnelForecast;
using NTS.Model.Plans;
using NTS.Model.ScheduleProject;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.FuturePersonnelForecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.FuturePersonnelForecast
{
    [RoutePrefix(prefix: "api/FuturePersonnelForecast")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class FuturePersonnelForecastController : BaseController
    {
        private readonly FuturePersonnelForecastBussiness bussiness = new FuturePersonnelForecastBussiness();

        [Route("Search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100001")]
        public HttpResponseMessage SearchTasks(FuturePersonnelForecastModel modelSearch)
        {

            var result = bussiness.Search(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("SearchSelectProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100001")]
        public HttpResponseMessage SearchSelectProject(FuturePersonnelForecastModel modelSearch)
        {
            modelSearch.SBUId = GetSbuIdByRequest();
            var result = bussiness.SearchSelectProject(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPlans")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100001")]
        public HttpResponseMessage SearchPlans(FuturePersonnelForecastSearchModel modelSearch)
        {
            //modelSearch.SbuId = GetSbuIdByRequest();
            var result = bussiness.GetListPlanByProjectId(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
