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
using NTS.Model.CustomerRequirement;

namespace QLTK.Api.Controllers.Solution
{
    [RoutePrefix("api/SolutionAnalysisProduct")]
    [ApiHandleExceptionSystem]
    public class SolutionAnalysisProductController : BaseController
    {
        private readonly SolutionAnalysisProductBussiness _business = new SolutionAnalysisProductBussiness();

        [Route("SearchSolutionProducts")]
        [HttpPost]
        public HttpResponseMessage SearchSolutionProduct(SolutionAnalysisProductSearchModel modelSearch)
        {
            var result = _business.SearchSolutionProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateSolutionProduct")]
        [HttpPost]
        public HttpResponseMessage CreateSolutionProduct(SolutionAnalysisProductModel model)
        {
            string userId = GetUserIdByRequest();
            _business.CreateSolutionProduct(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSolutionProduct")]
        [HttpPost]
        public HttpResponseMessage UpdateSolutionProduct(SolutionAnalysisProductModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSolutionProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSolutionProduct")]
        [HttpPost]
        public HttpResponseMessage DeleteSolutionProduct(string solutionProductId)
        {
            _business.DeleteSolutionProduct(solutionProductId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetByIdSolutionProduct")]
        [HttpPost]
        public HttpResponseMessage GetByIdSolutionProduct(string solutionProductId)
        {
            var result = _business.GetByIdSolutionProduct(solutionProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}
