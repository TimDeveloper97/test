using NTS.Model.PracticeFile;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PracticeFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeFile
{
    [RoutePrefix("api/PracticeFile")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeFileController : BaseController
    {
        private readonly PracticeFileBussiness _business = new PracticeFileBussiness();

        [Route("GetPracticeFileInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040720")]
        public HttpResponseMessage GetPracticeFileInfo(PracticeFileModel model)
        {
            var result = _business.GetPracticeFileInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040720")]
        public HttpResponseMessage AddFile(PracticeFileModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddPracticeFiles(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
