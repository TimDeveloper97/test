using NTS.Model.MaterialParameterValue;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Materials
{
    [RoutePrefix("api/MaterialParameterValue")]
    public class MaterialParameterValueController : BaseController
    {
        private readonly MaterialParameterValueBusiness _business = new MaterialParameterValueBusiness();

        [Route("GetValueByParameterId")]
        [HttpPost]
        public HttpResponseMessage GetValueByParameterId(string Id)
        {
            try
            {
                var result = _business.GetValueByParameterId(Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddMaterialParameterValue")]
        [HttpPost]
        public HttpResponseMessage AddMaterialParameterValue(MaterialParameterValueModel model)
        {
            try
            {
                model.CreateBy = GetUserIdByRequest();
                var result = _business.AddMaterialParameterValue(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CheckRelationship")]
        [HttpPost]
        public HttpResponseMessage CheckRelationship(string Id)
        {
            try
            {
                var result = _business.CheckRelationship(Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
