using NTS.Model.MasterEmployee;
using QLTK.Api.Attributes;
using QLTK.Business.MasterEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.MasterEmployee
{
    [RoutePrefix("api/MasterEmployee")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F100013")]
    public class MasterEmployeeController : ApiController
    {
        private readonly MasterEmployeeBussiness _business = new MasterEmployeeBussiness();
        [Route("SearchMasterEmployee")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100006")]
        public HttpResponseMessage SearchMasterEmployee(MasterEmployeeModel model)
        {

            var result = _business.SearchMasterEmployee(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F100006")]
        public HttpResponseMessage ExportExcel(MasterEmployeeModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
