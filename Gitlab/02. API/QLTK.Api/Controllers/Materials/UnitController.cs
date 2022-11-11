using NTS.Model.Unit;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Unit
{
    [RoutePrefix("api/Unit")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F000605")]
    public class UnitController : BaseController
    {
        private readonly UnitBusiness _unitBusiness = new UnitBusiness();

        [Route("SearchUnit")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000600")] 

        public HttpResponseMessage SearchMaterial(UnitSearchModel modelSearch)
        {
            var result = _unitBusiness.SearchUnit(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteUnit")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000603")]
        public HttpResponseMessage DeleteManufacture(UnitModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _unitBusiness.DeleteUnit(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddUnit")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000601")]
        public HttpResponseMessage AddManufacture(UnitModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _unitBusiness.AddUnit(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetUnitInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000604;F000602")]
        public HttpResponseMessage GetUnitInfo(UnitModel model)
        {
            var result = _unitBusiness.GetUnitInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateUnit")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000602")]
        public HttpResponseMessage UpdateManufacture(UnitModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _unitBusiness.UpdateUnit(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListUnit")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000604")]
        public HttpResponseMessage GetListUnit()
        {
            var rs = _unitBusiness.GetListUnit();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
