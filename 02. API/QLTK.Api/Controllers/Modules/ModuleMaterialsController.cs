using NTS.Model.ModuleMaterials;
using NTS.Model.QLTKMODULE;
using NTS.Model.SimilarMaterial;
using NTS.Model.SimilarMaterialConfig;
using QLTK.Api.Attributes;
using QLTK.Business.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ModuleMaterials
{
    [RoutePrefix("api/ModuleMaterial")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020129")]
    public class ModuleMaterialsController : BankController
    {
        private readonly ModuleMaterialBusiness _business = new ModuleMaterialBusiness();

        [Route("SearchModuleMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020113")]
        public HttpResponseMessage SearchModuleMaterial(ModuleMaterialSearchModel model)
        {
            var result = _business.SearchModuleMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSimilarMaterialConfig")]
        [HttpPost]
        public HttpResponseMessage SearchSimilarMaterialConfig(SimilarMaterialConfigSearchModel modelSearch)
        {
            var result = _business.SearchSimilarMaterialConfig(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSimilarMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            var result = _business.SearchSimilarMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020113")]
        public HttpResponseMessage ExportExcel(ModuleMaterialSearchModel model)
        {
            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ExportExcelMaterialBOMDraft")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020113")]
        public HttpResponseMessage ExportExcelMaterialBOMDraft(ModuleMaterialSearchModel model)
        {
            string path = _business.ExportExcelMaterialBOMDraft(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("SearchModuleMaterialsSetup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020118")]
        public HttpResponseMessage SearchModuleMaterialsSetup(ModuleMaterialSearchModel modelSearch)
        {
            var result = _business.SearchModuleMaterialsSetup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateFileModuleMaterial")]
        [HttpPost]
        public HttpResponseMessage UpdateFileModuleMaterial(ModuleMaterialUploadFileModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.UpdateFileModuleMaterial(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
