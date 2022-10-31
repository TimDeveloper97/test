using NTS.Model.QLTKMG;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.QLTKMG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.QLTKMG
{
    [RoutePrefix("api/QLTKMG")]
    [ApiHandleExceptionSystemAttribute]
    [Authorize]
    public class QLTKMGController : BaseController
    {
        private readonly QLTKMGBusiness _qLTKMG1000Business = new QLTKMGBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000100")]
        public HttpResponseMessage SearchMaterialGroup(QLTKMGSearchModel modelSearch)
        {
            var result = _qLTKMG1000Business.GetListMaterialGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000103")]
        public HttpResponseMessage DeleteMaterialGroup(QLTKMGModel model)
        {
            _qLTKMG1000Business.DeleteMaterialGroup(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000101")]
        public HttpResponseMessage AddMaterialGroup(QLTKMGModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _qLTKMG1000Business.AddMaterialGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateMaterialGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000102;F000104")]
        public HttpResponseMessage UpdateMaterialGroup(QLTKMGModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _qLTKMG1000Business.UpdateMaterialGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetMaterialGroupInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000104")]
        public HttpResponseMessage GetMaterialGroupInfo(QLTKMGModel model)
        {
            var result = _qLTKMG1000Business.GetMaterialGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("GetCbbMaterialGroupFullChild")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000100")]
        public HttpResponseMessage GetCbbDocumentTemplateTypeFullChild(string Id)
        {
            object result = _qLTKMG1000Business.GetCbbMaterialGroupFullChild(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}