using NTS.Model.SupplierGroup;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SupplierGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers
{
    [RoutePrefix("api/SupplierGroup")]
    [ApiHandleExceptionSystemAttribute]

    public class SupplierGroupsController : BaseController
    {
        private readonly SupplierGroupBusiness _business = new SupplierGroupBusiness();

        [Route("SearchSupplierGroup")]
        [HttpPost]
        public HttpResponseMessage SearchSupplierGroup(SupplierGroupModel modelSearch)
        {
            var result = _business.SearchSupplierGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSupplierGroup")]
        [HttpPost]
        public HttpResponseMessage AddSupplierGroup(SupplierGroupModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSupplierGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSupplierGroup")]
        [HttpPost]
        public HttpResponseMessage UpdateSupplierGroup(SupplierGroupModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateSupplierGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeletesupplierGroup")]
        [HttpPost]
        public HttpResponseMessage DeletesupplierGroup(SupplierGroupModel model)
        {
            _business.DeletesupplierGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSupplierGroupInfo")]
        [HttpPost]
        public HttpResponseMessage GetSupplierGroupInfo(SupplierGroupModel model)
        {
            var result = _business.GetSupplierGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
