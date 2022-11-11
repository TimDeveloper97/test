using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Model.Expert;
using NTS.Model.Function;
using NTS.Model.Specialize;
using NTS.Model.WorkPlace;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;

namespace QLTK.Api.Controllers.Expert
{
    [RoutePrefix("api/Expert")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F040306")]
    public class ExpertController : BaseController
    {
        private readonly Business.Expert.Expert _expert = new Business.Expert.Expert();
        private readonly Business.Expert.Expert _workPlace = new Business.Expert.Expert();
        private readonly Business.Expert.Expert _specialize = new Business.Expert.Expert();

        [Route("SearchExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040300")]
        public HttpResponseMessage SearchFunction(ExpertSearchModel modelSearch)
        {
            var result = _expert.SearchExpert(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchWorkPlace")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040301;F040302")]
        public HttpResponseMessage SearchWorkPlace(WorkPlaceSearchModel modelSearch)
        {
            var result = _workPlace.SearchworkPlace(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSpecialize")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040301;F040302")]
        public HttpResponseMessage SearchSpecialize(SpecializeSearchModel modelSearch)
        {
            var result = _specialize.SearchSqecialize(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040301")]
        public HttpResponseMessage AddExpert(ExpertModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _expert.AddExpert(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040302;F040304")]
        public HttpResponseMessage GetExpert(ExpertModel model)
        {
            var result = _expert.GetIdExpert(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040302")]
        public HttpResponseMessage UpdateExPert(ExpertModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _expert.UpdateExpert(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteExpert")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F040303")]
        public HttpResponseMessage DeleteExpert(ExpertModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _expert.DeleteExpert(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [NTSAuthorize(AllowFeature = "F040305")]
        public HttpResponseMessage ExportExcel(ExpertSearchModel model)
        {
            try
            {
                string path = _expert.ExportExcelExpert(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CheckDelete/{expertId}")]
        [HttpDelete]
        [NTSAuthorize(AllowFeature = "F040301;F040302")]
        public HttpResponseMessage CheckDeleteVillage(string expertId)
        {
            _expert.CheckDeleteBank(expertId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
    }
}
