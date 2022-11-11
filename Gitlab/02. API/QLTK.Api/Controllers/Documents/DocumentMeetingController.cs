using NTS.Model.DocumentMeeting;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DocumentMeeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DocumentMeeting
{
    [RoutePrefix("api/document-meeting")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121323")]
    public class DocumentMeetingController : BaseController
    {
        private readonly DocumentMeetingBussiness _business = new DocumentMeetingBussiness();

        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121316")]
        public HttpResponseMessage SearchDocument(DocumentMeetingSearchModel searchModel)
        {
            var result = _business.SearchDocumentMeeting(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121319")]
        public HttpResponseMessage Delete(DocumentMeetingModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteDocumentMeeting(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121317")]
        public HttpResponseMessage CreateDocument(ReportMeetingModel model)
        {
            string userId = GetUserIdByRequest();
            _business.CreateDocumentMeeting(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121318")]
        public HttpResponseMessage Update(ReportMeetingModel model)
        {
            string userId = GetUserIdByRequest();
            _business.UpdateDocumentMeeting(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("getMeetingInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121318")]
        public HttpResponseMessage GetDocumentProblem(ReportMeetingModel modelSearch)
        {

            var result = _business.GetDocumentMeeting(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
