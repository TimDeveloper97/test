using Newtonsoft.Json;
using NTS.Model.ModuleGroupProductStandard;
using NTS.Model.ProductStandards;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.Stage;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.QLTKGROUPMODUL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.GroupModule
{
    [RoutePrefix("api/GroupModule")]
    [ApiHandleExceptionSystemAttribute]
    [Authorize]
    public class GroupModuleController : BaseController
    {
        private readonly GroupModuleBussiness _business = new GroupModuleBussiness();
        [Route("SearchGroupModules")]
        [HttpPost]
        public HttpResponseMessage SearchGroupModule(GroupModuleSearchModel modelSearch)
        {
            var result = _business.GetListModuleGroup(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchGroupModulesById")]
        [HttpPost]
        public HttpResponseMessage SearchGroupModuleById(GroupModuleSearchModel modelSearch)
        {
            var result = _business.GetModuleGroupById(modelSearch.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteGroupModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020003")]
        public HttpResponseMessage DeleteGroupModul(GroupModuleModel model)
        {
            _business.DeleteGroupModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020003")]
        public HttpResponseMessage DeleteProductStandards(ModuleGroupProductStandardModel model)
        {
            _business.DeleteProductStandards(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddGroupModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020001")]
        public HttpResponseMessage AddModuleGroup(GroupModuleModel model)
        {
            _business.AddGroupModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        [Route("GetGroupModuleInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020002")]
        public HttpResponseMessage GetGroupModuleInfo(GroupModuleModel model)
        {
            model.DepartementId = GetDepartmentIdByRequest();
            var result = _business.GetGroupModuleInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateGroupModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020002")]
        public HttpResponseMessage UpdateGroupModule(GroupModuleModel model)
        {
            model.DepartementId = GetDepartmentIdByRequest();
            _business.UpdateGroupModule(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchProductStandard")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020002")]
        public HttpResponseMessage SearchProductStandard(ProductStandardsSearchModel modelSearch)
        {
            var result = _business.ProductStandards(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Danh sách công đoạn
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchStage")]
        [HttpPost]
        public HttpResponseMessage SearchStage(StageSearchModel modelSearch)
        {
            modelSearch.DepartmentId = GetDepartmentIdByRequest();
            var result = _business.Stage(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("SearchGroupModulesExpect")]
        [HttpPost]
        public HttpResponseMessage SearchGroupModuleExcepted(GroupModuleSearchModel modelSearch)
        {
            var result = _business.GetModuleGroupExcepted(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
