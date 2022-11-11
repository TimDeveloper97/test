using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Meeting;
using System.Net;
using NTS.Model.SaleGroups;
using QLTK.Business.SaleGroups;
using QLTK.Business.ProjectProducts;
using NTS.Model.Customers;
using NTS.Model.Project;
using NTS.Model.Plans;
using NTS.Model.Repositories;

namespace QLTK.Api.Controllers.Mobile
{
    [RoutePrefix("api/mobile/qc")]
    [ApiHandleExceptionSystem]
    public class QcProductMobileController : BaseController
    {
        private readonly ProjectProductQcBusiness _qcbusiness = new ProjectProductQcBusiness();

        [Route("product")]
        [HttpGet]
        public HttpResponseMessage GetProductItemInfoBySerialNumber(string serialNumber)
        {
            var result = _qcbusiness.GetProductItemInfoBySerialNumber(serialNumber);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ProductStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateProductStatus(QCResultCreatModel model)
        {
            string UserId = GetUserIdByRequest();
            _qcbusiness.UpdateProductStatus(model, UserId);
            return Request.CreateResponse(HttpStatusCode.OK, String.Empty);
        }

        [Route("checklist")]
        [HttpPost]
        public HttpResponseMessage GetListStandardById(QCCheckListModel model)
        {
            var result = _qcbusiness.GetListStandardById(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("result/{id}")]
        [HttpGet]
        public HttpResponseMessage GetQCCheckListById(string id)
        {
            var result = _qcbusiness.GetQCCheckListById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("qcresult")]
        [HttpPost]
        public HttpResponseMessage CreatQCResult(QCResultCreatModel model)
        {
            model.UserId = GetUserIdByRequest();
            _qcbusiness.CreatQCResult(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}