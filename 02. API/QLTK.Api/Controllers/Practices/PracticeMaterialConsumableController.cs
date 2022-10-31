using NTS.Model.Materials;
using NTS.Model.PracticeMaterialConsumable;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.PracticeMaterialConsumable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeMaterialConsumable
{
    [RoutePrefix("api/PracticeMaterialConsumable")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040730")]
    public class PracticeMaterialConsumableController : BaseController
    {
        private readonly PracticeMaterialConsumableBussiness _bussiness = new PracticeMaterialConsumableBussiness();

        [Route("SearchPracticeMaterialConsumable")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040715")]
        public HttpResponseMessage SearchPracticeMaterialConsumable(PracticeMaterialConsumableSearchModel modelSearch)
        {
            var result = _bussiness.SearchPracticeMaterialConsumable(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040713")]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _bussiness.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddPracticeMaterialConsumable")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040713")]
        public HttpResponseMessage AddPracticeMaterialConsumable(PracticeMaterialConsumableModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _bussiness.AddPracticeMaterialConsumable(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcelPracticeMaterialConsumable")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040716")]
        public HttpResponseMessage ExportExcelPracticeMaterialConsumable(PracticeMaterialConsumableSearchModel modelSearch)
        {
            var result = _bussiness.ExportExcelPracticeMaterialConsumable(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ImportMaterialConsumable")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110201")]
        public HttpResponseMessage ImportMaterialConsumable()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var results = _bussiness.ImportMaterialConsumable(userId, hfc[0]);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}
