using NTS.Model.ConverUnit;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ConverUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ConverUnit
{
    [RoutePrefix("api/ConverUnit")]
    [ApiHandleExceptionSystemAttribute]

    public class ConverUnitsController : BaseController
    {
        private readonly ConverUnitBusiness _business = new ConverUnitBusiness();

        [Route("GetListConverUnit")]
        [HttpPost]
        public HttpResponseMessage GetListConverUnit(ConverUnitModel modelSearch)
        {
            var result = _business.GetListConverUnit(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddConverUnit")]
        [HttpPost]
        public HttpResponseMessage AddConverUnit(ConverUnitModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddConverUnit(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetUnitName")]
        [HttpPost]
        public HttpResponseMessage GetUnitName(ConverUnitModel model)
        {
            var result = _business.GetUnitName(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
