using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.SurveyMaterial;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using QLTK.Business.SurveyMaterial;

namespace QLTK.Api.Controllers.Solution
{
    [RoutePrefix("api/SurveyMaterial")]
    [ApiHandleExceptionSystem]
    public class SurveyMaterialController : BaseController
    {
        private readonly SurveyMaterialBussiness _business = new SurveyMaterialBussiness();

        //[Route("SearchSurveyMaterial")]
        //[HttpPost]
        //[NTSAuthorize(AllowFeature = "F040100")]
        //public HttpResponseMessage SearchSurveyMaterial(SurveyMaterialSearchModel modelSearch)
        //{
        //    var result = _business.SearchSurveyMaterial(modelSearch);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [Route("GetSurveyMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040202;F040204")]
        public HttpResponseMessage GetSurveyMaterial(SurveyMaterialCreateModel model)
        {
            var result = _business.GetSurveyMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSurveyMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040201")]
        public HttpResponseMessage AddSurveyMaterial(SurveyMaterialCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _business.AddSurveyMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateSurveyMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040202")]

        public HttpResponseMessage UpdateSurveyMaterial(SurveyMaterialCreateModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSurveyMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSurveyMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040203")]
        public HttpResponseMessage DeleteSurveyMaterial(SurveyMaterialCreateModel model)
        {
            model.UpdateBy = GetUserIdByRequest();  
            _business.DeleteSurveyMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
;        }
    }
}
