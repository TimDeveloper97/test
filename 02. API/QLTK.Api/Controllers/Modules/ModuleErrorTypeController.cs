using QLTK.Business.ModuleError;
using NTS.Model.Combobox;
using NTS.Model.ModuleError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using QLTK.Business.ModuleErrorType;

namespace NTS.Api.Controllers.ModuleErrorType
{
    [RoutePrefix("api/ModuleErrorType")]
    public class ModuleErrorTypeController : ApiController
    {
        private readonly ModuleErrorTypeBusiness _business = new ModuleErrorTypeBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchModuleErrorType")]
        [HttpPost]
        public HttpResponseMessage SearchModuleErrorType(string type)
        {
            try
            {
                var result = _business.SearchModuleErrorType(type);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CreateModuleErrorType")]
        [HttpPost]
        public HttpResponseMessage CreateModuleErrorType(ModuleErrorTypeModel model)
        {
            try
            {
             
                _business.CreateModuleErrorType(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("UpdateModuleErrorType")]
        [HttpPost]
        public HttpResponseMessage UpdateModuleErrorType(ModuleErrorTypeModel model)
        {
            try
            {
                _business.UpdateModuleErrorType(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("GetModuleErrorTypeInfo")]
        [HttpPost]
        public HttpResponseMessage GetModuleErrorTypeInfo(ModuleErrorTypeModel model)
        {
            try
            {
                var result = _business.GetModuleErrorTypeInfo(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("DeleteModuleErrorType")]
        [HttpPost]
        public HttpResponseMessage DeleteModuleErrorType(ModuleErrorTypeModel model)
        {
            try
            {
                _business.DeleteModuleErrorType(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}