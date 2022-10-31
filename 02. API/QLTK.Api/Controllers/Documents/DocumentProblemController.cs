using NTS.Model.DocumentProblem;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DocumentProblem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers
{
    [RoutePrefix("api/document-problem")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030105")]
    public class DocumentProblemController : BaseController
    {
        private readonly DocumentProblemBussiness _business = new DocumentProblemBussiness();

        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030100")]
        public HttpResponseMessage SearchDocument(DocumentProblemSearchModel searchModel)
        {
            var result = _business.SearchDocumentProblem(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030103")]
        public HttpResponseMessage Delete(DocumentProblemModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteDocumentProblem(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030101")]
        public HttpResponseMessage Create(DocumentProblemModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateDocumentProblem(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102")]
        public HttpResponseMessage Update(DocumentProblemModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateDocumentProblem(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getProblemInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F030102;F030104")]
        public HttpResponseMessage GetDocumentProblem(DocumentProblemModel modelSearch)
        {

            var result = _business.GetDocumentProblem(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
