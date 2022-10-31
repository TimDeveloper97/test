using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Survey;
using NTS.Model.SolutionDesignDocuments;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Solution
{
    [RoutePrefix("api/survey")]
    [ApiHandleExceptionSystem]
    public class SurveyController : BaseController
    {
        private readonly SurveyBussiness _business = new SurveyBussiness();

        /// <summary>
        /// Tìm kiếm 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public HttpResponseMessage SearchChannel(SurveySearchResultModel searchModel)
        {
            var result = _business.SearchSurvey(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateSurvey(SurveyCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateSurvey(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _business.DeleteSurvey(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage UpdatetSurvey(SurveyCreateModel model)
        {
            _business.UpdatetSurvey(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetById(string id)
        {
            var result = _business.GetSurveyById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}
