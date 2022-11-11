using NTS.Model.EmployeeDegree;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.EmployeeDegrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.EmployeeDegrees
{
    [RoutePrefix("api/EmployeeDegrees")]
    [ApiHandleExceptionSystemAttribute]
    public class EmployeeDegreesController : BaseController
    {
        private readonly EmployeeDegreeBusiness _business = new EmployeeDegreeBusiness();
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchModel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080016;F080002;F080004")]
        public HttpResponseMessage SearchModel(EmployeeDegreeSearcModel modelSearch)
        {
                var result = _business.SearchModel(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
           
        }



        [Route("SearchModels")]
        [HttpPost]
        public HttpResponseMessage SearchModels(EmployeeDegreeSearcModel modelSearch)
        {
            var result = _business.SearchModel(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        /// 
        [Route("Deletes")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080011")]
        public HttpResponseMessage Delete(EmployeeDegreeModel model)
        {
                _business.Deletes(model);
                return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        /// 
        [Route("Adds")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080009")]
        public HttpResponseMessage Adds(EmployeeDegreeModel model)
        {
            _business.Adds(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        /// 
        [Route("GetInfos")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080010;F080016")]
        public HttpResponseMessage GetInfos(EmployeeDegreeModel model)
        {
            var result = _business.GetInfos(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        /// 
        [Route("Updates")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080010")]
        public HttpResponseMessage Updates(EmployeeDegreeModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.Updates(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

    }
}
