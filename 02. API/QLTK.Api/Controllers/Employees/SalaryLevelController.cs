using NTS.Model.SalaryLevel;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SalaryLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.SalaryLevel
{
    [RoutePrefix("api/salarylevel")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121504")]
    public class SalaryLevelController : BaseController
    {
        private readonly SalaryLevelBussiness _business = new SalaryLevelBussiness();

        /// <summary>
        /// Tìm kiếm mức lương
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121500")]
        public HttpResponseMessage SearchSalaryLevel(SalaryLevelSearchModel searchModel)
        {
            var result = _business.SearchSalaryLevel(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa mức lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121503")]
        public HttpResponseMessage DeleteJobPositions(SalaryLevelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteSalaryLevel(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm mức lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121501")]
        public HttpResponseMessage AddJobPositions(SalaryLevelModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateSalaryLevel(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin mức lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getSalaryLevelInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121502")]
        public HttpResponseMessage GetSalaryLevel(SalaryLevelModel modelSearch)
        {

            var result = _business.GetInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật mức lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121502")]
        public HttpResponseMessage UpdateJobPositions(SalaryLevelModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.Update(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]

        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _business.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ManufactureImportFile")]
        [HttpPost]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _business.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
