using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business;

namespace QLTK.Api.Controllers.WebService
{
    [RoutePrefix("api/WebService")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F090702")]
    public class TestDesignStructureController : BaseController
    {
        private WebServiceBussiness _bussiness = new WebServiceBussiness();

        [Route("SearchList3D")]
        [HttpPost]
        public HttpResponseMessage SearchList3D()
        {
            var result = _bussiness.SearchDesign3D();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchMaterial()
        {
            var result = _bussiness.SearchListMaterial();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListModuleDesignDocument")]
        [HttpPost]
        public HttpResponseMessage SearchListModuleDesignDocument()
        {
            var result = _bussiness.SearchListModuleDesignDocument();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListRawMaterial")]
        [HttpPost]
        public HttpResponseMessage SearchListRawMaterial()
        {
            var result = _bussiness.SearchListRawMaterial();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListModuleErrorNotDone")]
        [HttpPost]
        public HttpResponseMessage SearchListModuleErrorNotDone()
        {
            var result = _bussiness.SearchErrorModuleNotDone();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListConvertUnit")]
        [HttpPost]
        public HttpResponseMessage SearchListConvertUnit()
        {
            var result = _bussiness.SearchConvertUnit();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListDesignStructure")]
        [HttpPost]
        public HttpResponseMessage SearchListDesignStructure()
        {
            var result = _bussiness.SearchDesignStructure();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListDesignStructureFile")]
        [HttpPost]
        public HttpResponseMessage SearchListDesignStructureFile()
        {
            var result = _bussiness.SearchDesignStructureFile();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchModuleByCode")]
        [HttpPost]
        public HttpResponseMessage SearchModuleByCode(string moduleCode)
        {
            var result = _bussiness.SearchModule(moduleCode);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchModule")]
        [HttpPost]
        public HttpResponseMessage SearchModule()
        {
            var result = _bussiness.SearchListModule();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchManufacture")]
        [HttpPost]
        public HttpResponseMessage SearchManufacture()
        {
            var result = _bussiness.GetListManufacture();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchUnit")]
        [HttpPost]
        public HttpResponseMessage SearchUnit()
        {
            var result = _bussiness.SearchListUnit();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListDesignStrcture")]
        [HttpPost]
        public HttpResponseMessage GetListDesignStrcture()
        {
            var result = _bussiness.GetListDesignStrcture();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
