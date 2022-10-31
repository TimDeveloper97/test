using Newtonsoft.Json;
using NTS.Model.PracticeGroup;
using QLTK.Api.Attributes;
using QLTK.Business.PracticeGroups;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.PracticeGroup
{
    [RoutePrefix("api/PracticeGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040604")]
    public class PracticeGroupController : ApiController
    {
        private readonly PracticeGroupBusiness _business = new PracticeGroupBusiness();
        [Route("SearchPracticeGroup")]
        [HttpPost]
        public HttpResponseMessage SearchPracticeGroup(PracticeGroupSearchModel modelSearch)
        {
            var result = _business.SearchPracticeGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeletePracticeGroup")]
        [HttpPost]
        public HttpResponseMessage DeletePracticeGroup(PracticeGroupModel model)
        {
            _business.DeletePracticeGroup(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddPracticeGroup")]
        [HttpPost]
        public HttpResponseMessage AddProductGroup(PracticeGroupModel model)
        {

            _business.AddPracticeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        //[Route("AddPracticeGroup")]
        //[HttpPost]
        //public HttpResponseMessage AddPracticeGroup()
        //{
        //    try
        //    {
        //        var modelJson = HttpContext.Current.Request.Form["Model"];
        //        var PracticeGroupModel = JsonConvert.DeserializeObject<PracticeGroupModel>(modelJson);
        //        var hfc = HttpContext.Current.Request.Files;
        //        _business.AddPracticeGroup(PracticeGroupModel, hfc);
        //        return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        [Route("GetPracticeGroupInfo")]
        [HttpPost]
        public HttpResponseMessage GetPracticeGroupInfo(PracticeGroupModel model)
        {
            var result = _business.GetPracticeGroupInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdatePracticeGroup")]
        [HttpPost]
        public HttpResponseMessage UpdatePracticeGroup(PracticeGroupModel model)
        {
            //var modelJson = HttpContext.Current.Request.Form["Model"];
            //var PracticeGroupModel = JsonConvert.DeserializeObject<PracticeGroupModel>(modelJson);
            //var hfc = HttpContext.Current.Request.Files;
            _business.UpdatePracticeGroup(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
