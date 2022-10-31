using NTS.Model.NTSDepartment;
using NTS.Model.SBU;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SBUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SBU
{
    [RoutePrefix("api/SBU")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030205")]
    public class SBUsController : BaseController
    {
        private readonly SBUBusiness _business = new SBUBusiness();

        [Route("SearchSBU")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030200")]
        public HttpResponseMessage SearchSBU(SBUSearchModel modelSearch)
        {
            var result = _business.SearchSBU(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[Route("SearchDepartment")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F030200")]
        //public HttpResponseMessage SearchDepartment(DepartmentSearchModel modelSearch)
        //{
        //    var result = _business.SearchDepartment(modelSearch);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [Route("AddSBU")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030201")]
        public HttpResponseMessage AddSBU(SBUModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSBU(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSBU")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030202")]
        public HttpResponseMessage UpdateSBU(SBUModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSBU(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSBU")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030203")]
        public HttpResponseMessage DeleteSBU(SBUModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteSBU(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSBUInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030202;F030204")]
        public HttpResponseMessage GetSBUInfo(SBUModel model)
        {
            var result = _business.GetSBUInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
