using NTS.Model.Config;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Config
{
    [RoutePrefix("api/Config")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F110452")]
    public class ConfigController : BaseController
    {
        private readonly ConfigBussiness config = new ConfigBussiness();

        [Route("SearchConfig")]
        [HttpPost]
        public HttpResponseMessage SearchConfig()
        {
            var result = config.SearchConfig();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetConfigInfo")]
        [HttpPost]
        public HttpResponseMessage GetConfigInfo(ConfigModel model)
        {
            var result = config.GetConfigInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateConfig")]
        [HttpPost]
        public HttpResponseMessage UpdateConfig(ConfigModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            config.UpdateConfig(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
