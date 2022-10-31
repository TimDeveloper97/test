using NTS.Model.MasterLibrary;
using QLTK.Api.Attributes;
using QLTK.Business.MasterLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.MasterLibrary
{
    [RoutePrefix("api/MasterLibrary")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class MasterLibraryController : ApiController
    {
        private readonly MasterLibraryBussiness _business = new MasterLibraryBussiness();
        [Route("SearchMasterLibrary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100004")]
        public HttpResponseMessage SearchMasterLibrary(MasterLibraryModel model)
        {

            var result = _business.SearchMasterLibrary(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100004")]
        public HttpResponseMessage ExportExcel(MasterLibraryModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
