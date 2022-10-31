using NTS.Model.MaterialParameter;
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
    [RoutePrefix("api/MaterialParameter")]
    public class MaterialParameterController : BaseController
    {
        private readonly MaterialParameterBusiness _business = new MaterialParameterBusiness();

        [Route("GetParameterByGroupId")]
        [HttpPost]
        public HttpResponseMessage GetParameterByGroupId(string Id)
        {
            try
            {
                var result = _business.GetParameterByGroupId(Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddMaterialParameter")]
        [HttpPost]
        public HttpResponseMessage AddMaterialParameter(MaterialParameterModel model)
        {
            try
            {
                model.CreateBy = GetUserIdByRequest();
                var result = _business.AddMaterialParameter(model);
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
