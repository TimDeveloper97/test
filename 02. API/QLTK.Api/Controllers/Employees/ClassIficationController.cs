using NTS.Model.Classification;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ClassIfication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ClassIfication
{
    [RoutePrefix("api/ClassIfication")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F110405")]
    public class ClassIficationController : BaseController
    {
        private readonly ClassIficationService _ificationService = new ClassIficationService();

        [Route("SearchClassIfication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110400")]
        public HttpResponseMessage SearchClassIfication(ClassIficationSearchModel modelSearch)
        {
            var result = _ificationService.SearchClassIfication(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateClassIfication")]
        [HttpPost]        
        [NTSAuthorize(AllowFeature = "F110401")]
        public HttpResponseMessage CreateClassIfication(ClassIficationModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _ificationService.CreateClassIfication(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetInforClassIfication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110402;F110404")]
        public HttpResponseMessage GetClassIficationInfo(string ificationId)
        {
            var result = _ificationService.GetClassIficationInfo(ificationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateClassIfication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110402")]
        public HttpResponseMessage UpdateClassIfication(ClassIficationModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _ificationService.UpdateClassIfication(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteClassIfication")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110403")]
        public HttpResponseMessage DeleteClassIfication(string ificationId)
        {
            _ificationService.DeleteClassIfication(ificationId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
