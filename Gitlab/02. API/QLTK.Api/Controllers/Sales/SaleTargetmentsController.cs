using NTS.Model.Sale.SaleProduct;
using NTS.Model.Sale.SaleTartgetment;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SaleProducts;
using QLTK.Business.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace QLTK.Api.Controllers.Sales
{
    [RoutePrefix("api/SaleTartgetments")]
    [ApiHandleExceptionSystemAttribute]
    public class SaleTargetmentsController : BaseController
    {
        private SaleTartgetmentBussiness _saleTartgetmentBussiness = new SaleTartgetmentBussiness();

        [Route("GetSaleTartgetmentInfo")]
        [HttpPost]
        public HttpResponseMessage GetSaleTartgetmentInfo(SaleTartgetmentModel model)
        {
            var data = _saleTartgetmentBussiness.GetSaleTartgetmentInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("CreateUpdateSaleTartgetment")]
        [HttpPost]
        public HttpResponseMessage CreateUpdateSaleTartgetment(SaleTartgetmentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _saleTartgetmentBussiness.CreateUpdateSaleTartgetment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetAllSaleTartgetments")]
        [HttpPost]
        public HttpResponseMessage GetAllSaleTartgetments(SaleTartgetmentModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var data =_saleTartgetmentBussiness.GetAllSaleTartgetments(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("DeleteSaleTartgetment")]
        [HttpPost]
        public HttpResponseMessage DeleteSaleTartgetment(SaleTartgetmentModel model)
        {
            _saleTartgetmentBussiness.DeleteSaleTartgetment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}