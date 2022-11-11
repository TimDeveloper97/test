using NTS.Model.Materials;
using NTS.Model.PracticeSupMaterial;
using NTS.Model.QLTKMODULE;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PracticeSupMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeSupMaterial
{
    /// <summary>
    /// Thiết bị phụ trợ bài thực hành
    /// </summary>
    [RoutePrefix("api/PracticeSubMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeSupMaterialsController : BaseController
    {
        private readonly PracticeSupMaterialBussiness _bussiness = new PracticeSupMaterialBussiness();

        [Route("SearchPracticeSubMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040711")]

        public HttpResponseMessage SearchPracticeSubMaterial(PracticeSupMaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeSupMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040709")]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040709")]
        public HttpResponseMessage SearchModule(ModuleSearchModel model)
        {
            var result = _bussiness.SearchModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPriceModule")]
        [HttpPost]
        public HttpResponseMessage GetPriceModule(List<MaterialModel> listModule)
        {
            var result = _bussiness.GetPriceModule(listModule);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPracticeSupMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040709")]
        public HttpResponseMessage AddPracticeSupMaterial(PracticeSupMaterialInfoModel model)
        {
            _bussiness.AddPracticeSupMaterial(model, GetUserIdByRequest());

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcelPracticeSubMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040712")]
        public HttpResponseMessage ExportExcelPracticeSubMaterial(PracticeSupMaterialSearchModel modelSearch)
        {
            var result = _bussiness.ExportExcelPracticeSupMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ImportSubMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040731")]
        public HttpResponseMessage ImportSubMaterial()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var results = _bussiness.ImportSubMaterial(userId, hfc[0]);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}
