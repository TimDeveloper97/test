using NTS.Model.FileDefinition;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.FileDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.FileDefinition
{
    [RoutePrefix("api/FileDefinition")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090608")]

    public class FileDefinitionsController : BaseController
    {
        private readonly FileDefinitionBusiness _business = new FileDefinitionBusiness();

        [Route("AddFileDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090604")]
        public HttpResponseMessage AddFileDefinition(FileDefinitionModel model)
        {
            var deparmtnetId = this.GetDepartmentIdByRequest();
            _business.AddFileDefinition(model, deparmtnetId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateFileDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090605")]
        public HttpResponseMessage UpdateFileDefinition(FileDefinitionModel model)
        {
            var deparmtnetId = this.GetDepartmentIdByRequest();
            _business.UpdateFileDefinition(model, deparmtnetId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteFileDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090606")]
        public HttpResponseMessage DeleteFileDefinition(FileDefinitionModel model)
        {
            var deparmtnetId = this.GetDepartmentIdByRequest();
            _business.DeleteFileDefinition(model, deparmtnetId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListFileDefinition")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090600")]
        public HttpResponseMessage GetListFileDefinition(FileDefinitionModel model)
        {
            var result = _business.GetListFileDefinition(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetFileDefinitionInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090605")]
        public HttpResponseMessage GetFileDefinitionInfo(FileDefinitionModel model)
        {
            var result = _business.GetFileDefinitionInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetFileDefinitions")]
        [HttpGet]
        [NTSAuthorize(AllowFeature = "F090600")]
        public HttpResponseMessage GetFileDefinitions(int designType, int objectType)
        {
            var deparmtnetId = this.GetDepartmentIdByRequest();
            var result = _business.GetFileDefinitions(designType, objectType, deparmtnetId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
