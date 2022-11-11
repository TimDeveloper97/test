using NTS.Common;
using NTS.Model.Guideline;
using NTS.Model.ImportPR;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Guidelines;
using QLTK.Business.ImportPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Guidelines
{
    [RoutePrefix("api/Guideline")]
    [ApiHandleExceptionSystem]
    public class GuidelineController : BaseController
    {
        private readonly GuidelineBusiness _business = new GuidelineBusiness();

        [Route("GetGuidelineInfo")]
        [HttpPost]
        public HttpResponseMessage GetGuidelineInfo(GuidelineModel model)
        {
            var result = _business.GetGuidelineInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateGuidelineInfo")]
        [HttpPost]
        public HttpResponseMessage UpdateGuidelineInfo(GuidelineModel model)
        {
            _business.UpdateGuidelineInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK,string.Empty);
        }
    }
}
