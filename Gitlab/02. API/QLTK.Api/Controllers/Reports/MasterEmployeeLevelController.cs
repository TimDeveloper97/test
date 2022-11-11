using NTS.Model.MasterEmployeeLevel;
using QLTK.Api.Attributes;
using QLTK.Business.MasterEmployeeLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.MasterEmployeeLevel
{
    [RoutePrefix("api/MasterEmployeeLevel")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class MasterEmployeeLevelController : ApiController
    {
        private readonly MasterEmployeeLevelBussiness _business = new MasterEmployeeLevelBussiness();
        [Route("SearchMasterEmployeeLevel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100005")]
        public HttpResponseMessage SearchMasterEmployeeLevel(MasterEmployeeLevelModel model)
        {

            var result = _business.SearchEmployeeLevel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100005")]
        public HttpResponseMessage ExportExcel(MasterEmployeeLevelModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
