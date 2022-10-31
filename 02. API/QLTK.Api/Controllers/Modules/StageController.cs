using NTS.Common;
using NTS.Model.Stage;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Stage
{
    [RoutePrefix("api/Stage")]
    [ApiHandleExceptionSystem]
  //  [NTSIPAuthorize(AllowFeature = "F110308")]
    public class StageController : BaseController
    {
        private readonly StageBussiness _business = new StageBussiness();

        [Route("SearchStage")]
        [HttpPost]
      //  [NTSAuthorize(AllowFeature = "F110300")]
        public HttpResponseMessage SearchStage(StageSearchModel modelSearch)
        {
            var result = _business.SearchStage(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchListStage")]
        [HttpGet]
        public HttpResponseMessage SearchListStage(string projectProductId)
        {
            var result = _business.SearchListStage(projectProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddStage")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110301")]
        public HttpResponseMessage AddStage(StageModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddStage(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateStage")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110302; F110305;")]
        public HttpResponseMessage UpdateStage(StageModel model)
        {
            bool isCheckPermission = false;
            if (this.CheckPermission(Constants.Permission_Code_F110305))
            {
                isCheckPermission = true;
            }
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateStage(model, isCheckPermission, GetDepartmentIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteStage")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110303;F110306")]
        public HttpResponseMessage DeleteStage(StageModel model)
        {
            bool isCheckPermission = false;
            if (this.CheckPermission(Constants.Permission_Code_F110306))
            {
                isCheckPermission = true;
            }

            _business.DeleteStage(model, isCheckPermission, GetDepartmentIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetStageInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110304;F110307")]
        public HttpResponseMessage GetStageInfo(StageModel model)
        {

            bool isViewOtherDepartment = false;
            if (this.CheckPermission(Constants.Permission_Code_F110307))
            {
                isViewOtherDepartment = true;
            }

            var result = _business.GetStageInfo(model, GetDepartmentIdByRequest(), isViewOtherDepartment);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("createIndex")]
        [HttpPost]
        public HttpResponseMessage createIndex(StageSearchModel model)
        {
            _business.createIndex(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
