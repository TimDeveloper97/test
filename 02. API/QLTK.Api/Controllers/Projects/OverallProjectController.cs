using NTS.Common;
using NTS.Model;
using NTS.Model.Plans;
using NTS.Model.ScheduleProject;
using NTS.Model.TaskTimeStandardModel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.OverallProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.OverallProject
{
    [RoutePrefix("api/OverallProject")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060709")]
    public class OverallProjectController : BaseController
    {
        private readonly OverallProjectBusiness _overallProject = new OverallProjectBusiness();


        /// <summary>
        /// Tổng thể dự án
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchOverallProject/{Id}")]
        [HttpPost]
        public HttpResponseMessage SearchWorkingReports(string id)
        {
            var result = _overallProject.Excel(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
