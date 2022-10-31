using NTS.Model.TestCriteriaGroup;
using QLTK.Api.Attributes;
using QLTK.Business.TestCriteriaGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.TestCriteriaGroup
{
    [RoutePrefix("api/TestCriteriaGroup")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020204")]
    public class TestCriteriaGroupController : ApiController
    {
        private readonly TestCriteriaGroupBusiness testCriteriaGroupBusiness = new TestCriteriaGroupBusiness();
        // tìm kiếm
        [Route("SearchRawTestCriteriaGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020300")]
        public HttpResponseMessage SearchTestCriterialGroup(TestCriteriaGroupSearchModel modelSearch)
        {
                var result = testCriteriaGroupBusiness.SearchTestCriterialGroup(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
           
        }
        // xóa
        [Route("DeleteTestCriteralGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020203")]
        public HttpResponseMessage DeleteTestCriteralGroup(TestCriteriaGroupModel model)
        {

                testCriteriaGroupBusiness.DeleteTestCriteralGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
           
        }
        // get
        [Route("GetTestCriteralGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020202")]
        public HttpResponseMessage GetTestCriteralGroup(TestCriteriaGroupModel model)
        {

                var result = testCriteriaGroupBusiness.GetTestCriteralGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            
        }
        // thêm mới
        [Route("AddTestCriteralGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020201")]
        public HttpResponseMessage AddTestCriteralGroup(TestCriteriaGroupModel model)
        {
           
                testCriteriaGroupBusiness.AddTestCriteralGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
           
        }
        // cập nhật
        [Route("UpdateTestCriteralGroup")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020202")]
        public HttpResponseMessage UpdateTestCriteralGroup(TestCriteriaGroupModel model)
        {
           
                testCriteriaGroupBusiness.UpdateTestCriteralGroup(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            
        }

    }
}
