using Newtonsoft.Json;
using NTS.Model.TestCriteria;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.TestCriterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.TestCriteria
{
    [RoutePrefix("api/TestCriteria")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020306")]
    public class TestCriteriaController : BaseController
    {
        private readonly TestCriteriaBusiness _business = new TestCriteriaBusiness();
        // tìm kiếm
        [Route("SearchTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020300")]
        public HttpResponseMessage SearchTestCriteria(TestCriterSearchModel modelSearch)
        {

            var result = _business.SearchModel(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        //Xóa
        [Route("DeleteTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020303")]
        public HttpResponseMessage DeleteTestCriteria(TestCriteriaModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteTestCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
        // thêm
        [Route("AddTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020301")]
        public HttpResponseMessage AddTestCriteria(TestCriteriaModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        // cập nhật
        [Route("UpdateTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020302")]
        public HttpResponseMessage UpdateTestCriteria(TestCriteriaModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateTestCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
        // get
        [Route("GetTestCriteriaInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020302;F020304")]
        public HttpResponseMessage GetTestCriteriaInfo(TestCriteriaModel model)
        {

            var result = _business.GetTestCriteriaInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        // 04-02-2020 * thêm mới xuất excel
        [Route("ExcelTestCriteria")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020305")]
        public HttpResponseMessage Excel(TestCriteriaModel model)
        {
            var result = _business.ExcelTestCriteria(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
