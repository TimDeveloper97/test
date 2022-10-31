using NTS.Model.Holiday;
using NTS.Model.ImportProfileDocumentConfigs;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Holidays;
using QLTK.Business.ImportProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ImportProfiles
{
    [RoutePrefix("api/DocumentConfig")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F120602")]
    public class ImportProfileDocumentConfigController : BaseController
    {
        ImportProfileDocumentConfigBussiness _importProfileDocumentConfigBussiness = new ImportProfileDocumentConfigBussiness();

        [Route("GetDocumentConfigByType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120600")]
        public HttpResponseMessage GetDocumentConfigByType(ImportProfileDocumentConfigModel documentConfig)
        {
            var result = _importProfileDocumentConfigBussiness.GetDocumentConfigByType(documentConfig.Step);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateDocumentConfig")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F120601")]
        public HttpResponseMessage UpdateDocumentConfig(ImportProfileDocumentConfigUpdateModel documentConfig)
        {
            _importProfileDocumentConfigBussiness.UpdateDocumentConfig(documentConfig, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
