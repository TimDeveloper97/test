using NTS.Model.Categories;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Categories
{
    [RoutePrefix("api/Categories")]
    [ApiHandleExceptionSystem]
    public class CategoryController : BaseController
    {
        private readonly QuoteStepBussiness _quoteBussiness = new QuoteStepBussiness();
        private readonly TechnologySolutionBussiness _techBussiness = new TechnologySolutionBussiness();
        

        [Route("SearchQuoteStep")]
        [HttpPost]
        public HttpResponseMessage SearchQuoteStep(SearchQuoteStepModel search)
        {
            var result = _quoteBussiness.SearchQuoteStep(search);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetQuoteInfo")]
        [HttpPost]
        public HttpResponseMessage GetQuoteInfo(QuoteStepModel model)
        {
            var result = _quoteBussiness.GetQuoteInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetIndex")]
        [HttpPost]
        public HttpResponseMessage GetIndex()
        {
            try
            {
                var result = _quoteBussiness.GetIndex();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CreateQuote")]
        [HttpPost]
        public HttpResponseMessage CreateQuote(QuoteStepModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _quoteBussiness.CreateQuote(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateQuote")]
        [HttpPost]
        public HttpResponseMessage UpdateQuote(QuoteStepModel model)
        {
            _quoteBussiness.UpdateQuote(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateIndex")]
        [HttpPost]
        public HttpResponseMessage CreateIndex(SearchQuoteStepModel model)
        {
            _quoteBussiness.CreateIndex(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }


        [Route("DeleteQuote")]
        [HttpPost]
        public HttpResponseMessage DeleteQuote(SearchQuoteStepModel model)
        {
            _quoteBussiness.DeleteQuote(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        //TechnologySolution

        [Route("SearchTech")]
        [HttpPost]
        public HttpResponseMessage SearchTech(SearchTechnologySolutionModel search)
        {
            var result = _techBussiness.SearchTech(search);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteTech")]
        [HttpPost]
        public HttpResponseMessage DeleteTech(SearchTechnologySolutionModel model)
        {
            _techBussiness.DeleteTech(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetTechIndex")]
        [HttpPost]
        public HttpResponseMessage GetTechIndex()
        {
            try
            {
                var result = _techBussiness.GetTechIndex();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CreateTech")]
        [HttpPost]
        public HttpResponseMessage CreateTech(TechnologySolutionModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _techBussiness.CreateTech(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateTech")]
        [HttpPost]
        public HttpResponseMessage UpdateTech(TechnologySolutionModel model)
        {
            _techBussiness.UpdateTech(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateTechIndex")]
        [HttpPost]
        public HttpResponseMessage CreateTechIndex(SearchTechnologySolutionModel model)
        {
            _techBussiness.CreateTechIndex(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }


        [Route("GetTechInfo")]
        [HttpPost]
        public HttpResponseMessage GetTechInfo(TechnologySolutionModel model)
        {
            var result = _techBussiness.GetTechInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
