using NTS.Model.QLTKMP;
using QLTK.Business.QLTKMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.QLTKMP
{
    [RoutePrefix("api/QLTKMP")]
    public class QLTKMPController : ApiController
    {
        private readonly QLTKMPBusiness _qLTKMPBusiness = new QLTKMPBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchMaterialParameter")]
        [HttpPost]
        public HttpResponseMessage SearchMaterialParameter(QLTKMPSearchModel modelSearch)
        {
            try
            {
                var result = _qLTKMPBusiness.SearchMaterialParameter(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteMaterialParameter")]
        [HttpPost]
        public HttpResponseMessage DeleteMaterialParameter(QLTKMPModel model)
        {
            try
            {
                _qLTKMPBusiness.DeleteMaterialGroup(model);
                return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddMaterialParameter")]
        [HttpPost]
        public HttpResponseMessage AddMaterialParameter(QLTKMPModel model)
        {
            try
            {
                _qLTKMPBusiness.AddMaterialParameter(model);
                return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateMaterialParameter")]
        [HttpPost]
        public HttpResponseMessage UpdateMaterialParameter(QLTKMPModel model)
        {
            try
            {
                _qLTKMPBusiness.UpdateMaterialParameter(model);
                return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}