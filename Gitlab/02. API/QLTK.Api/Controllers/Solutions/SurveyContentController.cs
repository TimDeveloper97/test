using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.ProjectAttch;
using NTS.Model.SurveyContent;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using QLTK.Business.SurveyContent;

namespace QLTK.Api.Controllers.Solution
{
    [RoutePrefix("api/SurveyContent")]
    [ApiHandleExceptionSystem]
    public class SurveyContentController : BaseController
    {
        private readonly SurveyContentBussiness _business = new SurveyContentBussiness();

        [Route("Search")]
        [HttpPost]
        public HttpResponseMessage Search (SurveyContentSearchModel modelSearch)
        {
            var result = _business.Search(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var result = _business.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create (SurveyContentCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _business.Create(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("search-document-file")]
        [HttpPost]
        public HttpResponseMessage SearchDocumentFile(SurveyContentAttachModel searchModel)
        {
            var result = _business.SearchDocumentFile(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("update/{id}")]
        [HttpPost]

        public HttpResponseMessage Update (SurveyContentCreateModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.Update (model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete (string id)
        {
            _business.Delete (id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
;        }
    }
}
