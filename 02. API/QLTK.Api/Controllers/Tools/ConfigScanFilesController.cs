using NTS.Model.ConfigScanFile;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ConfigScanFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ConfigScanFile
{
    [RoutePrefix("api/ConfigScanFile")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F091005")]
    public class ConfigScanFilesController : BaseController
    {
        private readonly ConfigScanFileBussiness _business = new ConfigScanFileBussiness();

        [Route("SearchConfigScanFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001101")]
        public HttpResponseMessage SearchConfigScanFile(ConfigScanFileSearchModel modelSearch)
        {
            var result = _business.SearchConfigScanFile(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddConfigScanFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001102")]
        public HttpResponseMessage AddConfigScanFile(ConfigScanFileModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddConfigScanFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateConfigScanFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001103")]
        public HttpResponseMessage UpdateConfigScanFile(ConfigScanFileModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateConfigScanFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteConfigScanFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001104")]
        public HttpResponseMessage DeleteConfigScanFile(ConfigScanFileModel model)
        {
            _business.DeleteConfigScanFile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetConfigScanFileInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F001103")]
        public HttpResponseMessage GetConfigScanFileInfo(ConfigScanFileModel model)
        {
            var result = _business.GetConfigScanFileInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
