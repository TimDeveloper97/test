
using NTS.Model.JobGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.JobGroup
{
    [RoutePrefix("api/JobGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050404")]
    public class JobGroupController : BaseController
    {
        private readonly QLTK.Business.JobGroups.JobGroupBusiness jobGroup = new Business.JobGroups.JobGroupBusiness();

        [Route("SearchJobGroups")]
        [HttpPost]
        public HttpResponseMessage SearchJobGroups(JobGroupSearchModel modelSearch)
        {
            var result = jobGroup.SearchJobGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddJobGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050401")]
        public HttpResponseMessage AddJobGroup(JobGroupModel model)
        {

            jobGroup.AddJobGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetJobGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050402")]
        public HttpResponseMessage GetFunction(JobGroupModel model)
        {
            var result = jobGroup.GetJobGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateJobGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050402")]
        public HttpResponseMessage UpdateJobGroup(JobGroupModel model)
        {
            jobGroup.UpdateJobGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteJobGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050403")]
        public HttpResponseMessage DeleteJobGroup(JobGroupModel model)
        {
            jobGroup.DeleteJobGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
